using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClickableTransparentOverlay;
using ImGuiNET;

namespace AotForms;

internal class ESP : Overlay
{
	private enum WDA : uint
	{
		WDA_NONE = 0u,
		WDA_MONITOR = 1u,
		WDA_EXCLUDEFROMCAPTURE = 17u
	}

	private struct EntityRenderData
	{
		public Vector2 headScreenPos;

		public Vector2 bottomScreenPos;

		public float Distance;

		public bool IsValid;
	}

	public static class FontManager
	{
		public static ImFontPtr SmallFont;

		public static ImFontPtr BigFont;
	}

	private string fontpath = "C:\\Windows\\Fonts\\taileb.ttf";

	private const int GWL_EXSTYLE = -20;

	private const int WS_EX_TOOLWINDOW = 128;

	private const int WS_EX_APPWINDOW = 262144;

	private const int SW_HIDE = 0;

	private const int SW_SHOW = 5;

	public static int AimAssistRadius = 100;

	private bool isProcessing;

	private float progress;

	private ConcurrentDictionary<int, EntityRenderData> processedEntities = new ConcurrentDictionary<int, EntityRenderData>();

	private Task entityProcessingTask;

	private static readonly ConcurrentDictionary<string, string> _weaponPathCache = new ConcurrentDictionary<string, string>();

	internal static bool IsLoading = false;

	private bool fontLoaded;

	private CancellationTokenSource _cancellationTokenSource;

	private int EnemyCount;

	private nint hWnd;

	private bool isAutoRefreshActive;

	[DllImport("user32.dll")]
	private static extern bool SetWindowDisplayAffinity(nint hWnd, uint dwAffinity);

	private static string ResolveWeaponImagePath(string weaponName)
	{
		if (string.IsNullOrEmpty(weaponName))
		{
			return null;
		}
		if (_weaponPathCache.TryGetValue(weaponName, out var value))
		{
			return value;
		}
		string text = weaponName.ToLower().Trim();
		string text2 = text.Replace(" ", "").Replace("-", "");
		string text3 = text.Split('-')[0].Trim();
		string[] array = new string[6]
		{
			"C:\\weaponff",
			Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ảnh súng"),
			Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\ảnh súng"),
			Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "weaponff"),
			Path.Combine(Directory.GetCurrentDirectory(), "ảnh súng"),
			"d:\\AIM\\ảnh súng"
		};
		foreach (string text4 in array)
		{
			try
			{
				if (!Directory.Exists(text4))
				{
					continue;
				}
				string text5 = Path.Combine(text4, text + ".png");
				if (File.Exists(text5))
				{
					_weaponPathCache[weaponName] = text5;
					return text5;
				}
				string text6 = Path.Combine(text4, text2 + ".png");
				if (File.Exists(text6))
				{
					_weaponPathCache[weaponName] = text6;
					return text6;
				}
				if (!string.IsNullOrEmpty(text3) && text3 != text)
				{
					string text7 = Path.Combine(text4, text3 + ".png");
					if (File.Exists(text7))
					{
						_weaponPathCache[weaponName] = text7;
						return text7;
					}
				}
			}
			catch
			{
			}
		}
		_weaponPathCache[weaponName] = null;
		return null;
	}

	[DllImport("user32.dll")]
	private static extern short GetAsyncKeyState(Keys vKey);

	public static bool IsKeyDown(Keys key)
	{
		return (GetAsyncKeyState(key) & 0x8000) != 0;
	}

	private async void autorefresh_Tick(object sender, EventArgs e)
	{
		while (isAutoRefreshActive)
		{
			Memory.ClearCaches();
			Core.Entities = new ConcurrentDictionary<ulong, Entity>();
			await Task.Delay(1000);
		}
	}

	private static bool IsKeyPressed(Keys key)
	{
		return (GetAsyncKeyState(key) & 1) != 0;
	}

	protected override void Render()
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0861: Unknown result type (might be due to invalid IL or missing references)
		//IL_089e: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a55: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0970: Unknown result type (might be due to invalid IL or missing references)
		//IL_0975: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a37: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0659: Unknown result type (might be due to invalid IL or missing references)
		//IL_065e: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c4: Unknown result type (might be due to invalid IL or missing references)
		CreateHandle();
		if (!fontLoaded)
		{
			try
			{
				((Overlay)this).ReplaceFont(fontpath, 15, (FontGlyphRangeType)6);
			}
			catch
			{
				try
				{
					((Overlay)this).ReplaceFont("C:\\Windows\\Fonts\\arial.ttf", 15, (FontGlyphRangeType)0);
				}
				catch
				{
				}
			}
			ImGuiIOPtr iO = ImGui.GetIO();
			FontManager.SmallFont = iO.FontDefault;
			iO = ImGui.GetIO();
			FontManager.BigFont = iO.FontDefault;
			fontLoaded = true;
		}
		if (IsLoading)
		{
			ImDrawListPtr foregroundDrawList = ImGui.GetForegroundDrawList();
			float num = ((Core.Width > 0) ? Core.Width : ((Overlay)this).Size.Width);
			float num2 = ((Core.Height > 0) ? Core.Height : ((Overlay)this).Size.Height);
			foregroundDrawList.AddRectFilled(Vector2.Zero, new Vector2(num, num2), 3422552064u);
			float num3 = (float)ImGui.GetTime();
			float num4 = 30f;
			Vector2 vector = new Vector2(num / 2f, num2 / 2f);
			int num5 = 64;
			Vector2[] array = new Vector2[num5 + 1];
			float num6 = num3 * 4f;
			float num7 = 5.0265484f;
			for (int i = 0; i <= num5; i++)
			{
				float x = num6 + num7 * (float)i / (float)num5;
				array[i] = vector + new Vector2(MathF.Cos(x), MathF.Sin(x)) * num4;
			}
			foregroundDrawList.AddPolyline(ref array[0], array.Length, ColorToUint32(Color.White), (ImDrawFlags)0, 4f);
			return;
		}
		RenderImgui.Render();
		if (!Config.EspMaster || !Core.HaveMatrix || Core.CameraMatrix == Matrix4x4.Identity)
		{
			return;
		}
		int width = Core.Width;
		_ = Core.Height;
		ImDrawListPtr val;
		if (Config.DrawFov)
		{
			int num8 = 1000;
			val = ImGui.GetForegroundDrawList();
			val.AddCircle(new Vector2((float)Core.Width / 2f, (float)Core.Height / 2f), Config.AimbotFOV, ColorToUint32(Color.FromArgb(200, Config.FovColor)), num8, 1.0f);
		}
		ConcurrentDictionary<ulong, Entity> entities = Core.Entities;
		if (entities == null || entities.Count == 0)
		{
			return;
		}
		EnemyCount = entities.Values.Count((Entity entity) => !entity.IsDead && entity.IsTeam != Bool3.True);
		ImGui.PushFont(FontManager.BigFont);
		string text = $"{EnemyCount}";
		float x2 = ((float)width - ImGui.CalcTextSize(text).X) / 2f;
		int num9 = 80;
		uint num10 = ImGui.ColorConvertFloat4ToU32(new Vector4(1f, 1f, 1f, 1f));
		ImDrawListPtr foregroundDrawList2 = ImGui.GetForegroundDrawList();
		foregroundDrawList2.AddText(new Vector2(x2, num9), num10, text);
		ImGui.PopFont();
		bool flag = Config.AimbotVisible1 || Config.AimLegit || Config.AimBot;
		foregroundDrawList2.AddText(new Vector2(10f, 10f), flag ? ImGui.GetColorU32(new Vector4(0f, 1f, 0f, 1f)) : ImGui.GetColorU32(new Vector4(1f, 0f, 0f, 1f)), "AimBot : " + (flag ? "ON" : "OFF"));
		EnemyCount = entities.Values.Count((Entity entity) => !entity.IsDead && entity.IsKnown && entity.IsTeam != Bool3.True);
		nint num21 = default;
		uint num22 = default;
		uint num23 = default;
		foreach (Entity value in entities.Values)
		{
			if (value.IsDead || !value.IsKnown || (value.IsTeam == Bool3.True && !Config.ESPTeam) || (!Config.IsVisible && !value.IsVisible))
			{
				continue;
			}
			float num11 = Vector3.Distance(Core.LocalMainCamera, value.Head);
			if (num11 > 200f)
			{
				continue;
			}
			Vector2 vector2 = W2S.WorldToScreen(Core.CameraMatrix, value.Head, Core.Width, Core.Height);
			Vector2 vector3 = W2S.WorldToScreen(Core.CameraMatrix, value.Root, Core.Width, Core.Height);
			if (vector2.X <= 0f || vector2.Y <= 0f || vector3.X <= 0f || vector3.Y <= 0f)
			{
				continue;
			}
			float num12 = Math.Abs(vector2.Y - vector3.Y);
			Vector2 vector4 = W2S.WorldToScreen(Core.CameraMatrix, value.LeftFoot, Core.Width, Core.Height);
			Vector2 vector5 = W2S.WorldToScreen(Core.CameraMatrix, value.RightFoot, Core.Width, Core.Height);
			float num13 = vector3.Y;
			if (vector4.Y > 0f && vector5.Y > 0f)
			{
				num13 = Math.Max(vector4.Y, vector5.Y);
			}
			else if (vector4.Y > 0f)
			{
				num13 = vector4.Y;
			}
			else if (vector5.Y > 0f)
			{
				num13 = vector5.Y;
			}
			float num14 = vector2.Y - 5f;
			num12 = Math.Abs(num13 - num14);
			float num15 = num12 * 0.65f;
			float num16 = vector2.X - num15 / 2f;
			float num17 = num14;
			bool flag2 = value.IsTeam == Bool3.True;
			uint num18 = ColorToUint32(Config.ESPTeamColor);
			bool flag3 = Vector2.Distance(new Vector2((float)Core.Width / 2f, (float)Core.Height / 2f), vector2) <= Config.AimbotFOV;
			bool flag4 = (!flag2 & flag3) && Aimmouse.CurrentTarget != null && value.Address == Aimmouse.CurrentTarget.Address;
			uint num19 = ColorToUint32(Color.FromArgb(240, 0, 180, 255));
			if (Config.ESPLine)
			{
				uint num20;
				if (flag2)
				{
					num20 = num18;
				}
				else if (flag4)
				{
					num20 = num19;
				}
				else
				{
					num20 = ((Config.IgnoreKnocked && value.IsKnocked) ? ColorToUint32(Color.FromArgb(210, Color.Red)) : ColorToUint32(Color.FromArgb(210, Config.ESPLineColor)));
				}
				val = ImGui.GetBackgroundDrawList();
				val.AddLine(new Vector2((float)Core.Width / 2f, 0f), new Vector2(vector2.X, vector2.Y), num20, flag4 ? 1.8f : 1.2f);
			}
			if (Config.ESPWeapon && !string.IsNullOrEmpty(value.WeaponName))
			{
				Vector2 vector6 = new Vector2(95f, 16f);
				if (vector2.X >= 0f && vector2.Y >= 0f && vector2.X <= (float)Core.Width && vector2.Y <= (float)Core.Height)
				{
					Vector2 vector7 = new Vector2(vector2.X - vector6.X / 2f, vector2.Y - vector6.Y - 13f);
					bool flag5 = false;
					try
					{
						string text2 = ResolveWeaponImagePath(value.WeaponName);
						if (!string.IsNullOrEmpty(text2) && File.Exists(text2))
						{
							((Overlay)this).AddOrGetImagePointer(text2, true, out num21, out num22, out num23);
							if (num21 != IntPtr.Zero)
							{
								Vector2 vector8 = new Vector2(60f, 20f);
								Vector2 vector9 = new Vector2(vector7.X + (vector6.X - vector8.X) / 2f, vector7.Y - vector8.Y - 2f);
								val = ImGui.GetForegroundDrawList();
								val.AddImage((IntPtr)num21, vector9, vector9 + vector8);
								flag5 = true;
							}
						}
					}
					catch (Exception ex)
					{
						Console.WriteLine("[ESPWeapon] Image load error: " + ex.Message);
					}
					if (!flag5)
					{
						string weaponName = value.WeaponName;
						Vector2 vector10 = ImGui.CalcTextSize(weaponName);
						Vector2 pos = new Vector2(vector2.X - vector10.X / 2f, vector7.Y - vector10.Y - 2f);
						uint col = (flag2 ? num18 : ColorToUint32(Color.FromArgb(255, 230, 110)));
						DrawOutlinedText(ImGui.GetForegroundDrawList(), pos, col, weaponName);
					}
				}
			}
			if (Config.ESPLineDuoi)
			{
				uint num24;
				if (flag2)
				{
					num24 = num18;
				}
				else
				{
					num24 = (flag4 ? num19 : ColorToUint32(Color.FromArgb(130, Config.ESPLineDuoiColor)));
				}
				val = ImGui.GetBackgroundDrawList();
				val.AddLine(new Vector2((float)Core.Width / 2f, (float)Core.Height / 2f), vector2, num24, flag4 ? 1.5f : 1f);
			}
			if (Config.ESPDistance)
			{
				string text3 = $"{num11:F2}m";
				Vector2 vector11 = ImGui.CalcTextSize(text3);
				Vector2 vector12 = new Vector2(vector3.X - vector11.X / 2f, num13 + 5f);
				uint num25;
				if (flag2)
				{
					num25 = num18;
				}
				else
				{
					num25 = ((Config.IgnoreKnocked && value.IsKnocked) ? ColorToUint32(Color.Red) : ColorToUint32(Color.White));
				}
				val = ImGui.GetForegroundDrawList();
				val.AddText(vector12, num25, text3);
			}
			if (Config.ESPName)
			{
				string text4;
				if (string.IsNullOrEmpty(value.Name))
				{
					text4 = (flag2 ? "TEAM" : "BOT");
				}
				else
				{
					text4 = (flag2 ? ("[TEAM] " + value.Name) : value.Name);
				}
				Vector2 vector13 = ImGui.CalcTextSize(text4);
				Vector2 vector14 = new Vector2(num16 + num15 / 2f - vector13.X / 2f, num17 - vector13.Y - 10f);
				uint num26;
				if (flag2)
				{
					num26 = num18;
				}
				else
				{
					num26 = ((Config.IgnoreKnocked && value.IsKnocked) ? ColorToUint32(Color.Red) : ColorToUint32(Config.ESPNameColor));
				}
				val = ImGui.GetForegroundDrawList();
				val.AddText(vector14, num26, text4);
			}
			if (Config.ESPHealth)
			{
				ImDrawListPtr foregroundDrawList3 = ImGui.GetForegroundDrawList();
				float num27;
				if (value.Health > 1000)
				{
					num27 = 1f;
				}
				else
				{
					num27 = ((value.Health < 0) ? 1f : ((float)value.Health / (float)((value.Health > 230) ? 500 : 200)));
				}
				float num28 = 4f;
				float num29 = num12 * num27;
				uint num30;
				if (num27 > 0.8f)
				{
					num30 = ColorToUint32(Color.GreenYellow);
				}
				else
				{
					num30 = ((!(num27 > 0.4f)) ? ColorToUint32(Color.Red) : ColorToUint32(Color.Orange));
				}
				Vector2 vector15 = new Vector2(num16 - num28 - 3f, num17);
				foregroundDrawList3.AddRectFilled(vector15, new Vector2(vector15.X + num28, num17 + num12), 2415919104u);
				num30 = (value.IsKnocked ? ColorToUint32(Color.Red) : num30);
				foregroundDrawList3.AddRectFilled(vector15, new Vector2(vector15.X + num28, num17 + num29), num30);
			}
			if (Config.ESPBox)
			{
				uint color;
				if (flag2)
				{
					color = num18;
				}
				else if (flag4)
				{
					color = num19;
				}
				else
				{
					color = ((Config.IgnoreKnocked && value.IsKnocked) ? ColorToUint32(Color.FromArgb(170, Color.Red)) : ColorToUint32(Color.FromArgb(170, Config.ESPBoxColor)));
				}
				DrawCorneredBox(num16, num17, num15, num12, color, flag4 ? 1.2f : 1.0f);
			}
			if (Config.ESPSkeleton)
			{
				uint skeletonColor;
				if (flag2)
				{
					skeletonColor = num18;
				}
				else if (flag4)
				{
					skeletonColor = num19;
				}
				else
				{
					skeletonColor = ((Config.IgnoreKnocked && value.IsKnocked) ? ColorToUint32(Color.FromArgb(170, Color.Red)) : ColorToUint32(Color.FromArgb(170, Config.ESPSkeletonColor)));
				}
				DrawSkeleton(value, skeletonColor, flag4 ? 1.2f : 1.0f);
			}
		}
		if (Config.minimap)
		{
			DrawMinimap();
		}
		if (Config.Esp360)
		{
			DrawDirectionalESP();
		}
		if (Config.EspLootEnabled)
		{
			DrawLootItems();
		}
		DisplayEnemyCount(Core.Width, Core.Height);
	}

	private void DrawLootItems()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		if (!Config.EspLootEnabled || !Core.HaveMatrix || Core.CameraMatrix == Matrix4x4.Identity)
		{
			return;
		}
		List<GroundLootEntry> groundLootCopy = EspItem.GetGroundLootCopy();
		if (groundLootCopy == null || groundLootCopy.Count == 0)
		{
			return;
		}
		ImDrawListPtr foregroundDrawList = ImGui.GetForegroundDrawList();
		foreach (GroundLootEntry item in groundLootCopy)
		{
			if (item.ItemId == 0)
			{
				continue;
			}
			int itemCategory = EspItem.GetItemCategory(item.ItemId);
			if ((Config.EspLootCategoryFilter.Count <= 0 || Config.EspLootCategoryFilter.Contains(itemCategory)) && (Config.EspLootItemFilter.Count <= 0 || Config.EspLootItemFilter.Contains(item.ItemId)))
			{
				Vector2 vector = W2S.WorldToScreen(Core.CameraMatrix, item.Position, Core.Width, Core.Height);
				if (!(vector.X < -40f) && !(vector.Y < -40f) && !(vector.X > (float)Core.Width + 40f) && !(vector.Y > (float)Core.Height + 40f))
				{
					uint col = ImGui.ColorConvertFloat4ToU32(EspItem.GetItemColor(item.ItemId));
					string text = (Config.EspLootShowDistance ? $"{item.Name} [{(int)item.Distance}m]" : item.Name);
					Vector2 vector2 = ImGui.CalcTextSize(text);
					Vector2 pos = new Vector2(vector.X - vector2.X * 0.5f, vector.Y - vector2.Y - 4f);
					Vector2 vector3 = new Vector2(pos.X - 7f, pos.Y + vector2.Y * 0.5f);
					foregroundDrawList.AddCircleFilled(vector3, 3f, uint.MaxValue, 10);
					DrawOutlinedText(foregroundDrawList, pos, col, text);
				}
			}
		}
	}

	private void DrawOutlinedText(ImDrawListPtr drawList, Vector2 pos, uint col, string text)
	{
		uint num = 3690987520u;
		drawList.AddText(pos + new Vector2(-1f, 0f), num, text);
		drawList.AddText(pos + new Vector2(1f, 0f), num, text);
		drawList.AddText(pos + new Vector2(0f, -1f), num, text);
		drawList.AddText(pos + new Vector2(0f, 1f), num, text);
		drawList.AddText(pos, col, text);
	}

	private void DrawDirectionalESP()
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		if (!Config.Esp360 || !Core.HaveMatrix || Core.CameraMatrix == Matrix4x4.Identity)
		{
			return;
		}
		ConcurrentDictionary<ulong, Entity> entities = Core.Entities;
		if (entities == null || entities.Count == 0)
		{
			return;
		}
		float num = (float)Core.Width / 2f;
		float num2 = (float)Core.Height / 2f;
		ImDrawListPtr foregroundDrawList = ImGui.GetForegroundDrawList();
		float x = 0f - GetCameraYaw();
		float num3 = MathF.Cos(x);
		float num4 = MathF.Sin(x);
		foreach (Entity value in entities.Values)
		{
			if (value.IsDead || !value.IsKnown || value.IsTeam == Bool3.True || (!Config.IsVisible && !value.IsVisible))
			{
				continue;
			}
			float num5 = Vector3.Distance(Core.LocalMainCamera, value.Head);
			if (num5 < 1f || num5 > 200f)
			{
				continue;
			}
			Vector3 vector = value.Head - Core.LocalMainCamera;
			float num6 = vector.X * num3 - vector.Z * num4;
			float num7 = vector.X * num4 + vector.Z * num3;
			float x2 = MathF.Atan2(num6, num7);
			if (num7 > 0f)
			{
				if (MathF.Abs(x2) < 0.7f)
				{
					continue;
				}
				Vector2 vector2 = W2S.WorldToScreen(Core.CameraMatrix, value.Head, Core.Width, Core.Height);
				if (vector2.X >= 50f && vector2.X <= (float)(Core.Width - 50) && vector2.Y >= 50f && vector2.Y <= (float)(Core.Height - 50))
				{
					continue;
				}
			}
			float num8 = MathF.Sqrt(num6 * num6 + num7 * num7);
			if (num8 < 0.001f)
			{
				continue;
			}
			float num9 = num6 / num8;
			float num10 = (0f - num7) / num8;
			Vector2 vector3 = new Vector2(num + num9 * 180f, num2 + num10 * 180f);
			float x3 = MathF.Atan2(num10, num9);
			float num11 = vector3.X - 10f;
			float num12 = vector3.Y - 10f;
			float num13 = 3.5f;
			float num14 = 20f;
			uint num15 = 4278255360u;
			if (value.Health > 0)
			{
				float num16;
				if (value.Health > 1000)
				{
					num16 = 1f;
				}
				else
				{
					num16 = ((value.Health < 0) ? 1f : ((float)value.Health / (float)((value.Health > 230) ? 500 : 200)));
				}
				if (num16 > 0.8f)
				{
					num15 = ColorToUint32(Color.GreenYellow);
				}
				else
				{
					num15 = ((!(num16 > 0.4f)) ? ColorToUint32(Color.Red) : ColorToUint32(Color.Orange));
				}
				if (value.IsKnocked)
				{
					num15 = ColorToUint32(Color.Red);
				}
			}
			foregroundDrawList.AddRectFilled(new Vector2(num11 - 0.5f, num12 - 0.5f), new Vector2(num11 + num13 + 0.5f, num12 + num14 + 0.5f), 4278190080u);
			foregroundDrawList.AddRectFilled(new Vector2(num11, num12), new Vector2(num11 + num13, num12 + num14), num15);
			Vector2 vector4 = new Vector2(vector3.X + 6f, vector3.Y);
			Vector2 vector5 = vector4 + new Vector2(MathF.Cos(x3), MathF.Sin(x3)) * 9.8f;
			Vector2 vector6 = vector4 - new Vector2(MathF.Cos(x3), MathF.Sin(x3)) * 4.2000003f;
			Vector2 vector7 = new Vector2(0f - MathF.Sin(x3), MathF.Cos(x3));
			Vector2 vector8 = vector6 + vector7 * 8f;
			Vector2 vector9 = vector6 - vector7 * 8f;
			foregroundDrawList.AddTriangleFilled(vector5, vector8, vector9, uint.MaxValue);
			foregroundDrawList.AddTriangle(vector5, vector8, vector9, 4278190080u, 1f);
			string text;
			if (value.IsBot)
			{
				text = "BOT";
			}
			else
			{
				text = (string.IsNullOrEmpty(value.Name) ? "BOT" : value.Name);
			}
			Vector2 vector10 = ImGui.CalcTextSize(text);
			Vector2 vector11 = new Vector2(vector3.X - vector10.X / 2f, vector3.Y - 12f - vector10.Y);
			foregroundDrawList.AddText(vector11 + new Vector2(1f, 1f), 4278190080u, text);
			foregroundDrawList.AddText(vector11, uint.MaxValue, text);
			string text2 = $"{num5:F1}m";
			Vector2 vector12 = ImGui.CalcTextSize(text2);
			Vector2 vector13 = new Vector2(vector3.X - vector12.X / 2f, vector3.Y + 12f);
			foregroundDrawList.AddText(vector13 + new Vector2(1f, 1f), 4278190080u, text2);
			foregroundDrawList.AddText(vector13, uint.MaxValue, text2);
		}
	}

	private void DrawMinimap()
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		float num = 100f;
		Vector2 vector = new Vector2(20f + num, (float)Core.Height - num - 20f);
		float num2 = 0f - GetCameraYaw();
		float num3 = MathF.Cos(num2);
		float num4 = MathF.Sin(num2);
		ImDrawListPtr backgroundDrawList = ImGui.GetBackgroundDrawList();
		backgroundDrawList.AddCircleFilled(vector, num, ColorToUint32(Color.FromArgb(150, 15, 15, 15)));
		backgroundDrawList.AddCircle(vector, num, ColorToUint32(Color.White), 64, 1f);
		uint num5 = ColorToUint32(Color.FromArgb(40, 255, 255, 255));
		for (int i = 1; i <= 3; i++)
		{
			backgroundDrawList.AddCircle(vector, num * (float)i / 3f, num5, 64, 0.5f);
		}
		string[] array = new string[4] { "N", "E", "S", "W" };
		for (int j = 0; j < 4; j++)
		{
			float x = -(float)Math.PI / 2f + (float)j * (float)Math.PI / 2f + num2;
			Vector2 vector2 = vector + new Vector2(MathF.Cos(x), MathF.Sin(x)) * (num - 12f);
			backgroundDrawList.AddText(vector2 - new Vector2(4f, 4f), ColorToUint32(Color.White), array[j]);
		}
		backgroundDrawList.AddCircleFilled(vector, 5f, ColorToUint32(Color.Cyan));
		Vector2 vector3 = vector + new Vector2(0f, -1f) * 11f;
		backgroundDrawList.AddLine(vector, vector3, ColorToUint32(Color.Cyan), 1.5f);
		int value = Core.Entities.Values.Count((Entity e) => !e.IsDead && e.IsTeam != Bool3.True && Vector3.Distance(Core.LocalMainCamera, e.Head) <= 250f);
		string text = $"Detected: {value}";
		ImGui.PushFont(FontManager.SmallFont);
		Vector2 vector4 = ImGui.CalcTextSize(text);
		Vector2 vector5 = new Vector2(vector.X - vector4.X / 2f, vector.Y - num - vector4.Y - 10f);
		uint num6 = ColorToUint32(Color.FromArgb(180, 0, 0, 0));
		Vector2[] array2 = new Vector2[4]
		{
			new Vector2(-1f, -1f),
			new Vector2(1f, -1f),
			new Vector2(-1f, 1f),
			new Vector2(1f, 1f)
		};
		foreach (Vector2 vector6 in array2)
		{
			backgroundDrawList.AddText(vector5 + vector6, num6, text);
		}
		backgroundDrawList.AddText(vector5, ColorToUint32(Color.White), text);
		ImGui.PopFont();
		foreach (Entity value2 in Core.Entities.Values)
		{
			if (!value2.IsDead && (value2.IsTeam != Bool3.True || Config.ESPTeam) && !(Vector3.Distance(Core.LocalMainCamera, value2.Head) > 250f))
			{
				Vector3 vector7 = value2.Head - Core.LocalMainCamera;
				float num8 = vector7.X * num3 - vector7.Z * num4;
				float num9 = vector7.X * num4 + vector7.Z * num3;
				float num10 = num / 250f;
				Vector2 vector8 = new Vector2(vector.X + num8 * num10, vector.Y - num9 * num10);
				if (Vector2.Distance(vector, vector8) <= num)
				{
					uint num11;
					if (value2.IsTeam != Bool3.True)
					{
						if (value2.IsKnown)
						{
							num11 = (value2.IsKnocked ? ColorToUint32(Color.Yellow) : ColorToUint32(Color.Red));
						}
						else
						{
							num11 = ColorToUint32(Color.Blue);
						}
					}
					else
					{
						num11 = ColorToUint32(Config.ESPTeamColor);
					}
					backgroundDrawList.AddCircleFilled(vector8, 4f, num11);
				}
			}
		}
	}

	private void DrawSkeleton(Entity entity, uint skeletonColor, float thickness = 1.2f)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		ImDrawListPtr drawList = ImGui.GetForegroundDrawList();
		float SW = Core.Width;
		float SH = Core.Height;
		if (entity.LeftShoulder == Vector3.Zero || entity.RightShoulder == Vector3.Zero)
		{
			return;
		}
		Vector3 vector = (entity.LeftShoulder + entity.RightShoulder) * 0.5f;
		Vector3 vector2 = Vector3.Zero;
		if (entity.LeftAnkle != Vector3.Zero && entity.RightAnkle != Vector3.Zero)
		{
			vector2 = (entity.LeftAnkle + entity.RightAnkle) * 0.5f;
		}
		else if (entity.LeftFoot != Vector3.Zero && entity.RightFoot != Vector3.Zero)
		{
			vector2 = (entity.LeftFoot + entity.RightFoot) * 0.5f;
		}
		else if (entity.Root != Vector3.Zero)
		{
			vector2 = entity.Root;
		}
		else if (entity.LeftAnkle != Vector3.Zero)
		{
			vector2 = entity.LeftAnkle;
		}
		else if (entity.RightAnkle != Vector3.Zero)
		{
			vector2 = entity.RightAnkle;
		}
		else if (entity.LeftFoot != Vector3.Zero)
		{
			vector2 = entity.LeftFoot;
		}
		else if (entity.RightFoot != Vector3.Zero)
		{
			vector2 = entity.RightFoot;
		}
		Vector3 vector3;
		if (vector2 != Vector3.Zero)
		{
			vector3 = Vector3.Lerp(vector, vector2, 0.48f);
		}
		else
		{
			vector3 = ((!(entity.Breast != Vector3.Zero)) ? (vector - new Vector3(0f, 0.65f, 0f)) : (vector + (entity.Breast - vector) * 2.1f));
		}
		Vector2 vector4 = W2SI(vector);
		Vector2 vector5 = W2SI(vector3);
		Vector2 a = W2SI(entity.LeftShoulder);
		Vector2 vector6 = W2SI(entity.RightShoulder);
		if (vector4.X < 0f || vector4.Y < 0f)
		{
			return;
		}
		DrawLine2D(a, vector4);
		DrawLine2D(vector4, vector6);
		if (vector5.X >= 0f && vector5.Y >= 0f)
		{
			DrawLine2D(vector4, vector5);
		}
		Vector3 vector7 = ((vector3 != Vector3.Zero && vector != Vector3.Zero) ? Vector3.Normalize(vector3 - vector) : new Vector3(0f, -1f, 0f));
		Vector3 vector8 = ((entity.LeftHand != Vector3.Zero) ? entity.LeftHand : entity.LeftWrist);
		if (vector8 != Vector3.Zero)
		{
			Vector3 vector9 = entity.LeftElbow;
			if (vector9 == Vector3.Zero)
			{
				Vector3 vector10 = Vector3.Normalize(entity.LeftShoulder - vector);
				float num = Vector3.Distance(entity.LeftShoulder, vector8);
				vector9 = (entity.LeftShoulder + vector8) * 0.5f + (vector10 * 0.28f + vector7 * 0.12f) * num;
			}
			Vector2 vector11 = W2SI(vector9);
			Vector2 b = W2SI(vector8);
			if (vector11.X >= 0f && vector11.Y >= 0f && b.X >= 0f && b.Y >= 0f)
			{
				DrawLine2D(a, vector11);
				DrawLine2D(vector11, b);
			}
			else if (b.X >= 0f && b.Y >= 0f)
			{
				DrawLine2D(a, b);
			}
		}
		Vector3 vector12 = ((entity.RightHand != Vector3.Zero) ? entity.RightHand : entity.RightWrist);
		if (vector12 != Vector3.Zero)
		{
			Vector3 vector13 = entity.RightElbow;
			if (vector13 == Vector3.Zero)
			{
				Vector3 vector14 = Vector3.Normalize(entity.RightShoulder - vector);
				float num2 = Vector3.Distance(entity.RightShoulder, vector12);
				vector13 = (entity.RightShoulder + vector12) * 0.5f + (vector14 * 0.28f + vector7 * 0.12f) * num2;
			}
			Vector2 vector15 = W2SI(vector13);
			Vector2 b2 = W2SI(vector12);
			if (vector15.X >= 0f && vector15.Y >= 0f && b2.X >= 0f && b2.Y >= 0f)
			{
				DrawLine2D(vector6, vector15);
				DrawLine2D(vector15, b2);
			}
			else if (b2.X >= 0f && b2.Y >= 0f)
			{
				DrawLine2D(vector6, b2);
			}
		}
		if (!(vector5.X >= 0f) || !(vector5.Y >= 0f))
		{
			return;
		}
		Vector2 vector16 = W2SI(entity.LeftAnkle);
		Vector2 b3 = W2SI(entity.LeftFoot);
		if (vector16.X >= 0f && vector16.Y >= 0f)
		{
			DrawLine2D(vector5, vector16);
			if (b3.X >= 0f && b3.Y >= 0f)
			{
				DrawLine2D(vector16, b3);
			}
		}
		else if (b3.X >= 0f && b3.Y >= 0f)
		{
			DrawLine2D(vector5, b3);
		}
		Vector2 vector17 = W2SI(entity.RightAnkle);
		Vector2 b4 = W2SI(entity.RightFoot);
		if (vector17.X >= 0f && vector17.Y >= 0f)
		{
			DrawLine2D(vector5, vector17);
			if (b4.X >= 0f && b4.Y >= 0f)
			{
				DrawLine2D(vector17, b4);
			}
		}
		else if (b4.X >= 0f && b4.Y >= 0f)
		{
			DrawLine2D(vector5, b4);
		}
		void DrawLine2D(Vector2 vector18, Vector2 vector19)
		{
			if (!(vector18.X < 0f) && !(vector18.Y < 0f) && !(vector19.X < 0f) && !(vector19.Y < 0f) && (!(vector18.X < -100f) || !(vector19.X < -100f)) && (!(vector18.X > SW + 100f) || !(vector19.X > SW + 100f)) && (!(vector18.Y < -100f) || !(vector19.Y < -100f)) && (!(vector18.Y > SH + 100f) || !(vector19.Y > SH + 100f)))
			{
				drawList.AddLine(vector18, vector19, skeletonColor, thickness);
			}
		}
		static Vector2 W2SI(Vector3 world)
		{
			if (world == Vector3.Zero)
			{
				return new Vector2(-1f, -1f);
			}
			return W2S.WorldToScreen(Core.CameraMatrix, world, Core.Width, Core.Height);
		}
	}

	private float GetCameraYaw()
	{
		return MathF.Atan2(Core.CameraMatrix.M31, Core.CameraMatrix.M33);
	}

	public void DrawCorneredBox(float X, float Y, float W, float H, uint color, float thickness)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		ImDrawListPtr foregroundDrawList = ImGui.GetForegroundDrawList();
		_ = W / 3f;
		_ = H / 3f;
		foregroundDrawList.AddLine(new Vector2(X, Y), new Vector2(X + W, Y), color, thickness);
		foregroundDrawList.AddLine(new Vector2(X, Y), new Vector2(X, Y + H), color, thickness);
		foregroundDrawList.AddLine(new Vector2(X + W, Y), new Vector2(X + W, Y + H), color, thickness);
		foregroundDrawList.AddLine(new Vector2(X, Y + H), new Vector2(X + W, Y + H), color, thickness);
	}

	private static uint ColorToUint32(Color color)
	{
		return ImGui.ColorConvertFloat4ToU32(new Vector4((float)(int)color.R / 255f, (float)(int)color.G / 255f, (float)(int)color.B / 255f, (float)(int)color.A / 255f));
	}

	private void CreateHandle()
	{
		hWnd = Core.Handle;
		if (Config.StreamMode)
		{
			SetWindowDisplayAffinity(hWnd, 17u);
		}
		else
		{
			SetWindowDisplayAffinity(hWnd, 0u);
		}
		if (WinAPI.GetWindowRect(Core.Handle, out var rect))
		{
			int left = rect.Left;
			int top = rect.Top;
			int num = rect.Right - rect.Left;
			int num2 = rect.Bottom - rect.Top;
			if (num > 0 && num2 > 0)
			{
				ImGui.SetWindowSize(new Vector2(num, num2));
				ImGui.SetWindowPos(new Vector2(left, top));
				((Overlay)this).Size = new Size(num, num2);
				((Overlay)this).Position = new Point(left, top);
				Core.Width = num;
				Core.Height = num2;
			}
		}
	}

	private void DisplayEnemyCount(float windowWidth, float windowHeight)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		float num = 220f;
		ImGuiIOPtr iO = ImGui.GetIO();
		iO.FontGlobalScale = num / ImGui.GetFontSize();
		iO = ImGui.GetIO();
		iO.FontGlobalScale = 1f;
	}
}
