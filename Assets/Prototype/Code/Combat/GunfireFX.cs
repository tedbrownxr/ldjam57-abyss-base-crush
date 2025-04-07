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
	public class GunfireFX : MonoBehaviour, IFX 
	{
		public float MaxSphereSize = 1;
		public float MaxLightIntensity = 1;
		public float MaxLightRange = 10;
		public Transform _sphere;
		public Light _light;
		float _timer;
		float _duration = 0.2f;
	
		public bool IsActive ()
		{
			return _timer < _duration;
		}

		public void Activate (Vector3 position)
		{
			_timer = 0;
			transform.position = position;
			_sphere.localScale = Vector3.one * MaxSphereSize;
			_light.intensity = MaxLightIntensity;
			_light.range = MaxLightRange;
			gameObject.SetActive(true);
		}

		public void Process ()
		{
			if (IsActive())
			{
				_timer += Time.deltaTime;
				float t = Mathf.Clamp01(_timer / _duration);
				_sphere.localScale = Vector3.one * MaxSphereSize * (1-t);
				_light.intensity = MaxLightIntensity * (1-t);
				_light.range = MaxLightRange * (1-t);
				if (_timer >= _duration)
				{
					gameObject.SetActive(false);
				}
			}
		}
	}
}
