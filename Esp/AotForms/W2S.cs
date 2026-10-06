using System.Numerics;

namespace AotForms;

internal static class W2S
{
	internal static Vector2 WorldToScreen(Matrix4x4 viewMatrix, Vector3 pos, int width, int height)
	{
		Vector2 result = new Vector2(-1f, -1f);
		float num = pos.X * viewMatrix.M11 + pos.Y * viewMatrix.M21 + pos.Z * viewMatrix.M31 + viewMatrix.M41;
		float num2 = pos.X * viewMatrix.M12 + pos.Y * viewMatrix.M22 + pos.Z * viewMatrix.M32 + viewMatrix.M42;
		float num3 = pos.X * viewMatrix.M14 + pos.Y * viewMatrix.M24 + pos.Z * viewMatrix.M34 + viewMatrix.M44;
		if ((double)num3 >= 0.001)
		{
			float num4 = (float)width / 2f;
			float num5 = (float)height / 2f;
			result.X = num4 + num4 * num / num3;
			result.Y = num5 - num5 * num2 / num3;
		}
		return result;
	}
}
