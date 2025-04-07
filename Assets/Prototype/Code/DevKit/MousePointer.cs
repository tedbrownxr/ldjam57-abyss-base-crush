using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DevKit
{
	/// <summary>Uses the mouse position to determine its ray</summary>
	public class MousePointer : Pointer 
	{
		private Camera _camera;

		protected override Ray GetRay ()
		{
			return _camera.ScreenPointToRay(Input.mousePosition);
		}

		protected override void Awake ()
		{
			base.Awake();
			_camera = Camera.main;
		}
	}
}
