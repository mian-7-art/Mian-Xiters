using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;

namespace AotForms;

internal static class Data
{
	private static int entityDiagCount;

	internal static void Work()
	{
		while (true)
		{
			Thread.Sleep(0);
			if (!Memory.IsValid)
			{
				continue;
			}
			if (!Memory.Read<uint>(Offsets.Il2CppBase + Offsets.InitBase, out var value) || value == 0)
			{
				ResetCache();
			}
			else
			{
				if (!Memory.Read<uint>(value, out var value2) || value2 == 0 || !Memory.Read<uint>(value2 + Offsets.StaticClass, out var value3) || value3 == 0 || !Memory.Read<uint>(value3, out var value4) || value4 == 0 || !Memory.Read<uint>(value4 + Offsets.CurrentMatch, out var value5) || value5 == 0)
				{
					continue;
				}
				if ((!Memory.Read<uint>(value5 + Offsets.LocalPlayer, out var value6) || value6 == 0) && Memory.Read<uint>(value5 + Offsets.CurrentObserver, out var value7) && value7 != 0 && Memory.Read<uint>(value7 + Offsets.ObserverPlayer, out var value8) && value8 != 0)
				{
					value6 = value8;
				}
				if (value6 == 0)
				{
					ResetCache();
					continue;
				}
				if (Core.LocalPlayer != value6)
				{
					ResetCache();
				}
				Core.LocalPlayer = value6;
				if (Memory.Read<uint>(value6 + 1200, out var value9) && value9 != 0)
				{
					Transform.GetNodePosition(value9, out var result);
					Core.playerpos = result;
				}
				if (Config.NoRecoil && Memory.Read<uint>(value6 + Offsets.Weapon, out var value10) && value10 != 0 && Memory.Read<uint>(value10 + Offsets.WeaponData, out var value11) && value11 != 0 && Memory.Read<float>(value11 + Offsets.WeaponRecoil, out var value12) && value12 != 0f)
				{
					Memory.Write(value11 + Offsets.WeaponRecoil, 0f);
				}
				uint value14;
				if (Config.FastReload)
				{
					if (Memory.Read<uint>(value6 + Offsets.LocalPlayerAttributes, out var value13) && value13 != 0)
					{
						Memory.Write(value13 + Offsets.NoReload, value: true);
					}
				}
				else if (Memory.Read<uint>(value6 + Offsets.LocalPlayerAttributes, out value14) && value14 != 0)
				{
					Memory.Write(value14 + Offsets.NoReload, value: false);
				}
				if (Memory.Read<uint>(value6 + Offsets.MainCameraTransform, out var value15) && value15 != 0 && Transform.GetPosition(value15, out var pos))
				{
					Core.LocalMainCamera = pos;
				}
				if (!Memory.Read<uint>(value6 + Offsets.FollowCamera, out var value16) || value16 == 0 || !Memory.Read<uint>(value16 + Offsets.Camera, out var value17) || value17 == 0 || !Memory.Read<uint>(value17 + 8, out var value18) || value18 == 0 || !Memory.Read<Matrix4x4>(value18 + Offsets.ViewMatrix, out var value19))
				{
					continue;
				}
				Core.CameraMatrix = value19;
				Core.HaveMatrix = true;
				if (Config.EspLootEnabled)
				{
					EspItem.ScanGround(value4, Core.LocalMainCamera);
				}
				foreach (uint entity in GetEntities(value4, Offsets.DictionaryEntities))
				{
					if (entity == 0 || entity == value6 || entity == (uint)Core.LocalPlayer)
					{
						continue;
					}
					if (Core.Entities.TryGetValue(entity, out var player))
					{
						player.Address = entity;
						if (player.IsTeam == Bool3.True && !Config.ESPTeam)
						{
							continue;
						}
						uint value20 = 0u;
						uint value21 = 0u;
						bool flag = (Memory.Read<uint>(entity + Offsets.AvatarManager, out value20) && value20 != 0 && Memory.Read<uint>(value20 + Offsets.Avatar, out value21) && value21 != 0) || (Memory.Read<uint>(entity + 1216, out value20) && value20 != 0 && Memory.Read<uint>(value20 + Offsets.Avatar, out value21) && value21 != 0);
						if (player.IsTeam == Bool3.Unknown)
						{
							if (flag && Memory.Read<uint>(value21 + Offsets.Avatar_Data, out var value22) && value22 != 0 && Memory.Read<bool>(value22 + Offsets.Avatar_Data_IsTeam, out var value23))
							{
								player.IsTeam = ((!value23) ? Bool3.False : Bool3.True);
								player.IsKnown = true;
							}
							if (!player.IsKnown)
							{
								player.IsKnown = true;
								player.IsTeam = Bool3.False;
							}
						}
						if ((player.IsTeam == Bool3.True && !Config.ESPTeam) || !player.IsKnown)
						{
							continue;
						}
						if (flag && Memory.Read<byte>(value21 + Offsets.Avatar_IsVisible, out var value24))
						{
							player.IsVisible = value24 != 0;
						}
						if (Config.IgnoreKnocked && Memory.Read<uint>(entity + Offsets.Player_ShadowBase, out var value25) && value25 != 0 && Memory.Read<int>(value25 + Offsets.XPose, out var value26))
						{
							player.IsKnocked = value26 == 8;
						}
						if (Memory.Read<bool>(entity + Offsets.Player_IsDead, out var value27))
						{
							player.IsDead = value27;
						}
						if (Config.ESPWeapon)
						{
							int num = 0;
							if (Memory.Read<uint>(entity + Offsets.Player_Data, out var value28) && value28 != 0 && Memory.Read<uint>(value28 + 8, out var value29) && value29 != 0 && Memory.Read<uint>(value29 + 32, out var value30) && value30 != 0)
							{
								short value32;
								if (Memory.Read<int>(value30 + 16, out var value31) && value31 > 0)
								{
									num = value31;
								}
								else if (Memory.Read<short>(value30 + 16, out value32) && value32 > 0)
								{
									num = value32;
								}
							}
							if (num <= 0 && Offsets.Weapon != 0 && Memory.Read<uint>(entity + Offsets.Weapon, out var value33) && value33 != 0 && Memory.Read<uint>(value33 + Offsets.Weapon_ItemId, out var value34) && value34 != 0)
							{
								num = (int)value34;
							}
							if (num > 0)
							{
								player.WeaponName = WeaponIndex.GetWeaponName(num);
							}
						}
						if ((Config.ESPName || Config.ESPHealth) && Memory.Read<uint>(entity + Offsets.Player_Data, out var value35) && value35 != 0 && Memory.Read<uint>(value35 + 8, out var value36) && value36 != 0)
						{
							if (Config.ESPName && Memory.Read<uint>(entity + Offsets.Player_Name, out var value37) && value37 != 0 && Memory.Read<int>(value37 + 8, out var value38) && value38 > 0)
							{
								string text = Memory.ReadString(value37 + 12, value38);
								if (text != "")
								{
									player.Name = text;
								}
							}
							if ((Config.ESPHealth || Config.AimbotVisible1) && Memory.Read<uint>(value36 + 16, out var value39) && value39 != 0)
							{
								short value42;
								if (Memory.Read<uint>(value39 + 12, out var value40) && value40 != 0)
								{
									if (Memory.Read<short>(value40, out var value41))
									{
										player.Health = value41;
									}
								}
								else if (Memory.Read<short>(value39 + 16, out value42))
								{
									player.Health = value42;
								}
							}
						}
						if (Memory.Read<uint>(entity + 1180, out var value43) && value43 != 0 && Transform.GetNodePosition(value43, out var result2))
						{
							player.Head = result2;
							player.Distance = Vector3.Distance(Core.LocalMainCamera, result2);
						}
						UpdateBone(entity, Bones.Root, player, (Vector3 v) =>
						{
							player.Root = v;
						});
						UpdateBone(entity, Bones.Breast, player, (Vector3 v) =>
						{
							player.Breast = v;
							player.Spine = v;
						});
						UpdateBone(entity, Bones.Breast, player, (Vector3 v) =>
						{
							player.Hip = v;
						});
						UpdateBone(entity, Bones.LeftShoulder, player, (Vector3 v) =>
						{
							player.LeftShoulder = v;
						});
						UpdateBone(entity, Bones.RightShoulder, player, (Vector3 v) =>
						{
							player.RightShoulder = v;
						});
						UpdateBone(entity, Bones.LeftHand, player, (Vector3 v) =>
						{
							player.LeftHand = v;
							player.LeftWrist = v;
						});
						UpdateBone(entity, Bones.RightHand, player, (Vector3 v) =>
						{
							player.RightHand = v;
							player.RightWrist = v;
						});
						UpdateBone(entity, Bones.LeftFoot, player, (Vector3 v) =>
						{
							player.LeftFoot = v;
						});
						UpdateBone(entity, Bones.RightFoot, player, (Vector3 v) =>
						{
							player.RightFoot = v;
						});
						UpdateBone(entity, Bones.LeftLeg, player, (Vector3 v) =>
						{
							player.LeftLeg = v;
						});
						UpdateBone(entity, Bones.RightLeg, player, (Vector3 v) =>
						{
							player.RightLeg = v;
						});
						UpdateBone(entity, Bones.leftankle, player, (Vector3 v) =>
						{
							player.LeftAnkle = v;
						});
						UpdateBone(entity, Bones.rightankle, player, (Vector3 v) =>
						{
							player.RightAnkle = v;
						});
					}
					else
					{
						player = new Entity();
						player.Address = entity;
						player.IsTeam = Bool3.Unknown;
						player.IsKnown = false;
						player.IsDead = true;
						player.Health = 0;
						player.IsKnocked = false;
						player.Head = Vector3.Zero;
						player.Root = Vector3.Zero;
						player.Name = "";
						player.IsFiring = false;
						player.IsVisible = false;
						Core.Entities.TryAdd(entity, player);
					}
				}
			}
		}
	}

	private static void UpdateBone(uint entity, Bones bone, Entity player, Action<Vector3> setter)
	{
		if (Memory.Read<uint>((ulong)(entity + bone), out var value) && value != 0 && Transform.GetNodePosition(value, out var result))
		{
			setter(result);
		}
	}

	private static List<uint> GetEntities(uint baseGame, uint offset)
	{
		List<uint> list = new List<uint>();
		if (!Memory.Read<uint>(baseGame + offset, out var value) || value == 0)
		{
			return list;
		}
		if (!Memory.Read<int>(value + 16, out var value2) || value2 < 1 || value2 > 10000)
		{
			return list;
		}
		if (!Memory.Read<uint>(value + 12, out var value3) || value3 == 0)
		{
			return list;
		}
		uint num = value3 + 16;
		for (uint num2 = 0u; num2 < (uint)value2; num2++)
		{
			if (Memory.Read<uint>(num + num2 * 16 + 12, out var value4) && value4 != 0)
			{
				list.Add(value4);
			}
		}
		entityDiagCount++;
		return list;
	}

	private static void ResetCache(string id = "")
	{
		Core.HaveMatrix = false;
		Core.MatchStatus = 0;
		Core.Entities = new ConcurrentDictionary<ulong, Entity>();
		Thread.Sleep(10);
	}
}
