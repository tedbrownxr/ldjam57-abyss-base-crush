using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DevKit
{
	[RequireComponent(typeof(EventTrigger))]
	public class CanvasPointerCheck : MonoBehaviour
	{
		public static bool IsOverCanvas;

		private void HandleHoverCanvas()
		{
			IsOverCanvas = true;
			Debug.Log("over canvas");
		}
		private void HandleUnhoverCanvas()
		{
			IsOverCanvas = false;
			Debug.Log("off canvas");
		}

		protected void Start()
		{
			EventTrigger eventTrigger = GetComponent<EventTrigger>();

			if (eventTrigger != null)
			{
				// Pointer Enter
				EventTrigger.Entry enterUIEntry = new EventTrigger.Entry();
				enterUIEntry.eventID = EventTriggerType.PointerEnter;
				enterUIEntry.callback.AddListener((eventData) => { HandleHoverCanvas(); });
				eventTrigger.triggers.Add(enterUIEntry);

				//Pointer Exit
				EventTrigger.Entry exitUIEntry = new EventTrigger.Entry();
				exitUIEntry.eventID = EventTriggerType.PointerExit;
				exitUIEntry.callback.AddListener((eventData) => { HandleUnhoverCanvas(); });
				eventTrigger.triggers.Add(exitUIEntry);
			}
		}
	}
}
