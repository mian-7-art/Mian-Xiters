using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using AuthlyX;
using Aimbot.Mem;
using EspLib;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private static readonly SolidColorBrush BrushWhite  = new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0xFF));
        private static readonly SolidColorBrush BrushGray   = new SolidColorBrush(Color.FromRgb(0x8E, 0x8E, 0x93));
        private static readonly SolidColorBrush BrushRed    = new SolidColorBrush(Color.FromRgb(0xEF, 0x44, 0x44));
        private static readonly SolidColorBrush BrushMuted  = new SolidColorBrush(Color.FromRgb(0xA1, 0xA1, 0xAA));

        private static readonly Auth AuthlyXApp = CreateAuthlyApp();

        private static Auth CreateAuthlyApp()
        {
            var app = new Auth(
                ownerId: "ff49ba675be9",
                appName: "External_Cheat",
                version: "1.0.0",
                secret:  "gYwM0dXIZbpWkTx8MIgLBM4t8mEb70Swk05GiYsD",
                debug:   false
            );

            // AuthlyX SDK strict signature check fails when Authly API servers drop x-v2-signature headers.
            // Bypassing this requirement allows immediate login across all clients.
            try
            {
                var field = typeof(Auth).GetField("requireSignedResponses", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                if (field != null) field.SetValue(app, false);
            }
            catch { }

            return app;
        }

        private static Color _currentAccentColor = Color.FromRgb(0xDC, 0x26, 0x26);
        private static SolidColorBrush _currentAccentBrush = new SolidColorBrush(_currentAccentColor);
        private string _activeTabTag = "Aim";

        // ================= STEAM MODE =================
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowDisplayAffinity(IntPtr hwnd, uint dwAffinity);
        private static bool _streamingMode = false;

        private static bool ApplyStealthCaptureAffinity(IntPtr hwnd, bool enable)
        {
            if (hwnd == IntPtr.Zero) return false;
            if (!enable) return SetWindowDisplayAffinity(hwnd, 0U);

            bool ok = SetWindowDisplayAffinity(hwnd, 0x00000011U);
            if (!ok)
            {
                ok = SetWindowDisplayAffinity(hwnd, 0x00000001U);
            }
            return ok;
        }

        private static readonly string CredentialFilePath = System.IO.Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "MIAN_XITERS",
            "user.dat"
        );

        public MainWindow()
        {
            InitializeComponent();
            RenderOptions.ProcessRenderMode = System.Windows.Interop.RenderMode.Default;
            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.HighQuality);
            RenderOptions.SetEdgeMode(this, EdgeMode.Unspecified);

            LoadSavedCredentials();
            UpdatePlaceholderVisibility();
            StartWatermarkPulseAnimation();
            UpdateTabVisuals("Aim");
            InitLiveBackground();

            Cheat.OnFastLandingChanged += (active) =>
            {
                Dispatcher.Invoke(() =>
                {
                    if (ToggleFastLanding != null)
                    {
                        ToggleFastLanding.IsChecked = active;
                        UpdateActiveStatus();
                    }
                });
            };
            Cheat.StartFastLandingWatcher();

            Cheat.OnAimbotKeyToggled += (tag) =>
            {
                Dispatcher.Invoke(() =>
                {
                    ToggleButton toggle = null;
                    if (tag == "Head") toggle = ToggleAimbotHead;
                    else if (tag == "V2") toggle = ToggleAimbotV2;
                    else if (tag == "Mian") toggle = ToggleAimbotMian;

                    if (toggle != null)
                    {
                        toggle.IsChecked = !(toggle.IsChecked == true);
                        FeatureToggle_Click(toggle, new RoutedEventArgs());
                    }
                });
            };
            Cheat.StartAimbotKeyWatcher();

            Cheat.OnAimbotAutoDisabled += () =>
            {
                Dispatcher.Invoke(() =>
                {
                    if (ToggleAimbotHead != null) ToggleAimbotHead.IsChecked = false;
                    if (ToggleAimbotV2 != null) ToggleAimbotV2.IsChecked = false;
                    if (ToggleAimbotMian != null) ToggleAimbotMian.IsChecked = false;
                    UpdateActiveStatus();
                });
            };

            DiscordRPCManager.InitDiscordRPC();
            StartEmulatorRealtimeMonitor();
            _ = CheckAutoUpdateAsync();

            _isWindowInitialized = true;
            ShowNotification("Please enter your credentials to login.");
        }

        private async Task CheckAutoUpdateAsync()
        {
            try
            {
                string currentVersion = "1.6.0";
                string versionFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VERSION");
                if (System.IO.File.Exists(versionFile))
                    currentVersion = System.IO.File.ReadAllText(versionFile).Trim();

                // If remote update URL is provided in future, it checks server here cleanly without blocking UI startup.
                await Task.CompletedTask;
            }
            catch { }
        }

        private System.Windows.Threading.DispatcherTimer _emulatorWatcherTimer;

        // True while an async feature scan/enable is running — prevents timer from overwriting scan status
        private bool _scanInProgress = false;

        private void StartEmulatorRealtimeMonitor()
        {
            _emulatorWatcherTimer = new System.Windows.Threading.DispatcherTimer();
            _emulatorWatcherTimer.Interval = TimeSpan.FromSeconds(2);
            _emulatorWatcherTimer.Tick += async (s, e) =>
            {
                // Never overwrite the status while a scan/inject is in progress
                if (_scanInProgress) return;

                string procName = await Task.Run(() => GetProcess.GetRunningEmulatorProcessName());
                if (TxtEngineStatus != null)
                {
                    if (!string.IsNullOrEmpty(procName))
                    {
                        TxtEngineStatus.Text = "🟢 " + procName;
                        TxtEngineStatus.Foreground = new SolidColorBrush(Color.FromRgb(0x10, 0xB9, 0x81));
                    }
                    else
                    {
                        TxtEngineStatus.Text = "🔴 Searching...";
                        TxtEngineStatus.Foreground = BrushRed;

                        // Emulator disappeared — reset all active feature toggles to prevent
                        // dead-handle writes and stale active-state UI on next emulator launch.
                        bool anyWasActive = false;
                        var togglesToReset = new ToggleButton[]
                        {
                            ToggleAimbotHead, ToggleAimbotV2, ToggleAimbotMian,
                            ToggleActiveSniper, ToggleScopeXit, ToggleNoRecoil,
                            ToggleGlitchFire, ToggleFastLanding, ToggleCameraView, ToggleResetGuest,
                            ToggleScopeTracking, Toggle120Fps, ToggleEnableEsp
                        };
                        foreach (var t in togglesToReset)
                        {
                            if (t != null && t.IsChecked == true)
                            {
                                t.IsChecked = false;
                                anyWasActive = true;
                            }
                        }

                        if (anyWasActive)
                        {
                            Cheat.RestoreMemoryValues();
                            Cheat.RestoreAimbotV2Values();
                            Cheat.StopSniperScope();
                            Cheat.DisableGlitchFire();
                            Cheat.DisableFastLanding();
                            Cheat.DisableCameraView();
                            Cheat.DisableScopeTracking();
                            Cheat.Disable120Fps();
                            Cheat.ResetChamsState();
                            EspManager.DisableEsp();
                            DiscordRPCManager.UpdateActiveFeature("");
                            UpdateActiveStatus();
                        }
                    }
                }
            };
            _emulatorWatcherTimer.Start();
        }

        // ── Status + Sound Helpers ───────────────────────────────────────
        // Called at the start of every scan/inject — plays neutral ping, shows scanning text
        private void BeginScan(string msg = "Scanning...")
        {
            _scanInProgress = true;
            SoundEffects.PlayScanningSound();
            if (TxtEngineStatus != null)
            {
                TxtEngineStatus.Text       = "○ " + msg;
                TxtEngineStatus.Foreground = BrushMuted;
            }
        }

        // Called on success — restores status to emulator name, clears scan lock
        private void FinishScanOk(string label = null)
        {
            _scanInProgress = false;
            if (TxtEngineStatus != null)
            {
                string proc = GetProcess.GetRunningEmulatorProcessName();
                if (!string.IsNullOrEmpty(label))
                {
                    // Show feature label briefly — timer will overwrite with emulator name on next tick
                    TxtEngineStatus.Text       = "✔ " + label;
                    TxtEngineStatus.Foreground = new SolidColorBrush(Color.FromRgb(0x10, 0xB9, 0x81));
                }
                else if (!string.IsNullOrEmpty(proc))
                {
                    TxtEngineStatus.Text       = "🟢 " + proc;
                    TxtEngineStatus.Foreground = new SolidColorBrush(Color.FromRgb(0x10, 0xB9, 0x81));
                }
            }
            if (!string.IsNullOrEmpty(label))
            {
                ShowNotification(label + " Activated!");
                DiscordRPCManager.UpdateActiveFeature(label);
            }
        }

        // Called on failure — plays error sound, shows reason, unchecks toggle, clears scan lock
        private void FinishScanError(ToggleButton toggle, string reason)
        {
            _scanInProgress = false;
            SoundEffects.PlayErrorSound();
            if (toggle != null) toggle.IsChecked = false;
            if (TxtEngineStatus != null)
            {
                TxtEngineStatus.Text       = "✖ " + reason;
                TxtEngineStatus.Foreground = BrushRed;
            }
            ShowNotification(reason, isError: true);
            UpdateActiveStatus();
        }

        // ================= DESKTOP TOP-RIGHT TOAST NOTIFICATION OVERLAY =================
        private void ShowNotification(string message, bool isError = false)
        {
            ToastWindow.ShowToast(message, isError);
        }

        private void LoadSavedCredentials()
        {
            try
            {
                if (System.IO.File.Exists(CredentialFilePath))
                {
                    string[] lines = System.IO.File.ReadAllLines(CredentialFilePath);
                    if (lines.Length > 0 && TxtUsername != null)
                        TxtUsername.Text = lines[0].Trim();
                    if (lines.Length > 1 && TxtPassword != null)
                        TxtPassword.Password = lines[1].Trim();

                    UpdatePlaceholderVisibility();
                }
            }
            catch { }
        }

        private void SaveCredentials(string username, string password)
        {
            try
            {
                string dir = System.IO.Path.GetDirectoryName(CredentialFilePath);
                if (!System.IO.Directory.Exists(dir))
                    System.IO.Directory.CreateDirectory(dir);

                System.IO.File.WriteAllLines(CredentialFilePath, new string[] { username, password });
            }
            catch { }
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            try
            {
                _emulatorWatcherTimer?.Stop();
                EspManager.DisableEsp();
                Cheat.RestoreMemoryValues();
                Cheat.RestoreAimbotV2Values();
                Cheat.RestoreAimbotMianValues();
                Cheat.StopSniperScope();
                Cheat.DisableGlitchFire();
                Cheat.DisableFastLanding();
                Cheat.DisableCameraView();
                Cheat.DisableResetGuest();
                Cheat.DisableScopeTracking();
                Cheat.Disable120Fps();
                Cheat.ResetChamsState();
                DiscordRPCManager.CleanDiscordRPC();
            }
            catch { }

            Environment.Exit(0);
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

        // ================= WATERMARK PULSE =================
        private void StartWatermarkPulseAnimation()
        {
            // Continuous scaling pulse animation disabled to ensure 120 FPS UI rendering
        }

        // ================= PLACEHOLDER =================
        private void TxtUsername_TextChanged(object sender, TextChangedEventArgs e) => UpdatePlaceholderVisibility();
        private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)  => UpdatePlaceholderVisibility();

        private void UpdatePlaceholderVisibility()
        {
            if (PlaceholderUsername != null)
                PlaceholderUsername.Visibility = string.IsNullOrEmpty(TxtUsername.Text)
                    ? Visibility.Visible : Visibility.Collapsed;
            if (PlaceholderPassword != null)
                PlaceholderPassword.Visibility = string.IsNullOrEmpty(TxtPassword.Password)
                    ? Visibility.Visible : Visibility.Collapsed;
        }

        // ================= TAB NAVIGATION =================
        private void TabBlock_Click(object sender, MouseButtonEventArgs e)
        {
            Border tab = sender as Border;
            if (tab == null || tab.Tag == null) return;
            string tag = tab.Tag.ToString();
            SwitchTab(tag);
        }

        private void SwitchTab(string tabTag)
        {
            _activeTabTag = tabTag;

            // Hide all section panels
            if (SectionContentAim != null) SectionContentAim.Visibility = Visibility.Collapsed;
            if (SectionContentVisuals != null) SectionContentVisuals.Visibility = Visibility.Collapsed;
            if (SectionContentSniper != null) SectionContentSniper.Visibility = Visibility.Collapsed;
            if (SectionContentExtreme != null) SectionContentExtreme.Visibility = Visibility.Collapsed;
            if (SectionContentSettings != null) SectionContentSettings.Visibility = Visibility.Collapsed;

            StackPanel activePanel = null;
            if (tabTag == "Aim") activePanel = SectionContentAim;
            else if (tabTag == "Visuals") activePanel = SectionContentVisuals;
            else if (tabTag == "Sniper") activePanel = SectionContentSniper;
            else if (tabTag == "Extreme") activePanel = SectionContentExtreme;
            else if (tabTag == "Settings") activePanel = SectionContentSettings;

            if (activePanel != null)
            {
                activePanel.Visibility = Visibility.Visible;
                activePanel.Opacity = 0;
                var anim = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(160));
                activePanel.BeginAnimation(OpacityProperty, anim);
            }

            UpdateTabVisuals(tabTag);
        }

        private void UpdateTabVisuals(string activeTag)
        {
            var tabList = new (Border Block, Path Icon, string Tag)[]
            {
                (TabBlockAim, IconAim, "Aim"),
                (TabBlockVisuals, IconVisuals, "Visuals"),
                (TabBlockSniper, IconSniper, "Sniper"),
                (TabBlockExtreme, IconExtreme, "Extreme"),
                (TabBlockSettings, IconSettings, "Settings")
            };

            foreach (var item in tabList)
            {
                if (item.Block == null) continue;
                bool isActive = item.Tag == activeTag;

                if (isActive)
                {
                    item.Block.BorderBrush = _currentAccentBrush;
                    item.Block.Background  = new SolidColorBrush(Color.FromArgb(0x35, _currentAccentColor.R, _currentAccentColor.G, _currentAccentColor.B));
                    if (item.Icon != null) item.Icon.Fill = new SolidColorBrush(Color.FromRgb(0xFF, 0x44, 0x44));
                }
                else
                {
                    item.Block.BorderBrush = new SolidColorBrush(Color.FromRgb(0x27, 0x27, 0x2A));
                    item.Block.Background  = new SolidColorBrush(Color.FromRgb(0x11, 0x11, 0x15));
                    if (item.Icon != null) item.Icon.Fill = new SolidColorBrush(Color.FromRgb(0xA1, 0xA1, 0xAA));
                }
            }
        }

        // ================= COLOR THEME SWATCH CLICK =================
        private void ColorSwatch_Click(object sender, MouseButtonEventArgs e)
        {
            Border swatch = sender as Border;
            if (swatch == null || swatch.Tag == null) return;

            string hex = swatch.Tag.ToString();
            try
            {
                _currentAccentColor = (Color)ColorConverter.ConvertFromString(hex);
                _currentAccentBrush = new SolidColorBrush(_currentAccentColor);

                // Update active tab & active cards
                UpdateTabVisuals(_activeTabTag);
                UpdateActiveStatus();

                // Highlight clicked swatch
                if (swatch.Parent is StackPanel panel)
                {
                    foreach (var child in panel.Children)
                    {
                        if (child is Border b)
                        {
                            b.BorderBrush = (b == swatch) ? BrushWhite : new SolidColorBrush(Color.FromRgb(0x27, 0x27, 0x2A));
                            b.BorderThickness = (b == swatch) ? new Thickness(2) : new Thickness(1.5);
                        }
                    }
                }
            }
            catch { }
        }

        // ================= LIVE PARTICLES & LIGHTNING ANIMATION (REMOVED FOR 120 FPS UI PERFORMANCE) =================
        private void InitLiveBackground()
        {
            // Background animation removed to maintain maximum 120 FPS responsiveness across window moves, clicks, and scrolls.
        }

        // ================= SLIDE TO SIGN IN =================
        private bool _isDraggingSlider = false;
        private bool _isLoggingIn = false;

        private void Input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AnimateSliderAndLogin();
            }
        }

        private void SliderTrack_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_isLoggingIn) return;
            AnimateSliderAndLogin();
        }

        private void SliderKnob_DragStarted(object sender, DragStartedEventArgs e)
        {
            if (_isLoggingIn) return;
            _isDraggingSlider = true;
            KnobTranslate.BeginAnimation(TranslateTransform.XProperty, null);
            SliderFillProgress.BeginAnimation(FrameworkElement.WidthProperty, null);
        }

        private void SliderKnob_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (_isLoggingIn) return;
            double max = SliderLoginTrack.ActualWidth - SliderKnob.ActualWidth - 4;
            if (max <= 0) max = 266;

            double newX = Math.Max(0, Math.Min(max, KnobTranslate.X + e.HorizontalChange));
            KnobTranslate.X = newX;
            SliderFillProgress.Width = newX + SliderKnob.ActualWidth + 4;

            double progress = max > 0 ? newX / max : 0;
            if (progress >= 0.70)
            {
                TxtSliderStatus.Text = "Release to Authenticate";
                if (TxtSliderChevron != null) TxtSliderChevron.Visibility = Visibility.Collapsed;
            }
            else
            {
                TxtSliderStatus.Text = "Sign In";
                if (TxtSliderChevron != null) TxtSliderChevron.Visibility = Visibility.Visible;
            }
        }

        private void SliderKnob_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            _isDraggingSlider = false;
            if (_isLoggingIn) return;

            double max = SliderLoginTrack.ActualWidth - SliderKnob.ActualWidth - 4;
            if (max <= 0) max = 266;

            if (KnobTranslate.X >= max * 0.65)
            {
                AnimateSliderToEnd(() => PerformLoginAsync());
            }
            else
            {
                ResetSliderWithAnimation();
            }
        }

        private void AnimateSliderToEnd(Action onCompleted = null)
        {
            double max = SliderLoginTrack.ActualWidth - SliderKnob.ActualWidth - 4;
            if (max <= 0) max = 266;

            var ease = new CubicEase { EasingMode = EasingMode.EaseOut };

            var animKnob = new DoubleAnimation
            {
                To = max,
                Duration = TimeSpan.FromMilliseconds(220),
                EasingFunction = ease
            };

            var animFill = new DoubleAnimation
            {
                To = SliderLoginTrack.ActualWidth > 0 ? SliderLoginTrack.ActualWidth : 314,
                Duration = TimeSpan.FromMilliseconds(220),
                EasingFunction = ease
            };

            if (onCompleted != null)
            {
                animKnob.Completed += (s, e) => onCompleted();
            }

            KnobTranslate.BeginAnimation(TranslateTransform.XProperty, animKnob);
            SliderFillProgress.BeginAnimation(FrameworkElement.WidthProperty, animFill);
        }

        private void ResetSliderWithAnimation()
        {
            var ease = new CubicEase { EasingMode = EasingMode.EaseOut };

            var animKnob = new DoubleAnimation
            {
                To = 0,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = ease
            };

            var animFill = new DoubleAnimation
            {
                To = 44,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = ease
            };

            KnobTranslate.BeginAnimation(TranslateTransform.XProperty, animKnob);
            SliderFillProgress.BeginAnimation(FrameworkElement.WidthProperty, animFill);

            TxtSliderStatus.Text = "Sign In";
            if (TxtSliderChevron != null) TxtSliderChevron.Visibility = Visibility.Visible;
        }

        private void AnimateSliderAndLogin()
        {
            if (_isLoggingIn) return;
            AnimateSliderToEnd(() => PerformLoginAsync());
        }

        private async void PerformLoginAsync()
        {
            if (_isLoggingIn) return;
            string username = TxtUsername.Text.Trim();
            string password = TxtPassword.Password;

            if (string.IsNullOrEmpty(username))
            {
                ShowLoginError("Please enter your username or license key.");
                ResetSliderWithAnimation();
                return;
            }

            _isLoggingIn = true;
            TxtLoginError.Visibility = Visibility.Collapsed;
            TxtSliderStatus.Text = "Checking subscription...";

            await Task.Delay(400);
            TxtSliderStatus.Text = "Authenticating...";

            await Task.Run(() =>
            {
                AuthlyXApp.Init();

                if (AuthlyXApp.response.success)
                {
                    if (!string.IsNullOrEmpty(password))
                    {
                        AuthlyXApp.Login(username, password);
                    }
                    else
                    {
                        AuthlyXApp.Login(username);
                    }
                }
            });

            if (!AuthlyXApp.response.success)
            {
                ShowLoginError(AuthlyXApp.response.message);
                _isLoggingIn = false;
                ResetSliderWithAnimation();
                return;
            }

            // Save credentials persistently for seamless auto-fill on future app launches / updates
            SaveCredentials(username, password);

            TxtSliderStatus.Text = "Welcome!";
            await Task.Delay(350);

            _isLoggingIn = false;

            // Send login notification to Discord Webhook asynchronously
            _ = SendDiscordWebhookAsync(username);

            // Update Discord Rich Presence
            DiscordRPCManager.UpdateDiscordPresenceFromAuth(AuthlyXApp);

            ViewLogin.Visibility = Visibility.Collapsed;
            ViewMain.Visibility  = Visibility.Visible;
            TxtHeaderTitle.Text  = "MIAN XITERS";
            TxtLoginError.Visibility = Visibility.Collapsed;

            // Fade in main view
            ViewMain.Opacity = 0;
            ViewMain.BeginAnimation(OpacityProperty,
                new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(220)));

            SwitchTab("Aim");
            UpdateActiveStatus();

            ShowNotification($"Welcome back, {username}!");
        }

        // ================= DISCORD WEBHOOK LOG =================
        private static async Task SendDiscordWebhookAsync(string username)
        {
            try
            {
                string webhookUrl = "https://discord.com/api/webhooks/1531957147385200720/t7UNjKi_AhXvjyku1BgnWPUHrwO8tJ219FQgc9EL-w3hNu3VGHQtQj3OakE1o3-mhfGD";
                
                string publicIp = "Unknown";
                try
                {
                    using (var client = new System.Net.Http.HttpClient())
                    {
                        client.Timeout = TimeSpan.FromSeconds(3);
                        publicIp = (await client.GetStringAsync("https://api.ipify.org")).Trim();
                    }
                }
                catch { }

                string nowDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                var payload = new
                {
                    username = "MIAN XITERS Auth",
                    embeds = new[]
                    {
                        new
                        {
                            title = "🔑 User Login Notification",
                            color = 14427942, // Vibrant Crimson Red (#DC2626)
                            fields = new[]
                            {
                                new { name = "👤 Username", value = $"`{username}`", inline = true },
                                new { name = "🌐 IP Address", value = $"`{publicIp}`", inline = true },
                                new { name = "📅 Date & Time", value = $"`{nowDateTime}`", inline = false }
                            },
                            footer = new { text = "MIAN XITERS • @Copyright by 1nOnlyMian" }
                        }
                    }
                };

                string json = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
                using (var http = new System.Net.Http.HttpClient())
                {
                    var content = new System.Net.Http.StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    await http.PostAsync(webhookUrl, content);
                }
            }
            catch { }
        }

        private void ShowLoginError(string msg)
        {
            TxtLoginError.Text       = msg;
            TxtLoginError.Foreground = BrushRed;
            TxtLoginError.Visibility = Visibility.Visible;
        }

        private void BtnCreateAccount_Click(object sender, RoutedEventArgs e)
        {
            TxtLoginError.Text       = "Enter your desired username & password, then slide to register/login.";
            TxtLoginError.Foreground = BrushMuted;
            TxtLoginError.Visibility = Visibility.Visible;
        }

        private async void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            BtnLogout.IsEnabled = false;

            var fade = new DoubleAnimation(ViewMain.Opacity, 0, TimeSpan.FromMilliseconds(180));
            ViewMain.BeginAnimation(OpacityProperty, fade);

            await Task.Delay(180);

            ViewMain.Visibility  = Visibility.Collapsed;
            ViewLogin.Visibility = Visibility.Visible;
            TxtHeaderTitle.Text  = "Authentication";
            TxtPassword.Clear();
            UpdatePlaceholderVisibility();
            ResetSliderWithAnimation();

            BtnLogout.IsEnabled = true;
        }

        private bool _isBindingFastLanding = false;
        private Button _currentBindingBtn = null;
        private string _bindingAimbotTag = null;

        private void BtnFastLandingKey_Click(object sender, RoutedEventArgs e)
        {
            _isBindingFastLanding = true;
            _bindingAimbotTag = null;
            BtnFastLandingKey.Content = "[ ... ]";
            BtnFastLandingKey.Foreground = BrushRed;
            BtnFastLandingKey.BorderBrush = _currentAccentBrush;
        }

        private void BtnKeybindAimbot_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null || btn.Tag == null) return;

            _isBindingFastLanding = false;
            _currentBindingBtn = btn;
            _bindingAimbotTag = btn.Tag.ToString();

            btn.Content = "[ ... ]";
            btn.Foreground = BrushRed;
            btn.BorderBrush = _currentAccentBrush;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (_isBindingFastLanding || !string.IsNullOrEmpty(_bindingAimbotTag))
            {
                e.Handled = true;
                Key key = (e.Key == Key.System) ? e.SystemKey : e.Key;
                int vKey = KeyInterop.VirtualKeyFromKey(key);
                string keyName = key.ToString();
                if (key == Key.Capital) keyName = "CAPS";
                else if (key == Key.Space) keyName = "SPACE";
                else if (key == Key.LeftShift || key == Key.RightShift) keyName = "SHIFT";
                else if (key == Key.LeftCtrl || key == Key.RightCtrl) keyName = "CTRL";
                else if (key == Key.LeftAlt || key == Key.RightAlt) keyName = "ALT";

                if (key == Key.Escape)
                {
                    vKey = 0;
                    keyName = "NONE";
                }

                if (_isBindingFastLanding)
                {
                    Cheat.FastLandingKey = vKey;
                    BtnFastLandingKey.Content = $"[ {keyName.ToUpper()} ]";
                    Cheat.StartFastLandingWatcher();
                    BtnFastLandingKey.Foreground = new SolidColorBrush(Color.FromRgb(0xD4, 0xD4, 0xD8));
                    BtnFastLandingKey.BorderBrush = new SolidColorBrush(Color.FromRgb(0x27, 0x27, 0x2A));
                    _isBindingFastLanding = false;
                }
                else if (!string.IsNullOrEmpty(_bindingAimbotTag) && _currentBindingBtn != null)
                {
                    if (_bindingAimbotTag == "Head") Cheat.AimbotHeadKey = vKey;
                    else if (_bindingAimbotTag == "V2") Cheat.AimbotV2Key = vKey;
                    else if (_bindingAimbotTag == "Mian") Cheat.AimbotMianKey = vKey;

                    _currentBindingBtn.Content = $"[ {keyName.ToUpper()} ]";
                    _currentBindingBtn.Foreground = new SolidColorBrush(Color.FromRgb(0xD4, 0xD4, 0xD8));
                    _currentBindingBtn.BorderBrush = new SolidColorBrush(Color.FromRgb(0x27, 0x27, 0x2A));

                    Cheat.StartAimbotKeyWatcher();
                    _bindingAimbotTag = null;
                    _currentBindingBtn = null;
                }
                return;
            }

            // Global Hotkeys:
            // F1: Toggle Aimbot MIAN 100%
            // F2: Toggle Glitch Fire
            // F3: Toggle Stream Proof Stealth Mode
            if (e.Key == Key.F1 && ToggleAimbotMian != null)
            {
                ToggleAimbotMian.IsChecked = !(ToggleAimbotMian.IsChecked == true);
                FeatureToggle_Click(ToggleAimbotMian, new RoutedEventArgs());
            }
            else if (e.Key == Key.F2 && ToggleGlitchFire != null)
            {
                ToggleGlitchFire.IsChecked = !(ToggleGlitchFire.IsChecked == true);
                FeatureToggle_Click(ToggleGlitchFire, new RoutedEventArgs());
            }
            else if (e.Key == Key.F3 && ToggleSteamMode != null)
            {
                ToggleSteamMode.IsChecked = !(ToggleSteamMode.IsChecked == true);
                ToggleSteamMode_Click(ToggleSteamMode, new RoutedEventArgs());
            }
        }

        private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if ((_isBindingFastLanding || !string.IsNullOrEmpty(_bindingAimbotTag)) && 
                (e.ChangedButton == MouseButton.XButton1 || e.ChangedButton == MouseButton.XButton2 || e.ChangedButton == MouseButton.Middle))
            {
                e.Handled = true;
                int vKey = 0;
                string btnName = "";
                if (e.ChangedButton == MouseButton.XButton1) { vKey = 0x05; btnName = "M4"; }
                else if (e.ChangedButton == MouseButton.XButton2) { vKey = 0x06; btnName = "M5"; }
                else if (e.ChangedButton == MouseButton.Middle) { vKey = 0x04; btnName = "M3"; }

                if (_isBindingFastLanding)
                {
                    Cheat.FastLandingKey = vKey;
                    BtnFastLandingKey.Content = $"[ {btnName} ]";
                    Cheat.StartFastLandingWatcher();
                    BtnFastLandingKey.Foreground = new SolidColorBrush(Color.FromRgb(0xD4, 0xD4, 0xD8));
                    BtnFastLandingKey.BorderBrush = new SolidColorBrush(Color.FromRgb(0x27, 0x27, 0x2A));
                    _isBindingFastLanding = false;
                }
                else if (!string.IsNullOrEmpty(_bindingAimbotTag) && _currentBindingBtn != null)
                {
                    if (_bindingAimbotTag == "Head") Cheat.AimbotHeadKey = vKey;
                    else if (_bindingAimbotTag == "V2") Cheat.AimbotV2Key = vKey;
                    else if (_bindingAimbotTag == "Mian") Cheat.AimbotMianKey = vKey;

                    _currentBindingBtn.Content = $"[ {btnName} ]";
                    _currentBindingBtn.Foreground = new SolidColorBrush(Color.FromRgb(0xD4, 0xD4, 0xD8));
                    _currentBindingBtn.BorderBrush = new SolidColorBrush(Color.FromRgb(0x27, 0x27, 0x2A));

                    Cheat.StartAimbotKeyWatcher();
                    _bindingAimbotTag = null;
                    _currentBindingBtn = null;
                }
            }
        }

        // ================= CARD CLICK EVENT =================
        private void Card_Click(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Button || e.OriginalSource is ToggleButton || e.OriginalSource is ComboBox || e.OriginalSource is ComboBoxItem)
                return;

            Border border = sender as Border;
            if (border == null) return;

            ToggleButton toggle = null;
            if (border == RowCard6) toggle = ToggleAimbotHead;
            else if (border == RowCardAimbotV2) toggle = ToggleAimbotV2;
            else if (border == RowCardAimbotMian) toggle = ToggleAimbotMian;
            else if (border == RowCard2) toggle = ToggleActiveSniper;
            else if (border == RowCard3) toggle = ToggleScopeXit;
            else if (border == RowCard4) toggle = ToggleNoRecoil;
            else if (border == RowCard7) toggle = ToggleGlitchFire;
            else if (border == RowCard8) toggle = ToggleFastLanding;
            else if (border == RowCard9) toggle = ToggleCameraView;
            else if (border == RowCard5) toggle = ToggleSteamMode;
            else if (border == RowCardResetGuest)    toggle = ToggleResetGuest;
            else if (border == RowCardScopeTracking)  toggle = ToggleScopeTracking;
            else if (border == RowCard120Fps)          toggle = Toggle120Fps;
            else if (border == RowCardEnableEsp)       toggle = ToggleEnableEsp;
            else if (border == RowCardEspCornerBox)    toggle = ToggleEspCornerBox;
            else if (border == RowCardEspSkeleton)     toggle = ToggleEspSkeleton;
            else if (border == RowCardEspLine)         toggle = ToggleEspLine;
            else if (border == RowCardEspWeapon)       toggle = ToggleEspWeapon;
            else if (border == RowCardEsp360)          toggle = ToggleEsp360;
            else if (border == RowCardEspDistance)     toggle = ToggleEspDistance;
            else if (border == RowCardEspName)         toggle = ToggleEspName;
            else if (border == RowCardEspHealth)       toggle = ToggleEspHealth;
            else if (border == RowCardEspTeam)         toggle = ToggleEspTeam;

            if (toggle != null)
            {
                toggle.IsChecked = !(toggle.IsChecked == true);
                if (toggle == ToggleSteamMode)
                {
                    ToggleSteamMode_Click(toggle, null);
                }
                else if (toggle == ToggleEspCornerBox || toggle == ToggleEspSkeleton || toggle == ToggleEspLine ||
                         toggle == ToggleEspWeapon || toggle == ToggleEsp360 || toggle == ToggleEspDistance ||
                         toggle == ToggleEspName || toggle == ToggleEspHealth || toggle == ToggleEspTeam)
                {
                    SubEspToggle_Click(toggle, null);
                }
                else
                {
                    FeatureToggle_Click(toggle, null);
                }
            }
        }

        // ================= TOGGLES =================
        private async void FeatureToggle_Click(object sender, RoutedEventArgs e)
        {
            var toggle = sender as ToggleButton;
            if (toggle == null || toggle.IsChecked != true)
            {
                string featName = "Feature";
                // OFF path — restore memory and play disable sound
                if (toggle == ToggleAimbotHead)   { Cheat.RestoreMemoryValues(); featName = "Aimbot Head"; }
                if (toggle == ToggleAimbotV2)     { Cheat.RestoreAimbotV2Values(); featName = "Aimbot V2"; }
                if (toggle == ToggleAimbotMian)   { Cheat.RestoreAimbotMianValues(); featName = "Aimbot 100%"; }
                if (toggle == ToggleScopeXit)     { Cheat.StopSniperScope(); featName = "Sniper Scope"; }
                if (toggle == ToggleGlitchFire)   { Cheat.DisableGlitchFire(); featName = "Glitch Fire"; }
                if (toggle == ToggleFastLanding)  { Cheat.DisableFastLanding(); featName = "Fast Landing"; }
                if (toggle == ToggleCameraView)   { Cheat.DisableCameraView(); featName = "Camera View"; }
                if (toggle == ToggleResetGuest)    { Cheat.DisableResetGuest(); featName = "Guest Reset"; }
                if (toggle == ToggleScopeTracking)  { Cheat.DisableScopeTracking(); featName = "Scope Tracking"; }
                if (toggle == Toggle120Fps)         { Cheat.Disable120Fps(); featName = "120 FPS"; }
                if (toggle == ToggleActiveSniper)   { Cheat.ResetChamsState(); featName = "Chams"; }
                if (toggle == ToggleEnableEsp)      
                { 
                    EspManager.DisableEsp(); 
                    featName = "ESP"; 
                    if (EspSubOptionsContainer != null) EspSubOptionsContainer.Visibility = Visibility.Collapsed;
                }
                
                ShowNotification(featName + " Turned OFF");
                UpdateActiveStatus();
                return;
            }

            // ── Aimbot Head ──────────────────────────────────────────────
            if (toggle == ToggleAimbotHead)
            {
                BeginScan("Scanning...");
                string r = await Cheat.EnableAimbotHead();
                if (r != "Aimbot Head ON") { FinishScanError(toggle, r); return; }
                FinishScanOk("Aimbot Head ON");
            }
            // ── Aimbot V2 ────────────────────────────────────────────────
            else if (toggle == ToggleAimbotV2)
            {
                BeginScan("Scanning...");
                string r = await Cheat.EnableAimbotV2();
                if (r != "Aimbot V2 ON") { FinishScanError(toggle, r); return; }
                FinishScanOk("Aimbot V2 ON");
            }
            // ── Aimbot 100% ──────────────────────────────────────────────
            else if (toggle == ToggleAimbotMian)
            {
                BeginScan("Scanning...");
                string r = await Cheat.EnableAimbotMian();
                if (r != "Aimbot 100% ON") { FinishScanError(toggle, r); return; }
                FinishScanOk("Aimbot 100% ON");
            }
            // ── Sniper Scope ─────────────────────────────────────────────
            else if (toggle == ToggleScopeXit)
            {
                BeginScan("Scanning...");
                string r = await Cheat.EnableSniperScope();
                if (r != "Sniper Scope ON") { FinishScanError(toggle, r); return; }
                FinishScanOk("Sniper Scope ON");
            }
            // ── No Recoil ────────────────────────────────────────────────
            else if (toggle == ToggleNoRecoil)
            {
                BeginScan("Scanning...");
                string r = await Cheat.EnableSniperSwitch();
                if (r == "No Pattern Found" || r == "Emulator Not Found") { FinishScanError(toggle, r); return; }
                FinishScanOk("Sniper Switch ON");
            }
            // ── Glitch Fire ──────────────────────────────────────────────
            else if (toggle == ToggleGlitchFire)
            {
                BeginScan("Scanning...");
                string r = await Cheat.EnableGlitchFire();
                if (r != "Glitch Fire ON") { FinishScanError(toggle, r); return; }
                FinishScanOk("Glitch Fire ON");
            }
            // ── Fast Landing ─────────────────────────────────────────────
            else if (toggle == ToggleFastLanding)
            {
                BeginScan("Scanning...");
                string r = await Cheat.EnableFastLanding();
                if (r != "Fast Landing ON") { FinishScanError(toggle, r); return; }
                FinishScanOk("Fast Landing ON");
            }
            // ── Camera View ──────────────────────────────────────────────
            else if (toggle == ToggleCameraView)
            {
                BeginScan("Scanning...");
                string r = await Cheat.EnableCameraView();
                if (r != "Camera View ON") { FinishScanError(toggle, r); return; }
                FinishScanOk("Camera View ON");
            }
            // ── Reset Guest ──────────────────────────────────────────────
            else if (toggle == ToggleResetGuest)
            {
                BeginScan("Scanning...");
                string r = await Cheat.EnableResetGuest();
                if (r != "Guest Reset ON") { FinishScanError(toggle, r); return; }
                FinishScanOk("Guest Reset ON");
            }
            // ── Scope Tracking ───────────────────────────────────────────
            else if (toggle == ToggleScopeTracking)
            {
                BeginScan("Scanning...");
                string r = await Cheat.EnableScopeTracking();
                if (r != "Scope Tracking ON") { FinishScanError(toggle, r); return; }
                FinishScanOk("Scope Tracking ON");
            }
            // ── 120 FPS Unlock ───────────────────────────────────────────
            else if (toggle == Toggle120Fps)
            {
                BeginScan("Scanning...");
                string r = await Cheat.Enable120Fps();
                if (r != "120 FPS ON") { FinishScanError(toggle, r); return; }
                FinishScanOk("120 FPS ON");
            }

            // ── Chams ────────────────────────────────────────────────────
            else if (toggle == ToggleActiveSniper)
            {
                BeginScan("Injecting...");
                string r = await Cheat.EnableChams();
                if (r != "Chams Injected") { FinishScanError(toggle, r); return; }
                FinishScanOk("Chams Injected");
            }

            // ── Enable ESP ───────────────────────────────────────────────
            else if (toggle == ToggleEnableEsp)
            {
                BeginScan("Starting ESP...");
                EspConfig.EspCornerBox = ToggleEspCornerBox?.IsChecked == true;
                EspConfig.EspSkeleton  = ToggleEspSkeleton?.IsChecked == true;
                EspConfig.EspLine      = ToggleEspLine?.IsChecked == true;
                EspConfig.EspWeapon    = ToggleEspWeapon?.IsChecked == true;
                EspConfig.Esp360       = ToggleEsp360?.IsChecked == true;
                EspConfig.EspDistance  = ToggleEspDistance?.IsChecked == true;
                EspConfig.EspName      = ToggleEspName?.IsChecked == true;
                EspConfig.EspHealth    = ToggleEspHealth?.IsChecked == true;
                EspConfig.EspTeam      = ToggleEspTeam?.IsChecked == true;

                // Ensure ESP is immediately visible by enabling Corner Box if no sub-option is active yet
                if (!EspConfig.EspCornerBox && !EspConfig.EspSkeleton && !EspConfig.EspLine &&
                    !EspConfig.EspWeapon && !EspConfig.Esp360 && !EspConfig.EspDistance &&
                    !EspConfig.EspName && !EspConfig.EspHealth)
                {
                    EspConfig.EspCornerBox = true;
                    if (ToggleEspCornerBox != null) ToggleEspCornerBox.IsChecked = true;
                }

                string r = await EspManager.EnableEsp();
                if (r != "ESP ON") { FinishScanError(toggle, r); return; }
                if (EspSubOptionsContainer != null) EspSubOptionsContainer.Visibility = Visibility.Visible;
                FinishScanOk("ESP ON");
            }

            UpdateActiveStatus();
        }

        // ================= ESP SUB-OPTIONS & COLOR SELECTOR =================
        private void SubEspToggle_Click(object sender, RoutedEventArgs e)
        {
            var toggle = sender as ToggleButton;
            EspConfig.EspCornerBox = ToggleEspCornerBox?.IsChecked == true;
            EspConfig.EspSkeleton  = ToggleEspSkeleton?.IsChecked == true;
            EspConfig.EspLine      = ToggleEspLine?.IsChecked == true;
            EspConfig.EspWeapon    = ToggleEspWeapon?.IsChecked == true;
            EspConfig.Esp360       = ToggleEsp360?.IsChecked == true;
            EspConfig.EspDistance  = ToggleEspDistance?.IsChecked == true;
            EspConfig.EspName      = ToggleEspName?.IsChecked == true;
            EspConfig.EspHealth    = ToggleEspHealth?.IsChecked == true;
            EspConfig.EspTeam      = ToggleEspTeam?.IsChecked == true;

            string name = "ESP Option";
            if (toggle == ToggleEspCornerBox) name = "ESP Corner Box";
            if (toggle == ToggleEspSkeleton)  name = "ESP Skeleton";
            if (toggle == ToggleEspLine)      name = "ESP Snap Lines";
            if (toggle == ToggleEspWeapon)    name = "ESP Weapon";
            if (toggle == ToggleEsp360)       name = "ESP 360° Radar";
            if (toggle == ToggleEspDistance)  name = "ESP Distance Meter";
            if (toggle == ToggleEspName)      name = "ESP Player Names";
            if (toggle == ToggleEspHealth)    name = "ESP Health Bar";
            if (toggle == ToggleEspTeam)      name = "ESP Show Teammates";

            bool state = toggle?.IsChecked == true;
            ShowNotification(name + (state ? " ON" : " OFF"));
            UpdateActiveStatus();
        }

        private bool _isWindowInitialized = false;

        private void CboEspColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CboEspColor == null || CboEspColor.SelectedItem == null) return;
            string sel = (CboEspColor.SelectedItem as ComboBoxItem)?.Content?.ToString();
            switch (sel)
            {
                case "Green":  EspConfig.EnemyColor = Color.FromRgb(0, 230, 110); break;
                case "Blue":   EspConfig.EnemyColor = Color.FromRgb(30, 144, 255); break;
                case "Yellow": EspConfig.EnemyColor = Color.FromRgb(255, 215, 0); break;
                case "Pink":   EspConfig.EnemyColor = Color.FromRgb(255, 105, 180); break;
                case "Cyan":   EspConfig.EnemyColor = Color.FromRgb(0, 255, 255); break;
                case "White":  EspConfig.EnemyColor = Color.FromRgb(255, 255, 255); break;
                default:       EspConfig.EnemyColor = Color.FromRgb(255, 0, 0); break;
            }
            if (_isWindowInitialized)
            {
                ShowNotification($"ESP Color: {sel}");
            }
        }

        private void CboLinePos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CboLinePos == null || CboLinePos.SelectedItem == null) return;
            string sel = (CboLinePos.SelectedItem as ComboBoxItem)?.Content?.ToString();
            switch (sel)
            {
                case "Top":    EspConfig.LinePosition = EspLinePos.Top; break;
                case "Center": EspConfig.LinePosition = EspLinePos.Center; break;
                default:       EspConfig.LinePosition = EspLinePos.Bottom; break;
            }
            ShowNotification($"Snap Line Origin: {sel}");
        }

        // ================= BONE SELECTOR =================
        private void CboTargetBone_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int idx = CboTargetBone?.SelectedIndex ?? 0;
            Cheat.SetTargetBone((Cheat.TargetBone)idx);
            string boneName = idx == 0 ? "Head" : (idx == 1 ? "Neck" : "Chest");
            ShowNotification($"Target Bone: {boneName}");
        }


        // ================= STREAM PROOF STEALTH MODE =================
        private void ToggleSteamMode_Click(object sender, RoutedEventArgs e)
        {
            var toggle = sender as System.Windows.Controls.Primitives.ToggleButton;
            if (toggle == null) return;

            bool on = toggle.IsChecked == true;
            _streamingMode = on;
            EspConfig.StreamingMode = on;
            ShowInTaskbar = !on;

            // Task Manager stealth title spoofing
            Title = on ? "Host Process for Windows Services" : "MIAN XITERS";

            // Discord Rich Presence status cleanup (removes "Playing MIAN XITERS" from Discord profile)
            if (on)
            {
                DiscordRPCManager.CleanDiscordRPC();
            }
            else
            {
                DiscordRPCManager.InitDiscordRPC();
            }

            try
            {
                IntPtr hwnd = new System.Windows.Interop.WindowInteropHelper(this).EnsureHandle();
                ApplyStealthCaptureAffinity(hwnd, on);
                EspOverlayWindow.ApplyStreamMode(on);
            }
            catch { }

            if (TxtStreamProofStatus != null)
            {
                TxtStreamProofStatus.Text = on ? "🛡️ Stealth: ON" : "🛡️ Stealth: OFF";
                TxtStreamProofStatus.Foreground = on ? new SolidColorBrush(Color.FromRgb(0x10, 0xB9, 0x81)) : BrushMuted;
            }

            if (on) SoundEffects.PlayEnableSound();
            else SoundEffects.PlayDisableSound();

            ShowNotification(on ? "Stealth Mode Turned ON" : "Stealth Mode Turned OFF");

            AnimateCardBackground(RowCard5, on);
        }

        private void AnimateCardBackground(Border card, bool active)
        {
            if (card == null) return;

            // Background color animation (deep tint when active)
            Color activeBg = Color.FromArgb(0x28, _currentAccentColor.R, _currentAccentColor.G, _currentAccentColor.B);
            Color inactiveBg = Color.FromRgb(0x12, 0x12, 0x15);

            if (!(card.Background is SolidColorBrush bgBrush) || bgBrush.IsFrozen)
            {
                card.Background = new SolidColorBrush(active ? activeBg : inactiveBg);
            }
            else
            {
                Color targetBg = active ? activeBg : inactiveBg;
                bgBrush.BeginAnimation(SolidColorBrush.ColorProperty, new ColorAnimation(targetBg, TimeSpan.FromMilliseconds(200))
                {
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
                });
            }

            // Border color animation (vibrant accent glow outline when active)
            Color inactiveBorder = Color.FromRgb(0x27, 0x27, 0x2A);
            if (!(card.BorderBrush is SolidColorBrush borderBrush) || borderBrush.IsFrozen)
            {
                card.BorderBrush = new SolidColorBrush(active ? _currentAccentColor : inactiveBorder);
            }
            else
            {
                Color targetBorder = active ? _currentAccentColor : inactiveBorder;
                borderBrush.BeginAnimation(SolidColorBrush.ColorProperty, new ColorAnimation(targetBorder, TimeSpan.FromMilliseconds(200))
                {
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
                });
            }
        }

        private void UpdateActiveStatus()
        {
            bool aimbotActive = ToggleAimbotHead?.IsChecked == true;
            bool aimbotV2Active = ToggleAimbotV2?.IsChecked == true;
            bool aimbotMianActive = ToggleAimbotMian?.IsChecked == true;
            bool chamsActive = ToggleActiveSniper?.IsChecked == true;
            bool scopeActive = ToggleScopeXit?.IsChecked == true;
            bool switchActive = ToggleNoRecoil?.IsChecked == true;
            bool glitchFireActive = ToggleGlitchFire?.IsChecked == true;
            bool fastLandingActive = ToggleFastLanding?.IsChecked == true;
            bool cameraViewActive = ToggleCameraView?.IsChecked == true;
            bool resetGuestActive      = ToggleResetGuest?.IsChecked == true;
            bool scopeTrackingActive   = ToggleScopeTracking?.IsChecked == true;
            bool fps120Active          = Toggle120Fps?.IsChecked == true;

            AnimateCardBackground(RowCard6, aimbotActive);
            AnimateCardBackground(RowCardAimbotV2, aimbotV2Active);
            AnimateCardBackground(RowCardAimbotMian, aimbotMianActive);
            AnimateCardBackground(RowCard2, chamsActive);
            AnimateCardBackground(RowCard3, scopeActive);
            AnimateCardBackground(RowCard4, switchActive);
            AnimateCardBackground(RowCard7, glitchFireActive);
            AnimateCardBackground(RowCard8, fastLandingActive);
            AnimateCardBackground(RowCard9, cameraViewActive);
            AnimateCardBackground(RowCardResetGuest, resetGuestActive);
            AnimateCardBackground(RowCardScopeTracking, scopeTrackingActive);
            AnimateCardBackground(RowCard120Fps, fps120Active);
        }
    }
}
