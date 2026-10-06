using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace AotForms;

internal static class AiAgentDetector
{
	private static Thread _monitorThread;

	private static volatile bool _isRunning = false;

	private static readonly HashSet<string> BannedProcessNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
	{
		"claude", "claude-code", "aider", "aider-chat", "goose", "goose-cli", "goose-ai", "agy", "antigravity-cli", "gemini-cli",
		"copilot", "gh-copilot", "copilot-agent", "github-copilot-cli", "cursor-agent", "cursor-cli", "devin", "devin-cli", "open-interpreter", "plandex",
		"cline", "crush", "codex-cli"
	};

	private static readonly HashSet<string> ScriptAndShellNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
	{
		"node", "python", "python3", "pythonw", "bun", "deno", "cmd", "powershell", "pwsh", "bash",
		"wsl"
	};

	private static readonly HashSet<string> TerminalHostNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
	{
		"cmd", "powershell", "pwsh", "windowsterminal", "wt", "conhost", "alacritty", "wezterm", "hyper", "mintty",
		"bash", "wsl"
	};

	private static readonly string[] CommandLineSignatures = new string[41]
	{
		"@anthropic-ai/claude-code", "claude-code", "claude.js", "claude.cmd", "\\claude.exe", "/claude.exe", " claude ", "\\claude ", "/claude ", "aider-chat",
		"aider.main", "run_aider", "\\aider.exe", "/aider.exe", " aider ", "\\aider ", "/aider ", "goose-ai", "goose.exe", "goose agent",
		" goose ", "\\goose ", "/goose ", "gh copilot", "gh-copilot", "copilot-agent", "github-copilot-cli", "@github/copilot", "gemini-cli", "antigravity-cli",
		"\\agy.exe", "/agy.exe", " agy ", "\\agy ", "/agy ", "open-interpreter", "open_interpreter", "plandex", "cursor-agent", "devin-cli",
		"codex-cli"
	};

	private static readonly string[] WindowTitleSignatures = new string[12]
	{
		"claude code", "claude", "aider", "copilot", "goose-ai", "goose cli", "gemini-cli", "open-interpreter", "plandex", "cursor-agent",
		"antigravity-cli", "agy cli"
	};

	public static void StartMonitoring()
	{
		CheckAndEnforce();
		if (!_isRunning)
		{
			_isRunning = true;
			_monitorThread = new Thread(WatchdogLoop)
			{
				IsBackground = true,
				Priority = ThreadPriority.BelowNormal,
				Name = "AiAgentDetectorWatchdog"
			};
			_monitorThread.Start();
		}
	}

	public static void StopMonitoring()
	{
		_isRunning = false;
	}

	private static void WatchdogLoop()
	{
		while (_isRunning)
		{
			try
			{
				CheckAndEnforce();
			}
			catch
			{
			}
			Thread.Sleep(1500);
		}
	}

	public static void CheckAndEnforce()
	{
		int processId = Environment.ProcessId;
		List<(Process, string)> list = new List<(Process, string)>();
		Process[] processes;
		try
		{
			processes = Process.GetProcesses();
		}
		catch
		{
			return;
		}
		Process[] array = processes;
		foreach (Process process in array)
		{
			if (process.Id == processId)
			{
				continue;
			}
			string processName;
			try
			{
				processName = process.ProcessName;
			}
			catch
			{
				continue;
			}
			if (processName.Equals("Antigravity", StringComparison.OrdinalIgnoreCase) || processName.Equals("Code", StringComparison.OrdinalIgnoreCase))
			{
				continue;
			}
			if (BannedProcessNames.Contains(processName))
			{
				list.Add((process, "Process Name: " + processName));
				continue;
			}
			string[] windowTitleSignatures;
			if (TerminalHostNames.Contains(processName))
			{
				try
				{
					string mainWindowTitle = process.MainWindowTitle;
					if (!string.IsNullOrWhiteSpace(mainWindowTitle))
					{
						windowTitleSignatures = WindowTitleSignatures;
						foreach (string value in windowTitleSignatures)
						{
							if (mainWindowTitle.Contains(value, StringComparison.OrdinalIgnoreCase))
							{
								list.Add((process, $"Window Title '{mainWindowTitle}' matched '{value}'"));
								break;
							}
						}
					}
				}
				catch
				{
				}
			}
			if (!ScriptAndShellNames.Contains(processName))
			{
				continue;
			}
			string commandLine = GetCommandLine(process.Id);
			if (string.IsNullOrEmpty(commandLine))
			{
				continue;
			}
			windowTitleSignatures = CommandLineSignatures;
			foreach (string text in windowTitleSignatures)
			{
				if (commandLine.Contains(text, StringComparison.OrdinalIgnoreCase))
				{
					list.Add((process, "Command line matched '" + text + "' in " + processName));
					break;
				}
			}
		}
		if (list.Count > 0)
		{
			TerminateAndExit(list);
		}
	}

	private static void TerminateAndExit(List<(Process Proc, string Reason)> detectedList)
	{
		try
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("\n========================================================");
			Console.WriteLine("[SECURITY ALERT] AI CLI Agent detected!");
			foreach (var detected in detectedList)
			{
				Console.WriteLine($" -> Terminating {detected.Proc.ProcessName} (PID: {detected.Proc.Id}): {detected.Reason}");
			}
			Console.WriteLine("[SECURITY ALERT] Automatically shutting down application...");
			Console.WriteLine("========================================================\n");
			Console.ResetColor();
		}
		catch
		{
		}
		foreach (var detected2 in detectedList)
		{
			try
			{
				detected2.Proc.Kill(entireProcessTree: true);
			}
			catch
			{
			}
		}
		try
		{
			Process.GetCurrentProcess().Kill();
		}
		catch
		{
			Environment.Exit(0);
		}
	}

	public static string GetCommandLine(int pid)
	{
		nint num = NativeMethods.OpenProcess(5136u, bInheritHandle: false, pid);
		if (num == IntPtr.Zero)
		{
			num = NativeMethods.OpenProcess(4096u, bInheritHandle: false, pid);
		}
		if (num == IntPtr.Zero)
		{
			return string.Empty;
		}
		nint num2 = IntPtr.Zero;
		try
		{
			num2 = Marshal.AllocHGlobal(32768);
			if (NativeMethods.NtQueryInformationProcess(num, 60, num2, 32768, out var _) == 0)
			{
				NativeMethods.UNICODE_STRING uNICODE_STRING = Marshal.PtrToStructure<NativeMethods.UNICODE_STRING>(num2);
				if (uNICODE_STRING.Length > 0 && uNICODE_STRING.Buffer != IntPtr.Zero)
				{
					return Marshal.PtrToStringUni(uNICODE_STRING.Buffer, uNICODE_STRING.Length / 2) ?? string.Empty;
				}
			}
		}
		catch
		{
		}
		finally
		{
			if (num2 != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(num2);
			}
			NativeMethods.CloseHandle(num);
		}
		return string.Empty;
	}
}
