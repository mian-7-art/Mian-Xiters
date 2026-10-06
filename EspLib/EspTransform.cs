using System.Numerics;

namespace EspLib
{
    /// <summary>
    /// IL2CPP Transform matrix walker — resolves world position from Unity node transforms.
    /// Ported from AotForms.Transform.
    /// </summary>
    internal static class EspTransform
    {
        internal struct TMatrix
        {
            internal Vector4 position;
            internal Quaternion rotation;
            internal Vector4 scale;
        }

        internal static bool GetNodePosition(ulong nodeTransform, out Vector3 result)
        {
            result = Vector3.Zero;
            if (!EspMemory.Read<uint>(nodeTransform + 8, out var value)) return false;
            return GetPosition(value, out result);
        }

        internal static bool GetPosition(uint transform, out Vector3 pos)
        {
            pos = Vector3.Zero;
            if (!EspMemory.Read<uint>(transform + 8, out var v1)) return false;
            if (!EspMemory.Read<uint>(v1 + 36, out var v2)) return false;
            if (!EspMemory.Read<uint>(v1 + 32, out var v3)) return false;
            if (!EspMemory.Read<uint>(v3 + 24, out var v4)) return false;
            if (!EspMemory.Read<uint>(v3 + 28, out var v5)) return false;
            if (!EspMemory.Read<Vector3>(v2 * 48 + v4, out var local)) return false;

            const int MaxIter = 50;
            int iter = 0;
            if (!EspMemory.Read<int>(v2 * 4 + v5, out var parentIdx)) return false;
            while (parentIdx >= 0)
            {
                iter++;
                if (iter == MaxIter) break;
                if (!EspMemory.Read<TMatrix>((uint)(48 * parentIdx + v4), out var m)) return false;

                float x = m.rotation.X, y = m.rotation.Y, z = m.rotation.Z, w = m.rotation.W;
                float nx = local.X * m.scale.X;
                float ny = local.Y * m.scale.Y;
                float nz = local.Z * m.scale.Z;
                local.X = (float)(m.position.X + nx + nx * (-2d * (y * y) - 2d * (z * z))
                                 + ny * (-2d * (w * z) + 2d * (y * x))
                                 + nz * (2d * (z * x) + 2d * (w * y)));
                local.Y = (float)(m.position.Y + ny + nx * (2d * (x * y) + 2d * (w * z))
                                 + ny * (-2d * (z * z) - 2d * (x * x))
                                 + nz * (-2d * (w * x) + 2d * (z * y)));
                local.Z = (float)(m.position.Z + nz + nx * (-2d * (w * y) + 2d * (x * z))
                                 + ny * (2d * (y * z) + 2d * (w * x))
                                 + nz * (-2d * (x * x) - 2d * (y * y)));

                if (!EspMemory.Read<int>((uint)(parentIdx * 4 + v5), out parentIdx)) return false;
            }
            pos = local;
            return iter != MaxIter;
        }
    }
}
