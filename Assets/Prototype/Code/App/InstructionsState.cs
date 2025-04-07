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
	public class InstructionsState : AppState 
	{
		public const string Name = "Instructions";
		public override string StateName => Name;

		public void ClickStart ()
		{
			_app.GoToStateOnLateUpdate(CombatState.Name);
		}

		protected override void OnAfterEnter()
		{
			
		}

		protected override void OnProcess()
		{
			
		}

		protected override void OnBeforeExit()
		{
			
		}		
	}
}
