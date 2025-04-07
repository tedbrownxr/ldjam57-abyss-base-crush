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
	public class UpgradeButton : MonoBehaviour 
	{
		public TMP_Text _name;
		public TMP_Text _description;
		
		private bool _isValid;
		private UpgradeManager _upgradeManager;
		private UpgradeType _upgradeType;
		private int _nextLevel;
		UpgradeMenu _upgradeMenu;

		public void HandleClick ()
		{
			if (_isValid)
			{
				_upgradeManager.HandleUpgrade(_upgradeType);
			}
			else Debug.LogError("upgrade button not valid", gameObject);
			_upgradeMenu.HideUpgrades();
		}

		public void ShowUpgrade (UpgradeType upgradeType, int level)
		{
			_isValid = UpgradeData.TryGetUpgrade(upgradeType, level, out Upgrade upgrade);
			if (_isValid)
			{
				_name.text = upgrade.Name;
				_description.text = upgrade.Description;
				_upgradeType = upgradeType;
				_nextLevel = level;
			}
		}

		protected void Awake ()
		{
			_upgradeManager = FindObjectOfType<UpgradeManager>();
			_upgradeMenu = GetComponentInParent<UpgradeMenu>();
		}
	}
}
