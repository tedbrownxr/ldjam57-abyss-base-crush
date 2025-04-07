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
	public class DebugHud : MonoBehaviour 
	{
		public Pointer _pointer;
		private List<DebugHudText> _hudText;

		public void Clear (ContentAlignment align)
		{
			foreach (DebugHudText hudText in _hudText)
			{
				if (hudText.ContentAlignment == align)
				{
					hudText.Clear();
				}
			}
		}

		public void Set (ContentAlignment align, string value)
		{
			foreach (DebugHudText hudText in _hudText)
			{
				if (hudText.ContentAlignment == align)
				{
					hudText.Set(value);
				}
			}
		}

		protected void Awake ()
		{
			_hudText = new List<DebugHudText>();
			_hudText.AddRange(GetComponentsInChildren<DebugHudText>());
		}

		protected void Update ()
		{
			if (_pointer.HasPointerTarget)
			{
				Set(ContentAlignment.LowerLeft, _pointer.Target.name);
			}
			else
			{
				Clear(ContentAlignment.LowerLeft);
			}
		}
	}
}
