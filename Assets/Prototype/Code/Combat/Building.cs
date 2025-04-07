using DevKit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Prototype
{
	[SelectionBase]
	public class Building : MonoBehaviour 
	{
		public Action<int> OnDamage;
		public Action<Building> OnDestroy;

		public bool IsDestroyed => _hitpoints <= 0;
		public bool IsDamaged => _hitpoints < _maxHitpoints;

		public Vector3 HealthIconWorldAnchor => _healthIconWorldAnchor.position;
		public int MaxHitpoints => _maxHitpoints;
		public int Hitpoints => _hitpoints;
		public AttackerPosition[] AttackerPositions => _attackerPositions;
		public GameObject[] _deactivateOnDestroyObjects;
		public ParticleSystem _particleSystem;
	
		protected int _maxHitpoints;
		protected int _hitpoints;
		protected AttackerPosition[] _attackerPositions;
		protected PointerTarget _pointerTarget;
		protected bool _isTargeted;
		[SerializeField] private Transform _healthIconWorldAnchor;
		bool _particlesHavePlayed;

		public void RestoreHealth ()
		{
			_hitpoints = _maxHitpoints;
		}

		public void TakeDamage (int amount)
		{
			int prev = _hitpoints;
			_hitpoints = Math.Max(_hitpoints - amount, 0);
			OnDamage?.Invoke(prev - _hitpoints);
			if (_hitpoints <= 0)
			{
				OnDestroy?.Invoke(this);
				foreach (GameObject go in _deactivateOnDestroyObjects)
				{
					go.SetActive(false);
				}
				if (_particlesHavePlayed == false)
				{
					if (_particleSystem != null)
					{
						_particleSystem.Play();
					}
					_particlesHavePlayed = true;
				}
			}
		}

		public bool TryGetOpenAttackerPosition (out AttackerPosition attackerPosition)
		{
			if (_hitpoints <= 0 || _attackerPositions.Length == 0) 
			{
				attackerPosition = null;
				return false;
			}

			List<int> indices = new List<int>();
			for (int i = 0; i < _attackerPositions.Length; i++)
			{
				if (_attackerPositions[i].IsClaimed == false)
				{
					indices.Add(i);
				}
			}

			if (indices.Count == 0)
			{
				attackerPosition = null;
				return false;
			}

			int index = UnityEngine.Random.Range(0, indices.Count);
			attackerPosition = _attackerPositions[index];
			return true;
		}

		public virtual void SetAttackerPositions (float attackerRadius)
		{
			_attackerPositions = new AttackerPosition[0];
		}

		protected virtual void HandleTarget (Pointer pointer)
		{
			_isTargeted = true;
		}

		protected virtual void HandleUntarget (Pointer pointer)
		{
			_isTargeted = false;
		}

		protected virtual void Awake ()
		{
			_maxHitpoints = Constants.BuildingHitPoints;
			_hitpoints = Constants.BuildingHitPoints;
			_pointerTarget = GetComponent<PointerTarget>();
			_pointerTarget.OnTarget += HandleTarget;
			_pointerTarget.OnUntarget += HandleUntarget;
		}

		protected void OnDrawGizmos()
		{
			if (_attackerPositions == null || _hitpoints <= 0 || _attackerPositions.Length == 0) return;
			Color claimedColor = new Color(1f, 0f, 0f, 1f);
			Color unclaimedColor = new Color(0f, 0f, 1f, 1f);
			foreach (AttackerPosition attackerPosition in _attackerPositions)
			{
				Gizmos.color = attackerPosition.IsClaimed ? claimedColor : unclaimedColor;
				Gizmos.DrawWireSphere(attackerPosition.Position, 0.6f);
			}
		}

		// protected void Update ()
		// {
		// 	if (_isTargeted)
		// 	{
		// 		if (Input.GetMouseButtonDown(0))
		// 		{
		// 			FindObjectOfType<UnitManager>().Units[0].MoveTo(this);
		// 		}
		// 	}
		// }
	}
}
