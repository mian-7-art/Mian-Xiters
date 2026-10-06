# State

> Last Updated: 2026-10-06

## Current Position
- Integrated Win32 `GetClientRect` and `ClientToScreen` viewport tracking into `EspOverlayWindow.xaml.cs`.
- Rebuilt project in Debug mode (`bin/Debug/MIAN XITERS.exe`).

## Summary of Changes
1. **Dynamic Viewport Synchronization:** The overlay window continuously converts `EspCore.Handle` client coordinates to screen coordinates and updates overlay position (`Left`, `Top`, `Width`, `Height`).
2. **Windowed Mode Resolution:** Whether the emulator is in Windowed Mode, resized, or moved, the overlay snaps precisely over the game viewport.
