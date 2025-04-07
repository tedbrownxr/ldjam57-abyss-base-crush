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
	public class DebugSpawner : MonoBehaviour 
	{
		public Pointer _pointer;

		protected EnemyManager _enemyManager;

		protected void Awake ()
		{
			_enemyManager = FindObjectOfType<EnemyManager>();
		}

		protected void Update ()
		{
			if (_pointer.HasPointerTarget && _pointer.Target.GetComponent<Ground>())
			{
				if (Input.GetMouseButtonDown(0))
				{
					_enemyManager.Spawn(_pointer.HitPoint, 0);
				}
			}
		}
	}
}
