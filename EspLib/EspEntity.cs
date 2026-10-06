using System.Numerics;

namespace EspLib
{
    internal class EspEntity
    {
        internal ulong Address;
        internal bool IsKnown;
        internal EspBool3 IsTeam;

        // Bone 3D World Positions
        internal Vector3 Head;
        internal Vector3 Breast;
        internal Vector3 Root;
        internal Vector3 LeftShoulder;
        internal Vector3 RightShoulder;
        internal Vector3 LeftElbow;
        internal Vector3 RightElbow;
        internal Vector3 LeftHand;
        internal Vector3 RightHand;
        internal Vector3 LeftHip;
        internal Vector3 RightHip;
        internal Vector3 LeftKnee;
        internal Vector3 RightKnee;
        internal Vector3 LeftAnkle;
        internal Vector3 RightAnkle;
        internal Vector3 LeftFoot;
        internal Vector3 RightFoot;

        // Info Flags
        internal bool IsDead;
        internal bool IsKnocked;
        internal bool IsVisible;
        internal string Name;
        internal string WeaponName;
        internal short Health;
        internal bool IsTeammate => IsTeam == EspBool3.True;
    }
}
