using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public static class SecurityGuard
    {
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        private static extern bool CheckRemoteDebuggerPresent(IntPtr hProcess, ref bool isDebuggerPresent);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        private static extern bool IsDebuggerPresent();

        private static readonly string[] BlacklistedTools = new string[]
        {
            "dnSpy",
            "de4dot",
            "x64dbg",
            "x32dbg",
            "ida64",
            "idag",
            "idaw",
            "Cheat Engine",
            "HTTPDebuggerUI",
            "HTTPDebuggerSvc",
            "Fiddler",
            "Wireshark",
            "ProcessHacker",
            "OllyDbg"
        };

        public static bool ValidateEnvironment()
        {
            string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "boot.log");
            try
            {
                if (Debugger.IsAttached)
                {
                    try { File.AppendAllText(logPath, $"[{DateTime.Now}] SecurityGuard failed: Debugger.IsAttached\n"); } catch { }
                    return false;
                }

                if (IsDebuggerPresent())
                {
                    try { File.AppendAllText(logPath, $"[{DateTime.Now}] SecurityGuard failed: IsDebuggerPresent\n"); } catch { }
                    return false;
                }

                bool remoteDebugger = false;
                CheckRemoteDebuggerPresent(Process.GetCurrentProcess().Handle, ref remoteDebugger);
                if (remoteDebugger)
                {
                    try { File.AppendAllText(logPath, $"[{DateTime.Now}] SecurityGuard failed: CheckRemoteDebuggerPresent\n"); } catch { }
                    return false;
                }

                foreach (var tool in BlacklistedTools)
                {
                    if (Process.GetProcessesByName(tool).Length > 0)
                    {
                        try { File.AppendAllText(logPath, $"[{DateTime.Now}] SecurityGuard failed: Blacklisted tool {tool} found!\n"); } catch { }
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                try { File.AppendAllText(logPath, $"[{DateTime.Now}] SecurityGuard exception: {ex.Message}\n"); } catch { }
            }

            return true;
        }

        public static void StartContinuousProtection()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(3000);
                    if (!ValidateEnvironment())
                    {
                        string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "boot.log");
                        try { File.AppendAllText(logPath, $"[{DateTime.Now}] StartContinuousProtection exiting process!\n"); } catch { }
                        Environment.Exit(0);
                    }
                }
            });
        }
    }
}
