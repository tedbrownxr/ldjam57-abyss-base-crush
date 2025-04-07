using DevKit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Prototype
{
	public class MainBase : MonoBehaviour 
	{
		public static MainBase Instance;

		public bool HasDamagedBuildingsThatCanBeRepaired
		{
			get
			{
				foreach (Building b in FindObjectsOfType<Building>())
				{
					if (b.IsDamaged == true && b.IsDestroyed == false)
					{
						return true;
					}
				}				
				return false;
			}
		}

		public Building _hub;
		public GameObject _towerPrefab;
		public GameObject _tunnelPrefab;
		public Transform _towerContainer;
		public Transform _outerTunnelContainer;
		public Transform _innerTunnelContainer;

		private static Dictionary<Building, float> s_buildingToDistanceMap;

		private Building[] _towers;
		private Building[] _outerTunnels;
		private Building[] _innerTunnels;
		private NodeMap _nodeMap;

		// called by button element
		public void RepairAllBuildings ()
		{
			foreach (Building b in FindObjectsOfType<Building>())
			{
				if (b.IsDamaged == true && b.IsDestroyed == false)
				{
					b.RestoreHealth();
				}
			}
			FindObjectOfType<UpgradeState>().RequestChangeState();
		}

		// TODO: update to allow access to hub / inner tunnels
		public IEnumerable<KeyValuePair<Building, float>> GetBuildingsSortedByDistance (Vector3 position)
		{
			if (s_buildingToDistanceMap != null) s_buildingToDistanceMap.Clear();
			else s_buildingToDistanceMap = new Dictionary<Building, float>();

			// Hub
			{
				float d = (position - _hub.transform.position).magnitude;
				s_buildingToDistanceMap.Add(_hub, d);
			}
			foreach (Building b in _towers)
			{
				float d = (position - b.transform.position).magnitude;
				s_buildingToDistanceMap.Add(b, d);
			}
			foreach (Building b in _outerTunnels)
			{
				float d = (position - b.transform.position).magnitude;
				s_buildingToDistanceMap.Add(b, d);
			}
			foreach (Building b in _innerTunnels)
			{
				float d = (position - b.transform.position).magnitude;
				s_buildingToDistanceMap.Add(b, d);
			}		
			return s_buildingToDistanceMap.OrderBy(x => x.Value);
		}

		public Building GetNearestTower (Vector3 position)
		{
			Building nearest = null;
			float bestDistance = float.MaxValue;
			position.y = 0;
			foreach (Building tower in _towers)
			{
				if (tower.Hitpoints > 0)
				{
					Vector3 p2 = tower.transform.position;
					p2.y = 0;
					float d = Vector3.Distance(position, p2);
					if (d < bestDistance)
					{
						bestDistance = d;
						nearest = tower;
					}
				}
			}
			return nearest;
		}

		protected void Awake ()
		{
			Instance = this;

			float radius = 7;
			Hex[] towers = Hex.GetNeighborValues(Hex.zero);
			_towers = new Building[6];
			_outerTunnels = new Building[6];
			_innerTunnels = new Building[6];
			for (int i = 0; i < towers.Length; i++)
			{
				int nextBuildingIndex = (i + 1) % 6;
				Vector3 towerPosition = towers[i].GetPosition(radius);
				Vector3 nextTowerPosition = towers[nextBuildingIndex].GetPosition(radius);

				_towers[i] = Instantiate(_towerPrefab, _towerContainer).GetComponent<Building>();
				_towers[i].transform.position = towerPosition;
				Vector3 towerDirection = towerPosition.normalized;
				_towers[i].transform.rotation = Quaternion.LookRotation(towerDirection, Vector3.up);
				_towers[i].name = "Tower " + i;

				_outerTunnels[i] = Instantiate(_tunnelPrefab, _outerTunnelContainer).GetComponent<Building>();
				_outerTunnels[i].transform.position = (towerPosition + nextTowerPosition) / 2;
				Vector3 outerDirection = _outerTunnels[i].transform.position.normalized;
				_outerTunnels[i].transform.rotation = Quaternion.LookRotation(outerDirection, Vector3.up);
				_outerTunnels[i].name = "Outer " + i;

				_innerTunnels[i] = Instantiate(_tunnelPrefab, _innerTunnelContainer).GetComponent<Building>();
				_innerTunnels[i].transform.position = (towerPosition) / 2;
				Vector3 innerDirection = Quaternion.Euler(Vector3.up * 90) * towerPosition.normalized;
				_innerTunnels[i].transform.rotation = Quaternion.LookRotation(innerDirection, Vector3.up);
				_innerTunnels[i].name = "Inner " + i;
			}

			// set up building linkages
			for (int i = 0; i < towers.Length; i++)
			{
				int nextBuildingIndex = (i + 1) % 6;
				Tunnel outerTunnel = (Tunnel)_outerTunnels[i];
				outerTunnel.SetBuildingAnchors(_towers[i], _towers[nextBuildingIndex]);

				Tunnel innerTunnel = (Tunnel)_innerTunnels[i];
				innerTunnel.SetBuildingAnchors(_towers[i], _hub);
				innerTunnel.IsInnerTunnel = true;
			}

			// rotate because I forgot how to configure the hex properly :/
			transform.Rotate(Vector3.up * 90);

			float enemyRadius = 0.6f;
			_hub.SetAttackerPositions(enemyRadius);
			foreach (Building tower in _towers)
			{
				tower.SetAttackerPositions(enemyRadius);
			}
			foreach (Building tunnel in _outerTunnels)
			{
				tunnel.SetAttackerPositions(enemyRadius);
			}
			foreach (Building tunnel in _innerTunnels)
			{
				tunnel.SetAttackerPositions(enemyRadius);
			}			

			// node map
			_nodeMap = FindObjectOfType<NodeMap>();
			_nodeMap.AddNode(_hub);
			for (int i = 0; i < towers.Length; i++)
			{
				int nextBuildingIndex = (i + 1) % 6;
				_nodeMap.Connect(_hub, _innerTunnels[i]);
				_nodeMap.Connect(_innerTunnels[i], _towers[i]);
				_nodeMap.Connect(_towers[i], _outerTunnels[i]);
				_nodeMap.Connect(_outerTunnels[i], _towers[nextBuildingIndex]);
			}
			
		}
	}
}
