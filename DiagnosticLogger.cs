using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Aimbot.Mem;
using EspLib;

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
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AutoResolution { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SuggestedAction { get; set; }
    }

    public class CrashReportData
    {
        public string Timestamp { get; set; }
        public string ErrorType { get; set; }
        public string SourceContext { get; set; }
        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }
        public string InnerException { get; set; }
        public string AttachedProcess { get; set; }
        public int ProcessId { get; set; }
        public long ProcessRamMb { get; set; }
        public string OperatingSystem { get; set; }
        public string ActiveFeatureStates { get; set; }
        public string AutomatedDiagnosis { get; set; }
        public string SuggestedFix { get; set; }
    }

    public static class DiagnosticLogger
    {
        private static readonly string LogFilePath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "pattern_health.json"
        );

        private static readonly string CrashJsonPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "crash_report.json"
        );

        private static readonly string CrashTxtPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "crash_report.txt"
        );

        private static readonly object _logLock = new object();
        private static int _monitoredPid = 0;
        private static System.Threading.Thread _watcherThread = null;

        public static readonly List<PatternDiagnosticReport> RecentReports = new List<PatternDiagnosticReport>();

        public static void StartEmulatorWatcher()
        {
#if DEBUG
            if (_watcherThread != null && _watcherThread.IsAlive) return;

            _watcherThread = new System.Threading.Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        var (pName, pid) = GetProcess.GetRunningEmulatorProcess();
                        if (pid > 0 && pid != _monitoredPid)
                        {
                            _monitoredPid = pid;
                            MonitorProcess(pName, pid);
                        }
                    }
                    catch { }
                    System.Threading.Thread.Sleep(1500);
                }
            }) { IsBackground = true };
            _watcherThread.Start();
#endif
        }

        private static void MonitorProcess(string pName, int pid)
        {
            try
            {
                var proc = System.Diagnostics.Process.GetProcessById(pid);
                proc.EnableRaisingEvents = true;
                proc.Exited += (s, e) =>
                {
                    try
                    {
                        int exitCode = -1;
                        try { exitCode = proc.ExitCode; } catch { }

                        string hexCode = $"0x{exitCode:X8}";
                        string reason = exitCode == 0 ? "Normal User Exit" : $"Emulator crashed with ExitCode {hexCode}";

                        LogEmulatorCrash(pName, pid, reason);
                    }
                    catch { }
                };
            }
            catch { }
        }

        public static void PurgeUserDiagnostics()
        {
            // Retained for backward compatibility
        }

        public static void LogCrash(Exception ex, string contextSource = "Unhandled Exception")
        {
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

                string activeFeatures = GetActiveFeaturesSummary();
                string diagnosis = DiagnoseCrash(ex, procName, pid);
                string suggestedFix = SuggestFixForCrash(ex, procName, pid);

                var report = new CrashReportData
                {
                    Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    ErrorType = ex?.GetType().FullName ?? "Unknown Exception",
                    SourceContext = contextSource,
                    ErrorMessage = ex?.Message ?? "No exception message provided",
                    StackTrace = ex?.StackTrace ?? "No stack trace available",
                    InnerException = ex?.InnerException?.ToString() ?? "None",
                    AttachedProcess = procName ?? "No Emulator Attached",
                    ProcessId = pid,
                    ProcessRamMb = ramMb,
                    OperatingSystem = Environment.OSVersion.ToString(),
                    ActiveFeatureStates = activeFeatures,
                    AutomatedDiagnosis = diagnosis,
                    SuggestedFix = suggestedFix
                };

#if DEBUG
                lock (_logLock)
                {
                    // Update crash_report.json
                    List<CrashReportData> crashLogs = new List<CrashReportData>();
                    if (File.Exists(CrashJsonPath))
                    {
                        try
                        {
                            string json = File.ReadAllText(CrashJsonPath);
                            crashLogs = JsonConvert.DeserializeObject<List<CrashReportData>>(json) ?? new List<CrashReportData>();
                        }
                        catch { }
                    }
                    crashLogs.Add(report);
                    if (crashLogs.Count > 50) crashLogs.RemoveAt(0);

                    File.WriteAllText(CrashJsonPath, JsonConvert.SerializeObject(crashLogs, Formatting.Indented));

                    // Append to human-readable crash_report.txt
                    string txtHeader = $"==================== CRASH REPORT [{report.Timestamp}] ====================\r\n" +
                                       $"Source Context      : {report.SourceContext}\r\n" +
                                       $"Error Type          : {report.ErrorType}\r\n" +
                                       $"Message             : {report.ErrorMessage}\r\n" +
                                       $"Attached Emulator   : {report.AttachedProcess} (PID: {report.ProcessId}, RAM: {report.ProcessRamMb} MB)\r\n" +
                                       $"OS                  : {report.OperatingSystem}\r\n" +
                                       $"Active Features     : {report.ActiveFeatureStates}\r\n" +
                                       $"Automated Diagnosis : {report.AutomatedDiagnosis}\r\n" +
                                       $"Suggested Fix       : {report.SuggestedFix}\r\n" +
                                       $"-------------------- Stack Trace --------------------\r\n" +
                                       $"{report.StackTrace}\r\n" +
                                       $"=====================================================================\r\n\r\n";

                    File.AppendAllText(CrashTxtPath, txtHeader);
                }
#endif
            }
            catch { }
        }

        public static void LogEmulatorCrash(string emulatorName, int pid, string reason)
        {
            try
            {
                var ex = new InvalidOperationException($"Emulator crash or unexpected exit detected: {reason}");
                LogCrash(ex, $"Emulator Watcher [{emulatorName}]");
            }
            catch { }
        }

        private static string GetActiveFeaturesSummary()
        {
            List<string> active = new List<string>();
            if (EspConfig.EspMaster) active.Add("ESP Master");
            if (EspConfig.EspBox) active.Add("ESP Box");
            if (EspConfig.EspSkeleton) active.Add("ESP Skeleton");
            if (EspConfig.EspHealth) active.Add("ESP Health");
            if (EspConfig.EspName) active.Add("ESP Name");
            if (EspConfig.EspLine) active.Add("ESP SnapLines");
            if (EspConfig.EspDistance) active.Add("ESP Distance");
            if (EspConfig.Esp360) active.Add("ESP Offscreen Radar (360°)");

            return active.Count > 0 ? string.Join(", ", active) : "None Active";
        }

        private static string DiagnoseCrash(Exception ex, string procName, int pid)
        {
            if (pid <= 0 || string.IsNullOrEmpty(procName))
            {
                return "Emulator process is not running or was closed prior to the crash.";
            }

            string msg = ex?.Message ?? "";
            if (msg.IndexOf("AccessViolation", StringComparison.OrdinalIgnoreCase) >= 0 ||
                msg.IndexOf("ReadProcessMemory", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "Game process memory page was invalidated or freed during match transition.";
            }
            if (msg.IndexOf("NullReference", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "Null pointer encountered during memory offsets translation.";
            }
            if (msg.IndexOf("libil2cpp", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "Guest VM VMA mapping for libil2cpp.so returned invalid base address.";
            }

            return "General runtime exception occurred during execution.";
        }

        private static string SuggestFixForCrash(Exception ex, string procName, int pid)
        {
            if (pid <= 0)
            {
                return "Relaunch BlueStacks / Free Fire, wait until in-game lobby, then re-enable cheat features.";
            }

            string msg = ex?.Message ?? "";
            if (msg.IndexOf("Access", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "Ensure cheat application is launched as Administrator and antivirus real-time scan is disabled.";
            }

            return "Press F5 key to instantly refresh ESP and memory handles.";
        }

        public static void LogResult(string featureName, string pattern, bool success, int matchCount = 0, string offsets = null, string details = null, string autoResolution = null, string suggestedAction = null)
        {
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

                string statusText = success ? "SUCCESS" : "FAILED";
                if (success && !string.IsNullOrEmpty(autoResolution) && autoResolution.IndexOf("Auto-Recovered", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    statusText = "AUTO_RECOVERED";
                }

                var report = new PatternDiagnosticReport
                {
                    Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    FeatureName = featureName,
                    ProcessName = procName ?? "Not Running",
                    Status = statusText,
                    MatchCount = matchCount,
                    Details = detailedInfo,
                    AutoResolution = autoResolution,
                    SuggestedAction = suggestedAction
                };

                lock (_logLock)
                {
                    RecentReports.Add(report);
                    if (RecentReports.Count > 100) RecentReports.RemoveAt(0);

#if DEBUG
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
#endif
                }
            }
            catch { }
        }
    }
}
