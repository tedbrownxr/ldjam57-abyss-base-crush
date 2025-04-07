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
	public class AwarenessTrigger : MonoBehaviour 
	{
		// the center trigger uses distance to origin. side triggers use local Z.
		public bool _useDistanceToTurret;
		public HashSet<Enemy> _enemies;
		public List<Enemy> _debugEnemyList;

		private TurretAwareness _turretAwareness;

		public bool Contains (Enemy enemy)
		{
			return _enemies.Contains(enemy);
		}
		
		public float GetDistance (Vector3 position)
		{
			if (_useDistanceToTurret)
			{
				Vector3 home = _turretAwareness.Building.transform.position;
				position.y = home.y;
				return Vector3.Distance(position, home) - Tower.Radius;
			}
			position = transform.InverseTransformPoint(position);
			return position.z * transform.localScale.z;
		}

		protected void OnTriggerEnter (Collider other)
		{
			Enemy e = other.GetComponentInParent<Enemy>();
			if (e && _enemies.Contains(e) == false)
			{
				_turretAwareness.HandleEnemyEnter(e, this);
				_enemies.Add(e);
			}
		}

		protected void OnTriggerStay (Collider other)
		{
			Enemy e = other.GetComponentInParent<Enemy>();
			if (e && _enemies.Contains(e) == false)
			{
				_turretAwareness.HandleEnemyEnter(e, this);
				_enemies.Add(e);
			}
		}

		protected void OnTriggerExit (Collider other)
		{
			Enemy e = other.GetComponentInParent<Enemy>();
			if (e && _enemies.Contains(e))
			{
				_turretAwareness.HandleEnemyExit(e, this);
				_enemies.Remove(e);
			}
		}

		protected void Awake ()
		{
			_enemies = new HashSet<Enemy>();
			_debugEnemyList = new List<Enemy>();
			_turretAwareness = GetComponentInParent<TurretAwareness>();
		}

		protected void Update ()
		{
			_debugEnemyList.Clear();
			_debugEnemyList.AddRange(_enemies);
		}	
	}
}
