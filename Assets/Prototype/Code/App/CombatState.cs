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
	public class CombatState : AppState
	{
		public const string Name = "Combat";
		public override string StateName => Name;

		private Hub _hub;
		private EnemyManager _enemyManager;
		private GunManager _gunManager;
		float _nextStateTimer;
		float _winDelay = 3;
		float _failDelay = 6;
		bool _failed;
		bool _finished;

		protected override void OnAfterEnter()
		{
			if (_hub == null) _hub = FindObjectOfType<Hub>();
			_hub.OnDestroy += HandleBuildingDestroyed;
			
			if (_enemyManager == null) _enemyManager = FindObjectOfType<EnemyManager>();
			_enemyManager.OnWaveComplete += HandleWaveComplete;

			if (_gunManager == null) _gunManager = FindObjectOfType<GunManager>();
			_enemyManager.StartWave();
			_nextStateTimer = 0;
			_failed = false;
			_finished = false;
		}

		protected override void OnProcess()
		{
			if (_failed)
			{
				_nextStateTimer += Time.deltaTime;
				if (_nextStateTimer > _failDelay)
				{
					_app.GoToStateOnLateUpdate(EndingState.Name);
				}
			}
			else if (_finished)
			{
				_nextStateTimer += Time.deltaTime;
				if (_nextStateTimer > _winDelay)
				{
					_app.GoToStateOnLateUpdate(UpgradeState.Name);
				}
			}
#if UNITY_EDITOR
			else if (Input.GetKeyDown(KeyCode.Q))
			{
				_hub.TakeDamage(100000000);
			}
#endif
		}

		protected override void OnBeforeExit()
		{
			_enemyManager.OnWaveComplete -= HandleWaveComplete;
			_hub.OnDestroy -= HandleBuildingDestroyed;
		}

		// we know this is the hub based on how we set it up
		private void HandleBuildingDestroyed (Building building)
		{
			_failed = true;
			_gunManager.StopGuns();
			_enemyManager.ScatterAllEnemies();
		}

		private void HandleWaveComplete ()
		{
			_finished = true;
		}
	}
}
