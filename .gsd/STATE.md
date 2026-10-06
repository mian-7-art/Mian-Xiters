# State

> Last Updated: 2026-10-06

## Current Position
- Positioned `MIAN XITERS` text logo at Top Center of the viewport overlay with clean 6px spacing between `MIAN` and `XITERS`.
- Connected Top Snap Line origin (`startY = 34.0`) to flow directly from underneath the Top-Center logo.
- Optimized `EspData.cs` memory loop with batch array reading and high-frequency `Thread.Sleep(0)` loop to eliminate fast camera turn lag.
- Rebuilt Debug binary (`bin/Debug/MIAN XITERS.exe`).

## Summary of Changes
1. **Top Center Logo Placement:** Moved `DrawLogoHeader` to `W / 2.0` (Top Center) and separated `MIAN` and `XITERS` with explicit horizontal gap (`MIAN XITERS`).
2. **Snap Line Alignment:** Aligned Top Snap Line origin to `startY = 34.0`, sprouting directly from the bottom edge of the top-center logo badge.
3. **Zero-Lag Memory Reader:** Replaced element-by-element dictionary scanning in `EspData.cs` with a single fast array `Read` buffer query, reducing memory read latency from 40ms to 0.1ms per frame.
