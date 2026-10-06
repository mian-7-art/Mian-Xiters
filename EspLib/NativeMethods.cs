using System.Runtime.InteropServices;

namespace EspLib
{

internal static class NativeMethods
{
	public struct MEMORY_BASIC_INFORMATION
	{
		public nuint BaseAddress;

		public nuint AllocationBase;

		public uint AllocationProtect;

		public nuint RegionSize;

		public uint State;

		public uint Protect;

		public uint Type;
	}

	public struct PROCESS_BASIC_INFORMATION
	{
		public int ExitStatus;

		public nuint PebBaseAddress;

		public nuint AffinityMask;

		public int BasePriority;

		public nuint UniqueProcessId;

		public nuint InheritedFromUniqueProcessId;
	}

	public struct UNICODE_STRING
	{
		public ushort Length;

		public ushort MaximumLength;

		public nint Buffer;
	}

	public struct LIST_ENTRY
	{
		public nuint Flink;

		public nuint Blink;
	}

	public const uint PROCESS_QUERY_LIMITED_INFORMATION = 4096u;

	public const int ProcessCommandLineInformation = 60;

	public const uint PROCESS_QUERY_INFORMATION = 1024u;

	public const uint PROCESS_VM_READ = 16u;

	public const uint PROCESS_VM_WRITE = 32u;

	public const uint PROCESS_VM_OPERATION = 8u;

	public const uint PROCESS_CREATE_THREAD = 2u;

	public const uint MEM_COMMIT = 4096u;

	public const uint MEM_RESERVE = 8192u;

	public const uint MEM_RELEASE = 32768u;

	public const uint PAGE_READWRITE = 4u;

	public const uint PAGE_EXECUTE_READWRITE = 64u;

	public const uint PAGE_READONLY = 2u;

	public const uint INFINITE = uint.MaxValue;

	[DllImport("kernel32.dll", SetLastError = true)]
	public static extern bool ReadProcessMemory(nint hProcess, nuint lpBaseAddress, byte[] lpBuffer, nuint nSize, out nuint lpNumberOfBytesRead);

	[DllImport("kernel32.dll", SetLastError = true)]
	public static extern bool ReadProcessMemory(nint hProcess, nuint lpBaseAddress, nint lpBuffer, nuint nSize, out nuint lpNumberOfBytesRead);

	[DllImport("kernel32.dll", SetLastError = true)]
	public static extern bool WriteProcessMemory(nint hProcess, nuint lpBaseAddress, byte[] lpBuffer, nuint nSize, out nuint lpNumberOfBytesWritten);

	[DllImport("kernel32.dll", SetLastError = true)]
	public static extern bool WriteProcessMemory(nint hProcess, nuint lpBaseAddress, nint lpBuffer, nuint nSize, out nuint lpNumberOfBytesWritten);

	[DllImport("kernel32.dll", SetLastError = true)]
	public static extern nint OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

	[DllImport("kernel32.dll", SetLastError = true)]
	public static extern bool CloseHandle(nint hObject);

	[DllImport("kernel32.dll", SetLastError = true)]
	public static extern uint VirtualQueryEx(nint hProcess, nuint lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, nuint dwLength);

	[DllImport("kernel32.dll", SetLastError = true)]
	public static extern nuint VirtualAllocEx(nint hProcess, nuint lpAddress, nuint dwSize, uint flAllocationType, uint flProtect);

	[DllImport("kernel32.dll", SetLastError = true)]
	public static extern bool VirtualFreeEx(nint hProcess, nuint lpAddress, nuint dwSize, uint dwFreeType);

	[DllImport("kernel32.dll", SetLastError = true)]
	public static extern nint CreateRemoteThread(nint hProcess, nint lpThreadAttributes, nuint dwStackSize, nint lpStartAddress, nuint lpParameter, uint dwCreationFlags, out nint lpThreadId);

	[DllImport("kernel32.dll", SetLastError = true)]
	public static extern uint WaitForSingleObject(nint hHandle, uint dwMilliseconds);

	[DllImport("kernel32.dll", SetLastError = true)]
	public static extern bool GetExitCodeThread(nint hThread, out uint lpExitCode);

	[DllImport("user32.dll")]
	public static extern uint GetWindowThreadProcessId(nint hWnd, out uint lpdwProcessId);

	[DllImport("ntdll.dll")]
	public static extern int NtQuerySystemInformation(int informationClass, nint informationBuffer, int informationLength, out int returnLength);

	[DllImport("ntdll.dll")]
	public static extern int NtQueryInformationProcess(nint processHandle, int processInformationClass, out PROCESS_BASIC_INFORMATION processInformation, int processInformationLength, out int returnLength);

	[DllImport("ntdll.dll")]
	public static extern int NtQueryInformationProcess(nint processHandle, int processInformationClass, nint processInformation, int processInformationLength, out int returnLength);
}


}
