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
	public class TitleScreen : MonoBehaviour 
	{
		public static TitleScreen Instance;

		public GameObject _container;

		public void Hide ()
		{
			_container.SetActive(false);
		}

		public void Show ()
		{
			_container.SetActive(true);
		}

		protected void Awake ()
		{
			Instance = this;
			_container.SetActive(false);
		}
	}
}
