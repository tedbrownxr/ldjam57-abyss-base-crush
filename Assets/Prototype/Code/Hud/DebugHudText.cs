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
	public class DebugHudText : MonoBehaviour 
	{
		public ContentAlignment ContentAlignment;

		private TMP_Text _label;

		public void Clear ()
		{
			_label.text = string.Empty;
		}

		public void Set (string text)
		{
			_label.text = text;
		}

		protected void Awake ()
		{
			_label = GetComponentInChildren<TMP_Text>();
			Clear();
		}
	}
}
