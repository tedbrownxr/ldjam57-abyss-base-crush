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
	public class TestTriggerPosition : MonoBehaviour 
	{
		public AwarenessTrigger trigger;

		protected void Update ()
		{
			Debug.Log(trigger.GetDistance(transform.position));
		}
	}
}
