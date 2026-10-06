using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AotForms;

internal static class Config
{
	public static float TeleOffsetX = 0.5f;

	public static float TeleOffsetY = 1.5f;

	public static float TeleOffsetZ = 0f;

	public static bool KeyAlreadyPressed = false;

	public static bool KeyAlreadyPressed3 = false;

	public static bool KeyAlreadyPressed4 = false;

	public static bool KeyAlreadyPressed6 = false;

	internal static bool DrawFov = false;

	public static float AimBotMaxDistance = 150f;

	public static bool IgnoreKnocked = false;

	public static float AimbotFOV = 90f;

	internal static bool FastReload = false;

	internal static bool AimBot = false;

	internal static bool AimbotAi = false;

	internal static bool AimLegit = false;

	internal static bool AimPlus = false;

	public static float AimbotLegitDelayMs = 150f;

	public static int AimbotLegitThreadMs = 5;

	internal static bool IgnoreBot = false;

	public static float AimbotDelayMs = 0.3f;

	internal static bool StreamMode = false;

	internal static Keys Silent2 = Keys.LButton;

	internal static bool WaitingForKeybindSp = false;

	public static Keys SpKey = Keys.None;

	public static string SpKeyLabel = "None";

	public static bool SilentAimEnabled = false;

	internal static bool WaitingForKeybindTelePort = false;

	public static Keys TelePortKey = Keys.None;

	public static string TelePortKeyLabel = "None";

	internal static bool WaitingForKeybindUpPlayer = false;

	public static Keys UpPlayerKey = Keys.None;

	public static string UpPlayerKeyLabel = "None";

	internal static bool WaitingForKeybindTeleKill = false;

	public static Keys TeleKillKey = Keys.None;

	public static string TeleKillKeyLabel = "None";

	public static float downSpeed = 0.8f;

	internal static bool proxtelekill = false;

	internal static bool UpPlayer = false;

	internal static bool Slient = false;

	internal static bool Slient2 = false;

	internal static bool minimap = false;

	internal static bool telekill = false;

	internal static bool NoRecoil = false;

	internal static bool NoCache = false;

	internal static Color FovColor = Color.White;

	internal static bool EspMaster = false;

	internal static bool ESPLine = false;

	internal static Color ESPLineColor = Color.White;

	internal static bool ESPBox = false;

	internal static Color ESPBoxColor = Color.White;

	internal static bool ESPName = false;

	internal static Color ESPNameColor = Color.White;

	internal static bool ESPHealth = false;

	internal static bool ESPLineDuoi = false;

	internal static Color ESPLineDuoiColor = Color.Red;

	internal static bool ESPSkeleton = false;

	internal static Color ESPSkeletonColor = Color.FromArgb(171, 196, 253);

	internal static bool IsVisible = false;

	internal static bool ESPDistance = false;

	internal static bool down = false;

	internal static bool speed = false;

	internal static float test = 100f;

	internal static bool ESPWeapon = false;

	internal static bool Esp360 = false;

	internal static bool ESPTeam = false;

	internal static Color ESPTeamColor = Color.FromArgb(0, 255, 128);

	internal static bool AimbotVisible1 = false;

	public static float AimMouseSensi = 1f;

	public static float AimMouseSmooth = 5f;

	public static bool AimMouseOnLButton = true;

	internal static bool EspLootEnabled = false;

	internal static bool EspLootShowDistance = true;

	internal static float EspLootMaxDistance = 150f;

	internal static HashSet<int> EspLootCategoryFilter = new HashSet<int>();

	internal static HashSet<uint> EspLootItemFilter = new HashSet<uint>();

	internal static bool SkinChanger = false;

	internal static Dictionary<uint, uint> ActiveSkins = new Dictionary<uint, uint>();

	internal static bool AimbotLegitEnabled
	{
		get
		{
			return AimLegit;
		}
		set
		{
			AimLegit = value;
		}
	}

	public static float aimfov
	{
		get
		{
			return AimbotFOV;
		}
		set
		{
			AimbotFOV = value;
		}
	}
}
