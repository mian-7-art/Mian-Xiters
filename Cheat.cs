using Aimbot.Mem;
using MOZIPRIME;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Cheat
    {
        #region Win32

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttribute, IntPtr dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, uint dwFreeType);

        private const uint MEM_RELEASE = 0x00008000;

        #endregion

        #region Aimbot Keybind System & Heartbeat Watcher

        public static int AimbotHeadKey { get; set; } = 0;
        public static int AimbotV2Key   { get; set; } = 0;
        public static int AimbotMianKey { get; set; } = 0;

        public static event Action<string> OnAimbotKeyToggled;

        private static readonly object _aimbotLock = new object();
        private static bool _aimbotWatcherRunning = false;
        private static bool _aimbotKeyCooldown = false;
        private static bool _heartbeatRunning = false;

        public static void StartAimbotKeyWatcher()
        {
            lock (_aimbotLock)
            {
                if (!_aimbotWatcherRunning)
                {
                    _aimbotWatcherRunning = true;
                    _ = AimbotKeyWatcher();
                }
            }
        }

        private static async Task AimbotKeyWatcher()
        {
            while (_aimbotWatcherRunning)
            {
                await Task.Delay(20);
                if (_aimbotKeyCooldown) continue;

                int headKey  = AimbotHeadKey;
                int v2Key    = AimbotV2Key;
                int mianKey  = AimbotMianKey;

                if (headKey > 0 && (GetAsyncKeyState(headKey) & 0x8000) != 0)
                {
                    _aimbotKeyCooldown = true;
                    try { OnAimbotKeyToggled?.Invoke("Head"); } catch { }
                    await Task.Delay(350);
                    _aimbotKeyCooldown = false;
                }
                else if (v2Key > 0 && (GetAsyncKeyState(v2Key) & 0x8000) != 0)
                {
                    _aimbotKeyCooldown = true;
                    try { OnAimbotKeyToggled?.Invoke("V2"); } catch { }
                    await Task.Delay(350);
                    _aimbotKeyCooldown = false;
                }
                else if (mianKey > 0 && (GetAsyncKeyState(mianKey) & 0x8000) != 0)
                {
                    _aimbotKeyCooldown = true;
                    try { OnAimbotKeyToggled?.Invoke("Mian"); } catch { }
                    await Task.Delay(350);
                    _aimbotKeyCooldown = false;
                }
                else if ((GetAsyncKeyState(0x74) & 0x8000) != 0) // F5 Key: Instant ESP Refresh & Cache Flush
                {
                    _aimbotKeyCooldown = true;
                    try { EspLib.EspManager.RefreshEsp(); } catch { }
                    await Task.Delay(350);
                    _aimbotKeyCooldown = false;
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
                    _ = AimbotHeartbeatLoop();
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
            while (_heartbeatRunning)
            {
                await Task.Delay(500);

                var listHead = _writeList;
                var listV2   = _writeListV2;

                bool hasHead = listHead != null && listHead.Length > 0;
                bool hasV2   = listV2 != null && listV2.Length > 0;

                if (!hasHead && !hasV2)
                {
                    _heartbeatRunning = false;
                    break;
                }

                // Check emulator process health: if emulator process closed, exit watcher
                if (!moziii.IsOpen && !_mozV2.IsOpen)
                {
                    _heartbeatRunning = false;
                    break;
                }

                // SAFE LOCK RE-ENFORCEMENT (Zero Crash across CS, Rooms, BR Ranked)
                // Write ONLY if the address currently holds w.offVal (game reverted it, so safe to re-patch).
                // If it holds w.onVal: already patched, no write needed.
                // If it holds 0 or anything else: entity is dead or memory reallocated for new round/object -> NEVER TOUCH IT.
                try
                {
                    if (hasHead)
                    {
                        foreach (var w in listHead)
                        {
                            if (w.addr > 0x1000)
                            {
                                int current = moziii.ReadInt(w.addr);
                                if (current == w.offVal)
                                {
                                    moziii.WriteInt(w.addr, w.onVal);
                                }
                            }
                        }
                    }
                    if (hasV2)
                    {
                        foreach (var w in listV2)
                        {
                            if (w.addr > 0x1000)
                            {
                                int current = _mozV2.ReadInt(w.addr);
                                if (current == w.offVal)
                                {
                                    _mozV2.WriteInt(w.addr, w.onVal);
                                }
                            }
                        }
                    }
                }
                catch { }
            }
        }

        #endregion

        #region Aimbot Head

        private static string aimAob = "FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ?? ?? ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ?? ?? ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 00 00 A5 43";
        private static long ReadOffset = 0xB9;
        private static long WriteOffset = 0xB5;
        private static readonly string[] TaskName = new string[] { "HD-Player", "Bluestacks", "Bluestacks-5", "HD-Agent", "HdPlayerService", "BstkSVC", "LdBoxHeadless", "dnplayer", "NoxVMHandle", "MEmuHeadless" };
        private static mozih4x moziii = new mozih4x();

        private static (long addr, int onVal, int offVal)[] _writeList = new (long, int, int)[0];

        public static void RestoreMemoryValues()
        {
            StopAimbotHeartbeatWatcher();
            var list = _writeList;
            lock (_aimbotLock)
            {
                _writeList = new (long, int, int)[0];
            }
            if (list != null && list.Length > 0)
            {
                foreach (var w in list)
                {
                    try
                    {
                        // Only restore if address still holds the onVal pointer (prevents corrupting reallocated memory in CS/Rooms/Lobby)
                        int cur = moziii.ReadInt(w.addr);
                        if (cur == w.onVal)
                        {
                            moziii.WriteInt(w.addr, w.offVal);
                        }
                    }
                    catch { }
                }
            }
        }

        public static async Task<string> ScanHeadAsync()
        {
            string emulator = GetProcess.GetRunningEmulatorProcessName();
            if (!moziii.SetProcess(new string[] { emulator ?? "HD-Player" }))
                return "Emulator Not Found!! Open Emulator First";

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
                    DiagnosticLogger.LogResult("Aimbot Head", aimAob, false, 0, $"ReadOffset=0x{ReadOffset:X}, WriteOffset=0x{WriteOffset:X}", "AoBScan found 0 matches in emulator memory");
                    return "Pattern Not Found";
                }

                var list = new List<(long, int, int)>();
                await Task.Run(() =>
                {
                    foreach (long addr in addresses)
                    {
                        long readAddr  = addr + ReadOffset;
                        long writeAddr = addr + WriteOffset;
                        int origRead  = moziii.ReadInt(readAddr);
                        int origWrite = moziii.ReadInt(writeAddr);

                        if (origRead != 0 && origWrite != 0 &&
                            (uint)origRead > 0x1000 && (uint)origWrite > 0x1000)
                        {
                            list.Add((writeAddr, origRead,  origWrite));
                            list.Add((readAddr,  origWrite, origRead));
                        }
                    }
                });

                if (list.Count == 0)
                {
                    DiagnosticLogger.LogResult("Aimbot Head", aimAob, false, addresses.Count, $"ReadOffset=0x{ReadOffset:X}, WriteOffset=0x{WriteOffset:X}", "Addresses found but entity pointers were invalid");
                    return "Pattern Not Found";
                }

                lock (_aimbotLock)
                {
                    _writeList = list.ToArray();
                }
                moziii.ClearScanCache();
                DiagnosticLogger.LogResult("Aimbot Head", aimAob, true, list.Count, $"ReadOffset=0x{ReadOffset:X}, WriteOffset=0x{WriteOffset:X}");
                return "Pattern Found";
            }
            catch (Exception ex)
            {
                DiagnosticLogger.LogResult("Aimbot Head", aimAob, false, 0, $"ReadOffset=0x{ReadOffset:X}, WriteOffset=0x{WriteOffset:X}", ex.Message);
                return $"Scan Error: {ex.Message}";
            }
        }

        public static async Task<string> EnableAimbotHead()
        {
            string r = await ScanHeadAsync();
            if (r != "Pattern Found") return r;

            var list = _writeList;
            await Task.Run(() =>
            {
                if (list != null)
                {
                    foreach (var w in list)
                    {
                        try { moziii.WriteInt(w.addr, w.onVal); } catch { }
                    }
                }
            });
            StartAimbotHeartbeatWatcher();
            SoundEffects.PlayEnableSound();
            return "Aimbot Head ON";
        }

        #endregion

        #region Aimbot V2

        private static string aimV2Aob = "FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 00 00 A5 43";
        private static long ReadOffset1 = 0xBA;
        private static long ReadOffset2 = 0xBC;
        private static long WriteOffset1 = 0xB6;
        private static long WriteOffset2 = 0xB8;

        private static readonly mozih4x _mozV2 = new mozih4x();

        private static (long addr, int onVal, int offVal)[] _writeListV2 = new (long, int, int)[0];

        public static void RestoreAimbotV2Values()
        {
            StopAimbotHeartbeatWatcher();
            var list = _writeListV2;
            lock (_aimbotLock)
            {
                _writeListV2 = new (long, int, int)[0];
            }
            if (list != null && list.Length > 0)
            {
                foreach (var w in list)
                {
                    try
                    {
                        // Only restore if address still holds the onVal pointer (prevents corrupting reallocated memory in CS/Rooms/Lobby)
                        int cur = _mozV2.ReadInt(w.addr);
                        if (cur == w.onVal)
                        {
                            _mozV2.WriteInt(w.addr, w.offVal);
                        }
                    }
                    catch { }
                }
            }
        }

        public static async Task<string> ScanAimbotV2Async()
        {
            string emulator = GetProcess.GetRunningEmulatorProcessName();
            if (!_mozV2.SetProcess(new string[] { emulator ?? "HD-Player" }))
                return "Emulator Not Found!! Open Emulator First";

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
                    DiagnosticLogger.LogResult("Aimbot V2", aimV2Aob, false, 0, $"Read1=0x{ReadOffset1:X}, Write1=0x{WriteOffset1:X}, Read2=0x{ReadOffset2:X}, Write2=0x{WriteOffset2:X}");
                    return "Pattern Not Found";
                }

                var list = new List<(long, int, int)>();
                await Task.Run(() =>
                {
                    foreach (long addr in addresses)
                    {
                        // Pair 1: READ 0xBA, WRITE 0xB6
                        long readAddr1  = addr + ReadOffset1;
                        long writeAddr1 = addr + WriteOffset1;
                        int origRead1  = _mozV2.ReadInt(readAddr1);
                        int origWrite1 = _mozV2.ReadInt(writeAddr1);

                        if (origRead1 != 0 && origWrite1 != 0 &&
                            (uint)origRead1 > 0x1000 && (uint)origWrite1 > 0x1000)
                        {
                            list.Add((writeAddr1, origRead1,  origWrite1));
                            list.Add((readAddr1,  origWrite1, origRead1));
                        }

                        // Pair 2: READ 0xBC, WRITE 0xB8
                        long readAddr2  = addr + ReadOffset2;
                        long writeAddr2 = addr + WriteOffset2;
                        int origRead2  = _mozV2.ReadInt(readAddr2);
                        int origWrite2 = _mozV2.ReadInt(writeAddr2);

                        if (origRead2 != 0 && origWrite2 != 0 &&
                            (uint)origRead2 > 0x1000 && (uint)origWrite2 > 0x1000)
                        {
                            list.Add((writeAddr2, origRead2,  origWrite2));
                            list.Add((readAddr2,  origWrite2, origRead2));
                        }
                    }
                });

                if (list.Count == 0)
                {
                    DiagnosticLogger.LogResult("Aimbot V2", aimV2Aob, false, addresses.Count, $"Read1=0x{ReadOffset1:X}, Write1=0x{WriteOffset1:X}, Read2=0x{ReadOffset2:X}, Write2=0x{WriteOffset2:X}", "Addresses found but entity pointers were invalid");
                    return "Pattern Not Found";
                }

                lock (_aimbotLock)
                {
                    _writeListV2 = list.ToArray();
                }
                _mozV2.ClearScanCache();
                DiagnosticLogger.LogResult("Aimbot V2", aimV2Aob, true, list.Count, $"Read1=0x{ReadOffset1:X}, Write1=0x{WriteOffset1:X}, Read2=0x{ReadOffset2:X}, Write2=0x{WriteOffset2:X}");
                return "Pattern Found";
            }
            catch (Exception ex)
            {
                DiagnosticLogger.LogResult("Aimbot V2", aimV2Aob, false, 0, $"Read1=0x{ReadOffset1:X}, Write1=0x{WriteOffset1:X}, Read2=0x{ReadOffset2:X}, Write2=0x{WriteOffset2:X}", ex.Message);
                return $"Scan Error: {ex.Message}";
            }
        }

        public static async Task<string> EnableAimbotV2()
        {
            string r = await ScanAimbotV2Async();
            if (r != "Pattern Found") return r;

            var list = _writeListV2;
            await Task.Run(() =>
            {
                if (list != null)
                {
                    foreach (var w in list)
                    {
                        try { _mozV2.WriteInt(w.addr, w.onVal); } catch { }
                    }
                }
            });
            StartAimbotHeartbeatWatcher();
            SoundEffects.PlayEnableSound();
            return "Aimbot V2 ON";
        }

        #endregion

        #region Aimbot 100% (Ultra-Fast Dual-Layer Head Lock)

        public static void RestoreAimbotMianValues()
        {
            RestoreMemoryValues();
            RestoreAimbotV2Values();
        }

        public static async Task<string> EnableAimbotMian()
        {
            // Step 1: Scan BOTH patterns on RAW entity memory before writing
            string resHead = await ScanHeadAsync();
            string resV2   = await ScanAimbotV2Async();

            var listHead = _writeList;
            var listV2   = _writeListV2;

            if ((listHead == null || listHead.Length == 0) && (listV2 == null || listV2.Length == 0))
            {
                RestoreAimbotMianValues();
                return "Lobby/Plane: Land First!";
            }

            // Step 2: Write both memory locks together simultaneously safely
            await Task.Run(() =>
            {
                if (listHead != null && listHead.Length > 0)
                {
                    foreach (var w in listHead)
                    {
                        try { moziii.WriteInt(w.addr, w.onVal); } catch { }
                    }
                }

                if (listV2 != null && listV2.Length > 0)
                {
                    foreach (var w in listV2)
                    {
                        try { _mozV2.WriteInt(w.addr, w.onVal); } catch { }
                    }
                }
            });

            StartAimbotHeartbeatWatcher();
            SoundEffects.PlayEnableSound();
            return "Aimbot 100% ON";
        }

        #endregion

        #region Sniper Scope (Dynamic Patcher)

        private static readonly string ScopeAob     = "FF FF FF FF 08 00 00 00 00 00 60 40 CD CC 8C 3F 8F C2 F5 3C CD CC CC 3D 06 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 33 33 13 40 00 00 B0 3F 00 00 80 3F 01";
        private static readonly string ScopeReplace = "FF FF FF FF 08 00 00 00 00 00 60 40 CD CC 8C 3F 8F C2 F5 3C CD CC CC 3D 06 00 00 00 00 00 19 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00";
        private static IEnumerable<long> _sniperScope1 = new List<long>();
        private static readonly mozih4x _scopeMem = new mozih4x();
        private static bool _scopeWatcherRunning = false;
        private static bool _scopeCooldown = false;

        public static async Task<string> ScanSniperScopeAsync()
        {
            string emulator = GetProcess.GetRunningEmulatorProcessName();
            if (!_scopeMem.SetProcess(new string[] { emulator ?? "HD-Player" }))
                return "Emulator Not Found";

            _sniperScope1 = await _scopeMem.AoBScan(ScopeAob);
            if (_sniperScope1 == null || !_sniperScope1.Any())
                return "No Pattern Found";

            return "Sniper Scope Ready";
        }

        public static async Task<string> EnableSniperScope()
        {
            if (!_sniperScope1.Any())
            {
                string r = await ScanSniperScopeAsync();
                if (r != "Sniper Scope Ready") return r;
            }
            SoundEffects.PlayEnableSound();
            if (!_scopeWatcherRunning)
            {
                _scopeWatcherRunning = true;
                _ = SniperAiWatching();
            }
            return "Sniper Scope ON";
        }

        public static void StopSniperScope() => _scopeWatcherRunning = false;

        private static async Task SniperAiWatching()
        {
            while (_scopeWatcherRunning)
            {
                await Task.Delay(20);
                bool pressed =
                    (GetAsyncKeyState(0x05) & 0x8000) != 0 ||
                    (GetAsyncKeyState(0x06) & 0x8000) != 0 ||
                    (GetAsyncKeyState(0x56) & 0x8000) != 0 ||
                    (GetAsyncKeyState(0x01) & 0x8000) != 0 ||
                    (GetAsyncKeyState(0x54) & 0x8000) != 0 ||
                    (GetAsyncKeyState(0x45) & 0x8000) != 0 ||
                    (GetAsyncKeyState(0x51) & 0x8000) != 0 ||
                    (GetAsyncKeyState(0x04) & 0x8000) != 0;

                if (pressed && !_scopeCooldown)
                {
                    _scopeCooldown = true;
                    _ = SniperBrilerControl();
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
            catch { }
        }

        private static void ReapplySniperScope()
        {
            if (_sniperScope1 == null) return;
            foreach (var addr in _sniperScope1)
                _scopeMem.AobReplace(addr, ScopeReplace);
        }

        private static void RestoreSniperScope()
        {
            if (_sniperScope1 == null) return;
            foreach (var addr in _sniperScope1)
                _scopeMem.AobReplace(addr, ScopeAob);
        }

        #endregion

        #region Sniper Switch

        private static List<long> _sniperSwitchCache = new List<long>();
        private static readonly mozih4x _switchMem = new mozih4x();

        public static async Task<string> ScanSniperSwitchAsync()
        {
            string emulator = GetProcess.GetRunningEmulatorProcessName();
            if (!_switchMem.SetProcess(new string[] { emulator ?? "HD-Player" }))
                return "Emulator Not Found";

            var matches = await _switchMem.AoBScan("3F 00 00 80 3E 00 00 00 00 04 00 00 00 00 00 80 3F 00 00 20 41 00 00 34 42 01 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F");
            if (matches == null || !matches.Any())
                return "No Pattern Found";

            _sniperSwitchCache = matches.ToList();
            return "Sniper Switch Ready";
        }

        public static async Task<string> EnableSniperSwitch()
        {
            if (_sniperSwitchCache.Count == 0)
            {
                string r = await ScanSniperSwitchAsync();
                if (r != "Sniper Switch Ready") return r;
            }
            await Task.Run(() =>
            {
                foreach (var addr in _sniperSwitchCache)
                    try { _switchMem.AobReplace(addr, "01 00 00 80 00 00 00 00 00 04 00 00 00 00 00 80 3F 00 00 20 41 00 00 34 42 01 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F"); } catch { }
            });
            SoundEffects.PlayEnableSound();
            return "Sniper Switch ON";
        }

        #endregion

        #region Chams (MIAN XITERS.DLL Injection)

        private static bool _chamsInjected = false;

        public static async Task<string> EnableChams()
        {
            // Guard: DLL already injected into this session — re-injecting causes multiple instances
            // which corrupt shared vertex/bone buffers and produce the stretching glitch shown.
            if (_chamsInjected) return "Chams Injected";

            try
            {
                string procName = GetProcess.GetRunningEmulatorProcessName();
                if (procName == null) return "Emulator Not Running";

                // Extract DLL from embedded resources
                string dllPath = Path.Combine(Path.GetTempPath(), "MIAN XITERS.dll");
                var asm = Assembly.GetExecutingAssembly();
                // Try possible resource names for the native inject DLL
                string resName = asm.GetManifestResourceNames()
                    .FirstOrDefault(n => n.EndsWith("MIAN XITERS.DLL", StringComparison.OrdinalIgnoreCase) ||
                                         n.EndsWith("MIAN_XITERS.DLL", StringComparison.OrdinalIgnoreCase) ||
                                         (n.IndexOf("MIAN", StringComparison.OrdinalIgnoreCase) >= 0 && n.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) && !n.EndsWith("AuthlyX.dll", StringComparison.OrdinalIgnoreCase)));

                if (resName == null) return "Resource Not Found";

                using (var stream = asm.GetManifestResourceStream(resName))
                using (var fs = new FileStream(dllPath, FileMode.Create))
                {
                    byte[] buf = new byte[stream.Length];
                    stream.Read(buf, 0, buf.Length);
                    fs.Write(buf, 0, buf.Length);
                }

                string result = await InjectDll(procName, dllPath);
                if (result == "OK")
                {
                    _chamsInjected = true;
                    SoundEffects.PlayEnableSound();
                    return "Chams Injected";
                }
                return result;
            }
            catch (Exception ex) { return $"Chams Error: {ex.Message}"; }
        }

        private static async Task<string> InjectDll(string processName, string dllPath)
        {
            return await Task.Run(() =>
            {
                var (pName, pid) = GetProcess.GetRunningEmulatorProcess();
                if (pid <= 0) return "Emulator Not Running";

                IntPtr hProcess = OpenProcess(0x1F0FFF, false, pid);
                if (hProcess == IntPtr.Zero) return "OpenProcess Failed";

                IntPtr alloc = IntPtr.Zero;
                try
                {
                    IntPtr loadLib = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryW");
                    byte[] pathBytes = System.Text.Encoding.Unicode.GetBytes(dllPath + "\0");
                    alloc = VirtualAllocEx(hProcess, IntPtr.Zero, (IntPtr)pathBytes.Length, 0x1000 | 0x2000, 0x04);
                    IntPtr written;
                    WriteProcessMemory(hProcess, alloc, pathBytes, (uint)pathBytes.Length, out written);
                    IntPtr hThread = CreateRemoteThread(hProcess, IntPtr.Zero, IntPtr.Zero, loadLib, alloc, 0, IntPtr.Zero);
                    if (hThread == IntPtr.Zero) return "Inject Failed";
                    WaitForSingleObject(hThread, 5000);
                    CloseHandle(hThread);
                    return "OK";
                }
                finally
                {
                    // Always free the allocated remote memory to prevent leaks after thread completes
                    if (alloc != IntPtr.Zero)
                        VirtualFreeEx(hProcess, alloc, IntPtr.Zero, MEM_RELEASE);
                    CloseHandle(hProcess);
                }
            });
        }

        #endregion

        #region Chams State Reset

        /// <summary>
        /// Resets chams injection state. Call when emulator restarts or user toggles Chams off.
        /// Allows re-injection into a fresh emulator process on next enable call.
        /// </summary>
        public static void ResetChamsState()
        {
            _chamsInjected = false;
        }

        #endregion

        #region Glitch Fire

        private static readonly string GlitchFireAob     = "90 C1 00 00 70 41 01 00 00 00 00 00 C0 3F 00 00 00 3F 00 00 80 3F";
        private static readonly string GlitchFireReplace = "90 C1 00 00 70 41 01 00 00 00 00 00 00 BF 00 00 00 3F 00 00 80 3F";
        private static List<long> _glitchFireAddresses = new List<long>();
        private static readonly mozih4x _glitchFireMem = new mozih4x();

        public static async Task<string> ScanGlitchFireAsync()
        {
            string emulator = GetProcess.GetRunningEmulatorProcessName();
            if (!_glitchFireMem.SetProcess(new string[] { emulator ?? "HD-Player" }))
                return "Emulator Not Found";

            var matches = await _glitchFireMem.AoBScan(GlitchFireAob);
            if (matches == null || !matches.Any())
            {
                var replacedMatches = await _glitchFireMem.AoBScan(GlitchFireReplace);
                if (replacedMatches == null || !replacedMatches.Any())
                    return "No Pattern Found";
                _glitchFireAddresses = replacedMatches.ToList();
            }
            else
            {
                _glitchFireAddresses = matches.ToList();
            }

            return "Glitch Fire Ready";
        }

        public static async Task<string> EnableGlitchFire()
        {
            if (_glitchFireAddresses.Count == 0)
            {
                string r = await ScanGlitchFireAsync();
                if (r != "Glitch Fire Ready") return r;
            }

            await Task.Run(() =>
            {
                foreach (var addr in _glitchFireAddresses)
                {
                    try { _glitchFireMem.AobReplace(addr, GlitchFireReplace); } catch { }
                }
            });

            return "Glitch Fire ON";
        }

        public static void DisableGlitchFire()
        {
            if (_glitchFireAddresses != null && _glitchFireAddresses.Count > 0)
            {
                foreach (var addr in _glitchFireAddresses)
                {
                    try { _glitchFireMem.AobReplace(addr, GlitchFireAob); } catch { }
                }
            }
            _glitchFireAddresses.Clear();
        }

        #endregion

        #region Fast Landing

        public static event Action<bool> OnFastLandingChanged;

        private static readonly string FastLandingAob     = "00 00 00 00 00 00 80 3F 00 00 00 00 00 00 00 00 00 00 80 BF 00 00 00 00 00 00 80 BF 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 00 00 00 00 00 00 00 00 80 BF 00 00 80 7F 00 00 80 7F 00 00 80 7F 00 00 80 FF";
        private static readonly string FastLandingReplace = "00 00 00 00 00 00 FF 41 00 00 00 00 00 00 00 00 00 00 80 BF 00 00 00 00 00 00 80 BF 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 00 00 00 00 00 00 00 00 80 BF 00 00 80 7F 00 00 80 7F 00 00 80 7F 00 00 80 FF";
        private static List<long> _fastLandingAddresses = new List<long>();
        private static readonly mozih4x _fastLandingMem = new mozih4x();
        private static bool _fastLandingWatcherRunning = false;
        private static bool _fastLandingActive = false;
        private static bool _fastLandingCooldown = false;
        public static int FastLandingKey { get; set; } = 0;

        public static void StartFastLandingWatcher()
        {
            if (!_fastLandingWatcherRunning)
            {
                _fastLandingWatcherRunning = true;
                _ = FastLandingKeyWatcher();
            }
        }

        public static async Task<string> ScanFastLandingAsync()
        {
            string emulator = GetProcess.GetRunningEmulatorProcessName();
            if (!_fastLandingMem.SetProcess(new string[] { emulator ?? "HD-Player" }))
                return "Emulator Not Found";

            var matches = await _fastLandingMem.AoBScan(FastLandingAob);
            if (matches == null || !matches.Any())
            {
                var replacedMatches = await _fastLandingMem.AoBScan(FastLandingReplace);
                if (replacedMatches == null || !replacedMatches.Any())
                    return "No Pattern Found";
                _fastLandingAddresses = replacedMatches.ToList();
            }
            else
            {
                _fastLandingAddresses = matches.ToList();
            }

            return "Fast Landing Ready";
        }

        public static async Task<string> EnableFastLanding()
        {
            if (_fastLandingAddresses.Count == 0)
            {
                string r = await ScanFastLandingAsync();
                if (r != "Fast Landing Ready") return r;
            }

            ApplyFastLanding(true);
            StartFastLandingWatcher();
            SoundEffects.PlayEnableSound();

            return "Fast Landing ON";
        }

        public static void DisableFastLanding()
        {
            ApplyFastLanding(false);
            _fastLandingAddresses.Clear();
        }

        public static void ApplyFastLanding(bool enable)
        {
            if (_fastLandingAddresses == null || _fastLandingAddresses.Count == 0) return;
            string pattern = enable ? FastLandingReplace : FastLandingAob;
            foreach (var addr in _fastLandingAddresses)
            {
                try { _fastLandingMem.AobReplace(addr, pattern); } catch { }
            }
            _fastLandingActive = enable;
        }

        public static bool IsFastLandingActive => _fastLandingActive;

        private static async Task FastLandingKeyWatcher()
        {
            while (_fastLandingWatcherRunning)
            {
                await Task.Delay(15);
                if (FastLandingKey <= 0) continue;

                bool isPressed = (GetAsyncKeyState(FastLandingKey) & 0x8000) != 0;

                if (isPressed && !_fastLandingCooldown)
                {
                    _fastLandingCooldown = true;

                    if (_fastLandingAddresses.Count == 0)
                    {
                        string r = await ScanFastLandingAsync();
                        if (r != "Fast Landing Ready")
                        {
                            await Task.Delay(300);
                            _fastLandingCooldown = false;
                            continue;
                        }
                    }

                    _fastLandingActive = !_fastLandingActive;
                    ApplyFastLanding(_fastLandingActive);

                    if (_fastLandingActive)
                        SoundEffects.PlayEnableSound();
                    else
                        SoundEffects.PlayDisableSound();

                    OnFastLandingChanged?.Invoke(_fastLandingActive);

                    await Task.Delay(300);
                    _fastLandingCooldown = false;
                }
            }
        }

        #endregion

        #region Camera View

        private static readonly string CameraViewAob     = "00 00 00 00 00 00 80 40 00 00 00 00 00 00 00 00 00 00 80 BF 00 00 00 00 00 00 80 BF 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 00 00 00 00 80 BF";
        private static readonly string CameraViewReplace = "00 00 00 00 00 00 80 3F 00 00 00 00 00 00 00 00 00 00 80 BF 00 00 00 00 00 00 80 BF 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 00 00 00 00 00 00";
        private static List<long> _cameraViewAddresses = new List<long>();
        private static readonly mozih4x _cameraViewMem = new mozih4x();
        private static bool _cameraViewWatcherRunning = false;
        private static bool _cameraViewActive = false;
        private static bool _cameraViewCooldown = false;

        public static async Task<string> ScanCameraViewAsync()
        {
            string emulator = GetProcess.GetRunningEmulatorProcessName();
            if (!_cameraViewMem.SetProcess(new string[] { emulator ?? "HD-Player" }))
                return "Emulator Not Found";

            var matches = await _cameraViewMem.AoBScan(CameraViewAob);
            if (matches == null || !matches.Any())
            {
                var replacedMatches = await _cameraViewMem.AoBScan(CameraViewReplace);
                if (replacedMatches == null || !replacedMatches.Any())
                    return "No Pattern Found";
                _cameraViewAddresses = replacedMatches.ToList();
            }
            else
            {
                _cameraViewAddresses = matches.ToList();
            }

            return "Camera View Ready";
        }

        public static async Task<string> EnableCameraView()
        {
            if (_cameraViewAddresses.Count == 0)
            {
                string r = await ScanCameraViewAsync();
                if (r != "Camera View Ready") return r;
            }

            ApplyCameraView(true);

            if (!_cameraViewWatcherRunning)
            {
                _cameraViewWatcherRunning = true;
                _ = CameraViewKeyWatcher();
            }

            return "Camera View ON";
        }

        public static void DisableCameraView()
        {
            _cameraViewWatcherRunning = false;
            ApplyCameraView(false);
            _cameraViewAddresses.Clear();
        }

        public static void ApplyCameraView(bool enable)
        {
            if (_cameraViewAddresses == null || _cameraViewAddresses.Count == 0) return;
            string pattern = enable ? CameraViewReplace : CameraViewAob;
            foreach (var addr in _cameraViewAddresses)
            {
                try { _cameraViewMem.AobReplace(addr, pattern); } catch { }
            }
            _cameraViewActive = enable;
        }

        public static bool IsCameraViewActive => _cameraViewActive;

        private static async Task CameraViewKeyWatcher()
        {
            while (_cameraViewWatcherRunning)
            {
                await Task.Delay(15);
                // VK_CAPITAL = 0x14 (Caps Lock)
                bool isPressed = (GetAsyncKeyState(0x14) & 0x8000) != 0;

                if (isPressed && !_cameraViewCooldown)
                {
                    _cameraViewCooldown = true;
                    _cameraViewActive = !_cameraViewActive;
                    ApplyCameraView(_cameraViewActive);
                    await Task.Delay(250);
                    _cameraViewCooldown = false;
                }
            }
        }

        #endregion

        #region Guest Reset

        private static readonly string GuestResetAob     = "10 40 2D E9 D0 40 9F E5 04 40 8F E0 00 00 D4 E5 00 00 50 E3 04 00 00 1A C0 00 9F E5 00 00 9F E7 F1";
        private static readonly string GuestResetReplace = "01 00 A0 E3 1E FF 2F E1 04 40 8F E0 00 00 D4 E5 00 00 50 E3 04 00 00 1A C0 00 9F E5 00 00 9F E7 F1";
        private static List<long> _guestResetAddresses = new List<long>();
        private static readonly mozih4x _guestResetMem = new mozih4x();

        public static async Task<string> ScanResetGuestAsync()
        {
            string emulator = GetProcess.GetRunningEmulatorProcessName();
            if (!_guestResetMem.SetProcess(new string[] { emulator ?? "HD-Player" }))
                return "Emulator Not Found";

            var matches = await _guestResetMem.AoBScan(GuestResetAob);
            if (matches == null || !matches.Any())
            {
                var replacedMatches = await _guestResetMem.AoBScan(GuestResetReplace);
                if (replacedMatches == null || !replacedMatches.Any())
                    return "No Pattern Found";
                _guestResetAddresses = replacedMatches.ToList();
            }
            else
            {
                _guestResetAddresses = matches.ToList();
            }

            return "Guest Reset Ready";
        }

        public static async Task<string> EnableResetGuest()
        {
            if (_guestResetAddresses.Count == 0)
            {
                string r = await ScanResetGuestAsync();
                if (r != "Guest Reset Ready") return r;
            }

            await Task.Run(() =>
            {
                foreach (var addr in _guestResetAddresses)
                {
                    try { _guestResetMem.AobReplace(addr, GuestResetReplace); } catch { }
                }
            });

            SoundEffects.PlayEnableSound();
            return "Guest Reset ON";
        }

        public static void DisableResetGuest()
        {
            if (_guestResetAddresses != null && _guestResetAddresses.Count > 0)
            {
                foreach (var addr in _guestResetAddresses)
                {
                    try { _guestResetMem.AobReplace(addr, GuestResetAob); } catch { }
                }
            }
            _guestResetAddresses.Clear();
        }

        #endregion

        #region Scope Tracking

        private static readonly string ScopeTrackingAob     = "CD CC CC 3D 02 00 00 00 EC 51 B8 3D CD CC 4C 3F 00 00 00 00 00 00 A0 42 00 00 C0 3F 33 33 13 40 00 00 F0 3F 00 00 80 3F 01 00 00 00";
        private static readonly string ScopeTrackingReplace = "CD CC CC 3D 02 00 00 00 EC 51 B8 3D CD CC 4C 3F 00 00 00 00 00 00 A0 42 00 00 C0 3F 33 33 13 40 00 00 F0 3F 00 00 80 4F 01 00 00 00";
        private static List<long> _scopeTrackingAddresses = new List<long>();
        private static readonly mozih4x _scopeTrackingMem = new mozih4x();

        public static async Task<string> ScanScopeTrackingAsync()
        {
            string emulator = GetProcess.GetRunningEmulatorProcessName();
            if (!_scopeTrackingMem.SetProcess(new string[] { emulator ?? "HD-Player" }))
                return "Emulator Not Found";

            var matches = await _scopeTrackingMem.AoBScan(ScopeTrackingAob);
            if (matches == null || !matches.Any())
            {
                var replacedMatches = await _scopeTrackingMem.AoBScan(ScopeTrackingReplace);
                if (replacedMatches == null || !replacedMatches.Any())
                    return "No Pattern Found";
                _scopeTrackingAddresses = replacedMatches.ToList();
            }
            else
            {
                _scopeTrackingAddresses = matches.ToList();
            }

            return "Scope Tracking Ready";
        }

        public static async Task<string> EnableScopeTracking()
        {
            if (_scopeTrackingAddresses.Count == 0)
            {
                string r = await ScanScopeTrackingAsync();
                if (r != "Scope Tracking Ready") return r;
            }

            await Task.Run(() =>
            {
                foreach (var addr in _scopeTrackingAddresses)
                {
                    try { _scopeTrackingMem.AobReplace(addr, ScopeTrackingReplace); } catch { }
                }
            });

            SoundEffects.PlayEnableSound();
            return "Scope Tracking ON";
        }

        public static void DisableScopeTracking()
        {
            if (_scopeTrackingAddresses != null && _scopeTrackingAddresses.Count > 0)
            {
                foreach (var addr in _scopeTrackingAddresses)
                {
                    try { _scopeTrackingMem.AobReplace(addr, ScopeTrackingAob); } catch { }
                }
            }
            _scopeTrackingAddresses.Clear();
        }

        #endregion

        #region 120 FPS Unlock

        private static readonly string Fps120Aob     = "30 48 2D E9 A4 40 9F E5 04 40 8F E0 00 00 D4 E5 00 00 50 E3 04 00 00 1A 94 00 9F E5 00 00 9F E7 04 DB 77 EB 01 00 A0 E3 00 00 C4 E5 54 04 03 E3 00 10 A0 E3 68 85 FA EB 00 00 50 E3 0A 00 00 0A 54 04 03 E3 00 10 A0 E3 97 85 FA EB 00 40 A0 E1";
        private static readonly string Fps120Replace = "01 00 A0 E3 1E FF 2F E1 04 40 8F E0 00 00 D4 E5 00 00 50 E3 04 00 00 1A 94 00 9F E5 00 00 9F E7 04 DB 77 EB 01 00 A0 E3 00 00 C4 E5 54 04 03 E3 00 10 A0 E3 68 85 FA EB 00 00 50 E3 0A 00 00 0A 54 04 03 E3 00 10 A0 E3 97 85 FA EB 00 40 A0 E1";
        private static List<long> _fps120Addresses = new List<long>();
        private static readonly mozih4x _fps120Mem = new mozih4x();

        public static async Task<string> Scan120FpsAsync()
        {
            string emulator = GetProcess.GetRunningEmulatorProcessName();
            if (!_fps120Mem.SetProcess(new string[] { emulator ?? "HD-Player" }))
                return "Emulator Not Found";

            var matches = await _fps120Mem.AoBScan(Fps120Aob);
            if (matches == null || !matches.Any())
            {
                var replacedMatches = await _fps120Mem.AoBScan(Fps120Replace);
                if (replacedMatches == null || !replacedMatches.Any())
                    return "No Pattern Found";
                _fps120Addresses = replacedMatches.ToList();
            }
            else
            {
                _fps120Addresses = matches.ToList();
            }

            return "120 FPS Ready";
        }

        public static async Task<string> Enable120Fps()
        {
            if (_fps120Addresses.Count == 0)
            {
                string r = await Scan120FpsAsync();
                if (r != "120 FPS Ready") return r;
            }

            await Task.Run(() =>
            {
                foreach (var addr in _fps120Addresses)
                {
                    try { _fps120Mem.AobReplace(addr, Fps120Replace); } catch { }
                }
            });

            SoundEffects.PlayEnableSound();
            return "120 FPS ON";
        }

        public static void Disable120Fps()
        {
            if (_fps120Addresses != null && _fps120Addresses.Count > 0)
            {
                foreach (var addr in _fps120Addresses)
                {
                    try { _fps120Mem.AobReplace(addr, Fps120Aob); } catch { }
                }
            }
            _fps120Addresses.Clear();
        }

        #endregion



        #region Dynamic Bone Selection

        // SAFE — controls which bone offset the aimbot targets, no suspicious stat patterns
        // Bone offsets from Bones.cs (v1.132.6):
        //   Head = 0x49C, Neck = 0x4A4, Chest/Spine = 0x4A4, Hip = 0x4A0, Feet = 0x4C0
        public enum TargetBone { Head = 0, Neck = 1, Chest = 2, Hip = 3, Feet = 4 }

        private static TargetBone _selectedBone = TargetBone.Head;

        public static void SetTargetBone(TargetBone bone)
        {
            _selectedBone = bone;
        }

        public static TargetBone GetTargetBone() => _selectedBone;

        // Returns the bone offset value for use in aimbot write targeting
        public static long GetActiveBoneOffset()
        {
            switch (_selectedBone)
            {
                case TargetBone.Head:  return 0x49C;
                case TargetBone.Neck:  return 0x4A4;
                case TargetBone.Chest: return 0x4A4;
                case TargetBone.Hip:   return 0x4A0;
                case TargetBone.Feet:  return 0x4C0;
                default:               return 0x49C;
            }
        }

        #endregion
    }
}
