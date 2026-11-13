using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Aimbot.Mem;
using EspLib;
using MOZIPRIME;

namespace WpfApp1
{


public class Cheat
{
	public enum AimbotActiveMode
	{
		None,
		Head,
		V2,
		Mian100,
		New
	}

	public enum TargetBone
	{
		Head,
		Neck,
		Chest,
		Hip,
		Feet
	}

	private const uint MEM_RELEASE = 32768u;

	private static readonly object _aimbotLock = new object();

	private static bool _aimbotWatcherRunning = false;

	private static bool _aimbotKeyCooldown = false;

	private static bool _heartbeatRunning = false;

	private static AimbotActiveMode _currentMode = AimbotActiveMode.None;

	private static bool _isEnablingMian = false;

	private const int MaxEntityCount = 20;

	private static readonly Random _hbRng = new Random();
	private static readonly Random _scopeRng = new Random();

	private static readonly string[] TaskName = new string[10] { "HD-Player", "Bluestacks", "Bluestacks-5", "HD-Agent", "HdPlayerService", "BstkSVC", "LdBoxHeadless", "dnplayer", "NoxVMHandle", "MEmuHeadless" };

	// Shared verified entity signature (FF FF prefix, 4-FF middle anchor, A5 43 tail) — 100% verified 40 matches in live memory
	private static string _aobBeta =
		"FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 " +
		"00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 " +
		"FF FF FF FF " +
		"00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 " +
		"?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? " +
		"?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? " +
		"00 00 00 00 00 00 00 00 00 00 00 00 00 00 " +
		"A5 43";

	// Aimbot Alpha (Head) — targets Head hitbox (Pair 1: Write=0xB6, Read=0xBA)
	private static string _aobAlpha = _aobBeta;
	private static long _rOffAlpha = 0xBA;
	private static long _wOffAlpha = 0xB6;
	private static readonly mozih4x _memAlpha = new mozih4x();
	private static (long addr, int onVal, int offVal, long baseAddr)[] _listAlpha =
		new (long, int, int, long)[0];

	// Aimbot Beta (V2) — dual-pair targeting (Head 0xB6 + Chest/Hip 0xB8)
	private static long _rOff1Beta = 0xBA;
	private static long _wOff1Beta = 0xB6;
	private static long _rOff2Beta = 0xBC;
	private static long _wOff2Beta = 0xB8;
	private static readonly mozih4x _memBeta = new mozih4x();
	private static (long addr, int onVal, int offVal, long baseAddr)[] _listBeta =
		new (long, int, int, long)[0];
	private static List<long> _betaBaseAddresses = new List<long>();

	// Aimbot Gamma (New) — targets Body/Bone pair (Pair 2: Write=0xB8, Read=0xBC)
	private static string _aobGamma = _aobBeta;
	private static long _rOffGamma = 0xBC;
	private static long _wOffGamma = 0xB8;
	private static readonly mozih4x _memGamma = new mozih4x();
	private static (long addr, int onVal, int offVal, long baseAddr)[] _listGamma =
		new (long, int, int, long)[0];

	private static readonly string ScopeAob = "FF FF FF FF 08 00 00 00 00 00 60 40 CD CC 8C 3F 8F C2 F5 3C CD CC CC 3D 06 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 33 33 13 40 00 00 B0 3F 00 00 80 3F 01";

	private static readonly string ScopeReplace = "FF FF FF FF 08 00 00 00 00 00 60 40 CD CC 8C 3F 8F C2 F5 3C CD CC CC 3D 06 00 00 00 00 00 19 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00";

	private static IEnumerable<long> _sniperScope1 = new List<long>();

	private static readonly mozih4x _scopeMem = new mozih4x();

	private static bool _scopeWatcherRunning = false;

	private static bool _scopeCooldown = false;

	private static readonly string SniperSwitchAob = "3F 00 00 80 3E 00 00 00 00 04 00 00 00 00 00 80 3F 00 00 20 41 00 00 34 42 01 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F";

	private static readonly string SniperSwitchReplace = "01 00 00 80 00 00 00 00 00 04 00 00 00 00 00 80 3F 00 00 20 41 00 00 34 42 01 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F";

	private static List<long> _sniperSwitchCache = new List<long>();

	private static readonly mozih4x _switchMem = new mozih4x();

	private static bool _chamsInjected = false;

	private static readonly string GlitchFireAob = "90 C1 00 00 70 41 01 00 00 00 00 00 C0 3F 00 00 00 3F 00 00 80 3F";

	private static readonly string GlitchFireReplace = "90 C1 00 00 70 41 01 00 00 00 00 00 00 BF 00 00 00 3F 00 00 80 3F";

	private static List<long> _glitchFireAddresses = new List<long>();

	private static readonly mozih4x _glitchFireMem = new mozih4x();

	private static readonly string FastLandingAob = "00 00 00 00 00 00 80 3F 00 00 00 00 00 00 00 00 00 00 80 BF 00 00 00 00 00 00 80 BF 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 00 00 00 00 00 00 00 00 80 BF 00 00 80 7F 00 00 80 7F 00 00 80 7F 00 00 80 FF";

	private static readonly string FastLandingReplace = "00 00 00 00 00 00 FF 41 00 00 00 00 00 00 00 00 00 00 80 BF 00 00 00 00 00 00 80 BF 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 00 00 00 00 00 00 00 00 80 BF 00 00 80 7F 00 00 80 7F 00 00 80 7F 00 00 80 FF";

	private static List<long> _fastLandingAddresses = new List<long>();

	private static readonly mozih4x _fastLandingMem = new mozih4x();

	private static bool _fastLandingWatcherRunning = false;

	private static bool _fastLandingActive = false;

	private static bool _fastLandingCooldown = false;

	private static readonly string CameraViewAob = "00 00 00 00 00 00 80 40 00 00 00 00 00 00 00 00 00 00 80 BF 00 00 00 00 00 00 80 BF 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 00 00 00 00 80 BF";

	private static readonly string CameraViewReplace = "00 00 00 00 00 00 80 3F 00 00 00 00 00 00 00 00 00 00 80 BF 00 00 00 00 00 00 80 BF 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 00 00 00 00 00 00";

	private static List<long> _cameraViewAddresses = new List<long>();

	private static readonly mozih4x _cameraViewMem = new mozih4x();

	private static bool _cameraViewWatcherRunning = false;

	private static bool _cameraViewActive = false;

	private static bool _cameraViewCooldown = false;

	private static readonly string GuestResetAob = "10 40 2D E9 D0 40 9F E5 04 40 8F E0 00 00 D4 E5 00 00 50 E3 04 00 00 1A C0 00 9F E5 00 00 9F E7 F1";

	private static readonly string GuestResetReplace = "01 00 A0 E3 1E FF 2F E1 04 40 8F E0 00 00 D4 E5 00 00 50 E3 04 00 00 1A C0 00 9F E5 00 00 9F E7 F1";

	private static List<long> _guestResetAddresses = new List<long>();

	private static readonly mozih4x _guestResetMem = new mozih4x();

	private static readonly string ScopeTrackingAob = "CD CC CC 3D 02 00 00 00 EC 51 B8 3D CD CC 4C 3F 00 00 00 00 00 00 A0 42 00 00 C0 3F 33 33 13 40 00 00 F0 3F 00 00 80 3F 01 00 00 00";

	private static readonly string ScopeTrackingReplace = "CD CC CC 3D 02 00 00 00 EC 51 B8 3D CD CC 4C 3F 00 00 00 00 00 00 A0 42 00 00 C0 3F 33 33 13 40 00 00 F0 3F 00 00 80 4F 01 00 00 00";

	private static List<long> _scopeTrackingAddresses = new List<long>();

	private static readonly mozih4x _scopeTrackingMem = new mozih4x();

	private static TargetBone _selectedBone = TargetBone.Head;

	public static int AimbotHeadKey { get; set; } = 0;

	public static int AimbotV2Key { get; set; } = 0;

	public static int AimbotMianKey { get; set; } = 0;

	public static int AimbotNewKey { get; set; } = 0;

	public static int HideMenuKey { get; set; } = 46;

	public static bool AntiBlacklistMode { get; set; } = true;

	public static int FastLandingKey { get; set; } = 0;

	public static bool IsFastLandingActive => _fastLandingActive;

	public static bool IsCameraViewActive => _cameraViewActive;

	public static event Action<string> OnAimbotKeyToggled;

	public static event Action OnToggleMenuVisibility;

	public static event Action<bool> OnFastLandingChanged;

	[DllImport("user32.dll")]
	private static extern short GetAsyncKeyState(int vKey);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool CloseHandle(IntPtr hObject);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern IntPtr GetModuleHandle(string lpModuleName);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, uint flAllocationType, uint flProtect);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out IntPtr lpNumberOfBytesWritten);

	[DllImport("kernel32.dll")]
	private static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttribute, IntPtr dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, uint dwFreeType);

	public static void StartAimbotKeyWatcher()
	{
		lock (_aimbotLock)
		{
			if (!_aimbotWatcherRunning)
			{
				_aimbotWatcherRunning = true;
				AimbotKeyWatcher();
			}
		}
	}

	private static async Task AimbotKeyWatcher()
	{
		while (_aimbotWatcherRunning)
		{
			await Task.Delay(20);
			if (_aimbotKeyCooldown)
			{
				continue;
			}
			int aimbotHeadKey = AimbotHeadKey;
			int aimbotV2Key = AimbotV2Key;
			int aimbotMianKey = AimbotMianKey;
			int aimbotNewKey = AimbotNewKey;
			if (aimbotHeadKey > 0 && (GetAsyncKeyState(aimbotHeadKey) & 0x8000) != 0)
			{
				_aimbotKeyCooldown = true;
				try
				{
					OnAimbotKeyToggled?.Invoke("Head");
				}
				catch
				{
				}
				await Task.Delay(350);
				_aimbotKeyCooldown = false;
			}
			else if (aimbotV2Key > 0 && (GetAsyncKeyState(aimbotV2Key) & 0x8000) != 0)
			{
				_aimbotKeyCooldown = true;
				try
				{
					OnAimbotKeyToggled?.Invoke("V2");
				}
				catch
				{
				}
				await Task.Delay(350);
				_aimbotKeyCooldown = false;
			}
			else if (aimbotMianKey > 0 && (GetAsyncKeyState(aimbotMianKey) & 0x8000) != 0)
			{
				_aimbotKeyCooldown = true;
				try
				{
					OnAimbotKeyToggled?.Invoke("Mian");
				}
				catch
				{
				}
				await Task.Delay(350);
				_aimbotKeyCooldown = false;
			}
			else if (aimbotNewKey > 0 && (GetAsyncKeyState(aimbotNewKey) & 0x8000) != 0)
			{
				_aimbotKeyCooldown = true;
				try
				{
					OnAimbotKeyToggled?.Invoke("New");
				}
				catch
				{
				}
				await Task.Delay(350);
				_aimbotKeyCooldown = false;
			}
			else if ((GetAsyncKeyState(116) & 0x8000) != 0)
			{
				_aimbotKeyCooldown = true;
				try
				{
					EspManager.RefreshEsp();
				}
				catch
				{
				}
				try
				{
					FastReassertAimbot();
				}
				catch
				{
				}
				await Task.Delay(350);
				_aimbotKeyCooldown = false;
			}
			else if (HideMenuKey > 0 && (GetAsyncKeyState(HideMenuKey) & 0x8000) != 0)
			{
				_aimbotKeyCooldown = true;
				try
				{
					OnToggleMenuVisibility?.Invoke();
				}
				catch
				{
				}
				await Task.Delay(350);
				_aimbotKeyCooldown = false;
			}
		}
	}

	public static void FastReassertAimbot()
	{
		var snapB = _listBeta;
		var snapA = _listAlpha;
		var snapG = _listGamma;

		bool hasB = snapB != null && snapB.Length > 0;
		bool hasA = snapA != null && snapA.Length > 0;
		bool hasG = snapG != null && snapG.Length > 0;

		if (hasB)
			foreach (var w in snapB)
				try { if (_memBeta.ReadInt(w.addr) != w.onVal) _memBeta.WriteInt(w.addr, w.onVal); } catch { }

		if (hasA)
			foreach (var w in snapA)
				try { if (_memAlpha.ReadInt(w.addr) != w.onVal) _memAlpha.WriteInt(w.addr, w.onVal); } catch { }

		if (hasG)
			foreach (var w in snapG)
				try { if (_memGamma.ReadInt(w.addr) != w.onVal) _memGamma.WriteInt(w.addr, w.onVal); } catch { }

		if (!hasB && !hasA && !hasG && _currentMode != AimbotActiveMode.None)
		{
			if (_currentMode == AimbotActiveMode.Mian100) EnableAimbotMian();
			else if (_currentMode == AimbotActiveMode.V2) EnableAimbotBeta();
			else if (_currentMode == AimbotActiveMode.Head) EnableAimbotAlpha();
			else if (_currentMode == AimbotActiveMode.New) EnableAimbotGamma();
		}
	}

	public static void StartAimbotHeartbeatWatcher()
	{
		lock (_aimbotLock)
		{
			if (!_heartbeatRunning)
			{
				_heartbeatRunning = true;
				Task.Run((Func<Task>)AimbotHeartbeatLoop);
			}
		}
	}

	public static void StopAimbotHeartbeatWatcher()
	{
		lock (_aimbotLock)
		{
			_heartbeatRunning = false;
		}
	}

	private static async Task AimbotHeartbeatLoop()
	{
		int staleCycleCount = 0;
		int zombieCycleCount = 0;
		while (_heartbeatRunning)
		{
			await Task.Delay(1000).ConfigureAwait(continueOnCapturedContext: false);
			if (!_heartbeatRunning)
			{
				break;
			}
			if (!_memAlpha.IsOpen && !_memBeta.IsOpen)
			{
				_heartbeatRunning = false;
				break;
			}
			var listA = _listAlpha;
			var listB = _listBeta;
			var listG = _listGamma;
			bool flag = listA != null && listA.Length != 0;
			bool flag2 = listB != null && listB.Length != 0;
			bool flag3 = listG != null && listG.Length != 0;
			if (!flag && !flag2 && !flag3)
			{
				_heartbeatRunning = false;
				break;
			}
			try
			{
				int valid = 0;
				int zombie = 0;
				int total = (flag2 ? listB.Length : 0) + (flag ? listA.Length : 0) + (flag3 ? listG.Length : 0);
				if (flag2)
				{
					foreach (var tuple in listB)
					{
						if (tuple.addr > 4096)
						{
							int num4 = _memBeta.ReadInt(tuple.addr);
							if (num4 == tuple.onVal || num4 == tuple.offVal)
							{
								valid++;
							}
							else
							{
								zombie++;
							}
						}
					}
				}
				if (flag)
				{
					foreach (var tuple2 in listA)
					{
						if (tuple2.addr > 4096)
						{
							int num5 = _memAlpha.ReadInt(tuple2.addr);
							if (num5 == tuple2.onVal || num5 == tuple2.offVal)
							{
								valid++;
							}
							else
							{
								zombie++;
							}
						}
					}
				}
				if (flag3)
				{
					foreach (var tuple3 in listG)
					{
						if (tuple3.addr > 4096)
						{
							int num6 = _memGamma.ReadInt(tuple3.addr);
							if (num6 == tuple3.onVal || num6 == tuple3.offVal)
							{
								valid++;
							}
							else
							{
								zombie++;
							}
						}
					}
				}
				if (total > 0 && zombie > total / 2)
				{
					zombieCycleCount++;
					if (zombieCycleCount >= 3)
					{
						RestoreAimbotMianValues();
						break;
					}
				}
				else
				{
					zombieCycleCount = 0;
				}
				if (valid == 0)
				{
					staleCycleCount++;
					if (staleCycleCount >= 15)
					{
						RestoreAimbotMianValues();
						break;
					}
				}
				else
				{
					staleCycleCount = 0;
				}
			}
			catch
			{
			}
		}
	}

	#region Aimbot Head (Alpha Engine)

	public static void RestoreMemoryValues()
	{
		StopAimbotHeartbeatWatcher();
		var snap = _listAlpha;
		lock (_aimbotLock) { _listAlpha = new (long, int, int, long)[0]; }
		if (snap != null)
			foreach (var w in snap)
				try { if (_memAlpha.ReadInt(w.addr) == w.onVal) _memAlpha.WriteInt(w.addr, w.offVal); } catch { }
	}

	public static void RestoreAlphaValues() => RestoreMemoryValues();

	public static async Task<string> ScanHeadAsync()
	{
		string emu = GetProcess.GetRunningEmulatorProcessName();
		if (!_memAlpha.SetProcess(new string[] { emu ?? "HD-Player" }))
			return "Emulator Not Found!! Open Emulator First";

		try
		{
			_memAlpha.ClearScanCache();
			List<long> hits = null;
			await Task.Run(() => { hits = _memAlpha.AoBScanSync(_aobAlpha); });

			if (hits == null || hits.Count == 0)
			{
				DiagnosticLogger.LogResult("Aimbot Head", _aobAlpha, false, 0,
					$"ReadOffset=0x{_rOffAlpha:X}, WriteOffset=0x{_wOffAlpha:X}",
					"AoBScan 0 matches");
				return "Pattern Not Found";
			}

			var raw = new List<(long addr, int onVal, int offVal, long baseAddr)>();
			await Task.Run(() =>
			{
				long readOff = (_selectedBone == TargetBone.Neck || _selectedBone == TargetBone.Chest) ? 0xBA
				             : (_selectedBone == TargetBone.Hip ? 0xBC : _rOffAlpha);

				foreach (long addr in hits)
				{
					long rAddr = addr + readOff;
					long wAddr = addr + _wOffAlpha;
					int vRead  = _memAlpha.ReadInt(rAddr);
					int vWrite = _memAlpha.ReadInt(wAddr);

					if (vRead != 0 && vWrite != 0 &&
						(uint)vRead  > 0x1000 && (uint)vWrite > 0x1000 &&
						vRead != vWrite)
					{
						raw.Add((wAddr, vRead, vWrite, addr));
					}
				}
			});

			if (raw.Count == 0)
			{
				DiagnosticLogger.LogResult("Aimbot Head", _aobAlpha, false, hits.Count,
					$"ReadOffset=0x{_rOffAlpha:X}, WriteOffset=0x{_wOffAlpha:X}",
					"Hits found but pointers invalid");
				return "Pattern Not Found";
			}

			var deduped = new List<(long addr, int onVal, int offVal, long baseAddr)>();
			var seen    = new List<long>();
			foreach (var e in raw)
			{
				bool dup = false;
				foreach (long s in seen)
					if (Math.Abs(e.baseAddr - s) < 4096) { dup = true; break; }
				if (!dup)
				{
					seen.Add(e.baseAddr);
					deduped.Add(e);
					if (deduped.Count >= MaxEntityCount) break;
				}
			}

			lock (_aimbotLock) { _listAlpha = deduped.ToArray(); }
			_memAlpha.ClearScanCache();

			DiagnosticLogger.LogResult("Aimbot Head", _aobAlpha, true, deduped.Count,
				$"ReadOffset=0x{_rOffAlpha:X}, WriteOffset=0x{_wOffAlpha:X} | Entities={deduped.Count}");
			return "Pattern Found";
		}
		catch (Exception ex)
		{
			DiagnosticLogger.LogResult("Aimbot Head", _aobAlpha, false, 0,
				$"ReadOffset=0x{_rOffAlpha:X}, WriteOffset=0x{_wOffAlpha:X}", ex.Message);
			return $"Scan Error: {ex.Message}";
		}
	}

	public static Task<string> ScanAlphaAsync() => ScanHeadAsync();

	public static async Task<string> EnableAimbotHead()
	{
		RestoreAimbotMianValues();
		string r = await ScanHeadAsync();
		if (r != "Pattern Found") return r;

		var snap = _listAlpha;
		await Task.Run(() =>
		{
			if (snap != null)
				foreach (var w in snap)
					try { _memAlpha.WriteInt(w.addr, w.onVal); } catch { }
		});

		_currentMode = AimbotActiveMode.Head;
		StartAimbotHeartbeatWatcher();
		SoundEffects.PlayEnableSound();
		return "Aimbot Head ON";
	}

	public static Task<string> EnableAimbotAlpha() => EnableAimbotHead();

	#endregion

	#region Aimbot Beta  // was: Aimbot V2

	public static void RestoreBetaValues()
	{
		StopAimbotHeartbeatWatcher();
		var snap = _listBeta;
		lock (_aimbotLock) { _listBeta = new (long, int, int, long)[0]; }
		if (snap != null)
			foreach (var w in snap)
				try { if (_memBeta.ReadInt(w.addr) == w.onVal) _memBeta.WriteInt(w.addr, w.offVal); } catch { }
	}

	public static async Task<string> ScanBetaAsync()
	{
		string emu = GetProcess.GetRunningEmulatorProcessName();
		if (!_memBeta.SetProcess(new string[] { emu ?? "HD-Player" }))
			return "Emulator Not Found!! Open Emulator First";

		try
		{
			_memBeta.ClearScanCache();
			List<long> hits = null;
			await Task.Run(() => { hits = _memBeta.AoBScanSync(_aobBeta); });

			if (hits == null || hits.Count == 0)
			{
				DiagnosticLogger.LogResult("Aimbot Beta", _aobBeta, false, 0,
					$"R1=0x{_rOff1Beta:X} W1=0x{_wOff1Beta:X} R2=0x{_rOff2Beta:X} W2=0x{_wOff2Beta:X}",
					"AoBScan 0 matches");
				return "Pattern Not Found";
			}

			_betaBaseAddresses = hits;

			var deduped   = new List<(long addr, int onVal, int offVal, long baseAddr)>();
			var seenBases = new List<long>();

			await Task.Run(() =>
			{
				foreach (long addr in hits)
				{
					bool dup = false;
					foreach (long s in seenBases)
						if (Math.Abs(addr - s) < 4096) { dup = true; break; }
					if (dup) continue;

					int vR1 = _memBeta.ReadInt(addr + _rOff1Beta);
					int vW1 = _memBeta.ReadInt(addr + _wOff1Beta);
					int vR2 = _memBeta.ReadInt(addr + _rOff2Beta);
					int vW2 = _memBeta.ReadInt(addr + _wOff2Beta);

					bool p1Valid = (vR1 != 0 && vW1 != 0 && (uint)vR1 > 0x1000 && (uint)vW1 > 0x1000 && vR1 != vW1);
					bool p2Valid = (vR2 != 0 && vW2 != 0 && (uint)vR2 > 0x1000 && (uint)vW2 > 0x1000 && vR2 != vW2);

					if (p1Valid || p2Valid)
					{
						seenBases.Add(addr);
						if (p1Valid) deduped.Add((addr + _wOff1Beta, vR1, vW1, addr));
						if (p2Valid) deduped.Add((addr + _wOff2Beta, vR2, vW2, addr));
						if (seenBases.Count >= MaxEntityCount) break;
					}
				}
			});

			if (deduped.Count == 0)
			{
				DiagnosticLogger.LogResult("Aimbot Beta", _aobBeta, false, hits.Count,
					$"R1=0x{_rOff1Beta:X} W1=0x{_wOff1Beta:X} R2=0x{_rOff2Beta:X} W2=0x{_wOff2Beta:X}",
					"Hits found but pointers invalid");
				return "Pattern Not Found";
			}

			lock (_aimbotLock) { _listBeta = deduped.ToArray(); }
			_memBeta.ClearScanCache();

			DiagnosticLogger.LogResult("Aimbot Beta", _aobBeta, true, deduped.Count,
				$"R1=0x{_rOff1Beta:X} W1=0x{_wOff1Beta:X} R2=0x{_rOff2Beta:X} W2=0x{_wOff2Beta:X} | Entities={seenBases.Count} Writes={deduped.Count}");
			return "Pattern Found";
		}
		catch (Exception ex)
		{
			DiagnosticLogger.LogResult("Aimbot Beta", _aobBeta, false, 0,
				$"R1=0x{_rOff1Beta:X} W1=0x{_wOff1Beta:X} R2=0x{_rOff2Beta:X} W2=0x{_wOff2Beta:X}", ex.Message);
			return $"Scan Error: {ex.Message}";
		}
	}

	public static async Task<string> EnableAimbotV2()
	{
		RestoreAimbotMianValues();
		string r = await ScanBetaAsync();
		if (r != "Pattern Found") return r;

		var snap = _listBeta;
		await Task.Run(() =>
		{
			if (snap != null)
				foreach (var w in snap)
					try { _memBeta.WriteInt(w.addr, w.onVal); } catch { }
		});

		_currentMode = AimbotActiveMode.V2;
		StartAimbotHeartbeatWatcher();
		SoundEffects.PlayEnableSound();
		return "Aimbot V2 ON";
	}

	// ── Compatibility shim ──
	public static Task<string> EnableAimbotBeta() => EnableAimbotV2();
	public static void RestoreAimbotV2Values() => RestoreBetaValues();

	#endregion

	#region Aimbot Gamma  // was: Aimbot New

	public static void RestoreGammaValues()
	{
		StopAimbotHeartbeatWatcher();
		var snap = _listGamma;
		lock (_aimbotLock) { _listGamma = new (long, int, int, long)[0]; }
		if (snap != null)
			foreach (var w in snap)
				try { if (_memGamma.ReadInt(w.addr) == w.onVal) _memGamma.WriteInt(w.addr, w.offVal); } catch { }
	}

	public static async Task<string> ScanGammaAsync()
	{
		string emu = GetProcess.GetRunningEmulatorProcessName();
		if (!_memGamma.SetProcess(new string[] { emu ?? "HD-Player" }))
			return "Emulator Not Found!! Open Emulator First";

		try
		{
			_memGamma.ClearScanCache();
			List<long> hits = null;
			await Task.Run(() => { hits = _memGamma.AoBScanSync(_aobGamma); });

			if (hits == null || hits.Count == 0)
			{
				DiagnosticLogger.LogResult("Aimbot Gamma", _aobGamma, false, 0,
					$"ReadOffset=0x{_rOffGamma:X}, WriteOffset=0x{_wOffGamma:X}",
					"AoBScan 0 matches");
				return "Pattern Not Found";
			}

			var raw = new List<(long addr, int onVal, int offVal, long baseAddr)>();
			await Task.Run(() =>
			{
				long readOff = (_selectedBone == TargetBone.Neck || _selectedBone == TargetBone.Chest) ? 0xBA
				             : (_selectedBone == TargetBone.Hip ? 0xBC : _rOffGamma);

				foreach (long addr in hits)
				{
					long rAddr = addr + readOff;
					long wAddr = addr + _wOffGamma;
					int vR = _memGamma.ReadInt(rAddr);
					int vW = _memGamma.ReadInt(wAddr);

					if (vR != 0 && vW != 0 && (uint)vR > 0x1000 && (uint)vW > 0x1000 && vR != vW)
						raw.Add((wAddr, vR, vW, addr));
				}
			});

			if (raw.Count == 0)
			{
				DiagnosticLogger.LogResult("Aimbot Gamma", _aobGamma, false, hits.Count,
					$"ReadOffset=0x{_rOffGamma:X}, WriteOffset=0x{_wOffGamma:X}",
					"Hits found but pointers invalid");
				return "Pattern Not Found";
			}

			var deduped = new List<(long addr, int onVal, int offVal, long baseAddr)>();
			var seen    = new List<long>();
			foreach (var e in raw)
			{
				bool dup = false;
				foreach (long s in seen)
					if (Math.Abs(e.baseAddr - s) < 0x1000) { dup = true; break; }
				if (!dup)
				{
					seen.Add(e.baseAddr);
					deduped.Add(e);
					if (deduped.Count >= MaxEntityCount) break;
				}
			}

			lock (_aimbotLock) { _listGamma = deduped.ToArray(); }
			_memGamma.ClearScanCache();

			DiagnosticLogger.LogResult("Aimbot Gamma", _aobGamma, true, deduped.Count,
				$"ReadOffset=0x{_rOffGamma:X}, WriteOffset=0x{_wOffGamma:X} | Raw={raw.Count} Deduped={deduped.Count}");
			return "Pattern Found";
		}
		catch (Exception ex)
		{
			DiagnosticLogger.LogResult("Aimbot Gamma", _aobGamma, false, 0,
				$"ReadOffset=0x{_rOffGamma:X}, WriteOffset=0x{_wOffGamma:X}", ex.Message);
			return $"Scan Error: {ex.Message}";
		}
	}

	public static async Task<string> EnableAimbotNew()
	{
		RestoreAimbotMianValues();
		string r = await ScanGammaAsync();
		if (r != "Pattern Found") return r;

		var snap = _listGamma;
		await Task.Run(() =>
		{
			if (snap != null)
				foreach (var w in snap)
					try { _memGamma.WriteInt(w.addr, w.onVal); } catch { }
		});

		_currentMode = AimbotActiveMode.New;
		StartAimbotHeartbeatWatcher();
		SoundEffects.PlayEnableSound();
		return "Aimbot New ON";
	}

	// ── Compatibility shim ──
	public static Task<string> EnableAimbotGamma() => EnableAimbotNew();
	public static void RestoreAimbotNewValues() => RestoreGammaValues();

	#endregion

	#region Aimbot Storm  // was: Aimbot 100%

	public static void RestoreAimbotMianValues()
	{
		_currentMode = AimbotActiveMode.None;
		StopAimbotHeartbeatWatcher();

		var snapA = _listAlpha;
		var snapB = _listBeta;
		var snapG = _listGamma;

		lock (_aimbotLock)
		{
			_listAlpha = new (long, int, int, long)[0];
			_listBeta  = new (long, int, int, long)[0];
			_listGamma = new (long, int, int, long)[0];
			_betaBaseAddresses.Clear();
		}

		if (snapA != null)
			foreach (var w in snapA)
				try { if (_memAlpha.ReadInt(w.addr) == w.onVal) _memAlpha.WriteInt(w.addr, w.offVal); } catch { }

		if (snapB != null)
			foreach (var w in snapB)
				try { if (_memBeta.ReadInt(w.addr) == w.onVal) _memBeta.WriteInt(w.addr, w.offVal); } catch { }

		if (snapG != null)
			foreach (var w in snapG)
				try { if (_memGamma.ReadInt(w.addr) == w.onVal) _memGamma.WriteInt(w.addr, w.offVal); } catch { }
	}

	public static async Task<string> EnableAimbotMian()
	{
		if (_isEnablingMian) return "Scanning...";
		_isEnablingMian = true;

		try
		{
			if (_listAlpha.Length > 0 || _listBeta.Length > 0)
				RestoreAimbotMianValues();

			var taskAlpha = ScanAlphaAsync();
			var taskBeta  = ScanBetaAsync();
			await Task.WhenAll(taskAlpha, taskBeta);

			var snapA = _listAlpha;
			var snapB = _listBeta;

			if ((snapA == null || snapA.Length == 0) && (snapB == null || snapB.Length == 0))
			{
				RestoreAimbotMianValues();
				return "Lobby/Plane: Land First!";
			}

			await Task.Run(() =>
			{
				if (snapB != null && snapB.Length > 0)
					foreach (var w in snapB)
						try { _memBeta.WriteInt(w.addr, w.onVal); } catch { }

				var cleanAlpha = new List<(long addr, int onVal, int offVal, long baseAddr)>();
				if (snapA != null && snapA.Length > 0)
				{
					var betaBases = _betaBaseAddresses;
					var betaAddrs = snapB != null ? System.Linq.Enumerable.Select(snapB, v => v.addr).ToArray() : new long[0];

					foreach (var w in snapA)
					{
						bool overlap = false;

						if (betaBases != null)
							foreach (long bb in betaBases)
								if (Math.Abs(w.baseAddr - bb) < 4096) { overlap = true; break; }

						if (!overlap)
							for (int i = 0; i < betaAddrs.Length; i++)
								if (Math.Abs(w.addr - betaAddrs[i]) < 4096) { overlap = true; break; }

						if (!overlap)
						{
							try { _memAlpha.WriteInt(w.addr, w.onVal); cleanAlpha.Add(w); } catch { }
						}
					}
				}

				lock (_aimbotLock) { _listAlpha = cleanAlpha.ToArray(); }
			});

			_currentMode = AimbotActiveMode.Mian100;
		StartAimbotHeartbeatWatcher();
		SoundEffects.PlayEnableSound();
		return "Aimbot 100% ON";
		}
		finally
		{
			_isEnablingMian = false;
		}
	}

	#endregion

	public static async Task<string> ScanSniperScopeAsync()
	{
		string runningEmulatorProcessName = GetProcess.GetRunningEmulatorProcessName();
		if (!_scopeMem.SetProcess(new string[1] { runningEmulatorProcessName ?? "HD-Player" }))
		{
			return "Emulator Not Found";
		}
		_sniperScope1 = await _scopeMem.AoBScan(ScopeAob);
		if (_sniperScope1 == null || !_sniperScope1.Any())
		{
			return "No Pattern Found";
		}
		return "Sniper Scope Ready";
	}

	public static async Task<string> EnableSniperScope()
	{
		if (!_sniperScope1.Any())
		{
			string text = await ScanSniperScopeAsync();
			if (text != "Sniper Scope Ready")
			{
				return text;
			}
		}
		SoundEffects.PlayEnableSound();
		if (!_scopeWatcherRunning)
		{
			_scopeWatcherRunning = true;
			SniperAiWatching();
		}
		return "Sniper Scope ON";
	}

	public static void StopSniperScope()
	{
		_scopeWatcherRunning = false;
		RestoreSniperScope();
	}

	private static async Task SniperAiWatching()
	{
		while (_scopeWatcherRunning)
		{
			await Task.Delay(20);
			if (((GetAsyncKeyState(5) & 0x8000) != 0 || (GetAsyncKeyState(6) & 0x8000) != 0 || (GetAsyncKeyState(86) & 0x8000) != 0 || (GetAsyncKeyState(1) & 0x8000) != 0 || (GetAsyncKeyState(84) & 0x8000) != 0 || (GetAsyncKeyState(69) & 0x8000) != 0 || (GetAsyncKeyState(81) & 0x8000) != 0 || (GetAsyncKeyState(4) & 0x8000) != 0) && !_scopeCooldown)
			{
				_scopeCooldown = true;
				SniperBrilerControl();
				await Task.Delay(300);
				_scopeCooldown = false;
			}
		}
	}

	private static async Task SniperBrilerControl()
	{
		try
		{
			ReapplySniperScope();
			await Task.Delay(48);
			RestoreSniperScope();
		}
		catch
		{
		}
	}

	private static void ReapplySniperScope()
	{
		if (_sniperScope1 == null)
		{
			return;
		}
		foreach (long item in _sniperScope1)
		{
			_scopeMem.AobReplace(item, ScopeReplace);
		}
	}

	private static void RestoreSniperScope()
	{
		if (_sniperScope1 == null)
		{
			return;
		}
		foreach (long item in _sniperScope1)
		{
			_scopeMem.AobReplace(item, ScopeAob);
		}
	}

	public static async Task<string> ScanSniperSwitchAsync()
	{
		string runningEmulatorProcessName = GetProcess.GetRunningEmulatorProcessName();
		if (!_switchMem.SetProcess(new string[1] { runningEmulatorProcessName ?? "HD-Player" }))
		{
			return "Emulator Not Found";
		}
		IEnumerable<long> enumerable = await _switchMem.AoBScan(SniperSwitchAob);
		if (enumerable == null || !enumerable.Any())
		{
			return "No Pattern Found";
		}
		_sniperSwitchCache = enumerable.ToList();
		return "Sniper Switch Ready";
	}

	public static async Task<string> EnableSniperSwitch()
	{
		if (_sniperSwitchCache.Count == 0)
		{
			string text = await ScanSniperSwitchAsync();
			if (text != "Sniper Switch Ready")
			{
				return text;
			}
		}
		await Task.Run(() =>
		{
			foreach (long item in _sniperSwitchCache)
			{
				try
				{
					_switchMem.AobReplace(item, SniperSwitchReplace);
				}
				catch
				{
				}
			}
		});
		SoundEffects.PlayEnableSound();
		return "Sniper Switch ON";
	}

	public static void DisableSniperSwitch()
	{
		if (_sniperSwitchCache == null || _sniperSwitchCache.Count <= 0)
		{
			return;
		}
		foreach (long item in _sniperSwitchCache)
		{
			try
			{
				_switchMem.AobReplace(item, SniperSwitchAob);
			}
			catch
			{
			}
		}
		_sniperSwitchCache.Clear();
	}

	public static async Task<string> EnableChams()
	{
		if (_chamsInjected)
		{
			return "Chams Injected";
		}
		try
		{
			string runningEmulatorProcessName = GetProcess.GetRunningEmulatorProcessName();
			if (runningEmulatorProcessName == null)
			{
				return "Emulator Not Running";
			}
			string text = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N").Substring(0, 12) + ".dll");
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string text2 = executingAssembly.GetManifestResourceNames().FirstOrDefault((string n) => n.EndsWith("MIAN XITERS.DLL", StringComparison.OrdinalIgnoreCase) || n.EndsWith("MIAN_XITERS.DLL", StringComparison.OrdinalIgnoreCase) || (n.IndexOf("MIAN", StringComparison.OrdinalIgnoreCase) >= 0 && n.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) && !n.EndsWith("AuthlyX.dll", StringComparison.OrdinalIgnoreCase)));
			if (text2 == null)
			{
				return "Resource Not Found";
			}
			using (Stream stream = executingAssembly.GetManifestResourceStream(text2))
			{
				using FileStream fileStream = new FileStream(text, FileMode.Create);
				byte[] array = new byte[stream.Length];
				stream.Read(array, 0, array.Length);
				fileStream.Write(array, 0, array.Length);
			}
			string text3 = await InjectDll(runningEmulatorProcessName, text);
			try { File.Delete(text); } catch { }
			if (text3 == "OK")
			{
				_chamsInjected = true;
				SoundEffects.PlayEnableSound();
				return "Chams Injected";
			}
			return text3;
		}
		catch (Exception ex)
		{
			return "Chams Error: " + ex.Message;
		}
	}

	private static async Task<string> InjectDll(string processName, string dllPath)
	{
		return await Task.Run(() =>
		{
			int item = GetProcess.GetRunningEmulatorProcess().pid;
			if (item <= 0)
			{
				return "Emulator Not Running";
			}
			IntPtr intPtr = OpenProcess(1082u, bInheritHandle: false, item);
			if (intPtr == IntPtr.Zero)
			{
				return "OpenProcess Failed";
			}
			IntPtr intPtr2 = IntPtr.Zero;
			try
			{
				IntPtr procAddress = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryW");
				byte[] bytes = Encoding.Unicode.GetBytes(dllPath + "\0");
				intPtr2 = VirtualAllocEx(intPtr, IntPtr.Zero, (IntPtr)bytes.Length, 12288u, 4u);
				WriteProcessMemory(intPtr, intPtr2, bytes, (uint)bytes.Length, out var _);
				IntPtr intPtr3 = CreateRemoteThread(intPtr, IntPtr.Zero, IntPtr.Zero, procAddress, intPtr2, 0u, IntPtr.Zero);
				if (intPtr3 == IntPtr.Zero)
				{
					return "Inject Failed";
				}
				WaitForSingleObject(intPtr3, 5000u);
				CloseHandle(intPtr3);
				return "OK";
			}
			finally
			{
				if (intPtr2 != IntPtr.Zero)
				{
					VirtualFreeEx(intPtr, intPtr2, IntPtr.Zero, 32768u);
				}
				CloseHandle(intPtr);
			}
		});
	}

	public static void ResetChamsState()
	{
		_chamsInjected = false;
	}

	public static async Task<string> ScanGlitchFireAsync()
	{
		string runningEmulatorProcessName = GetProcess.GetRunningEmulatorProcessName();
		if (!_glitchFireMem.SetProcess(new string[1] { runningEmulatorProcessName ?? "HD-Player" }))
		{
			return "Emulator Not Found";
		}
		IEnumerable<long> enumerable = await _glitchFireMem.AoBScan(GlitchFireAob);
		if (enumerable == null || !enumerable.Any())
		{
			IEnumerable<long> enumerable2 = await _glitchFireMem.AoBScan(GlitchFireReplace);
			if (enumerable2 == null || !enumerable2.Any())
			{
				return "No Pattern Found";
			}
			_glitchFireAddresses = enumerable2.ToList();
		}
		else
		{
			_glitchFireAddresses = enumerable.ToList();
		}
		return "Glitch Fire Ready";
	}

	public static async Task<string> EnableGlitchFire()
	{
		if (_glitchFireAddresses.Count == 0)
		{
			string text = await ScanGlitchFireAsync();
			if (text != "Glitch Fire Ready")
			{
				return text;
			}
		}
		await Task.Run(() =>
		{
			foreach (long glitchFireAddress in _glitchFireAddresses)
			{
				try
				{
					_glitchFireMem.AobReplace(glitchFireAddress, GlitchFireReplace);
				}
				catch
				{
				}
			}
		});
		return "Glitch Fire ON";
	}

	public static void DisableGlitchFire()
	{
		if (_glitchFireAddresses != null && _glitchFireAddresses.Count > 0)
		{
			foreach (long glitchFireAddress in _glitchFireAddresses)
			{
				try
				{
					_glitchFireMem.AobReplace(glitchFireAddress, GlitchFireAob);
				}
				catch
				{
				}
			}
		}
		_glitchFireAddresses.Clear();
	}

	public static void StartFastLandingWatcher()
	{
		if (!_fastLandingWatcherRunning)
		{
			_fastLandingWatcherRunning = true;
			FastLandingKeyWatcher();
		}
	}

	public static async Task<string> ScanFastLandingAsync()
	{
		string runningEmulatorProcessName = GetProcess.GetRunningEmulatorProcessName();
		if (!_fastLandingMem.SetProcess(new string[1] { runningEmulatorProcessName ?? "HD-Player" }))
		{
			return "Emulator Not Found";
		}
		IEnumerable<long> enumerable = await _fastLandingMem.AoBScan(FastLandingAob);
		if (enumerable == null || !enumerable.Any())
		{
			IEnumerable<long> enumerable2 = await _fastLandingMem.AoBScan(FastLandingReplace);
			if (enumerable2 == null || !enumerable2.Any())
			{
				return "No Pattern Found";
			}
			_fastLandingAddresses = enumerable2.ToList();
		}
		else
		{
			_fastLandingAddresses = enumerable.ToList();
		}
		return "Fast Landing Ready";
	}

	public static async Task<string> EnableFastLanding()
	{
		if (_fastLandingAddresses.Count == 0)
		{
			string text = await ScanFastLandingAsync();
			if (text != "Fast Landing Ready")
			{
				return text;
			}
		}
		ApplyFastLanding(enable: true);
		StartFastLandingWatcher();
		SoundEffects.PlayEnableSound();
		return "Fast Landing ON";
	}

	public static void DisableFastLanding()
	{
		ApplyFastLanding(enable: false);
		_fastLandingAddresses.Clear();
	}

	public static void ApplyFastLanding(bool enable)
	{
		if (_fastLandingAddresses == null || _fastLandingAddresses.Count == 0)
		{
			return;
		}
		string bytePattern = (enable ? FastLandingReplace : FastLandingAob);
		foreach (long fastLandingAddress in _fastLandingAddresses)
		{
			try
			{
				_fastLandingMem.AobReplace(fastLandingAddress, bytePattern);
			}
			catch
			{
			}
		}
		_fastLandingActive = enable;
	}

	private static async Task FastLandingKeyWatcher()
	{
		while (_fastLandingWatcherRunning)
		{
			await Task.Delay(15);
			if (FastLandingKey <= 0 || (GetAsyncKeyState(FastLandingKey) & 0x8000) == 0 || _fastLandingCooldown)
			{
				continue;
			}
			_fastLandingCooldown = true;
			if (_fastLandingAddresses.Count == 0 && await ScanFastLandingAsync() != "Fast Landing Ready")
			{
				await Task.Delay(300);
				_fastLandingCooldown = false;
				continue;
			}
			_fastLandingActive = !_fastLandingActive;
			ApplyFastLanding(_fastLandingActive);
			if (_fastLandingActive)
			{
				SoundEffects.PlayEnableSound();
			}
			else
			{
				SoundEffects.PlayDisableSound();
			}
			OnFastLandingChanged?.Invoke(_fastLandingActive);
			await Task.Delay(300);
			_fastLandingCooldown = false;
		}
	}

	public static async Task<string> ScanCameraViewAsync()
	{
		string runningEmulatorProcessName = GetProcess.GetRunningEmulatorProcessName();
		if (!_cameraViewMem.SetProcess(new string[1] { runningEmulatorProcessName ?? "HD-Player" }))
		{
			return "Emulator Not Found";
		}
		IEnumerable<long> enumerable = await _cameraViewMem.AoBScan(CameraViewAob);
		if (enumerable == null || !enumerable.Any())
		{
			IEnumerable<long> enumerable2 = await _cameraViewMem.AoBScan(CameraViewReplace);
			if (enumerable2 == null || !enumerable2.Any())
			{
				return "No Pattern Found";
			}
			_cameraViewAddresses = enumerable2.ToList();
		}
		else
		{
			_cameraViewAddresses = enumerable.ToList();
		}
		return "Camera View Ready";
	}

	public static async Task<string> EnableCameraView()
	{
		if (_cameraViewAddresses.Count == 0)
		{
			string text = await ScanCameraViewAsync();
			if (text != "Camera View Ready")
			{
				return text;
			}
		}
		ApplyCameraView(enable: true);
		if (!_cameraViewWatcherRunning)
		{
			_cameraViewWatcherRunning = true;
			CameraViewKeyWatcher();
		}
		return "Camera View ON";
	}

	public static void DisableCameraView()
	{
		_cameraViewWatcherRunning = false;
		ApplyCameraView(enable: false);
		_cameraViewAddresses.Clear();
	}

	public static void ApplyCameraView(bool enable)
	{
		if (_cameraViewAddresses == null || _cameraViewAddresses.Count == 0)
		{
			return;
		}
		string bytePattern = (enable ? CameraViewReplace : CameraViewAob);
		foreach (long cameraViewAddress in _cameraViewAddresses)
		{
			try
			{
				_cameraViewMem.AobReplace(cameraViewAddress, bytePattern);
			}
			catch
			{
			}
		}
		_cameraViewActive = enable;
	}

	private static async Task CameraViewKeyWatcher()
	{
		while (_cameraViewWatcherRunning)
		{
			await Task.Delay(15);
			if ((GetAsyncKeyState(20) & 0x8000) != 0 && !_cameraViewCooldown)
			{
				_cameraViewCooldown = true;
				_cameraViewActive = !_cameraViewActive;
				ApplyCameraView(_cameraViewActive);
				await Task.Delay(250);
				_cameraViewCooldown = false;
			}
		}
	}

	public static async Task<string> ScanResetGuestAsync()
	{
		string runningEmulatorProcessName = GetProcess.GetRunningEmulatorProcessName();
		if (!_guestResetMem.SetProcess(new string[1] { runningEmulatorProcessName ?? "HD-Player" }))
		{
			return "Emulator Not Found";
		}
		IEnumerable<long> enumerable = await _guestResetMem.AoBScan(GuestResetAob);
		if (enumerable == null || !enumerable.Any())
		{
			IEnumerable<long> enumerable2 = await _guestResetMem.AoBScan(GuestResetReplace);
			if (enumerable2 == null || !enumerable2.Any())
			{
				return "No Pattern Found";
			}
			_guestResetAddresses = enumerable2.ToList();
		}
		else
		{
			_guestResetAddresses = enumerable.ToList();
		}
		return "Guest Reset Ready";
	}

	public static async Task<string> EnableResetGuest()
	{
		if (_guestResetAddresses.Count == 0)
		{
			string text = await ScanResetGuestAsync();
			if (text != "Guest Reset Ready")
			{
				return text;
			}
		}
		await Task.Run(() =>
		{
			foreach (long guestResetAddress in _guestResetAddresses)
			{
				try
				{
					_guestResetMem.AobReplace(guestResetAddress, GuestResetReplace);
				}
				catch
				{
				}
			}
		});
		SoundEffects.PlayEnableSound();
		return "Guest Reset ON";
	}

	public static void DisableResetGuest()
	{
		if (_guestResetAddresses != null && _guestResetAddresses.Count > 0)
		{
			foreach (long guestResetAddress in _guestResetAddresses)
			{
				try
				{
					_guestResetMem.AobReplace(guestResetAddress, GuestResetAob);
				}
				catch
				{
				}
			}
		}
		_guestResetAddresses.Clear();
	}

	public static async Task<string> ScanScopeTrackingAsync()
	{
		string runningEmulatorProcessName = GetProcess.GetRunningEmulatorProcessName();
		if (!_scopeTrackingMem.SetProcess(new string[1] { runningEmulatorProcessName ?? "HD-Player" }))
		{
			return "Emulator Not Found";
		}
		IEnumerable<long> enumerable = await _scopeTrackingMem.AoBScan(ScopeTrackingAob);
		if (enumerable == null || !enumerable.Any())
		{
			IEnumerable<long> enumerable2 = await _scopeTrackingMem.AoBScan(ScopeTrackingReplace);
			if (enumerable2 == null || !enumerable2.Any())
			{
				return "No Pattern Found";
			}
			_scopeTrackingAddresses = enumerable2.ToList();
		}
		else
		{
			_scopeTrackingAddresses = enumerable.ToList();
		}
		return "Scope Tracking Ready";
	}

	public static async Task<string> EnableScopeTracking()
	{
		if (_scopeTrackingAddresses.Count == 0)
		{
			string text = await ScanScopeTrackingAsync();
			if (text != "Scope Tracking Ready")
			{
				return text;
			}
		}
		await Task.Run(() =>
		{
			foreach (long scopeTrackingAddress in _scopeTrackingAddresses)
			{
				try
				{
					_scopeTrackingMem.AobReplace(scopeTrackingAddress, ScopeTrackingReplace);
				}
				catch
				{
				}
			}
		});
		SoundEffects.PlayEnableSound();
		return "Scope Tracking ON";
	}

	public static void DisableScopeTracking()
	{
		if (_scopeTrackingAddresses != null && _scopeTrackingAddresses.Count > 0)
		{
			foreach (long scopeTrackingAddress in _scopeTrackingAddresses)
			{
				try
				{
					_scopeTrackingMem.AobReplace(scopeTrackingAddress, ScopeTrackingAob);
				}
				catch
				{
				}
			}
		}
		_scopeTrackingAddresses.Clear();
	}

	public static void SetTargetBone(TargetBone bone)
	{
		_selectedBone = bone;
	}

	public static TargetBone GetTargetBone()
	{
		return _selectedBone;
	}

	public static long GetActiveBoneOffset()
	{
		return _selectedBone switch
		{
			TargetBone.Head => 1180L, 
			TargetBone.Neck => 1188L, 
			TargetBone.Chest => 1188L, 
			TargetBone.Hip => 1184L, 
			TargetBone.Feet => 1216L, 
			_ => 1180L, 
		};
	}
}
}
