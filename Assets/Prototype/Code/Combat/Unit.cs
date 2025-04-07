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
	public enum UnitState
	{
		None, Idle, Move, Fix, Gun, Research
	}

	public class Unit : MonoBehaviour 
	{
		public UnitState State => _state;

		private bool _getPathToTargetNode;
		private Node _currentNode;
		private Node _targetNode;
		protected UnitState _state;
		private NodeMap _nodeMap;
		private List<Node> _pathToTarget;
		private int _pathIndex;
		private float _moveTimer;
		private float _moveDuration;

		public void Initialize (Building building, NodeMap nodeMap)
		{
			_nodeMap = nodeMap;
			_currentNode = _nodeMap.GetNode(building);
			_targetNode = _currentNode;
			transform.position = _currentNode.Position;
		}

		public void MoveTo (Building building)
		{
			if (building == _currentNode.Building) return;
			_targetNode = _nodeMap.GetNode(building);
			_getPathToTargetNode = true;
		}

		protected void Awake ()
		{
			_state = UnitState.Idle;
			_pathToTarget = new List<Node>();
		}

		protected void Update ()
		{
			if (Input.GetKeyDown(KeyCode.Return))
			{
				_targetNode = _nodeMap.GetRandomNodeThatIsNotNode(_currentNode);
				_getPathToTargetNode = true;
			}

			if (_getPathToTargetNode)
			{
				if (NodeMap.TryGetPath(_currentNode, _targetNode, NodeMap.CanEnterNode, out _pathToTarget))
				{
					_state = UnitState.Move;
					_pathIndex = 0;
					_moveTimer = 0;
					_moveDuration = Vector3.Distance(_currentNode.Position, _pathToTarget[0].Position) / Constants.UnitMoveSpeed;
				}
				_getPathToTargetNode = false;
			}

			if (_state == UnitState.Move)
			{
				_moveTimer += Time.deltaTime;
				float t = Mathf.Clamp01(_moveTimer / _moveDuration);
				transform.position = Vector3.Lerp(_currentNode.Position, _pathToTarget[_pathIndex].Position, t);
				if (t >= 1)
				{
					_currentNode = _pathToTarget[_pathIndex];
					_pathIndex++;
					if (_pathIndex >= _pathToTarget.Count)
					{
						_state = UnitState.Idle;
					}
					else
					{
						_moveTimer = 0;
						_moveDuration = Vector3.Distance(_currentNode.Position, _pathToTarget[_pathIndex].Position) / Constants.UnitMoveSpeed;
					}
				}
			}
		}
	}
}
