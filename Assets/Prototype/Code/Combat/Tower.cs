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
	public class Tower : Building 
	{
		public const float Radius = 1.5f;

		private BuildingVisual _visual;

		public override void SetAttackerPositions (float attackerRadius)
		{
			List<AttackerPosition> attackerPositions = new List<AttackerPosition>();
			Vector3 fwd = transform.forward;
			float angle = 0;
			// radians = rought estimation of arc length / radius
			float angleIncrease = ((attackerRadius * 2f) / (Radius + attackerRadius)) * Mathf.Rad2Deg;

			while (angle < 90)
			{
				{
					Vector3 placementDirection = Quaternion.Euler(Vector3.up * angle) * fwd;
					Vector3 position = transform.position + placementDirection * (Radius + attackerRadius);
					Vector3 attackDirection = (transform.position - position).normalized;
					attackerPositions.Add(new AttackerPosition(this, position, attackerRadius, attackDirection));
				}

				if (angle != 0)
				{
					Vector3 placementDirection = Quaternion.Euler(Vector3.up * -angle) * fwd;
					Vector3 position = transform.position + placementDirection * (Radius + attackerRadius);
					Vector3 attackDirection = (transform.position - position).normalized;
					attackerPositions.Add(new AttackerPosition(this, position, attackerRadius, attackDirection));
				}
				
				angle += angleIncrease;
			}

			_attackerPositions = attackerPositions.ToArray();
		}

		protected override void HandleTarget(Pointer pointer)
		{
			base.HandleTarget(pointer);
			_visual.SetTargeted(true);
		}

		protected override void HandleUntarget(Pointer pointer)
		{
			base.HandleUntarget(pointer);
			_visual.SetTargeted(false);
		}

		protected override void Awake ()
		{
			base.Awake();
			_visual = GetComponent<BuildingVisual>();
		}

	}
}
