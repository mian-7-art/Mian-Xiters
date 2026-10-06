using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    /// <summary>
    /// Lightweight, zero-performance-impact security guard that protects the Debug executable
    /// against debuggers, decompilers (dnSpy, ILSpy), memory dumpers, and cracking tools.
    /// Runs silently without altering any memory engines or cheat functionality.
    /// </summary>
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

        /// <summary>
        /// Validates process environment. Returns false if a debugger or cracking tool is attached.
        /// </summary>
        public static bool ValidateEnvironment()
        {
            try
            {
                // 1. Check managed debugger
                if (Debugger.IsAttached) return false;

                // 2. Check native Win32 debugger
                if (IsDebuggerPresent()) return false;

                // 3. Check remote process debugger
                bool remoteDebugger = false;
                CheckRemoteDebuggerPresent(Process.GetCurrentProcess().Handle, ref remoteDebugger);
                if (remoteDebugger) return false;

                // 4. Scan active processes for reverse engineering tools
                foreach (var tool in BlacklistedTools)
                {
                    if (Process.GetProcessesByName(tool).Length > 0)
                    {
                        return false;
                    }
                }
            }
            catch
            {
                // Fail-safe: continue execution if OS policy restricts process query
            }

            return true;
        }

        /// <summary>
        /// Simple XOR string obfuscation helper for sensitive memory patterns and endpoints.
        /// </summary>
        public static string DecodeString(byte[] encodedData, byte key = 0x5A)
        {
            byte[] result = new byte[encodedData.Length];
            for (int i = 0; i < encodedData.Length; i++)
            {
                result[i] = (byte)(encodedData[i] ^ key);
            }
            return Encoding.UTF8.GetString(result);
        }

        /// <summary>
        /// Starts a background watcher thread that silently monitors for newly opened cracking tools.
        /// </summary>
        public static void StartContinuousProtection()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(3000);
                    if (!ValidateEnvironment())
                    {
                        Environment.Exit(0);
                    }
                }
            });
        }
    }
}
