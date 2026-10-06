using System.Numerics;
using System.Threading;

namespace AotForms;

internal static class UpPlayer
{
	internal static void Work()
	{
		while (true)
		{
			if (!Config.UpPlayer)
			{
				Thread.Sleep(1);
				continue;
			}
			if (Core.Width == -1 || Core.Height == -1 || !Core.HaveMatrix)
			{
				Thread.Sleep(1);
				continue;
			}
			foreach (Entity value5 in Core.Entities.Values)
			{
				if (value5.IsKnown && !value5.IsDead && (!Config.IgnoreKnocked || !value5.IsKnocked) && !(Vector3.Distance(Core.LocalMainCamera, value5.Head) > Config.AimBotMaxDistance))
				{
					Memory.Read<uint>(value5.Address + 1180, out var value);
					Memory.Read<uint>(value + 8, out var value2);
					Memory.Read<uint>(value2 + 8, out var value3);
					Memory.Read<uint>(value3 + 32, out var value4);
					Transform.GetNodePosition(value, out var result);
					Memory.Write(value4 + 128, result + new Vector3(0f, 0.1f, 0f));
					Thread.Sleep((int)Config.test);
				}
			}
		}
	}
}
