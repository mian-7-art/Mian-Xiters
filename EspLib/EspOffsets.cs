namespace EspLib
{
    /// <summary>
    /// IL2CPP memory offsets for Free Fire on BlueStacks.
    /// Ported from AotForms.Offsets — DO NOT CHANGE without updated offset scans.
    /// </summary>
    public static class EspOffsets
    {
        public static ulong Il2CppBase = 0x0;

        // --- Root pointer chain ---
        public static uint InitBase             = 0xA342EFC;
        public static uint StaticClass          = 0x5C;
        public static uint CurrentMatch         = 0x50;
        public static uint MatchStatus          = 0x8C;
        public static uint LocalPlayer          = 0x94;
        public static uint DictionaryEntities   = 0x68;
        public static uint CurrentObserver      = 0xB4;
        public static uint ObserverPlayer       = 0x28;

        // --- Player ---
        public static uint Player_IsDead        = 0x50;
        public static uint Player_Name          = 0x31C;
        public static uint Player_Data          = 0x48;
        public static uint Player_ShadowBase    = 0x1A60;
        public static uint XPose                = 0x78;
        public static uint LocalPlayerAttributes = 0x500;
        public static uint NoReload             = 0xC1;

        // --- Avatar ---
        public static uint AvatarManager        = 0x504;
        public static uint Avatar               = 0xA8;
        public static uint Avatar_IsVisible     = 0x95;
        public static uint Avatar_Data          = 0x14;
        public static uint Avatar_Data_IsTeam   = 0x59;

        // --- Camera ---
        public static uint FollowCamera         = 0x494;
        public static uint Camera               = 0x18;
        public static uint Camera_Intptr        = 0x8;
        public static uint MainCameraTransform  = 0x28C;
        public static uint ViewMatrix           = 0xE8;
        public static uint AimRotation          = 0x400;

        // --- Weapon ---
        public static uint Weapon               = 0x434;
        public static uint WeaponData           = 0x58;
        public static uint WeaponName           = 0x38;
        public static uint WeaponRecoil         = 0xC;
        public static uint Weapon_ItemId        = 0x494;

        // --- Aim ---
        public static uint sAim1                = 0x440;
        public static uint sAim2                = 0x454;
        public static uint sAim3                = 0x38;
        public static uint sAim4                = 0x2C;
        public static uint HeadCoilder          = 0x4E8;
        public static uint MainCollider         = 0x54;
    }
}
