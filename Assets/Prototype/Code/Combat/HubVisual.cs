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
	public class HubVisual : MonoBehaviour 
	{
		const float DamageVibrateDuration = 0.4f;
		const float MaxVibrateDistance = 0.4f;

		public GameObject _wholeObject;
		public GameObject _brokenObject;

		protected Building _building;
		protected Renderer _wholeRenderer;
		protected Vector3 _basePosition;

		protected bool _hasBeenDestroyed;
		protected bool _isDestroyed;
		protected float _damageTimer;
		protected Color _normalColor = Color.gray;
		protected Color _damageColor = Color.red;
		protected Vector3 _damagePosition;
		private Dictionary<Renderer, Color> _baseColors;

		public void HandleDamage (int amount)
		{
			_damageTimer = 0;
		}

		public void SetSelected (bool isSelected)
		{

		}

		public void SetTargeted (bool isTargeted)
		{
			_normalColor = isTargeted ? Color.white : Color.gray;
			ColorTool.SetRendererColor(_wholeRenderer, _normalColor);
		}

		public void HandleDestroy (Building building)
		{
			_isDestroyed = true;
		}

		protected void Awake ()
		{
			_baseColors = new Dictionary<Renderer, Color>();
			foreach (Renderer r in gameObject.GetComponentsInChildren<Renderer>())
			{
				_baseColors.Add(r, r.sharedMaterial.color);
			}
		}
		protected void Start ()
		{
			_building = GetComponent<Building>();
			_building.OnDamage += HandleDamage;
			_building.OnDestroy += HandleDestroy;
			if (_brokenObject != null) _brokenObject.SetActive(false);
			_wholeObject.SetActive(true);
			_basePosition = _wholeObject.transform.position;
			_wholeRenderer = _wholeObject.GetComponent<Renderer>();
			_damageTimer = DamageVibrateDuration;
		}

		protected void Update ()
		{
			if (_damageTimer < DamageVibrateDuration)
			{
				_damageTimer += Time.deltaTime;
				float t = Mathf.Clamp01(_damageTimer / DamageVibrateDuration);
				float ox = UnityEngine.Random.Range(-MaxVibrateDistance, MaxVibrateDistance) * (1-t);
				float oz = UnityEngine.Random.Range(-MaxVibrateDistance, MaxVibrateDistance) * (1-t);
				_wholeObject.transform.position = _basePosition + new Vector3(ox, 0, oz);

				foreach (Renderer r in _baseColors.Keys)
				{
					Color c = Color.Lerp(_damageColor, _baseColors[r], t);
					ColorTool.SetRendererColor(r, c);
				}				
			}
			else
			{
				if (_hasBeenDestroyed == false && _isDestroyed)
				{
					_wholeObject.SetActive(false);
					if (_brokenObject != null) _brokenObject.SetActive(true);
					_hasBeenDestroyed = true;
				}
			}
		}		
	}
}
