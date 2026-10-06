using System.Numerics;

namespace EspLib
{
    internal static class EspW2S
    {
        /// <summary>
        /// Projects a 3D world position to 2D screen coordinates.
        /// Returns (-1, -1) if the point is behind the camera.
        /// </summary>
        internal static Vector2 WorldToScreen(Matrix4x4 viewMatrix, Vector3 pos, int width, int height)
        {
            Vector2 result = new Vector2(-1f, -1f);
            float cx = pos.X * viewMatrix.M11 + pos.Y * viewMatrix.M21 + pos.Z * viewMatrix.M31 + viewMatrix.M41;
            float cy = pos.X * viewMatrix.M12 + pos.Y * viewMatrix.M22 + pos.Z * viewMatrix.M32 + viewMatrix.M42;
            float cw = pos.X * viewMatrix.M14 + pos.Y * viewMatrix.M24 + pos.Z * viewMatrix.M34 + viewMatrix.M44;
            if (cw >= 0.001f)
            {
                float hw = width  / 2f;
                float hh = height / 2f;
                result.X = hw + hw * cx / cw;
                result.Y = hh - hh * cy / cw;
            }
            return result;
        }
    }
}
