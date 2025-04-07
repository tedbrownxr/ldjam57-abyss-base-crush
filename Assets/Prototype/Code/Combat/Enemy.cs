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
	public class Enemy : MonoBehaviour 
	{
		public enum State { Confused, Approach, Attack, Wander, Flee }
		public static Action<Enemy> OnAnyEnemyDie;
		public static Action<Enemy> OnAnyEnemyHurt;

		public Action<Enemy> OnDie;
		public Action<Enemy, int> OnDamage;

		public Material[] _bodyMaterials;

		public int Level => _semanticLevel;
		private bool _isTryingToFlee;
		public Transform _visualsObject;
		public float DistanceToAttackerPosition;
		protected float _attackRadius = 0.5f;
		protected float _attackTimer;
		protected AttackerPosition _attackerPosition;
		protected NavMeshAgent _agent;
		protected float _wanderTimer = 0;
		private State _state;
		private int _hitpoints;
		private CreepyLegs _legs;
		private EnemyClaws _claws;
		public Renderer[] _levelRenderers;
		float _sizeScale = 1;
		int _semanticLevel = 1; // meaning level 0 is level 1

		static NavMeshPath s_navMeshPathCheck;

		public void SetLevel (int level)
		{
			_semanticLevel = level;
			foreach (Renderer r in _levelRenderers)
			{
				r.sharedMaterial = _bodyMaterials[level];
			}
			_sizeScale = (1 + (float) level * 0.5f);
			transform.localScale = Vector3.one * _sizeScale;

			_semanticLevel = level + 1;
			_agent.speed = Constants.EnemyMoveSpeed - level;
			_hitpoints = Constants.EnemyHitPoints * _semanticLevel * _semanticLevel;
		}

		public void Flee ()
		{
			_isTryingToFlee = true;
			float angle = UnityEngine.Random.Range(-180, 180);
			Vector3 direction = Quaternion.Euler(Vector3.up * angle) * Vector3.forward;
			// assumes origin of 0,0,0
			Vector3 destination = direction * Constants.EnemyFleeDistance;
			if (NavMesh.SamplePosition(destination, out NavMeshHit hit, 1, NavMesh.AllAreas))
			{
				_agent.enabled = true;
				_agent.isStopped = false;
				_agent.SetDestination(destination);
				_wanderTimer = 0;
				_state = State.Flee;
				_agent.speed *= 2;
				_agent.angularSpeed *= 2;
				_agent.acceleration *= 2;
			}
			else
			{
				_state = State.Confused;
			}
		}

		public void TakeDamage (int amount)
		{
			int prev = _hitpoints;
			_hitpoints = Math.Max(_hitpoints - amount, 0);
			OnDamage?.Invoke(this, prev - _hitpoints);
			if (_hitpoints <= 0)
			{
				OnAnyEnemyDie?.Invoke(this);
				OnDie?.Invoke(this);
			}
			else
			{
				OnAnyEnemyHurt?.Invoke(this);
			}
		}

		public bool TryToWander ()
		{
			float radius = UnityEngine.Random.Range(Constants.EnemyWanderMin, Constants.EnemyWanderMax);
			float angle = UnityEngine.Random.Range(-180, 180);
			Vector3 direction = Quaternion.Euler(Vector3.up * angle) * Vector3.forward;
			// assumes origin of 0,0,0
			Vector3 destination = direction * radius;
			if (NavMesh.SamplePosition(destination, out NavMeshHit hit, 1, NavMesh.AllAreas))
			{
				_agent.enabled = true;
				_agent.isStopped = false;
				_agent.SetDestination(destination);
				_wanderTimer = 0;
				_state = State.Wander;
				return true;
			}
			_state = State.Confused;
			return false;
		}

		public bool TryToFindAttackPosition ()
		{
			IEnumerable<KeyValuePair<Building, float>> buildingsSortedByDistance = MainBase.Instance.GetBuildingsSortedByDistance(transform.position);
			AttackerPosition attackerPosition;
			foreach (var pair in buildingsSortedByDistance)
			{
				if (pair.Key.TryGetOpenAttackerPosition(out attackerPosition))
				{
					if (NavMesh.SamplePosition(attackerPosition.Position, out NavMeshHit hit, 0.2f, NavMesh.AllAreas))
					{
				    	NavMeshPath path = new NavMeshPath();
						_agent.CalculatePath(attackerPosition.Position, s_navMeshPathCheck);
						if (s_navMeshPathCheck.status == NavMeshPathStatus.PathComplete)
						{
							_attackerPosition = attackerPosition;
							_attackerPosition.Claim(this);
							_agent.enabled = true;
							_agent.isStopped = false;
							_agent.SetDestination(_attackerPosition.Position);
							_state = State.Approach;
							return true;
						}
					}
				}
			}
			_state = State.Confused;
			return false;
		}

		public void SetPosition (Vector3 position)
		{
			if (NavMesh.SamplePosition(position, out NavMeshHit hit, 1, NavMesh.AllAreas))
			{
				position = hit.position;
				_agent.Warp(position);
			}
			else
			{
				Debug.LogError("not on a mesh", gameObject);
				return;
			}
		}

		protected void Awake ()
		{
			_agent = GetComponent<NavMeshAgent>();
			_agent.speed = Constants.EnemyMoveSpeed;
			_hitpoints = Constants.EnemyHitPoints;
			_legs = GetComponent<CreepyLegs>();
			_claws = GetComponent<EnemyClaws>();
			if (s_navMeshPathCheck == null) s_navMeshPathCheck = new NavMeshPath();
		}

		protected void OnDestroy ()
		{
			if (_attackerPosition != null)
			{
				_attackerPosition.ReleaseClaim(this);
			}
		}

		protected void Update ()
		{
			_visualsObject.localRotation = Quaternion.identity;

			if (_state == State.Confused)
			{
				if (_isTryingToFlee)
				{
					Flee();
					return;
				}
				if (TryToFindAttackPosition() == false)
				{
					TryToWander();
				}
				return;
			}

			if (_state == State.Wander)
			{
				_legs.Process();
				_wanderTimer += Time.deltaTime;
				if (_wanderTimer > Constants.EnemyWanderDuration || _agent.remainingDistance < 1)
				{
					_wanderTimer = 0;
					if (TryToFindAttackPosition() == false)
					{
						if (_agent.remainingDistance < 1)
						{
							TryToWander();
						}
					}
				}
				return;
			}

			if (_state == State.Approach)
			{
				if (_attackerPosition == null || _attackerPosition.Building.Hitpoints <= 0)
				{
					_legs.Reset();
					_state = State.Confused;
					return;
				}
				_legs.Process();

				DistanceToAttackerPosition = Vector3.Distance(transform.position, _attackerPosition.Position);
				float attackRadius = _attackerPosition.Building is Hub ? _attackRadius * 1.5f : _attackRadius;
				attackRadius *= _sizeScale;
				if (DistanceToAttackerPosition <= attackRadius)
				{
					_visualsObject.rotation = _attackerPosition.Rotation;
					_legs.Reset();
					_state = State.Attack;
				}
				return;
			}

			if (_state == State.Attack)
			{
				if (_attackerPosition == null || _attackerPosition.Building.Hitpoints <= 0)
				{
					_state = State.Confused;
					_claws.Reset();
					return;
				}

				_visualsObject.rotation = _attackerPosition.Rotation;
				_attackTimer += Time.deltaTime;
				if (_attackTimer >= Constants.EnemyAttackCooldown)
				{
					_attackerPosition.Building.TakeDamage(Constants.EnemyDamage + Constants.EnemyDamageBonusPerLevel * _semanticLevel);
					_attackTimer = 0;
					_claws.Reset();
				}
				else
				{
					float t = Mathf.Clamp01(_attackTimer / Constants.EnemyAttackCooldown);
					if (t > 0.5f)
					_claws.Process(Mathf.Clamp01((t - 0.5f) * 2));
				}
				return;
			}
		}
	}
}
