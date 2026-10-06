using System.Collections.Concurrent;
using System.Numerics;

namespace AotForms;

internal static class Core
{
	internal static nint Handle;

	internal static int Width = -1;

	internal static int Height = -1;

	internal static bool HaveMatrix = false;

	internal static Matrix4x4 CameraMatrix;

	internal static ulong LocalPlayer;

	internal static Vector3 LocalMainCamera;

	internal static Vector3 playerpos;

	internal static int MatchStatus;

	internal static ConcurrentDictionary<ulong, Entity> Entities = new ConcurrentDictionary<ulong, Entity>();
}
