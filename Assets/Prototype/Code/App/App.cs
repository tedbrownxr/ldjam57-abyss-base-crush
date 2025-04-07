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
	public class App : MonoBehaviour 
	{
		public AppState _initialState;

		protected AppState _activeState;
		protected Dictionary<string, AppState> _states;
		protected string _nextStateName;

		public void GoToStateOnLateUpdate (string name)
		{
			_nextStateName = name;
		}

		public void GoToStateImmediately (string name)
		{
			if (_activeState != null)
			{
				_activeState.Exit();
			}
			_states.TryGetValue(name, out _activeState);
			if (_activeState != null)
			{
				_activeState.Enter();
			}
			else
			{
				Debug.LogError("Missing state [" + name + "]");
			}
			_nextStateName = string.Empty;
		}

		protected void Awake ()
		{
			_states = new Dictionary<string, AppState>();
			foreach (AppState state in GetComponentsInChildren<AppState>())
			{
				_states.Add(state.StateName, state);
				state.Initialize(this);
			}
		}

		protected void Start ()
		{
			GoToStateImmediately(_initialState.StateName);
		}

		protected void Update ()
		{
			if (_activeState)
			{
				_activeState.Process();
			}
		}

		protected void LateUpdate ()
		{
			if (string.IsNullOrEmpty(_nextStateName) == false)
			{
				GoToStateImmediately(_nextStateName);
			}
		}
	}
}
