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
	public class TunnelVisual : MonoBehaviour 
	{
		const float DamageVibrateDuration = 0.4f;
		const float MaxVibrateDistance = 0.4f;

		public GameObject _capA;
		public GameObject _wedgeA;
		public GameObject[] _middles;
		public GameObject _capB;
		public GameObject _wedgeB;

		protected Tunnel _tunnel;
		protected List<Renderer> _wholeRenderers;
		protected Vector3 _basePosition;

		protected bool _hasBeenDestroyed;
		protected bool _isDestroyed;
		protected float _damageTimer;
		protected Color _damageColor = Color.red;
		protected Vector3 _damagePosition;
		private Dictionary<Renderer, Color> _baseColors;

		public void HandleAnchorADestroyed ()
		{
			_capA.SetActive(false);
			_wedgeA.SetActive(false);
		}

		public void HandleAnchorBDestroyed ()
		{
			_capB.SetActive(false);
			_wedgeB.SetActive(false);
		}

		public void HandleDamage (int amount)
		{
			_damageTimer = 0;
		}

		public void HandleDestroy (Building building)
		{
			_isDestroyed = true;
		}

		public void SetTargeted (bool isTargeted)
		{
			foreach (Renderer r in _wholeRenderers)
			{
				Color c = isTargeted ? Color.white : _baseColors[r];
				ColorTool.SetRendererColor(r, c);
			}
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
			_tunnel = GetComponent<Tunnel>();
			_tunnel.OnDamage += HandleDamage;
			_tunnel.OnDestroy += HandleDestroy;
			_tunnel.OnAnchorADestroyed += HandleAnchorADestroyed;
			_tunnel.OnAnchorBDestroyed += HandleAnchorBDestroyed;
			_basePosition = transform.position;
			_wholeRenderers = new List<Renderer>();
			_wholeRenderers.AddRange(_wedgeA.GetComponentsInChildren<Renderer>());
			_wholeRenderers.AddRange(_wedgeB.GetComponentsInChildren<Renderer>());
			foreach (GameObject go in _middles)
			{
				_wholeRenderers.AddRange(go.GetComponentsInChildren<Renderer>());
			}
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
				transform.position = _basePosition + new Vector3(ox, 0, oz);
				foreach (Renderer r in _wholeRenderers)
				{
					Color c = Color.Lerp(_damageColor, _baseColors[r], t);
					ColorTool.SetRendererColor(r, c);
				}
			}
			else
			{
				if (_hasBeenDestroyed == false && _isDestroyed)
				{
					foreach (GameObject go in _middles)
					{
						go.SetActive(false);
					}
					_wedgeA.SetActive(false);
					_wedgeB.SetActive(false);
					_hasBeenDestroyed = true;
				}
			}
		}
	}
}
