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
	public class TitleState : AppState 
	{
		public const string Name = "Title";
		public override string StateName => Name;

		Vector3 _maxTitleScale = Vector3.one * 31;
		Vector3 _splashScaleEnd = new Vector3(-1, 1, 1);
		Vector3 _splashScaleStart = new Vector3(-10, 20, 1);
		float _splashRotationEnd = -108.2f;
		float _splashRotateStart = 360;
		public Transform _splashTransform;
		public GameObject _startButtonObject;
		public Transform _titleTransform;
		bool _isAnimating;
		float _timer;
		float _duration = 4;

		public void ClickStart ()
		{
			_app.GoToStateOnLateUpdate(InstructionsState.Name);
		}

		protected override void OnAfterEnter()
		{
			_timer = 0;
			_isAnimating = true;
			_startButtonObject.SetActive(false);
		}

		protected override void OnProcess()
		{
			if (_isAnimating)
			{
				_timer += Time.deltaTime;
				if (Input.GetMouseButtonDown(0))
				{
					_timer = _duration;
				}
				if (_timer >= _duration)
				{
					_isAnimating = false;
					_startButtonObject.SetActive(true);
				}
				float t = Mathf.Clamp01(_timer / _duration);
				_titleTransform.localScale = Vector3.Lerp(_maxTitleScale, Vector3.one, t);
				_splashTransform.localScale = Vector3.Lerp(_splashScaleStart, _splashScaleEnd, t);
				_splashTransform.localRotation = Quaternion.Slerp(Quaternion.Euler(Vector3.forward * _splashRotateStart), Quaternion.Euler(Vector3.forward * _splashRotationEnd), t);
			}
		}

		protected override void OnBeforeExit()
		{
			
		}		
	}
}
