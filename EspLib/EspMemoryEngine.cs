using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace EspLib
{

public class EspMemoryEngine
{
	private static class Offsets
	{
		public const ulong VmCpuCount = 60uL;

		public const ulong VmReadCounter = 1093616uL;

		public const ulong VmDirectRamWindow = 1064960uL;

		public const ulong VmDirectRamTag = 1073152uL;

		public const ulong VmRamRangeMode = 1073177uL;

		public const ulong VmRangeCache = 1073232uL;

		public const ulong VmRangeTreeRoot = 1073304uL;

		public const ulong VmMmio2Table = 1073328uL;

		public const ulong VmChunkCache = 1075296uL;

		public const ulong VmChunkTreeRoot = 1076320uL;

		public const ulong VmPageMapCache = 1076352uL;

		public const ulong VmCpuArray = 1179136uL;

		public const ulong BstkMapChunkRva = 893040uL;

		public const ulong ExpectedPvcpuDelta = 1179648uL;

		public const ulong RangeStart = 0uL;

		public const ulong RangeSize = 8uL;

		public const ulong RangeHostBase = 48uL;

		public const ulong RangeLeft = 80uL;

		public const ulong RangeRight = 88uL;

		public const ulong RangePageDescTable = 112uL;

		public const ulong RangePageDescStride = 16uL;

		public const ulong PageDescState = 0uL;

		public const ulong PageDescInfo = 8uL;

		public const ulong ChunkNodeLeft = 0uL;

		public const ulong ChunkNodeRight = 8uL;

		public const ulong ChunkNodeKey = 16uL;

		public const ulong ChunkHostBase = 40uL;

		public const ulong MmioRangeHostBase = 112uL;

		public const ulong PageMapCacheStride = 32uL;

		public const ulong PageMapCachedTag = 0uL;

		public const ulong PageMapPageDesc = 8uL;

		public const ulong PageMapHostBase = 24uL;

		public const ulong RemoteStubPvm = 6uL;

		public const ulong RemoteStubChunkId = 15uL;

		public const ulong RemoteStubResultPtr0 = 21uL;

		public const ulong RemoteStubTargetFn = 31uL;

		public const ulong RemoteStubResultPtr1 = 43uL;
	}

	private static class Util
	{
		public const ulong LongModePageFrameMask = 4503599627366400uL;

		public const ulong PageSize = 4096uL;

		public const ulong LargePage1GbMask = 4503599625273344uL;

		public const ulong LargePage2MbMask = 4503599625273344uL;

		public const ulong UserPointerMin = 65536uL;

		public const ulong UserPointerMax = 140737488355327uL;

		public const ulong ReadCounterLimit = 1000000000000uL;

		public const ulong DirectStateMaskHigh = 1970324836974592uL;

		public const ulong DirectStateMaskLow = 4503599627366400uL;

		public const ulong PresentBit = 1uL;

		public const ulong PageSizeBit = 128uL;

		public const ulong PageMask = 18446744073709547520uL;

		public const ulong PageOffsetMask = 4095uL;

		public const uint ChunkCacheMask = 63u;

		public const ulong RangeCacheMask = 7uL;

		public const ulong PageMapCacheMask = 255uL;

		public const int RemoteStubSize = 64;

		public const int PEB_LDR = 24;

		public static bool IsLikelyUserPointer(ulong value)
		{
			if (value >= 65536)
			{
				return value < 140737488355327L;
			}
			return false;
		}

		public static bool IsLikelyGuestKernelPointer(ulong value)
		{
			return value >= 18446462598732840960uL;
		}

		public static string Hex(ulong value)
		{
			return $"0x{value:X}";
		}
	}

	private nint processHandle;

	private uint processId;

	private ulong moduleBase;

	private VmInfo vmInfo;

	private readonly object cacheLock = new object();

	private readonly Dictionary<(ulong cr3, ulong va), ulong> gvaToPhysicalPageCache = new Dictionary<(ulong, ulong), ulong>();

	private readonly Dictionary<(ulong cr3, ulong va), ulong> gvaToHostPageCache = new Dictionary<(ulong, ulong), ulong>();

	private readonly Dictionary<ulong, ulong> physToHostPageCache = new Dictionary<ulong, ulong>();

	private static readonly string[] s_emulatorProcessNames = new string[] 
	{ 
		"HD-Player", "Bluestacks", "Bluestacks-5", "BstkSVC", "HD-Agent", "HdPlayerService",
		"MSIAppPlayer", "AppPlayer", "BlueStacksApp", "NoxVMHandle", "LdBoxHeadless", 
		"dnplayer", "MEmuHeadless", "MEmu", "AndroidEmulatorEn", "VBoxHeadless", 
		"ProjectTitan", "SmartGaGa"
	};

	private static readonly string[] s_vmmModuleNames = new string[] 
	{ 
		"BstkVMM.dll", "BstkVMM2.dll", "bstk_vmm.dll", "BstkAppPlayer.dll", "HD-Player.exe",
		"VBoxVMM.dll", "VirtualBoxVM.dll", "VBoxRT.dll", "dnplayer.exe", "LdBoxHeadless.exe",
		"NoxVMHandle.exe", "ProjectTitan.exe", "TitanVMM.dll"
	};

	public bool IsInitialized
	{
		get
		{
			if (processHandle != IntPtr.Zero && processId != 0 && vmInfo.Pvm != 0L)
			{
				return vmInfo.Pvcpu0 != 0;
			}
			return false;
		}
	}

	public ulong Pvm => vmInfo.Pvm;

	public ulong Pvcpu => vmInfo.Pvcpu0;

	public uint ProcessId => processId;

	public string LastInitError { get; private set; } = "";

	private static List<uint> FindCandidateEmulatorPids()
	{
		var pids = new List<uint>();
		foreach (string name in s_emulatorProcessNames)
		{
			try
			{
				string cleanName = name.EndsWith(".exe", StringComparison.OrdinalIgnoreCase)
					? name.Substring(0, name.Length - 4)
					: name;
				var procs = Process.GetProcessesByName(cleanName);
				// Order by working set descending: the game instance uses 1GB+ RAM, while background services use <50MB
				foreach (var p in procs.OrderByDescending(p => { try { return p.WorkingSet64; } catch { return 0L; } }))
				{
					if (!pids.Contains((uint)p.Id))
					{
						pids.Add((uint)p.Id);
					}
				}
			}
			catch { }
		}
		return pids;
	}

	public bool Initialize(bool verbose = false)
	{
		Shutdown();
		LastInitError = "";

		var candidatePids = FindCandidateEmulatorPids();
		if (candidatePids.Count == 0)
		{
			LastInitError = "Emulator process not found (HD-Player, Bluestacks, LDPlayer, etc.)";
			return false;
		}

		int lastError = 0;
		foreach (uint pid in candidatePids)
		{
			nint handle = IntPtr.Zero;
			try
			{
				handle = NativeMethods.OpenProcess(0x1F0FFF, bInheritHandle: false, (int)pid);
				if (handle == IntPtr.Zero)
				{
					handle = NativeMethods.OpenProcess(1080u, bInheritHandle: false, (int)pid);
				}
			}
			catch
			{
				handle = IntPtr.Zero;
			}

			if (handle == IntPtr.Zero)
			{
				lastError = Marshal.GetLastWin32Error();
				continue;
			}

			processHandle = handle;
			processId = pid;

			// Probe for VMM module base if present
			foreach (string text2 in s_vmmModuleNames)
			{
				var tuple = FindModuleInProcess(processId, text2);
				if (tuple.Item1 != 0L)
				{
					moduleBase = tuple.Item1;
					break;
				}
			}

			// Locate VM memory structures directly in RAM
			VmInfo? vm = null;
			try
			{
				vm = LocateVmInfo(verbose);
			}
			catch { }

			if (vm.HasValue && vm.Value.Pvcpu0 != 0)
			{
				this.vmInfo = vm.Value;
				return true;
			}

			// Not the right process, clean up handle and try next candidate
			NativeMethods.CloseHandle(processHandle);
			processHandle = IntPtr.Zero;
			processId = 0u;
			moduleBase = 0uL;
			vmInfo = default;
		}

		if (processHandle == IntPtr.Zero && lastError == 5)
		{
			LastInitError = "OpenProcess access denied (Error 5) — run app as Administrator!";
		}
		else
		{
			LastInitError = "Virtual machine RAM structure not located — restart Free Fire in BlueStacks!";
		}
		Shutdown();
		return false;
	}

	public void Shutdown()
	{
		ClearCaches();
		if (processHandle != IntPtr.Zero)
		{
			NativeMethods.CloseHandle(processHandle);
			processHandle = IntPtr.Zero;
		}
		processId = 0u;
		moduleBase = 0uL;
		vmInfo = default;
	}

	public void ClearCaches()
	{
		lock (cacheLock)
		{
			gvaToPhysicalPageCache.Clear();
			gvaToHostPageCache.Clear();
			physToHostPageCache.Clear();
		}
	}

	private void ClearTranslationCaches()
	{
		ClearCaches();
	}

	private bool ReadHostBytes(ulong address, byte[] buffer, int size)
	{
		if (processHandle == IntPtr.Zero || buffer == null || size == 0)
		{
			return false;
		}
		if (NativeMethods.ReadProcessMemory(processHandle, (nuint)address, buffer, (nuint)size, out var lpNumberOfBytesRead))
		{
			return (int)lpNumberOfBytesRead == size;
		}
		return false;
	}

	private bool ReadHostBytesAt(ulong address, byte[] buffer, int size, int bufferOffset)
	{
		if (processHandle == IntPtr.Zero || buffer == null || size == 0)
		{
			return false;
		}
		GCHandle gCHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
		try
		{
			nint lpBuffer = IntPtr.Add(gCHandle.AddrOfPinnedObject(), bufferOffset);
			nuint lpNumberOfBytesRead;
			return NativeMethods.ReadProcessMemory(processHandle, (nuint)address, lpBuffer, (nuint)size, out lpNumberOfBytesRead) && (int)lpNumberOfBytesRead == size;
		}
		finally
		{
			gCHandle.Free();
		}
	}

	private bool ReadHostBytes(ulong address, nint buffer, int size)
	{
		if (processHandle == IntPtr.Zero || buffer == IntPtr.Zero || size == 0)
		{
			return false;
		}
		if (NativeMethods.ReadProcessMemory(processHandle, (nuint)address, buffer, (nuint)size, out var lpNumberOfBytesRead))
		{
			return (int)lpNumberOfBytesRead == size;
		}
		return false;
	}

	private bool WriteHostBytes(ulong address, byte[] buffer, int size)
	{
		if (processHandle == IntPtr.Zero || buffer == null || size == 0)
		{
			return false;
		}
		if (NativeMethods.WriteProcessMemory(processHandle, (nuint)address, buffer, (nuint)size, out var lpNumberOfBytesWritten))
		{
			return (int)lpNumberOfBytesWritten == size;
		}
		return false;
	}

	private bool WriteHostBytesAt(ulong address, byte[] buffer, int size, int bufferOffset)
	{
		if (processHandle == IntPtr.Zero || buffer == null || size == 0)
		{
			return false;
		}
		GCHandle gCHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
		try
		{
			nint lpBuffer = IntPtr.Add(gCHandle.AddrOfPinnedObject(), bufferOffset);
			nuint lpNumberOfBytesWritten;
			return NativeMethods.WriteProcessMemory(processHandle, (nuint)address, lpBuffer, (nuint)size, out lpNumberOfBytesWritten) && (int)lpNumberOfBytesWritten == size;
		}
		finally
		{
			gCHandle.Free();
		}
	}

	private T ReadHostValue<T>(ulong address) where T : struct
	{
		int num = Marshal.SizeOf<T>();
		byte[] array = new byte[num];
		if (!ReadHostBytes(address, array, num))
		{
			return default;
		}
		GCHandle gCHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
		try
		{
			return Marshal.PtrToStructure<T>(gCHandle.AddrOfPinnedObject());
		}
		finally
		{
			gCHandle.Free();
		}
	}

	private bool ReadHostValue<T>(ulong address, out T value) where T : struct
	{
		value = default;
		int num = Marshal.SizeOf<T>();
		byte[] array = new byte[num];
		if (!ReadHostBytes(address, array, num))
		{
			return false;
		}
		GCHandle gCHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
		try
		{
			value = Marshal.PtrToStructure<T>(gCHandle.AddrOfPinnedObject());
			return true;
		}
		finally
		{
			gCHandle.Free();
		}
	}

	private VmInfo? LocateVmInfo(bool verbose)
	{
		NativeMethods.MEMORY_BASIC_INFORMATION lpBuffer = default;
		ulong num = 0uL;
		VmInfo? result = null;
		int num2 = Marshal.SizeOf<NativeMethods.MEMORY_BASIC_INFORMATION>();
		while ((nuint)NativeMethods.VirtualQueryEx(processHandle, (nuint)num, out lpBuffer, (nuint)num2) == (nuint)num2)
		{
		uint num3 = lpBuffer.Protect & 0xFF;
			bool flag = num3 != 0 && (num3 & 0x01) == 0 && (lpBuffer.Protect & 0x100) == 0;
			if (((lpBuffer.State == 4096) & flag) && (ulong)lpBuffer.RegionSize >= 131072uL)
			{
				ulong num4 = lpBuffer.BaseAddress;
				uint num5 = ReadHostValue<uint>(num4 + 60);
				bool flag2 = num5 >= 1 && num5 <= 64;
				ulong num6 = ReadHostValue<ulong>(num4 + 1179136);
				bool flag4 = num6 != 0;
				bool flag5 = flag4 && (num6 == num4 + 1179648 || (num6 >= num4 && num6 <= num4 + (ulong)lpBuffer.RegionSize + 0x200000));
				ulong num7 = ReadHostValue<ulong>(num4 + 1093616);

				if (flag2 && flag4)
				{
					var candidate = new VmInfo
					{
						Pvm = num4,
						Pvcpu0 = num6,
						CpuCount = num5,
						ReadCounter = num7,
						RegionSize = lpBuffer.RegionSize
					};

					if (flag5 || num6 == num4 + 1179648)
					{
						return candidate;
					}
					else if (!result.HasValue)
					{
						result = candidate;
					}
				}
			}
			ulong num8 = num + lpBuffer.RegionSize;
			if (num8 <= num)
			{
				break;
			}
			num = num8;
		}
		return result;
	}

	private static uint FindProcessIdByName(string processName)
	{
		string value = processName.EndsWith(".exe", StringComparison.OrdinalIgnoreCase) ? processName.Substring(0, processName.Length - 4) : processName;
		Process[] processes = Process.GetProcesses();
		foreach (Process process in processes)
		{
			try
			{
				if (process.ProcessName.Equals(value, StringComparison.OrdinalIgnoreCase))
				{
					return (uint)process.Id;
				}
			}
			catch
			{
			}
		}
		return 0u;
	}

	private (ulong Base, uint Size) FindModuleInProcess(uint pid, string moduleName)
	{
		string value = moduleName.EndsWith(".exe", StringComparison.OrdinalIgnoreCase) ? moduleName.Substring(0, moduleName.Length - 4) : moduleName;
		try
		{
			foreach (ProcessModule module in Process.GetProcessById((int)pid).Modules)
			{
				try
				{
					string modNameNoExe = module.ModuleName.EndsWith(".exe", StringComparison.OrdinalIgnoreCase) ? module.ModuleName.Substring(0, module.ModuleName.Length - 4) : module.ModuleName;
					if (modNameNoExe.Equals(value, StringComparison.OrdinalIgnoreCase))
					{
						return (Base: (ulong)module.BaseAddress, Size: (uint)module.ModuleMemorySize);
					}
				}
				catch
				{
				}
			}
		}
		catch
		{
		}
		return (Base: 0uL, Size: 0u);
	}

	private bool MapChunkViaBstkVmm(uint chunkId, out ulong outChunkPointer, bool verbose)
	{
		outChunkPointer = 0uL;
		if (moduleBase == 0L || processId == 0)
		{
			return false;
		}
		nint num = NativeMethods.OpenProcess(1082u, bInheritHandle: false, (int)processId);
		if (num == IntPtr.Zero)
		{
			return false;
		}
		int num2 = Marshal.SizeOf<ulong>() + Marshal.SizeOf<long>();
		int num3 = num2 + 64;
		nuint num4 = NativeMethods.VirtualAllocEx(num, UIntPtr.Zero, (nuint)num3, 12288u, 64u);
		if (num4 == UIntPtr.Zero)
		{
			NativeMethods.CloseHandle(num);
			return false;
		}
		ulong num5 = num4;
		ulong num6 = num5 + (ulong)num2;
		ulong value = moduleBase + 893040;
		byte[] array = new byte[64];
		array[0] = 72;
		array[1] = 131;
		array[2] = 236;
		array[3] = 40;
		array[4] = 72;
		array[5] = 185;
		BitConverter.GetBytes(vmInfo.Pvm).CopyTo(array, 6);
		array[14] = 186;
		BitConverter.GetBytes(chunkId).CopyTo(array, 15);
		array[19] = 73;
		array[20] = 184;
		BitConverter.GetBytes(num5).CopyTo(array, 21);
		array[29] = 72;
		array[30] = 184;
		BitConverter.GetBytes(value).CopyTo(array, 31);
		array[39] = byte.MaxValue;
		array[40] = 208;
		array[41] = 72;
		array[42] = 185;
		BitConverter.GetBytes(num5).CopyTo(array, 43);
		array[51] = 72;
		array[52] = 99;
		array[53] = 192;
		array[54] = 72;
		array[55] = 137;
		array[56] = 65;
		array[57] = 8;
		array[58] = 72;
		array[59] = 131;
		array[60] = 196;
		array[61] = 40;
		array[62] = 195;
		byte[] lpBuffer = new byte[num2];
		NativeMethods.WriteProcessMemory(num, num4, lpBuffer, (nuint)num2, out var lpNumberOfBytesWritten);
		NativeMethods.WriteProcessMemory(num, (nuint)num6, array, 64u, out lpNumberOfBytesWritten);
		nint num7 = NativeMethods.CreateRemoteThread(num, IntPtr.Zero, UIntPtr.Zero, (nint)num6, UIntPtr.Zero, 0u, out var _);
		if (num7 == IntPtr.Zero)
		{
			NativeMethods.VirtualFreeEx(num, num4, UIntPtr.Zero, 32768u);
			NativeMethods.CloseHandle(num);
			return false;
		}
		uint num8 = NativeMethods.WaitForSingleObject(num7, 5000u);
		NativeMethods.CloseHandle(num7);
		if (num8 != 0)
		{
			NativeMethods.VirtualFreeEx(num, num4, UIntPtr.Zero, 32768u);
			NativeMethods.CloseHandle(num);
			return false;
		}
		byte[] array2 = new byte[num2];
		bool flag = NativeMethods.ReadProcessMemory(num, num4, array2, (nuint)num2, out var _);
		NativeMethods.VirtualFreeEx(num, num4, UIntPtr.Zero, 32768u);
		NativeMethods.CloseHandle(num);
		if (!flag)
		{
			return false;
		}
		ulong num9 = BitConverter.ToUInt64(array2, 0);
		if (BitConverter.ToInt64(array2, 8) < 0 || num9 == 0L)
		{
			return false;
		}
		outChunkPointer = num9;
		return true;
	}

	private ulong? FindChunkFromCache(uint chunkId)
	{
		uint num = chunkId & 0x3F;
		ulong num2 = vmInfo.Pvm + 1075296 + (ulong)((long)num * 16L);
		uint num3 = ReadHostValue<uint>(num2);
		ulong value = ReadHostValue<ulong>(num2 + 8);
		if (num3 != chunkId || !Util.IsLikelyUserPointer(value))
		{
			return null;
		}
		return value;
	}

	private ulong? FindChunkFromTree(uint chunkId, bool verbose)
	{
		ulong num = ReadHostValue<ulong>(vmInfo.Pvm + 1076320);
		while (num != 0L)
		{
			uint num2 = ReadHostValue<uint>(num + 16);
			if (num2 == chunkId)
			{
				return num;
			}
			ulong num3 = (ulong)((chunkId < num2) ? 0 : 8);
			num = ReadHostValue<ulong>(num + num3);
		}
		return null;
	}

	private ulong? FindPhysicalRange(ulong guestPhys)
	{
		ulong num = (guestPhys >> 20) & 7;
		ulong address = vmInfo.Pvm + 1073232 + num * 8;
		ulong num2 = ReadHostValue<ulong>(address);
		if (num2 != 0L)
		{
			ulong num3 = ReadHostValue<ulong>(num2);
			ulong num4 = ReadHostValue<ulong>(num2 + 8);
			if (guestPhys >= num3 && guestPhys - num3 < num4)
			{
				return num2;
			}
		}
		num2 = ReadHostValue<ulong>(vmInfo.Pvm + 1073304);
		while (num2 != 0L)
		{
			ulong num5 = ReadHostValue<ulong>(num2);
			ulong num6 = ReadHostValue<ulong>(num2 + 8);
			if (guestPhys >= num5 && guestPhys - num5 < num6)
			{
				return num2;
			}
			ulong num7 = (ulong)((guestPhys < num5) ? 80 : 88);
			num2 = ReadHostValue<ulong>(num2 + num7);
		}
		return null;
	}

	private ulong? TryPageMapCache(ulong guestPhys, bool verbose)
	{
		ulong num = (guestPhys >> 12) & 0xFF;
		ulong num2 = vmInfo.Pvm + 1076352 + num * 32;
		ulong num3 = guestPhys & 0xFFFFFFFFFFFFF000uL;
		ulong num4 = ReadHostValue<ulong>(num2);
		ReadHostValue<ulong>(num2 + 8);
		ulong num5 = ReadHostValue<ulong>(num2 + 24);
		if (num4 == num3 && Util.IsLikelyUserPointer(num5))
		{
			return num5 | (guestPhys & 0xFFF);
		}
		return null;
	}

	private ulong? MapPhysicalPageToHostBase(ulong guestPhys, bool verbose)
	{
		ulong? num = FindPhysicalRange(guestPhys);
		if (!num.HasValue)
		{
			return null;
		}
		ulong num2 = ReadHostValue<ulong>(num.Value);
		ulong num3 = guestPhys - num2 >> 12;
		ulong num4 = num.Value + 112 + num3 * 16;
		ulong num5 = ReadHostValue<ulong>(num4);
		uint num6 = ReadHostValue<uint>(num4 + 8);
		uint num7 = (uint)((num5 >> 51) & 7);
		uint num8 = (uint)((num5 >> 48) & 3);
		byte value = 0;
		ReadHostValue<byte>(vmInfo.Pvm + 1073177, out value);
		if (num7 != 2 && num7 != 3 && value != 0)
		{
			ulong num9 = ReadHostValue<ulong>(num.Value + 48);
			if (!Util.IsLikelyUserPointer(num9))
			{
				return null;
			}
			return num9 + ((guestPhys - num2) & 0xFFFFFFFFFFFFF000uL);
		}
		if (num8 == 0)
		{
			return vmInfo.Pvm + 1064960;
		}
		if (num7 == 2 || num7 == 3)
		{
			uint num10 = num6 >> 24;
			uint num11 = num6 & 0xFFFFFF;
			ulong num12 = ReadHostValue<ulong>(vmInfo.Pvm + 1073328 + (ulong)((long)num10 * 8L));
			if (!Util.IsLikelyUserPointer(num12))
			{
				return null;
			}
			return ReadHostValue<ulong>(num12 + 112) + ((ulong)num11 << 12);
		}
		uint num13 = num6 >> 9;
		uint num14 = num6 & 0x1FF;
		if (num13 != 0)
		{
			ulong? num15 = FindChunkFromCache(num13);
			if (!num15.HasValue)
			{
				num15 = FindChunkFromTree(num13, verbose);
				if (!num15.HasValue && MapChunkViaBstkVmm(num13, out var outChunkPointer, verbose))
				{
					num15 = outChunkPointer;
				}
				if (!num15.HasValue)
				{
					return null;
				}
			}
			return ReadHostValue<ulong>(num15.Value + 40) + ((ulong)num14 << 12);
		}
		if (num6 == 0)
		{
			ulong num16 = ReadHostValue<ulong>(vmInfo.Pvm + 1073152);
			bool flag = num7 == 4;
			bool flag2 = (num5 & 0x7000000000000L) == 0L && (num5 & 0xFFFFFFFFFF000L) == num16;
			if (flag | flag2)
			{
				return vmInfo.Pvm + 1064960;
			}
		}
		return null;
	}

	private ulong? MapPhysicalToHost(ulong guestPhys, bool verbose)
	{
		ulong key = guestPhys & 0xFFFFFFFFFFFFF000uL;
		lock (cacheLock)
		{
			if (physToHostPageCache.TryGetValue(key, out var value))
			{
				return value | (guestPhys & 0xFFF);
			}
		}
		ulong? result = TryPageMapCache(guestPhys, verbose);
		if (result.HasValue)
		{
			lock (cacheLock)
			{
				physToHostPageCache[key] = result.Value & 0xFFFFFFFFFFFFF000uL;
				return result;
			}
		}
		ulong? num = MapPhysicalPageToHostBase(guestPhys, verbose);
		if (!num.HasValue)
		{
			return null;
		}
		lock (cacheLock)
		{
			physToHostPageCache[key] = num.Value;
		}
		return num.Value | (guestPhys & 0xFFF);
	}

	public TranslationResult? ResolveGuestPhysical(ulong guestPhys, bool verbose = false)
	{
		ulong? num = MapPhysicalToHost(guestPhys, verbose);
		if (!num.HasValue)
		{
			return null;
		}
		return new TranslationResult
		{
			GuestPhysical = guestPhys,
			HostPointer = num.Value
		};
	}

	public bool ReadGuestPhysical(ulong guestPhys, byte[] buffer, int size, bool verbose = false)
	{
		int num2;
		for (int i = 0; i < size; i += num2)
		{
			ulong num = guestPhys + (ulong)i;
			num2 = Math.Min(size - i, (int)(4096 - (num & 0xFFF)));
			TranslationResult? translationResult = ResolveGuestPhysical(num, verbose);
			if (!translationResult.HasValue)
			{
				return false;
			}
			if (!ReadHostBytesAt(translationResult.Value.HostPointer, buffer, num2, i))
			{
				lock (cacheLock)
				{
					physToHostPageCache.Remove(num & 0xFFFFFFFFFFFFF000uL);
				}
				TranslationResult? translationResult2 = ResolveGuestPhysical(num, verbose);
				if (!translationResult2.HasValue || !ReadHostBytesAt(translationResult2.Value.HostPointer, buffer, num2, i))
				{
					return false;
				}
			}
		}
		return true;
	}

	public bool ReadGuestPhysical<T>(ulong guestPhys, out T value, bool verbose = false) where T : struct
	{
		value = default;
		int num = ((typeof(T) == typeof(bool)) ? 1 : Marshal.SizeOf<T>());
		byte[] array = new byte[num];
		if (!ReadGuestPhysical(guestPhys, array, num, verbose))
		{
			return false;
		}
		if (typeof(T) == typeof(bool))
		{
			value = (T)(object)(array[0] != 0);
			return true;
		}
		GCHandle gCHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
		try
		{
			value = Marshal.PtrToStructure<T>(gCHandle.AddrOfPinnedObject());
			return true;
		}
		finally
		{
			gCHandle.Free();
		}
	}

	public bool ReadGuestKernel(ulong kernelVa, byte[] buffer, int size, bool verbose = false)
	{
		int num2;
		for (int i = 0; i < size; i += num2)
		{
			ulong num = kernelVa + (ulong)i;
			num2 = Math.Min(size - i, (int)(4096 - (num & 0xFFF)));
			if (!TryConvertKernelVirtualToPhysical(num, out var outPhys))
			{
				return false;
			}
			TranslationResult? translationResult = ResolveGuestPhysical(outPhys, verbose);
			if (!translationResult.HasValue)
			{
				return false;
			}
			if (!ReadHostBytesAt(translationResult.Value.HostPointer, buffer, num2, i))
			{
				lock (cacheLock)
				{
					physToHostPageCache.Remove(outPhys & 0xFFFFFFFFFFFFF000uL);
				}
				TranslationResult? translationResult2 = ResolveGuestPhysical(outPhys, verbose);
				if (!translationResult2.HasValue || !ReadHostBytesAt(translationResult2.Value.HostPointer, buffer, num2, i))
				{
					return false;
				}
			}
		}
		return true;
	}

	public bool ReadGuestKernel<T>(ulong kernelVa, out T value, bool verbose = false) where T : struct
	{
		value = default;
		int num = ((typeof(T) == typeof(bool)) ? 1 : Marshal.SizeOf<T>());
		byte[] array = new byte[num];
		if (!ReadGuestKernel(kernelVa, array, num, verbose))
		{
			return false;
		}
		if (typeof(T) == typeof(bool))
		{
			value = (T)(object)(array[0] != 0);
			return true;
		}
		GCHandle gCHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
		try
		{
			value = Marshal.PtrToStructure<T>(gCHandle.AddrOfPinnedObject());
			return true;
		}
		finally
		{
			gCHandle.Free();
		}
	}

	public bool WriteGuestPhysical(ulong guestPhys, byte[] buffer, int size, bool verbose = false)
	{
		int num2;
		for (int i = 0; i < size; i += num2)
		{
			ulong num = guestPhys + (ulong)i;
			num2 = Math.Min(size - i, (int)(4096 - (num & 0xFFF)));
			TranslationResult? translationResult = ResolveGuestPhysical(num, verbose);
			if (!translationResult.HasValue)
			{
				return false;
			}
			if (!WriteHostBytesAt(translationResult.Value.HostPointer, buffer, num2, i))
			{
				lock (cacheLock)
				{
					physToHostPageCache.Remove(num & 0xFFFFFFFFFFFFF000uL);
				}
				TranslationResult? translationResult2 = ResolveGuestPhysical(num, verbose);
				if (!translationResult2.HasValue || !WriteHostBytesAt(translationResult2.Value.HostPointer, buffer, num2, i))
				{
					return false;
				}
			}
		}
		return true;
	}

	public bool WriteGuestPhysical<T>(ulong guestPhys, T value, bool verbose = false) where T : struct
	{
		if (typeof(T) == typeof(bool))
		{
			byte[] buffer = new byte[1] { ((bool)(object)value) ? ((byte)1) : ((byte)0) };
			return WriteGuestPhysical(guestPhys, buffer, 1, verbose);
		}
		int num = Marshal.SizeOf<T>();
		byte[] array = new byte[num];
		GCHandle gCHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
		try
		{
			Marshal.StructureToPtr(value, gCHandle.AddrOfPinnedObject(), fDeleteOld: false);
		}
		finally
		{
			gCHandle.Free();
		}
		return WriteGuestPhysical(guestPhys, array, num, verbose);
	}

	public bool ReadGuestUser(ulong guestCr3, ulong guestVa, byte[] buffer, int size, bool verbose = false)
	{
		int num = 0;
		while (num < size)
		{
			ulong num2 = guestVa + (ulong)num;
			ulong num3 = num2 & 0xFFF;
			int num4 = Math.Min(size - num, (int)(4096 - num3));
			ulong item = num2 & 0xFFFFFFFFFFFFF000uL;
			ulong item2 = guestCr3 & 0xFFFFFFFFFF000L;
			bool flag = false;
			ulong value;
			lock (cacheLock)
			{
				flag = gvaToHostPageCache.TryGetValue((item2, item), out value);
			}
			if (flag)
			{
				if (ReadHostBytesAt(value + num3, buffer, num4, num))
				{
					num += num4;
					continue;
				}
				lock (cacheLock)
				{
					gvaToHostPageCache.Remove((item2, item));
				}
			}
			bool flag2 = false;
			for (int i = 0; i < 2; i++)
			{
				if (flag2)
				{
					break;
				}
				ulong? num5 = TranslateGuestVa(guestCr3, num2, verbose);
				if (!num5.HasValue)
				{
					lock (cacheLock)
					{
						gvaToPhysicalPageCache.Remove((item2, item));
					}
					continue;
				}
				ulong? num6 = MapPhysicalToHost(num5.Value, verbose);
				if (!num6.HasValue)
				{
					lock (cacheLock)
					{
						gvaToPhysicalPageCache.Remove((item2, item));
						physToHostPageCache.Remove(num5.Value);
					}
					continue;
				}
				if (ReadHostBytesAt(num6.Value, buffer, num4, num))
				{
					lock (cacheLock)
					{
						gvaToHostPageCache[(item2, item)] = num6.Value & 0xFFFFFFFFFFFFF000uL;
					}
					flag2 = true;
					break;
				}
				lock (cacheLock)
				{
					gvaToHostPageCache.Remove((item2, item));
					gvaToPhysicalPageCache.Remove((item2, item));
					physToHostPageCache.Remove(num5.Value);
				}
			}
			if (!flag2)
			{
				return false;
			}
			num += num4;
		}
		return true;
	}

	public bool ReadGuestUser<T>(ulong guestCr3, ulong guestVa, out T value, bool verbose = false) where T : struct
	{
		value = default;
		int num = ((typeof(T) == typeof(bool)) ? 1 : Marshal.SizeOf<T>());
		byte[] array = new byte[num];
		if (!ReadGuestUser(guestCr3, guestVa, array, num, verbose))
		{
			return false;
		}
		if (typeof(T) == typeof(bool))
		{
			value = (T)(object)(array[0] != 0);
			return true;
		}
		GCHandle gCHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
		try
		{
			value = Marshal.PtrToStructure<T>(gCHandle.AddrOfPinnedObject());
			return true;
		}
		finally
		{
			gCHandle.Free();
		}
	}

	public bool WriteGuestUser(ulong guestCr3, ulong guestVa, byte[] buffer, int size, bool verbose = false)
	{
		int num3;
		for (int i = 0; i < size; i += num3)
		{
			ulong num = guestVa + (ulong)i;
			ulong num2 = num & 0xFFF;
			num3 = Math.Min(size - i, (int)(4096 - num2));
			ulong? num4 = TranslateGuestVa(guestCr3, num, verbose);
			if (!num4.HasValue)
			{
				return false;
			}
			ulong? num5 = MapPhysicalToHost(num4.Value, verbose);
			if (!num5.HasValue)
			{
				return false;
			}
			if (!WriteHostBytesAt(num5.Value, buffer, num3, i))
			{
				return false;
			}
		}
		return true;
	}

	public bool WriteGuestUser<T>(ulong guestCr3, ulong guestVa, T value, bool verbose = false) where T : struct
	{
		if (typeof(T) == typeof(bool))
		{
			byte[] buffer = new byte[1] { ((bool)(object)value) ? ((byte)1) : ((byte)0) };
			return WriteGuestUser(guestCr3, guestVa, buffer, 1, verbose);
		}
		int num = Marshal.SizeOf<T>();
		byte[] array = new byte[num];
		GCHandle gCHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
		try
		{
			Marshal.StructureToPtr(value, gCHandle.AddrOfPinnedObject(), fDeleteOld: false);
		}
		finally
		{
			gCHandle.Free();
		}
		return WriteGuestUser(guestCr3, guestVa, array, num, verbose);
	}

	public ulong? TranslateGuestVa(ulong guestCr3, ulong guestVa, bool verbose = false)
	{
		ulong num = guestVa >> 47;
		if (num != 0L && num != 131071)
		{
			return null;
		}
		ulong num2 = guestCr3 & 0xFFFFFFFFFF000L;
		ulong item = guestVa & 0xFFFFFFFFFFFFF000uL;
		lock (cacheLock)
		{
			if (gvaToPhysicalPageCache.TryGetValue((num2, item), out var value))
			{
				return value | (guestVa & 0xFFF);
			}
		}
		if (!ReadGuestPhysical<ulong>(num2 + ((guestVa >> 39) & 0x1FF) * 8, out var value2, verbose) || (value2 & 1) == 0L)
		{
			return null;
		}
		ulong num3 = value2 & 0xFFFFFFFFFF000L;
		if (!ReadGuestPhysical<ulong>(num3 + ((guestVa >> 30) & 0x1FF) * 8, out var value3, verbose) || (value3 & 1) == 0L)
		{
			return null;
		}
		if ((value3 & 0x80) != 0L)
		{
			ulong num4 = (value3 & 0xFFFFFFFE00000L) | (guestVa & 0x3FFFFFFF);
			lock (cacheLock)
			{
				gvaToPhysicalPageCache[(num2, item)] = num4 & 0xFFFFFFFFFFFFF000uL;
			}
			return num4;
		}
		ulong num5 = value3 & 0xFFFFFFFFFF000L;
		if (!ReadGuestPhysical<ulong>(num5 + ((guestVa >> 21) & 0x1FF) * 8, out var value4, verbose) || (value4 & 1) == 0L)
		{
			return null;
		}
		if ((value4 & 0x80) != 0L)
		{
			ulong num6 = (value4 & 0xFFFFFFFE00000L) | (guestVa & 0x1FFFFF);
			lock (cacheLock)
			{
				gvaToPhysicalPageCache[(num2, item)] = num6 & 0xFFFFFFFFFFFFF000uL;
			}
			return num6;
		}
		ulong num7 = value4 & 0xFFFFFFFFFF000L;
		if (!ReadGuestPhysical<ulong>(num7 + ((guestVa >> 12) & 0x1FF) * 8, out var value5, verbose) || (value5 & 1) == 0L)
		{
			return null;
		}
		ulong num8 = (value5 & 0xFFFFFFFFFF000L) | (guestVa & 0xFFF);
		lock (cacheLock)
		{
			gvaToPhysicalPageCache[(num2, item)] = num8 & 0xFFFFFFFFFFFFF000uL;
		}
		return num8;
	}

	public ulong? ReadMmCr3(ulong mmVa, bool verbose = false)
	{
		if (!ReadGuestKernel<ulong>(mmVa + 80, out var value, verbose) || value == 0L)
		{
			return null;
		}
		if (!TryConvertKernelVirtualToPhysical(value, out var outPhys))
		{
			return null;
		}
		return outPhys;
	}

	public static bool TryConvertKernelVirtualToPhysical(ulong kernelVa, out ulong outPhys)
	{
		outPhys = 0uL;
		if (kernelVa >= 18446612682070032384uL && kernelVa < 18446683050814210048uL)
		{
			outPhys = kernelVa - 18446612682070032384uL;
			return true;
		}
		if (kernelVa >= 18446612132314218496uL && kernelVa < 18446682501058396160uL)
		{
			outPhys = kernelVa - 18446612132314218496uL;
			return true;
		}
		if (kernelVa >= 18446744071562067968uL && kernelVa < 18446744073692774400uL)
		{
			outPhys = kernelVa - 18446744071562067968uL;
			return true;
		}
		return false;
	}
}



}
