using System;
using System.Collections.Generic;

namespace AotForms;

public static class SkinChanger
{
	public static bool Enabled = false;

	public static Dictionary<uint, uint> ActiveSkins = new Dictionary<uint, uint>();

	public static void CommitSkins(uint lp, Dictionary<uint, uint> skinMap)
	{
		if (lp == 0)
		{
			Console.WriteLine("[Error] CommitSkins: LocalPlayer is 0");
			return;
		}
		if (skinMap == null || skinMap.Count == 0)
		{
			Console.WriteLine("[Error] CommitSkins: skinMap is null or empty");
			return;
		}
		if (!Memory.Read<uint>(lp + Offsets.WeaponSkinMap, out var value) || value == 0)
		{
			Console.WriteLine("[Error] CommitSkins: Failed to read WeaponSkinMap");
			return;
		}
		if (!Memory.Read<uint>(value + Offsets.SkinMap_kArr, out var value2) || value2 == 0)
		{
			Console.WriteLine("[Error] CommitSkins: Failed to read kArr");
			return;
		}
		if (!Memory.Read<uint>(value + Offsets.SkinMap_vArr, out var value3) || value3 == 0)
		{
			Console.WriteLine("[Error] CommitSkins: Failed to read vArr");
			return;
		}
		if (!Memory.Read<int>(value + Offsets.SkinMap_Size, out var value4) || value4 < 0 || value4 >= 1000)
		{
			Console.WriteLine($"[Error] CommitSkins: Invalid size {value4}");
			return;
		}
		if (!Memory.Read<int>(value2 + Offsets.SkinArray_Capacity, out var value5))
		{
			Console.WriteLine("[Error] CommitSkins: Failed to read capacity");
			return;
		}
		if (value5 <= 0 || value2 == value3)
		{
			Console.WriteLine($"[Debug] CommitSkins: Fixing capacity or overlapping arrays (cap: {value5})");
			value5 = 50;
			Memory.Write(value2 + Offsets.SkinArray_Capacity, value5);
			if (Memory.Read<uint>(value2, out var value6))
			{
				uint num = value2 + 512;
				Console.WriteLine($"[Debug] CommitSkins: Forging vArr at 0x{num:X}");
				Memory.Write(num, value6);
				Memory.Write(num + 4, 0u);
				Memory.Write(num + 8, 0u);
				Memory.Write(num + Offsets.SkinArray_Capacity, value5);
				Memory.Write(value + Offsets.SkinMap_vArr, num);
				value3 = num;
			}
			else
			{
				Console.WriteLine("[Error] CommitSkins: Failed to read klass for forging");
				Memory.Write(value3 + Offsets.SkinArray_Capacity, value5);
			}
		}
		uint num2 = value2 + Offsets.SkinArray_Base;
		uint num3 = value3 + Offsets.SkinArray_Base;
		List<(uint, uint)> list = new List<(uint, uint)>();
		for (uint num4 = 0u; num4 < (uint)value4; num4++)
		{
			if (Memory.Read<uint>(num2 + 4 * num4, out var value7) && Memory.Read<uint>(num3 + 4 * num4, out var value8))
			{
				list.Add((value7, value8));
			}
		}
		bool flag = false;
		foreach (KeyValuePair<uint, uint> kvp in skinMap)
		{
			uint value9 = kvp.Value;
			int num5 = list.FindIndex(((uint weaponID, uint skin) x) => x.weaponID == kvp.Key);
			if (num5 != -1)
			{
				if (list[num5].Item2 != value9)
				{
					list[num5] = (kvp.Key, value9);
					flag = true;
				}
				continue;
			}
			if (list.Count < value5)
			{
				list.Add((kvp.Key, value9));
				flag = true;
				continue;
			}
			int num6 = list.FindIndex(((uint weaponID, uint skin) x) => !skinMap.ContainsKey(x.weaponID));
			if (num6 != -1)
			{
				list[num6] = (kvp.Key, value9);
				flag = true;
			}
		}
		if (flag)
		{
			Console.WriteLine($"[Info] CommitSkins: Updating {list.Count} entries");
			list.Sort(((uint weaponID, uint skin) a, (uint weaponID, uint skin) b) => a.weaponID.CompareTo(b.weaponID));
			for (uint num7 = 0u; num7 < list.Count; num7++)
			{
				Memory.Write(num2 + 4 * num7, list[(int)num7].Item1);
				Memory.Write(num3 + 4 * num7, list[(int)num7].Item2);
			}
			if (list.Count != value4)
			{
				Memory.Write(value + Offsets.SkinMap_Size, list.Count);
			}
		}
	}
}
