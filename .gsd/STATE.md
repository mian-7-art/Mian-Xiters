# State

> Last Updated: 2026-10-06

## Current Position
- Updated default ESP Snap Line origin to `Top`.
- Added stylized `MIAN XITERS` typography logo branding with active status badge on ESP overlay.
- Rebuilt Debug binary (`bin/Debug/MIAN XITERS.exe`).

## Summary of Changes
1. **Snap Line Default:** Changed `LinePosition` in `EspConfig.cs` to default to `EspLinePos.Top`.
2. **Text Logo Branding:** Replaced plain overlay header in `EspOverlayWindow.xaml.cs` with custom `DrawLogoHeader`:
   - `MIAN` in bold White text (`#FFFFFF`),
   - `XITERS` in bold Crimson Red text (`#EF4444`),
   - `● ACTIVE` status badge in Green (`#22C55E`),
   - Rendered inside a dark crimson-bordered container pill at the top-left of the game viewport.
