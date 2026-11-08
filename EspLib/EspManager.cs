using System;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Aimbot.Mem;
using WpfApp1;

namespace EspLib
{

public static class EspManager
{
	private static Process _espProcess;

	private static MemoryMappedFile _mmf;

	private static MemoryMappedViewAccessor _accessor;

	private static bool _running;

	private static readonly object _syncLock = new object();

	private static bool _watchdogRunning;

	private static bool _isRestarting;

	public static ulong CurrentLocalPlayer
	{
		get
		{
			if (_accessor == null)
			{
				return 0uL;
			}
			try
			{
				return ReadConfig().LocalPlayer;
			}
			catch
			{
				return 0uL;
			}
		}
	}

	public static bool IsRunning
	{
		get
		{
			if (_running && _espProcess != null)
			{
				return !_espProcess.HasExited;
			}
			return false;
		}
	}

	public static event Action<string> OnStatus;

	public static void RefreshEsp()
	{
		if (_running)
		{
			if (_espProcess == null || _espProcess.HasExited)
			{
				RestartEspProcessInternal();
			}
			else
			{
				UpdateOptions();
			}
		}
	}

	public static async Task<string> EnableEsp()
	{
		if (IsRunning)
		{
			UpdateOptions();
			return "ESP ON";
		}
		if (GetProcess.GetRunningEmulatorProcess().pid <= 0)
		{
			return "Target emulator (HD-Player) is not running.";
		}
		string text = FindOrExtractEspExecutable();
		if (string.IsNullOrEmpty(text) || !File.Exists(text))
		{
			return "Kernel ESP binary not found in assembly resources.";
		}
		try
		{
			int num = Marshal.SizeOf(typeof(MianSharedConfig));
			_mmf = MemoryMappedFile.CreateOrOpen("Local\\MianEspSharedConfig", num, MemoryMappedFileAccess.ReadWrite);
			_accessor = _mmf.CreateViewAccessor(0L, num, MemoryMappedFileAccess.ReadWrite);
			MianSharedConfig cfg = BuildCurrentConfig();
			cfg.Status = 1u;
			cfg.Terminate = 0u;
			cfg.Enable = 1u;
			cfg.ErrorMsg = "Connecting to guest VM...";
			WriteConfig(cfg);
		}
		catch (Exception ex)
		{
			DiagnosticLogger.LogResult("ESP Master", "IPC Memory", success: false, 0, null, ex.Message);
			return "Failed to initialize ESP shared memory: " + ex.Message;
		}
		try
		{
			KillExistingEspProcesses();
			_espProcess = Process.Start(new ProcessStartInfo
			{
				FileName = text,
				Arguments = "--launcher",
				WorkingDirectory = Path.GetDirectoryName(text),
				UseShellExecute = false,
				CreateNoWindow = true,
				WindowStyle = ProcessWindowStyle.Hidden
			});
			_running = true;
			EspConfig.EspMaster = true;
			StartWatchdog();
		}
		catch (Exception ex2)
		{
			DisableEsp();
			return "Failed to launch ESP engine: " + ex2.Message;
		}
		await Task.Delay(400);
		if (_espProcess == null || _espProcess.HasExited)
		{
			DisableEsp();
			return "ESP Engine process terminated on startup.";
		}
		UpdateOptions();
		DiagnosticLogger.LogResult("ESP Master", "DirectX 11 Engine", success: true, 1, "Status=Active", "Hardware-accelerated DirectX 11 ESP Running");
		return "ESP ON";
	}

	private static void StartWatchdog()
	{
		lock (_syncLock)
		{
			if (!_watchdogRunning)
			{
				_watchdogRunning = true;
				Task.Run((Func<Task>)EspWatchdogLoop);
			}
		}
	}

	private static async Task EspWatchdogLoop()
	{
		while (_running)
		{
			await Task.Delay(1500).ConfigureAwait(continueOnCapturedContext: false);
			if (!_running)
			{
				break;
			}
			try
			{
				if (_espProcess == null || _espProcess.HasExited)
				{
					DiagnosticLogger.LogResult("ESP Watchdog", "DirectX 11 Engine", success: false, 0, null, "KernelEsp process exited unexpectedly. Auto-reviving...");
					await RestartEspProcessInternal();
				}
				else if (_accessor != null)
				{
					MianSharedConfig mianSharedConfig = ReadConfig();
					if (mianSharedConfig.Status == 3 || mianSharedConfig.Magic != 1296646478)
					{
						UpdateOptions();
					}
				}
			}
			catch
			{
			}
		}
		lock (_syncLock)
		{
			_watchdogRunning = false;
		}
	}

	private static async Task RestartEspProcessInternal()
	{
		if (_isRestarting || !_running)
		{
			return;
		}
		_isRestarting = true;
		try
		{
			if (GetProcess.GetRunningEmulatorProcess().pid <= 0)
			{
				return;
			}
			string text = FindOrExtractEspExecutable();
			if (!string.IsNullOrEmpty(text) && File.Exists(text))
			{
				KillExistingEspProcesses();
				if (_accessor == null || _mmf == null)
				{
					int num = Marshal.SizeOf(typeof(MianSharedConfig));
					_mmf = MemoryMappedFile.CreateOrOpen("Local\\MianEspSharedConfig", num, MemoryMappedFileAccess.ReadWrite);
					_accessor = _mmf.CreateViewAccessor(0L, num, MemoryMappedFileAccess.ReadWrite);
				}
				MianSharedConfig cfg = BuildCurrentConfig();
				cfg.Status = 1u;
				cfg.Terminate = 0u;
				cfg.Enable = 1u;
				cfg.ErrorMsg = "Auto-reconnecting to guest VM...";
				WriteConfig(cfg);
				_espProcess = Process.Start(new ProcessStartInfo
				{
					FileName = text,
					Arguments = "--launcher",
					WorkingDirectory = Path.GetDirectoryName(text),
					UseShellExecute = false,
					CreateNoWindow = true,
					WindowStyle = ProcessWindowStyle.Hidden
				});
				await Task.Delay(400);
				if (_espProcess != null && !_espProcess.HasExited)
				{
					UpdateOptions();
					DiagnosticLogger.LogResult("ESP Watchdog", "DirectX 11 Engine", success: true, 1, "Status=Active", "Kernel ESP Engine Auto-Recovered");
				}
			}
		}
		catch (Exception ex)
		{
			DiagnosticLogger.LogResult("ESP Watchdog", "Auto-Restart", success: false, 0, null, ex.Message);
		}
		finally
		{
			_isRestarting = false;
		}
	}

	public static void UpdateOptions()
	{
		if (_accessor == null)
		{
			return;
		}
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
		catch
		{
		}
	}

	public static void ApplyStreamMode(bool streamMode)
	{
		if (_accessor == null)
		{
			return;
		}
		try
		{
			lock (_syncLock)
			{
				MianSharedConfig cfg = ReadConfig();
				cfg.StreamMode = (streamMode ? 1u : 0u);
				WriteConfig(cfg);
			}
		}
		catch
		{
		}
	}

	public static void DisableEsp()
	{
		_running = false;
		_watchdogRunning = false;
		EspConfig.EspMaster = false;
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
		catch
		{
		}
		try
		{
			if (_espProcess != null && !_espProcess.HasExited && !_espProcess.WaitForExit(500))
			{
				_espProcess.Kill();
			}
		}
		catch
		{
		}
		finally
		{
			_espProcess = null;
		}
		try
		{
			_accessor?.Dispose();
		}
		catch
		{
		}
		try
		{
			_mmf?.Dispose();
		}
		catch
		{
		}
		_accessor = null;
		_mmf = null;
	}

	private static MianSharedConfig BuildCurrentConfig()
	{
		return new MianSharedConfig
		{
			Magic = 1296646478u,
			Enable = 1u,
			Skeleton = (EspConfig.EspSkeleton ? 1u : 0u),
			Lines = (EspConfig.EspLine ? 1u : 0u),
			LinePos = (uint)EspConfig.LinePosition,
			Box = (EspConfig.EspCornerBox ? 1u : 0u),
			BoxType = 1u,
			HealthBar = (EspConfig.EspHealth ? 1u : 0u),
			Weapon = (EspConfig.EspWeapon ? 1u : 0u),
			Name = (EspConfig.EspName ? 1u : 0u),
			Distance = (EspConfig.EspDistance ? 1u : 0u),
			Radar = (EspConfig.Esp360 ? 1u : 0u),
			Team = (EspConfig.EspTeam ? 1u : 0u),
			StreamMode = (EspConfig.StreamingMode ? 1u : 0u),
			AutoRefresh = (EspConfig.AutoRefresh ? 1u : 0u),
			ColorR = (float)(int)EspConfig.EnemyColor.R / 255f,
			ColorG = (float)(int)EspConfig.EnemyColor.G / 255f,
			ColorB = (float)(int)EspConfig.EnemyColor.B / 255f,
			ColorA = 1f
		};
	}

	private static void WriteConfig(MianSharedConfig cfg)
	{
		if (_accessor == null)
		{
			return;
		}
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
		if (_accessor == null)
		{
			return default;
		}
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

	private static string FindOrExtractEspExecutable()
	{
		string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
		string text = Path.Combine(baseDirectory, "KernelEsp.exe");
		
		try
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string text3 = executingAssembly.GetManifestResourceNames().FirstOrDefault((string n) => n.EndsWith("KernelEsp.exe", StringComparison.OrdinalIgnoreCase) || n.IndexOf("KernelEsp", StringComparison.OrdinalIgnoreCase) >= 0);
			if (text3 != null)
			{
				using Stream stream = executingAssembly.GetManifestResourceStream(text3);
				if (stream != null)
				{
					if (!File.Exists(text) || new FileInfo(text).Length != stream.Length)
					{
						try
						{
							using (FileStream destination = new FileStream(text, FileMode.Create, FileAccess.Write))
							{
								stream.CopyTo(destination);
							}
							return text;
						}
						catch
						{
							string text4 = Path.Combine(Path.GetTempPath(), "KernelEsp.exe");
							stream.Position = 0L;
							using (FileStream destination2 = new FileStream(text4, FileMode.Create, FileAccess.Write))
							{
								stream.CopyTo(destination2);
							}
							return text4;
						}
					}
					return text;
				}
			}
		}
		catch
		{
		}
		if (File.Exists(text))
		{
			return text;
		}
		string path = Path.Combine(baseDirectory, "..", "..", "KernelEsp.exe");
		if (File.Exists(path))
		{
			return Path.GetFullPath(path);
		}
		return null;
	}

	private static void KillExistingEspProcesses()
	{
		string[] array = new string[2] { "KernelEsp", "RCC ESP Project" };
		foreach (string processName in array)
		{
			try
			{
				Process[] processesByName = Process.GetProcessesByName(processName);
				foreach (Process process in processesByName)
				{
					try
					{
						process.Kill();
					}
					catch
					{
					}
				}
			}
			catch
			{
			}
		}
	}
}
}
