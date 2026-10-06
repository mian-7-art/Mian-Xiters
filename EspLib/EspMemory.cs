using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Aimbot.Mem;

namespace EspLib
{
    /// <summary>
    /// Static memory-read/write facade supporting both hypervisor guest physical memory engine
    /// and automatic Windows Process Memory API fallback for universal emulator compatibility.
    /// </summary>
    public static class EspMemory
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadProcessMemory(IntPtr hProcess, ulong lpBaseAddress, byte[] lpBuffer, int nSize, out IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteProcessMemory(IntPtr hProcess, ulong lpBaseAddress, byte[] lpBuffer, int nSize, out IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hObject);

        private const uint PROCESS_ALL_ACCESS = 0x1F0FFF;
        private const uint PROCESS_VM_READ    = 0x0010;
        private const uint PROCESS_VM_WRITE   = 0x0020;
        private const uint PROCESS_VM_OPERATION = 0x0008;

        private static EspMemoryEngine engine = new EspMemoryEngine();
        private static ulong guestCr3;
        private static IntPtr fallbackHandle = IntPtr.Zero;
        private static int fallbackPid = 0;

        public static bool IsValid => engine.IsInitialized || (fallbackHandle != IntPtr.Zero && fallbackHandle != (IntPtr)(-1));
        public static uint ProcessId => engine.IsInitialized ? engine.ProcessId : (uint)fallbackPid;
        public static ulong Pvm => engine.Pvm;
        public static ulong Pvcpu => engine.Pvcpu;
        public static string LastInitError => engine.LastInitError;

        public static bool Setup(bool verbose = false)
        {
            bool ok = engine.Initialize(verbose);
            if (!ok)
            {
                SetupProcessFallback();
            }
            return IsValid;
        }

        public static bool SetupProcessFallback()
        {
            try
            {
                var (procName, pid) = GetProcess.GetRunningEmulatorProcess();
                if (pid > 0)
                {
                    if (fallbackHandle != IntPtr.Zero && fallbackHandle != (IntPtr)(-1))
                    {
                        CloseHandle(fallbackHandle);
                    }
                    fallbackPid = pid;
                    fallbackHandle = OpenProcess(PROCESS_ALL_ACCESS, false, pid);
                    if (fallbackHandle == IntPtr.Zero)
                    {
                        fallbackHandle = OpenProcess(PROCESS_VM_READ | PROCESS_VM_WRITE | PROCESS_VM_OPERATION, false, pid);
                    }
                    return fallbackHandle != IntPtr.Zero && fallbackHandle != (IntPtr)(-1);
                }
            }
            catch { }
            return false;
        }

        public static void Destroy()
        {
            engine.Shutdown();
            guestCr3 = 0uL;
            if (fallbackHandle != IntPtr.Zero && fallbackHandle != (IntPtr)(-1))
            {
                try { CloseHandle(fallbackHandle); } catch { }
                fallbackHandle = IntPtr.Zero;
            }
            fallbackPid = 0;
        }

        public static void SetCr3(ulong value) { guestCr3 = value; }
        public static void ClearCr3() { guestCr3 = 0uL; }
        public static void ClearCaches() { engine.ClearCaches(); }
        public static ulong GetActiveCr3() { return guestCr3; }

        public static bool Read<T>(ulong address, out T value, bool verbose = false) where T : struct
        {
            value = default;
            if (!IsValid) return false;
            try
            {
                if (engine.IsInitialized)
                {
                    if (guestCr3 != 0L && !EspUtility.IsLikelyGuestKernelPointer(address))
                        return engine.ReadGuestUser<T>(guestCr3, address, out value, verbose);
                    if (EspUtility.IsLikelyGuestKernelPointer(address))
                        return engine.ReadGuestKernel<T>(address, out value, verbose);
                    return engine.ReadGuestPhysical<T>(address, out value, verbose);
                }

                if (fallbackHandle != IntPtr.Zero)
                {
                    int size = Marshal.SizeOf<T>();
                    byte[] buffer = new byte[size];
                    if (ReadProcessMemory(fallbackHandle, address, buffer, size, out _))
                    {
                        GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                        try
                        {
                            value = Marshal.PtrToStructure<T>(handle.AddrOfPinnedObject());
                            return true;
                        }
                        finally { handle.Free(); }
                    }
                }
            }
            catch { return false; }
            return false;
        }

        public static T Read<T>(ulong address, bool verbose = false) where T : struct
        {
            Read<T>(address, out var value, verbose);
            return value;
        }

        public static bool Read(ulong address, byte[] buffer, int size, bool verbose = false)
        {
            if (!IsValid || buffer == null || size <= 0) return false;
            try
            {
                if (engine.IsInitialized)
                {
                    if (guestCr3 != 0L && !EspUtility.IsLikelyGuestKernelPointer(address))
                        return engine.ReadGuestUser(guestCr3, address, buffer, size, verbose);
                    if (EspUtility.IsLikelyGuestKernelPointer(address))
                        return engine.ReadGuestKernel(address, buffer, size, verbose);
                    return engine.ReadGuestPhysical(address, buffer, size, verbose);
                }

                if (fallbackHandle != IntPtr.Zero)
                {
                    return ReadProcessMemory(fallbackHandle, address, buffer, size, out _);
                }
            }
            catch { return false; }
            return false;
        }

        public static string ReadString(ulong address, int size, bool unicode = true)
        {
            if (!IsValid || size <= 0) return "";
            byte[] array = new byte[size];
            if (!Read(address, array, size)) return "";
            string text = (unicode ? Encoding.Unicode.GetString(array) : Encoding.Default.GetString(array));
            int num = text.IndexOf('\0');
            if (num >= 0) text = text.Substring(0, num);
            return text;
        }

        public static bool Write<T>(ulong address, T value) where T : struct
        {
            if (!IsValid) return false;
            try
            {
                if (engine.IsInitialized)
                {
                    if (guestCr3 != 0L && !EspUtility.IsLikelyGuestKernelPointer(address))
                        return engine.WriteGuestUser(guestCr3, address, value);
                    if (EspUtility.IsLikelyGuestKernelPointer(address))
                    {
                        int num = Marshal.SizeOf<T>();
                        byte[] array = new byte[num];
                        GCHandle gCHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
                        try { Marshal.StructureToPtr(value, gCHandle.AddrOfPinnedObject(), fDeleteOld: false); }
                        finally { gCHandle.Free(); }
                        return engine.WriteGuestPhysical(address, array, num);
                    }
                    return engine.WriteGuestPhysical(address, value);
                }

                if (fallbackHandle != IntPtr.Zero)
                {
                    int num = Marshal.SizeOf<T>();
                    byte[] array = new byte[num];
                    GCHandle handle = GCHandle.Alloc(array, GCHandleType.Pinned);
                    try { Marshal.StructureToPtr(value, handle.AddrOfPinnedObject(), fDeleteOld: false); }
                    finally { handle.Free(); }
                    return WriteProcessMemory(fallbackHandle, address, array, num, out _);
                }
            }
            catch { return false; }
            return false;
        }

        public static ulong GetCr3(ulong mm)
        {
            if (!engine.IsInitialized) return 0uL;
            ulong? num = engine.ReadMmCr3(mm);
            if (!num.HasValue) return 0uL;
            return num.Value;
        }

        public static bool ReadKernel<T>(ulong address, out T value, bool verbose = false) where T : struct
        {
            value = default;
            if (!engine.IsInitialized) return false;
            try { return engine.ReadGuestKernel<T>(address, out value, verbose); }
            catch { return false; }
        }

        public static bool ReadKernel(ulong address, byte[] buffer, int size, bool verbose = false)
        {
            if (!engine.IsInitialized) return false;
            try { return engine.ReadGuestKernel(address, buffer, size, verbose); }
            catch { return false; }
        }

        public static bool ReadUser(ulong cr3, ulong address, byte[] buffer, int size, bool verbose = false)
        {
            if (engine.IsInitialized)
            {
                try { return engine.ReadGuestUser(cr3, address, buffer, size, verbose); }
                catch { return false; }
            }
            return Read(address, buffer, size, verbose);
        }

        public static bool ReadUser<T>(ulong cr3, ulong address, out T value, bool verbose = false) where T : struct
        {
            if (engine.IsInitialized)
            {
                value = default;
                try { return engine.ReadGuestUser<T>(cr3, address, out value, verbose); }
                catch { return false; }
            }
            return Read<T>(address, out value, verbose);
        }
    }
}
