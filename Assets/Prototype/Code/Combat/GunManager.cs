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
	public class GunManager : MonoBehaviour 
	{
		private List<Turret> _turrets;

		public void AddTurret (Turret turret)
		{
			if (_turrets == null) _turrets = new List<Turret>();
			_turrets.Add(turret);
		}

		public void StopGuns ()
		{
			foreach (Turret turret in _turrets)
			{
				turret.Deactivate();
			}	
		}

		public void SetUpgrade (Upgrade upgrade)
		{
			switch (upgrade.UpgradeType)
			{
				case UpgradeType.GunCount:
					Debug.Log(upgrade.UpgradeType + " set to " + upgrade.IntValue);
					foreach (Turret turret in _turrets)
					{
						turret.SetGunCount(upgrade.IntValue);
					}
					break;

				case UpgradeType.GunDamage:
					Debug.Log(upgrade.UpgradeType + " set to " + upgrade.IntValue);
					Constants.GunDamage = upgrade.IntValue;
					break;

				case UpgradeType.GunPenetration:
					Debug.Log(upgrade.UpgradeType + " set to " + upgrade.IntValue);
					Constants.GunPenetration = upgrade.IntValue;
					break;

				case UpgradeType.GunRateOfFire:
					Debug.Log(upgrade.UpgradeType + " set to " + upgrade.FloatValue);
					Constants.GunFireCooldown = upgrade.FloatValue;
					break;

				case UpgradeType.GunTurnSpeed:
					Debug.Log(upgrade.UpgradeType + " set to " + upgrade.FloatValue);
					Constants.GunTurnSpeed = upgrade.FloatValue;
					break;

				default:
					Debug.LogWarning($"GunManager: UpgradeType.{upgrade.UpgradeType} has not been implemented");
					break;
			}
		}

		protected void Update ()
		{
#if UNITY_EDITOR
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				foreach (Turret turret in _turrets)
				{
					turret.SetGunCount(1);
				}				
			}

			if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				foreach (Turret turret in _turrets)
				{
					turret.SetGunCount(2);
				}				
			}

			if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				foreach (Turret turret in _turrets)
				{
					turret.SetGunCount(3);
				}				
			}

			if (Input.GetKeyDown(KeyCode.Alpha4))
			{
				foreach (Turret turret in _turrets)
				{
					turret.SetGunCount(4);
				}				
			}

			if (Input.GetKeyDown(KeyCode.Alpha5))
			{
				foreach (Turret turret in _turrets)
				{
					turret.SetGunCount(5);
				}				
			}
#endif							
		}
	}
}
