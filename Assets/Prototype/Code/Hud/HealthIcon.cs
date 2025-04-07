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
	public class HealthIcon : MonoBehaviour 
	{
		public Image[] _rings;
		public Image _danger;
		public TMP_Text _dangerIcon;
		public Color _dangerColorA;
		public Color _dangerColorB;

		private Building _building;
		private Camera _camera;
		private RectTransform _canvasTransform;

		public void Initialize (Building building)
		{
			_building = building;
			_camera = Camera.main;
			_canvasTransform = GetComponentInParent<Canvas>().transform as RectTransform;
			_dangerIcon.enabled = false;
			_danger.enabled = false;
		}

		protected void Update ()
		{
			float hp = _building.Hitpoints;
			float max = _building.MaxHitpoints;
			float t = Mathf.Clamp01(hp / max);

			if (t <= 0)
			{
				gameObject.SetActive(false);
				return;
			}

			if (t >= 1)
			{
				foreach (Image ring in _rings)
				{
					ring.enabled = false;
				}
				_danger.enabled = false;
				_dangerIcon.enabled = false;
				return;
			}

			foreach (Image ring in _rings)
			{
				ring.fillAmount = t;
				ring.enabled = true;
			}

			if (t <= 0.4f)
			{
				_danger.enabled = true;
				float colorLerp = (Mathf.Sin(Time.time * 4) + 1) / 2;
				_danger.color = Color.Lerp(_dangerColorA, _dangerColorB, colorLerp);
				_dangerIcon.enabled = true;
			}
			else
			{
				_danger.enabled = false;
				_dangerIcon.enabled = false;
			}

			//Convert the world for screen point so that it can be used with ScreenPointToLocalPointInRectangle function
			Vector3 screenPos = _camera.WorldToScreenPoint(_building.HealthIconWorldAnchor);

			//Convert the screenpoint to ui rectangle local point
			// pass null for the camera paramter since this canvas is an overlay
			RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasTransform, screenPos, null, out Vector2 iconPos);

			transform.localPosition = iconPos;			
		}
	}
}
