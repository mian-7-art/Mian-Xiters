using System;
using System.Collections.Generic;
using System.Numerics;

namespace AotForms;

internal static class EspItem
{
	public static readonly List<GroundLootEntry> GroundLoot;

	private static readonly object _lootLock;

	private static readonly Dictionary<uint, string> ItemNames;

	private static readonly List<List<uint>> Categories;

	private static readonly List<LootCatalogItem> Catalog;

	private static bool _isDbBuilt;

	public static readonly string[] CategoryNames;

	private const int MaxLootPerScan = 450;

	private const uint DictEntrySize = 16u;

	static EspItem()
	{
		GroundLoot = new List<GroundLootEntry>();
		_lootLock = new object();
		ItemNames = new Dictionary<uint, string>();
		Categories = new List<List<uint>>();
		Catalog = new List<LootCatalogItem>();
		_isDbBuilt = false;
		CategoryNames = new string[15]
		{
			"Rifles", "Marksman Rifles", "Machine Guns", "Sub Machine Guns", "Shotguns", "Sniper Rifles", "Pistols", "Melee", "Launchers & Crossbow", "Ammo",
			"Healing Utilities", "Projectiles & Landmines", "Armors & Backpacks", "Attachments", "Miscellaneous"
		};
		BuildDatabase();
	}

	private static void AddItem(uint id, string name)
	{
		ItemNames[id] = name;
	}

	private static void BuildDatabase()
	{
		if (_isDbBuilt)
		{
			return;
		}
		AddItem(2u, "M4A1");
		AddItem(80u, "M4A1-I");
		AddItem(81u, "M4A1-II");
		AddItem(82u, "M4A1-III");
		AddItem(6u, "AK47");
		AddItem(11u, "M14");
		AddItem(63u, "M14-I");
		AddItem(126u, "M14-II");
		AddItem(127u, "M14-III");
		AddItem(12u, "SCAR");
		AddItem(178u, "SCAR-I");
		AddItem(179u, "SCAR-II");
		AddItem(180u, "SCAR-III");
		AddItem(14u, "GROZA");
		AddItem(70u, "GROZA-X");
		AddItem(24u, "FAMAS");
		AddItem(67u, "FAMAS-I");
		AddItem(130u, "FAMAS-II");
		AddItem(131u, "FAMAS-III");
		AddItem(28u, "XM8");
		AddItem(33u, "AN94");
		AddItem(39u, "Plasma");
		AddItem(46u, "AUG");
		AddItem(193u, "AUG-I");
		AddItem(194u, "AUG-II");
		AddItem(195u, "AUG-III");
		AddItem(47u, "PARAFAL");
		AddItem(57u, "KingFisher");
		AddItem(73u, "G36-ASSAULT");
		AddItem(74u, "G36-RANGE");
		AddItem(18u, "SKS");
		AddItem(26u, "SVD");
		AddItem(72u, "SVD-Y");
		AddItem(48u, "Woodpecker");
		AddItem(89u, "AC80");
		AddItem(19u, "M249");
		AddItem(71u, "M249-X");
		AddItem(30u, "M60");
		AddItem(61u, "M60-I");
		AddItem(122u, "M60-II");
		AddItem(123u, "M60-III");
		AddItem(54u, "Kord");
		AddItem(7u, "UMP");
		AddItem(8u, "MP5");
		AddItem(60u, "MP5-I");
		AddItem(120u, "MP5-II");
		AddItem(121u, "MP5-III");
		AddItem(13u, "VSS");
		AddItem(62u, "VSS-I");
		AddItem(124u, "VSS-II");
		AddItem(125u, "VSS-III");
		AddItem(15u, "MP40");
		AddItem(32u, "P90");
		AddItem(35u, "CG15");
		AddItem(43u, "THOMPSON");
		AddItem(49u, "Vector");
		AddItem(69u, "DOUBLE-VECTOR");
		AddItem(88u, "MAC10");
		AddItem(228u, "MAC10-I");
		AddItem(229u, "MAC10-II");
		AddItem(230u, "MAC10-III");
		AddItem(150u, "BIZON");
		AddItem(5u, "M1014");
		AddItem(184u, "M1014-I");
		AddItem(185u, "M1014-II");
		AddItem(186u, "M1014-III");
		AddItem(29u, "SPAS12");
		AddItem(41u, "M1887");
		AddItem(119u, "M1887-X");
		AddItem(50u, "MAG-7");
		AddItem(86u, "Charge Buster");
		AddItem(181u, "Trogon-Shotgun");
		AddItem(182u, "Trogon-Grenade");
		AddItem(4u, "AWM");
		AddItem(65u, "AWM-Y");
		AddItem(21u, "Kar98K");
		AddItem(64u, "Kar98K-I");
		AddItem(128u, "Kar98K-II");
		AddItem(129u, "Kar98K-III");
		AddItem(45u, "M82B");
		AddItem(75u, "M24");
		AddItem(78u, "Heal Sniper");
		AddItem(197u, "VSK94");
		AddItem(3u, "USP");
		AddItem(56u, "USP-2");
		AddItem(9u, "Desert Eagle");
		AddItem(10u, "G18");
		AddItem(20u, "M1873");
		AddItem(25u, "M500");
		AddItem(55u, "M1917");
		AddItem(58u, "MINI UZI");
		AddItem(34u, "Katana");
		AddItem(16u, "PAN");
		AddItem(17u, "MACHETE");
		AddItem(27u, "Bat");
		AddItem(51u, "Scythe");
		AddItem(53u, "FF Knife");
		AddItem(1u, "Fist");
		AddItem(601u, "Grenade");
		AddItem(603u, "Smoke Grenade");
		AddItem(1201u, "Gloo Wall");
		AddItem(1204u, "Gloo Wall");
		AddItem(602u, "Flash Grenade");
		AddItem(608u, "Freeze Bomb");
		AddItem(21002u, "M590");
		AddItem(23u, "M79");
		AddItem(36u, "RGS-50");
		AddItem(196u, "FGL-24");
		AddItem(100u, "Flamethrower");
		AddItem(99u, "Shield Gun");
		AddItem(21001u, "Heal Pistol");
		AddItem(21006u, "Winchester");
		AddItem(638u, "Thompson-x");
		AddItem(501u, "Muzzle LV1");
		AddItem(502u, "Muzzle LV2");
		AddItem(503u, "Muzzle LV3");
		AddItem(504u, "Silencer");
		AddItem(213u, "Inhaler");
		AddItem(221u, "Energizer");
		AddItem(3001u, "FF Coin");
		AddItem(106u, "Repair Kit");
		AddItem(201u, "AR Ammo");
		AddItem(203u, "Sniper Ammo");
		AddItem(204u, "SG Ammo");
		AddItem(205u, "SMG Ammo");
		AddItem(207u, "Launcher Ammo");
		AddItem(102u, "Medikit");
		AddItem(1205u, "Super Medikit");
		AddItem(401u, "Backpack LV1");
		AddItem(402u, "Backpack LV2");
		AddItem(403u, "Backpack LV3");
		AddItem(301u, "Vest LV1");
		AddItem(302u, "Vest LV2");
		AddItem(303u, "Vest LV3");
		AddItem(309u, "Vest LV4");
		AddItem(304u, "Helmet LV1");
		AddItem(305u, "Helmet LV2");
		AddItem(306u, "Helmet LV3");
		AddItem(534u, "Scope 2x");
		AddItem(535u, "Scope 4x");
		AddItem(537u, "Thermal Scope");
		AddItem(511u, "Magazine LV1");
		AddItem(512u, "Magazine LV2");
		AddItem(513u, "Magazine LV3");
		AddItem(518u, "Fast Magazine");
		AddItem(521u, "Foregrip LV1");
		AddItem(522u, "Foregrip LV2");
		AddItem(523u, "Foregrip LV3");
		AddItem(526u, "Light Foregrip");
		AddItem(541u, "Stock LV1");
		AddItem(542u, "Stock LV2");
		AddItem(543u, "Stock LV3");
		AddItem(547u, "Power Stock");
		AddItem(3008u, "Upgrade Chip");
		AddItem(5601u, "Shield Booster");
		AddItem(1401u, "Landmine");
		AddItem(617u, "Gloo Melter");
		AddItem(624u, "Dragon Freeze");
		AddItem(5605u, "Helmet Thickener");
		AddItem(5652u, "Gloo Catalyst");
		AddItem(50010u, "Arsenal Key");
		AddItem(91007u, "Skill: Chrono");
		AddItem(91010u, "Skill: Dmitri");
		AddItem(91011u, "Skill: Kenta");
		AddItem(91012u, "Skill: Homer");
		AddItem(91053u, "Skill: Ignis");
		AddItem(91002u, "Skill: AI24");
		AddItem(93002u, "Supply Box");
		AddItem(93007u, "Mini Turret");
		AddItem(10006u, "M.Truck Drop");
		AddItem(92101u, "UAV Lite");
		AddItem(92102u, "Heal UAV Lite");
		AddItem(701u, "Mushroom lv1");
		AddItem(702u, "Mushroom lv2");
		AddItem(703u, "Mushroom lv3");
		AddItem(707u, "Mushroom lv4");
		Categories.Clear();
		Categories.Add(new List<uint>
		{
			2u, 80u, 81u, 82u, 6u, 11u, 63u, 126u, 127u, 12u,
			178u, 179u, 180u, 14u, 70u, 24u, 67u, 130u, 131u, 28u,
			33u, 39u, 46u, 193u, 194u, 195u, 47u, 57u, 73u, 74u
		});
		Categories.Add(new List<uint> { 18u, 26u, 72u, 48u, 89u });
		Categories.Add(new List<uint> { 19u, 71u, 30u, 61u, 122u, 123u, 54u });
		Categories.Add(new List<uint>
		{
			7u, 8u, 60u, 120u, 121u, 13u, 62u, 124u, 125u, 15u,
			32u, 35u, 43u, 49u, 69u, 88u, 228u, 229u, 230u, 150u
		});
		Categories.Add(new List<uint>
		{
			5u, 184u, 185u, 186u, 29u, 41u, 119u, 50u, 86u, 181u,
			182u, 21002u
		});
		Categories.Add(new List<uint> { 4u, 65u, 21u, 64u, 128u, 129u, 45u, 75u, 78u, 197u });
		Categories.Add(new List<uint> { 3u, 56u, 9u, 10u, 20u, 25u, 55u, 58u, 21001u });
		Categories.Add(new List<uint> { 34u, 16u, 17u, 27u, 51u, 53u, 1u });
		Categories.Add(new List<uint> { 23u, 36u, 196u, 100u, 99u, 21006u, 638u });
		Categories.Add(new List<uint> { 201u, 203u, 204u, 205u, 207u });
		Categories.Add(new List<uint> { 102u, 1205u, 213u, 221u, 5601u });
		Categories.Add(new List<uint> { 601u, 603u, 1201u, 1204u, 602u, 608u, 1401u, 617u, 624u });
		Categories.Add(new List<uint>
		{
			301u, 302u, 303u, 309u, 304u, 305u, 306u, 5605u, 401u, 402u,
			403u
		});
		Categories.Add(new List<uint>
		{
			501u, 502u, 503u, 504u, 534u, 535u, 537u, 511u, 512u, 513u,
			518u, 521u, 522u, 523u, 526u, 541u, 542u, 543u, 547u, 3008u,
			5652u
		});
		Categories.Add(new List<uint>
		{
			3001u, 106u, 50010u, 701u, 702u, 703u, 707u, 91007u, 91010u, 91011u,
			91012u, 91053u, 91002u, 93002u, 93007u, 10006u, 92101u, 92102u
		});
		Catalog.Clear();
		for (int i = 0; i < Categories.Count; i++)
		{
			foreach (uint item in Categories[i])
			{
				LootCatalogItem lootCatalogItem = new LootCatalogItem();
				lootCatalogItem.Id = item;
				lootCatalogItem.Category = i;
				lootCatalogItem.Name = (ItemNames.TryGetValue(item, out var value) ? value : $"Item {item}");
				Catalog.Add(lootCatalogItem);
			}
		}
		_isDbBuilt = true;
	}

	public static List<LootCatalogItem> GetCatalog()
	{
		return Catalog;
	}

	public static uint NormalizeItemId(uint rawItemId)
	{
		if (rawItemId == 0)
		{
			return 0u;
		}
		int num = (int)rawItemId;
		if (num < 0)
		{
			num += 25000;
		}
		if (num > 65535)
		{
			int num2 = num & 0xFFFF;
			if (num2 > 0 && num2 < 500)
			{
				return (uint)num2;
			}
		}
		if (num >= 20000 && num < 21000)
		{
			int num3 = num - 20000;
			if (num3 > 0)
			{
				return (uint)num3;
			}
		}
		if (num >= 21000 && num < 22000)
		{
			int num4 = num - 21000;
			if (num4 == 1)
			{
				return 21001u;
			}
			if (num4 == 2)
			{
				return 21002u;
			}
			if (num4 == 6)
			{
				return 21006u;
			}
			if (num4 == 20)
			{
				return 1u;
			}
			if (num4 > 0)
			{
				return (uint)num4;
			}
		}
		return (uint)num;
	}

	public static string GetItemName(uint itemId)
	{
		itemId = NormalizeItemId(itemId);
		if (ItemNames.TryGetValue(itemId, out var value))
		{
			return value;
		}
		return $"Item {itemId}";
	}

	public static int GetItemCategory(uint itemId)
	{
		itemId = NormalizeItemId(itemId);
		for (int i = 0; i < Categories.Count; i++)
		{
			if (Categories[i].Contains(itemId))
			{
				return i;
			}
		}
		return -1;
	}

	public static Vector4 GetItemColor(uint itemId)
	{
		itemId = NormalizeItemId(itemId);
		switch (itemId)
		{
		case 70u:
		case 80u:
		case 81u:
		case 82u:
		case 127u:
		case 180u:
		case 309u:
		case 1205u:
		case 5601u:
			return new Vector4(1f, 0.85f, 0.2f, 1f);
		case 303u:
		case 306u:
		case 403u:
		case 503u:
		case 513u:
		case 523u:
		case 543u:
		case 703u:
			return new Vector4(0.81f, 0.38f, 0.94f, 1f);
		case 302u:
		case 305u:
		case 402u:
		case 502u:
		case 512u:
		case 522u:
		case 542u:
		case 702u:
			return new Vector4(0.18f, 0.65f, 0.94f, 1f);
		case 102u:
		case 301u:
		case 304u:
		case 401u:
		case 501u:
		case 511u:
		case 521u:
		case 541u:
		case 701u:
			return new Vector4(0.36f, 0.9f, 0.46f, 1f);
		case 201u:
		case 203u:
		case 204u:
		case 205u:
		case 207u:
			return new Vector4(0.8f, 0.8f, 0.8f, 1f);
		default:
			return new Vector4(0.95f, 0.95f, 0.95f, 1f);
		}
	}

	private static uint ReadIdAtOffset(uint baseAddr, ulong offset, bool asShort)
	{
		if (baseAddr == 0)
		{
			return 0u;
		}
		if (asShort)
		{
			if (!Memory.Read<short>(baseAddr + offset, out var value) || value == 0)
			{
				return 0u;
			}
			int num = value;
			if (num < 0)
			{
				num += 25000;
			}
			return NormalizeItemId((uint)num);
		}
		if (!Memory.Read<uint>(baseAddr + offset, out var value2) || value2 == 0)
		{
			return 0u;
		}
		return NormalizeItemId(value2);
	}

	private static bool ReadPickupItemId(uint baseObj, out uint itemId)
	{
		itemId = 0u;
		if (baseObj == 0)
		{
			return false;
		}
		uint value = 0u;
		if (Offsets.LootPickupList != 0 && (!Memory.Read<uint>(baseObj + Offsets.LootPickupList, out value) || value == 0) && Offsets.LootPickupListAlt != 0)
		{
			Memory.Read<uint>(baseObj + Offsets.LootPickupListAlt, out value);
		}
		if (value == 0)
		{
			return false;
		}
		if (!Memory.Read<uint>(value + 8, out var value2) || value2 == 0)
		{
			return false;
		}
		if (!Memory.Read<int>(value + 12, out var value3) || value3 <= 0)
		{
			return false;
		}
		value2 += 16;
		if (!Memory.Read<uint>(value2, out var value4) || value4 == 0)
		{
			return false;
		}
		ulong[] array = new ulong[8] { 16uL, 24uL, 36uL, 20uL, 28uL, 32uL, 40uL, 12uL };
		foreach (ulong offset in array)
		{
			itemId = ReadIdAtOffset(value4, offset, asShort: true);
			if (itemId != 0)
			{
				break;
			}
			itemId = ReadIdAtOffset(value4, offset, asShort: false);
			if (itemId != 0)
			{
				break;
			}
		}
		if (itemId == 0)
		{
			array = new ulong[6] { 16uL, 24uL, 36uL, 20uL, 28uL, 32uL };
			foreach (ulong offset2 in array)
			{
				itemId = ReadIdAtOffset(baseObj, offset2, asShort: true);
				if (itemId != 0)
				{
					break;
				}
				itemId = ReadIdAtOffset(baseObj, offset2, asShort: false);
				if (itemId != 0)
				{
					break;
				}
			}
		}
		if (itemId == 0 && Memory.Read<uint>(value4 + Offsets.Weapon, out var value5) && value5 != 0)
		{
			if (Memory.Read<uint>(value5 + Offsets.Weapon_ItemId, out var value6) && value6 != 0)
			{
				itemId = NormalizeItemId(value6);
			}
			if (itemId == 0 && Memory.Read<uint>(value5 + Offsets.WeaponData, out var value7) && value7 != 0)
			{
				array = new ulong[4] { 16uL, 20uL, 24uL, 28uL };
				foreach (ulong num in array)
				{
					if (Memory.Read<uint>(value7 + (uint)(int)num, out var value8) && value8 != 0)
					{
						itemId = NormalizeItemId(value8);
						if (itemId != 0)
						{
							break;
						}
					}
				}
			}
		}
		return itemId != 0;
	}

	private static bool ReadObjectPosition(uint baseObj, out Vector3 pos)
	{
		pos = Vector3.Zero;
		if (baseObj == 0 || Offsets.LootTransformComponent == 0)
		{
			return false;
		}
		if (!Memory.Read<uint>(baseObj + Offsets.LootTransformComponent, out var value) || value == 0)
		{
			return false;
		}
		if (Transform.GetPosition(value, out pos) && pos != Vector3.Zero)
		{
			return true;
		}
		if (Memory.Read<Vector3>(value + 60, out pos))
		{
			return pos != Vector3.Zero;
		}
		return false;
	}

	private static void ScanObjectDict(uint objDict, Vector3 cameraPos, float maxDist, List<GroundLootEntry> outList)
	{
		if (objDict == 0 || outList.Count >= 450 || !Memory.Read<int>(objDict + 16, out var value) || value <= 0 || value > 800 || !Memory.Read<uint>(objDict + 12, out var value2) || value2 == 0)
		{
			return;
		}
		value2 += 16;
		for (int i = 0; i < value; i++)
		{
			if (outList.Count >= 450)
			{
				break;
			}
			uint num = value2 + (uint)(i * 16);
			if (Memory.Read<int>(num, out var value3) && value3 >= 0 && Memory.Read<uint>(num + 12, out var value4) && value4 != 0 && ReadPickupItemId(value4, out var itemId) && ReadObjectPosition(value4, out var pos))
			{
				float num2 = Vector3.Distance(cameraPos, pos);
				if (!(num2 > maxDist) && !(num2 < 0.05f))
				{
					GroundLootEntry item = new GroundLootEntry
					{
						ItemId = itemId,
						Position = pos,
						Distance = num2,
						Name = GetItemName(itemId)
					};
					outList.Add(item);
				}
			}
		}
	}

	public static void ScanGround(uint currentGame, Vector3 cameraPos)
	{
		if (!Config.EspLootEnabled || currentGame == 0)
		{
			lock (_lootLock)
			{
				GroundLoot.Clear();
				return;
			}
		}
		if (Offsets.LevelObjectManager == 0 && Offsets.LevelObjectManagerAlt == 0)
		{
			lock (_lootLock)
			{
				GroundLoot.Clear();
				return;
			}
		}
		List<GroundLootEntry> found = new List<GroundLootEntry>(128);
		float maxDist = Math.Clamp(Config.EspLootMaxDistance, 20f, 500f);
		Action<uint> action = (uint lom) =>
		{
			if (lom != 0 && Memory.Read<uint>(lom + 12, out var value3) && value3 != 0 && Memory.Read<int>(value3 + 16, out var value4) && value4 > 0 && value4 <= 200 && Memory.Read<uint>(value3 + 12, out var value5) && value5 != 0)
			{
				value5 += 16;
				for (int i = 0; i < value4; i++)
				{
					if (found.Count >= 450)
					{
						break;
					}
					uint num = value5 + (uint)(i * 16);
					if (Memory.Read<int>(num, out var value6) && value6 >= 0 && Memory.Read<uint>(num + 12, out var value7) && value7 != 0)
					{
						ScanObjectDict(value7, cameraPos, maxDist, found);
					}
				}
			}
		};
		uint value = 0u;
		uint value2 = 0u;
		if (Offsets.LevelObjectManager != 0)
		{
			Memory.Read<uint>(currentGame + Offsets.LevelObjectManager, out value);
		}
		if (Offsets.LevelObjectManagerAlt != 0)
		{
			Memory.Read<uint>(currentGame + Offsets.LevelObjectManagerAlt, out value2);
		}
		if (value != 0)
		{
			action(value);
		}
		if (value2 != 0 && value2 != value)
		{
			action(value2);
		}
		lock (_lootLock)
		{
			GroundLoot.Clear();
			GroundLoot.AddRange(found);
		}
	}

	public static List<GroundLootEntry> GetGroundLootCopy()
	{
		lock (_lootLock)
		{
			return new List<GroundLootEntry>(GroundLoot);
		}
	}
}
