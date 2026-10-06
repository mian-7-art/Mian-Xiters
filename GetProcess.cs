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
            "HD-Player",        // BlueStacks
            "NoxVMHandle",      // NoxPlayer
            "LdBoxHeadless",    // LDPlayer
            "MEmuHeadless",     // MEmu
            "AndroidEmulatorEn",// Gameloop
            "VBoxHeadless",     // Genymotion / Andy
            "AndyConsole",      // Andy
            "Droid4X",          // Droid4X
            "KoPlayer",         // KoPlayer
            "ProjectTitan"      // SmartGaGa
        };

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        public static string GetRunningEmulatorProcessName()
        {
            foreach (var name in TargetProcesses)
            {
                if (Process.GetProcessesByName(name).Any())
                {
                    return name;
                }
            }
            //MessageBox.Show("No supported emulator process is running.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return null;
        }

        public static (string processName, int pid) GetRunningEmulatorProcess()
        {
            if (EspLib.EspCore.Handle != IntPtr.Zero)
            {
                GetWindowThreadProcessId(EspLib.EspCore.Handle, out uint windowPid);
                if (windowPid > 0)
                {
                    try
                    {
                        var proc = Process.GetProcessById((int)windowPid);
                        if (proc != null && !proc.HasExited)
                        {
                            return (proc.ProcessName, (int)windowPid);
                        }
                    }
                    catch { }
                }
            }

            foreach (var name in TargetProcesses)
            {
                var processes = Process.GetProcessesByName(name);
                if (processes.Any())
                {
                    var mainProc = processes.OrderByDescending(p => {
                        try {
                            long score = Math.Max(p.WorkingSet64, p.PrivateMemorySize64);
                            if (p.MainWindowHandle != IntPtr.Zero) score += 10000000000L;
                            return score;
                        } catch { return 0L; }
                    }).FirstOrDefault();

                    if (mainProc != null)
                    {
                        return (name, mainProc.Id);
                    }
                }
            }
            return (null, 0);
        }
    }
}