using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Client;
using ImGuiNET;

namespace AotForms;

internal class RenderImgui
{
	private static bool showMenu = true;

	private static string licenseKey = "";

	private static bool isLoggedIn = true;

	private static string loginMessage = "";

	private static DateTime lastToggleTime = DateTime.MinValue;

	private static bool hotkeyThreadStarted = false;

	private static readonly string[] TabNames = new string[4] { "Combat", "Visuals", "Skins", "Settings" };

	private static int selectedTab = 0;

	private static bool cornerBox = true;

	private static bool wukongEsp = false;

	private static bool bloodWarning = false;

	private static bool lootAttachments = false;

	private static bool skinEvoGuns = true;

	private static bool skinAK47 = false;

	private static bool skinMP40 = true;

	private static bool skinM1014 = false;

	private static bool skinCrimRed = true;

	private static bool skinSakura = false;

	private static bool isExternalAimbotActive = false;
	private static string externalAimbotStatus = "";
	private static Vector4 externalAimbotStatusColor = new Vector4(1f, 1f, 1f, 1f);
	private static string aimHeadAOB = "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 00 00 A5 43";
	private static string readHeadHead = "0xB7";
	private static string writeHead = "0xB3";
	private static System.Collections.Generic.Dictionary<long, int> headOrigHead = new System.Collections.Generic.Dictionary<long, int>();
	private static System.Collections.Generic.Dictionary<long, int> headOrigWrite = new System.Collections.Generic.Dictionary<long, int>();
	private static FAST_MEMORY.FastMemory memScanner = new FAST_MEMORY.FastMemory();

	private static async void ToggleExternalAimbot()
	{
		if (!isExternalAimbotActive)
		{
			DeactivateExternalAimbot();
			return;
		}

		headOrigHead.Clear();
		headOrigWrite.Clear();
		externalAimbotStatus = "Scanning...";
		externalAimbotStatusColor = new Vector4(1f, 1f, 0f, 1f);

		var p = Process.GetProcessesByName("HD-Player");
		if (p.Length == 0) p = Process.GetProcessesByName("aow_exe");
		if (p.Length == 0) p = Process.GetProcessesByName("MsiPlayer");
		if (p.Length == 0) p = Process.GetProcessesByName("Bluestacks");

		if (p.Length == 0)
		{
			externalAimbotStatus = "Emulator Not Found!";
			externalAimbotStatusColor = new Vector4(1f, 0.2f, 0.2f, 1f);
			isExternalAimbotActive = false;
			return;
		}

		try
		{
			long ro = Convert.ToInt64(readHeadHead, 16);
			long wo = Convert.ToInt64(writeHead, 16);
			memScanner.OpenProcess(p[0].Id);
			var r = await memScanner.ScanPattern(aimHeadAOB, true, true);
			if (r != null && r.Count > 0)
			{
				foreach (var a in r)
				{
					long ha = a + ro, ca = a + wo;
					byte[] hb = memScanner.ReadBytes(ha, 4);
					byte[] cb = memScanner.ReadBytes(ca, 4);
					if (hb == null || cb == null || hb.Length < 4 || cb.Length < 4) continue;
					int hv = BitConverter.ToInt32(hb, 0), cv = BitConverter.ToInt32(cb, 0);
					headOrigHead[ha] = hv;
					headOrigWrite[ca] = cv;
					memScanner.WriteMemory(ca.ToString("X"), "int", hv.ToString());
					memScanner.WriteMemory(ha.ToString("X"), "int", cv.ToString());
				}
				isExternalAimbotActive = true;
				externalAimbotStatus = "ON";
				externalAimbotStatusColor = new Vector4(0.2f, 1f, 0.2f, 1f);
			}
			else
			{
				externalAimbotStatus = "Pattern Not Found!";
				externalAimbotStatusColor = new Vector4(1f, 0.2f, 0.2f, 1f);
				isExternalAimbotActive = false;
			}
		}
		catch (Exception ex)
		{
			externalAimbotStatus = "Error: " + ex.Message;
			externalAimbotStatusColor = new Vector4(1f, 0.2f, 0.2f, 1f);
			isExternalAimbotActive = false;
		}
	}

	private static void DeactivateExternalAimbot()
	{
		try
		{
			foreach (var item in headOrigWrite)
			{
				memScanner.WriteMemory(item.Key.ToString("X"), "int", item.Value.ToString());
			}
			foreach (var item in headOrigHead)
			{
				memScanner.WriteMemory(item.Key.ToString("X"), "int", item.Value.ToString());
			}
		}
		catch { }
		headOrigHead.Clear();
		headOrigWrite.Clear();
		isExternalAimbotActive = false;
		externalAimbotStatus = "OFF";
		externalAimbotStatusColor = new Vector4(0.7f, 0.7f, 0.7f, 1f);
	}

	public static void ApplyStyleFF()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_041e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0448: Unknown result type (might be due to invalid IL or missing references)
		//IL_044d: Unknown result type (might be due to invalid IL or missing references)
		ImGuiStylePtr style = ImGui.GetStyle();
		style.WindowRounding = 8f;
		style.WindowBorderSize = 1f;
		style.WindowPadding = new Vector2(14f, 12f);
		style.FrameRounding = 5f;
		style.FrameBorderSize = 1f;
		style.ItemSpacing = new Vector2(8f, 7f);
		style.Colors[2] = new Vector4(0.04f, 0.04f, 0.05f, 0.98f);
		style.Colors[5] = new Vector4(0.18f, 0.18f, 0.22f, 0.7f);
		style.Colors[6] = new Vector4(0f, 0f, 0f, 0f);
		style.Colors[7] = new Vector4(0.08f, 0.08f, 0.1f, 1f);
		style.Colors[8] = new Vector4(0.12f, 0.12f, 0.16f, 1f);
		style.Colors[9] = new Vector4(0.16f, 0.16f, 0.22f, 1f);
		style.Colors[10] = new Vector4(0.04f, 0.04f, 0.06f, 1f);
		style.Colors[11] = new Vector4(0.06f, 0.06f, 0.08f, 1f);
		style.Colors[12] = new Vector4(0.03f, 0.03f, 0.04f, 1f);
		style.Colors[18] = new Vector4(1f, 1f, 1f, 1f);
		style.Colors[19] = new Vector4(0.92f, 0.92f, 0.96f, 1f);
		style.Colors[20] = new Vector4(1f, 1f, 1f, 1f);
		style.Colors[21] = new Vector4(0.12f, 0.12f, 0.15f, 1f);
		style.Colors[22] = new Vector4(0.22f, 0.22f, 0.28f, 1f);
		style.Colors[23] = new Vector4(0.32f, 0.32f, 0.4f, 1f);
		style.Colors[24] = new Vector4(0.18f, 0.18f, 0.24f, 0.8f);
		style.Colors[25] = new Vector4(0.26f, 0.26f, 0.34f, 0.8f);
		style.Colors[26] = new Vector4(0.34f, 0.34f, 0.44f, 0.8f);
		style.Colors[14] = new Vector4(0f, 0f, 0f, 0f);
		style.Colors[15] = new Vector4(0f, 0f, 0f, 0f);
		style.Colors[16] = new Vector4(0f, 0f, 0f, 0f);
		style.Colors[17] = new Vector4(0f, 0f, 0f, 0f);
		style.ScrollbarSize = 0f;
	}

	[DllImport("user32.dll")]
	private static extern short GetAsyncKeyState(Keys vKey);

	private static bool IsKeyPressed(Keys key)
	{
		return (GetAsyncKeyState(key) & 1) != 0;
	}

	public void Draw()
	{
		Render();
	}

	public static void Render()
	{
		if (IsKeyPressed(Keys.Insert) && DateTime.Now - lastToggleTime > TimeSpan.FromMilliseconds(200.0))
		{
			showMenu = !showMenu;
			lastToggleTime = DateTime.Now;
		}
		if (!showMenu)
		{
			return;
		}
		if (!isLoggedIn)
		{
			ImGui.SetNextWindowSize(new Vector2(320f, string.IsNullOrEmpty(loginMessage) ? 164 : 186), (ImGuiCond)1);
			ImGui.Begin("GaySiTo Login", (ImGuiWindowFlags)34);
			float x = ImGui.CalcTextSize("Enter your License Key").X;
			ImGui.SetCursorPosX((ImGui.GetWindowSize().X - x) / 2f);
			ImGui.Text("Enter your License Key");
			ImGui.Spacing();
			float num = ImGui.GetWindowSize().X - 50f;
			float cursorPosX = (ImGui.GetWindowSize().X - num) / 2f;
			ImGui.SetCursorPosX(cursorPosX);
			ImGui.PushStyleVar((ImGuiStyleVar)11, new Vector2(4f, 10f));
			ImGui.SetNextItemWidth(num);
			ImGui.InputText("##license", ref licenseKey, 100u, (ImGuiInputTextFlags)16);
			ImGui.PopStyleVar();
			ImGui.Spacing();
			ImGui.SetCursorPosX(cursorPosX);
			if (ImGui.Button("Sign In", new Vector2(num, 36f)))
			{
				isLoggedIn = true;
				loginMessage = "Login successful!";
				StartHotkeyThread();
			}
			if (!string.IsNullOrEmpty(loginMessage))
			{
				ImGui.Spacing();
				float x2 = ImGui.CalcTextSize(loginMessage).X;
				ImGui.SetCursorPosX((ImGui.GetWindowSize().X - x2) / 2f);
				ImGui.TextColored(isLoggedIn ? new Vector4(0.2f, 1f, 0.2f, 1f) : new Vector4(1f, 0.2f, 0.2f, 1f), loginMessage);
			}
			ImGui.Spacing();
			string text = "Buy Key: Inbox @GaySiTo ";
			float x3 = ImGui.CalcTextSize(text).X;
			ImGui.SetCursorPosX((ImGui.GetWindowSize().X - x3) / 2f);
			ImGui.TextColored(new Vector4(0.7f, 0.7f, 0.7f, 1f), text);
			ImGui.End();
			return;
		}
		ImGui.SetNextWindowSize(new Vector2(340f, 320f));
		ImGui.Begin("MODE MENU", (ImGuiWindowFlags)2);
		if (ImGui.BeginTabBar("MainTabs", (ImGuiTabBarFlags)0))
		{
			if (ImGui.BeginTabItem("Aimbot"))
			{
				ImGui.Checkbox("Aim Legit", ref Config.AimLegit);
				ImGui.Spacing();
				ImGui.Separator();
				ImGui.Spacing();
				if (ImGui.Checkbox("Aimbot External", ref isExternalAimbotActive))
				{
					ToggleExternalAimbot();
				}
				if (!string.IsNullOrEmpty(externalAimbotStatus))
				{
					ImGui.SameLine();
					ImGui.TextColored(externalAimbotStatusColor, externalAimbotStatus);
				}
				ImGui.Spacing();
				ImGui.Separator();
				ImGui.Spacing();
				ImGui.Checkbox("Aim Mouse", ref Config.AimbotVisible1);
				if (Config.AimbotVisible1)
				{
					ImGui.Indent();
					if (!Config.DrawFov)
					{
						ImGui.TextColored(new Vector4(1f, 0.4f, 0.4f, 1f), "(!) Can bat 'Draw FOV' de hoat dong");
					}
					ImGui.SliderFloat("Aim Smooth", ref Config.AimMouseSmooth, 1.5f, 20f, "%.1f");
					ImGui.Unindent();
				}
				ImGui.Spacing();
				ImGui.Separator();
				ImGui.Spacing();
				ImGui.Checkbox("Draw FOV", ref Config.DrawFov);
				ImGui.SliderFloat("FOV Radius", ref Config.AimbotFOV, 0f, 1000f);
				ImGui.Checkbox("Ignore Knocked", ref Config.IgnoreKnocked);
				ImGui.Checkbox("No Recoil", ref Config.NoRecoil);
				ImGui.Spacing();
				ImGui.Separator();
				ImGui.Spacing();
				ImGui.EndTabItem();
			}
			if (ImGui.BeginTabItem("Visual"))
			{
				ImGui.Text("Visuals Panel");
				ImGui.Checkbox("ESP Master", ref Config.EspMaster);
				if (Config.EspMaster)
				{
					ImGui.Checkbox("Show Name", ref Config.ESPName);
					ImGui.Checkbox("Show Line", ref Config.ESPLine);
					ImGui.Checkbox("Show Box", ref Config.ESPBox);
					ImGui.Checkbox("Show Distance", ref Config.ESPDistance);
					ImGui.Checkbox("Show Health", ref Config.ESPHealth);
					ImGui.Checkbox("Show Skeleton", ref Config.ESPSkeleton);
					ImGui.Checkbox("Show 360", ref Config.Esp360);
					ImGui.Checkbox("Show Wukong", ref Config.IsVisible);
					ImGui.Checkbox("Show Weapon", ref Config.ESPWeapon);
					ImGui.Checkbox("Show Team", ref Config.ESPTeam);
				}
				if (ImGui.Button("Update Entities"))
				{
					UpdateEntities();
					Core.Entities = new ConcurrentDictionary<ulong, Entity>();
					Memory.ClearCaches();
				}
				ImGui.EndTabItem();
			}
			if (ImGui.BeginTabItem("Settings"))
			{
				ImGui.Text("Settings Panel");
				if (Offsets.Il2CppBase != 0L)
				{
					ImGui.TextColored(new Vector4(0.2f, 1f, 0.2f, 1f), $"il2cpp: 0x{Offsets.Il2CppBase:X}");
				}
				else
				{
					ImGui.TextColored(new Vector4(1f, 0.2f, 0.2f, 1f), "il2cpp: deo tim thay");
				}
				ImGui.Checkbox("Stream Mode", ref Config.StreamMode);
				bool flag = false;
				ImGui.Checkbox("Close Panel", ref flag);
				if (flag)
				{
					KillProcess("HD-Adb");
					Task.Delay(2000);
					KillProcess("HD-Player");
					Task.Delay(1000);
					Environment.Exit(0);
				}
				ImGui.EndTabItem();
			}
			ImGui.EndTabBar();
		}
		ImGui.End();
	}

	public static void UpdateEntities()
	{
		foreach (Entity value in Core.Entities.Values)
		{
			if (value.IsTeam == Bool3.False)
			{
				new TreeNode(value.Name)
				{
					Nodes = 
					{
						new TreeNode($"IsKnown: {value.IsKnown}"),
						new TreeNode($"IsTeam: {value.IsTeam}"),
						new TreeNode($"Head: {value.Head}"),
						new TreeNode($"Root: {value.Root}"),
						new TreeNode($"Health: {value.Health}"),
						new TreeNode($"IsDead: {value.IsDead}"),
						new TreeNode($"IsKnocked: {value.IsKnocked}")
					}
				};
			}
		}
		Thread.Sleep(1000);
	}

	public static void KillProcess(string processName)
	{
		Process[] processesByName = Process.GetProcessesByName(processName);
		foreach (Process obj in processesByName)
		{
			obj.Kill();
			obj.WaitForExit();
		}
	}

	public static void StartHotkeyThread()
	{
		if (hotkeyThreadStarted)
		{
			return;
		}
		hotkeyThreadStarted = true;
		Thread thread = new Thread(() =>
		{
			while (true)
			{
				try
				{
					if (Config.SpKey != Keys.None && KeyHelper.IsKeyDown(Config.SpKey))
					{
						if (!Config.KeyAlreadyPressed)
						{
							Config.speed = !Config.speed;
							Config.KeyAlreadyPressed = true;
						}
					}
					else
					{
						Config.KeyAlreadyPressed = false;
					}
					if (Config.TelePortKey != Keys.None && KeyHelper.IsKeyDown(Config.TelePortKey))
					{
						if (!Config.KeyAlreadyPressed3)
						{
							Config.telekill = !Config.telekill;
							Config.KeyAlreadyPressed3 = true;
						}
					}
					else
					{
						Config.KeyAlreadyPressed3 = false;
					}
					if (Config.TeleKillKey != Keys.None && KeyHelper.IsKeyDown(Config.TeleKillKey))
					{
						if (!Config.KeyAlreadyPressed4)
						{
							Config.proxtelekill = !Config.proxtelekill;
							Config.KeyAlreadyPressed4 = true;
						}
					}
					else
					{
						Config.KeyAlreadyPressed4 = false;
					}
					if (Config.UpPlayerKey != Keys.None && KeyHelper.IsKeyDown(Config.UpPlayerKey))
					{
						if (!Config.KeyAlreadyPressed6)
						{
							Config.UpPlayer = !Config.UpPlayer;
							Config.KeyAlreadyPressed6 = true;
						}
					}
					else
					{
						Config.KeyAlreadyPressed6 = false;
					}
					Thread.Sleep(10);
				}
				catch
				{
				}
			}
		});
		thread.IsBackground = true;
		thread.Start();
	}
}
