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
	public class UpgradeMenu : MonoBehaviour 
	{
		public GameObject _upgradeMenu;
		public UpgradeButton[] _upgradeButtons;

		public void HideUpgrades ()
		{
			_upgradeMenu.SetActive(false);
		}

		public void ShowUpgrade1 (UpgradeType a, int levelA)
		{
			foreach (UpgradeButton button in _upgradeButtons) button.gameObject.SetActive(true);
			_upgradeButtons[0].gameObject.SetActive(false);
			_upgradeButtons[1].ShowUpgrade(a, levelA);
			_upgradeButtons[2].gameObject.SetActive(false);
			_upgradeMenu.SetActive(true);
		}

		public void ShowUpgrades2 (UpgradeType a, int levelA, UpgradeType b, int levelB)
		{
			foreach (UpgradeButton button in _upgradeButtons) button.gameObject.SetActive(true);
			_upgradeButtons[0].ShowUpgrade(a, levelA);
			_upgradeButtons[1].gameObject.SetActive(false);
			_upgradeButtons[2].ShowUpgrade(b, levelB);
			_upgradeMenu.SetActive(true);
		}

		public void ShowUpgrades3 (UpgradeType a, int levelA, UpgradeType b, int levelB, UpgradeType c, int levelC)
		{
			foreach (UpgradeButton button in _upgradeButtons) button.gameObject.SetActive(true);
			_upgradeButtons[0].ShowUpgrade(a, levelA);
			_upgradeButtons[1].ShowUpgrade(b, levelB);
			_upgradeButtons[2].ShowUpgrade(c, levelC);
			_upgradeMenu.SetActive(true);
		}
	}
}
