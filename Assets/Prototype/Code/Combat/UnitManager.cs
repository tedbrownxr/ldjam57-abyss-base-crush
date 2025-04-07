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
	public class UnitManager : MonoBehaviour 
	{
		public GameObject _unitPrefab;
		public Building _hub;

		public List<Unit> Units;

		private NodeMap _nodeMap;

		protected void Awake ()
		{
			Units = new List<Unit>();
			_nodeMap = FindObjectOfType<NodeMap>();
		}

		protected void Start ()
		{
			Unit unit = Instantiate(_unitPrefab).GetComponent<Unit>();
			unit.Initialize(_hub, _nodeMap);
			Units.Add(unit);
		}
	}
}
