namespace AotForms
{
    internal enum Bones : uint
    {
        Head            = 0x49C,
        Neck            = 0x4A4,
        LeftWristJoint  = 0x4DC,
        LeftWrist       = 0x4DC,
        Root            = 0x4B0,
        RootBone        = 0x4B0,
        Spine           = 0x4A4,
        Hip             = 0x4A0,
        RightCalf       = 0x4BC, // updated V7A
        LeftCalf        = 0x4B8, // updated V7A
        RightFoot       = 0x4C4,
        LeftFoot        = 0x4C0,
        RightWrist      = 0x4D8, // updated V7A
        LeftHand        = 0x4C8, // updated V7A (Offsets.hpp: 0x4C8)
        LeftShoulder    = 0x4D0,
        RightShoulder   = 0x4D4,
        RightWristJoint = 0x4D8, // updated V7A
        LeftElbow       = 0x4E4,
        RightElbow      = 0x4E0,
        Groin           = 0x4AC,
        RightHand       = 0x498, // updated V7A (Offsets.hpp: 0x498)
        LeftAnkle       = 0x4B8,
        RightAnkle      = 0x4BC,
    }
}
