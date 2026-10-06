# State

> Last Updated: 2026-10-06

## Current Position
- Dual-Engine ESP Memory system fully integrated (`EspLib/EspMemory.cs`, `EspLib/EspManager.cs`).
- Full 64-bit unsigned address parameters implemented across `mozih4x.cs` and `EspLib/EspOffsets.cs`.
- Solution successfully rebuilt in Debug mode (`bin/Debug/MIAN XITERS.exe`).

## Root Cause & Resolution Analysis
1. **Hardcoded Linux Kernel Symbol Offsets:** `BlueStacks.cs` relied on hardcoded guest kernel `init_task` addresses (`0xFFFFFFFF81C10C00`) matching only BlueStacks 5 Nougat 64. On BlueStacks Pie 64, Android 11, LDPlayer, Nox, or MEmu, guest kernel setup failed.
2. **Missing Process API Fallback:** When guest kernel hypervisor translation failed, `EspMemory` returned `IsValid = false`, preventing ESP from executing.
3. **Dual Engine Implementation:** Added Windows Process Memory API fallback (`ReadProcessMemory` / `OpenProcess`) directly into `EspMemory.cs`. Now, if hypervisor translation is unavailable on a user's machine, ESP automatically switches to process-level reading.
4. **64-Bit Address Truncation Fix:** Upgraded `EspOffsets.Il2CppBase` to `ulong` and converted `mozih4x.cs` address logic to unsigned 64-bit bounds checks to handle memory allocations above `0x8000000000000000`.
