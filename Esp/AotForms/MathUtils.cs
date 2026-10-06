using System;
using System.Numerics;

namespace AotForms;

internal static class MathUtils
{
	private const float SMALL_float = 1E-10f;

	internal static Quaternion GetRotationToLocation(Vector3 targetLocation, float y_bias, Vector3 myLoc)
	{
		Vector3 forwards = targetLocation + new Vector3(0f, y_bias, 0f) - myLoc;
		Vector3 upwards = new Vector3(0f, 1f, 0f);
		return LookRotation(forwards, upwards);
	}

	internal static float QuaternionToAngle(Quaternion quaternion)
	{
		return QuaternionToEulerAngles(quaternion).Y;
	}

	private static Vector3 QuaternionToEulerAngles(Quaternion q)
	{
		float x = MathF.Asin(2f * (q.Y * q.Z + q.W * q.X));
		float y = MathF.Atan2(2f * (q.W * q.Y - q.X * q.Z), 1f - 2f * (q.Y * q.Y + q.Z * q.Z));
		float z = MathF.Atan2(2f * (q.X * q.Y + q.W * q.Z), 1f - 2f * (q.Y * q.Y + q.Z * q.Z));
		return new Vector3(x, y, z);
	}

	private static Quaternion LookRotation(Vector3 forwards, Vector3 upwards)
	{
		forwards = Normalized(forwards);
		upwards = Normalized(upwards);
		if (SqrMagnitude(forwards) < 1E-10f || SqrMagnitude(upwards) < 1E-10f)
		{
			return Quaternion.Identity;
		}
		if (1f - Math.Abs(Vector3.Dot(forwards, upwards)) < 1E-10f)
		{
			return FromToRotation(forwards, upwards);
		}
		Vector3 vector = Normalized(Vector3.Cross(upwards, forwards));
		upwards = Vector3.Cross(forwards, vector);
		float num = vector.X + upwards.Y + forwards.Z;
		Quaternion result = default;
		if (num > 0f)
		{
			result.W = MathF.Sqrt(1f + num) * 0.5f;
			float num2 = 1f / (4f * result.W);
			result.X = (upwards.Z - forwards.Y) * num2;
			result.Y = (forwards.X - vector.Z) * num2;
			result.Z = (vector.Y - upwards.X) * num2;
		}
		else if (vector.X >= upwards.Y && vector.X >= forwards.Z)
		{
			result.X = MathF.Sqrt(1f + vector.X - upwards.Y - forwards.Z) * 0.5f;
			float num3 = 1f / (4f * result.X);
			result.W = (upwards.Z - forwards.Y) * num3;
			result.Z = (forwards.X + vector.Z) * num3;
			result.Y = (vector.Y + upwards.X) * num3;
		}
		else if (upwards.Y > forwards.Z)
		{
			result.Y = MathF.Sqrt(1f - vector.X + upwards.Y - forwards.Z) * 0.5f;
			float num4 = 1f / (4f * result.Y);
			result.Z = (upwards.Z + forwards.Y) * num4;
			result.W = (forwards.X - vector.Z) * num4;
			result.X = (vector.Y + upwards.X) * num4;
		}
		else
		{
			result.Z = MathF.Sqrt(1f - vector.X - upwards.Y + forwards.Z) * 0.5f;
			float num5 = 1f / (4f * result.Z);
			result.Y = (upwards.Z + forwards.Y) * num5;
			result.X = (forwards.X + vector.Z) * num5;
			result.W = (vector.Y - upwards.X) * num5;
		}
		return result;
	}

	private static Quaternion FromToRotation(Vector3 forwards, Vector3 upwards)
	{
		float num = Vector3.Dot(forwards, upwards);
		float num2 = MathF.Sqrt(SqrMagnitude(forwards) * SqrMagnitude(upwards));
		if ((double)Math.Abs(num / num2 + 1f) < 1E-05)
		{
			return new Quaternion(Normalized(Orthogonal(forwards)), 0f);
		}
		return Normalized(new Quaternion(Vector3.Cross(forwards, upwards), num + num2));
	}

	private static Quaternion Normalized(Quaternion rotation)
	{
		float num = Norm(rotation);
		return new Quaternion(rotation.X / num, rotation.Y / num, rotation.Z / num, rotation.W / num);
	}

	private static float Norm(Quaternion rotation)
	{
		return MathF.Sqrt(rotation.X * rotation.X + rotation.Y * rotation.Y + rotation.Z * rotation.Z + rotation.W * rotation.W);
	}

	private static Vector3 Orthogonal(Vector3 v)
	{
		if (!(v.Z < v.X))
		{
			return new Vector3(0f, 0f - v.Z, v.Y);
		}
		return new Vector3(v.Y, 0f - v.X, 0f);
	}

	private static Vector3 Normalized(Vector3 vector)
	{
		float num = Magnitude(vector);
		if (num == 0f)
		{
			return Vector3.Zero;
		}
		return vector / num;
	}

	private static float Magnitude(Vector3 vector)
	{
		return MathF.Sqrt(SqrMagnitude(vector));
	}

	private static float SqrMagnitude(Vector3 v)
	{
		return v.X * v.X + v.Y * v.Y + v.Z * v.Z;
	}
}
