namespace Hydra
{
    internal static class Offsets
    {
        // ═══════════════ STATIC / BASE ═══════════════
        internal static uint InitBase                     = 0xA342EFC; // updated V7A
        internal static uint GameFacade_Script            = 0xA5BC344;
        internal static uint StaticClass                  = 0x5C;      // updated V7A
        internal static uint WardrobeClassOffset          = 0xA987F88;

        // ═══════════════ MATCH / GAME STATE ═══════════════
        internal static uint CurrentMatch                 = 0x50;      // updated V7A
        internal static uint CurrentMatchGame             = 0x4;
        internal static uint MatchState                   = 0x8C;
        internal static uint MatchStatus                  = 0x8C;      // updated V7A
        internal static uint GameTimer                    = 0x10;
        internal static uint GameFacadeTimer              = 0x10;      // updated V7A
        internal static uint FixedDeltaTime               = 0x24;
        internal static uint TimerFixedDeltaTime          = 0x24;      // updated V7A
        internal static uint Level                        = 0x14;

        // ═══════════════ PLAYER ═══════════════
        internal static uint LocalPlayer                  = 0x94;      // updated V7A
        internal static uint PlayerAttributes             = 0x500;     // updated V7A
        internal static uint LocalPlayerAttributes        = 0x500;     // updated V7A
        internal static uint DictionaryEntities           = 0x68;      // updated V7A
        internal static uint Player_Name                  = 0x31C;     // updated V7A
        internal static uint Player_IsDead                = 0x50;      // updated V7A
        internal static uint Player_Data                  = 0x48;      // updated V7A
        internal static uint Player_ShadowBase            = 0x1A60;    // updated V7A
        internal static uint IsClientBot                  = 0x324;
        internal static uint XPose                        = 0x78;      // updated V7A
        internal static uint IsFiring                     = 0x53D;     // updated V7A
        internal static uint IS_FIRING                    = 0x53D;     // updated V7A
        internal static uint InSnowSlideWayDashing        = 0x1768;    // updated V7A
        internal static uint SnowSlideWayLockDashOffset   = 0x1760;    // updated V7A
        internal static uint UnlimitedAmmo                = 0xE0;
        internal static uint Vida                         = 0x10;      // updated V7A
        internal static uint Player_Health                = 0x1C;      // updated V7A
        internal static uint Player_Data_Pool             = 0x18;      // updated V7A
        internal static uint Player_Data_PoolObj          = 0x110;     // updated V7A
        internal static uint Profile                      = 0x1A74;    // updated V7A
        internal static uint RankPoint                    = 0x5C;      // unchanged V7A

        // ═══════════════ AVATAR ═══════════════
        internal static uint Avatar                       = 0xA8;      // updated V7A
        internal static uint AvatarManager                = 0x504;     // updated V7A
        internal static uint Avatar_Data                  = 0x14;      // updated V7A
        internal static uint Avatar_Data_IsTeam           = 0x59;      // updated V7A
        internal static uint Avatar_IsVisible             = 0x95;      // updated V7A

        // ═══════════════ CAMERA ═══════════════
        internal static uint Camera                       = 0x18;      // updated V7A
        internal static uint Camera_Intptr                = 0x8;
        internal static uint MainCameraTransform          = 0x28C;     // updated V7A
        internal static uint FollowCamera                 = 0x494;     // updated V7A
        internal static uint AimRotation                  = 0x440;     // updated V7A
        internal static uint ViewMatrix                   = 0xE8;      // updated V7A

        // ═══════════════ OBSERVER ═══════════════
        internal static uint CurrentObserver              = 0xB4;
        internal static uint ObserverPlayer               = 0x28;

        // ═══════════════ WEAPON ═══════════════
        internal static uint Weapon                       = 0x434;     // updated V7A
        internal static uint WeaponData                   = 0x58;      // updated V7A
        internal static uint IntWeaponType                = 0x64;
        internal static uint WeaponRecoil                 = 0xC;       // updated V7A
        internal static uint NoReload                     = 0xC1;      // updated V7A
        internal static uint NoReloadPtr                  = 0xC1;
        internal static uint RapidfirePtr                 = 0x1C8;
        internal static uint DobleGunOffs1                = 0x4B4;     // updated V7A
        internal static uint DobleGunOffs2                = 0x4D0;     // updated V7A
        internal static uint MedikitFastOffs1             = 0x84;      // updated V7A
        internal static uint MedikitFastOffs2             = 0x88;      // updated V7A
        internal static uint CooldownHackOffs             = 0x158;     // updated V7A (+0x38)
        internal static uint BuffWeaponAmmoClip           = 0x104;     // updated V7A
        internal static uint m_FireIntervalScale          = 0x1AC;     // updated V7A
        internal static uint m_FireIntervalScaleSkill     = 0x1B0;     // updated V7A
        internal static uint m_FireIntervalScaleTwo       = 0x1B4;     // updated V7A
        internal static uint QuickSwap                    = 0x528;
        internal static uint WeaponInfoCtrl               = 0x35C;
        internal static uint WeaponIconColor              = 0xA4;
        internal static uint WeaponSkinMap                = 0x18D8;

        // ═══════════════ AIM / AIMBOT ═══════════════
        internal static uint AimbotVisible                = 0x4E8;
        internal static uint sAim1                        = 0x58C;     // updated V7A
        internal static uint sAim2                        = 0x9D0;     // updated V7A
        internal static uint sAim3                        = 0x38;      // updated V7A
        internal static uint sAim4                        = 0x2C;      // updated V7A
        internal static uint HeadCollider                 = 0x4E8;     // updated V7A
        internal static uint HeadCollider2                = 0x4EC;     // updated V7A

        // ═══════════════ PHYSICS / MOVEMENT / GRAVITY ═══════════════
        internal static uint Matrix1                      = 0x8;
        internal static uint Matrix3                      = 0x20;
        internal static uint Matrix10                     = 0x60;
        internal static uint Rigidbody                    = 0x14;
        internal static uint UseGravity                   = 0x38;
        internal static uint GravityVector                = 0x40;      // updated V7A
        internal static uint VelocityY                    = 0x24;
        internal static uint LastVelocity                 = 0x70;
        internal static uint IsGrounded                   = 0xB8;
        internal static uint RisingGravity                = 0x1770;    // updated V7A
        internal static uint FallingGravity               = 0x1774;    // updated V7A
        internal static uint EnableAccelerationOnFalling   = 0x28F3;    // updated V7A
        internal static uint EnableLowFallingSwapWeapon    = 0x2C3D;    // updated V7A
        internal static uint FlyHack_MovementComp         = 0x14E8;    // updated V7A
        internal static uint FlyHack_Position             = 0x20;      // updated V7A
        internal static uint FlyHack_VSpeed               = 0x2C;      // updated V7A
        internal static uint FlyHack_IsGrounded           = 0x1554;    // updated V7A

        // ═══════════════ UI / CONTROLS / SENSITIVITY ═══════════════
        internal static uint CrosshairsCtrl               = 0x3D0;     // updated V7A
        internal static uint FireCtrl                     = 0x3DC;     // updated V7A
        internal static uint AuxFireCtrl                  = 0x3B4;     // updated V7A
        internal static uint PlayerStatsCtrl              = 0x4E8;     // updated V7A
        internal static uint CrosshairNormalColor         = 0x208;     // updated V7A
        internal static uint CrosshairAlertColor          = 0x1F8;     // updated V7A
        internal static uint FireSpriteColor              = 0xA0;      // updated V7A
        internal static uint AuxFireColor                 = 0x70;      // updated V7A
        internal static uint StatsView                    = 0x5C;      // updated V7A
        internal static uint StatsSpr1                    = 0x58;
        internal static uint StatsSpr2                    = 0x74;
        internal static uint StatsColor                   = 0x94;      // updated V7A
        internal static uint UserControl                  = 0x344;     // updated V7A
        internal static uint AxisData                     = 0x34;      // updated V7A
        internal static uint FingerInDashArea             = 0x4C;      // updated V7A
        internal static uint IsTouched                    = 0x37;      // updated V7A
        internal static uint LockFingerInDashArea         = 0x50;      // updated V7A
        internal static uint DashByMovingJoystick         = 0x58;      // updated V7A
        internal static uint LockedAimingCollider         = 0x54;      // updated V7A
        internal static uint RotSensMin                   = 0xFD4;     // updated V7A
        internal static uint RotSensMax                   = 0xFD8;     // updated V7A
        internal static uint AimRotSensMin                = 0xFDC;     // updated V7A
        internal static uint AimRotSensMax                = 0xFE0;     // updated V7A

        // ═══════════════ MISC ═══════════════
        internal static uint Hydra                        = 0x54;
        internal static uint pomba                        = 0x544;
    }
}
