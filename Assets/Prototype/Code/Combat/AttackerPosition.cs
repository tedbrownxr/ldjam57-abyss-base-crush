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
	[Serializable]
	public class AttackerPosition 
	{
		public bool IsClaimed => Enemy != null;
		public Enemy Enemy;
		public Vector3 Position;
		public Building Building;
		public float Radius;
		public Quaternion Rotation;

		public AttackerPosition (Building building, Vector3 position, float radius, Vector3 attackDirection)
		{
			Building = building;
			Position = position;
			Radius = radius;
			Rotation = Quaternion.LookRotation(attackDirection, Vector3.up);
		}

		public void Claim (Enemy enemy)
		{
			Enemy = enemy;
		}

		public void ReleaseClaim (Enemy enemy)
		{
			if (enemy != null && Enemy == enemy)
			{
				Enemy = null;
			}
		}
	}
}
