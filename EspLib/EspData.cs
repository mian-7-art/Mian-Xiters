using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;

namespace EspLib
{
    /// <summary>
    /// ESP entity bone offsets (pointer offsets from entity base address).
    /// </summary>
    internal enum EspBones : uint
    {
        Head          = 1180u,
        Breast        = 1188u,
        Root          = 1200u,
        LeftHand      = 1224u,
        RightHand     = 1176u,
        LeftShoulder  = 1232u,
        RightShoulder = 1236u,
        LeftFoot      = 1216u,
        RightFoot     = 1220u,
        LeftLeg       = 1204u,
        RightLeg      = 1184u,
        RightAnkle    = 1212u,
        LeftAnkle     = 1208u,
    }

    /// <summary>
    /// Background data-reading thread — continuously reads entities from BlueStacks memory
    /// and updates EspCore.Entities and EspCore.CameraMatrix.
    /// </summary>
    internal static class EspData
    {
        private static int entityDiagCount;

        internal static void Work()
        {
            int missedMatchCount = 0;
            while (true)
            {
                Thread.Sleep(0);
                if (!EspMemory.IsValid)
                {
                    Thread.Sleep(10);
                    continue;
                }

                if (!EspMemory.Read<uint>((ulong)(EspOffsets.Il2CppBase + EspOffsets.InitBase), out var initPtr) || initPtr == 0)
                {
                    ResetCache();
                    continue;
                }
                if (!EspMemory.Read<uint>(initPtr, out var staticField) || staticField == 0) continue;
                if (!EspMemory.Read<uint>(staticField + EspOffsets.StaticClass, out var staticClass) || staticClass == 0) continue;
                if (!EspMemory.Read<uint>(staticClass, out var staticClassBase) || staticClassBase == 0) continue;
                if (!EspMemory.Read<uint>(staticClassBase + EspOffsets.CurrentMatch, out var match) || match == 0)
                {
                    missedMatchCount++;
                    if (missedMatchCount > 30) ResetCache();
                    continue;
                }

                // Local player
                uint localPlayer;
                if (!EspMemory.Read<uint>(match + EspOffsets.LocalPlayer, out localPlayer) || localPlayer == 0)
                {
                    // Try observer path
                    if (EspMemory.Read<uint>(match + EspOffsets.CurrentObserver, out var obs) && obs != 0 &&
                        EspMemory.Read<uint>(obs + EspOffsets.ObserverPlayer, out var obsPlayer) && obsPlayer != 0)
                        localPlayer = obsPlayer;
                }
                if (localPlayer == 0)
                {
                    missedMatchCount++;
                    if (missedMatchCount > 30) ResetCache();
                    continue;
                }

                missedMatchCount = 0;

                if (EspCore.LocalPlayer != (ulong)localPlayer) ResetCache();
                EspCore.LocalPlayer = localPlayer;

                // Camera position
                if (EspMemory.Read<uint>(localPlayer + EspOffsets.MainCameraTransform, out var camTr) && camTr != 0)
                    EspTransform.GetPosition(camTr, out EspCore.LocalMainCamera);

                // Camera matrix
                if (!EspMemory.Read<uint>(localPlayer + EspOffsets.FollowCamera, out var followCam) || followCam == 0) continue;
                if (!EspMemory.Read<uint>(followCam + EspOffsets.Camera, out var camObj) || camObj == 0) continue;
                if (!EspMemory.Read<uint>(camObj + 8, out var camIntPtr) || camIntPtr == 0) continue;
                if (!EspMemory.Read<Matrix4x4>(camIntPtr + EspOffsets.ViewMatrix, out var viewMatrix)) continue;

                EspCore.CameraMatrix = viewMatrix;
                EspCore.HaveMatrix   = true;

                // Entities
                foreach (uint entity in GetEntities(staticClassBase, EspOffsets.DictionaryEntities))
                {
                    if (entity == 0 || entity == localPlayer || entity == (uint)EspCore.LocalPlayer)
                        continue;

                    if (EspCore.Entities.TryGetValue(entity, out var player))
                    {
                        player.Address = entity;

                        // Team check
                        uint avMgr = 0, avBase = 0;
                        bool hasAv = (EspMemory.Read<uint>(entity + EspOffsets.AvatarManager, out avMgr) && avMgr != 0 &&
                                      EspMemory.Read<uint>(avMgr + EspOffsets.Avatar, out avBase) && avBase != 0) ||
                                     (EspMemory.Read<uint>(entity + 1216, out avMgr) && avMgr != 0 &&
                                      EspMemory.Read<uint>(avMgr + EspOffsets.Avatar, out avBase) && avBase != 0);

                        if (player.IsTeam == EspBool3.Unknown)
                        {
                            if (hasAv && EspMemory.Read<uint>(avBase + EspOffsets.Avatar_Data, out var avData) && avData != 0 &&
                                EspMemory.Read<bool>(avData + EspOffsets.Avatar_Data_IsTeam, out var isTeamFlag))
                            {
                                player.IsTeam  = isTeamFlag ? EspBool3.True : EspBool3.False;
                                player.IsKnown = true;
                            }
                        }

                        if (player.IsTeam == EspBool3.True && !EspConfig.EspTeam) continue;

                        // Visibility
                        if (hasAv && EspMemory.Read<byte>(avBase + EspOffsets.Avatar_IsVisible, out var visFlag))
                            player.IsVisible = visFlag != 0;

                        // Dead flag
                        if (EspMemory.Read<bool>(entity + EspOffsets.Player_IsDead, out var dead))
                            player.IsDead = dead;

                        // Name
                        if (EspMemory.Read<uint>(entity + EspOffsets.Player_Name, out var namePtr) && namePtr != 0 &&
                            EspMemory.Read<int>(namePtr + 8, out var nameLen) && nameLen > 0)
                        {
                            string n = EspMemory.ReadString(namePtr + 12, nameLen);
                            if (!string.IsNullOrEmpty(n)) player.Name = n;
                        }

                        // Health
                        if (EspMemory.Read<uint>(entity + EspOffsets.Player_Data, out var pData) && pData != 0 &&
                            EspMemory.Read<uint>(pData + 8, out var pInner) && pInner != 0)
                        {
                            if (EspMemory.Read<uint>(pInner + 16, out var hpPtr) && hpPtr != 0)
                            {
                                if (EspMemory.Read<uint>(hpPtr + 12, out var hpPtr2) && hpPtr2 != 0)
                                {
                                    if (EspMemory.Read<short>(hpPtr2, out var hpVal))
                                    {
                                        player.Health = hpVal;
                                        if (hpVal <= 0) player.IsDead = true;
                                    }
                                }
                                else if (EspMemory.Read<short>(hpPtr + 16, out var hpVal2))
                                {
                                    player.Health = hpVal2;
                                    if (hpVal2 <= 0) player.IsDead = true;
                                }
                            }
                        }

                        // Head position
                        if (EspMemory.Read<uint>(entity + (uint)EspBones.Head, out var headNode) && headNode != 0)
                            EspTransform.GetNodePosition(headNode, out player.Head);

                        // Weapon
                        if (EspConfig.EspWeapon)
                        {
                            if (EspMemory.Read<uint>(entity + EspOffsets.Weapon, out var wpnPtr) && wpnPtr != 0 &&
                                EspMemory.Read<uint>(wpnPtr + EspOffsets.WeaponData, out var wpnData) && wpnData != 0 &&
                                EspMemory.Read<int>(wpnData + EspOffsets.Weapon_ItemId, out var wpnId) && wpnId >= 0)
                            {
                                player.WeaponName = EspWeaponIndex.GetWeaponName(wpnId);
                            }
                        }

                        // Root / feet & Skeleton Bones
                        UpdateBone(entity, EspBones.Root, v => player.Root = v);
                        UpdateBone(entity, EspBones.LeftFoot, v => player.LeftFoot = v);
                        UpdateBone(entity, EspBones.RightFoot, v => player.RightFoot = v);

                        if (EspConfig.EspSkeleton)
                        {
                            UpdateBone(entity, EspBones.Breast, v => player.Breast = v);
                            UpdateBone(entity, EspBones.LeftShoulder, v => player.LeftShoulder = v);
                            UpdateBone(entity, EspBones.RightShoulder, v => player.RightShoulder = v);
                            UpdateBone(entity, EspBones.LeftHand, v => player.LeftHand = v);
                            UpdateBone(entity, EspBones.RightHand, v => player.RightHand = v);
                            UpdateBone(entity, EspBones.LeftLeg, v => player.LeftKnee = v);
                            UpdateBone(entity, EspBones.RightLeg, v => player.RightKnee = v);
                        }
                    }
                    else
                    {
                        var newPlayer = new EspEntity
                        {
                            Address   = entity,
                            IsTeam    = EspBool3.Unknown,
                            IsKnown   = false,
                            IsDead    = false,
                            Health    = 200,
                            IsKnocked = false,
                            IsVisible = true,
                            Name      = "",
                        };
                        EspCore.Entities.TryAdd(entity, newPlayer);
                    }
                }
            }
        }

        private static void UpdateBone(uint entity, EspBones bone, Action<Vector3> setter)
        {
            if (EspMemory.Read<uint>((ulong)(entity + (uint)bone), out var nodePtr) && nodePtr != 0)
            {
                if (EspTransform.GetNodePosition(nodePtr, out var pos))
                    setter(pos);
            }
        }

        private static List<uint> GetEntities(uint baseGame, uint offset)
        {
            var list = new List<uint>();
            if (!EspMemory.Read<uint>(baseGame + offset, out var dictPtr) || dictPtr == 0) return list;
            if (!EspMemory.Read<int>(dictPtr + 16, out var count) || count < 1 || count > 2000) return list;
            if (!EspMemory.Read<uint>(dictPtr + 12, out var entriesPtr) || entriesPtr == 0) return list;

            uint base2 = entriesPtr + 16;
            int bufferSize = count * 16;
            byte[] buffer = new byte[bufferSize];

            // Batch-read entire dictionary array in a single memory query (10x-50x faster)
            if (EspMemory.Read((ulong)base2, buffer, bufferSize))
            {
                for (int i = 0; i < count; i++)
                {
                    uint ent = BitConverter.ToUInt32(buffer, i * 16 + 12);
                    if (ent != 0)
                        list.Add(ent);
                }
            }
            else
            {
                for (uint i = 0; i < (uint)count; i++)
                {
                    if (EspMemory.Read<uint>(base2 + i * 16 + 12, out var ent) && ent != 0)
                        list.Add(ent);
                }
            }
            entityDiagCount++;
            return list;
        }

        private static void ResetCache()
        {
            EspCore.HaveMatrix  = false;
            EspCore.MatchStatus = 0;
            EspCore.Entities    = new ConcurrentDictionary<ulong, EspEntity>();
            Thread.Sleep(10);
        }
    }
}
