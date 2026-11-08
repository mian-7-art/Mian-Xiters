using System;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Aimbot.Mem;
using WpfApp1;

namespace EspLib
{
    /// <summary>
    /// Public facade for controlling the ESP overlay from the main application.
    /// Manages memory discovery, entity background processing, and transparent WPF overlay window.
    /// Self-contained in MIAN XITERS.exe with zero companion executables or loose files.
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

        // Shared memory IPC support if external telemetry is enabled
        private static MemoryMappedFile _mmf;
        private static MemoryMappedViewAccessor _accessor;
        private static readonly object _syncLock = new object();

        public static ulong CurrentLocalPlayer
        {
            get
            {
                if (EspCore.LocalPlayer != 0)
                    return EspCore.LocalPlayer;

                if (_accessor != null)
                {
                    try { return ReadConfig().LocalPlayer; } catch { }
                }
                return 0uL;
            }
        }

        public static bool IsRunning => _running;

        public static event Action<string> OnStatus;

        /// <summary>
        /// Instantly flushes entity dictionary, TLB, and memory caches (F5 key shortcut).
        /// </summary>
        public static void RefreshEsp()
        {
            EspMemory.ClearCaches();
            EspCore.Entities.Clear();
            EspCore.HaveMatrix = false;
            UpdateOptions();
        }

        private static readonly string[] s_emulators = new[]
        {
            "HD-Player", "HD-Frontend", "Bluestacks", "Bluestacks-5", "BstkSVC", "HD-Agent", "HdPlayerService",
            "MSIAppPlayer", "AppPlayer", "BlueStacksApp",
            "LdBoxHeadless", "Ld9BoxHeadless", "dnplayer", "NoxVMHandle", "Nox", "MEmu", "MEmuHeadless",
            "AndroidEmulator", "AndroidEmulatorEn", "aow_exe", "ProjectTitan", "Titan", "SmartGaGa"
        };

        /// <summary>
        /// Starts the ESP: connects to emulator memory, launches the DPI-aware overlay window and data thread.
        /// Must be called from the UI thread.
        /// </summary>
        public static async Task<string> EnableEsp()
        {
            if (_running)
            {
                UpdateOptions();
                return "ESP ON";
            }

            // 1. Find emulator viewport window first
            IntPtr gameWnd = FindGameWindow();
            if (gameWnd == IntPtr.Zero)
            {
                return "Emulator game viewport not found — please ensure emulator is running";
            }

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

            // 2. Find il2cpp base — BlueStacks.Setup() handles full hypervisor / process memory init
            ulong il2cpp = 0;
            string setupError = "";
            await Task.Run(() =>
            {
                try
                {
                    var bs = new BlueStacks();
                    string[] packagesToTry = new[] {
                        "com.dts.freefireth", "com.dts.freefiremax", "com.dts.freefirevn",
                        "com.dts.freefiretw", "com.dts.freefirein", "com.dts.freefire", null
                    };

                    for (int attempt = 0; attempt < 3; attempt++)
                    {
                        foreach (var pkgName in packagesToTry)
                        {
                            if (bs.Setup(pkgName))
                            {
                                string[] candidateModules = new[] { "libil2cpp.so", "libil2cpp", "il2cpp", "libunity.so", "libunity", "libmain.so" };
                                foreach (var mod in candidateModules)
                                {
                                    il2cpp = bs.GetModuleBase(mod);
                                    if (il2cpp != 0) break;
                                }
                                if (il2cpp != 0)
                                {
                                    EspOffsets.Il2CppBase = il2cpp;
                                    ulong cr3 = bs.GetCr3();
                                    if (cr3 != 0) EspMemory.SetCr3(cr3);
                                    break;
                                }
                            }
                        }
                        if (il2cpp != 0) break;
                        setupError = EspMemory.LastInitError;
                        Thread.Sleep(150);
                    }

                    // Dual Engine Fallback: If guest VMA scan failed, attach process memory fallback
                    if (il2cpp == 0)
                    {
                        if (EspMemory.SetupProcessFallback())
                        {
                            setupError = null;
                            var (pName, pid) = GetProcess.GetRunningEmulatorProcess();
                            if (pid > 0)
                            {
                                try
                                {
                                    var proc = Process.GetProcessById(pid);
                                    foreach (ProcessModule mod in proc.Modules)
                                    {
                                        string mName = mod.ModuleName;
                                        if (!string.IsNullOrEmpty(mName) &&
                                            (mName.IndexOf("il2cpp", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                             mName.IndexOf("unity", StringComparison.OrdinalIgnoreCase) >= 0))
                                        {
                                            il2cpp = (ulong)mod.BaseAddress.ToInt64();
                                            EspOffsets.Il2CppBase = il2cpp;
                                            break;
                                        }
                                    }
                                }
                                catch { }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    setupError = ex.Message;
                }
            });

            if (!EspMemory.IsValid || il2cpp == 0 || EspOffsets.Il2CppBase == 0)
            {
                string err = !string.IsNullOrEmpty(setupError) ? setupError : "Target emulator process or libil2cpp.so base address not found — is Free Fire active in-game?";
                DiagnosticLogger.LogResult("ESP Master", "libil2cpp.so", false, 0, null, err);
                return err;
            }

            // 3. Initialize Shared Memory IPC for config sync
            try
            {
                int num = Marshal.SizeOf(typeof(MianSharedConfig));
                _mmf = MemoryMappedFile.CreateOrOpen("Local\\MianEspSharedConfig", num, MemoryMappedFileAccess.ReadWrite);
                _accessor = _mmf.CreateViewAccessor(0L, num, MemoryMappedFileAccess.ReadWrite);
                MianSharedConfig cfg = BuildCurrentConfig();
                cfg.Status = 1u;
                cfg.Terminate = 0u;
                cfg.Enable = 1u;
                cfg.ErrorMsg = "Active";
                WriteConfig(cfg);
            }
            catch { }

            // 4. Launch background memory scanner thread
            _running = true;
            EspConfig.EspMaster = true;
            _dataThread = new Thread(EspData.Work) { IsBackground = true };
            _dataThread.Start();

            // 5. Launch native transparent overlay window (must run on UI thread)
            try
            {
                if (_overlay != null)
                {
                    _overlay.StopRendering();
                    _overlay.Close();
                }
            }
            catch { }

            _overlay = new EspOverlayWindow();
            _overlay.Show();

            DiagnosticLogger.LogResult("ESP Master", "libil2cpp.so", true, 1, $"Base=0x{il2cpp:X}", $"ESP Overlay Attached on Viewport: {gameWnd}");
            return "ESP ON";
        }

        /// <summary>
        /// Stops the ESP: hides overlay window and releases memory engine resources.
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

            try
            {
                if (_accessor != null)
                {
                    lock (_syncLock)
                    {
                        MianSharedConfig cfg = ReadConfig();
                        cfg.Terminate = 1u;
                        cfg.Enable = 0u;
                        cfg.Status = 0u;
                        WriteConfig(cfg);
                    }
                }
            }
            catch { }

            try
            {
                _accessor?.Dispose();
                _mmf?.Dispose();
            }
            catch { }
            _accessor = null;
            _mmf = null;

            try { EspMemory.Destroy(); } catch { }
        }

        public static void UpdateOptions()
        {
            if (_accessor == null) return;
            try
            {
                lock (_syncLock)
                {
                    MianSharedConfig cfg = BuildCurrentConfig();
                    cfg.Enable = (_running ? 1u : 0u);
                    cfg.Status = 2u;
                    WriteConfig(cfg);
                }
            }
            catch { }
        }

        public static void ApplyStreamMode(bool streamMode)
        {
            EspConfig.StreamingMode = streamMode;
            try
            {
                EspOverlayWindow.ApplyStreamMode(streamMode);
            }
            catch { }

            if (_accessor == null) return;
            try
            {
                lock (_syncLock)
                {
                    MianSharedConfig cfg = ReadConfig();
                    cfg.StreamMode = (streamMode ? 1u : 0u);
                    WriteConfig(cfg);
                }
            }
            catch { }
        }

        private static MianSharedConfig BuildCurrentConfig()
        {
            return new MianSharedConfig
            {
                Magic       = 1296646478u,
                Enable      = (uint)(_running ? 1 : 0),
                Skeleton    = (EspConfig.EspSkeleton ? 1u : 0u),
                Lines       = (EspConfig.EspLine ? 1u : 0u),
                LinePos     = (uint)EspConfig.LinePosition,
                Box         = (EspConfig.EspCornerBox ? 1u : 0u),
                BoxType     = 1u,
                HealthBar   = (EspConfig.EspHealth ? 1u : 0u),
                Weapon      = (EspConfig.EspWeapon ? 1u : 0u),
                Name        = (EspConfig.EspName ? 1u : 0u),
                Distance    = (EspConfig.EspDistance ? 1u : 0u),
                Radar       = (EspConfig.Esp360 ? 1u : 0u),
                Team        = (EspConfig.EspTeam ? 1u : 0u),
                StreamMode  = (EspConfig.StreamingMode ? 1u : 0u),
                AutoRefresh = (EspConfig.AutoRefresh ? 1u : 0u),
                LocalPlayer = (uint)EspCore.LocalPlayer,
                ColorR      = (float)(int)EspConfig.EnemyColor.R / 255f,
                ColorG      = (float)(int)EspConfig.EnemyColor.G / 255f,
                ColorB      = (float)(int)EspConfig.EnemyColor.B / 255f,
                ColorA      = 1f
            };
        }

        private static void WriteConfig(MianSharedConfig cfg)
        {
            if (_accessor == null) return;
            byte[] array = new byte[Marshal.SizeOf(typeof(MianSharedConfig))];
            GCHandle gCHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
            try
            {
                Marshal.StructureToPtr(cfg, gCHandle.AddrOfPinnedObject(), fDeleteOld: false);
                _accessor.WriteArray(0L, array, 0, array.Length);
            }
            finally
            {
                gCHandle.Free();
            }
        }

        private static MianSharedConfig ReadConfig()
        {
            if (_accessor == null) return default;
            byte[] array = new byte[Marshal.SizeOf(typeof(MianSharedConfig))];
            _accessor.ReadArray(0L, array, 0, array.Length);
            GCHandle gCHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
            try
            {
                return (MianSharedConfig)Marshal.PtrToStructure(gCHandle.AddrOfPinnedObject(), typeof(MianSharedConfig));
            }
            finally
            {
                gCHandle.Free();
            }
        }

        // ── Window-finder helpers ────────────────────────────────────────────
        private static IntPtr FindGameWindow()
        {
            // 1. First probe universal emulator window scanner
            try
            {
                IntPtr universalWnd = GetProcess.FindEmulatorGameWindow();
                if (universalWnd != IntPtr.Zero) return universalWnd;
            }
            catch { }

            // 2. Direct process search
            foreach (var name in s_emulators)
            {
                try
                {
                    var procs = Process.GetProcessesByName(name);
                    var sortedProcs = procs.OrderByDescending(p => {
                        try {
                            return Math.Max(p.WorkingSet64, p.PrivateMemorySize64);
                        } catch { return 0L; }
                    });

                    foreach (var p in sortedProcs)
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

            // 3. Search top-level known emulator window classes if process MainWindowHandle was 0
            foreach (string cls in new[] { "BlueStacksApp", "MSIAppPlayer", "BstkAppPlayer", "BlueStacks", "RenderWindow" })
            {
                IntPtr top = FindWindowEx(IntPtr.Zero, IntPtr.Zero, cls, null);
                if (top != IntPtr.Zero && IsWindowVisible(top))
                {
                    return top;
                }
            }

            return IntPtr.Zero;
        }

        private static IntPtr FindViewport(IntPtr emulatorWnd)
        {
            if (emulatorWnd == IntPtr.Zero) return IntPtr.Zero;

            // 1. Try known child class names
            string[] knownClasses = new string[] {
                "BlueStacksApp", "MSIAppPlayer", "BstkAppPlayer", "BlueStacks",
                "RenderWindow", "Qt5QWindowIcon", "subWin", "AEngineRender"
            };
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
