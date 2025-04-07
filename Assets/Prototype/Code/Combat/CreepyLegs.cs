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
	public class CreepyLegs : MonoBehaviour 
	{
		private class Leg
		{
			int _stage;
			float timer;
			float duration = 0.1f;
			Transform _transform;
			Quaternion[] _rotations;
			public Leg (Transform transform, float x, float z, int stage)
			{
				_transform = transform;
				_rotations = new Quaternion[3];
				_rotations[0] = Quaternion.identity;
				_rotations[1] = Quaternion.Euler(x, 0, 0);
				_rotations[2] = Quaternion.Euler(x * 0.5f, 0, z);
				_stage = stage % _rotations.Length;
			}
			public void Process ()
			{
				int next = (_stage + 1) % _rotations.Length;
				timer += Time.deltaTime;
				float t = Mathf.Clamp01(timer / duration);
				_transform.localRotation = Quaternion.Slerp(_rotations[_stage], _rotations[next], t);
				if (t >= 1) 
				{
					_stage = (_stage + 1) % _rotations.Length;
					timer = 0;
				}
			}
			public void Reset ()
			{
				_transform.localRotation = _rotations[0];
			}
		}
		public Transform[] _leftLegs;
		public Transform[] _rightLegs;

		private List<Leg> _legs;
		float maxAngleX = 20;
		float maxAngleZ = 30;

		protected void Awake ()
		{
			_legs = new List<Leg>();
			for (int i = 0; i < _leftLegs.Length; i++)
			{
				_legs.Add(new Leg(_leftLegs[i], maxAngleX, -maxAngleZ, i));
			}
			for (int i = 0; i < _rightLegs.Length; i++)
			{
				_legs.Add(new Leg(_rightLegs[i], maxAngleX, maxAngleZ, i));
			}			
		}

		public void Process ()
		{
			foreach (Leg leg in _legs)
			{
				leg.Process();
			}
		}

		public void Reset ()
		{
			foreach (Leg leg in _legs)
			{
				leg.Reset();
			}
		}
	}
}
