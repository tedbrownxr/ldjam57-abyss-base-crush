using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
	public class UpgradeData
	{
		public static List<Upgrade> GetUpgrades (UpgradeType upgradeType)
		{
			switch (upgradeType)
			{
				case UpgradeType.GunCount: return GunCountUpgrades;
				case UpgradeType.GunDamage: return GunDamageUpgrades;
				case UpgradeType.GunPenetration: return GunPenetrationUpgrades;
				case UpgradeType.GunRateOfFire: return GunRateOfFireUpgrades;
				case UpgradeType.GunTurnSpeed: return GunTurnSpeedUpgrades;
			}
			return new List<Upgrade>();
		}

		public static bool TryGetUpgrade (UpgradeType upgradeType, int level, out Upgrade upgrade)
		{
			int count = GetUpgrades(upgradeType).Count;
			if (count == 0 || count < level)
			{
				Debug.LogError($"no upgrade data for UpgradeType.{upgradeType} level {level}");
				upgrade = new Upgrade()
				{
					UpgradeType = upgradeType,
					Name = "Unknown",
					Description = "Error",
					IntValue = 1,
					FloatValue = 1
				};
				return false;
			}
			upgrade = GetUpgrades(upgradeType)[level];
			return true;
		}

		public static List<Upgrade> GunCountUpgrades = new List<Upgrade> ()
		{
			new Upgrade()
			{
				UpgradeType = UpgradeType.GunCount,
				Name = "Guns I",
				Description = "Starting Gun",
				IntValue = 1,
			},
			new Upgrade()
			{
				UpgradeType = UpgradeType.GunCount,
				Name = "Dual Guns",
				Description = "Dual Turret Guns",
				IntValue = 2,
			},
			new Upgrade()
			{
				UpgradeType = UpgradeType.GunCount,
				Name = "Triple Guns",
				Description = "Triple Turret Guns",
				IntValue = 3,
			},
			new Upgrade()
			{
				UpgradeType = UpgradeType.GunCount,
				Name = "Guns IV",
				Description = "Four Turret Guns",
				IntValue = 4,
			},
			new Upgrade()
			{
				UpgradeType = UpgradeType.GunCount,
				Name = "Guns V",
				Description = "FIVE TURRET GUNS!?",
				IntValue = 5,
			}
		};

		public static List<Upgrade> GunDamageUpgrades = new List<Upgrade> ()
		{
			new Upgrade()
			{
				UpgradeType = UpgradeType.GunDamage,
				Name = "Base Gun Damage",
				Description = "Starting Value",
				IntValue = 10,
			},
			new Upgrade()
			{
				UpgradeType = UpgradeType.GunDamage,
				Name = "Gun Damage 2x",
				Description = "Do more with the same",
				IntValue = 20,
			},
			new Upgrade()
			{
				UpgradeType = UpgradeType.GunDamage,
				Name = "3x Gun Damage",
				Description = "It's a banger!",
				IntValue = 30,
			},
			new Upgrade()
			{
				UpgradeType = UpgradeType.GunDamage,
				Name = "Quad Damage Guns",
				Description = "Wow.",
				IntValue = 40,
			}
		};

		public static List<Upgrade> GunPenetrationUpgrades = new List<Upgrade> ()
		{
			new Upgrade()
			{
				UpgradeType = UpgradeType.GunPenetration,
				Name = "Base Gun Piercing",
				Description = "Starting Value",
				IntValue = 0,
			},
			new Upgrade()
			{
				UpgradeType = UpgradeType.GunPenetration,
				Name = "Piercing I",
				Description = "Can hit one more enemy",
				IntValue = 1,
			},
			new Upgrade()
			{
				UpgradeType = UpgradeType.GunPenetration,
				Name = "Piercing II",
				Description = "Can hit two more enemies",
				IntValue = 2,
			},
			new Upgrade()
			{
				UpgradeType = UpgradeType.GunPenetration,
				Name = "Piercing III",
				Description = "Can hit three more enemies",
				IntValue = 3,
			}
		};

		public static List<Upgrade> GunRateOfFireUpgrades = new List<Upgrade> ()
		{
			new Upgrade()
			{
				UpgradeType = UpgradeType.GunRateOfFire,
				Name = "Base Rate of Fire",
				Description = "Starting Value",
				FloatValue = 1
			},
			new Upgrade()
			{
				UpgradeType = UpgradeType.GunRateOfFire,
				Name = "Rate of Fire I",
				Description = "Fast",
				FloatValue = 0.8f
			},
			new Upgrade()
			{
				UpgradeType = UpgradeType.GunRateOfFire,
				Name = "Rate of Fire II",
				Description = "Faster",
				FloatValue = 0.6f
			},
			new Upgrade()
			{
				UpgradeType = UpgradeType.GunRateOfFire,
				Name = "Rate of Fire III",
				Description = "Fastest",
				FloatValue = 0.4f
			}
		};

		public static List<Upgrade> GunTurnSpeedUpgrades = new List<Upgrade> ()
		{
			new Upgrade()
			{
				UpgradeType = UpgradeType.GunTurnSpeed,
				Name = "Base Turn Speed",
				Description = "Starting Value",
				FloatValue = 70
			},
			new Upgrade()
			{
				UpgradeType = UpgradeType.GunTurnSpeed,
				Name = "Turret Speed I",
				Description = "Fast",
				FloatValue = 90
			},
			new Upgrade()
			{
				UpgradeType = UpgradeType.GunTurnSpeed,
				Name = "Turret Speed II",
				Description = "Faster",
				FloatValue = 110
			},
			new Upgrade()
			{
				UpgradeType = UpgradeType.GunTurnSpeed,
				Name = "Turret Speed III",
				Description = "Fastest",
				FloatValue = 120
			}
		};	

	}
}
