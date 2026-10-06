using System.Numerics;

namespace AotForms;

internal class Entity
{
	internal ulong Address;

	internal bool IsKnown;

	internal Bool3 IsTeam;

	internal Vector3 Head;

	internal Vector3 Neck;

	internal Vector3 Root;

	internal Vector3 Spine;

	internal Vector3 Hip;

	internal Vector3 Breast;

	internal Vector3 LeftShoulder;

	internal Vector3 RightShoulder;

	internal Vector3 LeftElbow;

	internal Vector3 RightElbow;

	internal Vector3 LeftWrist;

	internal Vector3 RightWrist;

	internal Vector3 LeftHand;

	internal Vector3 RightHand;

	internal Vector3 Eye;

	internal Vector3 LeftFoot;

	internal Vector3 RightFoot;

	internal Vector3 LeftLeg;

	internal Vector3 RightLeg;

	internal Vector3 LeftAnkle;

	internal Vector3 RightAnkle;

	internal short Health;

	internal bool IsDead;

	internal bool IsKnocked;

	internal bool IsBot;

	internal bool IsFiring;

	internal bool IsVisible;

	internal string Name;

	internal float Distance;

	internal Vector3 Velocity;

	internal string WeaponName;

	internal int Id;

	internal bool IsTeammate => IsTeam == Bool3.True;
}
