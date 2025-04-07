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
	public class TurretAwareness : MonoBehaviour 
	{
		private AwarenessTrigger[] _triggers;
		private HashSet<AwarenessTrigger> _triggerHashSet;
		private Dictionary<Enemy, Threat> _threats;
		public Building Building => _building;

		private Building _building;
		private List<Threat> _prioritizedThreats;

		public List<Threat> GetThreatsForTurret (HashSet<AwarenessTrigger> awarenessTriggers)
		{
			List<Threat> threats = new List<Threat>();
			foreach (Threat threat in _threats.Values)
			{
				if (threat.IsSeenByTriggers(awarenessTriggers))
				{
					threats.Add(threat);
				}
			}
			return threats;
		}

		public void RemoveEnemy (Enemy enemy)
		{
			if (_threats.ContainsKey(enemy))
			{
				_threats.Remove(enemy);
			}
		}

		public bool TryGetGreatestThreat (out Threat threat)
		{
			if (_prioritizedThreats.Count > 0)
			{
				threat = _prioritizedThreats[0];
				return true;
			}
			threat = null;
			return false;
		}

		public void HandleEnemyEnter (Enemy enemy, AwarenessTrigger trigger)
		{
			if (_threats.TryGetValue(enemy, out Threat threat) == false)
			{
				threat = new Threat(enemy);
				_threats[enemy] = threat;
			}
			threat.AddAwarenessTrigger(trigger);
		}

		public void HandleEnemyExit (Enemy enemy, AwarenessTrigger trigger)
		{
			if (_threats.TryGetValue(enemy, out Threat threat))
			{
				threat.RemoveAwarenessTrigger(trigger);
				if (threat.AwarenessTriggers.Count == 0)
				{
					_threats.Remove(enemy);
				}
			}
		}

		private void HandleEnemyDie (Enemy enemy)
		{
			if (_threats.ContainsKey(enemy))
			{
				_threats.Remove(enemy);
			}
		}

		protected void Awake ()
		{
			_triggers = GetComponentsInChildren<AwarenessTrigger>();
			_triggerHashSet = new HashSet<AwarenessTrigger>();
			foreach (AwarenessTrigger trigger in _triggers)
			{
				_triggerHashSet.Add(trigger);
			}
			_threats = new Dictionary<Enemy, Threat>(256);
			Enemy.OnAnyEnemyDie += HandleEnemyDie;			
			_prioritizedThreats = new List<Threat>(256);
			_building = GetComponentInParent<Building>();
		}

		protected void LateUpdate ()
		{
			foreach (Threat threat in _threats.Values)
			{
				threat.UpdateThreat();
			}
			_prioritizedThreats.Clear();
			_prioritizedThreats.AddRange(_threats.Values.OrderByDescending(x => x.Danger));
		}

		protected void OnDestroy ()
		{
			Enemy.OnAnyEnemyDie -= HandleEnemyDie;			
		}
	}
}
