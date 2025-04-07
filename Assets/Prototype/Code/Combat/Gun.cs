using DevKit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Prototype
{
	public class Gun : MonoBehaviour 
	{
		const float BarrelLength = 1.3f;

		public Vector3 Direction => transform.forward;
		public Vector3 Firepoint => transform.position + Direction * (BarrelLength + 0.5f);

		private bool _isActive;
		private Turret _turret;
		private float _lastFiredTime;
		private Dictionary<Enemy, float> _enemiesAndDistance;
		private List<Enemy> _keys;
		private List<KeyValuePair<Enemy, float>> _enemiesOrderedByNearestDistance;

		public void Deactivate ()
		{
			_isActive = false;
		}

		public void Fire ()
		{
			FXManager.Instance.PlayGunfireFX(Firepoint);
			_lastFiredTime = Time.time;
			Vector3 firepoint = Firepoint;
			_keys.Clear();
			_keys.AddRange(_enemiesAndDistance.Keys);
			foreach (var enemy in _keys)
			{
				float d = (enemy.transform.position - firepoint).magnitude;
				_enemiesAndDistance[enemy] = d;
			}
			_enemiesOrderedByNearestDistance.Clear();
			_enemiesOrderedByNearestDistance.AddRange(_enemiesAndDistance.OrderBy(x => x.Value));
			for (int i = 0; i < Constants.GunPenetration + 1; i++)
			{
				if (i < _enemiesOrderedByNearestDistance.Count)
				{
					_enemiesOrderedByNearestDistance[i].Key.TakeDamage(Constants.GunDamage);
				}
			}
		}

		private void HandleWaveComplete ()
		{
			_enemiesAndDistance.Clear();
			_enemiesOrderedByNearestDistance.Clear();
		}

		private void HandleAnyEnemyDie (Enemy enemy)
		{
			if (_enemiesAndDistance.ContainsKey(enemy))
			{
				_enemiesAndDistance.Remove(enemy);
			}
		}

		protected void OnTriggerEnter (Collider other)
		{
			Enemy e = other.GetComponentInParent<Enemy>();
			if (e && _enemiesAndDistance.ContainsKey(e) == false)
			{
				_enemiesAndDistance.Add(e, float.MaxValue);
			}
		}

		protected void OnTriggerStay (Collider other)
		{
			Enemy e = other.GetComponentInParent<Enemy>();
			if (e && _enemiesAndDistance.ContainsKey(e) == false)
			{
				_enemiesAndDistance.Add(e, float.MaxValue);
			}
		}

		protected void OnTriggerExit (Collider other)
		{
			Enemy e = other.GetComponentInParent<Enemy>();
			if (e && _enemiesAndDistance.ContainsKey(e))
			{
				_enemiesAndDistance.Remove(e);
			}
		}

		protected void Awake ()
		{
			_turret = GetComponentInParent<Turret>();
			_enemiesAndDistance = new Dictionary<Enemy, float>(16);
			_enemiesOrderedByNearestDistance = new List<KeyValuePair<Enemy, float>>(16);
			Enemy.OnAnyEnemyDie += HandleAnyEnemyDie;
			_keys = new List<Enemy>(16);
			FindObjectOfType<EnemyManager>().OnWaveComplete += HandleWaveComplete;
			_isActive = true;
		}

		protected void OnDestroy ()
		{
			Enemy.OnAnyEnemyDie -= HandleAnyEnemyDie;
		}

		protected void Update ()
		{
			if (_isActive && _enemiesAndDistance.Count > 0)
			{
				if (Time.time - _lastFiredTime >= Constants.GunFireCooldown)
				{
					Fire();
				}
			}
		}
	}
}
