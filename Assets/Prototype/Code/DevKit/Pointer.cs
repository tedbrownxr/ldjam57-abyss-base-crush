using System.Collections.Generic;
using UnityEngine;

namespace DevKit
{
	// NOTE: To get BlockedByCanvas to work, add CanvasPointerCheck to all Canvas elements in your scene.
	public abstract class Pointer : MonoBehaviour 
	{
		public bool IsHittingCollider => _hitCollider != null;
		public bool HasPointerTarget => _pointerTarget != null;
		public bool IsEnabled => enabled;
		public PointerTarget Target => _pointerTarget;
		public Quaternion Rotation => Quaternion.LookRotation(GetRay().direction, Vector3.up);
		public Vector3 HitNormal => _hitNormal;
		public Vector3 HitPoint => _hitPoint;
		public Vector3 Origin => GetRay().origin;

		[SerializeField] protected LayerMask _layerMask = -1;

		protected Collider _hitCollider;
		protected int _lastUpdatedFrame;
		protected LayerMask _defaultLayerMask;
		protected List<LayerMask> _layerMaskHistory;
		protected PointerTarget _pointerTarget;
		protected Ray _ray;
		protected Vector3 _hitNormal;
		protected Vector3 _hitPoint;

		protected abstract Ray GetRay ();

		public void DisableAndUnhoverTarget ()
		{
			if (_pointerTarget != null)
			{
				_pointerTarget.Unhover(this);
			}
			_pointerTarget = null;
			enabled = false;
		}

		public void Enable ()
		{
			enabled = true;
		}

		public void PopLayerMask ()
		{
			int count = _layerMaskHistory.Count;
			if (count > 0)
			{
				_layerMaskHistory.Remove(count - 1);
			}
			count--;
			if (count > 0)
			{
				_layerMask = _layerMaskHistory[count - 1];
			}
			else
			{
				_layerMask = _defaultLayerMask;
			}
		}

		public void PushLayerMask (LayerMask layerMask)
		{
			_layerMaskHistory.Add(_layerMask);
			_layerMask = layerMask;
		}

		protected virtual PointerTarget GetPointerTarget ()
		{
			Ray ray = GetRay();
			_hitCollider = null;
			if (Physics.Raycast(ray, out RaycastHit hit, 1000, _layerMask, QueryTriggerInteraction.Ignore))
			{
				_hitCollider = hit.collider;
				_hitPoint = hit.point;
				_hitNormal = hit.normal;
				PointerTarget target = hit.collider.gameObject.GetComponentInParent<PointerTarget>();
				if (target != null && target.IsActive)
				{
					return target;
				}
			}
			return null; 
		}

		protected virtual void Awake ()
		{
			_defaultLayerMask = _layerMask;
			_layerMaskHistory = new List<LayerMask>();
		}

		protected virtual void Update ()
		{
			PointerTarget previousTarget = _pointerTarget;
			PointerTarget currentTarget = GetPointerTarget();

			// if the current target is the same as the previous target, only do something if the target was disabled
			if (currentTarget == previousTarget)
			{
				if (currentTarget != null)
				{
					if (currentTarget.IsActive == false)
					{
						currentTarget.Unhover(this);
						currentTarget = null;
					}
				}
			}
			// If we have a new target, unhover the old and hover the new
			else if (currentTarget != previousTarget)
			{
				if (previousTarget != null)
				{
					previousTarget.Unhover(this);
				}
				// Here we assume GetPointerTarget checks if the target is enabled or not
				// before it returns one. Otherwise, it should return a null value.
				if (currentTarget != null)
				{
					currentTarget.Hover(this);
				}
			}

			if (currentTarget != null)
			{
				currentTarget.SetPointerHitPoint(_hitPoint, _hitNormal);
			}
			_pointerTarget = currentTarget;
		}
	}
}
