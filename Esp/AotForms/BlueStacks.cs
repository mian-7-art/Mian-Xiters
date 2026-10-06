using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace AotForms;

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
		Memory.Destroy();
		for (int i = 1; i <= 20; i++)
		{
			onAttempt?.Invoke(i, 20);
			Memory.ClearCr3();
			if (Memory.Setup(verbose) && (FindTask(packageName, out taskValue, verbose) || (packageName == "com.dts.freefireth" && FindTask("com.dts.freefiremax", out taskValue, verbose) && (packageName = "com.dts.freefiremax") != null)))
			{
				guestCr3 = Memory.GetCr3(taskValue.Mm);
				if (guestCr3 != 0L)
				{
					Memory.SetCr3(guestCr3);
					return true;
				}
			}
			Memory.Destroy();
			if (i < 20)
			{
				Thread.Sleep(500);
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
		Memory.Destroy();
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
		if (!Memory.ReadKernel<ulong>(taskValue.Mm, out value, verbose))
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
			if (!Memory.ReadKernel<ulong>(value, out value2, verbose) || !Memory.ReadKernel<ulong>(value + 16, out value3, verbose) || !Memory.ReadKernel<ulong>(value + 160, out value4, verbose))
			{
				break;
			}
			if (value4 != 0L)
			{
				bool flag = MatchesLibraryName(ReadFileName(value4, verbose), moduleName);
				if (flag && num == 0L)
				{
					num = value4;
				}
				if ((flag || (num != 0L && value4 == num)) && !list2.Contains(value2))
				{
					list2.Add(value2);
				}
			}
			if (value3 == 0L || value3 == value)
			{
				break;
			}
			value = value3;
		}
		if (list2.Count < mapIndex)
		{
			return 0uL;
		}
		return list2[mapIndex - 1];
	}

	private bool ReadTask(ulong task, out Task outTask, bool verbose)
	{
		outTask = new Task
		{
			TaskAddr = task
		};
		if (!Memory.ReadKernel<ulong>(task + 1216, out outTask.Mm, verbose) || !Memory.ReadKernel<uint>(task + 1392, out outTask.Pid, verbose) || !Memory.ReadKernel<uint>(task + 1396, out outTask.Tgid, verbose))
		{
			return false;
		}
		outTask.Comm = ReadTaskComm(task, verbose);
		if (string.IsNullOrEmpty(outTask.Comm))
		{
			return false;
		}
		if (outTask.Mm != 0L && Utility.IsLikelyGuestKernelPointer(outTask.Mm))
		{
			ulong cr = Memory.GetCr3(outTask.Mm);
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
		if (!Memory.ReadKernel(task + 1824, array, 16, verbose))
		{
			return "";
		}
		return SanitizeString(Encoding.ASCII.GetString(array).TrimEnd('\0'));
	}

	private string ReadTaskCmdline(ulong mm, ulong guestCr3, bool verbose)
	{
		ulong value = 0uL;
		ulong value2 = 0uL;
		if (!Memory.ReadKernel<ulong>(mm + 312, out value, verbose) || !Memory.ReadKernel<ulong>(mm + 320, out value2, verbose) || value == 0L || value2 == 0L || value2 <= value)
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
		if (!Memory.ReadUser(guestCr3, value, array, (int)num, verbose))
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
		if (!Memory.ReadKernel(address, array, maxLength, verbose))
		{
			return "";
		}
		return SanitizeString(Encoding.UTF8.GetString(array).TrimEnd('\0'));
	}

	private string ReadDentryName(ulong dentry, bool verbose)
	{
		byte[] array = new byte[128];
		if (!Memory.ReadKernel(dentry, array, 128, verbose))
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
		if (!Memory.ReadKernel(file, array, 128, verbose))
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

	private int ScoreTask(Task task)
	{
		if (string.IsNullOrEmpty(packageName))
		{
			return 1;
		}
		int num = 0;
		if (!string.IsNullOrEmpty(task.Cmdline))
		{
			if (task.Cmdline.Equals(packageName, StringComparison.OrdinalIgnoreCase))
			{
				num = Math.Max(num, 100);
			}
			else if (task.Cmdline.IndexOf(packageName, StringComparison.OrdinalIgnoreCase) >= 0)
			{
				num = Math.Max(num, 90);
			}
			else if (packageName.IndexOf(task.Cmdline, StringComparison.OrdinalIgnoreCase) >= 0)
			{
				num = Math.Max(num, 80);
			}
		}
		if (task.Comm.Equals(packageName, StringComparison.OrdinalIgnoreCase))
		{
			num = Math.Max(num, 60);
		}
		else if (task.Comm.Length > 0 && packageName.StartsWith(task.Comm, StringComparison.OrdinalIgnoreCase))
		{
			num = Math.Max(num, 50);
		}
		if (task.Pid == task.Tgid && num > 0)
		{
			num++;
		}
		return num;
	}

	private bool FindTask(string packageName, out Task outTask, bool verbose)
	{
		outTask = default;
		ulong initTasksHead = 18446744071597804592uL;
		ulong value = 0uL;
		ulong value2 = 0uL;
		if (!Memory.ReadKernel<ulong>(initTasksHead, out value, verbose) || !Memory.ReadKernel<ulong>(initTasksHead + 8, out value2, verbose))
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
				if (ReadTask(task, out var outTask2, verbose) && outTask2.Mm != 0L && Utility.IsLikelyGuestKernelPointer(outTask2.Mm))
				{
					int num2 = ScoreTask(outTask2);
					if (num2 > bestScore)
					{
						bestScore = num2;
						bestTask = outTask2;
					}
				}
				ulong value3 = 0uL;
				int num3 = (reverse ? 8 : 0);
				if (!Memory.ReadKernel<ulong>(num + (ulong)num3, out value3) || value3 == 0L || value3 == num)
				{
					break;
				}
				num = value3;
			}
		};
		action(value, arg2: false);
		action(value2, arg2: true);
		if (bestScore == 0 || bestTask.Mm == 0L || !Utility.IsLikelyGuestKernelPointer(bestTask.Mm))
		{
			return false;
		}
		outTask = bestTask;
		return true;
	}

	private static bool MatchesLibraryName(string fileName, string wanted)
	{
		if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(wanted))
		{
			return false;
		}
		return fileName.Equals(wanted, StringComparison.OrdinalIgnoreCase);
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
