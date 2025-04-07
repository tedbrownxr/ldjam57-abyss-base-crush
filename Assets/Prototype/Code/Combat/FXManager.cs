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
	public class FXManager : MonoBehaviour 
	{
		public static FXManager Instance;

		public GameObject _gunfireFxPrefab;

		private List<IFX> _gunfireFX;

		public void PlayGunfireFX (Vector3 position)
		{
			IFX fx = null;
			foreach (IFX existingFX in _gunfireFX)
			{
				if (existingFX.IsActive() == false)
				{
					fx = existingFX;
					break;
				}
			}
			if (fx == null)
			{
				fx = Instantiate(_gunfireFxPrefab).GetComponent<GunfireFX>();
				_gunfireFX.Add(fx);
			}
			fx.Activate(position);
		}

		protected void Awake ()
		{
			Instance = this;
			_gunfireFX = new List<IFX>();
		}

		protected void Update ()
		{
			foreach (IFX fx in _gunfireFX)
			{
				fx.Process();
			}
		}
	}
}
