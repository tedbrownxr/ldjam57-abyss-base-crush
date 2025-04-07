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
	public class MapUnitDisplay : MonoBehaviour 
	{
		public Image _background;
		public Image _ringThin;
		public Image _ringThick;
		public Image _iconMove;
		public Image _iconIdle;
		public Image _iconFix;
		public Image _iconGun;
		public Image _iconResearch;
		public Color _colorIdle = Color.black;
		public Color _colorMove = Color.cyan;
		public Color _colorFix = Color.yellow;
		public Color _colorGun = Color.red;
		public Color _colorResearch = Color.green;

		private Camera _camera;
		protected RectTransform _canvasTransform;
		protected UnitState _currentState;
		protected Unit _unit;

		public void Initialize (Unit unit)
		{
			_unit = unit;
			_canvasTransform = GetComponentInParent<Canvas>().transform as RectTransform;
		}

		public void SetState (UnitState state)
		{
			switch (state)
			{
				case UnitState.Idle:
					_background.color = _colorIdle;
					break;
				case UnitState.Move:
					_background.color = _colorMove;
					break;
				case UnitState.Fix:
					_background.color = _colorFix;
				 	break;
				case UnitState.Gun:
					_background.color = _colorGun;
				 	break;
				case UnitState.Research:
					_background.color = _colorResearch;
					break;
			}
			_iconFix.gameObject.SetActive(state == UnitState.Fix);
			_iconMove.gameObject.SetActive(state == UnitState.Move);
			_iconIdle.gameObject.SetActive(state == UnitState.Idle);
			_iconGun.gameObject.SetActive(state == UnitState.Gun);
			_iconResearch.gameObject.SetActive(state == UnitState.Research);
			_currentState = state;
		}

		protected void Awake ()
		{
			_camera = Camera.main;
		}

		protected void Update ()
		{
			if (_currentState != _unit.State)
			{
				SetState(_unit.State);
			}

			//Convert the world for screen point so that it can be used with ScreenPointToLocalPointInRectangle function
			Vector3 screenPos = _camera.WorldToScreenPoint(_unit.transform.position);

			//Convert the screenpoint to ui rectangle local point
			// pass null for the camera paramter since this canvas is an overlay
			RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasTransform, screenPos, null, out Vector2 iconPos);

			transform.localPosition = iconPos;
		}
	}
}
