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
    /// </summary>
    public partial class EspOverlayWindow : Window
    {
        // ---- Win32 click-through helpers ----
        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
            int x, int y, int cx, int cy, uint uFlags);

        private const int GWL_EXSTYLE   = -20;
        private const int WS_EX_LAYERED = 0x00080000;
        private const int WS_EX_TRANSPARENT = 0x00000020;
        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private const uint SWP_NOMOVE     = 0x0002;
        private const uint SWP_NOSIZE     = 0x0001;
        private const uint SWP_NOACTIVATE = 0x0010;

        // ---- Rendering resources ----
        private readonly DispatcherTimer _renderTimer = new DispatcherTimer(DispatcherPriority.Render);
        private readonly DrawingVisual   _drawingVisual = new DrawingVisual();

        // Reusable pens / brushes
        private readonly Pen   _penTeam     = new Pen(new SolidColorBrush(Color.FromArgb(220, 0, 230, 100)), 1.5);
        private readonly Pen   _penTeamLine = new Pen(new SolidColorBrush(Color.FromArgb(170, 0, 220, 80)), 1.2);
        private readonly Brush _brushTeam  = new SolidColorBrush(Color.FromArgb(220, 0, 230, 100));
        private readonly Pen   _penBlackSnapLine = new Pen(new SolidColorBrush(Color.FromArgb(220, 0, 0, 0)), 3.0);
        private readonly Pen   _penBlackOutlineBox = new Pen(new SolidColorBrush(Color.FromArgb(220, 0, 0, 0)), 3.2);
        private readonly Typeface _font     = new Typeface("Consolas");

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowDisplayAffinity(IntPtr hWnd, uint dwAffinity);

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

        [DllImport("user32.dll")]
        private static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("user32.dll")]
        private static extern bool ClientToScreen(IntPtr hWnd, ref POINT lpPoint);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT { public int Left, Top, Right, Bottom; }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT { public int X, Y; }

        public EspOverlayWindow()
        {
            _instance = this;
            InitializeComponent();

            // Full primary screen default
            var screen  = System.Windows.SystemParameters.PrimaryScreenWidth;
            var screenH = System.Windows.SystemParameters.PrimaryScreenHeight;
            Left   = 0; Top    = 0;
            Width  = screen; Height = screenH;

            var host = new VisualHost(_drawingVisual)
            {
                Width  = screen,
                Height = screenH
            };
            EspCanvas.Children.Add(host);

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

                // Synchronize overlay bounds natively with emulator game viewport (0ms lag)
                if (EspCore.Handle != IntPtr.Zero)
                {
                    if (GetClientRect(EspCore.Handle, out var clientRect))
                    {
                        POINT pt = new POINT { X = 0, Y = 0 };
                        if (ClientToScreen(EspCore.Handle, ref pt))
                        {
                            int targetW = clientRect.Right - clientRect.Left;
                            int targetH = clientRect.Bottom - clientRect.Top;
                            if (targetW > 100 && targetH > 100)
                            {
                                if (Math.Abs(Left - pt.X) > 1 || Math.Abs(Top - pt.Y) > 1 ||
                                    Math.Abs(Width - targetW) > 1 || Math.Abs(Height - targetH) > 1)
                                {
                                    Left = pt.X;
                                    Top = pt.Y;
                                    Width = targetW;
                                    Height = targetH;
                                    EspCore.Width = targetW;

                                    IntPtr hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
                                    if (hwnd != IntPtr.Zero)
                                    {
                                        SetWindowPos(hwnd, HWND_TOPMOST, pt.X, pt.Y, targetW, targetH, SWP_NOACTIVATE);
                                    }
                                }
                            }
                        }
                    }
                }

                int W = (int)Width;
                int H = (int)Height;

                if (!EspCore.IsInMatch || !EspCore.HaveMatrix || EspCore.CameraMatrix == Matrix4x4.Identity)
                    return;

                Matrix4x4 camMatrix = EspCore.CameraMatrix;
                Vector3 camPos = EspCore.LocalMainCamera;
                var entities = EspCore.Entities.Values;

                // Active status indicator logo header centered at top when in game match
                DrawLogoHeader(dc, W / 2.0, 8);

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

                    var headSc = EspW2S.WorldToScreen(camMatrix, ent.Head, W, H);
                    var rootSc = EspW2S.WorldToScreen(camMatrix, ent.Root, W, H);

                    if (headSc.X <= -500f || headSc.Y <= -500f) continue;

                    bool isOffscreen = headSc.X <= 0 || headSc.Y <= 0 || headSc.X >= W || headSc.Y >= H;

                    // ── 360° Off-Screen Radar ───────────────────────────
                    if (isOffscreen)
                    {
                        if (EspConfig.Esp360)
                        {
                            Draw360OffscreenIndicator(dc, headSc, W, H, dist, lblBrush);
                        }
                        continue;
                    }

                    bool isTeam  = ent.IsTeam == EspBool3.True;
                    Pen curBoxPen   = isTeam ? _penTeam  : boxPen;
                    Pen curLinePen  = isTeam ? _penTeamLine : linePen;
                    Brush curLblBrush = isTeam ? _brushTeam : lblBrush;

                    float boxH = Math.Abs(rootSc.Y - headSc.Y);
                    if (rootSc.X <= -500f || rootSc.Y <= -500f || boxH < 5f || boxH > H * 0.9f)
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
                        DrawSkeleton(dc, ent, W, H, isTeam ? _penTeam : skelPen, camMatrix);
                    }

                    // ── Snap Line ────────────────────────────────────────
                    if (EspConfig.EspLine)
                    {
                        double startX = W / 2d;
                        double startY = H; // Bottom default
                        if (EspConfig.LinePosition == EspLinePos.Top)
                            startY = 34.0; // Originates directly underneath top-center MIAN XITERS logo badge!
                        else if (EspConfig.LinePosition == EspLinePos.Center)
                            startY = H / 2d;

                        // Black shadow line first for high contrast
                        dc.DrawLine(_penBlackSnapLine,
                            new System.Windows.Point(startX, startY),
                            new System.Windows.Point(headSc.X, headSc.Y));

                        // Colored foreground snap line
                        dc.DrawLine(curLinePen,
                            new System.Windows.Point(startX, startY),
                            new System.Windows.Point(headSc.X, headSc.Y));
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
        private void DrawSkeleton(DrawingContext dc, EspEntity ent, int W, int H, Pen skelPen, Matrix4x4 camMatrix)
        {
            var head = EspW2S.WorldToScreen(camMatrix, ent.Head, W, H);
            var root = EspW2S.WorldToScreen(camMatrix, ent.Root, W, H);
            if (head.X <= 0 || head.Y <= 0 || root.X <= 0 || root.Y <= 0) return;

            // Head to Root Spine line
            dc.DrawLine(skelPen, new Point(head.X, head.Y), new Point(root.X, root.Y));

            // Shoulders line
            if (ent.LeftShoulder != Vector3.Zero && ent.RightShoulder != Vector3.Zero)
            {
                var lSh = EspW2S.WorldToScreen(camMatrix, ent.LeftShoulder, W, H);
                var rSh = EspW2S.WorldToScreen(camMatrix, ent.RightShoulder, W, H);
                if (lSh.X > 0 && lSh.Y > 0 && rSh.X > 0 && rSh.Y > 0)
                {
                    dc.DrawLine(skelPen, new Point(lSh.X, lSh.Y), new Point(rSh.X, rSh.Y));

                    // Left Arm
                    if (ent.LeftHand != Vector3.Zero)
                    {
                        var lHand = EspW2S.WorldToScreen(camMatrix, ent.LeftHand, W, H);
                        if (lHand.X > 0 && lHand.Y > 0)
                            dc.DrawLine(skelPen, new Point(lSh.X, lSh.Y), new Point(lHand.X, lHand.Y));
                    }

                    // Right Arm
                    if (ent.RightHand != Vector3.Zero)
                    {
                        var rHand = EspW2S.WorldToScreen(camMatrix, ent.RightHand, W, H);
                        if (rHand.X > 0 && rHand.Y > 0)
                            dc.DrawLine(skelPen, new Point(rSh.X, rSh.Y), new Point(rHand.X, rHand.Y));
                    }
                }
            }

            // Legs line
            if (ent.LeftFoot != Vector3.Zero && ent.RightFoot != Vector3.Zero)
            {
                var lFt = EspW2S.WorldToScreen(camMatrix, ent.LeftFoot, W, H);
                var rFt = EspW2S.WorldToScreen(camMatrix, ent.RightFoot, W, H);
                if (lFt.X > 0 && lFt.Y > 0 && rFt.X > 0 && rFt.Y > 0)
                {
                    dc.DrawLine(skelPen, new Point(root.X, root.Y), new Point(lFt.X, lFt.Y));
                    dc.DrawLine(skelPen, new Point(root.X, root.Y), new Point(rFt.X, rFt.Y));
                }
            }
        }

        // ── 360° Offscreen Radar Indicator helper ─────────────────────────────
        private void Draw360OffscreenIndicator(DrawingContext dc, Vector2 targetSc, int W, int H, float dist, Brush brush)
        {
            float centerX = W / 2f;
            float centerY = H / 2f;
            float dx = targetSc.X - centerX;
            float dy = targetSc.Y - centerY;
            float angle = (float)Math.Atan2(dy, dx);

            float radius = Math.Min(W, H) * 0.42f;
            float arrowX = centerX + (float)Math.Cos(angle) * radius;
            float arrowY = centerY + (float)Math.Sin(angle) * radius;

            // Draw red indicator dot
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
