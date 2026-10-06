using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Aimbot.Mem;

namespace WpfApp1
{
    public class PatternDiagnosticReport
    {
        public string Timestamp { get; set; }
        public string FeatureName { get; set; }
        public string ProcessName { get; set; }
        public string Status { get; set; }
        public int MatchCount { get; set; }
        public string Details { get; set; }
    }

    public static class DiagnosticLogger
    {
        private static readonly string LogFilePath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "pattern_health.json"
        );

        private static readonly object _logLock = new object();

        // Flag to check if running specifically on your developer PC
        private static bool IsDeveloperMachine =>
            string.Equals(Environment.UserName, "nafee", StringComparison.OrdinalIgnoreCase) ||
            File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dev_debug.flag"));

        /// <summary>
        /// Automatically deletes any legacy pattern_health.json file when users launch the tool.
        /// </summary>
        public static void PurgeUserDiagnostics()
        {
            try
            {
                if (!IsDeveloperMachine && File.Exists(LogFilePath))
                {
                    File.Delete(LogFilePath);
                }
            }
            catch { }
        }

        public static void LogResult(string featureName, string pattern, bool success, int matchCount = 0, string offsets = null, string details = null)
        {
            // If running on end-user PC, purge the log file and DO NOT record anything
            if (!IsDeveloperMachine)
            {
                PurgeUserDiagnostics();
                return;
            }

            try
            {
                var (procName, pid) = GetProcess.GetRunningEmulatorProcess();
                long ramMb = 0;
                try
                {
                    if (pid > 0)
                    {
                        var p = System.Diagnostics.Process.GetProcessById(pid);
                        ramMb = p.WorkingSet64 / (1024 * 1024);
                    }
                }
                catch { }

                string detailedInfo = details ?? (success ? $"Found {matchCount} memory match(es)" : "0 matches found");
                if (!string.IsNullOrEmpty(offsets))
                {
                    detailedInfo += $" | Offsets: {offsets}";
                }
                detailedInfo += $" | Attached PID: {pid} (RAM: {ramMb} MB)";

                var report = new PatternDiagnosticReport
                {
                    Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    FeatureName = featureName,
                    ProcessName = procName ?? "Not Running",
                    Status = success ? "SUCCESS" : "FAILED",
                    MatchCount = matchCount,
                    Details = detailedInfo
                };

                lock (_logLock)
                {
                    List<PatternDiagnosticReport> logs = new List<PatternDiagnosticReport>();
                    if (File.Exists(LogFilePath))
                    {
                        try
                        {
                            string existingJson = File.ReadAllText(LogFilePath);
                            logs = JsonConvert.DeserializeObject<List<PatternDiagnosticReport>>(existingJson) ?? new List<PatternDiagnosticReport>();
                        }
                        catch
                        {
                            logs = new List<PatternDiagnosticReport>();
                        }
                    }

                    logs.Add(report);
                    if (logs.Count > 150) logs.RemoveAt(0);

                    string updatedJson = JsonConvert.SerializeObject(logs, Formatting.Indented);
                    File.WriteAllText(LogFilePath, updatedJson);
                }
            }
            catch { }
        }
    }
}
