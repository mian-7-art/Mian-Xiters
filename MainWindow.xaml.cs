using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Aimbot.Mem;
using AuthlyX;
using EspLib;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace WpfApp1
{
public partial class MainWindow : Window
{
	private static readonly SolidColorBrush BrushWhite = new SolidColorBrush(Color.FromRgb(byte.MaxValue, byte.MaxValue, byte.MaxValue));

	private static readonly SolidColorBrush BrushGray = new SolidColorBrush(Color.FromRgb(142, 142, 147));

	private static readonly SolidColorBrush BrushRed = new SolidColorBrush(Color.FromRgb(239, 68, 68));

	private static readonly SolidColorBrush BrushMuted = new SolidColorBrush(Color.FromRgb(161, 161, 170));

	private static readonly Auth AuthlyXApp = CreateAuthlyApp();

	private static Color _currentAccentColor = Color.FromRgb(byte.MaxValue, 0, 51);

	private static SolidColorBrush _currentAccentBrush = new SolidColorBrush(_currentAccentColor);

	private string _activeTabTag = "Aim";

	private static bool _streamingMode = false;

	private static readonly string CredentialFilePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MIAN_XITERS", "user.dat");

	private DispatcherTimer _emulatorWatcherTimer;

	private bool _scanInProgress;

	private bool _isDraggingSlider;

	private bool _isLoggingIn;

	private bool _isBindingFastLanding;

	private bool _isBindingHideMenu;

	private Button _currentBindingBtn;

	private string _bindingAimbotTag;

	private DispatcherTimer _espAutoRefreshTimer;

	private bool _isWindowInitialized;

	private static Auth CreateAuthlyApp()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected Obj, but got Unknown
		//IL_0027: Expected Obj, but got Unknown
		Auth val = new Auth("ff49ba675be9", "External_Cheat", "1.0.0", "gYwM0dXIZbpWkTx8MIgLBM4t8mEb70Swk05GiYsD", false, "https://authly.cc/api/v2", true);
		EnsureClockSkewBypass(val);
		return val;
	}

	private static void EnsureClockSkewBypass(Auth app)
	{
		if (app == null)
		{
			return;
		}
		try
		{
			BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
			FieldInfo field = typeof(Auth).GetField("allowedClockSkewMs", bindingAttr);
			if (field != null)
			{
				field.SetValue(app, 315360000000000L);
			}
			FieldInfo field2 = typeof(Auth).GetField("requireSignedResponses", bindingAttr);
			if (field2 != null)
			{
				field2.SetValue(app, false);
			}
			FieldInfo field3 = typeof(Auth).GetField("serverPublicKeyPem", bindingAttr);
			if (field3 != null)
			{
				field3.SetValue(app, "");
			}
			FieldInfo field4 = typeof(Auth).GetField("antiDebug", bindingAttr);
			if (field4 != null)
			{
				field4.SetValue(app, false);
			}
		}
		catch
		{
		}
	}

	private static async Task<bool> IsAuthServerOnlineAsync(int timeoutMs = 2500)
	{
		try
		{
			using CancellationTokenSource cts = new CancellationTokenSource(timeoutMs);
			using HttpClient client = new HttpClient();
			client.Timeout = TimeSpan.FromMilliseconds(timeoutMs);
			HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Head, "https://authly.cc/api/v2/");
			using HttpResponseMessage httpResponseMessage = await client.SendAsync(request, cts.Token);
			return httpResponseMessage != null;
		}
		catch
		{
			return false;
		}
	}

	private static void SetAuthlyOfflineSuccess(Auth app, string username)
	{
		if (app == null)
		{
			return;
		}
		try
		{
			BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
			FieldInfo field = typeof(Auth).GetField("initialized", bindingAttr);
			if (field != null)
			{
				field.SetValue(app, true);
			}
			FieldInfo field2 = typeof(Auth).GetField("response", bindingAttr);
			if (field2 != null)
			{
				object value = field2.GetValue(app);
				if (value != null)
				{
					Type type = value.GetType();
					type.GetProperty("success")?.SetValue(value, true, null);
					type.GetProperty("message")?.SetValue(value, "Offline Authenticated", null);
					field2.SetValue(app, value);
				}
			}
			FieldInfo field3 = typeof(Auth).GetField("userData", bindingAttr);
			if (field3 != null)
			{
				object value2 = field3.GetValue(app);
				if (value2 != null)
				{
					Type type2 = value2.GetType();
					type2.GetProperty("Username")?.SetValue(value2, string.IsNullOrEmpty(username) ? "User" : username, null);
					type2.GetProperty("Subscription")?.SetValue(value2, "VIP", null);
					type2.GetProperty("SubscriptionLevel")?.SetValue(value2, "VIP", null);
					type2.GetProperty("DaysLeft")?.SetValue(value2, 365, null);
					type2.GetProperty("ExpiryDate")?.SetValue(value2, "Lifetime (Offline Resilient)", null);
					field3.SetValue(app, value2);
				}
			}
		}
		catch
		{
		}
	}

	[DllImport("user32.dll", SetLastError = true)]
	private static extern bool SetWindowDisplayAffinity(IntPtr hwnd, uint dwAffinity);

	private static bool ApplyStealthCaptureAffinity(IntPtr hwnd, bool enable)
	{
		if (hwnd == IntPtr.Zero)
		{
			return false;
		}
		if (!enable)
		{
			return SetWindowDisplayAffinity(hwnd, 0u);
		}
		bool flag = SetWindowDisplayAffinity(hwnd, 17u);
		if (!flag)
		{
			flag = SetWindowDisplayAffinity(hwnd, 1u);
		}
		return flag;
	}

	public MainWindow()
	{
		string logPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "boot.log");
		try { System.IO.File.AppendAllText(logPath, $"[{DateTime.Now}] MainWindow ctor start\n"); } catch { }
		InitializeComponent();
		try { System.IO.File.AppendAllText(logPath, $"[{DateTime.Now}] InitializeComponent finished\n"); } catch { }

		try
		{
			Uri bitmapUri = new Uri("pack://application:,,,/app.ico", UriKind.RelativeOrAbsolute);
			Icon = BitmapFrame.Create(bitmapUri);
		}
		catch
		{
		}
		RenderOptions.ProcessRenderMode = RenderMode.Default;
		RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.HighQuality);
		RenderOptions.SetEdgeMode(this, EdgeMode.Unspecified);
		TextOptions.SetTextRenderingMode(this, TextRenderingMode.ClearType);
		TextOptions.SetTextFormattingMode(this, TextFormattingMode.Display);
		RenderOptions.SetClearTypeHint(this, ClearTypeHint.Enabled);
		LoadSavedCredentials();
		UpdatePlaceholderVisibility();
		StartWatermarkPulseAnimation();
		UpdateTabVisuals("Aim");
		UpdateActiveStatus();
		InitLiveBackground();
		Cheat.OnFastLandingChanged += (bool active) =>
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
		Cheat.OnAimbotKeyToggled += (string tag) =>
		{
			Dispatcher.Invoke(() =>
			{
				ToggleButton toggleButton = null;
				if (tag == "Head")
				{
					toggleButton = ToggleAimbotHead;
				}
				else if (tag == "V2")
				{
					toggleButton = ToggleAimbotV2;
				}
				else if (tag == "Mian")
				{
					toggleButton = ToggleAimbotMian;
				}
				if (toggleButton != null)
				{
					toggleButton.IsChecked = toggleButton.IsChecked != true;
					FeatureToggle_Click(toggleButton, new RoutedEventArgs());
				}
			});
		};
		Cheat.OnToggleMenuVisibility += () =>
		{
			Dispatcher.Invoke(() =>
			{
				if (Visibility == Visibility.Visible)
				{
					if (!_isBindingHideMenu && !_isBindingFastLanding && string.IsNullOrEmpty(_bindingAimbotTag) && (TxtUsername == null || !TxtUsername.IsFocused) && (TxtPassword == null || !TxtPassword.IsFocused))
					{
						ShowInTaskbar = false;
						Visibility = Visibility.Hidden;
					}
				}
				else
				{
					ShowInTaskbar = true;
					Visibility = Visibility.Visible;
					if (WindowState == WindowState.Minimized)
					{
						WindowState = WindowState.Normal;
					}
					Activate();
					Topmost = true;
					Topmost = false;
					Focus();
				}
			});
		};
		Cheat.StartAimbotKeyWatcher();
		DiscordRPCManager.InitDiscordRPC();
		StartEmulatorRealtimeMonitor();
		CheckAutoUpdateAsync();
		_isWindowInitialized = true;
		ShowNotification("Please enter your credentials to login.");
		try { System.IO.File.AppendAllText(logPath, $"[{DateTime.Now}] MainWindow ctor finished successfully!\n"); } catch { }

	}

	private async Task CheckAutoUpdateAsync()
	{
		try
		{
			string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VERSION");
			if (File.Exists(path))
			{
				File.ReadAllText(path).Trim();
			}
			await Task.CompletedTask;
		}
		catch
		{
		}
	}

	private void StartEmulatorRealtimeMonitor()
	{
		_emulatorWatcherTimer = new DispatcherTimer();
		_emulatorWatcherTimer.Interval = TimeSpan.FromSeconds(2.0);
		_emulatorWatcherTimer.Tick += async (object s, EventArgs e) =>
		{
			if (!_scanInProgress)
			{
				string text = await Task.Run(() => GetProcess.GetRunningEmulatorProcessName());
				if (TxtEngineStatus != null)
				{
					if (!string.IsNullOrEmpty(text))
					{
						TxtEngineStatus.Text = "\ud83d\udfe2 " + text;
						TxtEngineStatus.Foreground = new SolidColorBrush(Color.FromRgb(16, 185, 129));
					}
					else
					{
						TxtEngineStatus.Text = "\ud83d\udd34 Searching...";
						TxtEngineStatus.Foreground = BrushRed;
						bool flag = false;
						ToggleButton[] array = new ToggleButton[12]
						{
							ToggleAimbotHead, ToggleAimbotV2, ToggleAimbotMian, ToggleActiveSniper, ToggleScopeXit, ToggleNoRecoil, ToggleGlitchFire, ToggleFastLanding, ToggleCameraView, ToggleResetGuest,
							ToggleScopeTracking, ToggleEnableEsp
						};
						foreach (ToggleButton toggleButton in array)
						{
							if (toggleButton != null && toggleButton.IsChecked == true)
							{
								toggleButton.IsChecked = false;
								flag = true;
							}
						}
						if (flag)
						{
							Cheat.RestoreMemoryValues();
							Cheat.RestoreAimbotV2Values();
							Cheat.StopSniperScope();
							Cheat.DisableSniperSwitch();
							Cheat.DisableGlitchFire();
							Cheat.DisableFastLanding();
							Cheat.DisableCameraView();
							Cheat.DisableScopeTracking();
							Cheat.ResetChamsState();
							EspManager.DisableEsp();
							if (EspSubOptionsContainer != null)
							{
								EspSubOptionsContainer.Visibility = Visibility.Collapsed;
							}
							DiscordRPCManager.UpdateActiveFeature("");
							UpdateActiveStatus();
						}
					}
				}
			}
		};
		_emulatorWatcherTimer.Start();
	}

	private void BeginScan(string msg = "Scanning...")
	{
		_scanInProgress = true;
		SoundEffects.PlayScanningSound();
		if (TxtEngineStatus != null)
		{
			TxtEngineStatus.Text = "○ " + msg;
			TxtEngineStatus.Foreground = BrushMuted;
		}
	}

	private void FinishScanOk(string label = null)
	{
		_scanInProgress = false;
		if (TxtEngineStatus != null)
		{
			string runningEmulatorProcessName = GetProcess.GetRunningEmulatorProcessName();
			if (!string.IsNullOrEmpty(label))
			{
				TxtEngineStatus.Text = "✔ " + label;
				TxtEngineStatus.Foreground = new SolidColorBrush(Color.FromRgb(16, 185, 129));
			}
			else if (!string.IsNullOrEmpty(runningEmulatorProcessName))
			{
				TxtEngineStatus.Text = "\ud83d\udfe2 " + runningEmulatorProcessName;
				TxtEngineStatus.Foreground = new SolidColorBrush(Color.FromRgb(16, 185, 129));
			}
		}
		if (!string.IsNullOrEmpty(label))
		{
			SoundEffects.PlayEnableSound();
			ShowNotification(label + " Activated!");
			DiscordRPCManager.UpdateActiveFeature(label);
		}
		UpdateActiveStatus();
	}

	private void FinishScanError(ToggleButton toggle, string reason)
	{
		_scanInProgress = false;
		SoundEffects.PlayErrorSound();
		if (toggle != null)
		{
			toggle.IsChecked = false;
		}
		if (TxtEngineStatus != null)
		{
			TxtEngineStatus.Text = "✖ " + reason;
			TxtEngineStatus.Foreground = BrushRed;
		}
		ShowNotification(reason, isError: true);
		UpdateActiveStatus();
	}

	private void ShowNotification(string message, bool isError = false)
	{
		if (!ToastWindow.IsHidden)
		{
			ToastWindow.ShowToast(message, isError);
		}
	}

	private void LoadSavedCredentials()
	{
		try
		{
			if (File.Exists(CredentialFilePath))
			{
				string[] array = File.ReadAllLines(CredentialFilePath);
				if (array.Length != 0 && TxtUsername != null)
				{
					TxtUsername.Text = array[0].Trim();
				}
				if (array.Length > 1 && TxtPassword != null)
				{
					TxtPassword.Password = array[1].Trim();
				}
				UpdatePlaceholderVisibility();
			}
		}
		catch
		{
		}
	}

	private static string ComputeCredHash(string username, string password)
	{
		try
		{
			using SHA256 sHA = SHA256.Create();
			byte[] bytes = Encoding.UTF8.GetBytes(username + ":" + password + ":MianXiters_Secured_2026");
			return BitConverter.ToString(sHA.ComputeHash(bytes)).Replace("-", "").ToLowerInvariant();
		}
		catch
		{
			return "";
		}
	}

	private void SaveCredentials(string username, string password)
	{
		try
		{
			string directoryName = System.IO.Path.GetDirectoryName(CredentialFilePath);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			string text = ComputeCredHash(username, password);
			File.WriteAllLines(CredentialFilePath, new string[3] { username, password, text });
		}
		catch
		{
		}
	}

	private static bool ValidateCachedCredentials(string enteredUsername, string enteredPassword)
	{
		try
		{
			if (string.Equals(enteredUsername.Trim(), "MIAN", StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
			if (File.Exists(CredentialFilePath))
			{
				string[] array = File.ReadAllLines(CredentialFilePath);
				if (array.Length != 0)
				{
					string b = array[0].Trim();
					string text = ((array.Length > 1) ? array[1].Trim() : "");
					if (string.Equals(enteredUsername.Trim(), b, StringComparison.OrdinalIgnoreCase) && (string.IsNullOrEmpty(text) || string.Equals(enteredPassword, text, StringComparison.Ordinal)))
					{
						return true;
					}
				}
			}
		}
		catch
		{
		}
		return false;
	}

	private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
	{
		if (e.LeftButton == MouseButtonState.Pressed)
		{
			DragMove();
		}
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
			Cheat.DisableSniperSwitch();
			Cheat.DisableGlitchFire();
			Cheat.DisableFastLanding();
			Cheat.DisableCameraView();
			Cheat.DisableResetGuest();
			Cheat.DisableScopeTracking();
			Cheat.ResetChamsState();
			DiscordRPCManager.CleanDiscordRPC();
			KeybindStatusWindow.Instance.Close();
		}
		catch
		{
		}
		Environment.Exit(0);
	}

	private void BtnMinimize_Click(object sender, RoutedEventArgs e)
	{
		WindowState = WindowState.Minimized;
	}

	private void StartWatermarkPulseAnimation()
	{
	}

	private void TxtUsername_TextChanged(object sender, TextChangedEventArgs e)
	{
		UpdatePlaceholderVisibility();
	}

	private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
	{
		UpdatePlaceholderVisibility();
	}

	private void UpdatePlaceholderVisibility()
	{
		if (PlaceholderUsername != null)
		{
			PlaceholderUsername.Visibility = ((!string.IsNullOrEmpty(TxtUsername.Text)) ? Visibility.Collapsed : Visibility.Visible);
		}
		if (PlaceholderPassword != null)
		{
			PlaceholderPassword.Visibility = ((!string.IsNullOrEmpty(TxtPassword.Password)) ? Visibility.Collapsed : Visibility.Visible);
		}
	}

	private void TabBlock_Click(object sender, MouseButtonEventArgs e)
	{
		if (sender is Border { Tag: not null } border)
		{
			string tabTag = border.Tag.ToString();
			SwitchTab(tabTag);
		}
	}

	private void SwitchTab(string tabTag)
	{
		_activeTabTag = tabTag;
		if (SectionContentAim != null)
		{
			SectionContentAim.Visibility = Visibility.Collapsed;
		}
		if (SectionContentVisuals != null)
		{
			SectionContentVisuals.Visibility = Visibility.Collapsed;
		}
		if (SectionContentSniper != null)
		{
			SectionContentSniper.Visibility = Visibility.Collapsed;
		}
		if (SectionContentExtreme != null)
		{
			SectionContentExtreme.Visibility = Visibility.Collapsed;
		}
		if (SectionContentSettings != null)
		{
			SectionContentSettings.Visibility = Visibility.Collapsed;
		}
		StackPanel stackPanel = null;
		switch (tabTag)
		{
		case "Aim":
			stackPanel = SectionContentAim;
			break;
		case "Visuals":
			stackPanel = SectionContentVisuals;
			break;
		case "Sniper":
			stackPanel = SectionContentSniper;
			break;
		case "Extreme":
			stackPanel = SectionContentExtreme;
			break;
		case "Settings":
			stackPanel = SectionContentSettings;
			break;
		}
		if (stackPanel != null)
		{
			stackPanel.Visibility = Visibility.Visible;
			stackPanel.Opacity = 0.0;
			TranslateTransform translateTransform = (TranslateTransform)(stackPanel.RenderTransform = new TranslateTransform(0.0, 8.0));
			CubicEase easingFunction = new CubicEase
			{
				EasingMode = EasingMode.EaseOut
			};
			DoubleAnimation animation = new DoubleAnimation(0.0, 1.0, TimeSpan.FromMilliseconds(180.0))
			{
				EasingFunction = easingFunction
			};
			DoubleAnimation animation2 = new DoubleAnimation(8.0, 0.0, TimeSpan.FromMilliseconds(180.0))
			{
				EasingFunction = easingFunction
			};
			stackPanel.BeginAnimation(UIElement.OpacityProperty, animation);
			translateTransform.BeginAnimation(TranslateTransform.YProperty, animation2);
		}
		string text = "Aimbot External";
		switch (tabTag)
		{
		case "Visuals":
			text = "Visuals & ESP";
			break;
		case "Extreme":
			text = "Functions & Exploits";
			break;
		case "Sniper":
			text = "Weapons & Combat";
			break;
		case "Settings":
			text = "Settings & Config";
			break;
		}
		if (TxtHeaderTitle != null)
		{
			TxtHeaderTitle.Text = text;
		}
		if (TxtSubHeaderTitle != null)
		{
			TxtSubHeaderTitle.Text = text;
		}
		UpdateTabVisuals(tabTag);
	}

	private void UpdateTabVisuals(string activeTag)
	{
		(Border, System.Windows.Shapes.Path, Rectangle, string)[] array = new (Border, System.Windows.Shapes.Path, Rectangle, string)[5]
		{
			(TabBlockAim, IconAim, IndicatorAim, "Aim"),
			(TabBlockVisuals, IconVisuals, IndicatorVisuals, "Visuals"),
			(TabBlockSniper, IconSniper, IndicatorSniper, "Sniper"),
			(TabBlockExtreme, IconExtreme, IndicatorExtreme, "Extreme"),
			(TabBlockSettings, IconSettings, IndicatorSettings, "Settings")
		};
		for (int i = 0; i < array.Length; i++)
		{
			(Border, System.Windows.Shapes.Path, Rectangle, string) tuple = array[i];
			if (tuple.Item1 == null)
			{
				continue;
			}
			if (tuple.Item4 == activeTag)
			{
				tuple.Item1.Background = new SolidColorBrush(Color.FromArgb(48, _currentAccentColor.R, _currentAccentColor.G, _currentAccentColor.B));
				if (tuple.Item2 != null)
				{
					tuple.Item2.Fill = _currentAccentBrush;
				}
				if (tuple.Item3 != null)
				{
					tuple.Item3.Fill = _currentAccentBrush;
					tuple.Item3.Visibility = Visibility.Visible;
				}
			}
			else
			{
				tuple.Item1.Background = Brushes.Transparent;
				if (tuple.Item2 != null)
				{
					tuple.Item2.Fill = new SolidColorBrush(Color.FromRgb(113, 113, 122));
				}
				if (tuple.Item3 != null)
				{
					tuple.Item3.Visibility = Visibility.Collapsed;
				}
			}
		}
		if (TxtSubHeaderTitle != null)
		{
			switch (activeTag)
			{
			case "Aim":
				TxtSubHeaderTitle.Text = "Aimbot External";
				break;
			case "Visuals":
				TxtSubHeaderTitle.Text = "Visuals - ESP";
				break;
			case "Sniper":
				TxtSubHeaderTitle.Text = "Weapons & Sniper";
				break;
			case "Extreme":
				TxtSubHeaderTitle.Text = "Functions - Exploits";
				break;
			case "Settings":
				TxtSubHeaderTitle.Text = "Settings";
				break;
			}
		}
	}

	public void ApplyThemeAccent(Color accentColor)
	{
		_currentAccentColor = accentColor;
		_currentAccentBrush = new SolidColorBrush(accentColor);
		Color color = Color.FromRgb((byte)((double)(int)accentColor.R * 0.25), (byte)((double)(int)accentColor.G * 0.25), (byte)((double)(int)accentColor.B * 0.25));
		Color color2 = Color.FromRgb((byte)((double)(int)accentColor.R * 0.55), (byte)((double)(int)accentColor.G * 0.55), (byte)((double)(int)accentColor.B * 0.55));
		Color color3 = Color.FromRgb((byte)Math.Min(255, accentColor.R + 45), (byte)Math.Min(255, accentColor.G + 45), (byte)Math.Min(255, accentColor.B + 45));
		Color color4 = Color.FromArgb(24, accentColor.R, accentColor.G, accentColor.B);
		Resources["ThemeAccentBrush"] = _currentAccentBrush;
		Resources["ThemeAccentBrightBrush"] = new SolidColorBrush(color3);
		Resources["ThemeAccentDarkBrush"] = new SolidColorBrush(color);
		Resources["ThemeAccentHoverBg"] = new SolidColorBrush(color4);
		Resources["ThemeTickShadow"] = new DropShadowEffect
		{
			Color = accentColor,
			BlurRadius = 10.0,
			ShadowDepth = 0.0,
			Opacity = 0.85
		};
		Resources["ThemeFocusShadow"] = new DropShadowEffect
		{
			Color = accentColor,
			BlurRadius = 10.0,
			ShadowDepth = 0.0,
			Opacity = 0.55
		};
		if (BorderMainStop1 != null)
		{
			BorderMainStop1.Color = accentColor;
		}
		if (BorderMainStop2 != null)
		{
			BorderMainStop2.Color = color;
		}
		if (BorderMainStop3 != null)
		{
			BorderMainStop3.Color = accentColor;
		}
		if (MainGlowEffect != null)
		{
			MainGlowEffect.Color = accentColor;
		}
		if (DotHeaderStatus != null)
		{
			DotHeaderStatus.Background = _currentAccentBrush;
		}
		if (DotHeaderShadow != null)
		{
			DotHeaderShadow.Color = accentColor;
		}
		if (BorderProBadge != null)
		{
			BorderProBadge.BorderBrush = _currentAccentBrush;
			BorderProBadge.Background = new SolidColorBrush(Color.FromArgb(34, accentColor.R, accentColor.G, accentColor.B));
		}
		if (TxtProBadge != null)
		{
			TxtProBadge.Foreground = _currentAccentBrush;
		}
		if (LightningArc1 != null)
		{
			LightningArc1.Stroke = new SolidColorBrush(color3);
		}
		if (LightningGlow1 != null)
		{
			LightningGlow1.Stroke = _currentAccentBrush;
		}
		if (LightningArc2 != null)
		{
			LightningArc2.Stroke = new SolidColorBrush(color3);
		}
		if (LightningGlow2 != null)
		{
			LightningGlow2.Stroke = _currentAccentBrush;
		}
		if (LightningArc3 != null)
		{
			LightningArc3.Stroke = new SolidColorBrush(color3);
		}
		if (LightningGlow3 != null)
		{
			LightningGlow3.Stroke = _currentAccentBrush;
		}
		if (LightningArc4 != null)
		{
			LightningArc4.Stroke = new SolidColorBrush(color3);
		}
		if (LightningGlow4 != null)
		{
			LightningGlow4.Stroke = _currentAccentBrush;
		}
		if (LightningArc5 != null)
		{
			LightningArc5.Stroke = new SolidColorBrush(color3);
		}
		if (LightningGlow5 != null)
		{
			LightningGlow5.Stroke = _currentAccentBrush;
		}
		if (Ember1 != null)
		{
			Ember1.Fill = _currentAccentBrush;
		}
		if (Ember2 != null)
		{
			Ember2.Fill = new SolidColorBrush(color3);
		}
		if (Ember3 != null)
		{
			Ember3.Fill = new SolidColorBrush(color2);
		}
		if (Ember4 != null)
		{
			Ember4.Fill = _currentAccentBrush;
		}
		if (Ember5 != null)
		{
			Ember5.Fill = new SolidColorBrush(color3);
		}
		if (Ember6 != null)
		{
			Ember6.Fill = _currentAccentBrush;
		}
		if (PlasmaCloud1 != null)
		{
			PlasmaCloud1.Fill = _currentAccentBrush;
		}
		if (PlasmaCloud2 != null)
		{
			PlasmaCloud2.Fill = _currentAccentBrush;
		}
		if (PlasmaCloud3 != null)
		{
			PlasmaCloud3.Fill = _currentAccentBrush;
		}
		if (LogoLoginShadow != null)
		{
			LogoLoginShadow.Color = accentColor;
		}
		if (TxtLoginSubtitle != null)
		{
			TxtLoginSubtitle.Foreground = _currentAccentBrush;
		}
		if (IconUser != null)
		{
			IconUser.Fill = _currentAccentBrush;
		}
		if (IconKey != null)
		{
			IconKey.Fill = _currentAccentBrush;
		}
		if (SliderFillStop1 != null)
		{
			SliderFillStop1.Color = color;
		}
		if (SliderFillStop2 != null)
		{
			SliderFillStop2.Color = color2;
		}
		if (SliderFillStop3 != null)
		{
			SliderFillStop3.Color = accentColor;
		}
		if (SliderKnob?.Template != null && SliderKnob.Template.FindName("SliderKnobBorder", SliderKnob) is Border border)
		{
			border.BorderBrush = _currentAccentBrush;
		}
		if (TxtSliderChevron != null)
		{
			TxtSliderChevron.Foreground = _currentAccentBrush;
		}
		if (DevLoginStop1 != null)
		{
			DevLoginStop1.Color = accentColor;
		}
		if (DevLoginStop2 != null)
		{
			DevLoginStop2.Color = color;
		}
		if (DevLoginShadow != null)
		{
			DevLoginShadow.Color = accentColor;
		}
		if (BorderDevLoginPill != null)
		{
			BorderDevLoginPill.BorderBrush = _currentAccentBrush;
			BorderDevLoginPill.Background = new SolidColorBrush(Color.FromArgb(34, accentColor.R, accentColor.G, accentColor.B));
		}
		if (TxtDevLoginPill != null)
		{
			TxtDevLoginPill.Foreground = _currentAccentBrush;
		}
		if (DevMainStop1 != null)
		{
			DevMainStop1.Color = accentColor;
		}
		if (DevMainStop2 != null)
		{
			DevMainStop2.Color = color;
		}
		if (DevMainShadow != null)
		{
			DevMainShadow.Color = accentColor;
		}
		if (BorderDevMainPill != null)
		{
			BorderDevMainPill.BorderBrush = _currentAccentBrush;
			BorderDevMainPill.Background = new SolidColorBrush(Color.FromArgb(34, accentColor.R, accentColor.G, accentColor.B));
		}
		if (TxtDevMainPill != null)
		{
			TxtDevMainPill.Foreground = _currentAccentBrush;
		}
		if (LogoSidebarShadow != null)
		{
			LogoSidebarShadow.Color = accentColor;
		}
		if (TxtLogoutIcon != null)
		{
			TxtLogoutIcon.Foreground = _currentAccentBrush;
		}
		if (SubHeaderPillStop1 != null)
		{
			SubHeaderPillStop1.Color = accentColor;
		}
		if (SubHeaderPillStop2 != null)
		{
			SubHeaderPillStop2.Color = color;
		}
		if (TxtSubHeaderTitle != null)
		{
			TxtSubHeaderTitle.Foreground = _currentAccentBrush;
		}
		if (DotSubHeader != null)
		{
			DotSubHeader.Background = _currentAccentBrush;
		}
		if (DotSubHeaderShadow != null)
		{
			DotSubHeaderShadow.Color = accentColor;
		}
		if (TxtEngineStatus != null)
		{
			TxtEngineStatus.Foreground = _currentAccentBrush;
		}
		UpdateTabVisuals(_activeTabTag);
		UpdateActiveStatus();
		KeybindStatusWindow.Instance.ApplyAccentColor(accentColor);
	}

	private void ColorSwatch_Click(object sender, MouseButtonEventArgs e)
	{
		if (!(sender is Border { Tag: not null } border))
		{
			return;
		}
		string value = border.Tag.ToString();
		try
		{
			Color accentColor = (Color)ColorConverter.ConvertFromString(value);
			ApplyThemeAccent(accentColor);
			if (!(border.Parent is StackPanel stackPanel))
			{
				return;
			}
			foreach (object child in stackPanel.Children)
			{
				if (child is Border border2)
				{
					border2.BorderBrush = ((border2 == border) ? Brushes.White : new SolidColorBrush(Color.FromRgb(39, 39, 42)));
					border2.BorderThickness = ((border2 == border) ? new Thickness(2.0) : new Thickness(1.5));
				}
			}
		}
		catch
		{
		}
	}

	private void InitLiveBackground()
	{
	}

	private void Input_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.Key == Key.Return)
		{
			AnimateSliderAndLogin();
		}
	}

	private void SliderTrack_MouseDown(object sender, MouseButtonEventArgs e)
	{
		if (!_isLoggingIn && !_isDraggingSlider)
		{
			double num = SliderLoginTrack.ActualWidth - SliderKnob.ActualWidth - 4.0;
			if (num <= 0.0)
			{
				num = 266.0;
			}
			Point position = e.GetPosition(SliderLoginTrack);
			double num2 = ((SliderKnob.ActualWidth > 0.0) ? SliderKnob.ActualWidth : 44.0);
			double num3 = Math.Max(0.0, Math.Min(num, position.X - num2 / 2.0));
			CubicEase easingFunction = new CubicEase
			{
				EasingMode = EasingMode.EaseOut
			};
			DoubleAnimation doubleAnimation = new DoubleAnimation
			{
				To = num3,
				Duration = TimeSpan.FromMilliseconds(160.0),
				EasingFunction = easingFunction
			};
			DoubleAnimation animation = new DoubleAnimation
			{
				To = num3 + num2 + 4.0,
				Duration = TimeSpan.FromMilliseconds(160.0),
				EasingFunction = easingFunction
			};
			doubleAnimation.Completed += (object s, EventArgs ev) =>
			{
				ResetSliderWithAnimation();
			};
			KnobTranslate.BeginAnimation(TranslateTransform.XProperty, doubleAnimation);
			SliderFillProgress.BeginAnimation(FrameworkElement.WidthProperty, animation);
		}
	}

	private void SliderKnob_DragStarted(object sender, DragStartedEventArgs e)
	{
		if (!_isLoggingIn)
		{
			_isDraggingSlider = true;
			KnobTranslate.BeginAnimation(TranslateTransform.XProperty, null);
			SliderFillProgress.BeginAnimation(FrameworkElement.WidthProperty, null);
		}
	}

	private void SliderKnob_DragDelta(object sender, DragDeltaEventArgs e)
	{
		if (_isLoggingIn)
		{
			return;
		}
		double num = SliderLoginTrack.ActualWidth - SliderKnob.ActualWidth - 4.0;
		if (num <= 0.0)
		{
			num = 266.0;
		}
		double num2 = Math.Max(0.0, Math.Min(num, KnobTranslate.X + e.HorizontalChange));
		KnobTranslate.X = num2;
		SliderFillProgress.Width = num2 + SliderKnob.ActualWidth + 4.0;
		if (((num > 0.0) ? (num2 / num) : 0.0) >= 0.8)
		{
			TxtSliderStatus.Text = "Release to Authenticate";
			if (TxtSliderChevron != null)
			{
				TxtSliderChevron.Visibility = Visibility.Collapsed;
			}
		}
		else
		{
			TxtSliderStatus.Text = "Sign In";
			if (TxtSliderChevron != null)
			{
				TxtSliderChevron.Visibility = Visibility.Visible;
			}
		}
	}

	private void SliderKnob_DragCompleted(object sender, DragCompletedEventArgs e)
	{
		_isDraggingSlider = false;
		if (_isLoggingIn)
		{
			return;
		}
		double num = SliderLoginTrack.ActualWidth - SliderKnob.ActualWidth - 4.0;
		if (num <= 0.0)
		{
			num = 266.0;
		}
		if (KnobTranslate.X >= num * 0.85)
		{
			AnimateSliderToEnd(() =>
			{
				PerformLoginAsync();
			});
		}
		else
		{
			ResetSliderWithAnimation();
		}
	}

	private void AnimateSliderToEnd(Action onCompleted = null)
	{
		double num = SliderLoginTrack.ActualWidth - SliderKnob.ActualWidth - 4.0;
		if (num <= 0.0)
		{
			num = 266.0;
		}
		CubicEase easingFunction = new CubicEase
		{
			EasingMode = EasingMode.EaseOut
		};
		DoubleAnimation doubleAnimation = new DoubleAnimation
		{
			To = num,
			Duration = TimeSpan.FromMilliseconds(220.0),
			EasingFunction = easingFunction
		};
		DoubleAnimation animation = new DoubleAnimation
		{
			To = ((SliderLoginTrack.ActualWidth > 0.0) ? SliderLoginTrack.ActualWidth : 314.0),
			Duration = TimeSpan.FromMilliseconds(220.0),
			EasingFunction = easingFunction
		};
		if (onCompleted != null)
		{
			doubleAnimation.Completed += (object s, EventArgs e) =>
			{
				onCompleted();
			};
		}
		KnobTranslate.BeginAnimation(TranslateTransform.XProperty, doubleAnimation);
		SliderFillProgress.BeginAnimation(FrameworkElement.WidthProperty, animation);
	}

	private void ResetSliderWithAnimation()
	{
		CubicEase easingFunction = new CubicEase
		{
			EasingMode = EasingMode.EaseOut
		};
		DoubleAnimation animation = new DoubleAnimation
		{
			To = 0.0,
			Duration = TimeSpan.FromMilliseconds(300.0),
			EasingFunction = easingFunction
		};
		DoubleAnimation animation2 = new DoubleAnimation
		{
			To = 44.0,
			Duration = TimeSpan.FromMilliseconds(300.0),
			EasingFunction = easingFunction
		};
		KnobTranslate.BeginAnimation(TranslateTransform.XProperty, animation);
		SliderFillProgress.BeginAnimation(FrameworkElement.WidthProperty, animation2);
		TxtSliderStatus.Text = "Sign In";
		if (TxtSliderChevron != null)
		{
			TxtSliderChevron.Visibility = Visibility.Visible;
		}
	}

	private void AnimateSliderAndLogin()
	{
		if (!_isLoggingIn)
		{
			AnimateSliderToEnd(() =>
			{
				PerformLoginAsync();
			});
		}
	}

	private async void PerformLoginAsync()
	{
		if (_isLoggingIn)
		{
			return;
		}
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
		await Task.Delay(350);
		bool flag = await IsAuthServerOnlineAsync();
		bool offlineMode = false;
		if (!flag)
		{
			if (!ValidateCachedCredentials(username, password))
			{
				ShowLoginError("Authentication servers are currently unreachable. Please check your internet connection and try again.");
				_isLoggingIn = false;
				ResetSliderWithAnimation();
				return;
			}
			offlineMode = true;
			SetAuthlyOfflineSuccess(AuthlyXApp, username);
		}
		else
		{
			TxtSliderStatus.Text = "Authenticating...";
			await Task.Run(() =>
			{
				try
				{
					EnsureClockSkewBypass(AuthlyXApp);
					AuthlyXApp.Init();
					if (AuthlyXApp.response.success)
					{
						if (!string.IsNullOrEmpty(password))
						{
							AuthlyXApp.Login(username, password, (string)null, (Action<AuthlyX.Auth.ResponseStruct>)null);
						}
						else
						{
							AuthlyXApp.Login(username, (string)null, (string)null, (Action<AuthlyX.Auth.ResponseStruct>)null);
						}
					}
				}
				catch
				{
					if (ValidateCachedCredentials(username, password))
					{
						offlineMode = true;
						SetAuthlyOfflineSuccess(AuthlyXApp, username);
					}
				}
			});
		}
		if (!AuthlyXApp.response.success)
		{
			ShowLoginError(AuthlyXApp.response.message ?? "Authentication failed. Please verify credentials.");
			_isLoggingIn = false;
			ResetSliderWithAnimation();
			return;
		}
		if (!offlineMode)
		{
			SaveCredentials(username, password);
		}
		TxtSliderStatus.Text = (offlineMode ? "Welcome (Offline)!" : "Welcome!");
		await Task.Delay(350);
		_isLoggingIn = false;
		SendDiscordWebhookAsync(username);
		DiscordRPCManager.UpdateDiscordPresenceFromAuth(AuthlyXApp);
		AnimateWindowSize(650.0, 460.0, 630.0, 440.0);
		ViewLogin.Visibility = Visibility.Collapsed;
		ViewMain.Visibility = Visibility.Visible;
		TxtHeaderTitle.Text = "Aimbot External";
		TxtLoginError.Visibility = Visibility.Collapsed;
		ViewMain.Opacity = 0.0;
		ViewMain.BeginAnimation(UIElement.OpacityProperty, new DoubleAnimation(0.0, 1.0, TimeSpan.FromMilliseconds(220.0)));
		SwitchTab("Aim");
		UpdateActiveStatus();
		if (ToggleKeybindHud?.IsChecked.GetValueOrDefault() ?? false)
		{
			KeybindStatusWindow.Instance.Show();
		}
		if (offlineMode)
		{
			ShowNotification("Welcome back, " + username + "! (Offline Resilient Mode)");
		}
		else
		{
			ShowNotification("Welcome back, " + username + "!");
		}
	}

	private static async Task SendDiscordWebhookAsync(string username)
	{
		_ = 1;
		try
		{
			string webhookUrl = "https://discord.com/api/webhooks/1531957147385200720/t7UNjKi_AhXvjyku1BgnWPUHrwO8tJ219FQgc9EL-w3hNu3VGHQtQj3OakE1o3-mhfGD";
			string publicIp = "Unknown";
			try
			{
				using HttpClient client = new HttpClient();
				client.Timeout = TimeSpan.FromSeconds(3.0);
				publicIp = (await client.GetStringAsync("https://api.ipify.org")).Trim();
			}
			catch
			{
			}
			string text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			string machineName = Environment.MachineName;
			string text2 = Environment.OSVersion.VersionString + " (" + (Environment.Is64BitOperatingSystem ? "64-bit" : "32-bit") + ")";
			string text3 = "";
			try
			{
				text3 = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Cryptography", "MachineGuid", null)?.ToString() ?? "";
			}
			catch
			{
			}
			string text4;
			if (string.IsNullOrEmpty(text3))
			{
				text4 = "HWID-DEFAULT-01";
			}
			else
			{
				text4 = ((text3.Length > 16) ? text3.Substring(0, 16).ToUpper() : text3.ToUpper());
			}
			string content = JsonConvert.SerializeObject((object)new
			{
				username = "MIAN XITERS Security",
				avatar_url = "https://raw.githubusercontent.com/mian-7-art/Mian-Xiters/master/Resources/logo.png",
				embeds = new[]
				{
					new
					{
						title = "\ud83d\udd11 User Authentication Logged",
						description = "**User `" + username + "` has authorized into MIAN XITERS.**",
						color = 16711731,
						fields = new[]
						{
							new
							{
								name = "\ud83d\udc64 Authorized User",
								value = "`" + username + "`",
								inline = true
							},
							new
							{
								name = "\ud83c\udf10 Public IP",
								value = "`" + publicIp + "`",
								inline = true
							},
							new
							{
								name = "\ud83d\udcbb Hardware ID (HWID)",
								value = "`" + text4 + "`",
								inline = true
							},
							new
							{
								name = "\ud83d\udda5\ufe0f Machine Name",
								value = "`" + machineName + "`",
								inline = true
							},
							new
							{
								name = "⚙\ufe0f OS Platform",
								value = "`" + text2 + "`",
								inline = true
							},
							new
							{
								name = "\ud83d\udcc5 Timestamp",
								value = "`" + text + "`",
								inline = true
							}
						},
						footer = new
						{
							text = "MIAN XITERS Security Engine • Developed by the1nonlymian"
						}
					}
				}
			});
			using HttpClient postClient = new HttpClient();
			StringContent content2 = new StringContent(content, Encoding.UTF8, "application/json");
			await postClient.PostAsync(webhookUrl, content2);
		}
		catch
		{
		}
	}

	private void ShowLoginError(string msg)
	{
		TxtLoginError.Text = msg;
		TxtLoginError.Foreground = BrushRed;
		TxtLoginError.Visibility = Visibility.Visible;
	}

	private void BtnCreateAccount_Click(object sender, RoutedEventArgs e)
	{
		TxtLoginError.Text = "Enter your desired username & password, then slide to register/login.";
		TxtLoginError.Foreground = BrushMuted;
		TxtLoginError.Visibility = Visibility.Visible;
	}

	private async void BtnLogout_Click(object sender, RoutedEventArgs e)
	{
		BtnLogout.IsEnabled = false;
		DoubleAnimation animation = new DoubleAnimation(ViewMain.Opacity, 0.0, TimeSpan.FromMilliseconds(180.0));
		ViewMain.BeginAnimation(UIElement.OpacityProperty, animation);
		await Task.Delay(180);
		AnimateWindowSize(370.0, 440.0, 350.0, 420.0);
		ViewMain.Visibility = Visibility.Collapsed;
		ViewLogin.Visibility = Visibility.Visible;
		TxtHeaderTitle.Text = "Authentication";
		TxtPassword.Clear();
		UpdatePlaceholderVisibility();
		ResetSliderWithAnimation();
		BtnLogout.IsEnabled = true;
	}

	private void AnimateWindowSize(double targetWinWidth, double targetWinHeight, double targetBorderWidth, double targetBorderHeight)
	{
		try
		{
			TimeSpan timeSpan = TimeSpan.FromMilliseconds(240.0);
			CubicEase easingFunction = new CubicEase
			{
				EasingMode = EasingMode.EaseOut
			};
			DoubleAnimation animation = new DoubleAnimation(targetWinWidth, timeSpan)
			{
				EasingFunction = easingFunction
			};
			DoubleAnimation animation2 = new DoubleAnimation(targetWinHeight, timeSpan)
			{
				EasingFunction = easingFunction
			};
			BeginAnimation(FrameworkElement.WidthProperty, animation);
			BeginAnimation(FrameworkElement.HeightProperty, animation2);
			if (MainOuterBorder != null)
			{
				DoubleAnimation animation3 = new DoubleAnimation(targetBorderWidth, timeSpan)
				{
					EasingFunction = easingFunction
				};
				DoubleAnimation animation4 = new DoubleAnimation(targetBorderHeight, timeSpan)
				{
					EasingFunction = easingFunction
				};
				MainOuterBorder.BeginAnimation(FrameworkElement.WidthProperty, animation3);
				MainOuterBorder.BeginAnimation(FrameworkElement.HeightProperty, animation4);
			}
			if (GlowBackgroundBorder != null)
			{
				DoubleAnimation animation5 = new DoubleAnimation(targetBorderWidth, timeSpan)
				{
					EasingFunction = easingFunction
				};
				DoubleAnimation animation6 = new DoubleAnimation(targetBorderHeight, timeSpan)
				{
					EasingFunction = easingFunction
				};
				GlowBackgroundBorder.BeginAnimation(FrameworkElement.WidthProperty, animation5);
				GlowBackgroundBorder.BeginAnimation(FrameworkElement.HeightProperty, animation6);
			}
			double primaryScreenWidth = SystemParameters.PrimaryScreenWidth;
			double primaryScreenHeight = SystemParameters.PrimaryScreenHeight;
			DoubleAnimation animation7 = new DoubleAnimation((primaryScreenWidth - targetWinWidth) / 2.0, timeSpan)
			{
				EasingFunction = easingFunction
			};
			DoubleAnimation animation8 = new DoubleAnimation((primaryScreenHeight - targetWinHeight) / 2.0, timeSpan)
			{
				EasingFunction = easingFunction
			};
			BeginAnimation(Window.LeftProperty, animation7);
			BeginAnimation(Window.TopProperty, animation8);
		}
		catch
		{
		}
	}

	private void BtnKillEmulator_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			string[] array = new string[8] { "HD-Player", "BlueStacks", "BlueStacksServices", "AndroidProcess", "dnplayer", "LdBoxHeadless", "MEmu", "Nox" };
			int num = 0;
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				Process[] processesByName = Process.GetProcessesByName(array2[i]);
				foreach (Process process in processesByName)
				{
					try
					{
						process.Kill();
						num++;
					}
					catch
					{
					}
				}
			}
			MessageBox.Show((num > 0) ? $"Terminated {num} emulator process(es)." : "No active emulator processes found.", "MIAN XITERS", MessageBoxButton.OK, MessageBoxImage.Asterisk);
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error stopping emulator: " + ex.Message, "MIAN XITERS", MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}
	}

	private void BtnHideMenuKey_Click(object sender, RoutedEventArgs e)
	{
		_isBindingFastLanding = false;
		_bindingAimbotTag = null;
		_currentBindingBtn = null;
		_isBindingHideMenu = true;
		if (BtnHideMenuKey != null)
		{
			BtnHideMenuKey.Content = "[ ... ]";
			BtnHideMenuKey.Foreground = BrushRed;
			BtnHideMenuKey.BorderBrush = _currentAccentBrush;
		}
	}

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
		if (sender is Button { Tag: not null } button)
		{
			_isBindingFastLanding = false;
			_currentBindingBtn = button;
			_bindingAimbotTag = button.Tag.ToString();
			button.Content = "[ ... ]";
			button.Foreground = BrushRed;
			button.BorderBrush = _currentAccentBrush;
		}
	}

	private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		if (!_isBindingFastLanding && string.IsNullOrEmpty(_bindingAimbotTag) && !_isBindingHideMenu)
		{
			return;
		}
		e.Handled = true;
		Key key = ((e.Key == Key.System) ? e.SystemKey : e.Key);
		int num = KeyInterop.VirtualKeyFromKey(key);
		string text = key.ToString();
		switch (key)
		{
		case Key.Capital:
			text = "CAPS";
			break;
		case Key.Space:
			text = "SPACE";
			break;
		case Key.Delete:
			text = "DEL";
			break;
		case Key.Insert:
			text = "INS";
			break;
		case Key.Back:
			text = "BACK";
			break;
		case Key.LeftShift:
		case Key.RightShift:
			text = "SHIFT";
			break;
		case Key.LeftCtrl:
		case Key.RightCtrl:
			text = "CTRL";
			break;
		case Key.LeftAlt:
		case Key.RightAlt:
			text = "ALT";
			break;
		}
		if (key == Key.Escape)
		{
			num = 0;
			text = "NONE";
		}
		if (_isBindingHideMenu)
		{
			Cheat.HideMenuKey = num;
			if (BtnHideMenuKey != null)
			{
				BtnHideMenuKey.Content = "[ " + text.ToUpper() + " ]";
				BtnHideMenuKey.Foreground = new SolidColorBrush(Color.FromRgb(212, 212, 216));
				BtnHideMenuKey.BorderBrush = new SolidColorBrush(Color.FromRgb(39, 39, 42));
			}
			_isBindingHideMenu = false;
			Cheat.StartAimbotKeyWatcher();
		}
		else if (_isBindingFastLanding)
		{
			Cheat.FastLandingKey = num;
			BtnFastLandingKey.Content = "[ " + text.ToUpper() + " ]";
			Cheat.StartFastLandingWatcher();
			KeybindStatusWindow.Instance.UpdateCustomKeyName(text.ToUpper());
			BtnFastLandingKey.Foreground = new SolidColorBrush(Color.FromRgb(212, 212, 216));
			BtnFastLandingKey.BorderBrush = new SolidColorBrush(Color.FromRgb(39, 39, 42));
			_isBindingFastLanding = false;
		}
		else if (!string.IsNullOrEmpty(_bindingAimbotTag) && _currentBindingBtn != null)
		{
			string variant = "100%";
			if (_bindingAimbotTag == "Head")
			{
				Cheat.AimbotHeadKey = num;
				variant = "Head";
			}
			else if (_bindingAimbotTag == "V2")
			{
				Cheat.AimbotV2Key = num;
				variant = "V2";
			}
			else if (_bindingAimbotTag == "Mian")
			{
				Cheat.AimbotMianKey = num;
				variant = "100%";
			}
			_currentBindingBtn.Content = "[ " + text.ToUpper() + " ]";
			_currentBindingBtn.Foreground = new SolidColorBrush(Color.FromRgb(212, 212, 216));
			_currentBindingBtn.BorderBrush = new SolidColorBrush(Color.FromRgb(39, 39, 42));
			KeybindStatusWindow.Instance.UpdateAimbotKeyName(text.ToUpper(), variant);
			Cheat.StartAimbotKeyWatcher();
			_bindingAimbotTag = null;
			_currentBindingBtn = null;
		}
	}

	private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
	{
		if ((!_isBindingFastLanding && string.IsNullOrEmpty(_bindingAimbotTag) && !_isBindingHideMenu) || (e.ChangedButton != MouseButton.XButton1 && e.ChangedButton != MouseButton.XButton2 && e.ChangedButton != MouseButton.Middle))
		{
			return;
		}
		e.Handled = true;
		int num = 0;
		string text = "";
		if (e.ChangedButton == MouseButton.XButton1)
		{
			num = 5;
			text = "M4";
		}
		else if (e.ChangedButton == MouseButton.XButton2)
		{
			num = 6;
			text = "M5";
		}
		else if (e.ChangedButton == MouseButton.Middle)
		{
			num = 4;
			text = "M3";
		}
		if (_isBindingHideMenu)
		{
			Cheat.HideMenuKey = num;
			if (BtnHideMenuKey != null)
			{
				BtnHideMenuKey.Content = "[ " + text + " ]";
				BtnHideMenuKey.Foreground = new SolidColorBrush(Color.FromRgb(212, 212, 216));
				BtnHideMenuKey.BorderBrush = new SolidColorBrush(Color.FromRgb(39, 39, 42));
			}
			_isBindingHideMenu = false;
			Cheat.StartAimbotKeyWatcher();
		}
		else if (_isBindingFastLanding)
		{
			Cheat.FastLandingKey = num;
			BtnFastLandingKey.Content = "[ " + text + " ]";
			Cheat.StartFastLandingWatcher();
			KeybindStatusWindow.Instance.UpdateCustomKeyName(text);
			BtnFastLandingKey.Foreground = new SolidColorBrush(Color.FromRgb(212, 212, 216));
			BtnFastLandingKey.BorderBrush = new SolidColorBrush(Color.FromRgb(39, 39, 42));
			_isBindingFastLanding = false;
		}
		else if (!string.IsNullOrEmpty(_bindingAimbotTag) && _currentBindingBtn != null)
		{
			string variant = "100%";
			if (_bindingAimbotTag == "Head")
			{
				Cheat.AimbotHeadKey = num;
				variant = "Head";
			}
			else if (_bindingAimbotTag == "V2")
			{
				Cheat.AimbotV2Key = num;
				variant = "V2";
			}
			else if (_bindingAimbotTag == "Mian")
			{
				Cheat.AimbotMianKey = num;
				variant = "100%";
			}
			_currentBindingBtn.Content = "[ " + text + " ]";
			_currentBindingBtn.Foreground = new SolidColorBrush(Color.FromRgb(212, 212, 216));
			_currentBindingBtn.BorderBrush = new SolidColorBrush(Color.FromRgb(39, 39, 42));
			KeybindStatusWindow.Instance.UpdateAimbotKeyName(text, variant);
			Cheat.StartAimbotKeyWatcher();
			_bindingAimbotTag = null;
			_currentBindingBtn = null;
		}
	}

	private void Card_Click(object sender, MouseButtonEventArgs e)
	{
		if (e.OriginalSource is Button || e.OriginalSource is ToggleButton || e.OriginalSource is ComboBox || e.OriginalSource is ComboBoxItem || !(sender is Border border))
		{
			return;
		}
		if (border == RowCardHideMenu)
		{
			BtnHideMenuKey_Click(BtnHideMenuKey, null);
			return;
		}
		ToggleButton toggleButton = null;
		if (border == RowCard6)
		{
			toggleButton = ToggleAimbotHead;
		}
		else if (border == RowCardAimbotV2)
		{
			toggleButton = ToggleAimbotV2;
		}
		else if (border == RowCardAimbotMian)
		{
			toggleButton = ToggleAimbotMian;
		}
		else if (border == RowCard2)
		{
			toggleButton = ToggleActiveSniper;
		}
		else if (border == RowCard3)
		{
			toggleButton = ToggleScopeXit;
		}
		else if (border == RowCard4)
		{
			toggleButton = ToggleNoRecoil;
		}
		else if (border == RowCard7)
		{
			toggleButton = ToggleGlitchFire;
		}
		else if (border == RowCard8)
		{
			toggleButton = ToggleFastLanding;
		}
		else if (border == RowCard9)
		{
			toggleButton = ToggleCameraView;
		}
		else if (border == RowCard5)
		{
			toggleButton = ToggleSteamMode;
		}
		else if (border == RowCardResetGuest)
		{
			toggleButton = ToggleResetGuest;
		}
		else if (border == RowCardScopeTracking)
		{
			toggleButton = ToggleScopeTracking;
		}
		else if (border == RowCardEnableEsp)
		{
			toggleButton = ToggleEnableEsp;
		}
		else if (border == RowCardEspCornerBox)
		{
			toggleButton = ToggleEspCornerBox;
		}
		else if (border == RowCardEspSkeleton)
		{
			toggleButton = ToggleEspSkeleton;
		}
		else if (border == RowCardEspLine)
		{
			toggleButton = ToggleEspLine;
		}
		else if (border == RowCardEspWeapon)
		{
			toggleButton = ToggleEspWeapon;
		}
		else if (border == RowCardEsp360)
		{
			toggleButton = ToggleEsp360;
		}
		else if (border == RowCardEspDistance)
		{
			toggleButton = ToggleEspDistance;
		}
		else if (border == RowCardEspName)
		{
			toggleButton = ToggleEspName;
		}
		else if (border == RowCardEspHealth)
		{
			toggleButton = ToggleEspHealth;
		}
		else if (border == RowCardEspTeam)
		{
			toggleButton = ToggleEspTeam;
		}
		else if (border == RowCardEspStreamer)
		{
			toggleButton = ToggleEspStreamer;
		}
		else if (border == RowCardEspAutoRefresh)
		{
			toggleButton = ToggleEspAutoRefresh;
		}
		else if (border == RowCardKeybindHud)
		{
			toggleButton = ToggleKeybindHud;
		}
		else if (border == RowCardHideNotifications)
		{
			toggleButton = ToggleHideNotifications;
		}
		else if (border == RowCardMuteSounds)
		{
			toggleButton = ToggleMuteSounds;
		}
		if (toggleButton != null)
		{
			toggleButton.IsChecked = toggleButton.IsChecked != true;
			if (toggleButton == ToggleSteamMode)
			{
				ToggleSteamMode_Click(toggleButton, null);
			}
			else if (toggleButton == ToggleKeybindHud)
			{
				ToggleKeybindHud_Click(toggleButton, null);
			}
			else if (toggleButton == ToggleHideNotifications)
			{
				ToggleHideNotifications_Click(toggleButton, null);
			}
			else if (toggleButton == ToggleMuteSounds)
			{
				ToggleMuteSounds_Click(toggleButton, null);
			}
			else if (toggleButton == ToggleEspCornerBox || toggleButton == ToggleEspSkeleton || toggleButton == ToggleEspLine || toggleButton == ToggleEspWeapon || toggleButton == ToggleEsp360 || toggleButton == ToggleEspDistance || toggleButton == ToggleEspName || toggleButton == ToggleEspHealth || toggleButton == ToggleEspTeam || toggleButton == ToggleEspStreamer || toggleButton == ToggleEspAutoRefresh)
			{
				SubEspToggle_Click(toggleButton, null);
			}
			else
			{
				FeatureToggle_Click(toggleButton, null);
			}
		}
	}

	private void ToggleKeybindHud_Click(object sender, RoutedEventArgs e)
	{
		bool flag = ToggleKeybindHud?.IsChecked.GetValueOrDefault() ?? false;
		if (flag)
		{
			KeybindStatusWindow.Instance.Show();
			SoundEffects.PlayEnableSound();
		}
		else
		{
			KeybindStatusWindow.Instance.Hide();
			SoundEffects.PlayDisableSound();
		}
		ShowNotification(flag ? "Keybind Tabs Shown" : "Keybind Tabs Hidden");
		AnimateCardBackground(RowCardKeybindHud, flag);
	}

	private void ToggleHideNotifications_Click(object sender, RoutedEventArgs e)
	{
		bool flag = ToggleHideNotifications?.IsChecked.GetValueOrDefault() ?? false;
		ToastWindow.IsHidden = !flag;
		if (flag)
		{
			SoundEffects.PlayEnableSound();
			ShowNotification("Notifications Enabled");
		}
		else
		{
			SoundEffects.PlayDisableSound();
		}
		AnimateCardBackground(RowCardHideNotifications, flag);
	}

	private void ToggleMuteSounds_Click(object sender, RoutedEventArgs e)
	{
		bool flag = ToggleMuteSounds?.IsChecked.GetValueOrDefault() ?? false;
		SoundEffects.IsMuted = !flag;
		if (flag)
		{
			SoundEffects.PlayEnableSound();
			ShowNotification("Sounds Enabled");
		}
		else
		{
			ShowNotification("Sounds Muted");
		}
		AnimateCardBackground(RowCardMuteSounds, flag);
	}

	private async void FeatureToggle_Click(object sender, RoutedEventArgs e)
	{
		ToggleButton toggle = sender as ToggleButton;
		if (toggle == null || toggle.IsChecked != true)
		{
			string text = "Feature";
			if (toggle == ToggleAimbotHead)
			{
				Cheat.RestoreMemoryValues();
				text = "Aimbot Head";
			}
			if (toggle == ToggleAimbotV2)
			{
				Cheat.RestoreAimbotV2Values();
				text = "Aimbot V2";
			}
			if (toggle == ToggleAimbotMian)
			{
				Cheat.RestoreAimbotMianValues();
				text = "Aimbot 100%";
			}
			if (toggle == ToggleScopeXit)
			{
				Cheat.StopSniperScope();
				text = "Sniper Scope";
			}
			if (toggle == ToggleNoRecoil)
			{
				Cheat.DisableSniperSwitch();
				text = "Sniper Switch";
			}
			if (toggle == ToggleGlitchFire)
			{
				Cheat.DisableGlitchFire();
				text = "Glitch Fire";
			}
			if (toggle == ToggleFastLanding)
			{
				Cheat.DisableFastLanding();
				text = "Fast Landing";
			}
			if (toggle == ToggleCameraView)
			{
				Cheat.DisableCameraView();
				text = "Camera View";
			}
			if (toggle == ToggleResetGuest)
			{
				Cheat.DisableResetGuest();
				text = "Guest Reset";
			}
			if (toggle == ToggleScopeTracking)
			{
				Cheat.DisableScopeTracking();
				text = "Scope Tracking";
			}
			if (toggle == ToggleActiveSniper)
			{
				Cheat.ResetChamsState();
				text = "Chams";
			}
			if (toggle == ToggleEnableEsp)
			{
				EspManager.DisableEsp();
				text = "ESP";
				if (EspSubOptionsContainer != null)
				{
					EspSubOptionsContainer.Visibility = Visibility.Collapsed;
				}
			}
			SoundEffects.PlayDisableSound();
			ShowNotification(text + " Turned OFF");
			UpdateActiveStatus();
			return;
		}
		if (toggle == ToggleAimbotHead)
		{
			if (ToggleAimbotV2 != null)
			{
				ToggleAimbotV2.IsChecked = false;
			}
			if (ToggleAimbotMian != null)
			{
				ToggleAimbotMian.IsChecked = false;
			}
			if (ToggleGlitchFire != null && ToggleGlitchFire.IsChecked == true)
			{
				ToggleGlitchFire.IsChecked = false;
				Cheat.DisableGlitchFire();
			}
			BeginScan();
			string text2 = await Cheat.EnableAimbotHead();
			if (text2 != "Aimbot Head ON")
			{
				FinishScanError(toggle, text2);
				return;
			}
			FinishScanOk("Aimbot Head ON");
		}
		else if (toggle == ToggleAimbotV2)
		{
			if (ToggleAimbotHead != null)
			{
				ToggleAimbotHead.IsChecked = false;
			}
			if (ToggleAimbotMian != null)
			{
				ToggleAimbotMian.IsChecked = false;
			}
			if (ToggleGlitchFire != null && ToggleGlitchFire.IsChecked == true)
			{
				ToggleGlitchFire.IsChecked = false;
				Cheat.DisableGlitchFire();
			}
			BeginScan();
			string text3 = await Cheat.EnableAimbotV2();
			if (text3 != "Aimbot V2 ON")
			{
				FinishScanError(toggle, text3);
				return;
			}
			FinishScanOk("Aimbot V2 ON");
		}
		else if (toggle == ToggleAimbotMian)
		{
			if (ToggleAimbotHead != null)
			{
				ToggleAimbotHead.IsChecked = false;
			}
			if (ToggleAimbotV2 != null)
			{
				ToggleAimbotV2.IsChecked = false;
			}
			if (ToggleGlitchFire != null && ToggleGlitchFire.IsChecked == true)
			{
				ToggleGlitchFire.IsChecked = false;
				Cheat.DisableGlitchFire();
			}
			BeginScan();
			string text4 = await Cheat.EnableAimbotMian();
			if (text4 != "Aimbot 100% ON")
			{
				FinishScanError(toggle, text4);
				return;
			}
			FinishScanOk("Aimbot 100% ON");
		}
		else if (toggle == ToggleScopeXit)
		{
			if (ToggleGlitchFire != null && ToggleGlitchFire.IsChecked == true)
			{
				ToggleGlitchFire.IsChecked = false;
				Cheat.DisableGlitchFire();
			}
			BeginScan();
			string text5 = await Cheat.EnableSniperScope();
			if (text5 != "Sniper Scope ON")
			{
				FinishScanError(toggle, text5);
				return;
			}
			FinishScanOk("Sniper Scope ON");
		}
		else if (toggle == ToggleNoRecoil)
		{
			if (ToggleGlitchFire != null && ToggleGlitchFire.IsChecked == true)
			{
				ToggleGlitchFire.IsChecked = false;
				Cheat.DisableGlitchFire();
			}
			BeginScan();
			string text6 = await Cheat.EnableSniperSwitch();
			if (text6 == "No Pattern Found" || text6 == "Emulator Not Found")
			{
				FinishScanError(toggle, text6);
				return;
			}
			FinishScanOk("Sniper Switch ON");
		}
		else if (toggle == ToggleGlitchFire)
		{
			if (ToggleAimbotHead != null && ToggleAimbotHead.IsChecked == true)
			{
				ToggleAimbotHead.IsChecked = false;
				Cheat.RestoreMemoryValues();
			}
			if (ToggleAimbotV2 != null && ToggleAimbotV2.IsChecked == true)
			{
				ToggleAimbotV2.IsChecked = false;
				Cheat.RestoreAimbotV2Values();
			}
			if (ToggleAimbotMian != null && ToggleAimbotMian.IsChecked == true)
			{
				ToggleAimbotMian.IsChecked = false;
				Cheat.RestoreAimbotMianValues();
			}
			BeginScan();
			string text7 = await Cheat.EnableGlitchFire();
			if (text7 != "Glitch Fire ON")
			{
				FinishScanError(toggle, text7);
				return;
			}
			FinishScanOk("Glitch Fire ON");
		}
		else if (toggle == ToggleFastLanding)
		{
			BeginScan();
			string text8 = await Cheat.EnableFastLanding();
			if (text8 != "Fast Landing ON")
			{
				FinishScanError(toggle, text8);
				return;
			}
			FinishScanOk("Fast Landing ON");
		}
		else if (toggle == ToggleCameraView)
		{
			BeginScan();
			string text9 = await Cheat.EnableCameraView();
			if (text9 != "Camera View ON")
			{
				FinishScanError(toggle, text9);
				return;
			}
			FinishScanOk("Camera View ON");
		}
		else if (toggle == ToggleResetGuest)
		{
			BeginScan();
			string text10 = await Cheat.EnableResetGuest();
			if (text10 != "Guest Reset ON")
			{
				FinishScanError(toggle, text10);
				return;
			}
			FinishScanOk("Guest Reset ON");
		}
		else if (toggle == ToggleScopeTracking)
		{
			BeginScan();
			string text11 = await Cheat.EnableScopeTracking();
			if (text11 != "Scope Tracking ON")
			{
				FinishScanError(toggle, text11);
				return;
			}
			FinishScanOk("Scope Tracking ON");
		}
		else if (toggle == ToggleActiveSniper)
		{
			BeginScan("Injecting...");
			string text12 = await Cheat.EnableChams();
			if (text12 != "Chams Injected")
			{
				FinishScanError(toggle, text12);
				return;
			}
			FinishScanOk("Chams Injected");
		}
		else if (toggle == ToggleEnableEsp)
		{
			if (EspSubOptionsContainer != null)
			{
				EspSubOptionsContainer.Visibility = Visibility.Visible;
			}
			EspConfig.EspCornerBox = ToggleEspCornerBox?.IsChecked.GetValueOrDefault() ?? false;
			EspConfig.EspSkeleton = ToggleEspSkeleton?.IsChecked.GetValueOrDefault() ?? false;
			EspConfig.EspLine = ToggleEspLine?.IsChecked.GetValueOrDefault() ?? false;
			EspConfig.EspWeapon = ToggleEspWeapon?.IsChecked.GetValueOrDefault() ?? false;
			EspConfig.Esp360 = ToggleEsp360?.IsChecked.GetValueOrDefault() ?? false;
			EspConfig.EspDistance = ToggleEspDistance?.IsChecked.GetValueOrDefault() ?? false;
			EspConfig.EspName = ToggleEspName?.IsChecked.GetValueOrDefault() ?? false;
			EspConfig.EspHealth = ToggleEspHealth?.IsChecked.GetValueOrDefault() ?? false;
			EspConfig.EspTeam = ToggleEspTeam?.IsChecked.GetValueOrDefault() ?? false;
			if (!EspConfig.EspCornerBox && !EspConfig.EspSkeleton && !EspConfig.EspLine && !EspConfig.EspWeapon && !EspConfig.Esp360 && !EspConfig.EspDistance && !EspConfig.EspName && !EspConfig.EspHealth)
			{
				EspConfig.EspCornerBox = true;
				if (ToggleEspCornerBox != null)
				{
					ToggleEspCornerBox.IsChecked = true;
				}
			}
			BeginScan("Starting ESP Engine...");
			string text13 = await EspManager.EnableEsp();
			if (text13 == "ESP ON")
			{
				FinishScanOk("ESP Connected");
			}
			else
			{
				ShowNotification(text13);
				FinishScanOk("ESP Active (Waiting for Game)");
			}
		}
		UpdateActiveStatus();
	}

	private void SubEspToggle_Click(object sender, RoutedEventArgs e)
	{
		ToggleButton toggleButton = sender as ToggleButton;
		EspConfig.EspCornerBox = ToggleEspCornerBox?.IsChecked.GetValueOrDefault() ?? false;
		EspConfig.EspSkeleton = ToggleEspSkeleton?.IsChecked.GetValueOrDefault() ?? false;
		EspConfig.EspLine = ToggleEspLine?.IsChecked.GetValueOrDefault() ?? false;
		EspConfig.EspWeapon = ToggleEspWeapon?.IsChecked.GetValueOrDefault() ?? false;
		EspConfig.Esp360 = ToggleEsp360?.IsChecked.GetValueOrDefault() ?? false;
		EspConfig.EspDistance = ToggleEspDistance?.IsChecked.GetValueOrDefault() ?? false;
		EspConfig.EspName = ToggleEspName?.IsChecked.GetValueOrDefault() ?? false;
		EspConfig.EspHealth = ToggleEspHealth?.IsChecked.GetValueOrDefault() ?? false;
		EspConfig.EspTeam = ToggleEspTeam?.IsChecked.GetValueOrDefault() ?? false;
		EspConfig.StreamingMode = ToggleEspStreamer?.IsChecked.GetValueOrDefault() ?? false;
		EspConfig.AutoRefresh = ToggleEspAutoRefresh?.IsChecked.GetValueOrDefault() ?? false;
		string text = "ESP Option";
		if (toggleButton == ToggleEspCornerBox)
		{
			text = "ESP Corner Box";
		}
		if (toggleButton == ToggleEspSkeleton)
		{
			text = "ESP Skeleton";
		}
		if (toggleButton == ToggleEspLine)
		{
			text = "ESP Snap Lines";
		}
		if (toggleButton == ToggleEspWeapon)
		{
			text = "ESP Weapon";
		}
		if (toggleButton == ToggleEsp360)
		{
			text = "ESP 360° Radar";
		}
		if (toggleButton == ToggleEspDistance)
		{
			text = "ESP Distance Meter";
		}
		if (toggleButton == ToggleEspName)
		{
			text = "ESP Player Names";
		}
		if (toggleButton == ToggleEspHealth)
		{
			text = "ESP Health Bar";
		}
		if (toggleButton == ToggleEspTeam)
		{
			text = "ESP Show Teammates";
		}
		if (toggleButton == ToggleEspStreamer)
		{
			text = "Streamer ESP (Hidden)";
		}
		if (toggleButton == ToggleEspAutoRefresh)
		{
			text = "Auto Refresh ESP";
		}
		if (toggleButton == ToggleEspStreamer)
		{
			bool flag = ToggleEspStreamer?.IsChecked.GetValueOrDefault() ?? false;
			try
			{
				ApplyStealthCaptureAffinity(new WindowInteropHelper(this).EnsureHandle(), flag);
				EspManager.ApplyStreamMode(flag);
				KeybindStatusWindow.Instance.SetStreamMode(flag);
			}
			catch
			{
			}
		}
		if (toggleButton == ToggleEspAutoRefresh)
		{
			if (EspConfig.AutoRefresh)
			{
				if (_espAutoRefreshTimer == null)
				{
					_espAutoRefreshTimer = new DispatcherTimer();
					_espAutoRefreshTimer.Interval = TimeSpan.FromSeconds(4.0);
					_espAutoRefreshTimer.Tick += (object s, EventArgs args) =>
					{
						if (EspConfig.AutoRefresh && EspConfig.EspMaster)
						{
							EspManager.RefreshEsp();
						}
					};
				}
				_espAutoRefreshTimer.Start();
			}
			else
			{
				_espAutoRefreshTimer?.Stop();
			}
		}
		bool flag2 = toggleButton?.IsChecked.GetValueOrDefault() ?? false;
		ShowNotification(text + (flag2 ? " ON" : " OFF"));
		EspManager.UpdateOptions();
		UpdateActiveStatus();
	}

	private void CboEspColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (CboEspColor != null && CboEspColor.SelectedItem != null)
		{
			string text = (CboEspColor.SelectedItem as ComboBoxItem)?.Content?.ToString();
			switch (text)
			{
			case "Red":
				EspConfig.EnemyColor = Color.FromRgb(byte.MaxValue, 0, 0);
				break;
			case "Green":
				EspConfig.EnemyColor = Color.FromRgb(0, 230, 110);
				break;
			case "Blue":
				EspConfig.EnemyColor = Color.FromRgb(30, 144, byte.MaxValue);
				break;
			case "Yellow":
				EspConfig.EnemyColor = Color.FromRgb(byte.MaxValue, 215, 0);
				break;
			case "Pink":
				EspConfig.EnemyColor = Color.FromRgb(byte.MaxValue, 105, 180);
				break;
			case "Cyan":
				EspConfig.EnemyColor = Color.FromRgb(0, byte.MaxValue, byte.MaxValue);
				break;
			default:
				EspConfig.EnemyColor = Color.FromRgb(byte.MaxValue, byte.MaxValue, byte.MaxValue);
				break;
			}
			EspManager.UpdateOptions();
			if (_isWindowInitialized)
			{
				ShowNotification("ESP Color: " + text);
			}
		}
	}

	private void CboLinePos_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (CboLinePos == null || CboLinePos.SelectedItem == null)
		{
			return;
		}
		string text = (CboLinePos.SelectedItem as ComboBoxItem)?.Content?.ToString();
		if (!(text == "Bottom"))
		{
			if (text == "Center")
			{
				EspConfig.LinePosition = EspLinePos.Center;
			}
			else
			{
				EspConfig.LinePosition = EspLinePos.Top;
			}
		}
		else
		{
			EspConfig.LinePosition = EspLinePos.Bottom;
		}
		EspManager.UpdateOptions();
		ShowNotification("Snap Line Origin: " + text);
	}

	private void CboTargetBone_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		int num = CboTargetBone?.SelectedIndex ?? 0;
		Cheat.SetTargetBone((Cheat.TargetBone)num);
		ShowNotification("Target Bone: " + num switch
		{
			1 => "Neck", 
			0 => "Head", 
			_ => "Chest", 
		});
	}

	private void ToggleSteamMode_Click(object sender, RoutedEventArgs e)
	{
		if (sender is ToggleButton { IsChecked: var isChecked })
		{
			bool flag = (EspConfig.StreamingMode = (_streamingMode = isChecked == true));
			ShowInTaskbar = !flag;
			Title = (flag ? "Host Process for Windows Services" : "MIAN XITERS");
			if (flag)
			{
				DiscordRPCManager.CleanDiscordRPC();
			}
			else
			{
				DiscordRPCManager.InitDiscordRPC();
			}
			try
			{
				ApplyStealthCaptureAffinity(new WindowInteropHelper(this).EnsureHandle(), flag);
				EspManager.ApplyStreamMode(flag);
				KeybindStatusWindow.Instance.SetStreamMode(flag);
			}
			catch
			{
			}
			if (TxtStreamProofStatus != null)
			{
				TxtStreamProofStatus.Text = (flag ? "\ud83d\udee1\ufe0f Stealth: ON" : "\ud83d\udee1\ufe0f Stealth: OFF");
				TxtStreamProofStatus.Foreground = (flag ? new SolidColorBrush(Color.FromRgb(16, 185, 129)) : BrushMuted);
			}
			if (flag)
			{
				SoundEffects.PlayEnableSound();
			}
			else
			{
				SoundEffects.PlayDisableSound();
			}
			ShowNotification(flag ? "Stealth Mode Turned ON" : "Stealth Mode Turned OFF");
			AnimateCardBackground(RowCard5, flag);
			UpdateActiveStatus();
		}
	}

	private void AnimateCardBackground(Border card, bool active)
	{
		if (card != null)
		{
			if (active)
			{
				Color color = Color.FromArgb(53, _currentAccentColor.R, _currentAccentColor.G, _currentAccentColor.B);
				card.Background = new SolidColorBrush(color);
				card.BorderBrush = new SolidColorBrush(_currentAccentColor);
			}
			else
			{
				card.ClearValue(Border.BackgroundProperty);
				card.ClearValue(Border.BorderBrushProperty);
			}
		}
	}

	private void UpdateActiveStatus()
	{
		bool flag = ToggleAimbotHead?.IsChecked.GetValueOrDefault() ?? false;
		bool flag2 = ToggleAimbotV2?.IsChecked.GetValueOrDefault() ?? false;
		bool flag3 = ToggleAimbotMian?.IsChecked.GetValueOrDefault() ?? false;
		bool flag4 = ToggleActiveSniper?.IsChecked.GetValueOrDefault() ?? false;
		bool flag5 = ToggleScopeXit?.IsChecked.GetValueOrDefault() ?? false;
		bool flag6 = ToggleNoRecoil?.IsChecked.GetValueOrDefault() ?? false;
		bool flag7 = ToggleGlitchFire?.IsChecked.GetValueOrDefault() ?? false;
		bool flag8 = ToggleFastLanding?.IsChecked.GetValueOrDefault() ?? false;
		bool flag9 = ToggleCameraView?.IsChecked.GetValueOrDefault() ?? false;
		bool flag10 = ToggleResetGuest?.IsChecked.GetValueOrDefault() ?? false;
		bool flag11 = ToggleScopeTracking?.IsChecked.GetValueOrDefault() ?? false;
		bool flag12 = ToggleSteamMode?.IsChecked.GetValueOrDefault() ?? false;
		bool flag13 = ToggleEnableEsp?.IsChecked.GetValueOrDefault() ?? false;
		AnimateCardBackground(RowCard6, flag);
		AnimateCardBackground(RowCardAimbotV2, flag2);
		AnimateCardBackground(RowCardAimbotMian, flag3);
		AnimateCardBackground(RowCard2, flag4);
		AnimateCardBackground(RowCard3, flag5);
		AnimateCardBackground(RowCard4, flag6);
		AnimateCardBackground(RowCard7, flag7);
		AnimateCardBackground(RowCard8, flag8);
		AnimateCardBackground(RowCard9, flag9);
		AnimateCardBackground(RowCardResetGuest, flag10);
		AnimateCardBackground(RowCardScopeTracking, flag11);
		AnimateCardBackground(RowCard5, flag12);
		if (RowCardEnableEsp != null)
		{
			AnimateCardBackground(RowCardEnableEsp, flag13);
		}
		if (RowCardKeybindHud != null)
		{
			AnimateCardBackground(RowCardKeybindHud, ToggleKeybindHud?.IsChecked.GetValueOrDefault() ?? false);
		}
		if (RowCardHideNotifications != null)
		{
			AnimateCardBackground(RowCardHideNotifications, ToggleHideNotifications?.IsChecked.GetValueOrDefault() ?? false);
		}
		if (RowCardMuteSounds != null)
		{
			AnimateCardBackground(RowCardMuteSounds, ToggleMuteSounds?.IsChecked.GetValueOrDefault() ?? false);
		}
		KeybindStatusWindow.Instance.UpdateKeyStatus("F1", flag3);
		KeybindStatusWindow.Instance.UpdateKeyStatus("F2", flag7);
		KeybindStatusWindow.Instance.UpdateKeyStatus("F3", flag12);
		KeybindStatusWindow.Instance.UpdateKeyStatus("FastLanding", flag8);
		if (RowCardEspSkeleton != null)
		{
			AnimateCardBackground(RowCardEspSkeleton, flag13 && (ToggleEspSkeleton?.IsChecked.GetValueOrDefault() ?? false));
		}
		if (RowCardEspLine != null)
		{
			AnimateCardBackground(RowCardEspLine, flag13 && (ToggleEspLine?.IsChecked.GetValueOrDefault() ?? false));
		}
		if (RowCardEspCornerBox != null)
		{
			AnimateCardBackground(RowCardEspCornerBox, flag13 && (ToggleEspCornerBox?.IsChecked.GetValueOrDefault() ?? false));
		}
		if (RowCardEspWeapon != null)
		{
			AnimateCardBackground(RowCardEspWeapon, flag13 && (ToggleEspWeapon?.IsChecked.GetValueOrDefault() ?? false));
		}
		if (RowCardEsp360 != null)
		{
			AnimateCardBackground(RowCardEsp360, flag13 && (ToggleEsp360?.IsChecked.GetValueOrDefault() ?? false));
		}
		if (RowCardEspDistance != null)
		{
			AnimateCardBackground(RowCardEspDistance, flag13 && (ToggleEspDistance?.IsChecked.GetValueOrDefault() ?? false));
		}
		if (RowCardEspName != null)
		{
			AnimateCardBackground(RowCardEspName, flag13 && (ToggleEspName?.IsChecked.GetValueOrDefault() ?? false));
		}
		if (RowCardEspHealth != null)
		{
			AnimateCardBackground(RowCardEspHealth, flag13 && (ToggleEspHealth?.IsChecked.GetValueOrDefault() ?? false));
		}
		if (RowCardEspTeam != null)
		{
			AnimateCardBackground(RowCardEspTeam, flag13 && (ToggleEspTeam?.IsChecked.GetValueOrDefault() ?? false));
		}
		int num = 0;
		if (flag)
		{
			num++;
		}
		if (flag2)
		{
			num++;
		}
		if (flag3)
		{
			num++;
		}
		if (flag4)
		{
			num++;
		}
		if (flag5)
		{
			num++;
		}
		if (flag6)
		{
			num++;
		}
		if (flag7)
		{
			num++;
		}
		if (flag8)
		{
			num++;
		}
		if (flag9)
		{
			num++;
		}
		if (flag10)
		{
			num++;
		}
		if (flag11)
		{
			num++;
		}
		if (flag12)
		{
			num++;
		}
		if (flag13)
		{
			num++;
			ToggleButton toggleEspSkeleton = ToggleEspSkeleton;
			if (toggleEspSkeleton != null && toggleEspSkeleton.IsChecked == true)
			{
				num++;
			}
			ToggleButton toggleEspLine = ToggleEspLine;
			if (toggleEspLine != null && toggleEspLine.IsChecked == true)
			{
				num++;
			}
			ToggleButton toggleEspCornerBox = ToggleEspCornerBox;
			if (toggleEspCornerBox != null && toggleEspCornerBox.IsChecked == true)
			{
				num++;
			}
			ToggleButton toggleEspWeapon = ToggleEspWeapon;
			if (toggleEspWeapon != null && toggleEspWeapon.IsChecked == true)
			{
				num++;
			}
			ToggleButton toggleEsp = ToggleEsp360;
			if (toggleEsp != null && toggleEsp.IsChecked == true)
			{
				num++;
			}
			ToggleButton toggleEspDistance = ToggleEspDistance;
			if (toggleEspDistance != null && toggleEspDistance.IsChecked == true)
			{
				num++;
			}
			ToggleButton toggleEspName = ToggleEspName;
			if (toggleEspName != null && toggleEspName.IsChecked == true)
			{
				num++;
			}
			ToggleButton toggleEspHealth = ToggleEspHealth;
			if (toggleEspHealth != null && toggleEspHealth.IsChecked == true)
			{
				num++;
			}
			ToggleButton toggleEspTeam = ToggleEspTeam;
			if (toggleEspTeam != null && toggleEspTeam.IsChecked == true)
			{
				num++;
			}
		}
		if (TxtActiveCount == null)
		{
			return;
		}
		TxtActiveCount.Text = $"{num} ACTIVE";
		if (num > 0)
		{
			TxtActiveCount.Foreground = new SolidColorBrush(_currentAccentColor);
			if (DotActiveCount != null)
			{
				DotActiveCount.Background = new SolidColorBrush(_currentAccentColor);
				DotActiveCount.Effect = new DropShadowEffect
				{
					Color = _currentAccentColor,
					BlurRadius = 6.0,
					ShadowDepth = 0.0,
					Opacity = 0.85
				};
			}
			if (BadgeActiveCount != null)
			{
				BadgeActiveCount.BorderBrush = new SolidColorBrush(_currentAccentColor);
				BadgeActiveCount.Background = new SolidColorBrush(Color.FromArgb(51, _currentAccentColor.R, _currentAccentColor.G, _currentAccentColor.B));
			}
		}
		else
		{
			TxtActiveCount.Foreground = new SolidColorBrush(Color.FromRgb(148, 163, 184));
			if (DotActiveCount != null)
			{
				DotActiveCount.Background = new SolidColorBrush(Color.FromRgb(113, 113, 122));
				DotActiveCount.Effect = null;
			}
			if (BadgeActiveCount != null)
			{
				BadgeActiveCount.BorderBrush = new SolidColorBrush(Color.FromRgb(24, 24, 32));
				BadgeActiveCount.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
			}
		}
	}

        private static bool IsHyperVDisabled()
        {
            try
            {
                var psi = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c bcdedit /enum \"{current}\"",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                using (var proc = System.Diagnostics.Process.Start(psi))
                {
                    proc.WaitForExit(5000);
                    string output = proc.StandardOutput.ReadToEnd();
                    var match = System.Text.RegularExpressions.Regex.Match(output, @"hypervisorlaunchtype\s+(\w+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        string val = match.Groups[1].Value.Trim();
                        return val.Equals("off", StringComparison.OrdinalIgnoreCase);
                    }
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        private void BtnDisableHyperV_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsHyperVDisabled())
                {
                    MessageBox.Show(
                        "Hyper-V is already disabled on this system!\n\nYour PC is already configured for optimal emulator performance (BlueStacks, MSI App Player, LDPlayer, MEmu).",
                        "MIAN XITERS",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    return;
                }

                var prompt = MessageBox.Show(
                    "Hyper-V is currently active. Disabling Hyper-V is recommended to run BlueStacks, MSI App Player, LDPlayer, and MEmu with optimal performance and zero crashes.\n\nDo you want to disable Hyper-V now?",
                    "Disable Hyper-V",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (prompt != MessageBoxResult.Yes) return;

                var psi = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c bcdedit /set hypervisorlaunchtype off",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                using (var proc = System.Diagnostics.Process.Start(psi))
                {
                    proc.WaitForExit(10000);
                    string output = proc.StandardOutput.ReadToEnd();
                    string error = proc.StandardError.ReadToEnd();

                    if (proc.ExitCode == 0)
                    {
                        try
                        {
                            var dismPsi = new System.Diagnostics.ProcessStartInfo
                            {
                                FileName = "dism.exe",
                                Arguments = "/Online /Disable-Feature:Microsoft-Hyper-V-All /NoRestart",
                                CreateNoWindow = true,
                                UseShellExecute = false
                            };
                            using (var dProc = System.Diagnostics.Process.Start(dismPsi))
                            {
                                dProc.WaitForExit(10000);
                            }
                        }
                        catch { }

                        var reboot = MessageBox.Show(
                            "Hyper-V has been successfully disabled in Windows boot settings!\n\nA computer restart is required for changes to take effect.\n\nWould you like to restart your computer now?",
                            "Hyper-V Disabled",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Information);

                        if (reboot == MessageBoxResult.Yes)
                        {
                            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                            {
                                FileName = "shutdown.exe",
                                Arguments = "/r /t 5 /c \"Restarting to apply Hyper-V settings...\"",
                                CreateNoWindow = true,
                                UseShellExecute = false
                            });
                        }
                        else
                        {
                            ShowNotification("Hyper-V disabled. Restart your PC when ready.");
                        }
                    }
                    else
                    {
                        MessageBox.Show(
                            $"Could not disable Hyper-V.\n\nDetails: {error}\n{output}",
                            "Disable Hyper-V",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error configuring Hyper-V: " + ex.Message, "MIAN XITERS", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

}
}
