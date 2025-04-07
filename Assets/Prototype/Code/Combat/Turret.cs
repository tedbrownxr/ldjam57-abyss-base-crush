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
	public class Turret : MonoBehaviour 
	{
		public Gun[] _outerGuns;
		public Gun[] _innerGuns;
		public Gun _centerGun;
		public LineRenderer _debugLine;

		private bool _isActive;
		private List<Gun> _activeGuns;
		public TurretAwareness _awareness;
		private Dictionary<Enemy, Threat> _threats;

		public void Deactivate ()
		{
			_isActive = false;
			foreach (Gun gun in _activeGuns)
			{
				gun.Deactivate();
			}
		}

		public void SetGunCount (int value)
		{
			foreach (Gun gun in GetComponentsInChildren<Gun>())
			{
				gun.gameObject.SetActive(false);
			}
			
			if (_activeGuns == null) _activeGuns = new List<Gun>();
			else _activeGuns.Clear();

			switch (value)
			{
				case 1: 
					_activeGuns.Add(_centerGun); 
					break;
				case 2:
					_activeGuns.AddRange(_innerGuns);
					break;
				case 3:
					_activeGuns.Add(_centerGun); 
					_activeGuns.AddRange(_outerGuns);
					break;
				case 4:
					_activeGuns.AddRange(_innerGuns);
					_activeGuns.AddRange(_outerGuns);
					break;
				case 5:
					_activeGuns.Add(_centerGun); 
					_activeGuns.AddRange(_innerGuns);
					_activeGuns.AddRange(_outerGuns);
					break;
				default:
					Debug.LogError("can't set gun count to value " + value);
					break;
			}

			foreach (Gun gun in _activeGuns)
			{
				gun.gameObject.SetActive(true);
			}
		}

		protected void Awake ()
		{
			FindObjectOfType<GunManager>().AddTurret(this);
			_isActive = true;
		}

		protected void Update ()
		{
			if (_isActive == false)
			{
				return;
			}
			if (_awareness.TryGetGreatestThreat(out Threat threat))
			{
				_debugLine.gameObject.SetActive(Constants.TurretShowTargetLine);
				Vector3 a = transform.position;
				Vector3 b = threat.Enemy.transform.position;
				a.y = 1;
				b.y = 1;
				_debugLine.SetPositions(new Vector3[] { a, b } );
				
				Vector3 directionToTarget = (b - a).normalized;
				float deltaAngle = Vector3.SignedAngle(transform.forward, directionToTarget, Vector3.up);
				float maxTurnAngle = Constants.GunTurnSpeed * Time.deltaTime;
				float turnAngle = Mathf.Min(Mathf.Abs(deltaAngle), maxTurnAngle);
				if (deltaAngle < 0)
				{
					turnAngle *= -1;
				}
				transform.Rotate(Vector3.up * turnAngle, Space.World);
			}
			else
			{
				_debugLine.gameObject.SetActive(false);
			}
		}
	}
}
