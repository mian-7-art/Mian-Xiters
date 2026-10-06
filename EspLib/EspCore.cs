using System.Collections.Concurrent;
using System.Numerics;

namespace EspLib
{
    /// <summary>
    /// Shared state for the ESP — camera matrix, entities, screen dimensions.
    /// </summary>
    internal static class EspCore
    {
        internal static System.IntPtr Handle;
        internal static int Width  = -1;
        internal static int Height = -1;
        internal static bool HaveMatrix = false;
        internal static Matrix4x4 CameraMatrix;
        internal static ulong LocalPlayer;
        internal static Vector3 LocalMainCamera;
        internal static Vector3 PlayerPos;
        internal static int MatchStatus;
        internal static ConcurrentDictionary<ulong, EspEntity> Entities
            = new ConcurrentDictionary<ulong, EspEntity>();
    }
}
