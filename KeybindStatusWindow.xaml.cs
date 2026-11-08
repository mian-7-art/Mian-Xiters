using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace WpfApp1
{
    public partial class KeybindStatusWindow : Window
    {
        [DllImport("user32.dll")]
        private static extern uint SetWindowDisplayAffinity(IntPtr hWnd, uint dwAffinity);
        private const uint WDA_EXCLUDEFROMCAPTURE = 0x00000011;
        private const uint WDA_NONE = 0x00000000;

        private static KeybindStatusWindow _instance;
        public static KeybindStatusWindow Instance => _instance ?? (_instance = new KeybindStatusWindow());

        private SolidColorBrush _brushActive = new SolidColorBrush(Color.FromRgb(0xFF, 0x00, 0x33)); // Default Sharp Lightning Red
        private readonly SolidColorBrush _brushInactive = new SolidColorBrush(Color.FromRgb(0x71, 0x71, 0x7A)); // Muted Zinc

        public KeybindStatusWindow()
        {
            InitializeComponent();
            _instance = this;

            // Position near top-right of screen, matching video reference
            double screenWidth = SystemParameters.WorkArea.Right;
            Left = Math.Max(20, screenWidth - 210);
            Top = 80;
        }

        public void ApplyAccentColor(Color color)
        {
            Dispatcher.Invoke(() =>
            {
                _brushActive = new SolidColorBrush(color);
                if (HudOuterBorder != null) HudOuterBorder.BorderBrush = _brushActive;
                if (HudShadow != null) HudShadow.Color = color;
                if (DotHudHeader != null) DotHudHeader.Background = _brushActive;
                if (DotHudShadow != null) DotHudShadow.Color = color;
                if (TxtStatusReady != null) TxtStatusReady.Foreground = _brushActive;

                if (StatusF1 != null && StatusF1.Text == "ON") StatusF1.Foreground = _brushActive;
                if (StatusF2 != null && StatusF2.Text == "ON") StatusF2.Foreground = _brushActive;
                if (StatusF3 != null && StatusF3.Text == "ON") StatusF3.Foreground = _brushActive;
                if (StatusFastLanding != null && StatusFastLanding.Text == "ON") StatusFastLanding.Foreground = _brushActive;
            });
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        public void UpdateKeyStatus(string keyId, bool active)
        {
            Dispatcher.Invoke(() =>
            {
                switch (keyId)
                {
                    case "F1":
                    case "Aimbot":
                        if (StatusF1 != null)
                        {
                            StatusF1.Text = active ? "ON" : "OFF";
                            StatusF1.Foreground = active ? _brushActive : _brushInactive;
                        }
                        break;
                    case "F2":
                    case "GlitchFire":
                        if (StatusF2 != null)
                        {
                            StatusF2.Text = active ? "ON" : "OFF";
                            StatusF2.Foreground = active ? _brushActive : _brushInactive;
                        }
                        break;
                    case "F3":
                    case "StreamMode":
                        if (StatusF3 != null)
                        {
                            StatusF3.Text = active ? "ON" : "OFF";
                            StatusF3.Foreground = active ? _brushActive : _brushInactive;
                        }
                        break;
                    case "FastLanding":
                        if (StatusFastLanding != null)
                        {
                            StatusFastLanding.Text = active ? "ON" : "OFF";
                            StatusFastLanding.Foreground = active ? _brushActive : _brushInactive;
                        }
                        break;
                }
            });
        }

        public void UpdateAimbotKeyName(string keyName, string variant = "100%")
        {
            Dispatcher.Invoke(() =>
            {
                if (LblAimbotKey != null)
                {
                    if (string.IsNullOrEmpty(keyName) || keyName.Equals("NONE", StringComparison.OrdinalIgnoreCase))
                    {
                        LblAimbotKey.Text = $"[ NONE ] Aimbot {variant}";
                    }
                    else
                    {
                        LblAimbotKey.Text = $"[{keyName}] Aimbot {variant}";
                    }
                }
            });
        }

        public void UpdateCustomKeyName(string keyName)
        {
            Dispatcher.Invoke(() =>
            {
                if (LblFastLandingKey != null)
                {
                    if (string.IsNullOrEmpty(keyName) || keyName.Equals("NONE", StringComparison.OrdinalIgnoreCase))
                    {
                        LblFastLandingKey.Text = "[ NONE ] Fast Landing";
                    }
                    else
                    {
                        LblFastLandingKey.Text = $"[{keyName}] Fast Landing";
                    }
                }
            });
        }

        public void SetStreamMode(bool enabled)
        {
            try
            {
                IntPtr hwnd = new WindowInteropHelper(this).EnsureHandle();
                SetWindowDisplayAffinity(hwnd, enabled ? WDA_EXCLUDEFROMCAPTURE : WDA_NONE);
            }
            catch { }
        }
    }
}
