using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace EspLib
{

public class BlueStacks
{
	private static class TaskOffset
	{
		public const int Mm = 1216;

		public const int Tasks = 1136;

		public const int Pid = 1392;

		public const int Tgid = 1396;

		public const int Comm = 1824;
	}

	private static class ListHeadOffset
	{
		public const int Next = 0;

		public const int Prev = 8;
	}

	private static class MmOffset
	{
		public const int Mmap = 0;

		public const int ArgStart = 312;

		public const int ArgEnd = 320;
	}

	private static class VmaOffset
	{
		public const int VmStart = 0;

		public const int VmNext = 16;

		public const int VmFile = 160;
	}

	private static class DentryOffset
	{
		public const int DName = 32;

		public const int DNameLen = 36;

		public const int DNameName = 40;

		public const int DIname = 56;
	}

	private struct GuestListHead
	{
		public ulong Next;

		public ulong Prev;
	}

	private struct GuestQstr
	{
		public uint Hash;

		public uint Len;

		public ulong Name;
	}

	private struct GuestPath
	{
		public ulong Mnt;

		public ulong Dentry;
	}

	private struct GuestFile
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public byte[] Pad0;

		public GuestPath FPath;
	}

	private struct GuestDentry
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
		public byte[] Pad0;

		public ulong DParent;

		public GuestQstr DName;

		public ulong DInode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		public byte[] DIname;
	}

	private struct Task
	{
		public ulong TaskAddr;

		public ulong Mm;

		public uint Pid;

		public uint Tgid;

		public string Comm;

		public string Cmdline;
	}

	private string packageName;

	private Task taskValue;

	private ulong guestCr3;

	private const ulong InitTask = 18446744071597803456uL;

	public uint Pid => taskValue.Pid;

	public bool Setup(string pkg, bool verbose = false, Action<int, int> onAttempt = null)
	{
		packageName = pkg;
		taskValue = default;
		guestCr3 = 0uL;
		EspMemory.Destroy();

		string[] targetPackages = new[]
		{
			pkg,
			"com.dts.freefireth",
			"com.dts.freefiremax",
			"com.dts.freefirevn",
			"com.dts.freefiretw",
			"com.dts.freefirein",
			"com.dts.freefire"
		};

		for (int i = 1; i <= 3; i++)
		{
			onAttempt?.Invoke(i, 3);
			EspMemory.ClearCr3();
			if (EspMemory.Setup(verbose))
			{
				foreach (var target in targetPackages)
				{
					if (string.IsNullOrEmpty(target)) continue;
					if (FindTask(target, out taskValue, verbose))
					{
						packageName = target;
						guestCr3 = EspMemory.GetCr3(taskValue.Mm);
						if (guestCr3 != 0L)
						{
							EspMemory.SetCr3(guestCr3);
							return true;
						}
					}
				}
			}
			EspMemory.Destroy();
			if (i < 3)
			{
				Thread.Sleep(100);
			}
		}
		packageName = null;
		taskValue = default;
		guestCr3 = 0uL;
		return false;
	}

	public void Destroy()
	{
		packageName = null;
		taskValue = default;
		guestCr3 = 0uL;
		EspMemory.Destroy();
	}

	public ulong GetCr3()
	{
		return guestCr3;
	}

	public string GetTag()
	{
		if (string.IsNullOrEmpty(packageName))
		{
			return "FFTH";
		}
		if (packageName.IndexOf("max", StringComparison.OrdinalIgnoreCase) >= 0)
		{
			return "FFMAX";
		}
		return "FFTH";
	}

	public string GetGameTitle()
	{
		if (string.IsNullOrEmpty(packageName))
		{
			return "FREE FIRE TH";
		}
		if (packageName.IndexOf("max", StringComparison.OrdinalIgnoreCase) >= 0)
		{
			return "FREE FIRE MAX";
		}
		return "FREE FIRE TH";
	}

	public ulong GetModuleBase(string moduleName, int mapIndex = 1, bool verbose = false)
	{
		if (taskValue.Mm == 0L || guestCr3 == 0L || mapIndex == 0)
		{
			return 0uL;
		}
		ulong value = 0uL;
		if (!EspMemory.ReadKernel<ulong>(taskValue.Mm, out value, verbose))
		{
			return 0uL;
		}
		List<ulong> list = new List<ulong>();
		List<ulong> list2 = new List<ulong>();
		ulong num = 0uL;
		while (value != 0L && list.Count < 8192 && !list.Contains(value))
		{
			list.Add(value);
			ulong value2 = 0uL;
			ulong value3 = 0uL;
			ulong value4 = 0uL;
			if (!EspMemory.ReadKernel<ulong>(value, out value2, verbose) || !EspMemory.ReadKernel<ulong>(value + 16, out value3, verbose))
			{
				break;
			}

			// Probe multiple Linux kernel vm_area_struct vm_file offsets (0x80 to 0xB0)
			int[] vmFileOffsets = new int[] { 160, 144, 152, 168, 176, 128, 136 };
			foreach (int offset in vmFileOffsets)
			{
				if (EspMemory.ReadKernel<ulong>(value + (ulong)offset, out value4, verbose) && value4 != 0L)
				{
					string fName = ReadFileName(value4, verbose);
					if (!string.IsNullOrEmpty(fName))
					{
						bool flag = MatchesLibraryName(fName, moduleName);
						if (flag && num == 0L)
						{
							num = value4;
						}
						if ((flag || (num != 0L && value4 == num)) && !list2.Contains(value2))
						{
							list2.Add(value2);
							break;
						}
					}
				}
			}

			if (value3 == 0L || value3 == value)
			{
				break;
			}
			value = value3;
		}
		if (list2.Count >= mapIndex)
		{
			return list2[mapIndex - 1];
		}

		if (moduleName.IndexOf("il2cpp", StringComparison.OrdinalIgnoreCase) >= 0)
		{
			ulong probedBase = FindIl2CppBaseByHeaderProbe(verbose);
			if (probedBase != 0uL)
			{
				return probedBase;
			}
		}

		return 0uL;
	}

	private ulong FindIl2CppBaseByHeaderProbe(bool verbose = false)
	{
		if (taskValue.Mm == 0L || guestCr3 == 0L) return 0uL;

		ulong vma = 0uL;
		if (!EspMemory.ReadKernel<ulong>(taskValue.Mm, out vma, verbose) || vma == 0L) return 0uL;

		List<ulong> seenVmas = new List<ulong>();
		ulong candidateElfBase = 0uL;

		while (vma != 0L && seenVmas.Count < 8192 && !seenVmas.Contains(vma))
		{
			seenVmas.Add(vma);
			ulong vmStart = 0uL;
			ulong vmNext = 0uL;
			if (!EspMemory.ReadKernel<ulong>(vma, out vmStart, verbose) ||
			    !EspMemory.ReadKernel<ulong>(vma + 16, out vmNext, verbose))
			{
				break;
			}

			if (vmStart != 0L && (vmStart & 0xFFFuL) == 0uL && vmStart > 0x10000000uL)
			{
				if (EspMemory.ReadUser<uint>(guestCr3, vmStart, out uint magic, verbose) && magic == 0x464C457F)
				{
					if (EspOffsets.InitBase != 0)
					{
						if (EspMemory.ReadUser<ulong>(guestCr3, vmStart + EspOffsets.InitBase, out ulong initPtr, verbose) && initPtr != 0L)
						{
							return vmStart;
						}
					}

					if (candidateElfBase == 0uL)
					{
						candidateElfBase = vmStart;
					}
				}
			}

			if (vmNext == 0L || vmNext == vma) break;
			vma = vmNext;
		}

		return candidateElfBase;
	}

	private bool ReadTask(ulong task, out Task outTask, bool verbose)
	{
		outTask = new Task
		{
			TaskAddr = task
		};
		if (!EspMemory.ReadKernel<ulong>(task + 1216, out outTask.Mm, verbose) || !EspMemory.ReadKernel<uint>(task + 1392, out outTask.Pid, verbose) || !EspMemory.ReadKernel<uint>(task + 1396, out outTask.Tgid, verbose))
		{
			return false;
		}
		outTask.Comm = ReadTaskComm(task, verbose);
		if (string.IsNullOrEmpty(outTask.Comm))
		{
			return false;
		}
		if (outTask.Mm != 0L && EspUtility.IsLikelyGuestKernelPointer(outTask.Mm))
		{
			ulong cr = EspMemory.GetCr3(outTask.Mm);
			if (cr != 0L)
			{
				outTask.Cmdline = ReadTaskCmdline(outTask.Mm, cr, verbose);
			}
		}
		return true;
	}

	private string ReadTaskComm(ulong task, bool verbose)
	{
		byte[] array = new byte[16];
		if (!EspMemory.ReadKernel(task + 1824, array, 16, verbose))
		{
			return "";
		}
		return SanitizeString(Encoding.ASCII.GetString(array).TrimEnd('\0'));
	}

	private string ReadTaskCmdline(ulong mm, ulong guestCr3, bool verbose)
	{
		ulong value = 0uL;
		ulong value2 = 0uL;
		if (!EspMemory.ReadKernel<ulong>(mm + 312, out value, verbose) || !EspMemory.ReadKernel<ulong>(mm + 320, out value2, verbose) || value == 0L || value2 == 0L || value2 <= value)
		{
			return "";
		}
		ulong num = value2 - value;
		if (num > 8192)
		{
			num = 8192uL;
		}
		if (num == 0L)
		{
			return "";
		}
		byte[] array = new byte[num];
		if (!EspMemory.ReadUser(guestCr3, value, array, (int)num, verbose))
		{
			return "";
		}
		return SanitizeString(Encoding.ASCII.GetString(array).TrimEnd('\0'));
	}

	private string ReadKernelString(ulong address, int maxLength, bool verbose)
	{
		if (address == 0L || maxLength == 0)
		{
			return "";
		}
		byte[] array = new byte[maxLength + 1];
		if (!EspMemory.ReadKernel(address, array, maxLength, verbose))
		{
			return "";
		}
		return SanitizeString(Encoding.UTF8.GetString(array).TrimEnd('\0'));
	}

	private string ReadDentryName(ulong dentry, bool verbose)
	{
		byte[] array = new byte[128];
		if (!EspMemory.ReadKernel(dentry, array, 128, verbose))
		{
			return "";
		}
		uint num = BitConverter.ToUInt32(array, 36);
		ulong num2 = BitConverter.ToUInt64(array, 40);
		if (num != 0 && num <= 256 && num2 != 0L)
		{
			string text = ReadKernelString(num2, (int)num, verbose);
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
		}
		byte[] array2 = new byte[32];
		Array.Copy(array, 56, array2, 0, 32);
		return SanitizeString(Encoding.ASCII.GetString(array2).TrimEnd('\0'));
	}

	private string ReadFileName(ulong file, bool verbose)
	{
		byte[] array = new byte[128];
		if (!EspMemory.ReadKernel(file, array, 128, verbose))
		{
			return "";
		}
		ulong num = BitConverter.ToUInt64(array, 24);
		if (num == 0L)
		{
			return "";
		}
		return ReadDentryName(num, verbose);
	}

	private int ScoreTask(Task task, string targetPkg)
	{
		if (string.IsNullOrEmpty(targetPkg))
		{
			return 1;
		}
		int num = 0;
		if (!string.IsNullOrEmpty(task.Cmdline))
		{
			if (task.Cmdline.Equals(targetPkg, StringComparison.OrdinalIgnoreCase))
			{
				num = Math.Max(num, 100);
			}
			else if (task.Cmdline.IndexOf(targetPkg, StringComparison.OrdinalIgnoreCase) >= 0)
			{
				num = Math.Max(num, 90);
			}
			else if (targetPkg.IndexOf(task.Cmdline, StringComparison.OrdinalIgnoreCase) >= 0)
			{
				num = Math.Max(num, 80);
			}
		}
		if (task.Comm.Equals(targetPkg, StringComparison.OrdinalIgnoreCase))
		{
			num = Math.Max(num, 60);
		}
		else if (task.Comm.Length > 0 && targetPkg.StartsWith(task.Comm, StringComparison.OrdinalIgnoreCase))
		{
			num = Math.Max(num, 50);
		}
		else if (!string.IsNullOrEmpty(task.Comm) && (task.Comm.IndexOf("freefire", StringComparison.OrdinalIgnoreCase) >= 0 || task.Comm.IndexOf("dts", StringComparison.OrdinalIgnoreCase) >= 0 || task.Comm.IndexOf("ff", StringComparison.OrdinalIgnoreCase) >= 0))
		{
			num = Math.Max(num, 75);
		}
		if (task.Pid == task.Tgid && num > 0)
		{
			num++;
		}
		return num;
	}

	private bool FindTask(string targetPkg, out Task outTask, bool verbose)
	{
		outTask = default;
		ulong initTasksHead = 18446744071597804592uL;
		ulong value = 0uL;
		ulong value2 = 0uL;
		if (!EspMemory.ReadKernel<ulong>(initTasksHead, out value, verbose) || !EspMemory.ReadKernel<ulong>(initTasksHead + 8, out value2, verbose))
		{
			return false;
		}
		List<ulong> seenHeads = new List<ulong>();
		Task bestTask = default;
		int bestScore = 0;
		Action<ulong, bool> action = (ulong startHead, bool reverse) =>
		{
			ulong num = startHead;
			while (num != 0L && num != initTasksHead && seenHeads.Count < 4096 && num >= 1136 && !seenHeads.Contains(num))
			{
				seenHeads.Add(num);
				ulong task = num - 1136;
				if (ReadTask(task, out var outTask2, verbose) && outTask2.Mm != 0L && EspUtility.IsLikelyGuestKernelPointer(outTask2.Mm))
				{
					int num2 = ScoreTask(outTask2, targetPkg);
					if (num2 > bestScore)
					{
						bestScore = num2;
						bestTask = outTask2;
					}
				}
				ulong value3 = 0uL;
				int num3 = (reverse ? 8 : 0);
				if (!EspMemory.ReadKernel<ulong>(num + (ulong)num3, out value3) || value3 == 0L || value3 == num)
				{
					break;
				}
				num = value3;
			}
		};
		action(value, arg2: false);
		action(value2, arg2: true);
		if (bestScore == 0 || bestTask.Mm == 0L || !EspUtility.IsLikelyGuestKernelPointer(bestTask.Mm))
		{
			return false;
		}
		outTask = bestTask;
		return true;
	}

	private static bool MatchesLibraryName(string fileName, string wanted)
	{
		if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(wanted)) return false;

		string cleanFile = fileName.Trim('\0', ' ', '\t', '\r', '\n');
		string cleanWanted = wanted.Trim('\0', ' ', '\t', '\r', '\n');

		if (cleanFile.Length == 0 || cleanWanted.Length == 0) return false;

		// 1. Direct contains check (matches full path, partial path, APK-embedded paths like base.apk!libil2cpp.so)
		if (cleanFile.IndexOf(cleanWanted, StringComparison.OrdinalIgnoreCase) >= 0) return true;

		// 2. Strip extension (.so) and check
		string wantedNoExt = cleanWanted.EndsWith(".so", StringComparison.OrdinalIgnoreCase)
			? cleanWanted.Substring(0, cleanWanted.Length - 3)
			: cleanWanted;

		if (cleanFile.IndexOf(wantedNoExt, StringComparison.OrdinalIgnoreCase) >= 0) return true;

		// 3. Check for core keyword (e.g. "il2cpp", "unity")
		if (wantedNoExt.StartsWith("lib", StringComparison.OrdinalIgnoreCase) && wantedNoExt.Length > 3)
		{
			string coreName = wantedNoExt.Substring(3);
			if (cleanFile.IndexOf(coreName, StringComparison.OrdinalIgnoreCase) >= 0) return true;
		}

		return false;
	}

	private static string SanitizeString(string input)
	{
		if (string.IsNullOrEmpty(input))
		{
			return input;
		}
		StringBuilder stringBuilder = new StringBuilder(input.Length);
		foreach (char c in input)
		{
			if (c >= ' ' && c < '\u007f')
			{
				stringBuilder.Append(c);
			}
		}
		return stringBuilder.ToString();
	}
}



}
