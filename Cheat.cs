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

	private static string aimAob = "FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ?? ?? ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ?? ?? ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 00 00 A5 43";

	private static long ReadOffset = 184L;

	private static long WriteOffset = 180L;

	private static readonly string[] TaskName = new string[10] { "HD-Player", "Bluestacks", "Bluestacks-5", "HD-Agent", "HdPlayerService", "BstkSVC", "LdBoxHeadless", "dnplayer", "NoxVMHandle", "MEmuHeadless" };

	private const int MaxEntityCount = 20;

	private static mozih4x moziii = new mozih4x();

	private static (long addr, int onVal, int offVal, long baseAddr)[] _writeList = new (long, int, int, long)[0];

	private static List<long> _v2BaseAddresses = new List<long>();

	private static string aimNewAob = "FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 00 00 A5 43";

	private static long ReadOffsetNew = 184L;

	private static long WriteOffsetNew = 180L;

	private static readonly mozih4x _mozNew = new mozih4x();

	private static (long addr, int onVal, int offVal, long baseAddr)[] _writeListNew = new (long, int, int, long)[0];

	private static string aimV2Aob = "FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 00 00 A5 43";

	private static long ReadOffset1 = 186L;

	private static long ReadOffset2 = 188L;

	private static long WriteOffset1 = 182L;

	private static long WriteOffset2 = 184L;

	private static readonly mozih4x _mozV2 = new mozih4x();

	private static (long addr, int onVal, int offVal)[] _writeListV2 = new (long, int, int)[0];

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
		(long, int, int)[] writeListV = _writeListV2;
		(long, int, int, long)[] writeList = _writeList;
		(long, int, int, long)[] writeListNew = _writeListNew;
		bool flag = writeListV != null && writeListV.Length != 0;
		bool flag2 = writeList != null && writeList.Length != 0;
		bool flag3 = writeListNew != null && writeListNew.Length != 0;
		if (flag)
		{
			(long, int, int)[] array = writeListV;
			for (int i = 0; i < array.Length; i++)
			{
				(long, int, int) tuple = array[i];
				try
				{
					if (tuple.Item1 > 4096 && (uint)tuple.Item2 > 4096u)
					{
						_mozV2.WriteInt(tuple.Item1, tuple.Item2);
					}
				}
				catch
				{
				}
			}
		}
		if (flag2)
		{
			(long, int, int, long)[] array2 = writeList;
			for (int i = 0; i < array2.Length; i++)
			{
				(long, int, int, long) tuple2 = array2[i];
				try
				{
					if (tuple2.Item1 > 4096 && (uint)tuple2.Item2 > 4096u)
					{
						moziii.WriteInt(tuple2.Item1, tuple2.Item2);
					}
				}
				catch
				{
				}
			}
		}
		if (flag3)
		{
			(long, int, int, long)[] array2 = writeListNew;
			for (int i = 0; i < array2.Length; i++)
			{
				(long, int, int, long) tuple3 = array2[i];
				try
				{
					if (tuple3.Item1 > 4096 && (uint)tuple3.Item2 > 4096u)
					{
						_mozNew.WriteInt(tuple3.Item1, tuple3.Item2);
					}
				}
				catch
				{
				}
			}
		}
		if (!flag && !flag2 && !flag3 && _currentMode != AimbotActiveMode.None)
		{
			if (_currentMode == AimbotActiveMode.Mian100)
			{
				EnableAimbotMian();
			}
			else if (_currentMode == AimbotActiveMode.V2)
			{
				EnableAimbotV2();
			}
			else if (_currentMode == AimbotActiveMode.Head)
			{
				EnableAimbotHead();
			}
			else if (_currentMode == AimbotActiveMode.New)
			{
				EnableAimbotNew();
			}
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
			if (!moziii.IsOpen && !_mozV2.IsOpen)
			{
				_heartbeatRunning = false;
				break;
			}
			(long, int, int, long)[] writeList = _writeList;
			(long, int, int)[] writeListV = _writeListV2;
			(long, int, int, long)[] writeListNew = _writeListNew;
			bool flag = writeList != null && writeList.Length != 0;
			bool flag2 = writeListV != null && writeListV.Length != 0;
			bool flag3 = writeListNew != null && writeListNew.Length != 0;
			if (!flag && !flag2 && !flag3)
			{
				_heartbeatRunning = false;
				break;
			}
			try
			{
				int num = 0;
				int num2 = 0;
				int num3 = (flag2 ? writeListV.Length : 0) + (flag ? writeList.Length : 0) + (flag3 ? writeListNew.Length : 0);
				if (flag2)
				{
					(long, int, int)[] array = writeListV;
					for (int i = 0; i < array.Length; i++)
					{
						(long, int, int) tuple = array[i];
						if (tuple.Item1 > 4096)
						{
							int num4 = _mozV2.ReadInt(tuple.Item1);
							if (num4 == tuple.Item2)
							{
								num++;
							}
							else if (num4 == tuple.Item3)
							{
								num++;
							}
							else
							{
								num2++;
							}
						}
					}
				}
				if (flag)
				{
					(long, int, int, long)[] array2 = writeList;
					for (int i = 0; i < array2.Length; i++)
					{
						(long, int, int, long) tuple2 = array2[i];
						if (tuple2.Item1 > 4096)
						{
							int num5 = moziii.ReadInt(tuple2.Item1);
							if (num5 == tuple2.Item2)
							{
								num++;
							}
							else if (num5 == tuple2.Item3)
							{
								num++;
							}
							else
							{
								num2++;
							}
						}
					}
				}
				if (flag3)
				{
					(long, int, int, long)[] array2 = writeListNew;
					for (int i = 0; i < array2.Length; i++)
					{
						(long, int, int, long) tuple3 = array2[i];
						if (tuple3.Item1 > 4096)
						{
							int num6 = _mozNew.ReadInt(tuple3.Item1);
							if (num6 == tuple3.Item2)
							{
								num++;
							}
							else if (num6 == tuple3.Item3)
							{
								num++;
							}
							else
							{
								num2++;
							}
						}
					}
				}
				if (num3 > 0 && num2 > num3 / 2)
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
				if (num == 0)
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

	public static void RestoreMemoryValues()
	{
		StopAimbotHeartbeatWatcher();
		(long, int, int, long)[] writeList = _writeList;
		(long, int, int, long)[] writeListNew = _writeListNew;
		lock (_aimbotLock)
		{
			_writeList = new (long, int, int, long)[0];
			_writeListNew = new (long, int, int, long)[0];
		}
		(long, int, int, long)[] array;
		if (writeList != null && writeList.Length != 0)
		{
			array = writeList;
			for (int i = 0; i < array.Length; i++)
			{
				(long, int, int, long) tuple = array[i];
				try
				{
					if (moziii.ReadInt(tuple.Item1) == tuple.Item2)
					{
						moziii.WriteInt(tuple.Item1, tuple.Item3);
					}
				}
				catch
				{
				}
			}
		}
		if (writeListNew == null || writeListNew.Length == 0)
		{
			return;
		}
		array = writeListNew;
		for (int i = 0; i < array.Length; i++)
		{
			(long, int, int, long) tuple2 = array[i];
			try
			{
				if (_mozNew.ReadInt(tuple2.Item1) == tuple2.Item2)
				{
					_mozNew.WriteInt(tuple2.Item1, tuple2.Item3);
				}
			}
			catch
			{
			}
		}
	}

	public static async Task<string> ScanHeadAsync()
	{
		string runningEmulatorProcessName = GetProcess.GetRunningEmulatorProcessName();
		if (!moziii.SetProcess(new string[1] { runningEmulatorProcessName ?? "HD-Player" }))
		{
			return "Emulator Not Found!! Open Emulator First";
		}
		try
		{
			moziii.ClearScanCache();
			List<long> addresses = null;
			await Task.Run(() =>
			{
				addresses = moziii.AoBScanSync(aimAob);
			});
			if (addresses == null || addresses.Count == 0)
			{
				DiagnosticLogger.LogResult("Aimbot Head", aimAob, success: false, 0, $"ReadOffset=0x{ReadOffset:X}, WriteOffset=0x{WriteOffset:X}", "AoBScan found 0 matches in emulator memory");
				return "Pattern Not Found";
			}
			List<(long addr, int onVal, int offVal, long baseAddr)> list = new List<(long, int, int, long)>();
			await Task.Run(() =>
			{
				long num = ((_selectedBone == TargetBone.Neck || _selectedBone == TargetBone.Chest) ? 186 : ((_selectedBone == TargetBone.Hip) ? 188 : ReadOffset));
				foreach (long item in addresses)
				{
					long address = item + num;
					long num2 = item + WriteOffset;
					int num3 = moziii.ReadInt(address);
					int num4 = moziii.ReadInt(num2);
					if (num3 != 0 && num4 != 0 && (uint)num3 > 4096u && (uint)num4 > 4096u && num3 != num4)
					{
						list.Add((num2, num3, num4, item));
					}
				}
			});
			if (list.Count == 0)
			{
				DiagnosticLogger.LogResult("Aimbot Head", aimAob, success: false, addresses.Count, $"ReadOffset=0x{ReadOffset:X}, WriteOffset=0x{WriteOffset:X}", "Addresses found but entity pointers were invalid");
				return "Entities Not Loaded! Activate in Match / Training";
			}
			List<(long, int, int, long)> list2 = new List<(long, int, int, long)>();
			List<long> list3 = new List<long>();
			foreach (var item2 in list)
			{
				bool flag = false;
				foreach (long item3 in list3)
				{
					if (Math.Abs(item2.baseAddr - item3) < 4096)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					list3.Add(item2.baseAddr);
					list2.Add(item2);
					if (list2.Count >= 20)
					{
						break;
					}
				}
			}
			lock (_aimbotLock)
			{
				_writeList = list2.ToArray();
			}
			moziii.ClearScanCache();
			DiagnosticLogger.LogResult("Aimbot Head", aimAob, success: true, list2.Count, $"ReadOffset=0x{ReadOffset:X}, WriteOffset=0x{WriteOffset:X} | Raw={list.Count} Deduped={list2.Count}");
			return "Pattern Found";
		}
		catch (Exception ex)
		{
			DiagnosticLogger.LogResult("Aimbot Head", aimAob, success: false, 0, $"ReadOffset=0x{ReadOffset:X}, WriteOffset=0x{WriteOffset:X}", ex.Message);
			return "Scan Error: " + ex.Message;
		}
	}

	public static async Task<string> EnableAimbotHead()
	{
		RestoreAimbotMianValues();
		string text = await ScanHeadAsync();
		if (text != "Pattern Found")
		{
			return text;
		}
		(long addr, int onVal, int offVal, long baseAddr)[] list = _writeList;
		await Task.Run(() =>
		{
			if (list != null)
			{
				(long, int, int, long)[] array = list;
				for (int i = 0; i < array.Length; i++)
				{
					(long, int, int, long) tuple = array[i];
					try
					{
						moziii.WriteInt(tuple.Item1, tuple.Item2);
					}
					catch
					{
					}
				}
			}
		});
		_currentMode = AimbotActiveMode.Head;
		StartAimbotHeartbeatWatcher();
		SoundEffects.PlayEnableSound();
		return "Aimbot Head ON";
	}

	public static void RestoreAimbotNewValues()
	{
		StopAimbotHeartbeatWatcher();
		(long, int, int, long)[] writeListNew = _writeListNew;
		lock (_aimbotLock)
		{
			_writeListNew = new (long, int, int, long)[0];
		}
		if (writeListNew == null || writeListNew.Length == 0)
		{
			return;
		}
		(long, int, int, long)[] array = writeListNew;
		for (int i = 0; i < array.Length; i++)
		{
			(long, int, int, long) tuple = array[i];
			try
			{
				if (_mozNew.ReadInt(tuple.Item1) == tuple.Item2)
				{
					_mozNew.WriteInt(tuple.Item1, tuple.Item3);
				}
			}
			catch
			{
			}
		}
	}

	public static async Task<string> ScanAimbotNewAsync()
	{
		string runningEmulatorProcessName = GetProcess.GetRunningEmulatorProcessName();
		if (!_mozNew.SetProcess(new string[1] { runningEmulatorProcessName ?? "HD-Player" }))
		{
			return "Emulator Not Found!! Open Emulator First";
		}
		try
		{
			_mozNew.ClearScanCache();
			List<long> addresses = null;
			await Task.Run(() =>
			{
				addresses = _mozNew.AoBScanSync(aimNewAob);
			});
			if (addresses == null || addresses.Count == 0)
			{
				DiagnosticLogger.LogResult("Aimbot New", aimNewAob, success: false, 0, $"ReadOffset=0x{ReadOffsetNew:X}, WriteOffset=0x{WriteOffsetNew:X}", "AoBScan found 0 matches in emulator memory");
				return "Pattern Not Found";
			}
			List<(long addr, int onVal, int offVal, long baseAddr)> list = new List<(long, int, int, long)>();
			await Task.Run(() =>
			{
				long num = ((_selectedBone == TargetBone.Neck || _selectedBone == TargetBone.Chest) ? 186 : ((_selectedBone == TargetBone.Hip) ? 188 : ReadOffsetNew));
				foreach (long item in addresses)
				{
					long address = item + num;
					long num2 = item + WriteOffsetNew;
					int num3 = _mozNew.ReadInt(address);
					int num4 = _mozNew.ReadInt(num2);
					if (num3 != 0 && num4 != 0 && (uint)num3 > 4096u && (uint)num4 > 4096u && num3 != num4)
					{
						list.Add((num2, num3, num4, item));
					}
				}
			});
			if (list.Count == 0)
			{
				DiagnosticLogger.LogResult("Aimbot New", aimNewAob, success: false, addresses.Count, $"ReadOffset=0x{ReadOffsetNew:X}, WriteOffset=0x{WriteOffsetNew:X}", "Addresses found but entity pointers were invalid");
				return "Entities Not Loaded! Activate in Match / Training";
			}
			List<(long, int, int, long)> list2 = new List<(long, int, int, long)>();
			List<long> list3 = new List<long>();
			foreach (var item2 in list)
			{
				bool flag = false;
				foreach (long item3 in list3)
				{
					if (Math.Abs(item2.baseAddr - item3) < 4096)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					list3.Add(item2.baseAddr);
					list2.Add(item2);
					if (list2.Count >= 20)
					{
						break;
					}
				}
			}
			lock (_aimbotLock)
			{
				_writeListNew = list2.ToArray();
			}
			_mozNew.ClearScanCache();
			DiagnosticLogger.LogResult("Aimbot New", aimNewAob, success: true, list2.Count, $"ReadOffset=0x{ReadOffsetNew:X}, WriteOffset=0x{WriteOffsetNew:X} | Raw={list.Count} Deduped={list2.Count}");
			return "Pattern Found";
		}
		catch (Exception ex)
		{
			DiagnosticLogger.LogResult("Aimbot New", aimNewAob, success: false, 0, $"ReadOffset=0x{ReadOffsetNew:X}, WriteOffset=0x{WriteOffsetNew:X}", ex.Message);
			return "Scan Error: " + ex.Message;
		}
	}

	public static async Task<string> EnableAimbotNew()
	{
		RestoreAimbotMianValues();
		string text = await ScanAimbotNewAsync();
		if (text != "Pattern Found")
		{
			return text;
		}
		(long addr, int onVal, int offVal, long baseAddr)[] list = _writeListNew;
		await Task.Run(() =>
		{
			if (list != null)
			{
				(long, int, int, long)[] array = list;
				for (int i = 0; i < array.Length; i++)
				{
					(long, int, int, long) tuple = array[i];
					try
					{
						_mozNew.WriteInt(tuple.Item1, tuple.Item2);
					}
					catch
					{
					}
				}
			}
		});
		_currentMode = AimbotActiveMode.New;
		StartAimbotHeartbeatWatcher();
		SoundEffects.PlayEnableSound();
		return "Aimbot New ON";
	}

	public static void RestoreAimbotV2Values()
	{
		StopAimbotHeartbeatWatcher();
		(long, int, int)[] writeListV = _writeListV2;
		lock (_aimbotLock)
		{
			_writeListV2 = new (long, int, int)[0];
		}
		if (writeListV == null || writeListV.Length == 0)
		{
			return;
		}
		(long, int, int)[] array = writeListV;
		for (int i = 0; i < array.Length; i++)
		{
			(long, int, int) tuple = array[i];
			try
			{
				if (_mozV2.ReadInt(tuple.Item1) == tuple.Item2)
				{
					_mozV2.WriteInt(tuple.Item1, tuple.Item3);
				}
			}
			catch
			{
			}
		}
	}

	public static async Task<string> ScanAimbotV2Async()
	{
		string runningEmulatorProcessName = GetProcess.GetRunningEmulatorProcessName();
		if (!_mozV2.SetProcess(new string[1] { runningEmulatorProcessName ?? "HD-Player" }))
		{
			return "Emulator Not Found!! Open Emulator First";
		}
		try
		{
			_mozV2.ClearScanCache();
			List<long> addresses = null;
			await Task.Run(() =>
			{
				addresses = _mozV2.AoBScanSync(aimV2Aob);
			});
			if (addresses == null || addresses.Count == 0)
			{
				DiagnosticLogger.LogResult("Aimbot V2", aimV2Aob, success: false, 0, $"Read1=0x{ReadOffset1:X}, Write1=0x{WriteOffset1:X}, Read2=0x{ReadOffset2:X}, Write2=0x{WriteOffset2:X}");
				return "Pattern Not Found";
			}
			_v2BaseAddresses = addresses;
			List<(long addr, int onVal, int offVal)> list = new List<(long, int, int)>();
			await Task.Run(() =>
			{
				foreach (long item in addresses)
				{
					long address = item + ReadOffset1;
					long num2 = item + WriteOffset1;
					int num3 = _mozV2.ReadInt(address);
					int num4 = _mozV2.ReadInt(num2);
					if (num3 != 0 && num4 != 0 && (uint)num3 > 4096u && (uint)num4 > 4096u && num3 != num4)
					{
						list.Add((num2, num3, num4));
					}
					long address2 = item + ReadOffset2;
					long num5 = item + WriteOffset2;
					int num6 = _mozV2.ReadInt(address2);
					int num7 = _mozV2.ReadInt(num5);
					if (num6 != 0 && num7 != 0 && (uint)num6 > 4096u && (uint)num7 > 4096u && num6 != num7)
					{
						list.Add((num5, num6, num7));
					}
				}
			});
			if (list.Count == 0)
			{
				DiagnosticLogger.LogResult("Aimbot V2", aimV2Aob, success: false, addresses.Count, $"Read1=0x{ReadOffset1:X}, Write1=0x{WriteOffset1:X}, Read2=0x{ReadOffset2:X}, Write2=0x{WriteOffset2:X}", "Addresses found but entity pointers were invalid");
				return "Entities Not Loaded! Activate in Match / Training";
			}
			List<(long, int, int)> list2 = new List<(long, int, int)>();
			List<long> list3 = new List<long>();
			foreach (var item2 in list)
			{
				long num = item2.addr - WriteOffset1;
				bool flag = false;
				foreach (long item3 in list3)
				{
					if (Math.Abs(num - item3) < 4096)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					list3.Add(num);
				}
				if (list3.Count <= 20)
				{
					list2.Add(item2);
				}
			}
			lock (_aimbotLock)
			{
				_writeListV2 = list2.ToArray();
			}
			_mozV2.ClearScanCache();
			DiagnosticLogger.LogResult("Aimbot V2", aimV2Aob, success: true, list2.Count, $"Read1=0x{ReadOffset1:X}, Write1=0x{WriteOffset1:X}, Read2=0x{ReadOffset2:X}, Write2=0x{WriteOffset2:X} | Raw={list.Count} Deduped={list2.Count}");
			return "Pattern Found";
		}
		catch (Exception ex)
		{
			DiagnosticLogger.LogResult("Aimbot V2", aimV2Aob, success: false, 0, $"Read1=0x{ReadOffset1:X}, Write1=0x{WriteOffset1:X}, Read2=0x{ReadOffset2:X}, Write2=0x{WriteOffset2:X}", ex.Message);
			return "Scan Error: " + ex.Message;
		}
	}

	public static async Task<string> EnableAimbotV2()
	{
		RestoreAimbotMianValues();
		string text = await ScanAimbotV2Async();
		if (text != "Pattern Found")
		{
			return text;
		}
		(long addr, int onVal, int offVal)[] list = _writeListV2;
		await Task.Run(() =>
		{
			if (list != null && list.Length != 0)
			{
				(long, int, int)[] array = list;
				for (int i = 0; i < array.Length; i++)
				{
					(long, int, int) tuple = array[i];
					try
					{
						_mozV2.WriteInt(tuple.Item1, tuple.Item2);
					}
					catch
					{
					}
				}
			}
		});
		_currentMode = AimbotActiveMode.V2;
		StartAimbotHeartbeatWatcher();
		SoundEffects.PlayEnableSound();
		return "Aimbot V2 ON";
	}

	public static void RestoreAimbotMianValues()
	{
		_currentMode = AimbotActiveMode.None;
		StopAimbotHeartbeatWatcher();
		(long, int, int, long)[] writeList = _writeList;
		(long, int, int)[] writeListV = _writeListV2;
		(long, int, int, long)[] writeListNew = _writeListNew;
		lock (_aimbotLock)
		{
			_writeList = new (long, int, int, long)[0];
			_writeListV2 = new (long, int, int)[0];
			_writeListNew = new (long, int, int, long)[0];
			_v2BaseAddresses.Clear();
		}
		(long, int, int, long)[] array;
		if (writeList != null && writeList.Length != 0)
		{
			array = writeList;
			for (int i = 0; i < array.Length; i++)
			{
				(long, int, int, long) tuple = array[i];
				try
				{
					if (moziii.ReadInt(tuple.Item1) == tuple.Item2)
					{
						moziii.WriteInt(tuple.Item1, tuple.Item3);
					}
				}
				catch
				{
				}
			}
		}
		if (writeListV != null && writeListV.Length != 0)
		{
			(long, int, int)[] array2 = writeListV;
			for (int i = 0; i < array2.Length; i++)
			{
				(long, int, int) tuple2 = array2[i];
				try
				{
					if (_mozV2.ReadInt(tuple2.Item1) == tuple2.Item2)
					{
						_mozV2.WriteInt(tuple2.Item1, tuple2.Item3);
					}
				}
				catch
				{
				}
			}
		}
		if (writeListNew == null || writeListNew.Length == 0)
		{
			return;
		}
		array = writeListNew;
		for (int i = 0; i < array.Length; i++)
		{
			(long, int, int, long) tuple3 = array[i];
			try
			{
				if (_mozNew.ReadInt(tuple3.Item1) == tuple3.Item2)
				{
					_mozNew.WriteInt(tuple3.Item1, tuple3.Item3);
				}
			}
			catch
			{
			}
		}
	}

	public static async Task<string> EnableAimbotMian()
	{
		if (_isEnablingMian)
		{
			return "Scanning...";
		}
		_isEnablingMian = true;
		try
		{
			if (_writeList.Length != 0 || _writeListV2.Length != 0)
			{
				RestoreAimbotMianValues();
			}
			Task<string> task = ScanHeadAsync();
			Task<string> task2 = ScanAimbotV2Async();
			await Task.WhenAll<string>(task, task2);
			(long addr, int onVal, int offVal, long baseAddr)[] listHead = _writeList;
			(long addr, int onVal, int offVal)[] listV2 = _writeListV2;
			if ((listHead == null || listHead.Length == 0) && (listV2 == null || listV2.Length == 0))
			{
				RestoreAimbotMianValues();
				return "Lobby/Plane: Land First!";
			}
			await Task.Run(() =>
			{
				if (listV2 != null && listV2.Length != 0)
				{
					(long, int, int)[] array = listV2;
					for (int i = 0; i < array.Length; i++)
					{
						(long, int, int) tuple = array[i];
						try
						{
							_mozV2.WriteInt(tuple.Item1, tuple.Item2);
						}
						catch
						{
						}
					}
				}
				List<(long, int, int, long)> list = new List<(long, int, int, long)>();
				if (listHead != null && listHead.Length != 0)
				{
					List<long> v2BaseAddresses = _v2BaseAddresses;
					long[] array2 = ((listV2 != null) ? listV2.Select(((long addr, int onVal, int offVal) v) => v.addr).ToArray() : new long[0]);
					(long, int, int, long)[] array3 = listHead;
					for (int i = 0; i < array3.Length; i++)
					{
						(long, int, int, long) item = array3[i];
						bool flag = false;
						if (v2BaseAddresses != null && v2BaseAddresses.Count > 0)
						{
							foreach (long item2 in v2BaseAddresses)
							{
								if (Math.Abs(item.Item4 - item2) < 2048)
								{
									flag = true;
									break;
								}
							}
						}
						if (!flag)
						{
							for (int num = 0; num < array2.Length; num++)
							{
								if (Math.Abs(item.Item1 - array2[num]) < 2048)
								{
									flag = true;
									break;
								}
							}
						}
						if (!flag)
						{
							try
							{
								moziii.WriteInt(item.Item1, item.Item2);
								list.Add(item);
							}
							catch
							{
							}
						}
					}
				}
				lock (_aimbotLock)
				{
					_writeList = list.ToArray();
				}
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
			string text = Path.Combine(Path.GetTempPath(), "MIAN XITERS.dll");
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
