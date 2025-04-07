using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DevKit
{
	/// <summary>Uses the transform of an object to determine its ray</summary>
	public class TransformPointer : Pointer 
	{
		protected override Ray GetRay()
		{
			if (Time.frameCount == _lastUpdatedFrame)
			{
				return _ray;
			}
			_lastUpdatedFrame = Time.frameCount;
			_ray = new Ray(transform.position, transform.forward);
			return _ray;
		}
	}
}
