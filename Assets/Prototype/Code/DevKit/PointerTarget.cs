// DevKit
// Copyright (c) 2024 Ted Brown

using System;
using System.Collections.Generic;
using UnityEngine;

namespace DevKit
{
	public class PointerTarget : MonoBehaviour
	{
		public Action<Pointer> OnTarget;
		public Action<Pointer> OnUntarget;

		public bool IsActive => _isActive;
		public bool IsHovered => _hoveringPointers.Count > 0;
		public Vector3 HitNormal => _hitNormal;
		public Vector3 HitPoint => _hitPoint;

		[SerializeField] private bool _activeByDefault = true;

		private bool _isActive;
		private HashSet<Pointer> _hoveringPointers;
		private Vector3 _hitNormal;
		private Vector3 _hitPoint;

		public void Activate ()
		{
			_isActive = true;
			foreach (Pointer pointer in _hoveringPointers)
			{
				OnTarget?.Invoke(pointer);
			}
		}

		public void Deactivate ()
		{
			_isActive = false;
			foreach (Pointer pointer in _hoveringPointers)
			{
				OnUntarget?.Invoke(pointer);
			}
		}

		// The object always knows if it's hovered or not.
		// Active state simply determines if it broadcasts this or not.
		public void Hover (Pointer pointer)
		{
			_hoveringPointers.Add(pointer);
			if (_isActive)
			{
				OnTarget?.Invoke(pointer);
			}
		}

		public void SetPointerHitPoint (Vector3 hitPoint, Vector3 hitNormal)
		{
			_hitNormal = hitNormal;
			_hitPoint = hitPoint;
		}

		// The object always knows if it's hovered or not.
		// Active state simply determines if it broadcasts this or not.
		public void Unhover (Pointer pointer)
		{
			if (_hoveringPointers.Contains(pointer))
			{
				_hoveringPointers.Remove(pointer);
			}
			if (_isActive)
			{
				OnUntarget?.Invoke(pointer);
			}
		}

		protected void Awake ()
		{
			_hoveringPointers = new HashSet<Pointer>();
			_isActive = _activeByDefault;
		}
	}
}
