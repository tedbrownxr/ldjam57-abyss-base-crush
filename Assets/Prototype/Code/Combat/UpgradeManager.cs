using DevKit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Prototype
{
	public class UpgradeManager : MonoBehaviour 
	{
		public Action<Upgrade> OnUpgradeSelected;

		Dictionary<UpgradeType, int> _upgradeLevels;
		Dictionary<UpgradeType, int> _upgradeMaxLevels;
		GunManager _gunManager;
		UpgradeMenu _upgradeMenu;

		public void HandleUpgrade (UpgradeType upgradeType)
		{
			int level = Math.Min(_upgradeLevels[upgradeType] + 1, _upgradeMaxLevels[upgradeType]);
			_upgradeLevels[upgradeType] = level;
			Upgrade upgrade = SetUpgrade(upgradeType, level);
			OnUpgradeSelected?.Invoke(upgrade);
		}

		public Upgrade SetUpgrade (UpgradeType upgradeType, int level)
		{
			if (UpgradeData.TryGetUpgrade(upgradeType, level, out Upgrade upgrade) == false)
			{
				Debug.LogError($"could not find upgrade level {level} for type {upgradeType}");
				return null;
			}

			switch (upgradeType)
			{
				case UpgradeType.GunCount:
				case UpgradeType.GunDamage:
				case UpgradeType.GunPenetration:
				case UpgradeType.GunRateOfFire:
				case UpgradeType.GunTurnSpeed:
					_gunManager.SetUpgrade(upgrade);
					break;
			}
			return upgrade;
		}

		public void ShowRandomUpgrades ()
		{
			List<UpgradeType> availableUpgrades = new List<UpgradeType>();
			for (int i = 0; i < System.Enum.GetValues(typeof(UpgradeType)).Length; i++)
			{
				var type = (UpgradeType) i;
				if (_upgradeLevels.ContainsKey(type) == false) continue;
				if (_upgradeLevels[type] + 1 < _upgradeMaxLevels[type])
				{
					availableUpgrades.Add(type);
				}
			}

			if (availableUpgrades.Count == 0)
			{
				Debug.Log("no more upgrades");
				return;
			}

			if (availableUpgrades.Count == 1)
			{
				UpgradeType type = availableUpgrades[0];
				int nextLevel = _upgradeLevels[type] + 1;
				_upgradeMenu.ShowUpgrade1(type, nextLevel);
				return;
			}

			if (availableUpgrades.Count == 2)
			{
				UpgradeType a = availableUpgrades[0];
				int levelA = _upgradeLevels[a] + 1;
				UpgradeType b = availableUpgrades[1];
				int levelB = _upgradeLevels[b] + 1;
				_upgradeMenu.ShowUpgrades2(a, levelA, b, levelB);
				return;
			}

			{
				int indexA = UnityEngine.Random.Range(0, availableUpgrades.Count);
				UpgradeType a = availableUpgrades[indexA];
				availableUpgrades.RemoveAt(indexA);

				int indexB = UnityEngine.Random.Range(0, availableUpgrades.Count);
				UpgradeType b = availableUpgrades[indexB];
				availableUpgrades.RemoveAt(indexB);

				int indexC = UnityEngine.Random.Range(0, availableUpgrades.Count);
				UpgradeType c = availableUpgrades[indexC];

				_upgradeMenu.ShowUpgrades3(a, _upgradeLevels[a]+1, b, _upgradeLevels[b]+1, c, _upgradeLevels[c]+1);
			}
		}

		protected void Awake ()
		{
			_gunManager = FindObjectOfType<GunManager>();
			_upgradeLevels = new Dictionary<UpgradeType, int>();
			_upgradeMaxLevels = new Dictionary<UpgradeType, int>();
			for (int i = 0; i < System.Enum.GetValues(typeof(UpgradeType)).Length; i++)
			{
				var type = (UpgradeType) i;
				if (UpgradeData.GetUpgrades(type).Count > 0)
				{
					_upgradeLevels.Add(type, 0);
					_upgradeMaxLevels.Add(type, UpgradeData.GetUpgrades(type).Count);
				}
			}


			_upgradeMenu = FindObjectOfType<UpgradeMenu>();
		}

		protected void Start ()
		{
			// Reset all static values
			foreach (UpgradeType type in _upgradeLevels.Keys)
			{
				SetUpgrade(type, 0);
			}
		}

		protected void Update ()
		{
#if UNITY_EDITOR
			if (Input.GetKeyDown(KeyCode.U))
			{
				ShowRandomUpgrades();
			}
#endif
		}
	}
}
