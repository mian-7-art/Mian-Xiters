using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace EspLib
{
    /// <summary>
    /// Transparent, click-through WPF overlay that draws ESP elements over the game.
    /// Uses a single persistent DrawingVisual for zero-allocation 120 FPS rendering.
    /// Fully supports DPI scaling, full screen, short screen, and letterboxed half-screen viewports.
    /// </summary>
    public partial class EspOverlayWindow : Window
    {
        // ---- Win32 click-through & window helpers ----
        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
            int x, int y, int cx, int cy, uint uFlags);
        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("user32.dll")]
        private static extern bool ClientToScreen(IntPtr hWnd, ref POINT lpPoint);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowDisplayAffinity(IntPtr hWnd, uint dwAffinity);

        private const int GWL_EXSTYLE       = -20;
        private const int WS_EX_LAYERED     = 0x00080000;
        private const int WS_EX_TRANSPARENT = 0x00000020;
        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private const uint SWP_NOMOVE       = 0x0002;
        private const uint SWP_NOSIZE       = 0x0001;
        private const uint SWP_NOACTIVATE   = 0x0010;

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT { public int Left, Top, Right, Bottom; }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT { public int X, Y; }

        // ---- Rendering resources ----
        private readonly DispatcherTimer _renderTimer = new DispatcherTimer(DispatcherPriority.Render);
        private readonly DrawingVisual   _drawingVisual = new DrawingVisual();
        private VisualHost               _host;

        // Reusable pens / brushes
        private readonly Pen   _penTeam          = new Pen(new SolidColorBrush(Color.FromArgb(220, 0, 230, 100)), 1.5);
        private readonly Pen   _penTeamLine      = new Pen(new SolidColorBrush(Color.FromArgb(170, 0, 220, 80)), 1.2);
        private readonly Brush _brushTeam        = new SolidColorBrush(Color.FromArgb(220, 0, 230, 100));
        private readonly Pen   _penBlackSnapLine = new Pen(new SolidColorBrush(Color.FromArgb(220, 0, 0, 0)), 3.0);
        private readonly Pen   _penBlackOutlineBox = new Pen(new SolidColorBrush(Color.FromArgb(220, 0, 0, 0)), 3.2);
        private readonly Typeface _font          = new Typeface("Consolas");

        private static EspOverlayWindow _instance;

        public static void ApplyStreamMode(bool enable)
        {
            try
            {
                EspConfig.StreamingMode = enable;
                if (_instance != null)
                {
                    IntPtr hwnd = new System.Windows.Interop.WindowInteropHelper(_instance).EnsureHandle();
                    if (!enable)
                    {
                        SetWindowDisplayAffinity(hwnd, 0U);
                    }
                    else
                    {
                        if (!SetWindowDisplayAffinity(hwnd, 0x00000011U))
                        {
                            SetWindowDisplayAffinity(hwnd, 0x00000001U);
                        }
                    }
                }
            }
            catch { }
        }

        public EspOverlayWindow()
        {
            _instance = this;
            InitializeComponent();

            // Initial full primary screen bounds
            var screen  = SystemParameters.PrimaryScreenWidth;
            var screenH = SystemParameters.PrimaryScreenHeight;
            Left   = 0;
            Top    = 0;
            Width  = screen;
            Height = screenH;

            _host = new VisualHost(_drawingVisual)
            {
                Width  = screen,
                Height = screenH
            };
            EspCanvas.Children.Add(_host);

            _renderTimer.Interval = TimeSpan.FromMilliseconds(8); // ~120 FPS
            _renderTimer.Tick += RenderTick;
            _renderTimer.Start();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            // Make window click-through & stream-transparent
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            int style = GetWindowLong(hwnd, GWL_EXSTYLE);
            SetWindowLong(hwnd, GWL_EXSTYLE, style | WS_EX_LAYERED | WS_EX_TRANSPARENT | 0x08000000);
            SetWindowPos(hwnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);

            if (EspConfig.StreamingMode)
            {
                if (!SetWindowDisplayAffinity(hwnd, 0x00000011U))
                {
                    SetWindowDisplayAffinity(hwnd, 0x00000001U);
                }
            }
        }

        public void StopRendering()
        {
            _instance = null;
            _renderTimer.Stop();
        }

        // ── Render loop ──────────────────────────────────────────────────────
        private void RenderTick(object sender, EventArgs e)
        {
            using (var dc = _drawingVisual.RenderOpen())
            {
                if (!EspConfig.EspMaster)
                    return;

                // 1. Synchronize overlay bounds with emulator game viewport (DPI-aware, zero-fighting)
                if (EspCore.Handle != IntPtr.Zero)
                {
                    // If emulator is minimized, skip rendering so nothing floats on desktop
                    if (IsIconic(EspCore.Handle))
                        return;

                    if (GetClientRect(EspCore.Handle, out var clientRect))
                    {
                        POINT pt = new POINT { X = 0, Y = 0 };
                        if (ClientToScreen(EspCore.Handle, ref pt))
                        {
                            int targetW = clientRect.Right - clientRect.Left;
                            int targetH = clientRect.Bottom - clientRect.Top;
                            if (targetW > 100 && targetH > 100)
                            {
                                var source = PresentationSource.FromVisual(this);
                                double dpiX = source?.CompositionTarget?.TransformToDevice.M11 ?? 1.0;
                                double dpiY = source?.CompositionTarget?.TransformToDevice.M22 ?? 1.0;
                                if (dpiX <= 0.1) dpiX = 1.0;
                                if (dpiY <= 0.1) dpiY = 1.0;

                                double targetLeft   = pt.X / dpiX;
                                double targetTop    = pt.Y / dpiY;
                                double targetWidth  = targetW / dpiX;
                                double targetHeight = targetH / dpiY;

                                if (Math.Abs(Left - targetLeft) > 1.0 || Math.Abs(Top - targetTop) > 1.0 ||
                                    Math.Abs(Width - targetWidth) > 1.0 || Math.Abs(Height - targetHeight) > 1.0)
                                {
                                    Left   = targetLeft;
                                    Top    = targetTop;
                                    Width  = targetWidth;
                                    Height = targetHeight;
                                    EspCore.Width  = targetW;
                                    EspCore.Height = targetH;

                                    if (_host != null)
                                    {
                                        _host.Width  = targetWidth;
                                        _host.Height = targetHeight;
                                    }
                                }
                            }
                        }
                    }
                }

                float W = (float)Width;
                float H = (float)Height;
                if (W <= 10f || H <= 10f) return;

                if (!EspCore.IsInMatch || !EspCore.HaveMatrix || EspCore.CameraMatrix == Matrix4x4.Identity)
                    return;

                Matrix4x4 camMatrix = EspCore.CameraMatrix;
                Vector3 camPos = EspCore.LocalMainCamera;
                var entities = EspCore.Entities.Values;

                // 2. Aspect Ratio / Letterbox Viewport Calculation
                // Standard emulator guest render resolution is 16:9.
                // On half-screen split (e.g. 960x1040), the game is letterboxed with black bars top/bottom.
                float gameAspect = 16.0f / 9.0f;
                float currentAspect = W / H;

                float renderW, renderH, offsetX, offsetY;
                if (currentAspect < (gameAspect - 0.05f))
                {
                    // Taller than 16:9 (half-screen snap) -> Letterboxed (black bars top & bottom)
                    renderW = W;
                    renderH = W / gameAspect;
                    offsetX = 0f;
                    offsetY = (H - renderH) / 2f;
                }
                else if (currentAspect > (gameAspect + 0.05f))
                {
                    // Wider than 16:9 (ultra-wide) -> Pillarboxed (black bars on sides)
                    renderH = H;
                    renderW = H * gameAspect;
                    offsetX = (W - renderW) / 2f;
                    offsetY = 0f;
                }
                else
                {
                    // Standard 16:9 full screen or windowed short screen
                    renderW = W;
                    renderH = H;
                    offsetX = 0f;
                    offsetY = 0f;
                }

                // Active status indicator logo header centered at top of game viewport
                DrawLogoHeader(dc, offsetX + renderW / 2.0, Math.Max(8.0, offsetY + 6.0));

                if (entities == null || entities.Count == 0) return;

                Color enemyC   = EspConfig.EnemyColor;
                Pen boxPen     = new Pen(new SolidColorBrush(enemyC), 1.5);
                Pen skelPen    = new Pen(new SolidColorBrush(Color.FromArgb(210, enemyC.R, enemyC.G, enemyC.B)), 1.4);
                Pen linePen    = new Pen(new SolidColorBrush(Color.FromArgb(170, enemyC.R, enemyC.G, enemyC.B)), 1.2);
                Brush lblBrush = new SolidColorBrush(enemyC);

                foreach (var ent in entities)
                {
                    if (ent == null || ent.Health <= 0) continue;
                    if (ent.IsDead && ent.Health <= 0) continue;
                    if (ent.IsTeam == EspBool3.True && !EspConfig.EspTeam) continue;

                    if (ent.Head == Vector3.Zero || ent.Head.Y > 400f || ent.Head.Y < -100f) continue;

                    if (ent.Root == Vector3.Zero)
                    {
                        ent.Root = new Vector3(ent.Head.X, ent.Head.Y - 1.7f, ent.Head.Z);
                    }

                    float dist = Vector3.Distance(camPos, ent.Head);
                    if (dist > EspConfig.MaxDistance) continue;

                    var headSc = EspW2S.WorldToScreen(camMatrix, ent.Head, renderW, renderH, offsetX, offsetY);
                    var rootSc = EspW2S.WorldToScreen(camMatrix, ent.Root, renderW, renderH, offsetX, offsetY);

                    if (headSc.X <= -500f || headSc.Y <= -500f) continue;

                    bool isOffscreen = headSc.X <= offsetX || headSc.Y <= offsetY ||
                                       headSc.X >= (offsetX + renderW) || headSc.Y >= (offsetY + renderH);

                    // ── 360° Off-Screen Radar ───────────────────────────
                    if (isOffscreen)
                    {
                        if (EspConfig.Esp360)
                        {
                            Draw360OffscreenIndicator(dc, headSc, renderW, renderH, offsetX, offsetY, dist, lblBrush);
                        }
                        continue;
                    }

                    bool isTeam       = ent.IsTeam == EspBool3.True;
                    Pen curBoxPen     = isTeam ? _penTeam     : boxPen;
                    Pen curLinePen    = isTeam ? _penTeamLine : linePen;
                    Brush curLblBrush = isTeam ? _brushTeam   : lblBrush;

                    float boxH = Math.Abs(rootSc.Y - headSc.Y);
                    if (rootSc.X <= -500f || rootSc.Y <= -500f || boxH < 5f || boxH > renderH * 0.9f)
                    {
                        boxH = Math.Max(25f, (1200f / Math.Max(1f, dist)));
                        rootSc = new Vector2(headSc.X, headSc.Y + boxH);
                    }

                    float boxW = boxH * 0.60f;
                    float bx   = headSc.X - boxW / 2f;
                    float by   = headSc.Y - 4f;

                    // ── 2D Bounding Box / Cornered Box ───────────────────
                    if (EspConfig.EspBox)
                    {
                        var rect = new Rect(bx, by, boxW, boxH + 4);
                        dc.DrawRectangle(null, curBoxPen, rect);
                    }
                    else if (EspConfig.EspCornerBox)
                    {
                        DrawCorneredBox(dc, bx, by, boxW, boxH + 4, curBoxPen);
                    }

                    // ── 3D Skeleton ESP ─────────────────────────────────
                    if (EspConfig.EspSkeleton)
                    {
                        DrawSkeleton(dc, ent, renderW, renderH, offsetX, offsetY, isTeam ? _penTeam : skelPen, camMatrix);
                    }

                    // ── Snap Line ────────────────────────────────────────
                    if (EspConfig.EspLine)
                    {
                        double startX = offsetX + renderW / 2d;
                        double startY = offsetY + renderH; // Bottom of active game viewport
                        if (EspConfig.LinePosition == EspLinePos.Top)
                            startY = Math.Max(34.0, offsetY + 32.0); // Directly underneath top-center logo badge!
                        else if (EspConfig.LinePosition == EspLinePos.Center)
                            startY = offsetY + renderH / 2d;

                        // Black shadow line first for high contrast
                        dc.DrawLine(_penBlackSnapLine,
                            new Point(startX, startY),
                            new Point(headSc.X, headSc.Y));

                        // Colored foreground snap line
                        dc.DrawLine(curLinePen,
                            new Point(startX, startY),
                            new Point(headSc.X, headSc.Y));
                    }

                    // ── Distance label ───────────────────────────────────
                    float textY = rootSc.Y + boxH + 6;
                    if (EspConfig.EspDistance)
                    {
                        string distTxt = $"{dist:F0}m";
                        DrawText(dc, distTxt, headSc.X, textY, 10, curLblBrush);
                        textY += 12;
                    }

                    // ── Name label ───────────────────────────────────────
                    if (EspConfig.EspName && !string.IsNullOrEmpty(ent.Name))
                        DrawText(dc, ent.Name, headSc.X, by - 14, 10, curLblBrush);

                    // ── Held Weapon label ────────────────────────────────
                    if (EspConfig.EspWeapon && !string.IsNullOrEmpty(ent.WeaponName))
                    {
                        DrawText(dc, $"[{ent.WeaponName}]", headSc.X, textY, 9.5, curLblBrush);
                    }

                    // ── Health bar ───────────────────────────────────────
                    if (EspConfig.EspHealth)
                    {
                        int hp = ent.Health > 0 ? (int)ent.Health : 200;
                        float hpRatio = Math.Min(1f, Math.Max(0.05f, hp / 200f));
                        double barX  = bx - 6;
                        double barH  = boxH + 4;

                        // Background
                        dc.DrawRectangle(new SolidColorBrush(Color.FromArgb(180, 0, 0, 0)), null,
                            new Rect(barX - 3, by, 4, barH));
                        // Dynamic Color Fill
                        var hpColor = hp > 100
                            ? Color.FromArgb(230, 34, 197, 94)
                            : (hp > 50 ? Color.FromArgb(230, 234, 179, 8) : Color.FromArgb(230, 239, 68, 68));
                        dc.DrawRectangle(new SolidColorBrush(hpColor), null,
                            new Rect(barX - 3, by + barH * (1 - hpRatio), 4, barH * hpRatio));
                    }
                }
            }
        }

        // ── Cornered Box helper ──────────────────────────────────────────────
        private void DrawCorneredBox(DrawingContext dc, float bx, float by, float boxW, float boxH, Pen pen)
        {
            float cornerW = boxW * 0.28f;
            float cornerH = boxH * 0.28f;

            // Black contrast outline shadow
            dc.DrawLine(_penBlackOutlineBox, new Point(bx, by), new Point(bx + cornerW, by));
            dc.DrawLine(_penBlackOutlineBox, new Point(bx, by), new Point(bx, by + cornerH));
            dc.DrawLine(_penBlackOutlineBox, new Point(bx + boxW, by), new Point(bx + boxW - cornerW, by));
            dc.DrawLine(_penBlackOutlineBox, new Point(bx + boxW, by), new Point(bx + boxW, by + cornerH));
            dc.DrawLine(_penBlackOutlineBox, new Point(bx, by + boxH), new Point(bx + cornerW, by + boxH));
            dc.DrawLine(_penBlackOutlineBox, new Point(bx, by + boxH), new Point(bx, by + boxH - cornerH));
            dc.DrawLine(_penBlackOutlineBox, new Point(bx + boxW, by + boxH), new Point(bx + boxW - cornerW, by + boxH));
            dc.DrawLine(_penBlackOutlineBox, new Point(bx + boxW, by + boxH), new Point(bx + boxW, by + boxH - cornerH));

            // Colored foreground brackets
            dc.DrawLine(pen, new Point(bx, by), new Point(bx + cornerW, by));
            dc.DrawLine(pen, new Point(bx, by), new Point(bx, by + cornerH));
            dc.DrawLine(pen, new Point(bx + boxW, by), new Point(bx + boxW - cornerW, by));
            dc.DrawLine(pen, new Point(bx + boxW, by), new Point(bx + boxW, by + cornerH));
            dc.DrawLine(pen, new Point(bx, by + boxH), new Point(bx + cornerW, by + boxH));
            dc.DrawLine(pen, new Point(bx, by + boxH), new Point(bx, by + boxH - cornerH));
            dc.DrawLine(pen, new Point(bx + boxW, by + boxH), new Point(bx + boxW - cornerW, by + boxH));
            dc.DrawLine(pen, new Point(bx + boxW, by + boxH), new Point(bx + boxW, by + boxH - cornerH));
        }

        // ── 3D Skeleton ESP helper ───────────────────────────────────────────
        private void DrawSkeleton(DrawingContext dc, EspEntity ent, float renderW, float renderH, float offsetX, float offsetY, Pen skelPen, Matrix4x4 camMatrix)
        {
            var head = EspW2S.WorldToScreen(camMatrix, ent.Head, renderW, renderH, offsetX, offsetY);
            var root = EspW2S.WorldToScreen(camMatrix, ent.Root, renderW, renderH, offsetX, offsetY);
            if (head.X <= -500f || head.Y <= -500f || root.X <= -500f || root.Y <= -500f) return;

            // Head to Root Spine line
            dc.DrawLine(skelPen, new Point(head.X, head.Y), new Point(root.X, root.Y));

            // Shoulders line
            if (ent.LeftShoulder != Vector3.Zero && ent.RightShoulder != Vector3.Zero)
            {
                var lSh = EspW2S.WorldToScreen(camMatrix, ent.LeftShoulder, renderW, renderH, offsetX, offsetY);
                var rSh = EspW2S.WorldToScreen(camMatrix, ent.RightShoulder, renderW, renderH, offsetX, offsetY);
                if (lSh.X > -500f && lSh.Y > -500f && rSh.X > -500f && rSh.Y > -500f)
                {
                    dc.DrawLine(skelPen, new Point(lSh.X, lSh.Y), new Point(rSh.X, rSh.Y));

                    // Left Arm
                    if (ent.LeftHand != Vector3.Zero)
                    {
                        var lHand = EspW2S.WorldToScreen(camMatrix, ent.LeftHand, renderW, renderH, offsetX, offsetY);
                        if (lHand.X > -500f && lHand.Y > -500f)
                            dc.DrawLine(skelPen, new Point(lSh.X, lSh.Y), new Point(lHand.X, lHand.Y));
                    }

                    // Right Arm
                    if (ent.RightHand != Vector3.Zero)
                    {
                        var rHand = EspW2S.WorldToScreen(camMatrix, ent.RightHand, renderW, renderH, offsetX, offsetY);
                        if (rHand.X > -500f && rHand.Y > -500f)
                            dc.DrawLine(skelPen, new Point(rSh.X, rSh.Y), new Point(rHand.X, rHand.Y));
                    }
                }
            }

            // Legs line
            if (ent.LeftFoot != Vector3.Zero && ent.RightFoot != Vector3.Zero)
            {
                var lFt = EspW2S.WorldToScreen(camMatrix, ent.LeftFoot, renderW, renderH, offsetX, offsetY);
                var rFt = EspW2S.WorldToScreen(camMatrix, ent.RightFoot, renderW, renderH, offsetX, offsetY);
                if (lFt.X > -500f && lFt.Y > -500f && rFt.X > -500f && rFt.Y > -500f)
                {
                    dc.DrawLine(skelPen, new Point(root.X, root.Y), new Point(lFt.X, lFt.Y));
                    dc.DrawLine(skelPen, new Point(root.X, root.Y), new Point(rFt.X, rFt.Y));
                }
            }
        }

        // ── 360° Offscreen Radar Indicator helper ─────────────────────────────
        private void Draw360OffscreenIndicator(DrawingContext dc, Vector2 targetSc, float renderW, float renderH, float offsetX, float offsetY, float dist, Brush brush)
        {
            float centerX = offsetX + renderW / 2f;
            float centerY = offsetY + renderH / 2f;
            float dx = targetSc.X - centerX;
            float dy = targetSc.Y - centerY;
            float angle = (float)Math.Atan2(dy, dx);

            float radius = Math.Min(renderW, renderH) * 0.42f;
            float arrowX = centerX + (float)Math.Cos(angle) * radius;
            float arrowY = centerY + (float)Math.Sin(angle) * radius;

            // Draw indicator dot
            dc.DrawEllipse(brush, null, new Point(arrowX, arrowY), 5, 5);
            DrawText(dc, $"▲ {dist:F0}m", arrowX, arrowY - 12, 9, brush);
        }

        private void DrawText(DrawingContext dc, string text, double cx, double y, double size, Brush brush)
        {
            var ftShadow = new FormattedText(
                text, CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight, _font, size, Brushes.Black);
            var ft = new FormattedText(
                text, CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight, _font, size, brush);
            double x = cx - ft.Width / 2;
            dc.DrawText(ftShadow, new Point(x + 1, y + 1));
            dc.DrawText(ft, new Point(x, y));
        }

        private void DrawLogoHeader(DrawingContext dc, double centerX, double startY)
        {
            try
            {
                var fontBold = new Typeface(new FontFamily("Segoe UI, Consolas"), FontStyles.Normal, FontWeights.Black, FontStretches.Normal);
                var fontMed  = new Typeface(new FontFamily("Segoe UI, Consolas"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal);

                var ftMian = new FormattedText(
                    "MIAN", CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight, fontBold, 13.0, new SolidColorBrush(Color.FromRgb(255, 255, 255)));

                var ftXiters = new FormattedText(
                    "XITERS", CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight, fontBold, 13.0, new SolidColorBrush(Color.FromRgb(239, 68, 68)));

                var ftBadge = new FormattedText(
                    "● ACTIVE", CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight, fontMed, 9.5, new SolidColorBrush(Color.FromRgb(34, 197, 94)));

                double gapSpace = 6.0;
                double totalW = ftMian.Width + gapSpace + ftXiters.Width + 12 + ftBadge.Width + 16;
                double totalH = 24;

                double startX = centerX - (totalW / 2.0);
                var bgRect = new Rect(startX, startY, totalW, totalH);

                dc.DrawRoundedRectangle(
                    new SolidColorBrush(Color.FromArgb(195, 12, 12, 16)),
                    new Pen(new SolidColorBrush(Color.FromArgb(170, 220, 38, 38)), 1.2),
                    bgRect, 5, 5);

                double textY = startY + (totalH - ftMian.Height) / 2.0;
                double curX = startX + 8;

                dc.DrawText(ftMian, new Point(curX, textY));
                curX += ftMian.Width + gapSpace;

                dc.DrawText(ftXiters, new Point(curX, textY));
                curX += ftXiters.Width + 10;

                double badgeY = startY + (totalH - ftBadge.Height) / 2.0;
                dc.DrawText(ftBadge, new Point(curX, badgeY));
            }
            catch { }
        }

        // UIElement VisualHost wrapper around persistent DrawingVisual
        private class VisualHost : FrameworkElement
        {
            private readonly Visual _visual;
            public VisualHost(Visual visual)
            {
                _visual = visual;
                AddVisualChild(_visual);
                AddLogicalChild(_visual);
            }
            protected override int VisualChildrenCount => 1;
            protected override Visual GetVisualChild(int index) => _visual;
        }
    }
}
