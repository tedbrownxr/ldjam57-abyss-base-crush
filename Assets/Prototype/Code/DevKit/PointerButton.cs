// Zephyr
// Copyright (c) 2024 Ted Brown

using System;
using UnityEngine;

namespace DevKit
{
	public class PointerButton : MonoBehaviour 
	{
		public Action OnClick;
		public Action OnHover;
		public Action OnUnhover;

		[SerializeField] private PointerTarget _pointerTarget;

		public static float TapTime = 0.2f;
		public static float HoldTime = 0.3f;
		private static GameObject _lockInteractionsToObject;
		
		public float ActiveTime => _state == ButtonState.Pressed ? Time.time - _startTime : 0;
		public ButtonEventType Event => _event;
		public ButtonState State => _state;


		protected float _startTime;
		protected ButtonEventType _event;
		protected ButtonState _state;

		// if this is locked to a gameobject, 
		// events will only be broadcast to objects that have this object as a parent in their hierarchy.
		public static void LockToObject (GameObject targetObject)
		{
			_lockInteractionsToObject = targetObject;
		}

		public static void Unlock ()
		{
			_lockInteractionsToObject = null;
		}

		public void Activate ()
		{
			_state = ButtonState.Idle;
			_pointerTarget.Activate();
		}

		public void Deactivate ()
		{
			_pointerTarget.Deactivate();
			_state = ButtonState.Deactivated;
		}

		public virtual bool GetPrimaryButton ()
		{
			return Input.GetMouseButton(0);
		}

		protected void Awake ()
		{
			_state = ButtonState.Hovered;
		}

		protected void Update ()
		{
			_event = ButtonEventType.None;

			switch (_state)
			{
				case ButtonState.None:
					_state = ButtonState.Idle;
					break;

				case ButtonState.Idle:
					if (_pointerTarget.IsHovered)
					{
						_state = ButtonState.Hovered;
						OnHover?.Invoke();
					}
					break;

				case ButtonState.Hovered:
					if (_pointerTarget.IsHovered)
					{
						if (GetPrimaryButton())
						{
							_event = ButtonEventType.Start;
							_state = ButtonState.RecentlyPressed;
							_startTime = Time.time;
						}
					}
					else
					{
						_state = ButtonState.Idle;
						OnUnhover?.Invoke();
					}
					break;

				case ButtonState.RecentlyPressed:
					if (_pointerTarget.IsHovered)
					{
						if (GetPrimaryButton())
						{
							if (Time.time - _startTime > HoldTime)
							{
								_event = ButtonEventType.Hold;
								_state = ButtonState.Pressed;
							}
						}
						else
						{
							if (Time.time - _startTime < TapTime)
							{
								_event = ButtonEventType.Tap;
							}
							else
							{
								_event = ButtonEventType.Release;
							}
							_state = ButtonState.Hovered;
						}
					}
					else
					{
						_state = ButtonState.Idle;
						OnUnhover?.Invoke();
					}
					break;

				case ButtonState.Pressed:
					if (_pointerTarget.IsHovered)
					{
						if (GetPrimaryButton() == false)
						{
							_event = ButtonEventType.Release;
							_state = ButtonState.Hovered;
						}
					}
					else
					{
						_event = ButtonEventType.Release;
						_state = ButtonState.Idle;
						OnUnhover?.Invoke();
					}
					break;
			}
			
			if (_event == ButtonEventType.Tap)
			{
				if (_lockInteractionsToObject != null)
				{
					bool isAcceptedInteraction = false;
					Transform parent = transform;
					foreach (Transform t in parent)
					{
						// reached root
						if (t == t.parent) break;
						if (t.gameObject == _lockInteractionsToObject)
						{
							isAcceptedInteraction = true;
							break;
						}
					}
					if (isAcceptedInteraction == false)
					{
						return;
					}
				}
				OnClick?.Invoke();
			}
		}
	}
}
