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
	public class CombatHud : MonoBehaviour 
	{
		public Canvas _canvas;
		
		public GameObject _unitDisplayPrefab;
		public GameObject _healthIconPrefab;

		private RectTransform _baseRectransform;
		private UnitManager _unitManager;
		private Dictionary<Unit, MapUnitDisplay> _unitToDisplayMap;
		private List<HealthIcon> _healthIcons;

		protected void Awake ()
		{
			_unitManager = FindObjectOfType<UnitManager>();
			_unitToDisplayMap = new Dictionary<Unit, MapUnitDisplay>();
			_unitDisplayPrefab.SetActive(false);
			_baseRectransform = _canvas.GetComponent<RectTransform>();
			_healthIcons = new List<HealthIcon>();
		}

		protected void Start ()
		{
			foreach (Building building in FindObjectsOfType<Building>())
			{
				var icon = Instantiate(_healthIconPrefab, _baseRectransform).GetComponent<HealthIcon>();
				icon.Initialize(building);
				_healthIcons.Add(icon);
			}
		}

		// protected void Update ()
		// {
		// 	foreach (Unit unit in _unitManager.Units)
		// 	{
		// 		if (_unitToDisplayMap.ContainsKey(unit) == false)
		// 		{
		// 			_unitDisplayPrefab.SetActive(true);
		// 			MapUnitDisplay display = Instantiate(_unitDisplayPrefab, _baseRectransform).GetComponent<MapUnitDisplay>();
		// 			display.Initialize(unit);
		// 			_unitToDisplayMap.Add(unit, display);
		// 			_unitDisplayPrefab.SetActive(false);
		// 		}
		// 	}
		// }
	}
}
