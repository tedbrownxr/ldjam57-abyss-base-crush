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
	public class Tunnel : Building 
	{
		public const float HalfWidth = 0.5f;
		public const float Length = 10;

		public Action OnAnchorADestroyed;
		public Action OnAnchorBDestroyed;

		public bool IsInnerTunnel;
		public GameObject _objectsA;
		public GameObject _objectsMiddle;
		public GameObject _objectsB;

		private Building _attachedBuildingA;
		private Building _attachedBuildingB;
		private TunnelVisual _visual;

		public void SetBuildingAnchors (Building a, Building b)
		{
			_attachedBuildingA = a;
			_attachedBuildingB = b;
			a.OnDestroy += HandleBuildingDestroyed;
			b.OnDestroy += HandleBuildingDestroyed;
		}

		public override void SetAttackerPositions (float attackerRadius)
		{
			List<AttackerPosition> attackerPositions = new List<AttackerPosition>();
			float distance = attackerRadius + 0.3f;
			Vector3 middlePosition = transform.position + transform.forward * (HalfWidth + distance);

			int spots = (int)Length / (int)(attackerRadius*2);
			spots -= 4; // drop one from each end
			for (int i = 0; i < spots / 2; i++)
			{
				attackerPositions.Add(new AttackerPosition(this, middlePosition + transform.right * attackerRadius * 2 * i, distance, -transform.forward));
				if (i > 0)
				{
					attackerPositions.Add(new AttackerPosition(this, middlePosition - transform.right * attackerRadius * 2 * i, distance, -transform.forward));
				}
			}

			// add some on the other side for inner tunnel
			if (IsInnerTunnel)
			{
				middlePosition = transform.position - transform.forward * (HalfWidth + attackerRadius);
				for (int i = 0; i < spots / 2; i++)
				{
					attackerPositions.Add(new AttackerPosition(this, middlePosition + transform.right * attackerRadius * 2 * i, distance, transform.forward));
					if (i > 0)
					{
						attackerPositions.Add(new AttackerPosition(this, middlePosition - transform.right * attackerRadius * 2 * i, distance, transform.forward));
					}
				}
			}

			_attackerPositions = attackerPositions.ToArray();
		}

		private void HandleBuildingDestroyed (Building building)
		{
			if (building == _attachedBuildingA)
			{
				OnAnchorADestroyed?.Invoke();
				_objectsA.SetActive(false);
			}
			
			if (building == _attachedBuildingB)
			{
				OnAnchorBDestroyed?.Invoke();
				_objectsB.SetActive(false);
			}

			if (_attachedBuildingA.Hitpoints <= 0 && _attachedBuildingB.Hitpoints <= 0)
			{
				TakeDamage(_hitpoints);
			}
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
			_visual = GetComponent<TunnelVisual>();
		}		
	}
}
