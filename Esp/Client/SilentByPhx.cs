using System;
using System.Numerics;
using System.Threading;
using AotForms;

namespace Client;

internal static class SilentByPhx
{
	private static readonly Random rng = new Random();

	internal static void Work()
	{
		while (true)
		{
			if (!Config.Slient2)
			{
				Thread.Yield();
				continue;
			}
			if (Core.Width <= 0 || Core.Height <= 0 || !Core.HaveMatrix)
			{
				Thread.Yield();
				continue;
			}
			Entity entity = null;
			float num = float.MaxValue;
			Vector2 value = new Vector2((float)Core.Width / 2f, (float)Core.Height / 2f);
			foreach (Entity value7 in Core.Entities.Values)
			{
				if (value7 == null || value7.IsDead || value7.IsKnocked)
				{
					continue;
				}
				float num2 = Vector3.Distance(Core.LocalMainCamera, value7.Head);
				if (num2 > Config.AimBotMaxDistance || num2 < 0.1f)
				{
					continue;
				}
				Vector2 value2 = W2S.WorldToScreen(Core.CameraMatrix, value7.Head, Core.Width, Core.Height);
				if (float.IsFinite(value2.X) && float.IsFinite(value2.Y))
				{
					float num3 = Vector2.Distance(value, value2);
					if (!(num3 > Config.AimbotFOV) && num3 < num)
					{
						num = num3;
						entity = value7;
					}
				}
			}
			if (entity == null)
			{
				Thread.Yield();
				continue;
			}
			if (!Memory.Read<bool>(Core.LocalPlayer + Offsets.sAim1, out var value3) || !value3)
			{
				Thread.Yield();
				continue;
			}
			if (!Memory.Read<uint>(Core.LocalPlayer + Offsets.sAim2, out var value4) || value4 == 0)
			{
				Thread.Yield();
				continue;
			}
			if (!Memory.Read<Vector3>(value4 + Offsets.sAim3, out var value5))
			{
				Thread.Yield();
				continue;
			}
			value5.X += (float)(rng.NextDouble() * 2.0 - 1.0) * SilentConfig.MaxRandomOffset * 0.5f;
			value5.Y += (float)(rng.NextDouble() * 2.0 - 1.0) * SilentConfig.MaxRandomOffset;
			value5.Z += (float)(rng.NextDouble() * 2.0 - 1.0) * SilentConfig.MaxZOffset;
			Vector3 value6 = ChooseTargetBoneWithPrediction(entity, value5) - value5;
			Memory.Write(value4 + Offsets.sAim4, value6);
			Thread.Yield();
		}
	}

	private static Vector3 ChooseTargetBoneWithPrediction(Entity target, Vector3 muzzlePos)
	{
		Vector3 baseBonePosition = GetBaseBonePosition(target);
		Vector3 velocity = target.Velocity;
		float num = Vector3.Distance(muzzlePos, baseBonePosition) / SilentConfig.BulletSpeed;
		Vector3 vector = baseBonePosition + velocity * num * SilentConfig.PredictionMultiplier;
		float x = (float)(rng.NextDouble() * 2.0 - 1.0) * SilentConfig.MaxRandomOffset;
		float y = (float)(rng.NextDouble() * 2.0 - 1.0) * SilentConfig.MaxRandomOffset;
		float z = (float)(rng.NextDouble() * 2.0 - 1.0) * SilentConfig.MaxZOffset;
		return vector + new Vector3(x, y, z);
	}

	private static Vector3 GetBaseBonePosition(Entity target)
	{
		float num = (float)rng.NextDouble();
		if (num < SilentConfig.HeadRate)
		{
			return target.Head;
		}
		if (num < SilentConfig.HeadRate + (1f - SilentConfig.HeadRate) / 2f)
		{
			return target.Spine;
		}
		return target.Hip;
	}
}
