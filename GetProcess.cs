using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Aimbot.Mem
{
    public static class GetProcess
    {
        private static readonly string[] TargetProcesses = new[]
        {
            "HD-Player",        // BlueStacks 5 / 4 VM engine
            "HD-Frontend",      // BlueStacks 4 / MSI App Player UI window
            "BlueStacks",       // BlueStacks
            "BlueStacksApp",    // BlueStacks
            "MSIAppPlayer",     // MSI App Player
            "AppPlayer",        // MSI / BlueStacks variant
            "dnplayer",         // LDPlayer 3 / 4 / 5 / 9 UI
            "LdBoxHeadless",    // LDPlayer Headless VM
            "Ld9BoxHeadless",   // LDPlayer 9 Headless VM
            "MEmu",             // MEmu UI
            "MEmuHeadless",     // MEmu VM
            "Nox",              // NoxPlayer UI
            "NoxVMHandle",      // NoxPlayer VM
            "AndroidEmulator",  // Gameloop
            "AndroidEmulatorEn",// Gameloop
            "aow_exe",          // Gameloop AOW
            "VBoxHeadless",     // Genymotion / Andy / VirtualBox
            "AndyConsole",      // Andy
            "Droid4X",          // Droid4X
            "KoPlayer",         // KoPlayer
            "ProjectTitan",     // SmartGaGa
            "Titan"             // SmartGaGa
        };

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool EnumChildWindows(IntPtr hWndParent, EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT { public int Left, Top, Right, Bottom; }

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        public static string GetRunningEmulatorProcessName()
        {
            foreach (var name in TargetProcesses)
            {
                if (Process.GetProcessesByName(name).Any())
                {
                    return name;
                }
            }
            return null;
        }

        public static (string processName, int pid) GetRunningEmulatorProcess()
        {
            List<Process> allProcs = new List<Process>();

            foreach (var name in TargetProcesses)
            {
                var processes = Process.GetProcessesByName(name);
                allProcs.AddRange(processes);
            }

            var mainProc = allProcs.OrderByDescending(p => {
                try {
                    return Math.Max(p.WorkingSet64, p.PrivateMemorySize64);
                } catch { return 0L; }
            }).FirstOrDefault();

            if (mainProc != null)
            {
                return (mainProc.ProcessName, mainProc.Id);
            }

            return (null, 0);
        }

        /// <summary>
        /// Universal window finder that locates the emulator viewport across BlueStacks 4,
        /// BlueStacks 5, MSI App Player, LDPlayer, MEmu, Nox, and other Android emulators.
        /// Handles split-process architectures (e.g. HD-Frontend + HD-Player).
        /// </summary>
        public static IntPtr FindEmulatorGameWindow()
        {
            HashSet<uint> candidatePids = new HashSet<uint>();
            foreach (var name in TargetProcesses)
            {
                try
                {
                    foreach (var p in Process.GetProcessesByName(name))
                    {
                        candidatePids.Add((uint)p.Id);
                    }
                }
                catch { }
            }

            IntPtr bestViewport = IntPtr.Zero;
            int maxArea = 0;

            EnumWindows((hWnd, lParam) =>
            {
                if (!IsWindowVisible(hWnd)) return true;

                GetWindowThreadProcessId(hWnd, out uint wndPid);
                bool pidMatches = candidatePids.Contains(wndPid);

                StringBuilder sbTitle = new StringBuilder(256);
                GetWindowText(hWnd, sbTitle, 256);
                string title = sbTitle.ToString();

                StringBuilder sbClass = new StringBuilder(256);
                GetClassName(hWnd, sbClass, 256);
                string className = sbClass.ToString();

                bool titleMatches = title.IndexOf("BlueStacks", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                    title.IndexOf("MSI App Player", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                    title.IndexOf("LDPlayer", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                    title.IndexOf("MEmu", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                    title.IndexOf("Nox", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                    title.IndexOf("Free Fire", StringComparison.OrdinalIgnoreCase) >= 0;

                bool classMatches = className.IndexOf("BlueStacks", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                    className.IndexOf("MSI", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                    className.IndexOf("subWin", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                    className.IndexOf("Qt5", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                    className.IndexOf("RenderWindow", StringComparison.OrdinalIgnoreCase) >= 0;

                if (pidMatches || titleMatches || classMatches)
                {
                    // Scan children for viewport
                    EnumChildWindows(hWnd, (childHwnd, childParam) =>
                    {
                        if (!IsWindowVisible(childHwnd)) return true;
                        if (GetClientRect(childHwnd, out var rect))
                        {
                            int w = rect.Right - rect.Left;
                            int h = rect.Bottom - rect.Top;
                            int area = w * h;
                            if (w > 300 && h > 300 && area > maxArea)
                            {
                                bestViewport = childHwnd;
                                maxArea = area;
                            }
                        }
                        return true;
                    }, IntPtr.Zero);

                    // If no child was found, check top-level window dimensions
                    if (bestViewport == IntPtr.Zero && GetClientRect(hWnd, out var topRect))
                    {
                        int tw = topRect.Right - topRect.Left;
                        int th = topRect.Bottom - topRect.Top;
                        int topArea = tw * th;
                        if (tw > 300 && th > 300 && topArea > maxArea)
                        {
                            bestViewport = hWnd;
                            maxArea = topArea;
                        }
                    }
                }

                return true;
            }, IntPtr.Zero);

            return bestViewport;
        }
    }
}
