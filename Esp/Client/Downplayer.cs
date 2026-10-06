using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using AotForms;

namespace Client;

internal static class Downplayer
{
	private static Task downTask;

	private static CancellationTokenSource cts = new CancellationTokenSource();

	private static bool isRunning = false;

	internal static void Work()
	{
		if (isRunning)
		{
			return;
		}
		isRunning = true;
		downTask = Task.Run(async () =>
		{
			while (!cts.Token.IsCancellationRequested)
			{
				try
				{
					if (!Config.down || Core.Entities == null || !Core.HaveMatrix)
					{
						await Task.Delay(10, cts.Token);
						continue;
					}
					foreach (Entity value6 in Core.Entities.Values)
					{
						if (value6.IsKnown && !value6.IsDead && value6.Address != Core.LocalPlayer && Memory.Read<uint>(value6.Address + 1200, out var value) && value != 0 && Memory.Read<uint>(value + 8, out var value2) && value2 != 0 && Memory.Read<uint>(value2 + 8, out var value3) && value3 != 0 && Memory.Read<uint>(value3 + 32, out var value4) && value4 != 0 && Memory.Read<Vector3>(value4 + 96, out var value5))
						{
							value5.Y -= Config.downSpeed;
							Memory.Write(value4 + 96, value5);
						}
					}
				}
				catch
				{
				}
				await Task.Delay(10, cts.Token);
			}
		}, cts.Token);
	}

	internal static void Stop()
	{
		if (isRunning)
		{
			cts.Cancel();
			cts.Dispose();
			cts = new CancellationTokenSource();
			isRunning = false;
		}
	}
}
