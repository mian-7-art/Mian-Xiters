using System.Numerics;

namespace EspLib
{

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
		if (!EspMemory.Read<uint>(nodeTransform + 8, out var value))
		{
			return false;
		}
		return GetPosition(value, out result);
	}

	internal static bool GetPosition(uint transform, out Vector3 pos)
	{
		pos = Vector3.Zero;
		if (!EspMemory.Read<uint>(transform + 8, out var value))
		{
			return false;
		}
		if (!EspMemory.Read<uint>(value + 36, out var value2))
		{
			return false;
		}
		if (!EspMemory.Read<uint>(value + 32, out var value3))
		{
			return false;
		}
		if (!EspMemory.Read<uint>(value3 + 24, out var value4))
		{
			return false;
		}
		if (!EspMemory.Read<uint>(value3 + 28, out var value5))
		{
			return false;
		}
		if (!EspMemory.Read<Vector3>(value2 * 48 + value4, out var value6))
		{
			return false;
		}
		int num = 0;
		if (!EspMemory.Read<int>(value2 * 4 + value5, out var value7))
		{
			return false;
		}
		while (value7 >= 0)
		{
			num++;
			if (num == 50)
			{
				break;
			}
			if (!EspMemory.Read<TMatrix>((uint)(48 * value7 + value4), out var value8))
			{
				return false;
			}
			float x = value8.rotation.X;
			float y = value8.rotation.Y;
			float z = value8.rotation.Z;
			float w = value8.rotation.W;
			float num2 = value6.X * value8.scale.X;
			float num3 = value6.Y * value8.scale.Y;
			float num4 = value6.Z * value8.scale.Z;
			value6.X = (float)((double)(value8.position.X + num2) + (double)num2 * (-2.0 * (double)(y * y) - 2.0 * (double)(z * z)) + (double)num3 * (-2.0 * (double)(w * z) + 2.0 * (double)(y * x)) + (double)num4 * (2.0 * (double)(z * x) + 2.0 * (double)(w * y)));
			value6.Y = (float)((double)(value8.position.Y + num3) + (double)num2 * (2.0 * (double)(x * y) + 2.0 * (double)(w * z)) + (double)num3 * (-2.0 * (double)(z * z) - 2.0 * (double)(x * x)) + (double)num4 * (-2.0 * (double)(w * x) + 2.0 * (double)(z * y)));
			value6.Z = (float)((double)(value8.position.Z + num4) + (double)num2 * (-2.0 * (double)(w * y) + 2.0 * (double)(x * z)) + (double)num3 * (2.0 * (double)(y * z) + 2.0 * (double)(w * x)) + (double)num4 * (-2.0 * (double)(x * x) - 2.0 * (double)(y * y)));
			if (!EspMemory.Read<int>((uint)(value7 * 4 + value5), out value7))
			{
				return false;
			}
		}
		pos = value6;
		return num != 50;
	}
}
}
