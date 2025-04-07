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
	public class GroundSprites : MonoBehaviour 
	{
		public Image _deathSplatPrefab;
		public Image _hurtSplatPrefab;

		public Color[] _enemyLevelColors;
		[SerializeField] private RectTransform _rectTransform;

		private void HandleAnyEnemyDie (Enemy enemy)
		{
			Place(_deathSplatPrefab, enemy);
		}

		private void HandleAnyEnemyHurt (Enemy enemy)
		{
			Place(_hurtSplatPrefab, enemy);
		}

		private void Place (Image prefab, Enemy enemy)
		{
			Vector3 enemyPosition = enemy.transform.position;
			Vector3 position = new Vector3(enemyPosition.x, enemyPosition.z, 0);
			Quaternion rotation = Quaternion.Euler(Vector3.forward * UnityEngine.Random.Range(0, 360f));
			int scaleX = UnityEngine.Random.Range(0, 1f) < 0.5f ? -1 : 1;
			int scaleY = UnityEngine.Random.Range(0, 1f) < 0.5f ? -1 : 1;
			Vector3 scale = new Vector3(scaleX, scaleY, 1);
			scale *= UnityEngine.Random.Range(0.8f, 1.2f);
			Image splat = Instantiate(prefab, _rectTransform);
			splat.enabled = true;
			splat.rectTransform.localPosition = position;
			splat.rectTransform.localRotation = rotation;
			splat.rectTransform.localScale = scale;
			splat.color = _enemyLevelColors[enemy.Level - 1];
		}

		protected void Awake ()
		{
			Enemy.OnAnyEnemyDie += HandleAnyEnemyDie;
			Enemy.OnAnyEnemyHurt += HandleAnyEnemyHurt;
			_deathSplatPrefab.enabled = false;
			_hurtSplatPrefab.enabled = false;

		}

		protected void OnDestroy ()
		{
			Enemy.OnAnyEnemyDie -= HandleAnyEnemyDie;
			Enemy.OnAnyEnemyHurt -= HandleAnyEnemyHurt;
		}
	}
}
