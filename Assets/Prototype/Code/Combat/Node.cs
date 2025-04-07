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
	public class Node
	{
		public readonly int x, y;
		public Building Building;
		public Vector3 Position;
		public List<Node> ConnectedNodes;

		public Node (Building building)
		{
			Building = building;
			Position = building.transform.position;
			x = (int) (Position.x * 100);
			y = (int) (Position.y * 100);
			Position.y = 1;
			ConnectedNodes = new List<Node>();
		}

		public void ConnectTo (Node node)
		{
			if (ConnectedNodes.Contains(node) == false)
			{
				ConnectedNodes.Add(node);
			}
		}

		public void RemoveNode (Node node)
		{
			if (ConnectedNodes.Contains(node))
			{
				ConnectedNodes.Remove(node);
			}
		}
	}
}
