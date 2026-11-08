using System.Numerics;

namespace EspLib
{
    internal static class EspW2S
    {
        /// <summary>
        /// Projects a 3D world position to 2D screen coordinates with support for
        /// letterboxed / aspect-ratio corrected viewports (e.g. half-screen or non-16:9 windows).
        /// Returns (-9999, -9999) if the point is behind the camera.
        /// </summary>
        internal static Vector2 WorldToScreen(Matrix4x4 viewMatrix, Vector3 pos, float renderW, float renderH, float offsetX, float offsetY)
        {
            Vector2 result = new Vector2(-9999f, -9999f);
            float cx = pos.X * viewMatrix.M11 + pos.Y * viewMatrix.M21 + pos.Z * viewMatrix.M31 + viewMatrix.M41;
            float cy = pos.X * viewMatrix.M12 + pos.Y * viewMatrix.M22 + pos.Z * viewMatrix.M32 + viewMatrix.M42;
            float cw = pos.X * viewMatrix.M14 + pos.Y * viewMatrix.M24 + pos.Z * viewMatrix.M34 + viewMatrix.M44;
            if (cw >= 0.001f)
            {
                float hw = renderW / 2f;
                float hh = renderH / 2f;
                result.X = offsetX + hw + hw * (cx / cw);
                result.Y = offsetY + hh - hh * (cy / cw);
            }
            return result;
        }

        internal static Vector2 WorldToScreen(Matrix4x4 viewMatrix, Vector3 pos, int width, int height)
        {
            return WorldToScreen(viewMatrix, pos, (float)width, (float)height, 0f, 0f);
        }
    }
}
