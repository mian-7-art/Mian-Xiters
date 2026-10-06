using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Windows.Forms;

namespace AotForms;

internal static class AimbotLegit
{
	private static Thread _aimbotThread;

	private static volatile bool _isRunning = false;

	private static CancellationTokenSource _cts;

	private static readonly object _lockObj = new object();

	private static long _clickStartTick = 0L;

	private static volatile bool _alreadyReplaced = false;

	private static Entity _currentTarget = null;

	internal static float DelayProgress { get; private set; } = 0f;

	internal static bool IsWaitingDelay { get; private set; } = false;

	public static Entity CurrentTarget => _currentTarget;

	internal static void Start()
	{
		Work();
	}

	internal static void Stop()
	{
		lock (_lockObj)
		{
			if (!_isRunning)
			{
				return;
			}
			_isRunning = false;
			try
			{
				_cts?.Cancel();
			}
			catch
			{
			}
			try
			{
				if (_aimbotThread != null && _aimbotThread.IsAlive)
				{
					_aimbotThread.Join(100);
				}
			}
			catch
			{
			}
			_aimbotThread = null;
			ResetClickState();
		}
	}

	internal static void Work()
	{
		lock (_lockObj)
		{
			if (!_isRunning)
			{
				_isRunning = true;
				_cts = new CancellationTokenSource();
				_aimbotThread = new Thread(WorkLoop)
				{
					IsBackground = true,
					Priority = ThreadPriority.Highest,
					Name = "AimbotLegitThread"
				};
				_aimbotThread.Start();
			}
		}
	}

	private static void WorkLoop()
	{
		CancellationToken cancellationToken;
		try
		{
			cancellationToken = ((_cts != null) ? _cts.Token : CancellationToken.None);
		}
		catch
		{
			cancellationToken = CancellationToken.None;
		}
		while (_isRunning && !cancellationToken.IsCancellationRequested)
		{
			try
			{
				LegitAimbot();
			}
			catch (Exception ex)
			{
				try
				{
					Console.WriteLine("[LỖI] AimLegit: " + ex.Message);
				}
				catch
				{
				}
				ResetClickState();
			}
			int num = ((Config.AimbotLegitThreadMs > 0) ? Config.AimbotLegitThreadMs : 5);
			if (num < 1)
			{
				num = 1;
			}
			if (num > 50)
			{
				num = 50;
			}
			Thread.Sleep(num);
		}
	}

	internal static void LegitAimbot()
	{
		try
		{
			if ((!Config.AimbotAi && !Config.AimBot && !Config.AimLegit && !Config.AimbotLegitEnabled && !Config.AimPlus) || !Core.HaveMatrix || Core.LocalPlayer == 0L)
			{
				ResetClickState();
				return;
			}
			if (!Memory.IsValid)
			{
				ResetClickState();
				return;
			}
			if (Core.Width < 8 || Core.Height < 8)
			{
				ResetClickState();
				return;
			}
			Entity entity = null;
			try
			{
				entity = FindClosestEnemy();
			}
			catch
			{
				entity = null;
			}
			_currentTarget = entity;
			if (entity == null || entity.Address == 0L || entity.IsTeam == Bool3.True || entity.IsDead || entity.IsKnocked)
			{
				return;
			}
			bool flag;
			try
			{
				flag = (WinAPI.GetAsyncKeyState(Keys.LButton) & 0x8000) != 0;
			}
			catch
			{
				return;
			}
			if (!flag)
			{
				ResetClickState();
			}
			else
			{
				if (_alreadyReplaced)
				{
					return;
				}
				long tickCount = Environment.TickCount64;
				if (_clickStartTick == 0L)
				{
					_clickStartTick = tickCount;
					UpdateDelayBar(0f, waiting: true);
					return;
				}
				float delayToHeadSeconds = GetDelayToHeadSeconds();
				double num = (double)(tickCount - _clickStartTick) / 1000.0;
				if (!(delayToHeadSeconds > 0f) || !(num < (double)delayToHeadSeconds))
				{
					UpdateDelayBar(1f, waiting: false);
					uint value = 0u;
					try
					{
						if (!Memory.Read<uint>(entity.Address + Offsets.HeadCoilder, out value) || value == 0)
						{
							return;
						}
					}
					catch
					{
						return;
					}
					_alreadyReplaced = true;

					try
					{
						Thread.Sleep(25);
					}
					catch
					{
					}
					try
					{
						if (Core.Entities.TryGetValue(entity.Address, out var value2) && value2 != null && !value2.IsDead && value2.Address != 0L)
						{
							Memory.Write(entity.Address + Offsets.MainCollider, value);
						}
						return;
					}
					catch
					{
						return;
					}
				}
				UpdateDelayBar((float)(num / (double)delayToHeadSeconds), waiting: true);
			}
		}
		catch
		{
		}
	}

	private static Entity FindClosestEnemy()
	{
		float num = float.MaxValue;
		Entity result = null;
		ConcurrentDictionary<ulong, Entity> entities = Core.Entities;
		if (entities == null || entities.IsEmpty)
		{
			return null;
		}
		float num2 = (float)Core.Width / 2f;
		float num3 = (float)Core.Height / 2f;
		Matrix4x4 cameraMatrix = Core.CameraMatrix;
		Vector3 localMainCamera = Core.LocalMainCamera;
		float num4 = ((Config.AimBotMaxDistance > 0f) ? Config.AimBotMaxDistance : 999f);
		foreach (KeyValuePair<ulong, Entity> item in entities)
		{
			try
			{
				Entity value = item.Value;
				if (value == null || value.Address == 0L || value.IsDead || value.IsKnocked || value.Head == Vector3.Zero)
				{
					continue;
				}
				Vector2 vector = W2S.WorldToScreen(cameraMatrix, value.Head, Core.Width, Core.Height);
				if (!float.IsNaN(vector.X) && !float.IsNaN(vector.Y) && !(vector.X < 1f) && !(vector.Y < 1f) && (!(localMainCamera != Vector3.Zero) || !(Vector3.Distance(localMainCamera, value.Head) > num4)))
				{
					float num5 = vector.X - num2;
					float num6 = vector.Y - num3;
					float num7 = (float)Math.Sqrt(num5 * num5 + num6 * num6);
					if (Config.DrawFov && num7 > Config.AimbotFOV)
					{
						continue;
					}
					if (!(num7 >= num))
					{
						num = num7;
						result = value;
					}
				}
			}
			catch
			{
			}
		}
		return result;
	}

	private static float GetDelayToHeadSeconds()
	{
		float num = ((Config.AimbotLegitDelayMs > 0f) ? Config.AimbotLegitDelayMs : Config.AimbotDelayMs);
		if (num < 0f)
		{
			num = 0f;
		}
		return num / 1000f;
	}

	private static void ResetClickState()
	{
		_alreadyReplaced = false;
		_clickStartTick = 0L;
		_currentTarget = null;
		DelayProgress = 0f;
		IsWaitingDelay = false;
	}

	private static void UpdateDelayBar(float progress, bool waiting)
	{
		try
		{
			DelayProgress = Math.Clamp(progress, 0f, 1f);
			IsWaitingDelay = waiting;
		}
		catch
		{
			DelayProgress = 0f;
			IsWaitingDelay = false;
		}
	}
}
