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
	public class UpgradeState : AppState 
	{
		public const string Name = "Upgrade";
		public override string StateName => Name;

		public GameObject _repairButton;
		private MainBase _mainBase;
		private UpgradeManager _upgradeManager;

		public void RequestChangeState ()
		{
			_app.GoToStateOnLateUpdate(CombatState.Name);
		}

		protected override void OnAfterEnter()
		{
			if (_mainBase == null) _mainBase = FindObjectOfType<MainBase>();
			_repairButton.SetActive(_mainBase.HasDamagedBuildingsThatCanBeRepaired);

			if (_upgradeManager == null) _upgradeManager = FindObjectOfType<UpgradeManager>();
			_upgradeManager.ShowRandomUpgrades();
			_upgradeManager.OnUpgradeSelected += HandleUpgradeSelected;
		}

		protected override void OnProcess()
		{
			
		}

		protected override void OnBeforeExit()
		{
			_upgradeManager.OnUpgradeSelected -= HandleUpgradeSelected;
		}

		private void HandleUpgradeSelected (Upgrade upgrade)
		{
			_app.GoToStateOnLateUpdate(CombatState.Name);
		}
	}
}
