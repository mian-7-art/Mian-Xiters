using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace WpfApp1
{
    public partial class ToastWindow : Window
    {
        public static volatile bool IsHidden = false;
        private static ToastWindow _activeToast = null;
        private Storyboard _storyboard;

        public ToastWindow(string message, bool isError = false)
        {
            InitializeComponent();

            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.HighQuality);
            RenderOptions.SetClearTypeHint(this, ClearTypeHint.Enabled);

            TxtMessage.Text = message;
            TxtMessage.Foreground = isError 
                ? new SolidColorBrush(Color.FromRgb(0xEF, 0x44, 0x44)) 
                : new SolidColorBrush(Color.FromRgb(0xF4, 0xF4, 0xF5));

            LoadLogoImage();
            PositionTopRight();
        }

        private void LoadLogoImage()
        {
            try
            {
                // Guaranteed load from embedded assembly resource stream
                var uri = new Uri("pack://application:,,,/Resources/logo.png", UriKind.Absolute);
                var bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.UriSource = uri;
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.EndInit();
                bmp.Freeze();
                ImgLogo.Source = bmp;
            }
            catch
            {
                try
                {
                    string logoPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "logo.png");
                    if (System.IO.File.Exists(logoPath))
                    {
                        var bmp = new BitmapImage();
                        bmp.BeginInit();
                        bmp.UriSource = new Uri(logoPath, UriKind.Absolute);
                        bmp.CacheOption = BitmapCacheOption.OnLoad;
                        bmp.EndInit();
                        bmp.Freeze();
                        ImgLogo.Source = bmp;
                    }
                }
                catch { }
            }
        }

        private void PositionTopRight()
        {
            try
            {
                double workAreaRight = SystemParameters.WorkArea.Right;
                double workAreaTop = SystemParameters.WorkArea.Top;

                // Position on Top-Right of the monitor screen at top vertical height
                this.Left = workAreaRight - this.Width - 24;
                this.Top = workAreaTop + 20;
            }
            catch { }
        }

        public static void ShowToast(string message, bool isError = false)
        {
            if (IsHidden) return;
            Application.Current.Dispatcher.Invoke(() =>
            {
                try
                {
                    if (_activeToast != null)
                    {
                        _activeToast.CloseToastImmediately();
                    }

                    var toast = new ToastWindow(message, isError);
                    _activeToast = toast;
                    toast.Show();
                    toast.AnimateToast();
                }
                catch { }
            });
        }

        private void AnimateToast()
        {
            double targetLeft = this.Left;
            double startLeft = targetLeft + 80; // Slide in smoothly from right edge

            this.Left = startLeft;
            this.Opacity = 0;

            _storyboard = new Storyboard();

            // 1. Slide In from Right (startLeft -> targetLeft) over 220ms
            var animSlideIn = new DoubleAnimation
            {
                From = startLeft,
                To = targetLeft,
                Duration = TimeSpan.FromMilliseconds(220),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };
            Storyboard.SetTarget(animSlideIn, this);
            Storyboard.SetTargetProperty(animSlideIn, new PropertyPath(Window.LeftProperty));
            _storyboard.Children.Add(animSlideIn);

            // 2. Fade In: Opacity 0 -> 1 over 220ms
            var animFadeIn = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(220)
            };
            Storyboard.SetTarget(animFadeIn, this);
            Storyboard.SetTargetProperty(animFadeIn, new PropertyPath(Window.OpacityProperty));
            _storyboard.Children.Add(animFadeIn);

            // 3. Red Progress Line Shrink: ScaleX 1.0 -> 0.0 over 2500ms
            var animTimer = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromMilliseconds(2500)
            };
            Storyboard.SetTarget(animTimer, ToastTimerScale);
            Storyboard.SetTargetProperty(animTimer, new PropertyPath(ScaleTransform.ScaleXProperty));
            _storyboard.Children.Add(animTimer);

            // 4. Slide Out to Right (targetLeft -> startLeft) starting at 2300ms over 220ms
            var animSlideOut = new DoubleAnimation
            {
                From = targetLeft,
                To = startLeft,
                BeginTime = TimeSpan.FromMilliseconds(2300),
                Duration = TimeSpan.FromMilliseconds(220),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn }
            };
            Storyboard.SetTarget(animSlideOut, this);
            Storyboard.SetTargetProperty(animSlideOut, new PropertyPath(Window.LeftProperty));
            _storyboard.Children.Add(animSlideOut);

            // 5. Fade Out: Opacity 1 -> 0 starting at 2300ms over 220ms
            var animFadeOut = new DoubleAnimation
            {
                From = 1,
                To = 0,
                BeginTime = TimeSpan.FromMilliseconds(2300),
                Duration = TimeSpan.FromMilliseconds(220)
            };
            Storyboard.SetTarget(animFadeOut, this);
            Storyboard.SetTargetProperty(animFadeOut, new PropertyPath(Window.OpacityProperty));
            _storyboard.Children.Add(animFadeOut);

            _storyboard.Completed += (s, e) =>
            {
                CloseToastImmediately();
            };

            _storyboard.Begin();
        }

        private void CloseToastImmediately()
        {
            try
            {
                if (_storyboard != null)
                {
                    _storyboard.Stop();
                    _storyboard = null;
                }
                if (_activeToast == this)
                {
                    _activeToast = null;
                }
                this.Close();
            }
            catch { }
        }
    }
}
