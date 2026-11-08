using System.Windows.Media;

namespace EspLib
{

internal static class EspConfig
{
	internal static bool EspMaster = false;

	internal static bool EspSkeleton = false;

	internal static bool EspBox = false;

	internal static bool EspCornerBox = false;

	internal static bool EspLine = false;

	internal static EspLinePos LinePosition = EspLinePos.Top;

	internal static bool EspWeapon = false;

	internal static bool Esp360 = false;

	internal static bool EspDistance = false;

	internal static bool EspName = false;

	internal static bool EspHealth = false;

	internal static bool EspTeam = false;

	internal static float MaxDistance = 250f;

	internal static Color EnemyColor = Color.FromRgb(byte.MaxValue, byte.MaxValue, byte.MaxValue);

	internal static bool StreamingMode = false;

	internal static bool AutoRefresh = false;
}
}
