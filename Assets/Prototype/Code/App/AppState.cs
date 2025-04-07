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
	public abstract class AppState : MonoBehaviour 
	{
		public abstract string StateName { get; }

		protected App _app;

		public void Initialize (App app)
		{
			_app = app;
			gameObject.SetActive(false);
		}

		public void Enter ()
		{
			gameObject.SetActive(true);
			OnAfterEnter();
		}

		public void Process ()
		{
			OnProcess();
		}
		public void Exit ()
		{
			OnBeforeExit();
			gameObject.SetActive(false);
		}

		protected virtual void OnAfterEnter () {}
		protected virtual void OnProcess () {}
		protected virtual void OnBeforeExit () {}
	}
}
