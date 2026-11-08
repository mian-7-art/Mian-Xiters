using System.Runtime.InteropServices;

namespace EspLib
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MianSharedConfig
    {
        public uint Magic;
        public uint Status;
        public uint Terminate;
        public uint Enable;
        public uint Skeleton;
        public uint Lines;
        public uint LinePos;
        public uint Box;
        public uint BoxType;
        public uint HealthBar;
        public uint Weapon;
        public uint Name;
        public uint Distance;
        public uint Radar;
        public uint Team;
        public uint StreamMode;
        public uint AutoRefresh;
        public uint LocalPlayer;
        public uint ColorIndex;
        public float ColorR;
        public float ColorG;
        public float ColorB;
        public float ColorA;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string ErrorMsg;
    }
}
