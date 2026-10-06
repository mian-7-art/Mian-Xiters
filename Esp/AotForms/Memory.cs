using System.Runtime.InteropServices;
using System.Text;

namespace AotForms;

public static class Memory
{
	private static MemoryEngine engine = new MemoryEngine();

	private static ulong guestCr3;

	public static bool IsValid => engine.IsInitialized;

	public static uint ProcessId => engine.ProcessId;

	public static ulong Pvm => engine.Pvm;

	public static ulong Pvcpu => engine.Pvcpu;

	public static bool Setup(bool verbose = false)
	{
		return engine.Initialize(verbose);
	}

	public static void Destroy()
	{
		engine.Shutdown();
		guestCr3 = 0uL;
	}

	public static void SetCr3(ulong value)
	{
		guestCr3 = value;
	}

	public static void ClearCr3()
	{
		guestCr3 = 0uL;
	}

	public static void ClearCaches()
	{
		engine.ClearCaches();
	}

	public static ulong GetActiveCr3()
	{
		return guestCr3;
	}

	public static bool Read<T>(ulong address, out T value, bool verbose = false) where T : struct
	{
		value = default;
		if (!IsValid)
		{
			return false;
		}
		try
		{
			if (guestCr3 != 0L && !Utility.IsLikelyGuestKernelPointer(address))
			{
				return engine.ReadGuestUser<T>(guestCr3, address, out value, verbose);
			}
			if (Utility.IsLikelyGuestKernelPointer(address))
			{
				return engine.ReadGuestKernel<T>(address, out value, verbose);
			}
			return engine.ReadGuestPhysical<T>(address, out value, verbose);
		}
		catch
		{
			return false;
		}
	}

	public static T Read<T>(ulong address, bool verbose = false) where T : struct
	{
		Read<T>(address, out var value, verbose);
		return value;
	}

	public static bool Read(ulong address, byte[] buffer, int size, bool verbose = false)
	{
		if (!IsValid)
		{
			return false;
		}
		try
		{
			if (guestCr3 != 0L && !Utility.IsLikelyGuestKernelPointer(address))
			{
				return engine.ReadGuestUser(guestCr3, address, buffer, size, verbose);
			}
			if (Utility.IsLikelyGuestKernelPointer(address))
			{
				return engine.ReadGuestKernel(address, buffer, size, verbose);
			}
			return engine.ReadGuestPhysical(address, buffer, size, verbose);
		}
		catch
		{
			return false;
		}
	}

	public static bool ReadKernel<T>(ulong address, out T value, bool verbose = false) where T : struct
	{
		value = default;
		if (!IsValid)
		{
			return false;
		}
		try
		{
			return engine.ReadGuestKernel<T>(address, out value, verbose);
		}
		catch
		{
			return false;
		}
	}

	public static bool ReadKernel(ulong address, byte[] buffer, int size, bool verbose = false)
	{
		if (!IsValid)
		{
			return false;
		}
		try
		{
			return engine.ReadGuestKernel(address, buffer, size, verbose);
		}
		catch
		{
			return false;
		}
	}

	public static bool ReadUser(ulong cr3, ulong address, byte[] buffer, int size, bool verbose = false)
	{
		if (!IsValid)
		{
			return false;
		}
		try
		{
			return engine.ReadGuestUser(cr3, address, buffer, size, verbose);
		}
		catch
		{
			return false;
		}
	}

	public static string ReadString(ulong address, int size, bool unicode = true)
	{
		if (!IsValid || size <= 0)
		{
			return "";
		}
		byte[] array = new byte[size];
		if (!Read(address, array, size))
		{
			return "";
		}
		string text = (unicode ? Encoding.Unicode.GetString(array) : Encoding.Default.GetString(array));
		int num = text.IndexOf('\0');
		if (num >= 0)
		{
			text = text.Substring(0, num);
		}
		return text;
	}

	public static bool Write<T>(ulong address, T value) where T : struct
	{
		if (!IsValid)
		{
			return false;
		}
		try
		{
			if (guestCr3 != 0L && !Utility.IsLikelyGuestKernelPointer(address))
			{
				return engine.WriteGuestUser(guestCr3, address, value);
			}
			if (Utility.IsLikelyGuestKernelPointer(address))
			{
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
				return engine.WriteGuestPhysical(address, array, num);
			}
			return engine.WriteGuestPhysical(address, value);
		}
		catch
		{
			return false;
		}
	}

	public static ulong GetCr3(ulong mm)
	{
		ulong? num = engine.ReadMmCr3(mm);
		if (!num.HasValue)
		{
			return 0uL;
		}
		return num.Value;
	}
}
