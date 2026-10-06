using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace AotForms;

internal static class Aimmouse
{
	private struct INPUT
	{
		public uint type;

		public InputUnion U;
	}

	[StructLayout(LayoutKind.Explicit)]
	private struct InputUnion
	{
		[FieldOffset(0)]
		public MOUSEINPUT mi;
	}

	private struct MOUSEINPUT
	{
		public int dx;

		public int dy;

		public uint mouseData;

		public uint dwFlags;

		public uint time;

		public nint dwExtraInfo;
	}

	private static Entity _currentTarget;

	private static int _lastMoveX;

	private static int _lastMoveY;

	private const int MOUSEEVENTF_MOVE = 1;

	internal static Entity CurrentTarget => _currentTarget;

	private static Vector2 _screenCenter => new Vector2((float)Core.Width / 2f, (float)Core.Height / 2f);

	internal static void Work()
	{
		while (true)
		{
			if (Core.Width <= 0 || Core.Height <= 0 || !Core.HaveMatrix || !Config.AimbotVisible1 || !Config.DrawFov)
			{
				_currentTarget = null;
				_lastMoveX = 0;
				_lastMoveY = 0;
				Thread.Sleep(10);
				continue;
			}
			FindBestTarget();
			bool flag = (GetAsyncKeyState(Keys.LButton) & 0x8000) != 0;
			if ((!Config.AimMouseOnLButton | flag) && _currentTarget != null)
			{
				AimMouseAtTarget(_currentTarget);
			}
			else
			{
				_lastMoveX = 0;
				_lastMoveY = 0;
			}
			Thread.Sleep(15);
		}
	}

	private static Vector3 GetRealtimeHeadPos(Entity entity)
	{
		if (entity == null || entity.Address == 0L)
		{
			return Vector3.Zero;
		}
		if (Memory.Read<uint>(entity.Address + 1180, out var value) && value != 0 && Transform.GetNodePosition(value, out var result) && result != Vector3.Zero)
		{
			entity.Head = result;
			return result;
		}
		return entity.Head;
	}

	private static void FindBestTarget()
	{
		float aimbotFOV = Config.AimbotFOV;
		float num = aimbotFOV * aimbotFOV;
		if (_currentTarget != null)
		{
			if (Core.Entities.TryGetValue(_currentTarget.Address, out var value) && IsValidTarget(value))
			{
				Vector3 realtimeHeadPos = GetRealtimeHeadPos(value);
				Vector2 vector = W2S.WorldToScreen(Core.CameraMatrix, realtimeHeadPos, Core.Width, Core.Height);
				if (vector.X >= 1f && vector.Y >= 1f && (vector - _screenCenter).LengthSquared() <= num)
				{
					_currentTarget = value;
					return;
				}
			}
			_currentTarget = null;
		}
		Entity currentTarget = null;
		float num2 = float.MinValue;
		foreach (KeyValuePair<ulong, Entity> entity in Core.Entities)
		{
			Entity value2 = entity.Value;
			if (!IsValidTarget(value2))
			{
				continue;
			}
			Vector3 realtimeHeadPos2 = GetRealtimeHeadPos(value2);
			Vector2 vector2 = W2S.WorldToScreen(Core.CameraMatrix, realtimeHeadPos2, Core.Width, Core.Height);
			if (vector2.X < 1f || vector2.Y < 1f)
			{
				continue;
			}
			float num3 = (vector2 - _screenCenter).Length();
			if (!(num3 > aimbotFOV))
			{
				float num4 = CalculateTargetScore(value2, num3, aimbotFOV);
				if (num4 > num2)
				{
					num2 = num4;
					currentTarget = value2;
				}
			}
		}
		_currentTarget = currentTarget;
	}

	private static float CalculateTargetScore(Entity entity, float distCrosshair, float fov)
	{
		float num = 0f;
		num += (fov - distCrosshair) * 10f;
		float num2 = Vector3.Distance(Core.LocalMainCamera, entity.Head);
		num += Math.Max(0f, Config.AimBotMaxDistance - num2) * 1.5f;
		if (entity.Health > 0 && entity.Health <= 200)
		{
			num += (float)(200 - entity.Health) * 2.5f;
		}
		return num;
	}

	private static bool IsValidTarget(Entity entity)
	{
		if (entity != null && entity.Address != 0L && !entity.IsDead && (!Config.IgnoreKnocked || !entity.IsKnocked) && (!Config.IgnoreBot || !entity.IsBot))
		{
			if (!Config.IsVisible)
			{
				return entity.IsVisible;
			}
			return true;
		}
		return false;
	}

	private static void AimMouseAtTarget(Entity target)
	{
		Vector3 realtimeHeadPos = GetRealtimeHeadPos(target);
		Vector2 vector = W2S.WorldToScreen(Core.CameraMatrix, realtimeHeadPos, Core.Width, Core.Height);
		if (vector.X < 1f || vector.Y < 1f)
		{
			return;
		}
		Vector2 vector2 = vector - _screenCenter;
		float aimbotFOV = Config.AimbotFOV;
		float num = vector2.Length();
		if (num > aimbotFOV)
		{
			return;
		}
		if (num <= 12f)
		{
			_lastMoveX = 0;
			_lastMoveY = 0;
			return;
		}
		float num2 = Math.Clamp(Config.AimMouseSmooth, 1.5f, 25f);
		float num3 = 6f / num2;
		float value = vector2.X / 14f * num3;
		float value2 = vector2.Y / 14f * num3;
		float num4;
		if (num > 80f)
		{
			num4 = 4f;
		}
		else
		{
			num4 = ((num > 35f) ? 2f : 1f);
		}
		value = Math.Clamp(value, 0f - num4, num4);
		float num5 = Math.Clamp(value2, 0f - num4, num4);
		int num6 = (int)Math.Round(value);
		int num7 = (int)Math.Round(num5);
		if (Math.Abs(vector2.X) < 22f && Math.Abs(num6) > 1)
		{
			num6 = Math.Sign(num6);
		}
		if (Math.Abs(vector2.Y) < 22f && Math.Abs(num7) > 1)
		{
			num7 = Math.Sign(num7);
		}
		if (_lastMoveX != 0 && Math.Sign(num6) != Math.Sign(_lastMoveX))
		{
			num6 = 0;
		}
		if (_lastMoveY != 0 && Math.Sign(num7) != Math.Sign(_lastMoveY))
		{
			num7 = 0;
		}
		_lastMoveX = num6;
		_lastMoveY = num7;
		if (num6 != 0 || num7 != 0)
		{
			MoveMouseRelative(num6, num7);
		}
	}

	private static void MoveMouseRelative(int dx, int dy)
	{
		INPUT[] array = new INPUT[1];
		array[0].type = 0u;
		array[0].U.mi = new MOUSEINPUT
		{
			dx = dx,
			dy = dy,
			mouseData = 0u,
			dwFlags = 1u,
			time = 0u,
			dwExtraInfo = IntPtr.Zero
		};
		SendInput(1u, array, Marshal.SizeOf(typeof(INPUT)));
	}

	internal static void StartHotkeyThread()
	{
		Thread thread = new Thread(MonitorHotkeys);
		thread.IsBackground = true;
		thread.Start();
	}

	private static void MonitorHotkeys()
	{
		while (true)
		{
			if ((GetAsyncKeyState(Keys.F9) & 0x8000) != 0)
			{
				Config.AimbotVisible1 = !Config.AimbotVisible1;
				Console.WriteLine($"[Hotkey] AimbotVisible: {Config.AimbotVisible1}");
				Thread.Sleep(300);
			}
			Thread.Sleep(50);
		}
	}

	[DllImport("user32.dll")]
	private static extern short GetAsyncKeyState(Keys vKey);

	[DllImport("user32.dll")]
	private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);
}
