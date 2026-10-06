using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace MOZIPRIME
{
    public class mozih4x : IDisposable
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);

        // ntdll direct - zero FPS drop (bypasses kernel32 overhead)
        [DllImport("ntdll.dll", SetLastError = true)]
        static extern int NtReadVirtualMemory(IntPtr ProcessHandle, IntPtr BaseAddress, byte[] Buffer, uint NumberOfBytesToRead, out uint NumberOfBytesRead);

        [DllImport("ntdll.dll", SetLastError = true)]
        static extern int NtWriteVirtualMemory(IntPtr ProcessHandle, IntPtr BaseAddress, byte[] Buffer, uint NumberOfBytesToWrite, out uint NumberOfBytesWritten);

        [DllImport("ntdll.dll", EntryPoint = "NtReadVirtualMemory", SetLastError = true)]
        static extern unsafe int NtReadVirtualMemoryRaw(IntPtr ProcessHandle, IntPtr BaseAddress, void* Buffer, uint NumberOfBytesToRead, out uint NumberOfBytesRead);

        [DllImport("ntdll.dll", EntryPoint = "NtWriteVirtualMemory", SetLastError = true)]
        static extern unsafe int NtWriteVirtualMemoryRaw(IntPtr ProcessHandle, IntPtr BaseAddress, void* Buffer, uint NumberOfBytesToWrite, out uint NumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);

        const uint PROCESS_ALL_ACCESS     = 0x1F0FFF;
        const uint MEM_COMMIT             = 0x1000;
        const uint PAGE_NOACCESS          = 0x01;
        const uint PAGE_READONLY          = 0x02;
        const uint PAGE_READWRITE         = 0x04;
        const uint PAGE_WRITECOPY         = 0x08;
        const uint PAGE_EXECUTE           = 0x10;
        const uint PAGE_EXECUTE_READ      = 0x20;
        const uint PAGE_EXECUTE_READWRITE = 0x40;
        const uint PAGE_EXECUTE_WRITECOPY = 0x80;
        const uint PAGE_GUARD             = 0x100;

        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORY_BASIC_INFORMATION
        {
            public IntPtr  BaseAddress;
            public IntPtr  AllocationBase;
            public uint    AllocationProtect;
            public UIntPtr RegionSize;
            public uint    State;
            public uint    Protect;
            public uint    Type;
        }

        public IntPtr ProcessHandle { get; private set; }
        public bool   IsOpen        { get; private set; }
        public int    ProcessId     { get; private set; }

        private bool _disposed = false;

        // Per-instance lock for thread-safety on concurrent feature scans
        private readonly object _lock = new object();

        public mozih4x()
        {
            ProcessHandle = IntPtr.Zero;
            IsOpen = false;
        }

        // ── Process attach ──────────────────────────────────────────
        public bool SetProcess(string processName)
        {
            // Close previous handle first — prevents kernel handle leak on repeated SetProcess calls
            if (ProcessHandle != IntPtr.Zero)
            {
                CloseHandle(ProcessHandle);
                ProcessHandle = IntPtr.Zero;
                IsOpen = false;
            }
            var procs = Process.GetProcessesByName(processName);
            if (procs == null || procs.Length == 0) return false;

            // Pick the process instance with the highest WorkingSet64 (main emulator VM with highest RAM allocation)
            var proc = procs.OrderByDescending(p => {
                try { return p.WorkingSet64; } catch { return 0L; }
            }).FirstOrDefault();

            if (proc == null) return false;
            ProcessId     = proc.Id;
            ProcessHandle = OpenProcess(PROCESS_ALL_ACCESS, false, proc.Id);
            IsOpen        = ProcessHandle != IntPtr.Zero;
            return IsOpen;
        }

        public bool SetProcess(string[] processNames)
        {
            foreach (var name in processNames)
                if (SetProcess(name)) return true;
            return false;
        }

        // ── Read ────────────────────────────────────────────────────
        public byte[] ReadMemory(ulong address, int size)
        {
            if (!IsOpen) return null;
            byte[] buffer = new byte[size];
            uint bytesRead;
            NtReadVirtualMemory(ProcessHandle, (IntPtr)address, buffer, (uint)size, out bytesRead);
            return bytesRead == size ? buffer : null;
        }

        public byte[] ReadMemory(long address, int size) => ReadMemory((ulong)address, size);
        public byte[] ReadBytes(long address, int length) => ReadMemory((ulong)address, length);
        public byte[] ReadBytes(ulong address, int length) => ReadMemory(address, length);

        public unsafe int ReadInt(ulong address)
        {
            if (!IsOpen || address <= 0x10000 || address > 0x7FFFFFFFFFFF) return 0;
            int val = 0;
            uint bytesRead;
            int status = NtReadVirtualMemoryRaw(ProcessHandle, (IntPtr)address, &val, 4, out bytesRead);
            return (status == 0 && bytesRead == 4) ? val : 0;
        }

        public unsafe int ReadInt(long address) => ReadInt((ulong)address);

        // ── Write ───────────────────────────────────────────────────
        public unsafe bool WriteInt(ulong address, int value)
        {
            if (!IsOpen || address <= 0x10000 || address > 0x7FFFFFFFFFFF) return false;
            uint bytesWritten;
            int status = NtWriteVirtualMemoryRaw(ProcessHandle, (IntPtr)address, &value, 4, out bytesWritten);
            return status == 0 && bytesWritten == 4;
        }

        public unsafe bool WriteInt(long address, int value) => WriteInt((ulong)address, value);

        public bool WriteBytes(long address, byte[] data) => WriteMemory((ulong)address, data);
        public bool WriteBytes(ulong address, byte[] data) => WriteMemory(address, data);

        public bool WriteMemory(ulong address, byte[] data)
        {
            if (!IsOpen) return false;
            uint oldProtect = 0;
            VirtualProtectEx(ProcessHandle, (IntPtr)address, (UIntPtr)data.Length, PAGE_EXECUTE_READWRITE, out oldProtect);
            uint bytesWritten;
            int status = NtWriteVirtualMemory(ProcessHandle, (IntPtr)address, data, (uint)data.Length, out bytesWritten);
            if (oldProtect != 0)
            {
                uint temp;
                VirtualProtectEx(ProcessHandle, (IntPtr)address, (UIntPtr)data.Length, oldProtect, out temp);
            }
            return (status == 0 && bytesWritten == data.Length) || bytesWritten == data.Length;
        }

        public bool WriteMemory(long address, byte[] data) => WriteMemory((ulong)address, data);

        public bool AobReplace(ulong address, string bytePattern)
        {
            try
            {
                var parts = bytePattern.Split(' ');
                byte[] bytes = parts.Select(x => byte.Parse(x, NumberStyles.HexNumber)).ToArray();
                return WriteMemory(address, bytes);
            }
            catch { return false; }
        }

        public bool AobReplace(long address, string bytePattern) => AobReplace((ulong)address, bytePattern);

        public void ClearScanCache()
        {
            // No-op: read cache removed to prevent stale memory values. Kept for API compatibility.
        }

        // ── AoB Scan (sync, background-friendly) ────────────────────
        // Region limit: 50MB max (same as zero-FPS-drop reference)
        public unsafe List<long> AoBScanSync(string pattern)
        {
            var results      = new List<long>();
            byte[] patBytes  = ParsePattern(pattern);
            byte[] patMask   = ParseMask(pattern);
            uint   mbiSize   = (uint)Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION));
            IntPtr address   = IntPtr.Zero;

            int patLength = patBytes.Length;

            fixed (byte* pPat = patBytes, pMask = patMask)
            {
                while (VirtualQueryEx(ProcessHandle, address, out var mbi, mbiSize) != 0)
                {
                    if (mbi.State == MEM_COMMIT &&
                        (mbi.Protect & PAGE_GUARD) == 0 &&
                        (mbi.Protect & PAGE_NOACCESS) == 0 &&
                        (mbi.Protect == PAGE_READWRITE ||
                         mbi.Protect == PAGE_EXECUTE_READWRITE ||
                         mbi.Protect == PAGE_WRITECOPY ||
                         mbi.Protect == PAGE_EXECUTE_WRITECOPY ||
                         mbi.Protect == PAGE_EXECUTE_READ ||
                         mbi.Protect == PAGE_EXECUTE ||
                         mbi.Protect == PAGE_READONLY))
                    {
                        ulong regionSizeRaw = mbi.RegionSize.ToUInt64();
                        // Guard: skip regions larger than 200MB to prevent OOM and int overflow on 64-bit emulators
                        if (regionSizeRaw > 0 && regionSizeRaw < 200UL * 1024 * 1024)
                        {
                            int regionSize = (int)regionSizeRaw;
                            byte[] buffer = new byte[regionSize];
                            uint bytesRead;
                            NtReadVirtualMemory(ProcessHandle, mbi.BaseAddress, buffer, (uint)regionSize, out bytesRead);

                            if (bytesRead > 0)
                            {
                                int max = (int)bytesRead - patLength;
                                fixed (byte* pBuf = buffer)
                                {
                                    for (int i = 0; i <= max; i++)
                                    {
                                        bool found = true;
                                        byte* pCurrentBuf = pBuf + i;
                                        for (int j = 0; j < patLength; j++)
                                        {
                                            if (pMask[j] == 0xFF && pCurrentBuf[j] != pPat[j])
                                            {
                                                found = false;
                                                break;
                                            }
                                        }
                                        if (found)
                                        {
                                            results.Add((long)mbi.BaseAddress + i);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    ulong currentBase = (ulong)mbi.BaseAddress.ToInt64();
                    ulong regionSizeVal = mbi.RegionSize.ToUInt64();
                    ulong nextAddr = currentBase + regionSizeVal;
                    if (nextAddr <= currentBase) break;
                    address = (IntPtr)nextAddr;
                }
            }
            return results;
        }

        // Async wrapper - runs scan in background thread (UI stays responsive)
        public async Task<IEnumerable<long>> AoBScan(string pattern)
        {
            return await Task.Run(() => (IEnumerable<long>)AoBScanSync(pattern));
        }

        // ── Pattern helpers ─────────────────────────────────────────
        byte[] ParsePattern(string pattern)
        {
            var parts = pattern.Split(' ');
            byte[] result = new byte[parts.Length];
            for (int i = 0; i < parts.Length; i++)
                result[i] = parts[i] == "??" ? (byte)0 : Convert.ToByte(parts[i], 16);
            return result;
        }

        byte[] ParseMask(string pattern)
        {
            var parts = pattern.Split(' ');
            byte[] result = new byte[parts.Length];
            for (int i = 0; i < parts.Length; i++)
                result[i] = parts[i] == "??" ? (byte)0 : (byte)0xFF;
            return result;
        }

        // ── Dispose ─────────────────────────────────────────────────
        public void Dispose()
        {
            if (!_disposed)
            {
                if (ProcessHandle != IntPtr.Zero)
                {
                    CloseHandle(ProcessHandle);
                    ProcessHandle = IntPtr.Zero;
                }
                _disposed = true;
                IsOpen = false;
            }
        }

        ~mozih4x() => Dispose();
    }

    [Flags]
    public enum ProcessAccessFlags
    {
        AllAccess = 0x001F0FFF
    }
}
