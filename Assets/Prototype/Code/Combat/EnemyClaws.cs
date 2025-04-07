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
	public class EnemyClaws : MonoBehaviour 
	{
		private class Claw
		{
			Transform _t;
			Quaternion[] _rotations;
			public Claw (Transform transform, float x, float y)
			{
				_t = transform;
				_rotations = new Quaternion[2];
				_rotations[0] = Quaternion.identity;
				_rotations[1] = Quaternion.Euler(x, y, 0);
			}
			public void Process (float t)
			{
				_t.localRotation = Quaternion.Slerp(_rotations[0], _rotations[1], t);
			}
			public void Reset ()
			{
				_t.localRotation = Quaternion.identity;
			}
		}

		public Transform _rightClaw;
		public Transform _leftClaw;
		private Claw[] _claws;
		protected void Awake ()
		{
			_claws = new Claw[]
			{
				new Claw(_rightClaw, -19, 30),
				new Claw(_leftClaw, -19, -30)
			};
		}
		public void Process (float t)
		{
			foreach (Claw claw in _claws)
			{
				claw.Process(t);
			}
		}
		public void Reset ()
		{
			foreach (Claw claw in _claws)
			{
				claw.Reset();
			}
		}
	}
}
