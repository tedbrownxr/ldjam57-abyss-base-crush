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
	public class EnemyDamageVisuals : MonoBehaviour 
	{
		public GameObject _renderObject;

		private Dictionary<Renderer, Color> _baseColors;
		private Enemy _enemy;
		float _timer;
		float _duration = 0.35f;

		private void HandleDamage (Enemy enemy, int amount)
		{
			_timer = 0;
		}

		// make sure it happens after Set Level on this enemy
		protected void Start ()
		{
			_enemy = GetComponent<Enemy>();
			_enemy.OnDamage += HandleDamage;
			_baseColors = new Dictionary<Renderer, Color>();
			foreach (Renderer r in _renderObject.GetComponentsInChildren<Renderer>())
			{
				_baseColors.Add(r, r.sharedMaterial.color);
			}
			_timer = _duration;
		}

		protected void Update ()
		{
			if (_timer < _duration)
			{
				_timer += Time.deltaTime;
				float t = Mathf.Clamp01(_timer / _duration);
				foreach (Renderer r in _baseColors.Keys)
				{
					ColorTool.SetRendererColor(r, Color.Lerp(Color.white, _baseColors[r], t));
				}
			}
		}
	}
}
