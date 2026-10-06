using System.Collections.Generic;

namespace StrangeXexternalEsp;

public static class SkinDatabase
{
	public static uint ColorRed = 4281479922u;

	public static Dictionary<uint, string> GunIdToName = new Dictionary<uint, string>
	{
		{ 1u, "Fist" },
		{ 2u, "M4A1" },
		{ 3u, "USP" },
		{ 4u, "AWM" },
		{ 5u, "M1014" },
		{ 6u, "AK47" },
		{ 7u, "UMP" },
		{ 8u, "MP5" },
		{ 9u, "Deagle" },
		{ 10u, "G18" },
		{ 11u, "M14" },
		{ 12u, "SCAR" },
		{ 13u, "VSS" },
		{ 14u, "Groza" },
		{ 15u, "MP40" },
		{ 16u, "Pan" },
		{ 17u, "Knife" },
		{ 18u, "SKS" },
		{ 19u, "M249" },
		{ 20u, "M1873" },
		{ 21u, "Kar98K" },
		{ 24u, "FAMAS" },
		{ 25u, "M500" },
		{ 26u, "SVD" },
		{ 27u, "Bat" },
		{ 28u, "XM8" },
		{ 29u, "SPAS12" },
		{ 30u, "M60" },
		{ 32u, "P90" },
		{ 33u, "AN94" },
		{ 34u, "Katana" },
		{ 35u, "CG15" },
		{ 39u, "Plasma" },
		{ 41u, "M1887" },
		{ 43u, "Thompson" },
		{ 45u, "M82B" },
		{ 46u, "AUG" },
		{ 47u, "Parafal" },
		{ 48u, "Woodpecker" },
		{ 49u, "Vector" },
		{ 50u, "MAG-7" },
		{ 51u, "Scythe" },
		{ 55u, "M1917" },
		{ 56u, "USP-2" },
		{ 57u, "Kingfisher" },
		{ 58u, "Uzi" },
		{ 60u, "MP5-I" },
		{ 61u, "M60-I" },
		{ 63u, "M14-I" },
		{ 65u, "AWM-Y" },
		{ 67u, "FAMAS-I" },
		{ 70u, "Groza X" },
		{ 71u, "M249 X" },
		{ 72u, "SVD-Y" },
		{ 73u, "G36" },
		{ 74u, "G36" },
		{ 75u, "M24" },
		{ 78u, "Heal Sniper" },
		{ 80u, "M4A1-I" },
		{ 81u, "M4A1-II" },
		{ 82u, "M4A1-III" },
		{ 86u, "Charge Gun" },
		{ 88u, "MAC10" },
		{ 89u, "AC80" },
		{ 93u, "Treatment Gun" },
		{ 99u, "Shield Gun" },
		{ 100u, "Flamethrower" },
		{ 119u, "M1887-X" },
		{ 120u, "MP5-II" },
		{ 121u, "MP5-III" },
		{ 122u, "M60-II" },
		{ 123u, "M60-III" },
		{ 126u, "M14-II" },
		{ 127u, "M14-III" },
		{ 129u, "Kar98 2" },
		{ 130u, "FAMAS-II" },
		{ 131u, "FAMAS-III" },
		{ 150u, "Bizon" },
		{ 178u, "SCAR-I" },
		{ 179u, "SCAR-II" },
		{ 180u, "SCAR-III" },
		{ 181u, "Trogon" },
		{ 184u, "M1014-I" },
		{ 185u, "M1014-II" },
		{ 186u, "M1014-III" },
		{ 193u, "AUG-I" },
		{ 194u, "AUG-II" },
		{ 195u, "AUG-III" },
		{ 197u, "VSK94" },
		{ 228u, "MAC10-I" },
		{ 229u, "MAC10-II" },
		{ 230u, "MAC10-III" },
		{ 1204u, "GlooWall" },
		{ 21006u, "Winchester" },
		{ 21007u, "Winchester I" },
		{ 21008u, "Winchester II" },
		{ 21009u, "Winchester III" },
		{ 21020u, "Thompson X" },
		{ 21022u, "Thompson Y" },
		{ 21002u, "M590" }
	};

	public static Dictionary<string, uint> GunNameToId = new Dictionary<string, uint>
	{
		{ "AK47", 6u },
		{ "Fist", 1u },
		{ "M4A1", 2u },
		{ "USP", 3u },
		{ "AWM", 4u },
		{ "M1014", 5u },
		{ "UMP", 7u },
		{ "MP5", 8u },
		{ "Deagle", 9u },
		{ "G18", 10u },
		{ "M14", 11u },
		{ "SCAR", 12u },
		{ "VSS", 13u },
		{ "Groza", 14u },
		{ "MP40", 15u },
		{ "Pan", 16u },
		{ "Knife", 17u },
		{ "SKS", 18u },
		{ "M249", 19u },
		{ "M1873", 20u },
		{ "Kar98K", 21u },
		{ "FAMAS", 24u },
		{ "M500", 25u },
		{ "SVD", 26u },
		{ "Bat", 27u },
		{ "XM8", 28u },
		{ "SPAS12", 29u },
		{ "M60", 30u },
		{ "P90", 32u },
		{ "AN94", 33u },
		{ "Katana", 34u },
		{ "CG15", 35u },
		{ "Plasma", 39u },
		{ "M1887", 41u },
		{ "Thompson", 43u },
		{ "M82B", 45u },
		{ "AUG", 46u },
		{ "Parafal", 47u },
		{ "Woodpecker", 48u },
		{ "Vector", 49u },
		{ "MAG-7", 50u },
		{ "Scythe", 51u },
		{ "M1917", 55u },
		{ "USP-2", 56u },
		{ "Kingfisher", 57u },
		{ "Uzi", 58u },
		{ "MP5-I", 60u },
		{ "M60-I", 61u },
		{ "M14-I", 63u },
		{ "AWM-Y", 65u },
		{ "FAMAS-I", 67u },
		{ "Groza X", 70u },
		{ "M249 X", 71u },
		{ "SVD-Y", 72u },
		{ "G36", 73u },
		{ "M24", 75u },
		{ "Heal Sniper", 78u },
		{ "M4A1-I", 80u },
		{ "M4A1-II", 81u },
		{ "M4A1-III", 82u },
		{ "Charge Gun", 86u },
		{ "MAC10", 88u },
		{ "AC80", 89u },
		{ "Treatment Gun", 93u },
		{ "Shield Gun", 99u },
		{ "Flamethrower", 100u },
		{ "M1887-X", 119u },
		{ "MP5-II", 120u },
		{ "MP5-III", 121u },
		{ "M60-II", 122u },
		{ "M60-III", 123u },
		{ "M14-II", 126u },
		{ "M14-III", 127u },
		{ "Kar98 2", 129u },
		{ "FAMAS-II", 130u },
		{ "FAMAS-III", 131u },
		{ "Bizon", 150u },
		{ "SCAR-I", 178u },
		{ "SCAR-II", 179u },
		{ "SCAR-III", 180u },
		{ "Trogon", 181u },
		{ "M1014-I", 184u },
		{ "M1014-II", 185u },
		{ "M1014-III", 186u },
		{ "AUG-I", 193u },
		{ "AUG-II", 194u },
		{ "AUG-III", 195u },
		{ "VSK94", 197u },
		{ "MAC10-I", 228u },
		{ "MAC10-II", 229u },
		{ "MAC10-III", 230u },
		{ "GlooWall", 1204u },
		{ "Winchester", 21006u },
		{ "Winchester I", 21007u },
		{ "Winchester II", 21008u },
		{ "Winchester III", 21009u },
		{ "Thompson X", 21020u },
		{ "Thompson Y", 21022u },
		{ "M590", 21002u }
	};

	public static Dictionary<uint, string> GunIdToIcon = new Dictionary<uint, string>();

	public static Dictionary<uint, List<SkinInfo>> WeaponSkins = new Dictionary<uint, List<SkinInfo>>
	{
		{
			2u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Infernal Draco",
					SkinID = 907193101u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Infernal Draco",
					SkinID = 907193102u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Infernal Draco",
					SkinID = 907193103u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Infernal Draco",
					SkinID = 907193104u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Infernal Draco",
					SkinID = 907193105u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Infernal Draco",
					SkinID = 907193106u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Infernal Draco",
					SkinID = 907193107u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Griffin's Fury",
					SkinID = 907001315u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Red Griffin",
					SkinID = 907001429u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Blue Griffin",
					SkinID = 907001430u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Purple Griffin",
					SkinID = 907001431u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Genos",
					SkinID = 907002534u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Infernal Netherworld",
					SkinID = 907002612u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Shadow Netherworld",
					SkinID = 907002613u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Glacier Netherworld",
					SkinID = 907002614u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Venom Netherworld",
					SkinID = 907002615u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Griffin's Fury",
					SkinID = 907101315u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Griffin's Fury",
					SkinID = 907101325u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Red Griffin",
					SkinID = 907101429u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Blue Griffin",
					SkinID = 907101430u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Purple Griffin",
					SkinID = 907101431u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Genos",
					SkinID = 907102534u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Infernal Netherworld",
					SkinID = 907102612u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Shadow Netherworld",
					SkinID = 907102613u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Glacier Netherworld",
					SkinID = 907102614u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Venom Netherworld",
					SkinID = 907102615u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Sunrise Realm",
					SkinID = 907104202u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Stormy Ascent",
					SkinID = 907104729u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Naruto Theme",
					SkinID = 907104744u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Genos",
					SkinID = 907202534u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Infernal Netherworld",
					SkinID = 907202612u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Shadow Netherworld",
					SkinID = 907202613u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Glacier Netherworld",
					SkinID = 907202614u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Venom Netherworld",
					SkinID = 907202615u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Star General",
					SkinID = 907000071u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Wild Carnival",
					SkinID = 907001306u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Pink Laminate",
					SkinID = 907001406u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cataclysm",
					SkinID = 907001508u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Star Soul",
					SkinID = 907001509u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Scorching Sands",
					SkinID = 907001614u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Flaming Skull",
					SkinID = 907001819u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "FFCS",
					SkinID = 907002414u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "S19 Exclusive: Kelly \"the Swift\"",
					SkinID = 907002521u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Star General",
					SkinID = 907100071u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Winter Warrior",
					SkinID = 907100079u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Wild Carnival",
					SkinID = 907101306u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Wild Carnival",
					SkinID = 907101324u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Pink Laminate",
					SkinID = 907101406u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Pink Laminate",
					SkinID = 907101435u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cataclysm",
					SkinID = 907101508u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Star Soul",
					SkinID = 907101509u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cataclysm",
					SkinID = 907101538u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Star Soul",
					SkinID = 907101539u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Scorching Sands",
					SkinID = 907101614u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Flaming Skull",
					SkinID = 907101819u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "FFCS",
					SkinID = 907102414u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "S19 Exclusive: Kelly \"the Swift\"",
					SkinID = 907102521u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Feline's Burst",
					SkinID = 907104223u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Zombie Attack",
					SkinID = 907104316u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Goal Kick",
					SkinID = 907105308u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Scorching Sands",
					SkinID = 907201614u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Flaming Skull",
					SkinID = 907201819u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "FFCS",
					SkinID = 907202414u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "S19 Exclusive: Kelly \"the Swift\"",
					SkinID = 907202521u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Winter Warrior",
					SkinID = 907000079u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Flaming Wolf",
					SkinID = 907000087u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Ice Blue",
					SkinID = 907001421u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death Wish",
					SkinID = 907001502u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Scorpion",
					SkinID = 907001628u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Artificial Intelligence",
					SkinID = 907001722u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Deadly Bat",
					SkinID = 907001810u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Vandal Revolt",
					SkinID = 907002022u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Master of Minds",
					SkinID = 907002404u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Dr. Nanotech",
					SkinID = 907002518u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Flaming Wolf",
					SkinID = 907100087u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Ice Blue",
					SkinID = 907101421u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death Wish",
					SkinID = 907101502u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death Wish",
					SkinID = 907101532u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Scorpion",
					SkinID = 907101628u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Artificial Intelligence",
					SkinID = 907101722u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Deadly Bat",
					SkinID = 907101810u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Vandal Revolt",
					SkinID = 907102022u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Master of Minds",
					SkinID = 907102404u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Dr. Nanotech",
					SkinID = 907102518u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "The Golden M4A1",
					SkinID = 907103010u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Skull Punker",
					SkinID = 907103032u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Sterling Futurnetic",
					SkinID = 907103502u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Waggor Swagger",
					SkinID = 907104540u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Waggor Swagger",
					SkinID = 907104541u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Trouble Academy",
					SkinID = 907105022u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S36 Exclusive: Emoji",
					SkinID = 907105220u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136054u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Scorpion",
					SkinID = 907201628u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Artificial Intelligence",
					SkinID = 907201722u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Deadly Bat",
					SkinID = 907201810u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Vandal Revolt",
					SkinID = 907202022u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Master of Minds",
					SkinID = 907202404u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Dr. Nanotech",
					SkinID = 907202518u,
					Color = 4294901760u
				}
			}
		},
		{
			3u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Ebony & Ivory",
					SkinID = 907103858u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Ivory",
					SkinID = 907103860u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Fuji Folklore",
					SkinID = 907002501u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Fuji Folklore",
					SkinID = 907102501u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Sharp Tactician",
					SkinID = 907104913u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Sharp Tactician",
					SkinID = 907104933u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Fuji Folklore",
					SkinID = 907202501u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "The Falconer",
					SkinID = 907102722u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Fire Bones",
					SkinID = 907103125u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Copper Light",
					SkinID = 907103201u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "2",
					SkinID = 907104021u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Poison Tooth",
					SkinID = 907104413u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Poison Teeth",
					SkinID = 907104414u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907105331u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136084u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Waggor's Wonder",
					SkinID = 907104059u,
					Color = 4278255360u
				}
			}
		},
		{
			4u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Duke Swallowtail",
					SkinID = 907002217u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "F┼½rinkazan",
					SkinID = 907002833u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Duke Swallowtail",
					SkinID = 907102217u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "F┼½rinkazan",
					SkinID = 907102833u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Metal Wings",
					SkinID = 907103233u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Crimson Firehorn",
					SkinID = 907103508u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Mossy Vinehorn",
					SkinID = 907103509u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Iron Etherhorn",
					SkinID = 907103510u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Titanium Warhorn",
					SkinID = 907103511u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Duke Swallowtail",
					SkinID = 907202217u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Lucky Koi",
					SkinID = 907002616u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Lucky Koi",
					SkinID = 907102616u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Wavebreaker Kaze",
					SkinID = 907103003u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Breath of Silence",
					SkinID = 907104411u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Aurora's Shade",
					SkinID = 907104712u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Lucky Koi",
					SkinID = 907202616u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Hunter's Eye",
					SkinID = 907001608u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Old Fashioned",
					SkinID = 907001717u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Tagger's Revolt",
					SkinID = 907001913u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Agent Paws",
					SkinID = 907002008u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Valentines",
					SkinID = 907002017u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Vandal Revolt",
					SkinID = 907002024u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Justice Fighter",
					SkinID = 907002317u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Booyah Day",
					SkinID = 907002417u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Hunter's Eye",
					SkinID = 907101608u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Old Fashioned",
					SkinID = 907101717u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Tagger's Revolt",
					SkinID = 907101913u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Agent Paws",
					SkinID = 907102008u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Valentines",
					SkinID = 907102017u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Vandal Revolt",
					SkinID = 907102024u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Justice Fighter",
					SkinID = 907102317u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Booyah Day",
					SkinID = 907102417u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Xtreme Adventure",
					SkinID = 907102728u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Skull Punker",
					SkinID = 907103033u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Haunt's Outrage",
					SkinID = 907104016u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S32 Exclusive: Ash",
					SkinID = 907105039u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136069u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Hunter's Eye",
					SkinID = 907201608u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Old Fashioned",
					SkinID = 907201717u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Tagger's Revolt",
					SkinID = 907201913u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Agent Paws",
					SkinID = 907202008u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Valentines",
					SkinID = 907202017u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Vandal Revolt",
					SkinID = 907202024u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Justice Fighter",
					SkinID = 907202317u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Booyah Day",
					SkinID = 907202417u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Interstellar Igniter",
					SkinID = 907104217u,
					Color = 4278255360u
				},
				new SkinInfo
				{
					Name = "Frostfire Snip",
					SkinID = 907104222u,
					Color = 4281571839u
				},
				new SkinInfo
				{
					Name = "Bamboo Warrior",
					SkinID = 907105014u,
					Color = 4281571839u
				}
			}
		},
		{
			5u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Green Flame Draco",
					SkinID = 907192701u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Green Flame Draco",
					SkinID = 907192702u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Green Flame Draco",
					SkinID = 907192703u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Green Flame Draco",
					SkinID = 907192704u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Green Flame Draco",
					SkinID = 907192705u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Green Flame Draco",
					SkinID = 907192706u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Green Flame Draco",
					SkinID = 907192707u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Scorpio Shatter",
					SkinID = 907194101u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Scorpio Shatter",
					SkinID = 907194102u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Scorpio Shatter",
					SkinID = 907194103u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Scorpio Shatter",
					SkinID = 907194104u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Scorpio Shatter",
					SkinID = 907194105u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Scorpio Shatter",
					SkinID = 907194106u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Scorpio Shatter",
					SkinID = 907194107u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Scorpio Shatter",
					SkinID = 907194108u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Attack on Titan",
					SkinID = 907002105u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Red",
					SkinID = 907002228u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Dark",
					SkinID = 907002229u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Green",
					SkinID = 907002230u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Gold",
					SkinID = 907002231u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Attack on Titan",
					SkinID = 907102105u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Red",
					SkinID = 907102228u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Dark",
					SkinID = 907102229u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Green",
					SkinID = 907102230u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Gold",
					SkinID = 907102231u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Attack on Titan",
					SkinID = 907202105u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Red",
					SkinID = 907202228u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Dark",
					SkinID = 907202229u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Green",
					SkinID = 907202230u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Gold",
					SkinID = 907202231u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Winterlands",
					SkinID = 907000073u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cataclysm",
					SkinID = 907001504u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Enhanced Armor",
					SkinID = 907001610u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Wasteland",
					SkinID = 907002011u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Winterlands",
					SkinID = 907100073u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cataclysm",
					SkinID = 907101504u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cataclysm",
					SkinID = 907101534u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Enhanced Armor",
					SkinID = 907101610u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Wasteland",
					SkinID = 907102011u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Rave Skater",
					SkinID = 907103318u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Darkrose Lurker",
					SkinID = 907103518u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Defier's Anthem",
					SkinID = 907103830u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Big Bad Woof",
					SkinID = 907105115u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Enhanced Armor",
					SkinID = 907201610u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Wasteland",
					SkinID = 907202011u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Violet Terror",
					SkinID = 907000084u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Shark Attack",
					SkinID = 907001412u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Ice Blue",
					SkinID = 907001422u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death's Eye",
					SkinID = 907001514u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Artificial Intelligence",
					SkinID = 907001724u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Demolitionist",
					SkinID = 907001922u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Game Streamer",
					SkinID = 907002222u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Violet Terror",
					SkinID = 907100084u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Shark Attack",
					SkinID = 907101412u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Ice Blue",
					SkinID = 907101422u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death's Eye",
					SkinID = 907101514u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death's Eye",
					SkinID = 907101544u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Artificial Intelligence",
					SkinID = 907101724u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Demolitionist",
					SkinID = 907101922u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Game Streamer",
					SkinID = 907102222u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "The Golden M1014",
					SkinID = 907102826u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Roaring Gunfighter",
					SkinID = 907103232u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Demolitionist (30 Days)",
					SkinID = 907104498u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S42 Exclusive: Combustion",
					SkinID = 907104625u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136039u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Artificial Intelligence",
					SkinID = 907201724u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Demolitionist",
					SkinID = 907201922u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Game Streamer",
					SkinID = 907202222u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Waggor's Wonder",
					SkinID = 907104062u,
					Color = 4278255360u
				},
				new SkinInfo
				{
					Name = "Zombie Attack",
					SkinID = 907104243u,
					Color = 4278255360u
				},
				new SkinInfo
				{
					Name = "Undersea Rampage",
					SkinID = 907105302u,
					Color = 4281571839u
				},
				new SkinInfo
				{
					Name = "Dog At Hand",
					SkinID = 907136038u,
					Color = 4281571839u
				}
			}
		},
		{
			6u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Blue Flame Draco MAX",
					SkinID = 907092407u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Unicorn's Rage (Lava)",
					SkinID = 907001417u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Unicorn's Rage (Ice Age)",
					SkinID = 907001418u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Unicorn's Rage (Golden Era)",
					SkinID = 907001419u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Unicorn's Rage (Violet)",
					SkinID = 907001420u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Unicorn's Rage (Lava)",
					SkinID = 907101417u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Unicorn's Rage (Ice Age)",
					SkinID = 907101418u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Unicorn's Rage (Golden Era)",
					SkinID = 907101419u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Unicorn's Rage (Violet)",
					SkinID = 907101420u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Unicorn's Rage (Lava)",
					SkinID = 907101436u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Unicorn's Rage (Ice Age)",
					SkinID = 907101437u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Unicorn's Rage (Golden Era)",
					SkinID = 907101438u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Unicorn's Rage (Violet)",
					SkinID = 907101439u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Winterlands",
					SkinID = 907000074u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Water Balloon",
					SkinID = 907001405u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Unicorn's Rage",
					SkinID = 907001416u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Flaming Red",
					SkinID = 907001611u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Skull Hunter",
					SkinID = 907001801u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Pumpkin Flames",
					SkinID = 907001806u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Valentines",
					SkinID = 907002016u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "S20 Exclusive: Moco",
					SkinID = 907002633u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Winterlands",
					SkinID = 907100074u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Water Balloon",
					SkinID = 907101405u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Unicorn's Rage",
					SkinID = 907101416u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Water Balloon",
					SkinID = 907101434u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Flaming Red",
					SkinID = 907101611u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Skull Hunter",
					SkinID = 907101801u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Pumpkin Flames",
					SkinID = 907101806u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Valentines",
					SkinID = 907102016u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "S20 Exclusive: Moco",
					SkinID = 907102633u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Wrathful Wackadoo",
					SkinID = 907103338u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Sound Crafter",
					SkinID = 907103401u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "City on Fire",
					SkinID = 907104006u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Akatsuki Theme",
					SkinID = 907105004u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Pigment Splash",
					SkinID = 907136011u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Flaming Red",
					SkinID = 907201611u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Skull Hunter",
					SkinID = 907201801u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Pumpkin Flames",
					SkinID = 907201806u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Valentines",
					SkinID = 907202016u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "S20 Exclusive: Moco",
					SkinID = 907202633u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Digital Invasion",
					SkinID = 907001307u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Imperial Rome",
					SkinID = 907001311u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Red Samurai",
					SkinID = 907001407u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Gold Coated",
					SkinID = 907001425u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death's Eye",
					SkinID = 907001513u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Urban Rager",
					SkinID = 907002018u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Storm Whisperer",
					SkinID = 907002026u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Private Eye",
					SkinID = 907002227u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Justice Fighter",
					SkinID = 907002316u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Legendary Cobra",
					SkinID = 907002609u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Digital Invasion",
					SkinID = 907101307u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Imperial Rome",
					SkinID = 907101311u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Red Samurai",
					SkinID = 907101407u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Gold Coated",
					SkinID = 907101425u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death's Eye",
					SkinID = 907101513u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death's Eye",
					SkinID = 907101543u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Urban Rager",
					SkinID = 907102018u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Storm Whisperer",
					SkinID = 907102026u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Private Eye",
					SkinID = 907102227u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Justice Fighter",
					SkinID = 907102316u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Legendary Cobra",
					SkinID = 907102609u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Soloist",
					SkinID = 907102933u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Emerald Power",
					SkinID = 907103317u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "The Golden AK47",
					SkinID = 907103717u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Zombie Attack",
					SkinID = 907104102u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136050u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Urban Rager",
					SkinID = 907202018u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Storm Whisperer",
					SkinID = 907202026u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Private Eye",
					SkinID = 907202227u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Justice Fighter",
					SkinID = 907202316u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Legendary Cobra",
					SkinID = 907202609u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Waggor's Wonder",
					SkinID = 907104054u,
					Color = 4278255360u
				},
				new SkinInfo
				{
					Name = "Paradox Enforcer",
					SkinID = 907104406u,
					Color = 4281571839u
				}
			}
		},
		{
			7u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "BOOYAH DAY 2021",
					SkinID = 907193001u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "BOOYAH DAY 2021",
					SkinID = 907193002u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "BOOYAH DAY 2021",
					SkinID = 907193003u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "BOOYAH DAY 2021",
					SkinID = 907193004u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "BOOYAH DAY 2021",
					SkinID = 907193005u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "BOOYAH DAY 2021",
					SkinID = 907193006u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "BOOYAH DAY 2021",
					SkinID = 907193007u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Grizzly Papercut",
					SkinID = 907102708u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Tiger Papercut",
					SkinID = 907102709u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Zebra Papercut",
					SkinID = 907102710u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Gators Papercut",
					SkinID = 907102711u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Dawn Voyage",
					SkinID = 907104239u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Trumpet",
					SkinID = 907104703u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Pumpkin Flame",
					SkinID = 907000050u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cataclysm",
					SkinID = 907001503u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Enhanced Armor",
					SkinID = 907001609u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Art of War",
					SkinID = 907001817u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Wilderness Hunter",
					SkinID = 907002426u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Booyah Day",
					SkinID = 907092414u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Pumpkin Flame",
					SkinID = 907100050u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cataclysm",
					SkinID = 907101503u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cataclysm",
					SkinID = 907101533u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Enhanced Armor",
					SkinID = 907101609u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Art of War",
					SkinID = 907101817u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Wilderness Hunter",
					SkinID = 907102426u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Digital Dasher",
					SkinID = 907103414u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Creative Fighters",
					SkinID = 907103501u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Interstellar Impulse",
					SkinID = 907104216u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Mecha Attack",
					SkinID = 907104412u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Sea of Hues",
					SkinID = 907105221u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Booyah Day",
					SkinID = 907192414u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Enhanced Armor",
					SkinID = 907201609u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Art of War",
					SkinID = 907201817u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Wilderness Hunter",
					SkinID = 907202426u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Booyah Day",
					SkinID = 907292414u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Violet Terror",
					SkinID = 907000082u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Deadly Panther",
					SkinID = 907001444u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Lively Beast",
					SkinID = 907001604u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "BOOYAH",
					SkinID = 907001623u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Blood Hockey",
					SkinID = 907001804u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pumpkin Flames",
					SkinID = 907001807u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Head",
					SkinID = 907002010u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Kpop Stardom",
					SkinID = 907002119u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Private Eye",
					SkinID = 907002225u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Justice Fighter",
					SkinID = 907002319u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Booyah Day",
					SkinID = 907092411u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Booyah Day",
					SkinID = 907092412u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Booyah Day",
					SkinID = 907092413u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Violet Terror",
					SkinID = 907100082u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Deadly Panther",
					SkinID = 907101444u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Lively Beast",
					SkinID = 907101604u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "BOOYAH",
					SkinID = 907101623u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Blood Hockey",
					SkinID = 907101804u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pumpkin Flames",
					SkinID = 907101807u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Head",
					SkinID = 907102010u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Kpop Stardom",
					SkinID = 907102119u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Private Eye",
					SkinID = 907102225u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Justice Fighter",
					SkinID = 907102319u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Guns for Hire",
					SkinID = 907102802u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "The Golden UMP",
					SkinID = 907103127u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Copper Light",
					SkinID = 907103202u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S32 Exclusive: Shirou",
					SkinID = 907103718u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S29 Exclusive: Delivery",
					SkinID = 907104725u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Nightslayer Grizzly",
					SkinID = 907136018u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136046u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Booyah Day",
					SkinID = 907192411u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Booyah Day",
					SkinID = 907192412u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Booyah Day",
					SkinID = 907192413u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Lively Beast",
					SkinID = 907201604u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "BOOYAH",
					SkinID = 907201623u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Blood Hockey",
					SkinID = 907201804u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pumpkin Flames",
					SkinID = 907201807u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Head",
					SkinID = 907202010u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Kpop Stardom",
					SkinID = 907202119u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Private Eye",
					SkinID = 907202225u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Justice Fighter",
					SkinID = 907202319u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Booyah Day",
					SkinID = 907292411u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Booyah Day",
					SkinID = 907292412u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Booyah Day",
					SkinID = 907292413u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Waggor's Wonder",
					SkinID = 907104055u,
					Color = 4278255360u
				},
				new SkinInfo
				{
					Name = "Zombie Attack",
					SkinID = 907104242u,
					Color = 4278255360u
				}
			}
		},
		{
			8u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Platinum Divinity",
					SkinID = 907193301u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Platinum Divinity",
					SkinID = 907193302u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Platinum Divinity",
					SkinID = 907193303u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Platinum Divinity",
					SkinID = 907193304u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Platinum Divinity",
					SkinID = 907193305u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Platinum Divinity",
					SkinID = 907193306u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Platinum Divinity",
					SkinID = 907193307u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Spinning Bird",
					SkinID = 907002834u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Spinning Bird",
					SkinID = 907102834u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Frozen Lava",
					SkinID = 907102911u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Meta Lava",
					SkinID = 907102912u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Jungle Lava",
					SkinID = 907102913u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Rogue Lava",
					SkinID = 907102914u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Aurora Oni",
					SkinID = 907103113u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Cyan Ghost",
					SkinID = 907103451u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Crowned Glory",
					SkinID = 907105138u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Champion Boxer",
					SkinID = 907002408u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cyber Bounty Hunter",
					SkinID = 907002525u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "The Golden MP5",
					SkinID = 907002632u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Champion Boxer",
					SkinID = 907102408u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cyber Bounty Hunter",
					SkinID = 907102525u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "The Golden MP5",
					SkinID = 907102632u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Mr. Nutcracker",
					SkinID = 907102716u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Achiever",
					SkinID = 907103818u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Sublime Scorpio",
					SkinID = 907103908u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Troublemaker",
					SkinID = 907105019u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Champion Boxer",
					SkinID = 907202408u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cyber Bounty Hunter",
					SkinID = 907202525u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "The Golden MP5",
					SkinID = 907202632u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Imperial Rome",
					SkinID = 907001312u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Blood Red",
					SkinID = 907001317u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Red Samurai",
					SkinID = 907001409u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Star Soul",
					SkinID = 907001510u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death's Eye",
					SkinID = 907001516u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pink Devil",
					SkinID = 907001625u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Scorpion",
					SkinID = 907001629u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Vampire",
					SkinID = 907001706u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Old Fashioned",
					SkinID = 907001718u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Demolitionist",
					SkinID = 907001921u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Hipster Bunny",
					SkinID = 907002108u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Master of Minds",
					SkinID = 907002402u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Imperial Rome",
					SkinID = 907101312u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Blood Red",
					SkinID = 907101317u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Red Samurai",
					SkinID = 907101409u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Star Soul",
					SkinID = 907101510u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death's Eye",
					SkinID = 907101516u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Star Soul",
					SkinID = 907101540u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death's Eye",
					SkinID = 907101546u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pink Devil",
					SkinID = 907101625u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Scorpion",
					SkinID = 907101629u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Vampire",
					SkinID = 907101706u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Old Fashioned",
					SkinID = 907101718u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Demolitionist",
					SkinID = 907101921u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Hipster Bunny",
					SkinID = 907102108u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Master of Minds",
					SkinID = 907102402u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Evil Enchanted",
					SkinID = 907102703u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Beastly Wrath",
					SkinID = 907103015u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Vampire Malevolence",
					SkinID = 907103217u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S40 Exclusive: Goosy",
					SkinID = 907104519u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Shadow Ops",
					SkinID = 907104840u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S35 Exclusive: Fin",
					SkinID = 907105218u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Blood Red",
					SkinID = 907111317u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pigment Splash",
					SkinID = 907136012u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136044u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pink Devil",
					SkinID = 907201625u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Scorpion",
					SkinID = 907201629u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Vampire",
					SkinID = 907201706u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Old Fashioned",
					SkinID = 907201718u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Demolitionist",
					SkinID = 907201921u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Hipster Bunny",
					SkinID = 907202108u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Master of Minds",
					SkinID = 907202402u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Candy Bunny",
					SkinID = 907103808u,
					Color = 4281571839u
				}
			}
		},
		{
			10u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Ultimate Achiever",
					SkinID = 907194001u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Ultimate Achiever",
					SkinID = 907194002u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Ultimate Achiever",
					SkinID = 907194003u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Ultimate Achiever",
					SkinID = 907194004u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Ultimate Achiever",
					SkinID = 907194005u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Ultimate Achiever",
					SkinID = 907194006u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Ultimate Achiever",
					SkinID = 907194007u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Sharp Aim",
					SkinID = 907104609u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Anubis Legend II",
					SkinID = 907002302u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "The Golden G18",
					SkinID = 907002321u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Anubis Legend II",
					SkinID = 907102302u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "The Golden G18",
					SkinID = 907102321u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Crystal Shatter",
					SkinID = 907104301u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Anubis Legend II",
					SkinID = 907202302u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "The Golden G18",
					SkinID = 907202321u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cosmic Bounty Hunter",
					SkinID = 907002531u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Cosmic Bounty Hunter",
					SkinID = 907102531u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Persia Prowess",
					SkinID = 907102721u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Ice Bones",
					SkinID = 907103122u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Hypercore Blues",
					SkinID = 907103315u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Lava Lustre",
					SkinID = 907103521u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136085u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Cosmic Bounty Hunter",
					SkinID = 907202531u,
					Color = 4294901760u
				}
			}
		},
		{
			12u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907092501u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907092502u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907092503u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907092504u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907092505u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907092506u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907092507u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907192501u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907192502u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907192503u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907192504u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907192505u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907192506u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907192507u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Goldglare Phantom",
					SkinID = 907193601u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Goldglare Phantom",
					SkinID = 907193602u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Goldglare Phantom",
					SkinID = 907193603u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Goldglare Phantom",
					SkinID = 907193604u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Goldglare Phantom",
					SkinID = 907193605u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Goldglare Phantom",
					SkinID = 907193606u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Goldglare Phantom",
					SkinID = 907193607u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Goldglare Phantom",
					SkinID = 907193608u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907292501u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907292502u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907292503u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907292504u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907292505u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907292506u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907292507u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Ultimate Titan",
					SkinID = 907001517u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Inferno",
					SkinID = 907001518u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Paradise",
					SkinID = 907001519u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "The Beast",
					SkinID = 907001520u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Golden Strike",
					SkinID = 907001521u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Ultimate Titan",
					SkinID = 907101517u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Inferno",
					SkinID = 907101518u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Paradise",
					SkinID = 907101519u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "The Beast",
					SkinID = 907101520u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Golden Strike",
					SkinID = 907101521u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Ultimate Titan",
					SkinID = 907101547u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Inferno",
					SkinID = 907101548u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Paradise",
					SkinID = 907101549u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "The Beast",
					SkinID = 907101550u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Golden Strike",
					SkinID = 907101551u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Glistening Daystar",
					SkinID = 907104307u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Racer",
					SkinID = 907000034u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cupid",
					SkinID = 907001304u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Blood Moon",
					SkinID = 907001404u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Water Elemental",
					SkinID = 907001613u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Phantom Assassin",
					SkinID = 907001703u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Aurous Dragon",
					SkinID = 907001908u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Mystic Seeker",
					SkinID = 907002212u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Racer",
					SkinID = 907100034u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Winter Warrior",
					SkinID = 907100080u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cupid",
					SkinID = 907101304u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cupid",
					SkinID = 907101322u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Blood Moon",
					SkinID = 907101404u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Blood Moon",
					SkinID = 907101433u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Water Elemental",
					SkinID = 907101613u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Phantom Assassin",
					SkinID = 907101703u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Aurous Dragon",
					SkinID = 907101908u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Mystic Seeker",
					SkinID = 907102212u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Haven Warrior",
					SkinID = 907103301u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Nightfire Kami",
					SkinID = 907103814u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Zombie Attack",
					SkinID = 907104101u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Stitched Tailor",
					SkinID = 907104705u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Scarlet Nimbus",
					SkinID = 907104815u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Scarlet Ash",
					SkinID = 907105025u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Scarlet Ash",
					SkinID = 907105035u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Pearlescent",
					SkinID = 907105108u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Shadow of Scars",
					SkinID = 907105209u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Scarlet Romance",
					SkinID = 907105213u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Water Elemental",
					SkinID = 907201613u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Phantom Assassin",
					SkinID = 907201703u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Aurous Dragon",
					SkinID = 907201908u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Mystic Seeker",
					SkinID = 907202212u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Winter Warrior",
					SkinID = 907000080u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Violet Terror",
					SkinID = 907000081u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Special Forces",
					SkinID = 907001300u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Shark Attack",
					SkinID = 907001411u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Deadly Panther",
					SkinID = 907001443u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mad Samurai",
					SkinID = 907001524u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pink Devil",
					SkinID = 907001624u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Old Fashioned",
					SkinID = 907001716u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Party Animal",
					SkinID = 907002421u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Violet Terror",
					SkinID = 907100081u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Special Forces",
					SkinID = 907101300u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Shark Attack",
					SkinID = 907101411u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Deadly Panther",
					SkinID = 907101443u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mad Samurai",
					SkinID = 907101524u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mad Samurai",
					SkinID = 907101554u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pink Devil",
					SkinID = 907101624u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Old Fashioned",
					SkinID = 907101716u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Party Animal",
					SkinID = 907102421u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S22 Exclusive: Maxim",
					SkinID = 907102734u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Scarecrow",
					SkinID = 907102804u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Scarlet Groom",
					SkinID = 907102823u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Royal Warrior",
					SkinID = 907103007u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "The Golden SCAR",
					SkinID = 907103327u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Gusto Freerunner",
					SkinID = 907103553u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Scarlet Nimbus",
					SkinID = 907104816u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136051u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pink Devil",
					SkinID = 907201624u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Old Fashioned",
					SkinID = 907201716u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Party Animal",
					SkinID = 907202421u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Waggor's Wonder",
					SkinID = 907104053u,
					Color = 4278255360u
				},
				new SkinInfo
				{
					Name = "Monster Attack",
					SkinID = 907104131u,
					Color = 4281571839u
				}
			}
		},
		{
			13u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Glyph Grip",
					SkinID = 907104631u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Sweet Takedown",
					SkinID = 907105310u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Wound Strokes",
					SkinID = 907104430u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Tiger Boxer",
					SkinID = 907104909u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Fiery Battle",
					SkinID = 907105224u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Star General",
					SkinID = 907000072u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Dragon's Eye",
					SkinID = 907001402u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Shark Attack",
					SkinID = 907001414u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Metallic",
					SkinID = 907001441u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death's Eye",
					SkinID = 907001515u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Clandestine Impulse",
					SkinID = 907001904u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Loose Cannon",
					SkinID = 907001924u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Vandal Revolt",
					SkinID = 907002025u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Victory Wings",
					SkinID = 907002202u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Private Eye",
					SkinID = 907002226u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Rapper Underworld",
					SkinID = 907002310u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Hurricane Delivery",
					SkinID = 907002627u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Star General",
					SkinID = 907100072u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Dragon's Eye",
					SkinID = 907101402u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Shark Attack",
					SkinID = 907101414u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Metallic",
					SkinID = 907101441u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Metallic",
					SkinID = 907101446u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death's Eye",
					SkinID = 907101515u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death's Eye",
					SkinID = 907101545u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Clandestine Impulse",
					SkinID = 907101904u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Loose Cannon",
					SkinID = 907101924u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Vandal Revolt",
					SkinID = 907102025u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Victory Wings",
					SkinID = 907102202u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Private Eye",
					SkinID = 907102226u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Rapper Underworld",
					SkinID = 907102310u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Hurricane Delivery",
					SkinID = 907102627u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Checkered Knight",
					SkinID = 907103307u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Heartseeker",
					SkinID = 907103811u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136049u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Clandestine Impulse",
					SkinID = 907201904u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Loose Cannon",
					SkinID = 907201924u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Vandal Revolt",
					SkinID = 907202025u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Victory Wings",
					SkinID = 907202202u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Private Eye",
					SkinID = 907202226u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Rapper Underworld",
					SkinID = 907202310u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Hurricane Delivery",
					SkinID = 907202627u,
					Color = 4294901760u
				}
			}
		},
		{
			14u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Bang! Popblaster",
					SkinID = 907194111u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Bang! Popblaster",
					SkinID = 907194112u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Bang! Popblaster",
					SkinID = 907194113u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Bang! Popblaster",
					SkinID = 907194114u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Bang! Popblaster",
					SkinID = 907194115u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Bang! Popblaster",
					SkinID = 907194116u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Bang! Popblaster",
					SkinID = 907194117u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Jewel Mystified",
					SkinID = 907103206u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Flames Enchanted",
					SkinID = 907103207u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Airburst Entranced",
					SkinID = 907103208u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Thunder Electrified",
					SkinID = 907103209u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Bony Tunes",
					SkinID = 907104610u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Tagger's Revolt",
					SkinID = 907001912u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Great Plunder",
					SkinID = 907002312u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Tagger's Revolt",
					SkinID = 907101912u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Great Plunder",
					SkinID = 907102312u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Operano Sheng",
					SkinID = 907102920u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Phantom Predator",
					SkinID = 907103030u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Golden Roar",
					SkinID = 907103111u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Sterling Futurnetic",
					SkinID = 907103514u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Heartseeker",
					SkinID = 907103817u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Poppin' Shootin'",
					SkinID = 907104017u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Sasuke Theme",
					SkinID = 907104743u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Night Spookster",
					SkinID = 907105113u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Tagger's Revolt",
					SkinID = 907201912u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Great Plunder",
					SkinID = 907202312u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Winterlands",
					SkinID = 907000075u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "BOOYAH",
					SkinID = 907001622u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Swagger Ownage",
					SkinID = 907001902u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "FFCS",
					SkinID = 907002415u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Winterlands",
					SkinID = 907100075u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "BOOYAH",
					SkinID = 907101622u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Swagger Ownage",
					SkinID = 907101902u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "FFCS",
					SkinID = 907102415u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S24 Exclusive: Wrathful",
					SkinID = 907104321u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136057u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "BOOYAH",
					SkinID = 907201622u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Swagger Ownage",
					SkinID = 907201902u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "FFCS",
					SkinID = 907202415u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Reel On",
					SkinID = 907104137u,
					Color = 4278255360u
				},
				new SkinInfo
				{
					Name = "Runestone Sigil",
					SkinID = 907103959u,
					Color = 4281571839u
				},
				new SkinInfo
				{
					Name = "Yuji Itadori",
					SkinID = 907105201u,
					Color = 4281571839u
				}
			}
		},
		{
			15u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Predatory Cobra",
					SkinID = 907092601u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Predatory Cobra",
					SkinID = 907092602u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Predatory Cobra",
					SkinID = 907092603u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Predatory Cobra",
					SkinID = 907092604u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Predatory Cobra",
					SkinID = 907092605u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Predatory Cobra",
					SkinID = 907092606u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Predatory Cobra",
					SkinID = 907092607u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Predatory Cobra",
					SkinID = 907192601u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Predatory Cobra",
					SkinID = 907192602u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Predatory Cobra",
					SkinID = 907192603u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Predatory Cobra",
					SkinID = 907192604u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Predatory Cobra",
					SkinID = 907192605u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Predatory Cobra",
					SkinID = 907192606u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Predatory Cobra",
					SkinID = 907192607u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Chromasonic",
					SkinID = 907194201u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Chromasonic",
					SkinID = 907194202u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Chromasonic",
					SkinID = 907194203u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Chromasonic",
					SkinID = 907194204u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Chromasonic",
					SkinID = 907194205u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Chromasonic",
					SkinID = 907194206u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Chromasonic",
					SkinID = 907194207u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Chromasonic",
					SkinID = 907194208u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Predatory Cobra",
					SkinID = 907292601u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Predatory Cobra",
					SkinID = 907292602u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Predatory Cobra",
					SkinID = 907292603u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Predatory Cobra",
					SkinID = 907292604u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Predatory Cobra",
					SkinID = 907292605u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Predatory Cobra",
					SkinID = 907292606u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Predatory Cobra",
					SkinID = 907292607u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Blazing Heart",
					SkinID = 907002001u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Eternal Diamond",
					SkinID = 907002002u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Flashing Spade",
					SkinID = 907002003u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Dreamy Club",
					SkinID = 907002004u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Royal Flush",
					SkinID = 907002205u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Blazing Heart",
					SkinID = 907102001u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Eternal Diamond",
					SkinID = 907102002u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Flashing Spade",
					SkinID = 907102003u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Dreamy Club",
					SkinID = 907102004u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Royal Flush",
					SkinID = 907102205u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Tunes of Roses",
					SkinID = 907103535u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Blazing Heart",
					SkinID = 907202001u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Eternal Diamond",
					SkinID = 907202002u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Flashing Spade",
					SkinID = 907202003u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Dreamy Club",
					SkinID = 907202004u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Royal Flush",
					SkinID = 907202205u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "New Year",
					SkinID = 907000077u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Bloody Gold",
					SkinID = 907001305u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Crazy Bunny",
					SkinID = 907001403u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Mechanical",
					SkinID = 907001506u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Lightning Strike",
					SkinID = 907001612u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Sneaky Clown",
					SkinID = 907001720u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Carnival Carnage",
					SkinID = 907001906u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "New Year",
					SkinID = 907100077u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Bloody Gold",
					SkinID = 907101305u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Bloody Gold",
					SkinID = 907101323u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Crazy Bunny",
					SkinID = 907101403u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Crazy Bunny",
					SkinID = 907101432u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Mechanical",
					SkinID = 907101506u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Mechanical",
					SkinID = 907101536u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Lightning Strike",
					SkinID = 907101612u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Sneaky Clown",
					SkinID = 907101720u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Carnival Carnage",
					SkinID = 907101906u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Gusto Freerunner",
					SkinID = 907103552u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Robust Combust",
					SkinID = 907104602u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Lightning Strike",
					SkinID = 907201612u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Sneaky Clown",
					SkinID = 907201720u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Carnival Carnage",
					SkinID = 907201906u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Winterlands",
					SkinID = 907000085u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Flaming Wolf",
					SkinID = 907000088u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "War Machine",
					SkinID = 907001302u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Engineer",
					SkinID = 907001823u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Winterlands",
					SkinID = 907100085u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Flaming Wolf",
					SkinID = 907100088u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "War Machine",
					SkinID = 907101302u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Engineer",
					SkinID = 907101823u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Haven Warrior",
					SkinID = 907103302u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S23 Exclusive: Crystal",
					SkinID = 907104320u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S31 Exclusive: A124",
					SkinID = 907136021u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136043u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Engineer",
					SkinID = 907201823u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Uchiha's Legacy",
					SkinID = 907105001u,
					Color = 4281571839u
				},
				new SkinInfo
				{
					Name = "Uchiha's Legacy",
					SkinID = 907105002u,
					Color = 4281571839u
				},
				new SkinInfo
				{
					Name = "Uchiha's Legacy",
					SkinID = 907105003u,
					Color = 4281571839u
				}
			}
		},
		{
			18u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "High Voltage",
					SkinID = 907104334u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Duchess Swallowtail",
					SkinID = 907002218u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Hysteria",
					SkinID = 907002314u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Duchess Swallowtail",
					SkinID = 907102218u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Hysteria",
					SkinID = 907102314u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Puppet Shooter",
					SkinID = 907104215u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Prince Darkheart",
					SkinID = 907105016u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Duchess Swallowtail",
					SkinID = 907202218u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Hysteria",
					SkinID = 907202314u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Flaming Wolf",
					SkinID = 907000089u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Digital Invasion",
					SkinID = 907001310u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Red Samurai",
					SkinID = 907001408u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death Wish",
					SkinID = 907001501u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Phantom Assassin",
					SkinID = 907001704u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Flaming Skull",
					SkinID = 907001820u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Urban Rager",
					SkinID = 907002021u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Victory Wings",
					SkinID = 907002203u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Master of Minds",
					SkinID = 907002403u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Dr. Nanotech",
					SkinID = 907002519u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Flaming Wolf",
					SkinID = 907100089u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Digital Invasion",
					SkinID = 907101310u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Red Samurai",
					SkinID = 907101408u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death Wish",
					SkinID = 907101501u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death Wish",
					SkinID = 907101531u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Phantom Assassin",
					SkinID = 907101704u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Flaming Skull",
					SkinID = 907101820u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Urban Rager",
					SkinID = 907102021u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Victory Wings",
					SkinID = 907102203u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Master of Minds",
					SkinID = 907102403u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Dr. Nanotech",
					SkinID = 907102519u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S23 Exclusive: Andrew \"the Fierce\"",
					SkinID = 907102827u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Whirlwind Crease",
					SkinID = 907103104u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "The Golden SKS",
					SkinID = 907103219u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Gnarl Electrocution",
					SkinID = 907136003u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136067u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Phantom Assassin",
					SkinID = 907201704u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Flaming Skull",
					SkinID = 907201820u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Urban Rager",
					SkinID = 907202021u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Victory Wings",
					SkinID = 907202203u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Master of Minds",
					SkinID = 907202403u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Dr. Nanotech",
					SkinID = 907202519u,
					Color = 4294901760u
				}
			}
		},
		{
			19u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Fabled Foxes",
					SkinID = 907002102u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Endless Oblivion",
					SkinID = 907002429u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Fabled Foxes",
					SkinID = 907102102u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Endless Oblivion",
					SkinID = 907102429u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Riverdust Splasher",
					SkinID = 907103516u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Steel Cowboy",
					SkinID = 907103710u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cosmic Chaos",
					SkinID = 907104911u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Celestial Archer",
					SkinID = 907105114u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Fabled Foxes",
					SkinID = 907202102u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Endless Oblivion",
					SkinID = 907202429u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Striking Pink",
					SkinID = 907000056u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "War Machine",
					SkinID = 907001303u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Blood Hockey",
					SkinID = 907001805u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Spikey Spine",
					SkinID = 907002112u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Clownfish",
					SkinID = 907002215u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "MMA Fighter",
					SkinID = 907002425u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Superstar",
					SkinID = 907002629u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Striking Pink",
					SkinID = 907100056u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "War Machine",
					SkinID = 907101303u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Blood Hockey",
					SkinID = 907101805u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Spikey Spine",
					SkinID = 907102112u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Clownfish",
					SkinID = 907102215u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "MMA Fighter",
					SkinID = 907102425u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Superstar",
					SkinID = 907102629u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mesmerizing Nights",
					SkinID = 907102908u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Disc Jockey",
					SkinID = 907102934u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Fire Bones",
					SkinID = 907103118u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Violet Fear",
					SkinID = 907103418u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Jam Sessions",
					SkinID = 907103902u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Storm Surge",
					SkinID = 907103919u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Poppin' Griddin'",
					SkinID = 907104018u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Wisp",
					SkinID = 907104410u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S43 Exclusive: Stitched",
					SkinID = 907104724u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136061u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136077u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Blood Hockey",
					SkinID = 907201805u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Spikey Spine",
					SkinID = 907202112u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Clownfish",
					SkinID = 907202215u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "MMA Fighter",
					SkinID = 907202425u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Superstar",
					SkinID = 907202629u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Waggor's Wonder",
					SkinID = 907104058u,
					Color = 4278255360u
				},
				new SkinInfo
				{
					Name = "Zombie Attack",
					SkinID = 907104104u,
					Color = 4278255360u
				}
			}
		},
		{
			21u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "White Vines",
					SkinID = 907104335u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Golden Rule",
					SkinID = 907001702u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Rebel Academy",
					SkinID = 907002211u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "The Executioner",
					SkinID = 907002620u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Golden Rule",
					SkinID = 907101702u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Rebel Academy",
					SkinID = 907102211u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "The Executioner",
					SkinID = 907102620u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "King of Desert",
					SkinID = 907105026u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Golden Rule",
					SkinID = 907201702u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Rebel Academy",
					SkinID = 907202211u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "The Executioner",
					SkinID = 907202620u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Imperial Rome",
					SkinID = 907001313u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Ice Blue",
					SkinID = 907001424u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Star Soul",
					SkinID = 907001512u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Warrior's Spirit",
					SkinID = 907001816u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Kpop Stardom",
					SkinID = 907002121u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Great Plunder",
					SkinID = 907002313u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Imperial Rome",
					SkinID = 907101313u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Ice Blue",
					SkinID = 907101424u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Star Soul",
					SkinID = 907101512u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Star Soul",
					SkinID = 907101542u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Warrior's Spirit",
					SkinID = 907101816u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Kpop Stardom",
					SkinID = 907102121u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Great Plunder",
					SkinID = 907102313u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Big Bang",
					SkinID = 907103103u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Apex Underwater",
					SkinID = 907103503u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Night Bite",
					SkinID = 907103713u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Ink Lock",
					SkinID = 907104132u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S37 Exclusive: Electro",
					SkinID = 907104229u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136071u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Warrior's Spirit",
					SkinID = 907201816u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Kpop Stardom",
					SkinID = 907202121u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Great Plunder",
					SkinID = 907202313u,
					Color = 4294901760u
				}
			}
		},
		{
			24u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Demonic Grin",
					SkinID = 907192901u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Demonic Grin",
					SkinID = 907192902u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Demonic Grin",
					SkinID = 907192903u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Demonic Grin",
					SkinID = 907192904u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Demonic Grin",
					SkinID = 907192905u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Demonic Grin",
					SkinID = 907192906u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Demonic Grin",
					SkinID = 907192907u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Black Widow",
					SkinID = 907001615u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Ocean",
					SkinID = 907001616u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Golden",
					SkinID = 907001617u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Void",
					SkinID = 907001618u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Lightning",
					SkinID = 907001619u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow",
					SkinID = 907101615u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Ocean",
					SkinID = 907101616u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Golden",
					SkinID = 907101617u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Void",
					SkinID = 907101618u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Lightning",
					SkinID = 907101619u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Lava",
					SkinID = 907104311u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow",
					SkinID = 907201615u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Ocean",
					SkinID = 907201616u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Golden",
					SkinID = 907201617u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Void",
					SkinID = 907201618u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Lightning",
					SkinID = 907201619u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Metallic",
					SkinID = 907001440u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Vampire",
					SkinID = 907001705u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Warrior's Spirit",
					SkinID = 907001815u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Swagger Ownage",
					SkinID = 907001917u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "The Golden FAMAS",
					SkinID = 907002437u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Metallic",
					SkinID = 907101440u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Metallic",
					SkinID = 907101445u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Vampire",
					SkinID = 907101705u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Warrior's Spirit",
					SkinID = 907101815u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Swagger Ownage",
					SkinID = 907101917u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "The Golden FAMAS",
					SkinID = 907102437u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Wisdom's Vision",
					SkinID = 907104005u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "BLUE LOCK",
					SkinID = 907104652u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "HEARTROCKER",
					SkinID = 907105136u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Sculpted Excellence",
					SkinID = 907105313u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Vampire",
					SkinID = 907201705u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Warrior's Spirit",
					SkinID = 907201815u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Swagger Ownage",
					SkinID = 907201917u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "The Golden FAMAS",
					SkinID = 907202437u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Digital Invasion",
					SkinID = 907001308u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Imperial Rome",
					SkinID = 907001314u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Dragon's Eye",
					SkinID = 907001401u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Shark Attack",
					SkinID = 907001413u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Ice Blue",
					SkinID = 907001423u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pirate's Soul",
					SkinID = 907001827u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Loose Cannon",
					SkinID = 907001923u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Kpop Stardom",
					SkinID = 907002118u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Private Eye",
					SkinID = 907002224u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Wilderness Hunter",
					SkinID = 907002427u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Digital Invasion",
					SkinID = 907101308u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Imperial Rome",
					SkinID = 907101314u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Dragon's Eye",
					SkinID = 907101401u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Shark Attack",
					SkinID = 907101413u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Ice Blue",
					SkinID = 907101423u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pirate's Soul",
					SkinID = 907101827u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Loose Cannon",
					SkinID = 907101923u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Kpop Stardom",
					SkinID = 907102118u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Private Eye",
					SkinID = 907102224u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Wilderness Hunter",
					SkinID = 907102427u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Whirlwind Crease",
					SkinID = 907103105u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S26 Exclusive: Laura",
					SkinID = 907103128u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S30 Exclusive: Firefly",
					SkinID = 907104832u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Dragon Skin",
					SkinID = 907136019u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136053u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pirate's Soul",
					SkinID = 907201827u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Loose Cannon",
					SkinID = 907201923u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Kpop Stardom",
					SkinID = 907202118u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Private Eye",
					SkinID = 907202224u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Wilderness Hunter",
					SkinID = 907202427u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Waggor's Wonder",
					SkinID = 907104060u,
					Color = 4278255360u
				},
				new SkinInfo
				{
					Name = "Midnight Cityscape",
					SkinID = 907104808u,
					Color = 4281571839u
				}
			}
		},
		{
			25u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Seahorse",
					SkinID = 907104427u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Celestial Street",
					SkinID = 907002304u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Celestial Street",
					SkinID = 907102304u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Aqua Blast",
					SkinID = 907104009u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "FFWS Will of Fire",
					SkinID = 907105106u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Celestial Street",
					SkinID = 907202304u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Party Animal",
					SkinID = 907002420u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Party Animal",
					SkinID = 907102420u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "The Golden M500",
					SkinID = 907102730u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Fire Bones",
					SkinID = 907103123u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S29 Exclusive: Joseph",
					SkinID = 907103420u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Song of Hana",
					SkinID = 907103709u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136086u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Party Animal",
					SkinID = 907202420u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Electro Enforcer",
					SkinID = 907104220u,
					Color = 4278255360u
				}
			}
		},
		{
			26u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Chainsaw",
					SkinID = 907104539u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Swordsman Legends",
					SkinID = 907002405u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Swordsman Legends",
					SkinID = 907102405u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Eternal Enigma",
					SkinID = 907104614u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Chillin' Emoji",
					SkinID = 907105217u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Nightslayer Grizzly",
					SkinID = 907136017u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Swordsman Legends",
					SkinID = 907202405u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Artificial Intelligence",
					SkinID = 907001725u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Loose Cannon",
					SkinID = 907001925u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Spirited Overseers",
					SkinID = 907002307u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Dr. Nanotech",
					SkinID = 907002517u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Viper Gangster",
					SkinID = 907002623u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Artificial Intelligence",
					SkinID = 907101725u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Loose Cannon",
					SkinID = 907101925u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Spirited Overseers",
					SkinID = 907102307u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Dr. Nanotech",
					SkinID = 907102517u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Viper Gangster",
					SkinID = 907102623u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "The Falconer",
					SkinID = 907102725u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Frozen Platinum",
					SkinID = 907103213u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Sound Crafter",
					SkinID = 907103402u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S25 Exclusive: Breaker",
					SkinID = 907104423u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136066u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Artificial Intelligence",
					SkinID = 907201725u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Loose Cannon",
					SkinID = 907201925u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Spirited Overseers",
					SkinID = 907202307u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Dr. Nanotech",
					SkinID = 907202517u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Viper Gangster",
					SkinID = 907202623u,
					Color = 4294901760u
				}
			}
		},
		{
			28u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Destiny Guardian MAX",
					SkinID = 907192807u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Toxic Pumpkin",
					SkinID = 907002411u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Poison Pumpkin",
					SkinID = 907002412u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Fiery Pumpkin",
					SkinID = 907002413u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Sinister Pumpkin",
					SkinID = 907102410u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Toxic Pumpkin",
					SkinID = 907102411u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Poison Pumpkin",
					SkinID = 907102412u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Fiery Pumpkin",
					SkinID = 907102413u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Sinister Pumpkin",
					SkinID = 907202410u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Toxic Pumpkin",
					SkinID = 907202411u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Poison Pumpkin",
					SkinID = 907202412u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Fiery Pumpkin",
					SkinID = 907202413u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Abyssal",
					SkinID = 907002109u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Abyssal",
					SkinID = 907102109u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Paradise Defender",
					SkinID = 907103512u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Blizzard Brawl",
					SkinID = 907103816u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Solar Cloud",
					SkinID = 907105027u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Dreamspace",
					SkinID = 907105104u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Abyssal",
					SkinID = 907202109u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Lively Beast",
					SkinID = 907001605u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Hunter's Eye",
					SkinID = 907001607u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Sneaky Clown",
					SkinID = 907001721u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Deadly Bat",
					SkinID = 907001808u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Engineer",
					SkinID = 907001825u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Clandestine Impulse",
					SkinID = 907001903u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Aurous Dragon",
					SkinID = 907001909u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "The Punishers",
					SkinID = 907002015u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Game Streamer",
					SkinID = 907002220u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Hysteria",
					SkinID = 907002315u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Lively Beast",
					SkinID = 907101605u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Hunter's Eye",
					SkinID = 907101607u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Sneaky Clown",
					SkinID = 907101721u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Deadly Bat",
					SkinID = 907101808u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Engineer",
					SkinID = 907101825u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Clandestine Impulse",
					SkinID = 907101903u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Aurous Dragon",
					SkinID = 907101909u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "The Punishers",
					SkinID = 907102015u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Game Streamer",
					SkinID = 907102220u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Hysteria",
					SkinID = 907102315u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Zombie Attack",
					SkinID = 907104241u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S50 Exclusive: Voyager",
					SkinID = 907105219u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "The Golden XM8",
					SkinID = 907136020u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136052u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Lively Beast",
					SkinID = 907201605u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Hunter's Eye",
					SkinID = 907201607u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Sneaky Clown",
					SkinID = 907201721u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Deadly Bat",
					SkinID = 907201808u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Engineer",
					SkinID = 907201825u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Clandestine Impulse",
					SkinID = 907201903u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Aurous Dragon",
					SkinID = 907201909u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "The Punishers",
					SkinID = 907202015u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Game Streamer",
					SkinID = 907202220u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Hysteria",
					SkinID = 907202315u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Screaming Chicky",
					SkinID = 907104310u,
					Color = 4281571839u
				}
			}
		},
		{
			29u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Rip and Tear",
					SkinID = 907104630u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Cosmic Teleportia",
					SkinID = 907102919u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Golden Flash",
					SkinID = 907104817u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Winterlands",
					SkinID = 907000086u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Flaming Wolf",
					SkinID = 907000090u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Digital Invasion",
					SkinID = 907001309u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Plague Doctor",
					SkinID = 907001318u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Red Samurai",
					SkinID = 907001410u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Star Soul",
					SkinID = 907001511u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Skull Hunter",
					SkinID = 907001802u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Deadly Bat",
					SkinID = 907001811u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Arctic Blue",
					SkinID = 907001822u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Santa's Choice",
					SkinID = 907001911u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Wasteland",
					SkinID = 907002012u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Urban Rager",
					SkinID = 907002020u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Abyssal",
					SkinID = 907002110u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mystic Seeker",
					SkinID = 907002213u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "MMA Fighter",
					SkinID = 907002424u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Superstar",
					SkinID = 907002630u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Winterlands",
					SkinID = 907100086u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Flaming Wolf",
					SkinID = 907100090u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Digital Invasion",
					SkinID = 907101309u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Plague Doctor",
					SkinID = 907101318u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Red Samurai",
					SkinID = 907101410u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Star Soul",
					SkinID = 907101511u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Star Soul",
					SkinID = 907101541u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Skull Hunter",
					SkinID = 907101802u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Deadly Bat",
					SkinID = 907101811u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Arctic Blue",
					SkinID = 907101822u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Santa's Choice",
					SkinID = 907101911u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Wasteland",
					SkinID = 907102012u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Urban Rager",
					SkinID = 907102020u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Abyssal",
					SkinID = 907102110u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mystic Seeker",
					SkinID = 907102213u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "MMA Fighter",
					SkinID = 907102424u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Superstar",
					SkinID = 907102630u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Theatre of Torment",
					SkinID = 907102701u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mesmerizing Nights",
					SkinID = 907102907u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S27 Exclusive: Caroline",
					SkinID = 907103220u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Neon Opera",
					SkinID = 907103309u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Burning Lily",
					SkinID = 907103321u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Plague Doctor",
					SkinID = 907111318u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136042u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Skull Hunter",
					SkinID = 907201802u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Deadly Bat",
					SkinID = 907201811u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Arctic Blue",
					SkinID = 907201822u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Santa's Choice",
					SkinID = 907201911u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Wasteland",
					SkinID = 907202012u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Urban Rager",
					SkinID = 907202020u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Abyssal",
					SkinID = 907202110u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mystic Seeker",
					SkinID = 907202213u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "MMA Fighter",
					SkinID = 907202424u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Superstar",
					SkinID = 907202630u,
					Color = 4294901760u
				}
			}
		},
		{
			30u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Dreambreaker",
					SkinID = 907195111u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Dreambreaker",
					SkinID = 907195112u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Dreambreaker",
					SkinID = 907195113u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Dreambreaker",
					SkinID = 907195114u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Dreambreaker",
					SkinID = 907195115u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Dreambreaker",
					SkinID = 907195116u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Dreambreaker",
					SkinID = 907195117u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Azure Stormbringer",
					SkinID = 907102806u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Frost Sabertooth",
					SkinID = 907102807u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Volcanic Whirlwind",
					SkinID = 907102808u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Shadow Earthshaker",
					SkinID = 907102809u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Deep Sea Tentacle",
					SkinID = 907104428u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Gold Coated",
					SkinID = 907001316u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Golden Rule",
					SkinID = 907001701u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Crimson Red",
					SkinID = 907001821u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Santa's Choice",
					SkinID = 907001910u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Captain Bubbles",
					SkinID = 907002214u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Spirited Overseers",
					SkinID = 907002306u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Viper Gangster",
					SkinID = 907002622u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Gold Coated",
					SkinID = 907101316u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Gold Coated",
					SkinID = 907101326u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Golden Rule",
					SkinID = 907101701u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Crimson Red",
					SkinID = 907101821u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Santa's Choice",
					SkinID = 907101910u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Captain Bubbles",
					SkinID = 907102214u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Spirited Overseers",
					SkinID = 907102306u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Viper Gangster",
					SkinID = 907102622u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Reel Shooter",
					SkinID = 907104128u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Wisp",
					SkinID = 907104409u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Akatsuki Theme",
					SkinID = 907105005u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Gold Coated",
					SkinID = 907111316u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Golden Rule",
					SkinID = 907201701u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Crimson Red",
					SkinID = 907201821u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Santa's Choice",
					SkinID = 907201910u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Captain Bubbles",
					SkinID = 907202214u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Spirited Overseers",
					SkinID = 907202306u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Viper Gangster",
					SkinID = 907202622u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Lively Beast",
					SkinID = 907001603u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pink Devil",
					SkinID = 907001626u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pirate's Soul",
					SkinID = 907001828u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Agent Paws",
					SkinID = 907002007u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Victory Wings",
					SkinID = 907002201u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Justice Fighter",
					SkinID = 907002318u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Swordsman Legends",
					SkinID = 907002406u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Lively Beast",
					SkinID = 907101603u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pink Devil",
					SkinID = 907101626u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pirate's Soul",
					SkinID = 907101828u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Agent Paws",
					SkinID = 907102007u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Victory Wings",
					SkinID = 907102201u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Justice Fighter",
					SkinID = 907102318u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Swordsman Legends",
					SkinID = 907102406u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Ice Blossoms",
					SkinID = 907103126u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Porcelain Rush",
					SkinID = 907103204u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Sublime Scorpio",
					SkinID = 907103909u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136055u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Lively Beast",
					SkinID = 907201603u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pink Devil",
					SkinID = 907201626u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pirate's Soul",
					SkinID = 907201828u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Agent Paws",
					SkinID = 907202007u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Victory Wings",
					SkinID = 907202201u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Justice Fighter",
					SkinID = 907202318u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Swordsman Legends",
					SkinID = 907202406u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Zombie Attack",
					SkinID = 907104103u,
					Color = 4278255360u
				}
			}
		},
		{
			32u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Gilded Corrosion",
					SkinID = 907194911u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Gilded Corrosion",
					SkinID = 907194912u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Gilded Corrosion",
					SkinID = 907194913u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Gilded Corrosion",
					SkinID = 907194914u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Gilded Corrosion",
					SkinID = 907194915u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Gilded Corrosion",
					SkinID = 907194916u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Gilded Corrosion",
					SkinID = 907194917u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Rebel Academy",
					SkinID = 907002210u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Tune Blaster Orange",
					SkinID = 907002506u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Rebel Academy",
					SkinID = 907102210u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Tune Blaster Orange",
					SkinID = 907102506u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Genetic Equalizer",
					SkinID = 907104408u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Rebel Academy",
					SkinID = 907202210u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Tune Blaster Orange",
					SkinID = 907202506u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Phantom",
					SkinID = 907001707u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Blood Devil",
					SkinID = 907001714u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "The Punishers",
					SkinID = 907002014u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Attack on Titan",
					SkinID = 907002106u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Make it Rain",
					SkinID = 907002322u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Tune Blaster Red",
					SkinID = 907002507u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Tune Blaster Blue",
					SkinID = 907002508u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Tune Blaster Green",
					SkinID = 907002509u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "The Golden P90",
					SkinID = 907002520u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Phantom",
					SkinID = 907101707u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Blood Devil",
					SkinID = 907101714u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "The Punishers",
					SkinID = 907102014u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Attack on Titan",
					SkinID = 907102106u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Make it Rain",
					SkinID = 907102322u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Tune Blaster Red",
					SkinID = 907102507u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Tune Blaster Blue",
					SkinID = 907102508u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Tune Blaster Green",
					SkinID = 907102509u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "The Golden P90",
					SkinID = 907102520u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Modern Aquarium",
					SkinID = 907103904u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Bunny's Order",
					SkinID = 907104019u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Volcanic Might",
					SkinID = 907105020u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Phantom",
					SkinID = 907201707u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Blood Devil",
					SkinID = 907201714u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "The Punishers",
					SkinID = 907202014u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Attack on Titan",
					SkinID = 907202106u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Make it Rain",
					SkinID = 907202322u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Tune Blaster Red",
					SkinID = 907202507u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Tune Blaster Blue",
					SkinID = 907202508u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Tune Blaster Green",
					SkinID = 907202509u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "The Golden P90",
					SkinID = 907202520u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "The Viking",
					SkinID = 907001525u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Old Fashioned",
					SkinID = 907001719u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Swagger Ownage",
					SkinID = 907001918u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Midnight Mafia",
					SkinID = 907002515u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "The Viking",
					SkinID = 907101525u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "The Viking",
					SkinID = 907101555u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Old Fashioned",
					SkinID = 907101719u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Swagger Ownage",
					SkinID = 907101918u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Midnight Mafia",
					SkinID = 907102515u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Xtreme Adventure",
					SkinID = 907102729u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Porcelain Rush",
					SkinID = 907103203u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Metal Wings",
					SkinID = 907103235u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S28 Exclusive: Antonio",
					SkinID = 907103328u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Riverdust Splasher",
					SkinID = 907103517u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Fiery Fantasy",
					SkinID = 907103915u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "24K Kitty",
					SkinID = 907104714u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136045u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Old Fashioned",
					SkinID = 907201719u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Swagger Ownage",
					SkinID = 907201918u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Midnight Mafia",
					SkinID = 907202515u,
					Color = 4294901760u
				}
			}
		},
		{
			33u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Evil Howler",
					SkinID = 907193511u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Evil Howler",
					SkinID = 907193512u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Evil Howler",
					SkinID = 907193513u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Evil Howler",
					SkinID = 907193514u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Evil Howler",
					SkinID = 907193515u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Evil Howler",
					SkinID = 907193516u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Evil Howler",
					SkinID = 907193517u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Tornado Bolt",
					SkinID = 907103025u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Tsunami Bolt",
					SkinID = 907103026u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Twilight Bolt",
					SkinID = 907103027u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Wildfire Bolt",
					SkinID = 907103028u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Inky Roar",
					SkinID = 907105011u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Cataclysm",
					SkinID = 907001505u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Blood Devil",
					SkinID = 907001713u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Head",
					SkinID = 907002009u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Spikey Spine",
					SkinID = 907002111u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cataclysm",
					SkinID = 907101505u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cataclysm",
					SkinID = 907101535u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Blood Devil",
					SkinID = 907101713u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Head",
					SkinID = 907102009u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Spikey Spine",
					SkinID = 907102111u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Ruby Bride",
					SkinID = 907102822u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Dart Electric",
					SkinID = 907103714u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Bliss Popper",
					SkinID = 907104063u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Signal Error",
					SkinID = 907104904u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Undersea Mystery",
					SkinID = 907105307u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Blood Devil",
					SkinID = 907201713u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Head",
					SkinID = 907202009u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Spikey Spine",
					SkinID = 907202111u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "BOOYAH",
					SkinID = 907001620u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Art of War",
					SkinID = 907001818u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Engineer",
					SkinID = 907001824u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Carnival Carnage",
					SkinID = 907001907u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Game Streamer",
					SkinID = 907002221u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Plan Bermuda",
					SkinID = 907002323u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Hurricane Delivery",
					SkinID = 907002626u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "BOOYAH",
					SkinID = 907101620u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Art of War",
					SkinID = 907101818u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Engineer",
					SkinID = 907101824u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Carnival Carnage",
					SkinID = 907101907u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Game Streamer",
					SkinID = 907102221u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Plan Bermuda",
					SkinID = 907102323u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Hurricane Delivery",
					SkinID = 907102626u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S21 Exclusive: Hayato \"Firebrand\"",
					SkinID = 907102731u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "The Golden AN94",
					SkinID = 907102924u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Checkered Knight",
					SkinID = 907103306u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Spot On",
					SkinID = 907104014u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Zombie Attack",
					SkinID = 907104317u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136056u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "BOOYAH",
					SkinID = 907201620u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Art of War",
					SkinID = 907201818u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Engineer",
					SkinID = 907201824u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Carnival Carnage",
					SkinID = 907201907u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Game Streamer",
					SkinID = 907202221u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Plan Bermuda",
					SkinID = 907202323u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Hurricane Delivery",
					SkinID = 907202626u,
					Color = 4294901760u
				}
			}
		},
		{
			35u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Phantom",
					SkinID = 907001708u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Engineer",
					SkinID = 907001826u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Doomsday",
					SkinID = 907002006u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Urban Rager",
					SkinID = 907002019u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Phantom",
					SkinID = 907101708u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Engineer",
					SkinID = 907101826u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Doomsday",
					SkinID = 907102006u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Urban Rager",
					SkinID = 907102019u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Phantom",
					SkinID = 907201708u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Engineer",
					SkinID = 907201826u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Doomsday",
					SkinID = 907202006u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Urban Rager",
					SkinID = 907202019u,
					Color = 4294901760u
				}
			}
		},
		{
			39u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Underwater Dweller",
					SkinID = 907104837u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Fuji Folklore",
					SkinID = 907002502u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Fuji Folklore",
					SkinID = 907102502u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Sunrise Leo",
					SkinID = 907104502u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Darkwing Dazzle",
					SkinID = 907105311u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Fuji Folklore",
					SkinID = 907202502u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Champion Boxer",
					SkinID = 907002409u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Hand of Hope",
					SkinID = 907002713u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Champion Boxer",
					SkinID = 907102409u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Theatre of Torment",
					SkinID = 907102702u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Hand of Hope",
					SkinID = 907102713u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Joker",
					SkinID = 907103022u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Darkrose Lurker",
					SkinID = 907103519u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Sickly Sweet",
					SkinID = 907103809u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Aura of Chaos",
					SkinID = 907104613u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S45 Exclusive: Ramen",
					SkinID = 907104833u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Drowning Blizzard",
					SkinID = 907136005u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136062u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Champion Boxer",
					SkinID = 907202409u,
					Color = 4294901760u
				}
			}
		},
		{
			41u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Sterling Conqueror",
					SkinID = 907193501u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Sterling Conqueror",
					SkinID = 907193502u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Sterling Conqueror",
					SkinID = 907193503u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Sterling Conqueror",
					SkinID = 907193504u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Sterling Conqueror",
					SkinID = 907193505u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Sterling Conqueror",
					SkinID = 907193506u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Sterling Conqueror",
					SkinID = 907193507u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "One Punch Man",
					SkinID = 907002535u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Hand of Hope",
					SkinID = 907002712u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "One Punch Man",
					SkinID = 907102535u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Hand of Hope",
					SkinID = 907102712u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Golden Glare",
					SkinID = 907102937u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Incendium Burst",
					SkinID = 907103107u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Aqua Burst",
					SkinID = 907103108u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Solaris Burst",
					SkinID = 907103109u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Terrano Burst",
					SkinID = 907103110u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Emerald Power",
					SkinID = 907103316u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Eagle Gaze",
					SkinID = 907103916u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "One Punch Man",
					SkinID = 907202535u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Rapper Underworld",
					SkinID = 907002309u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Winterlands 2020",
					SkinID = 907002512u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Freezing Flame",
					SkinID = 907002732u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Rapper Underworld",
					SkinID = 907102309u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Winterlands 2020",
					SkinID = 907102512u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Freezing Flame",
					SkinID = 907102732u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Troublemaker",
					SkinID = 907105018u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Rapper Underworld",
					SkinID = 907202309u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Winterlands 2020",
					SkinID = 907202512u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Tropical Parrot",
					SkinID = 907002123u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Xtreme Adventure",
					SkinID = 907002727u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Tropical Parrot",
					SkinID = 907102123u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Xtreme Adventure",
					SkinID = 907102727u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Bunny's Order",
					SkinID = 907104020u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Glistening Nightstar",
					SkinID = 907104309u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136040u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Tropical Parrot",
					SkinID = 907202123u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Sandstorm Shimmer",
					SkinID = 907104807u,
					Color = 4281571839u
				}
			}
		},
		{
			43u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Cindered Colossus",
					SkinID = 907193901u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Cindered Colossus",
					SkinID = 907193902u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Cindered Colossus",
					SkinID = 907193903u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Cindered Colossus",
					SkinID = 907193904u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Cindered Colossus",
					SkinID = 907193905u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Cindered Colossus",
					SkinID = 907193906u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Cindered Colossus",
					SkinID = 907193907u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Thrash Metallic",
					SkinID = 907102915u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Galactic Panthera",
					SkinID = 907136007u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Dawnbreaker Lynx",
					SkinID = 907136008u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Firehound Jaguar",
					SkinID = 907136009u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Hoarfrost Cheetah",
					SkinID = 907136010u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Specter Squad",
					SkinID = 907002504u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Time Travellers",
					SkinID = 907002510u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Bloodwing City",
					SkinID = 907002602u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Specter Squad",
					SkinID = 907102504u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Time Travellers",
					SkinID = 907102510u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Bloodwing City",
					SkinID = 907102602u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Dragon Mob",
					SkinID = 907102714u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Freezing Flame",
					SkinID = 907102829u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Goldrim Tribute",
					SkinID = 907103001u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Fierce Demilord",
					SkinID = 907103210u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Daybreaker",
					SkinID = 907104402u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Specter Squad",
					SkinID = 907202504u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Time Travellers",
					SkinID = 907202510u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Bloodwing City",
					SkinID = 907202602u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "MMA Fighter",
					SkinID = 907002422u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Lucky Koi",
					SkinID = 907002617u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "MMA Fighter",
					SkinID = 907102422u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Lucky Koi",
					SkinID = 907102617u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "The Falconer",
					SkinID = 907102723u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S35 Exclusive: Tatsuya",
					SkinID = 907104022u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S26 Exclusive: Leo",
					SkinID = 907104520u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Shadow Aim",
					SkinID = 907105210u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136047u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "MMA Fighter",
					SkinID = 907202422u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Lucky Koi",
					SkinID = 907202617u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Waggor's Wonder",
					SkinID = 907104056u,
					Color = 4278255360u
				},
				new SkinInfo
				{
					Name = "Waggor's Wonder (15 Days)",
					SkinID = 907104499u,
					Color = 4278255360u
				},
				new SkinInfo
				{
					Name = "Hybrid Explosion",
					SkinID = 907103408u,
					Color = 4281571839u
				},
				new SkinInfo
				{
					Name = "Cyber Claws",
					SkinID = 907104607u,
					Color = 4281571839u
				}
			}
		},
		{
			45u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Titanium Glow",
					SkinID = 907104538u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Vampire Malevolence",
					SkinID = 907103216u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Fiery Rush",
					SkinID = 907103920u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Prismatic Blaze",
					SkinID = 907104308u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Taped Delivery",
					SkinID = 907104706u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Golden Dominance",
					SkinID = 907105212u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "MMA Fighter",
					SkinID = 907002423u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Time Travellers",
					SkinID = 907002511u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Deadly Glares",
					SkinID = 907002606u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Hurricane Delivery",
					SkinID = 907002625u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "MMA Fighter",
					SkinID = 907102423u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Time Travellers",
					SkinID = 907102511u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Deadly Glares",
					SkinID = 907102606u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Hurricane Delivery",
					SkinID = 907102625u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Dragon Mob",
					SkinID = 907102715u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "The Falconer",
					SkinID = 907102724u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Operano Sheng",
					SkinID = 907102921u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Ace Gamer",
					SkinID = 907103411u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Serpent Kami",
					SkinID = 907103807u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136070u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "MMA Fighter",
					SkinID = 907202423u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Time Travellers",
					SkinID = 907202511u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Deadly Glares",
					SkinID = 907202606u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Hurricane Delivery",
					SkinID = 907202625u,
					Color = 4294901760u
				}
			}
		},
		{
			46u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Cyber Bounty Hunter",
					SkinID = 907002524u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Cyber Bounty Hunter",
					SkinID = 907102524u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Mars Landcrusher",
					SkinID = 907103431u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Ventus Oceanbust",
					SkinID = 907103432u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Nebula Skydark",
					SkinID = 907103433u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Aqua Firetaming",
					SkinID = 907103434u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Cyber Bounty Hunter",
					SkinID = 907202524u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Booyah Day",
					SkinID = 907002416u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Booyah Day",
					SkinID = 907102416u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Roaring Gunfighter",
					SkinID = 907103231u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Jelly Shot",
					SkinID = 907104126u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Beast of Legend",
					SkinID = 907104315u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Jagged Laughs",
					SkinID = 907104601u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Jagged Laughs",
					SkinID = 907104604u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Astral Ripples",
					SkinID = 907105215u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Eclipse Rise",
					SkinID = 907105305u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Booyah Day",
					SkinID = 907202416u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Party Animal",
					SkinID = 907002418u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Winterlands 2020",
					SkinID = 907002513u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Party Animal",
					SkinID = 907102418u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Winterlands 2020",
					SkinID = 907102513u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mr. Nutcracker",
					SkinID = 907102717u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Quantic Unknown",
					SkinID = 907102909u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Cosmic Teleportia",
					SkinID = 907102918u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S25 Exclusive: Miguel",
					SkinID = 907103011u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Phoenix Knight",
					SkinID = 907103116u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Apex Underwater",
					SkinID = 907103504u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Cannibal Crush",
					SkinID = 907103707u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Aqua Blast",
					SkinID = 907104010u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S46 Exclusive: Signal",
					SkinID = 907104928u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136058u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Party Animal",
					SkinID = 907202418u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Winterlands 2020",
					SkinID = 907202513u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Aurora's Holler",
					SkinID = 907104709u,
					Color = 4281571839u
				}
			}
		},
		{
			47u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Lore Cyclone",
					SkinID = 907194511u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Lore Cyclone",
					SkinID = 907194512u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Lore Cyclone",
					SkinID = 907194513u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Lore Cyclone",
					SkinID = 907194514u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Lore Cyclone",
					SkinID = 907194515u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Lore Cyclone",
					SkinID = 907194516u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Lore Cyclone",
					SkinID = 907194517u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Flames Undying",
					SkinID = 907103310u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Electron Undying",
					SkinID = 907103311u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Firespark Undying",
					SkinID = 907103312u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Windrift Undying",
					SkinID = 907103313u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Persia Prowess",
					SkinID = 907102720u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Crimson Heir",
					SkinID = 907103005u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Sickly Sweet",
					SkinID = 907103815u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Bedtime",
					SkinID = 907104813u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cosmic Bounty Hunter",
					SkinID = 907002532u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Egg Hunter",
					SkinID = 907002619u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Cosmic Bounty Hunter",
					SkinID = 907102532u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Egg Hunter",
					SkinID = 907102619u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Lethal Finstooth",
					SkinID = 907102820u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Red Fury",
					SkinID = 907102938u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Aurora Oni",
					SkinID = 907103114u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Bumble Attack",
					SkinID = 907103404u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S36 Exclusive: Luna",
					SkinID = 907104135u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S27 Exclusive: Jagged",
					SkinID = 907104624u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Wraith Patrol",
					SkinID = 907136014u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136059u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Cosmic Bounty Hunter",
					SkinID = 907202532u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Egg Hunter",
					SkinID = 907202619u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "City Eyes",
					SkinID = 907104065u,
					Color = 4278255360u
				},
				new SkinInfo
				{
					Name = "Ink Puncture",
					SkinID = 907104145u,
					Color = 4281571839u
				}
			}
		},
		{
			48u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Majestic Prowler",
					SkinID = 907194211u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Majestic Prowler",
					SkinID = 907194212u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Majestic Prowler",
					SkinID = 907194213u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Majestic Prowler",
					SkinID = 907194214u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Majestic Prowler",
					SkinID = 907194215u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Majestic Prowler",
					SkinID = 907194216u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Majestic Prowler",
					SkinID = 907194217u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Ace Gamer",
					SkinID = 907103410u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Deity Rally",
					SkinID = 907103804u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Deity Warcry",
					SkinID = 907103805u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Deity Menace",
					SkinID = 907103806u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Merciless Necromancer",
					SkinID = 907102816u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Red Robster",
					SkinID = 907103140u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Jam Sessions",
					SkinID = 907103901u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Captain Tentacles",
					SkinID = 907104503u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Party Animal",
					SkinID = 907002419u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Cosmic Bounty Hunter",
					SkinID = 907002533u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Superstar",
					SkinID = 907002628u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Party Animal",
					SkinID = 907102419u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Cosmic Bounty Hunter",
					SkinID = 907102533u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Superstar",
					SkinID = 907102628u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Crimson Heir",
					SkinID = 907103006u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Fierce Demilord",
					SkinID = 907103211u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Gray Ambush",
					SkinID = 907103221u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S34 Exclusive: Iris",
					SkinID = 907103923u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S34 Exclusive: Archer",
					SkinID = 907105117u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136065u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Party Animal",
					SkinID = 907202419u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Cosmic Bounty Hunter",
					SkinID = 907202533u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Superstar",
					SkinID = 907202628u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Puppet Strike",
					SkinID = 907104138u,
					Color = 4278255360u
				}
			}
		},
		{
			49u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Aquablaze Wrath",
					SkinID = 907102935u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Jubilee Dragontail",
					SkinID = 907103702u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Revenge Dragontail",
					SkinID = 907103703u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Royal Dragontail",
					SkinID = 907103704u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Cobble Dragontail",
					SkinID = 907103705u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Tails Intertwine",
					SkinID = 907104711u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "FFWS 2021",
					SkinID = 907102718u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Hypercore Blues",
					SkinID = 907103314u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "OGSuperhero",
					SkinID = 907103937u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Golden Dominance",
					SkinID = 907105211u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Taunting Smile",
					SkinID = 907002523u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Superstar",
					SkinID = 907002631u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Taunting Smile",
					SkinID = 907102523u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Superstar",
					SkinID = 907102631u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Xtreme Adventure",
					SkinID = 907102726u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Red Robster",
					SkinID = 907103141u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Golden Prosperity",
					SkinID = 907103407u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Hawk Wing",
					SkinID = 907103917u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "BLUE LOCK",
					SkinID = 907104653u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Golden Twilight",
					SkinID = 907105021u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S49 Exclusive: Woof",
					SkinID = 907105118u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S37 Exclusive: Twinstars",
					SkinID = 907105316u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136048u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Taunting Smile",
					SkinID = 907202523u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Superstar",
					SkinID = 907202631u,
					Color = 4294901760u
				}
			}
		},
		{
			50u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Pharaoh's Choice",
					SkinID = 907104407u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Fury Tang Lotus",
					SkinID = 907105049u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Lethal Finstooth",
					SkinID = 907102819u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Shadow Rogue",
					SkinID = 907103324u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Noble Sanction",
					SkinID = 907104011u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Ink Lock",
					SkinID = 907104142u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Pearlescent",
					SkinID = 907105109u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Dr. Nanotech",
					SkinID = 907002516u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "The Executioner",
					SkinID = 907002621u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Hurricane Delivery",
					SkinID = 907002624u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Dr. Nanotech",
					SkinID = 907102516u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "The Executioner",
					SkinID = 907102621u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Hurricane Delivery",
					SkinID = 907102624u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "FFWS 2021",
					SkinID = 907102719u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Amber Megacypher",
					SkinID = 907102917u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Big Bang",
					SkinID = 907103102u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S30 Exclusive: Xayne",
					SkinID = 907103523u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "7",
					SkinID = 907104134u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Sandust Shimmer",
					SkinID = 907104809u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Cyber Invader",
					SkinID = 907105317u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Drowning Blizzard",
					SkinID = 907136004u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136041u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Dr. Nanotech",
					SkinID = 907202516u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "The Executioner",
					SkinID = 907202621u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Hurricane Delivery",
					SkinID = 907202624u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Galaxy Monstron",
					SkinID = 907103701u,
					Color = 4281571839u
				}
			}
		},
		{
			55u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Ace of Hearts",
					SkinID = 907104632u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Crowned Conqueror",
					SkinID = 907104225u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Horsin' Holiday",
					SkinID = 907104603u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Dark Matter",
					SkinID = 907104810u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Wavebreaker Kaze",
					SkinID = 907103004u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Color Blaze",
					SkinID = 907103214u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "The Golden M1917",
					SkinID = 907104066u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Bubble Trouble",
					SkinID = 907105111u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Goal Kick",
					SkinID = 907105309u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136082u,
					Color = 4294901760u
				}
			}
		},
		{
			56u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Ebony & Ivory",
					SkinID = 907103858u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Ivory",
					SkinID = 907103860u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Fuji Folklore",
					SkinID = 907002501u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Fuji Folklore",
					SkinID = 907102501u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Sharp Tactician",
					SkinID = 907104913u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Sharp Tactician",
					SkinID = 907104933u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Fuji Folklore",
					SkinID = 907202501u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "The Falconer",
					SkinID = 907102722u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Fire Bones",
					SkinID = 907103125u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Copper Light",
					SkinID = 907103201u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "2",
					SkinID = 907104021u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Poison Tooth",
					SkinID = 907104413u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Poison Teeth",
					SkinID = 907104414u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907105331u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136084u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Waggor's Wonder",
					SkinID = 907104059u,
					Color = 4278255360u
				}
			}
		},
		{
			58u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Space Voyage",
					SkinID = 907103910u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Space Trip",
					SkinID = 907103911u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Space Expedition",
					SkinID = 907103912u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Air Rizz",
					SkinID = 907105139u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Amber Megacypher",
					SkinID = 907102916u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Phoenix Knight",
					SkinID = 907103115u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Line Art Pro",
					SkinID = 907105017u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Uncharted Voyager",
					SkinID = 907105216u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Lethal Finstooth",
					SkinID = 907102821u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Frenzy Bunny",
					SkinID = 907103323u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Paradise Defender",
					SkinID = 907103513u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Steel Cowboy",
					SkinID = 907103711u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "The Golden Mini Uzi",
					SkinID = 907103922u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Eternal Enigma",
					SkinID = 907104615u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136079u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Waggor's Wonder",
					SkinID = 907104061u,
					Color = 4278255360u
				}
			}
		},
		{
			60u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Platinum Divinity",
					SkinID = 907193301u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Platinum Divinity",
					SkinID = 907193302u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Platinum Divinity",
					SkinID = 907193303u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Platinum Divinity",
					SkinID = 907193304u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Platinum Divinity",
					SkinID = 907193305u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Platinum Divinity",
					SkinID = 907193306u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Platinum Divinity",
					SkinID = 907193307u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Spinning Bird",
					SkinID = 907002834u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Spinning Bird",
					SkinID = 907102834u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Frozen Lava",
					SkinID = 907102911u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Meta Lava",
					SkinID = 907102912u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Jungle Lava",
					SkinID = 907102913u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Rogue Lava",
					SkinID = 907102914u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Aurora Oni",
					SkinID = 907103113u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Cyan Ghost",
					SkinID = 907103451u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Crowned Glory",
					SkinID = 907105138u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Champion Boxer",
					SkinID = 907002408u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cyber Bounty Hunter",
					SkinID = 907002525u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "The Golden MP5",
					SkinID = 907002632u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Champion Boxer",
					SkinID = 907102408u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cyber Bounty Hunter",
					SkinID = 907102525u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "The Golden MP5",
					SkinID = 907102632u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Mr. Nutcracker",
					SkinID = 907102716u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Achiever",
					SkinID = 907103818u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Sublime Scorpio",
					SkinID = 907103908u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Troublemaker",
					SkinID = 907105019u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Champion Boxer",
					SkinID = 907202408u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cyber Bounty Hunter",
					SkinID = 907202525u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "The Golden MP5",
					SkinID = 907202632u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Imperial Rome",
					SkinID = 907001312u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Blood Red",
					SkinID = 907001317u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Red Samurai",
					SkinID = 907001409u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Star Soul",
					SkinID = 907001510u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death's Eye",
					SkinID = 907001516u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pink Devil",
					SkinID = 907001625u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Scorpion",
					SkinID = 907001629u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Vampire",
					SkinID = 907001706u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Old Fashioned",
					SkinID = 907001718u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Demolitionist",
					SkinID = 907001921u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Hipster Bunny",
					SkinID = 907002108u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Master of Minds",
					SkinID = 907002402u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Imperial Rome",
					SkinID = 907101312u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Blood Red",
					SkinID = 907101317u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Red Samurai",
					SkinID = 907101409u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Star Soul",
					SkinID = 907101510u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death's Eye",
					SkinID = 907101516u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Star Soul",
					SkinID = 907101540u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death's Eye",
					SkinID = 907101546u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pink Devil",
					SkinID = 907101625u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Scorpion",
					SkinID = 907101629u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Vampire",
					SkinID = 907101706u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Old Fashioned",
					SkinID = 907101718u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Demolitionist",
					SkinID = 907101921u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Hipster Bunny",
					SkinID = 907102108u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Master of Minds",
					SkinID = 907102402u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Evil Enchanted",
					SkinID = 907102703u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Beastly Wrath",
					SkinID = 907103015u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Vampire Malevolence",
					SkinID = 907103217u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S40 Exclusive: Goosy",
					SkinID = 907104519u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Shadow Ops",
					SkinID = 907104840u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S35 Exclusive: Fin",
					SkinID = 907105218u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Blood Red",
					SkinID = 907111317u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pigment Splash",
					SkinID = 907136012u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136044u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pink Devil",
					SkinID = 907201625u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Scorpion",
					SkinID = 907201629u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Vampire",
					SkinID = 907201706u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Old Fashioned",
					SkinID = 907201718u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Demolitionist",
					SkinID = 907201921u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Hipster Bunny",
					SkinID = 907202108u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Master of Minds",
					SkinID = 907202402u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Candy Bunny",
					SkinID = 907103808u,
					Color = 4281571839u
				}
			}
		},
		{
			61u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Dreambreaker",
					SkinID = 907195111u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Dreambreaker",
					SkinID = 907195112u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Dreambreaker",
					SkinID = 907195113u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Dreambreaker",
					SkinID = 907195114u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Dreambreaker",
					SkinID = 907195115u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Dreambreaker",
					SkinID = 907195116u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Dreambreaker",
					SkinID = 907195117u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Azure Stormbringer",
					SkinID = 907102806u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Frost Sabertooth",
					SkinID = 907102807u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Volcanic Whirlwind",
					SkinID = 907102808u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Shadow Earthshaker",
					SkinID = 907102809u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Deep Sea Tentacle",
					SkinID = 907104428u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Gold Coated",
					SkinID = 907001316u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Golden Rule",
					SkinID = 907001701u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Crimson Red",
					SkinID = 907001821u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Santa's Choice",
					SkinID = 907001910u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Captain Bubbles",
					SkinID = 907002214u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Spirited Overseers",
					SkinID = 907002306u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Viper Gangster",
					SkinID = 907002622u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Gold Coated",
					SkinID = 907101316u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Gold Coated",
					SkinID = 907101326u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Golden Rule",
					SkinID = 907101701u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Crimson Red",
					SkinID = 907101821u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Santa's Choice",
					SkinID = 907101910u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Captain Bubbles",
					SkinID = 907102214u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Spirited Overseers",
					SkinID = 907102306u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Viper Gangster",
					SkinID = 907102622u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Reel Shooter",
					SkinID = 907104128u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Wisp",
					SkinID = 907104409u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Akatsuki Theme",
					SkinID = 907105005u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Gold Coated",
					SkinID = 907111316u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Golden Rule",
					SkinID = 907201701u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Crimson Red",
					SkinID = 907201821u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Santa's Choice",
					SkinID = 907201910u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Captain Bubbles",
					SkinID = 907202214u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Spirited Overseers",
					SkinID = 907202306u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Viper Gangster",
					SkinID = 907202622u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Lively Beast",
					SkinID = 907001603u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pink Devil",
					SkinID = 907001626u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pirate's Soul",
					SkinID = 907001828u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Agent Paws",
					SkinID = 907002007u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Victory Wings",
					SkinID = 907002201u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Justice Fighter",
					SkinID = 907002318u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Swordsman Legends",
					SkinID = 907002406u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Lively Beast",
					SkinID = 907101603u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pink Devil",
					SkinID = 907101626u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pirate's Soul",
					SkinID = 907101828u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Agent Paws",
					SkinID = 907102007u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Victory Wings",
					SkinID = 907102201u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Justice Fighter",
					SkinID = 907102318u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Swordsman Legends",
					SkinID = 907102406u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Ice Blossoms",
					SkinID = 907103126u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Porcelain Rush",
					SkinID = 907103204u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Sublime Scorpio",
					SkinID = 907103909u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136055u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Lively Beast",
					SkinID = 907201603u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pink Devil",
					SkinID = 907201626u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pirate's Soul",
					SkinID = 907201828u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Agent Paws",
					SkinID = 907202007u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Victory Wings",
					SkinID = 907202201u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Justice Fighter",
					SkinID = 907202318u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Swordsman Legends",
					SkinID = 907202406u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Zombie Attack",
					SkinID = 907104103u,
					Color = 4278255360u
				}
			}
		},
		{
			65u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Duke Swallowtail",
					SkinID = 907002217u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "F┼½rinkazan",
					SkinID = 907002833u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Duke Swallowtail",
					SkinID = 907102217u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "F┼½rinkazan",
					SkinID = 907102833u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Metal Wings",
					SkinID = 907103233u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Crimson Firehorn",
					SkinID = 907103508u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Mossy Vinehorn",
					SkinID = 907103509u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Iron Etherhorn",
					SkinID = 907103510u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Titanium Warhorn",
					SkinID = 907103511u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Duke Swallowtail",
					SkinID = 907202217u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Lucky Koi",
					SkinID = 907002616u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Lucky Koi",
					SkinID = 907102616u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Wavebreaker Kaze",
					SkinID = 907103003u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Breath of Silence",
					SkinID = 907104411u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Aurora's Shade",
					SkinID = 907104712u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Lucky Koi",
					SkinID = 907202616u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Hunter's Eye",
					SkinID = 907001608u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Old Fashioned",
					SkinID = 907001717u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Tagger's Revolt",
					SkinID = 907001913u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Agent Paws",
					SkinID = 907002008u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Valentines",
					SkinID = 907002017u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Vandal Revolt",
					SkinID = 907002024u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Justice Fighter",
					SkinID = 907002317u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Booyah Day",
					SkinID = 907002417u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Hunter's Eye",
					SkinID = 907101608u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Old Fashioned",
					SkinID = 907101717u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Tagger's Revolt",
					SkinID = 907101913u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Agent Paws",
					SkinID = 907102008u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Valentines",
					SkinID = 907102017u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Vandal Revolt",
					SkinID = 907102024u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Justice Fighter",
					SkinID = 907102317u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Booyah Day",
					SkinID = 907102417u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Xtreme Adventure",
					SkinID = 907102728u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Skull Punker",
					SkinID = 907103033u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Haunt's Outrage",
					SkinID = 907104016u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S32 Exclusive: Ash",
					SkinID = 907105039u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136069u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Hunter's Eye",
					SkinID = 907201608u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Old Fashioned",
					SkinID = 907201717u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Tagger's Revolt",
					SkinID = 907201913u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Agent Paws",
					SkinID = 907202008u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Valentines",
					SkinID = 907202017u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Vandal Revolt",
					SkinID = 907202024u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Justice Fighter",
					SkinID = 907202317u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Booyah Day",
					SkinID = 907202417u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Interstellar Igniter",
					SkinID = 907104217u,
					Color = 4278255360u
				},
				new SkinInfo
				{
					Name = "Frostfire Snip",
					SkinID = 907104222u,
					Color = 4281571839u
				},
				new SkinInfo
				{
					Name = "Bamboo Warrior",
					SkinID = 907105014u,
					Color = 4281571839u
				}
			}
		},
		{
			67u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Demonic Grin",
					SkinID = 907192901u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Demonic Grin",
					SkinID = 907192902u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Demonic Grin",
					SkinID = 907192903u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Demonic Grin",
					SkinID = 907192904u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Demonic Grin",
					SkinID = 907192905u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Demonic Grin",
					SkinID = 907192906u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Demonic Grin",
					SkinID = 907192907u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Black Widow",
					SkinID = 907001615u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Ocean",
					SkinID = 907001616u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Golden",
					SkinID = 907001617u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Void",
					SkinID = 907001618u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Lightning",
					SkinID = 907001619u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow",
					SkinID = 907101615u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Ocean",
					SkinID = 907101616u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Golden",
					SkinID = 907101617u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Void",
					SkinID = 907101618u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Lightning",
					SkinID = 907101619u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Lava",
					SkinID = 907104311u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow",
					SkinID = 907201615u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Ocean",
					SkinID = 907201616u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Golden",
					SkinID = 907201617u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Void",
					SkinID = 907201618u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Lightning",
					SkinID = 907201619u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Metallic",
					SkinID = 907001440u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Vampire",
					SkinID = 907001705u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Warrior's Spirit",
					SkinID = 907001815u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Swagger Ownage",
					SkinID = 907001917u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "The Golden FAMAS",
					SkinID = 907002437u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Metallic",
					SkinID = 907101440u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Metallic",
					SkinID = 907101445u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Vampire",
					SkinID = 907101705u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Warrior's Spirit",
					SkinID = 907101815u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Swagger Ownage",
					SkinID = 907101917u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "The Golden FAMAS",
					SkinID = 907102437u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Wisdom's Vision",
					SkinID = 907104005u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "BLUE LOCK",
					SkinID = 907104652u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "HEARTROCKER",
					SkinID = 907105136u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Sculpted Excellence",
					SkinID = 907105313u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Vampire",
					SkinID = 907201705u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Warrior's Spirit",
					SkinID = 907201815u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Swagger Ownage",
					SkinID = 907201917u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "The Golden FAMAS",
					SkinID = 907202437u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Digital Invasion",
					SkinID = 907001308u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Imperial Rome",
					SkinID = 907001314u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Dragon's Eye",
					SkinID = 907001401u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Shark Attack",
					SkinID = 907001413u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Ice Blue",
					SkinID = 907001423u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pirate's Soul",
					SkinID = 907001827u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Loose Cannon",
					SkinID = 907001923u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Kpop Stardom",
					SkinID = 907002118u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Private Eye",
					SkinID = 907002224u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Wilderness Hunter",
					SkinID = 907002427u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Digital Invasion",
					SkinID = 907101308u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Imperial Rome",
					SkinID = 907101314u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Dragon's Eye",
					SkinID = 907101401u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Shark Attack",
					SkinID = 907101413u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Ice Blue",
					SkinID = 907101423u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pirate's Soul",
					SkinID = 907101827u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Loose Cannon",
					SkinID = 907101923u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Kpop Stardom",
					SkinID = 907102118u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Private Eye",
					SkinID = 907102224u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Wilderness Hunter",
					SkinID = 907102427u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Whirlwind Crease",
					SkinID = 907103105u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S26 Exclusive: Laura",
					SkinID = 907103128u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S30 Exclusive: Firefly",
					SkinID = 907104832u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Dragon Skin",
					SkinID = 907136019u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136053u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pirate's Soul",
					SkinID = 907201827u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Loose Cannon",
					SkinID = 907201923u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Kpop Stardom",
					SkinID = 907202118u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Private Eye",
					SkinID = 907202224u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Wilderness Hunter",
					SkinID = 907202427u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Waggor's Wonder",
					SkinID = 907104060u,
					Color = 4278255360u
				},
				new SkinInfo
				{
					Name = "Midnight Cityscape",
					SkinID = 907104808u,
					Color = 4281571839u
				}
			}
		},
		{
			70u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Bang! Popblaster",
					SkinID = 907194111u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Bang! Popblaster",
					SkinID = 907194112u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Bang! Popblaster",
					SkinID = 907194113u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Bang! Popblaster",
					SkinID = 907194114u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Bang! Popblaster",
					SkinID = 907194115u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Bang! Popblaster",
					SkinID = 907194116u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Bang! Popblaster",
					SkinID = 907194117u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Jewel Mystified",
					SkinID = 907103206u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Flames Enchanted",
					SkinID = 907103207u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Airburst Entranced",
					SkinID = 907103208u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Thunder Electrified",
					SkinID = 907103209u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Bony Tunes",
					SkinID = 907104610u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Tagger's Revolt",
					SkinID = 907001912u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Great Plunder",
					SkinID = 907002312u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Tagger's Revolt",
					SkinID = 907101912u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Great Plunder",
					SkinID = 907102312u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Operano Sheng",
					SkinID = 907102920u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Phantom Predator",
					SkinID = 907103030u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Golden Roar",
					SkinID = 907103111u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Sterling Futurnetic",
					SkinID = 907103514u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Heartseeker",
					SkinID = 907103817u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Poppin' Shootin'",
					SkinID = 907104017u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Sasuke Theme",
					SkinID = 907104743u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Night Spookster",
					SkinID = 907105113u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Tagger's Revolt",
					SkinID = 907201912u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Great Plunder",
					SkinID = 907202312u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Winterlands",
					SkinID = 907000075u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "BOOYAH",
					SkinID = 907001622u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Swagger Ownage",
					SkinID = 907001902u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "FFCS",
					SkinID = 907002415u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Winterlands",
					SkinID = 907100075u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "BOOYAH",
					SkinID = 907101622u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Swagger Ownage",
					SkinID = 907101902u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "FFCS",
					SkinID = 907102415u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S24 Exclusive: Wrathful",
					SkinID = 907104321u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136057u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "BOOYAH",
					SkinID = 907201622u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Swagger Ownage",
					SkinID = 907201902u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "FFCS",
					SkinID = 907202415u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Reel On",
					SkinID = 907104137u,
					Color = 4278255360u
				},
				new SkinInfo
				{
					Name = "Runestone Sigil",
					SkinID = 907103959u,
					Color = 4281571839u
				},
				new SkinInfo
				{
					Name = "Yuji Itadori",
					SkinID = 907105201u,
					Color = 4281571839u
				}
			}
		},
		{
			71u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Fabled Foxes",
					SkinID = 907002102u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Endless Oblivion",
					SkinID = 907002429u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Fabled Foxes",
					SkinID = 907102102u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Endless Oblivion",
					SkinID = 907102429u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Riverdust Splasher",
					SkinID = 907103516u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Steel Cowboy",
					SkinID = 907103710u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cosmic Chaos",
					SkinID = 907104911u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Celestial Archer",
					SkinID = 907105114u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Fabled Foxes",
					SkinID = 907202102u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Endless Oblivion",
					SkinID = 907202429u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Striking Pink",
					SkinID = 907000056u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "War Machine",
					SkinID = 907001303u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Blood Hockey",
					SkinID = 907001805u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Spikey Spine",
					SkinID = 907002112u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Clownfish",
					SkinID = 907002215u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "MMA Fighter",
					SkinID = 907002425u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Superstar",
					SkinID = 907002629u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Striking Pink",
					SkinID = 907100056u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "War Machine",
					SkinID = 907101303u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Blood Hockey",
					SkinID = 907101805u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Spikey Spine",
					SkinID = 907102112u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Clownfish",
					SkinID = 907102215u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "MMA Fighter",
					SkinID = 907102425u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Superstar",
					SkinID = 907102629u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mesmerizing Nights",
					SkinID = 907102908u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Disc Jockey",
					SkinID = 907102934u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Fire Bones",
					SkinID = 907103118u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Violet Fear",
					SkinID = 907103418u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Jam Sessions",
					SkinID = 907103902u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Storm Surge",
					SkinID = 907103919u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Poppin' Griddin'",
					SkinID = 907104018u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Wisp",
					SkinID = 907104410u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S43 Exclusive: Stitched",
					SkinID = 907104724u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136061u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136077u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Blood Hockey",
					SkinID = 907201805u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Spikey Spine",
					SkinID = 907202112u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Clownfish",
					SkinID = 907202215u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "MMA Fighter",
					SkinID = 907202425u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Superstar",
					SkinID = 907202629u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Waggor's Wonder",
					SkinID = 907104058u,
					Color = 4278255360u
				},
				new SkinInfo
				{
					Name = "Zombie Attack",
					SkinID = 907104104u,
					Color = 4278255360u
				}
			}
		},
		{
			72u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Chainsaw",
					SkinID = 907104539u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Swordsman Legends",
					SkinID = 907002405u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Swordsman Legends",
					SkinID = 907102405u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Eternal Enigma",
					SkinID = 907104614u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Chillin' Emoji",
					SkinID = 907105217u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Nightslayer Grizzly",
					SkinID = 907136017u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Swordsman Legends",
					SkinID = 907202405u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Artificial Intelligence",
					SkinID = 907001725u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Loose Cannon",
					SkinID = 907001925u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Spirited Overseers",
					SkinID = 907002307u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Dr. Nanotech",
					SkinID = 907002517u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Viper Gangster",
					SkinID = 907002623u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Artificial Intelligence",
					SkinID = 907101725u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Loose Cannon",
					SkinID = 907101925u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Spirited Overseers",
					SkinID = 907102307u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Dr. Nanotech",
					SkinID = 907102517u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Viper Gangster",
					SkinID = 907102623u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "The Falconer",
					SkinID = 907102725u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Frozen Platinum",
					SkinID = 907103213u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Sound Crafter",
					SkinID = 907103402u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S25 Exclusive: Breaker",
					SkinID = 907104423u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136066u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Artificial Intelligence",
					SkinID = 907201725u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Loose Cannon",
					SkinID = 907201925u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Spirited Overseers",
					SkinID = 907202307u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Dr. Nanotech",
					SkinID = 907202517u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Viper Gangster",
					SkinID = 907202623u,
					Color = 4294901760u
				}
			}
		},
		{
			73u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Bunny Mastermind",
					SkinID = 907104507u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Bones of Terror",
					SkinID = 907136015u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Flaring Bionica",
					SkinID = 907103412u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Fiery Fantasy",
					SkinID = 907103914u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Beyond Infinity",
					SkinID = 907104908u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Violet Fear",
					SkinID = 907103326u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Fundamentality",
					SkinID = 907104143u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Road Savvy",
					SkinID = 907104611u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S28 Exclusive: Horsin'",
					SkinID = 907104626u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Bedtime",
					SkinID = 907104814u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Thorned Romance",
					SkinID = 907105214u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136076u,
					Color = 4294901760u
				}
			}
		},
		{
			74u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Bunny Mastermind",
					SkinID = 907104507u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Bones of Terror",
					SkinID = 907136015u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Flaring Bionica",
					SkinID = 907103412u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Fiery Fantasy",
					SkinID = 907103914u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Beyond Infinity",
					SkinID = 907104908u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Violet Fear",
					SkinID = 907103326u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Fundamentality",
					SkinID = 907104143u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Road Savvy",
					SkinID = 907104611u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S28 Exclusive: Horsin'",
					SkinID = 907104626u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Bedtime",
					SkinID = 907104814u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Thorned Romance",
					SkinID = 907105214u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136076u,
					Color = 4294901760u
				}
			}
		},
		{
			75u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Fabled Foxes",
					SkinID = 907002102u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Endless Oblivion",
					SkinID = 907002429u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Fabled Foxes",
					SkinID = 907102102u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Endless Oblivion",
					SkinID = 907102429u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Riverdust Splasher",
					SkinID = 907103516u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Steel Cowboy",
					SkinID = 907103710u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cosmic Chaos",
					SkinID = 907104911u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Celestial Archer",
					SkinID = 907105114u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Fabled Foxes",
					SkinID = 907202102u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Endless Oblivion",
					SkinID = 907202429u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Striking Pink",
					SkinID = 907000056u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "War Machine",
					SkinID = 907001303u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Blood Hockey",
					SkinID = 907001805u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Spikey Spine",
					SkinID = 907002112u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Clownfish",
					SkinID = 907002215u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "MMA Fighter",
					SkinID = 907002425u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Superstar",
					SkinID = 907002629u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Striking Pink",
					SkinID = 907100056u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "War Machine",
					SkinID = 907101303u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Blood Hockey",
					SkinID = 907101805u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Spikey Spine",
					SkinID = 907102112u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Clownfish",
					SkinID = 907102215u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "MMA Fighter",
					SkinID = 907102425u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Superstar",
					SkinID = 907102629u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mesmerizing Nights",
					SkinID = 907102908u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Disc Jockey",
					SkinID = 907102934u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Fire Bones",
					SkinID = 907103118u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Violet Fear",
					SkinID = 907103418u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Jam Sessions",
					SkinID = 907103902u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Storm Surge",
					SkinID = 907103919u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Poppin' Griddin'",
					SkinID = 907104018u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Wisp",
					SkinID = 907104410u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S43 Exclusive: Stitched",
					SkinID = 907104724u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136061u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136077u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Blood Hockey",
					SkinID = 907201805u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Spikey Spine",
					SkinID = 907202112u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Clownfish",
					SkinID = 907202215u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "MMA Fighter",
					SkinID = 907202425u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Superstar",
					SkinID = 907202629u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Waggor's Wonder",
					SkinID = 907104058u,
					Color = 4278255360u
				},
				new SkinInfo
				{
					Name = "Zombie Attack",
					SkinID = 907104104u,
					Color = 4278255360u
				}
			}
		},
		{
			78u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Ice Bones",
					SkinID = 907103117u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Steamy Ramen",
					SkinID = 907104805u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Infiltrated Red",
					SkinID = 907103024u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Sterling Futurnetic",
					SkinID = 907103515u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Dart Electric",
					SkinID = 907103715u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136073u,
					Color = 4294901760u
				}
			}
		},
		{
			80u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Infernal Draco",
					SkinID = 907193101u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Infernal Draco",
					SkinID = 907193102u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Infernal Draco",
					SkinID = 907193103u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Infernal Draco",
					SkinID = 907193104u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Infernal Draco",
					SkinID = 907193105u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Infernal Draco",
					SkinID = 907193106u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Infernal Draco",
					SkinID = 907193107u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Griffin's Fury",
					SkinID = 907001315u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Red Griffin",
					SkinID = 907001429u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Blue Griffin",
					SkinID = 907001430u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Purple Griffin",
					SkinID = 907001431u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Genos",
					SkinID = 907002534u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Infernal Netherworld",
					SkinID = 907002612u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Shadow Netherworld",
					SkinID = 907002613u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Glacier Netherworld",
					SkinID = 907002614u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Venom Netherworld",
					SkinID = 907002615u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Griffin's Fury",
					SkinID = 907101315u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Griffin's Fury",
					SkinID = 907101325u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Red Griffin",
					SkinID = 907101429u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Blue Griffin",
					SkinID = 907101430u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Purple Griffin",
					SkinID = 907101431u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Genos",
					SkinID = 907102534u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Infernal Netherworld",
					SkinID = 907102612u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Shadow Netherworld",
					SkinID = 907102613u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Glacier Netherworld",
					SkinID = 907102614u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Venom Netherworld",
					SkinID = 907102615u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Sunrise Realm",
					SkinID = 907104202u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Stormy Ascent",
					SkinID = 907104729u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Naruto Theme",
					SkinID = 907104744u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Genos",
					SkinID = 907202534u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Infernal Netherworld",
					SkinID = 907202612u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Shadow Netherworld",
					SkinID = 907202613u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Glacier Netherworld",
					SkinID = 907202614u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Venom Netherworld",
					SkinID = 907202615u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Star General",
					SkinID = 907000071u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Wild Carnival",
					SkinID = 907001306u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Pink Laminate",
					SkinID = 907001406u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cataclysm",
					SkinID = 907001508u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Star Soul",
					SkinID = 907001509u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Scorching Sands",
					SkinID = 907001614u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Flaming Skull",
					SkinID = 907001819u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "FFCS",
					SkinID = 907002414u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "S19 Exclusive: Kelly \"the Swift\"",
					SkinID = 907002521u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Star General",
					SkinID = 907100071u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Winter Warrior",
					SkinID = 907100079u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Wild Carnival",
					SkinID = 907101306u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Wild Carnival",
					SkinID = 907101324u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Pink Laminate",
					SkinID = 907101406u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Pink Laminate",
					SkinID = 907101435u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cataclysm",
					SkinID = 907101508u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Star Soul",
					SkinID = 907101509u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cataclysm",
					SkinID = 907101538u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Star Soul",
					SkinID = 907101539u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Scorching Sands",
					SkinID = 907101614u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Flaming Skull",
					SkinID = 907101819u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "FFCS",
					SkinID = 907102414u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "S19 Exclusive: Kelly \"the Swift\"",
					SkinID = 907102521u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Feline's Burst",
					SkinID = 907104223u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Zombie Attack",
					SkinID = 907104316u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Goal Kick",
					SkinID = 907105308u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Scorching Sands",
					SkinID = 907201614u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Flaming Skull",
					SkinID = 907201819u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "FFCS",
					SkinID = 907202414u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "S19 Exclusive: Kelly \"the Swift\"",
					SkinID = 907202521u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Winter Warrior",
					SkinID = 907000079u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Flaming Wolf",
					SkinID = 907000087u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Ice Blue",
					SkinID = 907001421u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death Wish",
					SkinID = 907001502u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Scorpion",
					SkinID = 907001628u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Artificial Intelligence",
					SkinID = 907001722u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Deadly Bat",
					SkinID = 907001810u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Vandal Revolt",
					SkinID = 907002022u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Master of Minds",
					SkinID = 907002404u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Dr. Nanotech",
					SkinID = 907002518u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Flaming Wolf",
					SkinID = 907100087u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Ice Blue",
					SkinID = 907101421u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death Wish",
					SkinID = 907101502u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death Wish",
					SkinID = 907101532u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Scorpion",
					SkinID = 907101628u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Artificial Intelligence",
					SkinID = 907101722u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Deadly Bat",
					SkinID = 907101810u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Vandal Revolt",
					SkinID = 907102022u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Master of Minds",
					SkinID = 907102404u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Dr. Nanotech",
					SkinID = 907102518u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "The Golden M4A1",
					SkinID = 907103010u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Skull Punker",
					SkinID = 907103032u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Sterling Futurnetic",
					SkinID = 907103502u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Waggor Swagger",
					SkinID = 907104540u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Waggor Swagger",
					SkinID = 907104541u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Trouble Academy",
					SkinID = 907105022u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S36 Exclusive: Emoji",
					SkinID = 907105220u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136054u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Scorpion",
					SkinID = 907201628u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Artificial Intelligence",
					SkinID = 907201722u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Deadly Bat",
					SkinID = 907201810u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Vandal Revolt",
					SkinID = 907202022u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Master of Minds",
					SkinID = 907202404u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Dr. Nanotech",
					SkinID = 907202518u,
					Color = 4294901760u
				}
			}
		},
		{
			81u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Infernal Draco",
					SkinID = 907193101u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Infernal Draco",
					SkinID = 907193102u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Infernal Draco",
					SkinID = 907193103u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Infernal Draco",
					SkinID = 907193104u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Infernal Draco",
					SkinID = 907193105u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Infernal Draco",
					SkinID = 907193106u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Infernal Draco",
					SkinID = 907193107u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Griffin's Fury",
					SkinID = 907001315u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Red Griffin",
					SkinID = 907001429u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Blue Griffin",
					SkinID = 907001430u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Purple Griffin",
					SkinID = 907001431u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Genos",
					SkinID = 907002534u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Infernal Netherworld",
					SkinID = 907002612u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Shadow Netherworld",
					SkinID = 907002613u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Glacier Netherworld",
					SkinID = 907002614u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Venom Netherworld",
					SkinID = 907002615u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Griffin's Fury",
					SkinID = 907101315u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Griffin's Fury",
					SkinID = 907101325u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Red Griffin",
					SkinID = 907101429u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Blue Griffin",
					SkinID = 907101430u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Purple Griffin",
					SkinID = 907101431u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Genos",
					SkinID = 907102534u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Infernal Netherworld",
					SkinID = 907102612u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Shadow Netherworld",
					SkinID = 907102613u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Glacier Netherworld",
					SkinID = 907102614u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Venom Netherworld",
					SkinID = 907102615u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Sunrise Realm",
					SkinID = 907104202u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Stormy Ascent",
					SkinID = 907104729u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Naruto Theme",
					SkinID = 907104744u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Genos",
					SkinID = 907202534u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Infernal Netherworld",
					SkinID = 907202612u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Shadow Netherworld",
					SkinID = 907202613u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Glacier Netherworld",
					SkinID = 907202614u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Venom Netherworld",
					SkinID = 907202615u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Star General",
					SkinID = 907000071u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Wild Carnival",
					SkinID = 907001306u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Pink Laminate",
					SkinID = 907001406u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cataclysm",
					SkinID = 907001508u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Star Soul",
					SkinID = 907001509u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Scorching Sands",
					SkinID = 907001614u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Flaming Skull",
					SkinID = 907001819u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "FFCS",
					SkinID = 907002414u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "S19 Exclusive: Kelly \"the Swift\"",
					SkinID = 907002521u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Star General",
					SkinID = 907100071u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Winter Warrior",
					SkinID = 907100079u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Wild Carnival",
					SkinID = 907101306u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Wild Carnival",
					SkinID = 907101324u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Pink Laminate",
					SkinID = 907101406u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Pink Laminate",
					SkinID = 907101435u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cataclysm",
					SkinID = 907101508u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Star Soul",
					SkinID = 907101509u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cataclysm",
					SkinID = 907101538u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Star Soul",
					SkinID = 907101539u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Scorching Sands",
					SkinID = 907101614u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Flaming Skull",
					SkinID = 907101819u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "FFCS",
					SkinID = 907102414u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "S19 Exclusive: Kelly \"the Swift\"",
					SkinID = 907102521u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Feline's Burst",
					SkinID = 907104223u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Zombie Attack",
					SkinID = 907104316u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Goal Kick",
					SkinID = 907105308u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Scorching Sands",
					SkinID = 907201614u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Flaming Skull",
					SkinID = 907201819u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "FFCS",
					SkinID = 907202414u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "S19 Exclusive: Kelly \"the Swift\"",
					SkinID = 907202521u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Winter Warrior",
					SkinID = 907000079u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Flaming Wolf",
					SkinID = 907000087u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Ice Blue",
					SkinID = 907001421u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death Wish",
					SkinID = 907001502u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Scorpion",
					SkinID = 907001628u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Artificial Intelligence",
					SkinID = 907001722u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Deadly Bat",
					SkinID = 907001810u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Vandal Revolt",
					SkinID = 907002022u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Master of Minds",
					SkinID = 907002404u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Dr. Nanotech",
					SkinID = 907002518u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Flaming Wolf",
					SkinID = 907100087u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Ice Blue",
					SkinID = 907101421u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death Wish",
					SkinID = 907101502u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death Wish",
					SkinID = 907101532u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Scorpion",
					SkinID = 907101628u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Artificial Intelligence",
					SkinID = 907101722u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Deadly Bat",
					SkinID = 907101810u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Vandal Revolt",
					SkinID = 907102022u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Master of Minds",
					SkinID = 907102404u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Dr. Nanotech",
					SkinID = 907102518u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "The Golden M4A1",
					SkinID = 907103010u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Skull Punker",
					SkinID = 907103032u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Sterling Futurnetic",
					SkinID = 907103502u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Waggor Swagger",
					SkinID = 907104540u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Waggor Swagger",
					SkinID = 907104541u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Trouble Academy",
					SkinID = 907105022u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S36 Exclusive: Emoji",
					SkinID = 907105220u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136054u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Scorpion",
					SkinID = 907201628u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Artificial Intelligence",
					SkinID = 907201722u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Deadly Bat",
					SkinID = 907201810u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Vandal Revolt",
					SkinID = 907202022u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Master of Minds",
					SkinID = 907202404u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Dr. Nanotech",
					SkinID = 907202518u,
					Color = 4294901760u
				}
			}
		},
		{
			82u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Infernal Draco",
					SkinID = 907193101u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Infernal Draco",
					SkinID = 907193102u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Infernal Draco",
					SkinID = 907193103u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Infernal Draco",
					SkinID = 907193104u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Infernal Draco",
					SkinID = 907193105u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Infernal Draco",
					SkinID = 907193106u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Infernal Draco",
					SkinID = 907193107u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Griffin's Fury",
					SkinID = 907001315u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Red Griffin",
					SkinID = 907001429u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Blue Griffin",
					SkinID = 907001430u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Purple Griffin",
					SkinID = 907001431u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Genos",
					SkinID = 907002534u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Infernal Netherworld",
					SkinID = 907002612u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Shadow Netherworld",
					SkinID = 907002613u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Glacier Netherworld",
					SkinID = 907002614u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Venom Netherworld",
					SkinID = 907002615u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Griffin's Fury",
					SkinID = 907101315u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Griffin's Fury",
					SkinID = 907101325u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Red Griffin",
					SkinID = 907101429u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Blue Griffin",
					SkinID = 907101430u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Purple Griffin",
					SkinID = 907101431u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Genos",
					SkinID = 907102534u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Infernal Netherworld",
					SkinID = 907102612u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Shadow Netherworld",
					SkinID = 907102613u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Glacier Netherworld",
					SkinID = 907102614u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Venom Netherworld",
					SkinID = 907102615u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Sunrise Realm",
					SkinID = 907104202u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Stormy Ascent",
					SkinID = 907104729u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Naruto Theme",
					SkinID = 907104744u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Genos",
					SkinID = 907202534u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Infernal Netherworld",
					SkinID = 907202612u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Shadow Netherworld",
					SkinID = 907202613u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Glacier Netherworld",
					SkinID = 907202614u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Venom Netherworld",
					SkinID = 907202615u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Star General",
					SkinID = 907000071u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Wild Carnival",
					SkinID = 907001306u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Pink Laminate",
					SkinID = 907001406u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cataclysm",
					SkinID = 907001508u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Star Soul",
					SkinID = 907001509u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Scorching Sands",
					SkinID = 907001614u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Flaming Skull",
					SkinID = 907001819u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "FFCS",
					SkinID = 907002414u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "S19 Exclusive: Kelly \"the Swift\"",
					SkinID = 907002521u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Star General",
					SkinID = 907100071u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Winter Warrior",
					SkinID = 907100079u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Wild Carnival",
					SkinID = 907101306u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Wild Carnival",
					SkinID = 907101324u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Pink Laminate",
					SkinID = 907101406u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Pink Laminate",
					SkinID = 907101435u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cataclysm",
					SkinID = 907101508u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Star Soul",
					SkinID = 907101509u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cataclysm",
					SkinID = 907101538u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Star Soul",
					SkinID = 907101539u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Scorching Sands",
					SkinID = 907101614u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Flaming Skull",
					SkinID = 907101819u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "FFCS",
					SkinID = 907102414u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "S19 Exclusive: Kelly \"the Swift\"",
					SkinID = 907102521u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Feline's Burst",
					SkinID = 907104223u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Zombie Attack",
					SkinID = 907104316u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Goal Kick",
					SkinID = 907105308u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Scorching Sands",
					SkinID = 907201614u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Flaming Skull",
					SkinID = 907201819u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "FFCS",
					SkinID = 907202414u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "S19 Exclusive: Kelly \"the Swift\"",
					SkinID = 907202521u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Winter Warrior",
					SkinID = 907000079u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Flaming Wolf",
					SkinID = 907000087u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Ice Blue",
					SkinID = 907001421u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death Wish",
					SkinID = 907001502u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Scorpion",
					SkinID = 907001628u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Artificial Intelligence",
					SkinID = 907001722u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Deadly Bat",
					SkinID = 907001810u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Vandal Revolt",
					SkinID = 907002022u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Master of Minds",
					SkinID = 907002404u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Dr. Nanotech",
					SkinID = 907002518u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Flaming Wolf",
					SkinID = 907100087u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Ice Blue",
					SkinID = 907101421u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death Wish",
					SkinID = 907101502u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death Wish",
					SkinID = 907101532u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Scorpion",
					SkinID = 907101628u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Artificial Intelligence",
					SkinID = 907101722u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Deadly Bat",
					SkinID = 907101810u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Vandal Revolt",
					SkinID = 907102022u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Master of Minds",
					SkinID = 907102404u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Dr. Nanotech",
					SkinID = 907102518u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "The Golden M4A1",
					SkinID = 907103010u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Skull Punker",
					SkinID = 907103032u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Sterling Futurnetic",
					SkinID = 907103502u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Waggor Swagger",
					SkinID = 907104540u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Waggor Swagger",
					SkinID = 907104541u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Trouble Academy",
					SkinID = 907105022u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S36 Exclusive: Emoji",
					SkinID = 907105220u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136054u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Scorpion",
					SkinID = 907201628u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Artificial Intelligence",
					SkinID = 907201722u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Deadly Bat",
					SkinID = 907201810u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Vandal Revolt",
					SkinID = 907202022u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Master of Minds",
					SkinID = 907202404u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Dr. Nanotech",
					SkinID = 907202518u,
					Color = 4294901760u
				}
			}
		},
		{
			88u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Mind's Eye",
					SkinID = 907103859u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Iron Iridescence",
					SkinID = 907104105u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Silver Iridescence",
					SkinID = 907104106u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Golden Iridescence",
					SkinID = 907104107u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Ionic Theory",
					SkinID = 907104710u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Frozen Platinum",
					SkinID = 907103212u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Enflamed Terror",
					SkinID = 907103416u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Ribbit Rain",
					SkinID = 907104303u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Bubble Trouble",
					SkinID = 907105110u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cyan Fear",
					SkinID = 907103121u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "The Golden MAC10",
					SkinID = 907103812u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Metro Flash",
					SkinID = 907104818u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S44 Exclusive: Manga",
					SkinID = 907104831u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Cosmic Chaos",
					SkinID = 907104912u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Bones of Terror",
					SkinID = 907136016u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136074u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Wisdom's Fire",
					SkinID = 907104064u,
					Color = 4278255360u
				},
				new SkinInfo
				{
					Name = "Eclipse Nova",
					SkinID = 907105301u,
					Color = 4281571839u
				}
			}
		},
		{
			89u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Scorpio Bane",
					SkinID = 907193801u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Scorpio Bane",
					SkinID = 907193802u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Scorpio Bane",
					SkinID = 907193803u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Scorpio Bane",
					SkinID = 907193804u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Scorpio Bane",
					SkinID = 907193805u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Scorpio Bane",
					SkinID = 907193806u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Scorpio Bane",
					SkinID = 907193807u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Phantasmal Touch",
					SkinID = 907104002u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Phantasmal Claws",
					SkinID = 907104003u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Phantasmal Grasp",
					SkinID = 907104004u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Aura of Chaos",
					SkinID = 907104608u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Royal Warrior",
					SkinID = 907103008u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cannibal Crush",
					SkinID = 907103706u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "The Finisher",
					SkinID = 907104073u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Inner Whisper",
					SkinID = 907104314u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Anemo Limelight",
					SkinID = 907104509u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Fearless Resolve",
					SkinID = 907104903u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Fury Tribe",
					SkinID = 907102922u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Golden Roar",
					SkinID = 907103112u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Rave Skater",
					SkinID = 907103319u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Digital Dasher",
					SkinID = 907103415u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "The Golden AC80",
					SkinID = 907103522u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S33 Exclusive: Homer",
					SkinID = 907103813u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136068u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Waggor's Wonder",
					SkinID = 907104057u,
					Color = 4278255360u
				},
				new SkinInfo
				{
					Name = "Immortal Ignition",
					SkinID = 907103832u,
					Color = 4281571839u
				}
			}
		},
		{
			119u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Sterling Conqueror",
					SkinID = 907193501u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Sterling Conqueror",
					SkinID = 907193502u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Sterling Conqueror",
					SkinID = 907193503u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Sterling Conqueror",
					SkinID = 907193504u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Sterling Conqueror",
					SkinID = 907193505u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Sterling Conqueror",
					SkinID = 907193506u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Sterling Conqueror",
					SkinID = 907193507u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "One Punch Man",
					SkinID = 907002535u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Hand of Hope",
					SkinID = 907002712u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "One Punch Man",
					SkinID = 907102535u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Hand of Hope",
					SkinID = 907102712u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Golden Glare",
					SkinID = 907102937u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Incendium Burst",
					SkinID = 907103107u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Aqua Burst",
					SkinID = 907103108u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Solaris Burst",
					SkinID = 907103109u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Terrano Burst",
					SkinID = 907103110u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Emerald Power",
					SkinID = 907103316u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Eagle Gaze",
					SkinID = 907103916u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "One Punch Man",
					SkinID = 907202535u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Rapper Underworld",
					SkinID = 907002309u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Winterlands 2020",
					SkinID = 907002512u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Freezing Flame",
					SkinID = 907002732u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Rapper Underworld",
					SkinID = 907102309u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Winterlands 2020",
					SkinID = 907102512u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Freezing Flame",
					SkinID = 907102732u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Troublemaker",
					SkinID = 907105018u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Rapper Underworld",
					SkinID = 907202309u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Winterlands 2020",
					SkinID = 907202512u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Tropical Parrot",
					SkinID = 907002123u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Xtreme Adventure",
					SkinID = 907002727u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Tropical Parrot",
					SkinID = 907102123u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Xtreme Adventure",
					SkinID = 907102727u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Bunny's Order",
					SkinID = 907104020u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Glistening Nightstar",
					SkinID = 907104309u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136040u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Tropical Parrot",
					SkinID = 907202123u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Sandstorm Shimmer",
					SkinID = 907104807u,
					Color = 4281571839u
				}
			}
		},
		{
			120u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Platinum Divinity",
					SkinID = 907193301u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Platinum Divinity",
					SkinID = 907193302u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Platinum Divinity",
					SkinID = 907193303u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Platinum Divinity",
					SkinID = 907193304u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Platinum Divinity",
					SkinID = 907193305u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Platinum Divinity",
					SkinID = 907193306u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Platinum Divinity",
					SkinID = 907193307u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Spinning Bird",
					SkinID = 907002834u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Spinning Bird",
					SkinID = 907102834u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Frozen Lava",
					SkinID = 907102911u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Meta Lava",
					SkinID = 907102912u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Jungle Lava",
					SkinID = 907102913u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Rogue Lava",
					SkinID = 907102914u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Aurora Oni",
					SkinID = 907103113u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Cyan Ghost",
					SkinID = 907103451u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Crowned Glory",
					SkinID = 907105138u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Champion Boxer",
					SkinID = 907002408u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cyber Bounty Hunter",
					SkinID = 907002525u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "The Golden MP5",
					SkinID = 907002632u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Champion Boxer",
					SkinID = 907102408u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cyber Bounty Hunter",
					SkinID = 907102525u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "The Golden MP5",
					SkinID = 907102632u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Mr. Nutcracker",
					SkinID = 907102716u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Achiever",
					SkinID = 907103818u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Sublime Scorpio",
					SkinID = 907103908u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Troublemaker",
					SkinID = 907105019u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Champion Boxer",
					SkinID = 907202408u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cyber Bounty Hunter",
					SkinID = 907202525u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "The Golden MP5",
					SkinID = 907202632u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Imperial Rome",
					SkinID = 907001312u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Blood Red",
					SkinID = 907001317u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Red Samurai",
					SkinID = 907001409u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Star Soul",
					SkinID = 907001510u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death's Eye",
					SkinID = 907001516u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pink Devil",
					SkinID = 907001625u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Scorpion",
					SkinID = 907001629u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Vampire",
					SkinID = 907001706u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Old Fashioned",
					SkinID = 907001718u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Demolitionist",
					SkinID = 907001921u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Hipster Bunny",
					SkinID = 907002108u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Master of Minds",
					SkinID = 907002402u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Imperial Rome",
					SkinID = 907101312u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Blood Red",
					SkinID = 907101317u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Red Samurai",
					SkinID = 907101409u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Star Soul",
					SkinID = 907101510u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death's Eye",
					SkinID = 907101516u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Star Soul",
					SkinID = 907101540u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death's Eye",
					SkinID = 907101546u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pink Devil",
					SkinID = 907101625u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Scorpion",
					SkinID = 907101629u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Vampire",
					SkinID = 907101706u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Old Fashioned",
					SkinID = 907101718u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Demolitionist",
					SkinID = 907101921u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Hipster Bunny",
					SkinID = 907102108u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Master of Minds",
					SkinID = 907102402u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Evil Enchanted",
					SkinID = 907102703u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Beastly Wrath",
					SkinID = 907103015u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Vampire Malevolence",
					SkinID = 907103217u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S40 Exclusive: Goosy",
					SkinID = 907104519u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Shadow Ops",
					SkinID = 907104840u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S35 Exclusive: Fin",
					SkinID = 907105218u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Blood Red",
					SkinID = 907111317u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pigment Splash",
					SkinID = 907136012u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136044u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pink Devil",
					SkinID = 907201625u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Scorpion",
					SkinID = 907201629u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Vampire",
					SkinID = 907201706u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Old Fashioned",
					SkinID = 907201718u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Demolitionist",
					SkinID = 907201921u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Hipster Bunny",
					SkinID = 907202108u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Master of Minds",
					SkinID = 907202402u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Candy Bunny",
					SkinID = 907103808u,
					Color = 4281571839u
				}
			}
		},
		{
			121u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Platinum Divinity",
					SkinID = 907193301u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Platinum Divinity",
					SkinID = 907193302u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Platinum Divinity",
					SkinID = 907193303u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Platinum Divinity",
					SkinID = 907193304u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Platinum Divinity",
					SkinID = 907193305u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Platinum Divinity",
					SkinID = 907193306u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Platinum Divinity",
					SkinID = 907193307u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Spinning Bird",
					SkinID = 907002834u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Spinning Bird",
					SkinID = 907102834u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Frozen Lava",
					SkinID = 907102911u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Meta Lava",
					SkinID = 907102912u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Jungle Lava",
					SkinID = 907102913u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Rogue Lava",
					SkinID = 907102914u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Aurora Oni",
					SkinID = 907103113u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Cyan Ghost",
					SkinID = 907103451u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Crowned Glory",
					SkinID = 907105138u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Champion Boxer",
					SkinID = 907002408u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cyber Bounty Hunter",
					SkinID = 907002525u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "The Golden MP5",
					SkinID = 907002632u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Champion Boxer",
					SkinID = 907102408u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cyber Bounty Hunter",
					SkinID = 907102525u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "The Golden MP5",
					SkinID = 907102632u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Mr. Nutcracker",
					SkinID = 907102716u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Achiever",
					SkinID = 907103818u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Sublime Scorpio",
					SkinID = 907103908u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Troublemaker",
					SkinID = 907105019u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Champion Boxer",
					SkinID = 907202408u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cyber Bounty Hunter",
					SkinID = 907202525u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "The Golden MP5",
					SkinID = 907202632u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Imperial Rome",
					SkinID = 907001312u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Blood Red",
					SkinID = 907001317u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Red Samurai",
					SkinID = 907001409u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Star Soul",
					SkinID = 907001510u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death's Eye",
					SkinID = 907001516u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pink Devil",
					SkinID = 907001625u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Scorpion",
					SkinID = 907001629u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Vampire",
					SkinID = 907001706u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Old Fashioned",
					SkinID = 907001718u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Demolitionist",
					SkinID = 907001921u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Hipster Bunny",
					SkinID = 907002108u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Master of Minds",
					SkinID = 907002402u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Imperial Rome",
					SkinID = 907101312u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Blood Red",
					SkinID = 907101317u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Red Samurai",
					SkinID = 907101409u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Star Soul",
					SkinID = 907101510u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death's Eye",
					SkinID = 907101516u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Star Soul",
					SkinID = 907101540u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death's Eye",
					SkinID = 907101546u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pink Devil",
					SkinID = 907101625u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Scorpion",
					SkinID = 907101629u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Vampire",
					SkinID = 907101706u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Old Fashioned",
					SkinID = 907101718u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Demolitionist",
					SkinID = 907101921u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Hipster Bunny",
					SkinID = 907102108u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Master of Minds",
					SkinID = 907102402u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Evil Enchanted",
					SkinID = 907102703u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Beastly Wrath",
					SkinID = 907103015u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Vampire Malevolence",
					SkinID = 907103217u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S40 Exclusive: Goosy",
					SkinID = 907104519u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Shadow Ops",
					SkinID = 907104840u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S35 Exclusive: Fin",
					SkinID = 907105218u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Blood Red",
					SkinID = 907111317u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pigment Splash",
					SkinID = 907136012u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136044u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pink Devil",
					SkinID = 907201625u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Scorpion",
					SkinID = 907201629u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Vampire",
					SkinID = 907201706u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Old Fashioned",
					SkinID = 907201718u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Demolitionist",
					SkinID = 907201921u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Hipster Bunny",
					SkinID = 907202108u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Master of Minds",
					SkinID = 907202402u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Candy Bunny",
					SkinID = 907103808u,
					Color = 4281571839u
				}
			}
		},
		{
			122u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Dreambreaker",
					SkinID = 907195111u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Dreambreaker",
					SkinID = 907195112u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Dreambreaker",
					SkinID = 907195113u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Dreambreaker",
					SkinID = 907195114u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Dreambreaker",
					SkinID = 907195115u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Dreambreaker",
					SkinID = 907195116u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Dreambreaker",
					SkinID = 907195117u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Azure Stormbringer",
					SkinID = 907102806u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Frost Sabertooth",
					SkinID = 907102807u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Volcanic Whirlwind",
					SkinID = 907102808u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Shadow Earthshaker",
					SkinID = 907102809u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Deep Sea Tentacle",
					SkinID = 907104428u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Gold Coated",
					SkinID = 907001316u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Golden Rule",
					SkinID = 907001701u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Crimson Red",
					SkinID = 907001821u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Santa's Choice",
					SkinID = 907001910u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Captain Bubbles",
					SkinID = 907002214u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Spirited Overseers",
					SkinID = 907002306u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Viper Gangster",
					SkinID = 907002622u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Gold Coated",
					SkinID = 907101316u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Gold Coated",
					SkinID = 907101326u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Golden Rule",
					SkinID = 907101701u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Crimson Red",
					SkinID = 907101821u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Santa's Choice",
					SkinID = 907101910u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Captain Bubbles",
					SkinID = 907102214u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Spirited Overseers",
					SkinID = 907102306u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Viper Gangster",
					SkinID = 907102622u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Reel Shooter",
					SkinID = 907104128u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Wisp",
					SkinID = 907104409u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Akatsuki Theme",
					SkinID = 907105005u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Gold Coated",
					SkinID = 907111316u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Golden Rule",
					SkinID = 907201701u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Crimson Red",
					SkinID = 907201821u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Santa's Choice",
					SkinID = 907201910u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Captain Bubbles",
					SkinID = 907202214u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Spirited Overseers",
					SkinID = 907202306u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Viper Gangster",
					SkinID = 907202622u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Lively Beast",
					SkinID = 907001603u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pink Devil",
					SkinID = 907001626u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pirate's Soul",
					SkinID = 907001828u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Agent Paws",
					SkinID = 907002007u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Victory Wings",
					SkinID = 907002201u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Justice Fighter",
					SkinID = 907002318u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Swordsman Legends",
					SkinID = 907002406u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Lively Beast",
					SkinID = 907101603u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pink Devil",
					SkinID = 907101626u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pirate's Soul",
					SkinID = 907101828u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Agent Paws",
					SkinID = 907102007u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Victory Wings",
					SkinID = 907102201u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Justice Fighter",
					SkinID = 907102318u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Swordsman Legends",
					SkinID = 907102406u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Ice Blossoms",
					SkinID = 907103126u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Porcelain Rush",
					SkinID = 907103204u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Sublime Scorpio",
					SkinID = 907103909u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136055u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Lively Beast",
					SkinID = 907201603u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pink Devil",
					SkinID = 907201626u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pirate's Soul",
					SkinID = 907201828u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Agent Paws",
					SkinID = 907202007u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Victory Wings",
					SkinID = 907202201u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Justice Fighter",
					SkinID = 907202318u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Swordsman Legends",
					SkinID = 907202406u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Zombie Attack",
					SkinID = 907104103u,
					Color = 4278255360u
				}
			}
		},
		{
			123u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Dreambreaker",
					SkinID = 907195111u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Dreambreaker",
					SkinID = 907195112u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Dreambreaker",
					SkinID = 907195113u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Dreambreaker",
					SkinID = 907195114u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Dreambreaker",
					SkinID = 907195115u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Dreambreaker",
					SkinID = 907195116u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Dreambreaker",
					SkinID = 907195117u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Azure Stormbringer",
					SkinID = 907102806u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Frost Sabertooth",
					SkinID = 907102807u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Volcanic Whirlwind",
					SkinID = 907102808u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Shadow Earthshaker",
					SkinID = 907102809u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Deep Sea Tentacle",
					SkinID = 907104428u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Gold Coated",
					SkinID = 907001316u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Golden Rule",
					SkinID = 907001701u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Crimson Red",
					SkinID = 907001821u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Santa's Choice",
					SkinID = 907001910u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Captain Bubbles",
					SkinID = 907002214u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Spirited Overseers",
					SkinID = 907002306u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Viper Gangster",
					SkinID = 907002622u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Gold Coated",
					SkinID = 907101316u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Gold Coated",
					SkinID = 907101326u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Golden Rule",
					SkinID = 907101701u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Crimson Red",
					SkinID = 907101821u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Santa's Choice",
					SkinID = 907101910u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Captain Bubbles",
					SkinID = 907102214u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Spirited Overseers",
					SkinID = 907102306u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Viper Gangster",
					SkinID = 907102622u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Reel Shooter",
					SkinID = 907104128u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Wisp",
					SkinID = 907104409u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Akatsuki Theme",
					SkinID = 907105005u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Gold Coated",
					SkinID = 907111316u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Golden Rule",
					SkinID = 907201701u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Crimson Red",
					SkinID = 907201821u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Santa's Choice",
					SkinID = 907201910u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Captain Bubbles",
					SkinID = 907202214u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Spirited Overseers",
					SkinID = 907202306u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Viper Gangster",
					SkinID = 907202622u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Lively Beast",
					SkinID = 907001603u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pink Devil",
					SkinID = 907001626u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pirate's Soul",
					SkinID = 907001828u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Agent Paws",
					SkinID = 907002007u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Victory Wings",
					SkinID = 907002201u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Justice Fighter",
					SkinID = 907002318u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Swordsman Legends",
					SkinID = 907002406u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Lively Beast",
					SkinID = 907101603u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pink Devil",
					SkinID = 907101626u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pirate's Soul",
					SkinID = 907101828u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Agent Paws",
					SkinID = 907102007u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Victory Wings",
					SkinID = 907102201u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Justice Fighter",
					SkinID = 907102318u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Swordsman Legends",
					SkinID = 907102406u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Ice Blossoms",
					SkinID = 907103126u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Porcelain Rush",
					SkinID = 907103204u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Sublime Scorpio",
					SkinID = 907103909u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136055u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Lively Beast",
					SkinID = 907201603u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pink Devil",
					SkinID = 907201626u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pirate's Soul",
					SkinID = 907201828u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Agent Paws",
					SkinID = 907202007u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Victory Wings",
					SkinID = 907202201u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Justice Fighter",
					SkinID = 907202318u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Swordsman Legends",
					SkinID = 907202406u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Zombie Attack",
					SkinID = 907104103u,
					Color = 4278255360u
				}
			}
		},
		{
			130u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Demonic Grin",
					SkinID = 907192901u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Demonic Grin",
					SkinID = 907192902u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Demonic Grin",
					SkinID = 907192903u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Demonic Grin",
					SkinID = 907192904u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Demonic Grin",
					SkinID = 907192905u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Demonic Grin",
					SkinID = 907192906u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Demonic Grin",
					SkinID = 907192907u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Black Widow",
					SkinID = 907001615u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Ocean",
					SkinID = 907001616u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Golden",
					SkinID = 907001617u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Void",
					SkinID = 907001618u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Lightning",
					SkinID = 907001619u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow",
					SkinID = 907101615u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Ocean",
					SkinID = 907101616u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Golden",
					SkinID = 907101617u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Void",
					SkinID = 907101618u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Lightning",
					SkinID = 907101619u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Lava",
					SkinID = 907104311u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow",
					SkinID = 907201615u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Ocean",
					SkinID = 907201616u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Golden",
					SkinID = 907201617u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Void",
					SkinID = 907201618u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Lightning",
					SkinID = 907201619u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Metallic",
					SkinID = 907001440u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Vampire",
					SkinID = 907001705u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Warrior's Spirit",
					SkinID = 907001815u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Swagger Ownage",
					SkinID = 907001917u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "The Golden FAMAS",
					SkinID = 907002437u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Metallic",
					SkinID = 907101440u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Metallic",
					SkinID = 907101445u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Vampire",
					SkinID = 907101705u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Warrior's Spirit",
					SkinID = 907101815u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Swagger Ownage",
					SkinID = 907101917u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "The Golden FAMAS",
					SkinID = 907102437u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Wisdom's Vision",
					SkinID = 907104005u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "BLUE LOCK",
					SkinID = 907104652u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "HEARTROCKER",
					SkinID = 907105136u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Sculpted Excellence",
					SkinID = 907105313u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Vampire",
					SkinID = 907201705u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Warrior's Spirit",
					SkinID = 907201815u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Swagger Ownage",
					SkinID = 907201917u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "The Golden FAMAS",
					SkinID = 907202437u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Digital Invasion",
					SkinID = 907001308u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Imperial Rome",
					SkinID = 907001314u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Dragon's Eye",
					SkinID = 907001401u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Shark Attack",
					SkinID = 907001413u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Ice Blue",
					SkinID = 907001423u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pirate's Soul",
					SkinID = 907001827u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Loose Cannon",
					SkinID = 907001923u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Kpop Stardom",
					SkinID = 907002118u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Private Eye",
					SkinID = 907002224u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Wilderness Hunter",
					SkinID = 907002427u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Digital Invasion",
					SkinID = 907101308u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Imperial Rome",
					SkinID = 907101314u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Dragon's Eye",
					SkinID = 907101401u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Shark Attack",
					SkinID = 907101413u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Ice Blue",
					SkinID = 907101423u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pirate's Soul",
					SkinID = 907101827u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Loose Cannon",
					SkinID = 907101923u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Kpop Stardom",
					SkinID = 907102118u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Private Eye",
					SkinID = 907102224u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Wilderness Hunter",
					SkinID = 907102427u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Whirlwind Crease",
					SkinID = 907103105u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S26 Exclusive: Laura",
					SkinID = 907103128u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S30 Exclusive: Firefly",
					SkinID = 907104832u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Dragon Skin",
					SkinID = 907136019u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136053u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pirate's Soul",
					SkinID = 907201827u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Loose Cannon",
					SkinID = 907201923u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Kpop Stardom",
					SkinID = 907202118u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Private Eye",
					SkinID = 907202224u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Wilderness Hunter",
					SkinID = 907202427u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Waggor's Wonder",
					SkinID = 907104060u,
					Color = 4278255360u
				},
				new SkinInfo
				{
					Name = "Midnight Cityscape",
					SkinID = 907104808u,
					Color = 4281571839u
				}
			}
		},
		{
			131u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Demonic Grin",
					SkinID = 907192901u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Demonic Grin",
					SkinID = 907192902u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Demonic Grin",
					SkinID = 907192903u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Demonic Grin",
					SkinID = 907192904u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Demonic Grin",
					SkinID = 907192905u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Demonic Grin",
					SkinID = 907192906u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Demonic Grin",
					SkinID = 907192907u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Black Widow",
					SkinID = 907001615u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Ocean",
					SkinID = 907001616u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Golden",
					SkinID = 907001617u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Void",
					SkinID = 907001618u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Lightning",
					SkinID = 907001619u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow",
					SkinID = 907101615u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Ocean",
					SkinID = 907101616u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Golden",
					SkinID = 907101617u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Void",
					SkinID = 907101618u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Lightning",
					SkinID = 907101619u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Lava",
					SkinID = 907104311u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow",
					SkinID = 907201615u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Ocean",
					SkinID = 907201616u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Golden",
					SkinID = 907201617u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Void",
					SkinID = 907201618u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Black Widow Lightning",
					SkinID = 907201619u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Metallic",
					SkinID = 907001440u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Vampire",
					SkinID = 907001705u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Warrior's Spirit",
					SkinID = 907001815u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Swagger Ownage",
					SkinID = 907001917u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "The Golden FAMAS",
					SkinID = 907002437u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Metallic",
					SkinID = 907101440u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Metallic",
					SkinID = 907101445u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Vampire",
					SkinID = 907101705u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Warrior's Spirit",
					SkinID = 907101815u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Swagger Ownage",
					SkinID = 907101917u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "The Golden FAMAS",
					SkinID = 907102437u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Wisdom's Vision",
					SkinID = 907104005u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "BLUE LOCK",
					SkinID = 907104652u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "HEARTROCKER",
					SkinID = 907105136u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Sculpted Excellence",
					SkinID = 907105313u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Vampire",
					SkinID = 907201705u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Warrior's Spirit",
					SkinID = 907201815u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Swagger Ownage",
					SkinID = 907201917u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "The Golden FAMAS",
					SkinID = 907202437u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Digital Invasion",
					SkinID = 907001308u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Imperial Rome",
					SkinID = 907001314u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Dragon's Eye",
					SkinID = 907001401u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Shark Attack",
					SkinID = 907001413u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Ice Blue",
					SkinID = 907001423u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pirate's Soul",
					SkinID = 907001827u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Loose Cannon",
					SkinID = 907001923u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Kpop Stardom",
					SkinID = 907002118u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Private Eye",
					SkinID = 907002224u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Wilderness Hunter",
					SkinID = 907002427u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Digital Invasion",
					SkinID = 907101308u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Imperial Rome",
					SkinID = 907101314u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Dragon's Eye",
					SkinID = 907101401u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Shark Attack",
					SkinID = 907101413u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Ice Blue",
					SkinID = 907101423u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pirate's Soul",
					SkinID = 907101827u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Loose Cannon",
					SkinID = 907101923u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Kpop Stardom",
					SkinID = 907102118u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Private Eye",
					SkinID = 907102224u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Wilderness Hunter",
					SkinID = 907102427u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Whirlwind Crease",
					SkinID = 907103105u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S26 Exclusive: Laura",
					SkinID = 907103128u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S30 Exclusive: Firefly",
					SkinID = 907104832u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Dragon Skin",
					SkinID = 907136019u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136053u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pirate's Soul",
					SkinID = 907201827u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Loose Cannon",
					SkinID = 907201923u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Kpop Stardom",
					SkinID = 907202118u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Private Eye",
					SkinID = 907202224u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Wilderness Hunter",
					SkinID = 907202427u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Waggor's Wonder",
					SkinID = 907104060u,
					Color = 4278255360u
				},
				new SkinInfo
				{
					Name = "Midnight Cityscape",
					SkinID = 907104808u,
					Color = 4281571839u
				}
			}
		},
		{
			150u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Inner Nightmare",
					SkinID = 907104312u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Tropical Predator",
					SkinID = 907104811u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Fundamentality",
					SkinID = 907104133u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Super Void",
					SkinID = 907104508u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Wraith Patrol",
					SkinID = 907136013u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Soul Stealth",
					SkinID = 907103505u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Blizzard Brawl",
					SkinID = 907103810u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S31 Exclusive: Fearless",
					SkinID = 907104929u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Prince Darkheart",
					SkinID = 907105023u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S37 Exclusive: Sculpted",
					SkinID = 907105315u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136078u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "MetalGreymon Power",
					SkinID = 907105157u,
					Color = 4281571839u
				}
			}
		},
		{
			178u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907092501u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907092502u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907092503u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907092504u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907092505u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907092506u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907092507u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907192501u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907192502u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907192503u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907192504u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907192505u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907192506u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907192507u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Goldglare Phantom",
					SkinID = 907193601u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Goldglare Phantom",
					SkinID = 907193602u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Goldglare Phantom",
					SkinID = 907193603u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Goldglare Phantom",
					SkinID = 907193604u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Goldglare Phantom",
					SkinID = 907193605u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Goldglare Phantom",
					SkinID = 907193606u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Goldglare Phantom",
					SkinID = 907193607u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Goldglare Phantom",
					SkinID = 907193608u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907292501u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907292502u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907292503u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907292504u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907292505u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907292506u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907292507u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Ultimate Titan",
					SkinID = 907001517u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Inferno",
					SkinID = 907001518u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Paradise",
					SkinID = 907001519u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "The Beast",
					SkinID = 907001520u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Golden Strike",
					SkinID = 907001521u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Ultimate Titan",
					SkinID = 907101517u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Inferno",
					SkinID = 907101518u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Paradise",
					SkinID = 907101519u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "The Beast",
					SkinID = 907101520u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Golden Strike",
					SkinID = 907101521u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Ultimate Titan",
					SkinID = 907101547u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Inferno",
					SkinID = 907101548u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Paradise",
					SkinID = 907101549u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "The Beast",
					SkinID = 907101550u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Golden Strike",
					SkinID = 907101551u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Glistening Daystar",
					SkinID = 907104307u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Racer",
					SkinID = 907000034u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cupid",
					SkinID = 907001304u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Blood Moon",
					SkinID = 907001404u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Water Elemental",
					SkinID = 907001613u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Phantom Assassin",
					SkinID = 907001703u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Aurous Dragon",
					SkinID = 907001908u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Mystic Seeker",
					SkinID = 907002212u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Racer",
					SkinID = 907100034u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Winter Warrior",
					SkinID = 907100080u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cupid",
					SkinID = 907101304u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cupid",
					SkinID = 907101322u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Blood Moon",
					SkinID = 907101404u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Blood Moon",
					SkinID = 907101433u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Water Elemental",
					SkinID = 907101613u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Phantom Assassin",
					SkinID = 907101703u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Aurous Dragon",
					SkinID = 907101908u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Mystic Seeker",
					SkinID = 907102212u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Haven Warrior",
					SkinID = 907103301u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Nightfire Kami",
					SkinID = 907103814u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Zombie Attack",
					SkinID = 907104101u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Stitched Tailor",
					SkinID = 907104705u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Scarlet Nimbus",
					SkinID = 907104815u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Scarlet Ash",
					SkinID = 907105025u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Scarlet Ash",
					SkinID = 907105035u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Pearlescent",
					SkinID = 907105108u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Shadow of Scars",
					SkinID = 907105209u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Scarlet Romance",
					SkinID = 907105213u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Water Elemental",
					SkinID = 907201613u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Phantom Assassin",
					SkinID = 907201703u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Aurous Dragon",
					SkinID = 907201908u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Mystic Seeker",
					SkinID = 907202212u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Winter Warrior",
					SkinID = 907000080u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Violet Terror",
					SkinID = 907000081u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Special Forces",
					SkinID = 907001300u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Shark Attack",
					SkinID = 907001411u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Deadly Panther",
					SkinID = 907001443u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mad Samurai",
					SkinID = 907001524u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pink Devil",
					SkinID = 907001624u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Old Fashioned",
					SkinID = 907001716u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Party Animal",
					SkinID = 907002421u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Violet Terror",
					SkinID = 907100081u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Special Forces",
					SkinID = 907101300u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Shark Attack",
					SkinID = 907101411u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Deadly Panther",
					SkinID = 907101443u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mad Samurai",
					SkinID = 907101524u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mad Samurai",
					SkinID = 907101554u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pink Devil",
					SkinID = 907101624u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Old Fashioned",
					SkinID = 907101716u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Party Animal",
					SkinID = 907102421u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S22 Exclusive: Maxim",
					SkinID = 907102734u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Scarecrow",
					SkinID = 907102804u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Scarlet Groom",
					SkinID = 907102823u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Royal Warrior",
					SkinID = 907103007u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "The Golden SCAR",
					SkinID = 907103327u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Gusto Freerunner",
					SkinID = 907103553u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Scarlet Nimbus",
					SkinID = 907104816u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136051u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pink Devil",
					SkinID = 907201624u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Old Fashioned",
					SkinID = 907201716u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Party Animal",
					SkinID = 907202421u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Waggor's Wonder",
					SkinID = 907104053u,
					Color = 4278255360u
				},
				new SkinInfo
				{
					Name = "Monster Attack",
					SkinID = 907104131u,
					Color = 4281571839u
				}
			}
		},
		{
			179u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907092501u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907092502u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907092503u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907092504u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907092505u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907092506u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907092507u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907192501u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907192502u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907192503u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907192504u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907192505u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907192506u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907192507u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Goldglare Phantom",
					SkinID = 907193601u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Goldglare Phantom",
					SkinID = 907193602u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Goldglare Phantom",
					SkinID = 907193603u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Goldglare Phantom",
					SkinID = 907193604u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Goldglare Phantom",
					SkinID = 907193605u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Goldglare Phantom",
					SkinID = 907193606u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Goldglare Phantom",
					SkinID = 907193607u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Goldglare Phantom",
					SkinID = 907193608u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907292501u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907292502u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907292503u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907292504u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907292505u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907292506u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907292507u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Ultimate Titan",
					SkinID = 907001517u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Inferno",
					SkinID = 907001518u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Paradise",
					SkinID = 907001519u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "The Beast",
					SkinID = 907001520u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Golden Strike",
					SkinID = 907001521u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Ultimate Titan",
					SkinID = 907101517u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Inferno",
					SkinID = 907101518u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Paradise",
					SkinID = 907101519u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "The Beast",
					SkinID = 907101520u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Golden Strike",
					SkinID = 907101521u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Ultimate Titan",
					SkinID = 907101547u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Inferno",
					SkinID = 907101548u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Paradise",
					SkinID = 907101549u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "The Beast",
					SkinID = 907101550u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Golden Strike",
					SkinID = 907101551u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Glistening Daystar",
					SkinID = 907104307u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Racer",
					SkinID = 907000034u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cupid",
					SkinID = 907001304u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Blood Moon",
					SkinID = 907001404u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Water Elemental",
					SkinID = 907001613u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Phantom Assassin",
					SkinID = 907001703u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Aurous Dragon",
					SkinID = 907001908u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Mystic Seeker",
					SkinID = 907002212u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Racer",
					SkinID = 907100034u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Winter Warrior",
					SkinID = 907100080u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cupid",
					SkinID = 907101304u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cupid",
					SkinID = 907101322u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Blood Moon",
					SkinID = 907101404u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Blood Moon",
					SkinID = 907101433u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Water Elemental",
					SkinID = 907101613u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Phantom Assassin",
					SkinID = 907101703u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Aurous Dragon",
					SkinID = 907101908u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Mystic Seeker",
					SkinID = 907102212u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Haven Warrior",
					SkinID = 907103301u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Nightfire Kami",
					SkinID = 907103814u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Zombie Attack",
					SkinID = 907104101u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Stitched Tailor",
					SkinID = 907104705u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Scarlet Nimbus",
					SkinID = 907104815u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Scarlet Ash",
					SkinID = 907105025u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Scarlet Ash",
					SkinID = 907105035u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Pearlescent",
					SkinID = 907105108u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Shadow of Scars",
					SkinID = 907105209u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Scarlet Romance",
					SkinID = 907105213u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Water Elemental",
					SkinID = 907201613u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Phantom Assassin",
					SkinID = 907201703u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Aurous Dragon",
					SkinID = 907201908u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Mystic Seeker",
					SkinID = 907202212u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Winter Warrior",
					SkinID = 907000080u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Violet Terror",
					SkinID = 907000081u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Special Forces",
					SkinID = 907001300u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Shark Attack",
					SkinID = 907001411u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Deadly Panther",
					SkinID = 907001443u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mad Samurai",
					SkinID = 907001524u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pink Devil",
					SkinID = 907001624u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Old Fashioned",
					SkinID = 907001716u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Party Animal",
					SkinID = 907002421u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Violet Terror",
					SkinID = 907100081u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Special Forces",
					SkinID = 907101300u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Shark Attack",
					SkinID = 907101411u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Deadly Panther",
					SkinID = 907101443u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mad Samurai",
					SkinID = 907101524u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mad Samurai",
					SkinID = 907101554u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pink Devil",
					SkinID = 907101624u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Old Fashioned",
					SkinID = 907101716u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Party Animal",
					SkinID = 907102421u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S22 Exclusive: Maxim",
					SkinID = 907102734u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Scarecrow",
					SkinID = 907102804u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Scarlet Groom",
					SkinID = 907102823u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Royal Warrior",
					SkinID = 907103007u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "The Golden SCAR",
					SkinID = 907103327u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Gusto Freerunner",
					SkinID = 907103553u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Scarlet Nimbus",
					SkinID = 907104816u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136051u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pink Devil",
					SkinID = 907201624u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Old Fashioned",
					SkinID = 907201716u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Party Animal",
					SkinID = 907202421u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Waggor's Wonder",
					SkinID = 907104053u,
					Color = 4278255360u
				},
				new SkinInfo
				{
					Name = "Monster Attack",
					SkinID = 907104131u,
					Color = 4281571839u
				}
			}
		},
		{
			180u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907092501u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907092502u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907092503u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907092504u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907092505u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907092506u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907092507u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907192501u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907192502u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907192503u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907192504u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907192505u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907192506u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907192507u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Goldglare Phantom",
					SkinID = 907193601u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Goldglare Phantom",
					SkinID = 907193602u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Goldglare Phantom",
					SkinID = 907193603u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Goldglare Phantom",
					SkinID = 907193604u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Goldglare Phantom",
					SkinID = 907193605u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Goldglare Phantom",
					SkinID = 907193606u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Goldglare Phantom",
					SkinID = 907193607u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Goldglare Phantom",
					SkinID = 907193608u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907292501u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907292502u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907292503u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907292504u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907292505u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907292506u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Megalodon Alpha",
					SkinID = 907292507u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Ultimate Titan",
					SkinID = 907001517u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Inferno",
					SkinID = 907001518u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Paradise",
					SkinID = 907001519u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "The Beast",
					SkinID = 907001520u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Golden Strike",
					SkinID = 907001521u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Ultimate Titan",
					SkinID = 907101517u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Inferno",
					SkinID = 907101518u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Paradise",
					SkinID = 907101519u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "The Beast",
					SkinID = 907101520u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Golden Strike",
					SkinID = 907101521u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Ultimate Titan",
					SkinID = 907101547u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Inferno",
					SkinID = 907101548u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Paradise",
					SkinID = 907101549u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "The Beast",
					SkinID = 907101550u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Golden Strike",
					SkinID = 907101551u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Glistening Daystar",
					SkinID = 907104307u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Racer",
					SkinID = 907000034u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cupid",
					SkinID = 907001304u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Blood Moon",
					SkinID = 907001404u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Water Elemental",
					SkinID = 907001613u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Phantom Assassin",
					SkinID = 907001703u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Aurous Dragon",
					SkinID = 907001908u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Mystic Seeker",
					SkinID = 907002212u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Racer",
					SkinID = 907100034u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Winter Warrior",
					SkinID = 907100080u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cupid",
					SkinID = 907101304u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cupid",
					SkinID = 907101322u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Blood Moon",
					SkinID = 907101404u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Blood Moon",
					SkinID = 907101433u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Water Elemental",
					SkinID = 907101613u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Phantom Assassin",
					SkinID = 907101703u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Aurous Dragon",
					SkinID = 907101908u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Mystic Seeker",
					SkinID = 907102212u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Haven Warrior",
					SkinID = 907103301u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Nightfire Kami",
					SkinID = 907103814u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Zombie Attack",
					SkinID = 907104101u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Stitched Tailor",
					SkinID = 907104705u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Scarlet Nimbus",
					SkinID = 907104815u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Scarlet Ash",
					SkinID = 907105025u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Scarlet Ash",
					SkinID = 907105035u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Pearlescent",
					SkinID = 907105108u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Shadow of Scars",
					SkinID = 907105209u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Scarlet Romance",
					SkinID = 907105213u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Water Elemental",
					SkinID = 907201613u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Phantom Assassin",
					SkinID = 907201703u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Aurous Dragon",
					SkinID = 907201908u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Mystic Seeker",
					SkinID = 907202212u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Winter Warrior",
					SkinID = 907000080u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Violet Terror",
					SkinID = 907000081u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Special Forces",
					SkinID = 907001300u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Shark Attack",
					SkinID = 907001411u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Deadly Panther",
					SkinID = 907001443u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mad Samurai",
					SkinID = 907001524u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pink Devil",
					SkinID = 907001624u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Old Fashioned",
					SkinID = 907001716u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Party Animal",
					SkinID = 907002421u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Violet Terror",
					SkinID = 907100081u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Special Forces",
					SkinID = 907101300u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Shark Attack",
					SkinID = 907101411u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Deadly Panther",
					SkinID = 907101443u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mad Samurai",
					SkinID = 907101524u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mad Samurai",
					SkinID = 907101554u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pink Devil",
					SkinID = 907101624u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Old Fashioned",
					SkinID = 907101716u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Party Animal",
					SkinID = 907102421u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S22 Exclusive: Maxim",
					SkinID = 907102734u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Scarecrow",
					SkinID = 907102804u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Scarlet Groom",
					SkinID = 907102823u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Royal Warrior",
					SkinID = 907103007u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "The Golden SCAR",
					SkinID = 907103327u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Gusto Freerunner",
					SkinID = 907103553u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Scarlet Nimbus",
					SkinID = 907104816u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136051u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Pink Devil",
					SkinID = 907201624u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Old Fashioned",
					SkinID = 907201716u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Party Animal",
					SkinID = 907202421u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Waggor's Wonder",
					SkinID = 907104053u,
					Color = 4278255360u
				},
				new SkinInfo
				{
					Name = "Monster Attack",
					SkinID = 907104131u,
					Color = 4281571839u
				}
			}
		},
		{
			184u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Green Flame Draco",
					SkinID = 907192701u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Green Flame Draco",
					SkinID = 907192702u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Green Flame Draco",
					SkinID = 907192703u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Green Flame Draco",
					SkinID = 907192704u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Green Flame Draco",
					SkinID = 907192705u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Green Flame Draco",
					SkinID = 907192706u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Green Flame Draco",
					SkinID = 907192707u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Scorpio Shatter",
					SkinID = 907194101u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Scorpio Shatter",
					SkinID = 907194102u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Scorpio Shatter",
					SkinID = 907194103u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Scorpio Shatter",
					SkinID = 907194104u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Scorpio Shatter",
					SkinID = 907194105u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Scorpio Shatter",
					SkinID = 907194106u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Scorpio Shatter",
					SkinID = 907194107u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Scorpio Shatter",
					SkinID = 907194108u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Attack on Titan",
					SkinID = 907002105u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Red",
					SkinID = 907002228u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Dark",
					SkinID = 907002229u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Green",
					SkinID = 907002230u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Gold",
					SkinID = 907002231u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Attack on Titan",
					SkinID = 907102105u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Red",
					SkinID = 907102228u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Dark",
					SkinID = 907102229u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Green",
					SkinID = 907102230u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Gold",
					SkinID = 907102231u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Attack on Titan",
					SkinID = 907202105u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Red",
					SkinID = 907202228u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Dark",
					SkinID = 907202229u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Green",
					SkinID = 907202230u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Gold",
					SkinID = 907202231u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Winterlands",
					SkinID = 907000073u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cataclysm",
					SkinID = 907001504u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Enhanced Armor",
					SkinID = 907001610u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Wasteland",
					SkinID = 907002011u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Winterlands",
					SkinID = 907100073u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cataclysm",
					SkinID = 907101504u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cataclysm",
					SkinID = 907101534u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Enhanced Armor",
					SkinID = 907101610u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Wasteland",
					SkinID = 907102011u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Rave Skater",
					SkinID = 907103318u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Darkrose Lurker",
					SkinID = 907103518u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Defier's Anthem",
					SkinID = 907103830u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Big Bad Woof",
					SkinID = 907105115u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Enhanced Armor",
					SkinID = 907201610u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Wasteland",
					SkinID = 907202011u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Violet Terror",
					SkinID = 907000084u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Shark Attack",
					SkinID = 907001412u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Ice Blue",
					SkinID = 907001422u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death's Eye",
					SkinID = 907001514u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Artificial Intelligence",
					SkinID = 907001724u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Demolitionist",
					SkinID = 907001922u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Game Streamer",
					SkinID = 907002222u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Violet Terror",
					SkinID = 907100084u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Shark Attack",
					SkinID = 907101412u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Ice Blue",
					SkinID = 907101422u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death's Eye",
					SkinID = 907101514u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death's Eye",
					SkinID = 907101544u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Artificial Intelligence",
					SkinID = 907101724u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Demolitionist",
					SkinID = 907101922u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Game Streamer",
					SkinID = 907102222u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "The Golden M1014",
					SkinID = 907102826u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Roaring Gunfighter",
					SkinID = 907103232u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Demolitionist (30 Days)",
					SkinID = 907104498u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S42 Exclusive: Combustion",
					SkinID = 907104625u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136039u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Artificial Intelligence",
					SkinID = 907201724u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Demolitionist",
					SkinID = 907201922u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Game Streamer",
					SkinID = 907202222u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Waggor's Wonder",
					SkinID = 907104062u,
					Color = 4278255360u
				},
				new SkinInfo
				{
					Name = "Zombie Attack",
					SkinID = 907104243u,
					Color = 4278255360u
				},
				new SkinInfo
				{
					Name = "Undersea Rampage",
					SkinID = 907105302u,
					Color = 4281571839u
				},
				new SkinInfo
				{
					Name = "Dog At Hand",
					SkinID = 907136038u,
					Color = 4281571839u
				}
			}
		},
		{
			185u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Green Flame Draco",
					SkinID = 907192701u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Green Flame Draco",
					SkinID = 907192702u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Green Flame Draco",
					SkinID = 907192703u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Green Flame Draco",
					SkinID = 907192704u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Green Flame Draco",
					SkinID = 907192705u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Green Flame Draco",
					SkinID = 907192706u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Green Flame Draco",
					SkinID = 907192707u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Scorpio Shatter",
					SkinID = 907194101u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Scorpio Shatter",
					SkinID = 907194102u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Scorpio Shatter",
					SkinID = 907194103u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Scorpio Shatter",
					SkinID = 907194104u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Scorpio Shatter",
					SkinID = 907194105u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Scorpio Shatter",
					SkinID = 907194106u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Scorpio Shatter",
					SkinID = 907194107u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Scorpio Shatter",
					SkinID = 907194108u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Attack on Titan",
					SkinID = 907002105u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Red",
					SkinID = 907002228u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Dark",
					SkinID = 907002229u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Green",
					SkinID = 907002230u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Gold",
					SkinID = 907002231u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Attack on Titan",
					SkinID = 907102105u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Red",
					SkinID = 907102228u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Dark",
					SkinID = 907102229u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Green",
					SkinID = 907102230u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Gold",
					SkinID = 907102231u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Attack on Titan",
					SkinID = 907202105u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Red",
					SkinID = 907202228u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Dark",
					SkinID = 907202229u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Green",
					SkinID = 907202230u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Gold",
					SkinID = 907202231u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Winterlands",
					SkinID = 907000073u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cataclysm",
					SkinID = 907001504u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Enhanced Armor",
					SkinID = 907001610u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Wasteland",
					SkinID = 907002011u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Winterlands",
					SkinID = 907100073u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cataclysm",
					SkinID = 907101504u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cataclysm",
					SkinID = 907101534u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Enhanced Armor",
					SkinID = 907101610u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Wasteland",
					SkinID = 907102011u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Rave Skater",
					SkinID = 907103318u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Darkrose Lurker",
					SkinID = 907103518u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Defier's Anthem",
					SkinID = 907103830u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Big Bad Woof",
					SkinID = 907105115u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Enhanced Armor",
					SkinID = 907201610u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Wasteland",
					SkinID = 907202011u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Violet Terror",
					SkinID = 907000084u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Shark Attack",
					SkinID = 907001412u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Ice Blue",
					SkinID = 907001422u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death's Eye",
					SkinID = 907001514u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Artificial Intelligence",
					SkinID = 907001724u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Demolitionist",
					SkinID = 907001922u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Game Streamer",
					SkinID = 907002222u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Violet Terror",
					SkinID = 907100084u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Shark Attack",
					SkinID = 907101412u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Ice Blue",
					SkinID = 907101422u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death's Eye",
					SkinID = 907101514u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death's Eye",
					SkinID = 907101544u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Artificial Intelligence",
					SkinID = 907101724u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Demolitionist",
					SkinID = 907101922u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Game Streamer",
					SkinID = 907102222u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "The Golden M1014",
					SkinID = 907102826u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Roaring Gunfighter",
					SkinID = 907103232u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Demolitionist (30 Days)",
					SkinID = 907104498u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S42 Exclusive: Combustion",
					SkinID = 907104625u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136039u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Artificial Intelligence",
					SkinID = 907201724u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Demolitionist",
					SkinID = 907201922u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Game Streamer",
					SkinID = 907202222u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Waggor's Wonder",
					SkinID = 907104062u,
					Color = 4278255360u
				},
				new SkinInfo
				{
					Name = "Zombie Attack",
					SkinID = 907104243u,
					Color = 4278255360u
				},
				new SkinInfo
				{
					Name = "Undersea Rampage",
					SkinID = 907105302u,
					Color = 4281571839u
				},
				new SkinInfo
				{
					Name = "Dog At Hand",
					SkinID = 907136038u,
					Color = 4281571839u
				}
			}
		},
		{
			186u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Green Flame Draco",
					SkinID = 907192701u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Green Flame Draco",
					SkinID = 907192702u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Green Flame Draco",
					SkinID = 907192703u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Green Flame Draco",
					SkinID = 907192704u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Green Flame Draco",
					SkinID = 907192705u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Green Flame Draco",
					SkinID = 907192706u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Green Flame Draco",
					SkinID = 907192707u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Scorpio Shatter",
					SkinID = 907194101u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Scorpio Shatter",
					SkinID = 907194102u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Scorpio Shatter",
					SkinID = 907194103u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Scorpio Shatter",
					SkinID = 907194104u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Scorpio Shatter",
					SkinID = 907194105u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Scorpio Shatter",
					SkinID = 907194106u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Scorpio Shatter",
					SkinID = 907194107u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Scorpio Shatter",
					SkinID = 907194108u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Attack on Titan",
					SkinID = 907002105u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Red",
					SkinID = 907002228u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Dark",
					SkinID = 907002229u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Green",
					SkinID = 907002230u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Gold",
					SkinID = 907002231u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Attack on Titan",
					SkinID = 907102105u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Red",
					SkinID = 907102228u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Dark",
					SkinID = 907102229u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Green",
					SkinID = 907102230u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Gold",
					SkinID = 907102231u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Attack on Titan",
					SkinID = 907202105u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Red",
					SkinID = 907202228u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Dark",
					SkinID = 907202229u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Green",
					SkinID = 907202230u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Apocalyptic Gold",
					SkinID = 907202231u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Winterlands",
					SkinID = 907000073u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cataclysm",
					SkinID = 907001504u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Enhanced Armor",
					SkinID = 907001610u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Wasteland",
					SkinID = 907002011u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Winterlands",
					SkinID = 907100073u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cataclysm",
					SkinID = 907101504u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cataclysm",
					SkinID = 907101534u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Enhanced Armor",
					SkinID = 907101610u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Wasteland",
					SkinID = 907102011u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Rave Skater",
					SkinID = 907103318u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Darkrose Lurker",
					SkinID = 907103518u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Defier's Anthem",
					SkinID = 907103830u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Big Bad Woof",
					SkinID = 907105115u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Enhanced Armor",
					SkinID = 907201610u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Wasteland",
					SkinID = 907202011u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Violet Terror",
					SkinID = 907000084u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Shark Attack",
					SkinID = 907001412u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Ice Blue",
					SkinID = 907001422u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death's Eye",
					SkinID = 907001514u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Artificial Intelligence",
					SkinID = 907001724u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Demolitionist",
					SkinID = 907001922u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Game Streamer",
					SkinID = 907002222u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Violet Terror",
					SkinID = 907100084u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Shark Attack",
					SkinID = 907101412u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Ice Blue",
					SkinID = 907101422u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death's Eye",
					SkinID = 907101514u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Death's Eye",
					SkinID = 907101544u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Artificial Intelligence",
					SkinID = 907101724u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Demolitionist",
					SkinID = 907101922u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Game Streamer",
					SkinID = 907102222u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "The Golden M1014",
					SkinID = 907102826u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Roaring Gunfighter",
					SkinID = 907103232u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Demolitionist (30 Days)",
					SkinID = 907104498u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S42 Exclusive: Combustion",
					SkinID = 907104625u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136039u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Artificial Intelligence",
					SkinID = 907201724u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Demolitionist",
					SkinID = 907201922u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Game Streamer",
					SkinID = 907202222u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Waggor's Wonder",
					SkinID = 907104062u,
					Color = 4278255360u
				},
				new SkinInfo
				{
					Name = "Zombie Attack",
					SkinID = 907104243u,
					Color = 4278255360u
				},
				new SkinInfo
				{
					Name = "Undersea Rampage",
					SkinID = 907105302u,
					Color = 4281571839u
				},
				new SkinInfo
				{
					Name = "Dog At Hand",
					SkinID = 907136038u,
					Color = 4281571839u
				}
			}
		},
		{
			193u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Cyber Bounty Hunter",
					SkinID = 907002524u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Cyber Bounty Hunter",
					SkinID = 907102524u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Mars Landcrusher",
					SkinID = 907103431u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Ventus Oceanbust",
					SkinID = 907103432u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Nebula Skydark",
					SkinID = 907103433u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Aqua Firetaming",
					SkinID = 907103434u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Cyber Bounty Hunter",
					SkinID = 907202524u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Booyah Day",
					SkinID = 907002416u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Booyah Day",
					SkinID = 907102416u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Roaring Gunfighter",
					SkinID = 907103231u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Jelly Shot",
					SkinID = 907104126u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Beast of Legend",
					SkinID = 907104315u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Jagged Laughs",
					SkinID = 907104601u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Jagged Laughs",
					SkinID = 907104604u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Astral Ripples",
					SkinID = 907105215u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Eclipse Rise",
					SkinID = 907105305u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Booyah Day",
					SkinID = 907202416u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Party Animal",
					SkinID = 907002418u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Winterlands 2020",
					SkinID = 907002513u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Party Animal",
					SkinID = 907102418u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Winterlands 2020",
					SkinID = 907102513u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mr. Nutcracker",
					SkinID = 907102717u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Quantic Unknown",
					SkinID = 907102909u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Cosmic Teleportia",
					SkinID = 907102918u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S25 Exclusive: Miguel",
					SkinID = 907103011u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Phoenix Knight",
					SkinID = 907103116u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Apex Underwater",
					SkinID = 907103504u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Cannibal Crush",
					SkinID = 907103707u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Aqua Blast",
					SkinID = 907104010u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S46 Exclusive: Signal",
					SkinID = 907104928u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136058u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Party Animal",
					SkinID = 907202418u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Winterlands 2020",
					SkinID = 907202513u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Aurora's Holler",
					SkinID = 907104709u,
					Color = 4281571839u
				}
			}
		},
		{
			194u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Cyber Bounty Hunter",
					SkinID = 907002524u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Cyber Bounty Hunter",
					SkinID = 907102524u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Mars Landcrusher",
					SkinID = 907103431u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Ventus Oceanbust",
					SkinID = 907103432u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Nebula Skydark",
					SkinID = 907103433u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Aqua Firetaming",
					SkinID = 907103434u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Cyber Bounty Hunter",
					SkinID = 907202524u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Booyah Day",
					SkinID = 907002416u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Booyah Day",
					SkinID = 907102416u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Roaring Gunfighter",
					SkinID = 907103231u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Jelly Shot",
					SkinID = 907104126u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Beast of Legend",
					SkinID = 907104315u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Jagged Laughs",
					SkinID = 907104601u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Jagged Laughs",
					SkinID = 907104604u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Astral Ripples",
					SkinID = 907105215u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Eclipse Rise",
					SkinID = 907105305u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Booyah Day",
					SkinID = 907202416u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Party Animal",
					SkinID = 907002418u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Winterlands 2020",
					SkinID = 907002513u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Party Animal",
					SkinID = 907102418u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Winterlands 2020",
					SkinID = 907102513u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mr. Nutcracker",
					SkinID = 907102717u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Quantic Unknown",
					SkinID = 907102909u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Cosmic Teleportia",
					SkinID = 907102918u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S25 Exclusive: Miguel",
					SkinID = 907103011u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Phoenix Knight",
					SkinID = 907103116u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Apex Underwater",
					SkinID = 907103504u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Cannibal Crush",
					SkinID = 907103707u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Aqua Blast",
					SkinID = 907104010u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S46 Exclusive: Signal",
					SkinID = 907104928u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136058u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Party Animal",
					SkinID = 907202418u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Winterlands 2020",
					SkinID = 907202513u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Aurora's Holler",
					SkinID = 907104709u,
					Color = 4281571839u
				}
			}
		},
		{
			195u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Cyber Bounty Hunter",
					SkinID = 907002524u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Cyber Bounty Hunter",
					SkinID = 907102524u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Mars Landcrusher",
					SkinID = 907103431u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Ventus Oceanbust",
					SkinID = 907103432u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Nebula Skydark",
					SkinID = 907103433u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Aqua Firetaming",
					SkinID = 907103434u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Cyber Bounty Hunter",
					SkinID = 907202524u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Booyah Day",
					SkinID = 907002416u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Booyah Day",
					SkinID = 907102416u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Roaring Gunfighter",
					SkinID = 907103231u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Jelly Shot",
					SkinID = 907104126u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Beast of Legend",
					SkinID = 907104315u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Jagged Laughs",
					SkinID = 907104601u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Jagged Laughs",
					SkinID = 907104604u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Astral Ripples",
					SkinID = 907105215u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Eclipse Rise",
					SkinID = 907105305u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Booyah Day",
					SkinID = 907202416u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Party Animal",
					SkinID = 907002418u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Winterlands 2020",
					SkinID = 907002513u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Party Animal",
					SkinID = 907102418u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Winterlands 2020",
					SkinID = 907102513u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mr. Nutcracker",
					SkinID = 907102717u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Quantic Unknown",
					SkinID = 907102909u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Cosmic Teleportia",
					SkinID = 907102918u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S25 Exclusive: Miguel",
					SkinID = 907103011u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Phoenix Knight",
					SkinID = 907103116u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Apex Underwater",
					SkinID = 907103504u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Cannibal Crush",
					SkinID = 907103707u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Aqua Blast",
					SkinID = 907104010u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S46 Exclusive: Signal",
					SkinID = 907104928u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136058u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Party Animal",
					SkinID = 907202418u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Winterlands 2020",
					SkinID = 907202513u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Aurora's Holler",
					SkinID = 907104709u,
					Color = 4281571839u
				}
			}
		},
		{
			197u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Projected Memories",
					SkinID = 907104702u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Byte Tamper",
					SkinID = 907104401u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "24K Kitty",
					SkinID = 907104713u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Undersea Mystery",
					SkinID = 907105306u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907104337u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S39 Exclusive: Byte",
					SkinID = 907104424u,
					Color = 4294901760u
				}
			}
		},
		{
			228u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Mind's Eye",
					SkinID = 907103859u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Iron Iridescence",
					SkinID = 907104105u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Silver Iridescence",
					SkinID = 907104106u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Golden Iridescence",
					SkinID = 907104107u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Ionic Theory",
					SkinID = 907104710u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Frozen Platinum",
					SkinID = 907103212u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Enflamed Terror",
					SkinID = 907103416u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Ribbit Rain",
					SkinID = 907104303u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Bubble Trouble",
					SkinID = 907105110u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cyan Fear",
					SkinID = 907103121u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "The Golden MAC10",
					SkinID = 907103812u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Metro Flash",
					SkinID = 907104818u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S44 Exclusive: Manga",
					SkinID = 907104831u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Cosmic Chaos",
					SkinID = 907104912u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Bones of Terror",
					SkinID = 907136016u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136074u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Wisdom's Fire",
					SkinID = 907104064u,
					Color = 4278255360u
				},
				new SkinInfo
				{
					Name = "Eclipse Nova",
					SkinID = 907105301u,
					Color = 4281571839u
				}
			}
		},
		{
			229u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Mind's Eye",
					SkinID = 907103859u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Iron Iridescence",
					SkinID = 907104105u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Silver Iridescence",
					SkinID = 907104106u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Golden Iridescence",
					SkinID = 907104107u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Ionic Theory",
					SkinID = 907104710u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Frozen Platinum",
					SkinID = 907103212u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Enflamed Terror",
					SkinID = 907103416u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Ribbit Rain",
					SkinID = 907104303u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Bubble Trouble",
					SkinID = 907105110u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cyan Fear",
					SkinID = 907103121u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "The Golden MAC10",
					SkinID = 907103812u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Metro Flash",
					SkinID = 907104818u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S44 Exclusive: Manga",
					SkinID = 907104831u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Cosmic Chaos",
					SkinID = 907104912u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Bones of Terror",
					SkinID = 907136016u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136074u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Wisdom's Fire",
					SkinID = 907104064u,
					Color = 4278255360u
				},
				new SkinInfo
				{
					Name = "Eclipse Nova",
					SkinID = 907105301u,
					Color = 4281571839u
				}
			}
		},
		{
			230u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Mind's Eye",
					SkinID = 907103859u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Iron Iridescence",
					SkinID = 907104105u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Silver Iridescence",
					SkinID = 907104106u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Golden Iridescence",
					SkinID = 907104107u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Ionic Theory",
					SkinID = 907104710u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Frozen Platinum",
					SkinID = 907103212u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Enflamed Terror",
					SkinID = 907103416u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Ribbit Rain",
					SkinID = 907104303u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Bubble Trouble",
					SkinID = 907105110u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Cyan Fear",
					SkinID = 907103121u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "The Golden MAC10",
					SkinID = 907103812u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Metro Flash",
					SkinID = 907104818u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S44 Exclusive: Manga",
					SkinID = 907104831u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Cosmic Chaos",
					SkinID = 907104912u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Bones of Terror",
					SkinID = 907136016u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136074u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Wisdom's Fire",
					SkinID = 907104064u,
					Color = 4278255360u
				},
				new SkinInfo
				{
					Name = "Eclipse Nova",
					SkinID = 907105301u,
					Color = 4281571839u
				}
			}
		},
		{
			21020u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Cindered Colossus",
					SkinID = 907193901u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Cindered Colossus",
					SkinID = 907193902u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Cindered Colossus",
					SkinID = 907193903u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Cindered Colossus",
					SkinID = 907193904u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Cindered Colossus",
					SkinID = 907193905u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Cindered Colossus",
					SkinID = 907193906u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Cindered Colossus",
					SkinID = 907193907u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Thrash Metallic",
					SkinID = 907102915u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Galactic Panthera",
					SkinID = 907136007u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Dawnbreaker Lynx",
					SkinID = 907136008u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Firehound Jaguar",
					SkinID = 907136009u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Hoarfrost Cheetah",
					SkinID = 907136010u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Specter Squad",
					SkinID = 907002504u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Time Travellers",
					SkinID = 907002510u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Bloodwing City",
					SkinID = 907002602u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Specter Squad",
					SkinID = 907102504u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Time Travellers",
					SkinID = 907102510u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Bloodwing City",
					SkinID = 907102602u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Dragon Mob",
					SkinID = 907102714u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Freezing Flame",
					SkinID = 907102829u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Goldrim Tribute",
					SkinID = 907103001u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Fierce Demilord",
					SkinID = 907103210u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Daybreaker",
					SkinID = 907104402u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Specter Squad",
					SkinID = 907202504u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Time Travellers",
					SkinID = 907202510u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Bloodwing City",
					SkinID = 907202602u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "MMA Fighter",
					SkinID = 907002422u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Lucky Koi",
					SkinID = 907002617u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "MMA Fighter",
					SkinID = 907102422u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Lucky Koi",
					SkinID = 907102617u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "The Falconer",
					SkinID = 907102723u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S35 Exclusive: Tatsuya",
					SkinID = 907104022u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S26 Exclusive: Leo",
					SkinID = 907104520u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Shadow Aim",
					SkinID = 907105210u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136047u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "MMA Fighter",
					SkinID = 907202422u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Lucky Koi",
					SkinID = 907202617u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Waggor's Wonder",
					SkinID = 907104056u,
					Color = 4278255360u
				},
				new SkinInfo
				{
					Name = "Waggor's Wonder (15 Days)",
					SkinID = 907104499u,
					Color = 4278255360u
				},
				new SkinInfo
				{
					Name = "Hybrid Explosion",
					SkinID = 907103408u,
					Color = 4281571839u
				},
				new SkinInfo
				{
					Name = "Cyber Claws",
					SkinID = 907104607u,
					Color = 4281571839u
				}
			}
		},
		{
			21022u,
			new List<SkinInfo>
			{
				new SkinInfo
				{
					Name = "Cindered Colossus",
					SkinID = 907193901u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Cindered Colossus",
					SkinID = 907193902u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Cindered Colossus",
					SkinID = 907193903u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Cindered Colossus",
					SkinID = 907193904u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Cindered Colossus",
					SkinID = 907193905u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Cindered Colossus",
					SkinID = 907193906u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Cindered Colossus",
					SkinID = 907193907u,
					Color = 4281479922u
				},
				new SkinInfo
				{
					Name = "Thrash Metallic",
					SkinID = 907102915u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Galactic Panthera",
					SkinID = 907136007u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Dawnbreaker Lynx",
					SkinID = 907136008u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Firehound Jaguar",
					SkinID = 907136009u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Hoarfrost Cheetah",
					SkinID = 907136010u,
					Color = 4278232575u
				},
				new SkinInfo
				{
					Name = "Specter Squad",
					SkinID = 907002504u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Time Travellers",
					SkinID = 907002510u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Bloodwing City",
					SkinID = 907002602u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Specter Squad",
					SkinID = 907102504u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Time Travellers",
					SkinID = 907102510u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Bloodwing City",
					SkinID = 907102602u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Dragon Mob",
					SkinID = 907102714u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Freezing Flame",
					SkinID = 907102829u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Goldrim Tribute",
					SkinID = 907103001u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Fierce Demilord",
					SkinID = 907103210u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Daybreaker",
					SkinID = 907104402u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Specter Squad",
					SkinID = 907202504u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Time Travellers",
					SkinID = 907202510u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "Bloodwing City",
					SkinID = 907202602u,
					Color = 4294902015u
				},
				new SkinInfo
				{
					Name = "MMA Fighter",
					SkinID = 907002422u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Lucky Koi",
					SkinID = 907002617u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "MMA Fighter",
					SkinID = 907102422u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Lucky Koi",
					SkinID = 907102617u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "The Falconer",
					SkinID = 907102723u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S35 Exclusive: Tatsuya",
					SkinID = 907104022u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "S26 Exclusive: Leo",
					SkinID = 907104520u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Shadow Aim",
					SkinID = 907105210u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Mamba Gnaw",
					SkinID = 907136047u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "MMA Fighter",
					SkinID = 907202422u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Lucky Koi",
					SkinID = 907202617u,
					Color = 4294901760u
				},
				new SkinInfo
				{
					Name = "Waggor's Wonder",
					SkinID = 907104056u,
					Color = 4278255360u
				},
				new SkinInfo
				{
					Name = "Waggor's Wonder (15 Days)",
					SkinID = 907104499u,
					Color = 4278255360u
				},
				new SkinInfo
				{
					Name = "Hybrid Explosion",
					SkinID = 907103408u,
					Color = 4281571839u
				},
				new SkinInfo
				{
					Name = "Cyber Claws",
					SkinID = 907104607u,
					Color = 4281571839u
				}
			}
		}
	};
}
