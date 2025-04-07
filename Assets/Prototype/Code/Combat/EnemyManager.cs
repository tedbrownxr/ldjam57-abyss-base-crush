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
	public class EnemyManager : MonoBehaviour 
	{
		enum WaveState { Idle, Spawning, Fighting }

		public Action OnWaveComplete;

		public int TotalEnemiesKilled => _totalEnemiesKilled;

		public bool IsWaveComplete => _waveState == WaveState.Idle;

		public GameObject _enemyPrefab;
		private List<Enemy> _enemies;
		private float _spawnTimer;
		private float _spawnDelay;
		private int _spawnCount;
		private int _enemiesRemaining;
		private int _enemiesSpawned;
		private WaveState _waveState;
		int _waveCount;
		int _totalEnemiesKilled;
		int _maxLevel = 0;

		public void Spawn (Vector3 position, int maxLevel)
		{
			// taking advantage of the fact that the base center is at origin
			Quaternion rotation = Quaternion.LookRotation(-position.normalized, Vector3.up);
			Enemy e = Instantiate(_enemyPrefab, position, rotation).GetComponent<Enemy>();
			_enemies.Add(e);
			int level = UnityEngine.Random.Range(0, maxLevel + 1);
			e.SetLevel(level);
		}

		public void ScatterAllEnemies ()
		{
			foreach (Enemy enemy in _enemies)
			{
				enemy.Flee();
			}
			_waveState = WaveState.Idle;
		}

		public void StartWave ()
		{
			_waveState = WaveState.Spawning;
			_spawnCount = Constants.EnemiesPerWave + (_waveCount * Constants.IncreasedEnemiesPerWave);
			_spawnTimer = 0;
			_spawnDelay = Math.Max(Constants.SpawnDelay - (_waveCount * Constants.SpawnDelayAdjustPerWave), Constants.SpawnDelayMin);
			_enemiesSpawned = 0;
			_enemiesRemaining = 0;
			if (_waveCount < Constants.WavesToIncreaseEnemyLevel) _maxLevel = 0;
			else if (_waveCount < Constants.WavesToIncreaseEnemyLevel * 2) _maxLevel = 1;
			else _maxLevel = 2;
			_waveCount++;
		}

		private void HandleAnyEnemyDie (Enemy enemy)
		{
			if (_enemies.Contains(enemy))
			{
				_enemies.Remove(enemy);
			}
			Destroy(enemy.gameObject);
			_enemiesRemaining = Math.Max(_enemiesRemaining - 1, 0);
			_totalEnemiesKilled += enemy.Level;
		}

		protected void Awake ()
		{
			_enemies = new List<Enemy>();
			Enemy.OnAnyEnemyDie += HandleAnyEnemyDie;
		}

		protected void OnDestroy ()
		{
			Enemy.OnAnyEnemyDie -= HandleAnyEnemyDie;
		}

		protected void Update ()
		{
			if (_waveState == WaveState.Spawning)
			{
				_spawnTimer += Time.deltaTime;
				if (_spawnTimer > _spawnDelay)
				{
					_spawnTimer = 0;
					float angle = UnityEngine.Random.Range(-180, 180);
					Vector3 direction = Quaternion.Euler(Vector3.up * angle) * Vector3.forward;
					Vector3 destination = direction * 30;
					Spawn(destination, _maxLevel);
					_enemiesSpawned++;
					_enemiesRemaining++;

					if (_enemiesSpawned >= _spawnCount)
					{
						_waveState = WaveState.Fighting;
					}
				}
			}
			else if (_waveState == WaveState.Fighting)
			{
				if (_enemiesRemaining <= 0)
				{
					_waveState = WaveState.Idle;
					OnWaveComplete?.Invoke();
				}
			}
		}
	}
}
