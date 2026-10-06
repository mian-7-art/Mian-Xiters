using System.Collections.Concurrent;
using System.Numerics;

namespace EspLib
{
    internal class EspFrameSnapshot
    {
        public Matrix4x4 CameraMatrix;
        public Vector3 LocalMainCamera;
        public System.Collections.Generic.List<EspEntity> Entities;
    }

    /// <summary>
    /// Shared state for the ESP — camera matrix, entities, screen dimensions.
    /// Supports atomic double-buffered snapshots for 100% glitch-free 144Hz rendering.
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
        internal static bool IsInMatch = false;
        internal static int MatchStatus;
        internal static ConcurrentDictionary<ulong, EspEntity> Entities
            = new ConcurrentDictionary<ulong, EspEntity>();

        private static EspFrameSnapshot _latestSnapshot;

        internal static void SetSnapshot(Matrix4x4 matrix, Vector3 camPos, System.Collections.Generic.List<EspEntity> entities)
        {
            var snap = new EspFrameSnapshot
            {
                CameraMatrix = matrix,
                LocalMainCamera = camPos,
                Entities = entities
            };
            System.Threading.Volatile.Write(ref _latestSnapshot, snap);
        }

        internal static EspFrameSnapshot GetSnapshot()
        {
            return System.Threading.Volatile.Read(ref _latestSnapshot);
        }
    }
}
