using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using WpfApp1;

namespace EspLib
{
    /// <summary>
    /// Public facade for controlling the ESP overlay from the main WPF application.
    /// Call <see cref="EnableEsp"/> when the toggle is turned ON
    /// and  <see cref="DisableEsp"/> when turned OFF.
    /// </summary>
    public static class EspManager
    {
        private delegate bool EnumChildProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr FindWindowEx(IntPtr parent, IntPtr childAfter, string cls, string title);
        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern bool EnumChildWindows(IntPtr hWndParent, EnumChildProc lpEnumFunc, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT { public int Left, Top, Right, Bottom; }

        private static EspOverlayWindow _overlay;
        private static Thread           _dataThread;
        private static bool             _running;

        // Status callback for UI feedback
        public static event Action<string> OnStatus;

        private static readonly string[] s_emulators = new[]
        {
            "HD-Player", "Bluestacks", "Bluestacks-5", "BstkSVC", "HD-Agent", "HdPlayerService",
            "LdBoxHeadless", "dnplayer", "NoxVMHandle", "Nox", "MEmu", "MEmuHeadless",
            "AndroidEmulatorEn", "ProjectTitan", "SmartGaGa"
        };

        /// <summary>
        /// Starts the ESP: connects to BlueStacks memory, launches the overlay window and data thread.
        /// Must be called from the UI thread.
        /// </summary>
        public static async Task<string> EnableEsp()
        {
            if (_running) return "ESP ON";

            // 1. Find emulator viewport window first (fast check)
            IntPtr gameWnd = FindGameWindow();
            if (gameWnd == IntPtr.Zero) return "BlueStacks game viewport not found";

            EspCore.Handle = gameWnd;

            if (GetClientRect(gameWnd, out var rect))
            {
                int w = rect.Right  - rect.Left;
                int h = rect.Bottom - rect.Top;
                if (w > 100 && h > 100)
                {
                    EspCore.Width  = w;
                    EspCore.Height = h;
                }
            }

            // 2. Find il2cpp base — BlueStacks.Setup() handles full memory init with 20 retries
            ulong il2cpp = 0;
            string setupError = "";
            await Task.Run(() =>
            {
                try
                {
                    var bs = new BlueStacks();
                    bool found = bs.Setup("com.dts.freefireth") || bs.Setup("com.dts.freefiremax");
                    if (found)
                    {
                        string[] candidateModules = new[] { "libil2cpp.so", "libil2cpp", "il2cpp", "libunity.so", "libunity", "libmain.so" };
                        for (int i = 0; i < 20; i++)
                        {
                            foreach (var mod in candidateModules)
                            {
                                il2cpp = bs.GetModuleBase(mod);
                                if (il2cpp != 0) break;
                            }
                            if (il2cpp != 0) break;
                            Thread.Sleep(200);
                        }
                        if (il2cpp != 0)
                        {
                            EspOffsets.Il2CppBase = il2cpp;
                            ulong cr3 = bs.GetCr3();
                            if (cr3 != 0) EspMemory.SetCr3(cr3);
                        }
                    }
                    else
                    {
                        setupError = EspMemory.LastInitError;
                    }

                    // Dual Engine Fallback: If hypervisor guest scan was unable to locate IL2CPP base, attach process memory fallback
                    if (il2cpp == 0)
                    {
                        if (EspMemory.SetupProcessFallback())
                        {
                            // If hypervisor translation is unavailable, process handle fallback is now active for EspMemory
                            setupError = null;
                        }
                    }
                }
                catch { }
            });

            if (!EspMemory.IsValid)
            {
                string err = !string.IsNullOrEmpty(setupError) ? setupError : "Target emulator process not found — is Free Fire running?";
                DiagnosticLogger.LogResult("ESP Master", "libil2cpp.so", false, 0, null, err);
                return err;
            }

            // 4. Launch data thread
            _running    = true;
            EspConfig.EspMaster = true;
            _dataThread = new Thread(EspData.Work) { IsBackground = true };
            _dataThread.Start();

            // 5. Show overlay (must be on UI thread)
            _overlay = new EspOverlayWindow();
            _overlay.Show();

            DiagnosticLogger.LogResult("ESP Master", "libil2cpp.so", true, 1, $"Base=0x{il2cpp:X}", $"ESP Attached on Window: {gameWnd}");
            return "ESP ON";
        }

        /// <summary>
        /// Stops the ESP: hides overlay and shuts down memory engine.
        /// Must be called from the UI thread.
        /// </summary>
        public static void DisableEsp()
        {
            _running            = false;
            EspConfig.EspMaster = false;
            EspCore.HaveMatrix  = false;
            EspOffsets.Il2CppBase = 0;

            try
            {
                _overlay?.StopRendering();
                _overlay?.Close();
            }
            catch { }
            _overlay = null;

            try { EspMemory.Destroy(); } catch { }
        }

        // ── Window-finder helpers ────────────────────────────────────────────
        private static IntPtr FindGameWindow()
        {
            foreach (var name in s_emulators)
            {
                try
                {
                    var procs = Process.GetProcessesByName(name);
                    foreach (var p in procs)
                    {
                        if (p.MainWindowHandle != IntPtr.Zero)
                        {
                            IntPtr vp = FindViewport(p.MainWindowHandle);
                            if (vp != IntPtr.Zero) return vp;
                        }
                    }
                }
                catch { }
            }

            // Direct search top-level known emulator window classes if process MainWindowHandle was 0
            foreach (string cls in new[] { "BlueStacksApp", "MSIAppPlayer", "BstkAppPlayer", "BlueStacks", "RenderWindow" })
            {
                IntPtr top = FindWindowEx(IntPtr.Zero, IntPtr.Zero, cls, null);
                if (top != IntPtr.Zero && IsWindowVisible(top))
                    return top;
            }

            return IntPtr.Zero;
        }

        private static IntPtr FindViewport(IntPtr emulatorWnd)
        {
            if (emulatorWnd == IntPtr.Zero) return IntPtr.Zero;

            // 1. Try known child class names
            string[] knownClasses = new string[] { "BlueStacksApp", "MSIAppPlayer", "BstkAppPlayer", "BlueStacks", "RenderWindow", "Qt5QWindowIcon", "subWin" };
            foreach (string cls in knownClasses)
            {
                IntPtr child = FindWindowEx(emulatorWnd, IntPtr.Zero, cls, null);
                if (child != IntPtr.Zero && IsWindowVisible(child))
                {
                    if (GetClientRect(child, out var r) && (r.Right - r.Left) > 100 && (r.Bottom - r.Top) > 100)
                        return child;
                }
            }

            // 2. Enum all child windows to find the largest visible child area
            IntPtr bestChild = IntPtr.Zero;
            int bestArea = 0;
            EnumChildWindows(emulatorWnd, (IntPtr hwnd, IntPtr lParam) =>
            {
                if (!IsWindowVisible(hwnd)) return true;
                if (GetClientRect(hwnd, out var lpRect))
                {
                    int w = lpRect.Right - lpRect.Left;
                    int h = lpRect.Bottom - lpRect.Top;
                    int area = w * h;
                    if (w > 100 && h > 100 && area > bestArea)
                    {
                        bestChild = hwnd;
                        bestArea = area;
                    }
                }
                return true;
            }, IntPtr.Zero);

            if (bestChild != IntPtr.Zero)
                return bestChild;

            // 3. Fallback to emulatorWnd itself if it has valid dimensions
            if (IsWindowVisible(emulatorWnd) && GetClientRect(emulatorWnd, out var mainRect))
            {
                int mw = mainRect.Right - mainRect.Left;
                int mh = mainRect.Bottom - mainRect.Top;
                if (mw > 100 && mh > 100)
                    return emulatorWnd;
            }

            return IntPtr.Zero;
        }
    }
}
