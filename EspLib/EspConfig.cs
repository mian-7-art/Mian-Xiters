namespace EspLib
{
    public enum EspLinePos
    {
        Bottom,
        Top,
        Center
    }

    /// <summary>
    /// ESP configuration flags — controlled by UI toggles.
    /// Default all sub-options to false until user turns them ON.
    /// </summary>
    internal static class EspConfig
    {
        /// <summary>Master ESP on/off switch (set by Enable ESP toggle).</summary>
        internal static bool EspMaster = false;

        /// <summary>Draw 3D skeleton bone structure.</summary>
        internal static bool EspSkeleton = false;

        /// <summary>Draw standard 2D bounding box.</summary>
        internal static bool EspBox = false;

        /// <summary>Draw cornered L-bracket box instead of full rectangle.</summary>
        internal static bool EspCornerBox = false;

        /// <summary>Draw snap-lines to enemy target.</summary>
        internal static bool EspLine = false;

        /// <summary>Origin position for snap lines (Bottom, Top, Center).</summary>
        internal static EspLinePos LinePosition = EspLinePos.Bottom;

        /// <summary>Show enemy held weapon name.</summary>
        internal static bool EspWeapon = false;

        /// <summary>Show 360-degree offscreen enemy radar indicators.</summary>
        internal static bool Esp360 = false;

        /// <summary>Show distance label under enemy box.</summary>
        internal static bool EspDistance = false;

        /// <summary>Show enemy name above box.</summary>
        internal static bool EspName = false;

        /// <summary>Show health bar beside box.</summary>
        internal static bool EspHealth = false;

        /// <summary>Draw teammates as well (green colour).</summary>
        internal static bool EspTeam = false;

        /// <summary>Maximum distance (game units) to render enemies.</summary>
        internal static float MaxDistance = 250f;

        /// <summary>Custom color for enemy overlay elements (Default: Red).</summary>
        internal static System.Windows.Media.Color EnemyColor = System.Windows.Media.Color.FromRgb(255, 0, 0);

        /// <summary>Hide windows from screen captures/Discord stream (WDA_EXCLUDEFROMCAPTURE).</summary>
        internal static bool StreamingMode = false;
    }
}
