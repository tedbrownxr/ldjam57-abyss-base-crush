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
	public class NodeMap : MonoBehaviour 
	{
		private Dictionary<Building, Node> _nodes;

		private void HandleBuildingDestroyed (Building building)
		{
			if (_nodes.ContainsKey(building))
			{
				Node node = _nodes[building];
				_nodes.Remove(building);
				foreach (Node otherNode in _nodes.Values)
				{
					otherNode.RemoveNode(node);
				}
			}
		}

		public static bool CanEnterNode (Node node)
		{
			return true;
		}

		public Node GetNode (Building building)
		{
			return _nodes[building];
		}

		public Node GetRandomNodeThatIsNotNode (Node node)
		{
			List<Node> nodes = new List<Node>();
			nodes.AddRange(_nodes.Values);
			if (nodes.Contains(node)) nodes.Remove(node);
			int index = UnityEngine.Random.Range(0, nodes.Count);
			return nodes[index];
		}

		public Node AddNode (Building building)
		{
			if (_nodes == null) _nodes = new Dictionary<Building, Node>();
			Node newNode = new Node(building);
			_nodes.Add(building, newNode);
			building.OnDestroy += HandleBuildingDestroyed;
			return newNode;
		}

		public void Connect (Building a, Building b)
		{
			if (_nodes.TryGetValue(a, out Node nodeA) == false)
			{
				nodeA = AddNode(a);
			}
			if (_nodes.TryGetValue(b, out Node nodeB) == false)
			{
				nodeB = AddNode(b);
			}
			nodeA.ConnectTo(nodeB);
			nodeB.ConnectTo(nodeA);
		}

		protected void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.green;
			foreach (Node node in _nodes.Values)
			{
				Gizmos.DrawWireSphere(node.Position, 0.6f);
				foreach (Node otherNode in node.ConnectedNodes)
				{
					Gizmos.DrawLine(node.Position, otherNode.Position);
				}
			}
		}

		public static bool TryGetPath (Node start, Node target, Func<Node, bool> canEnterNode, out List<Node> path)
		{
			Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
			Dictionary<Node, int> costSoFar = new Dictionary<Node, int>();

			var frontier = new PriorityQueue<Node, int>();
			frontier.Enqueue(start, 0);

			cameFrom[start] = start;
			costSoFar[start] = 0;

			// build a flow map that directs us towards the goal
			while (frontier.Count > 0)
			{
				Node current = frontier.Dequeue();

				if (current.Equals(target))
				{
					break;
				}

				// Get all of the hypothetical neighbors for this node, 
				// then remove ones that aren't valid for this map.
				List<Node> neighbors = new List<Node>();
				neighbors.AddRange(current.ConnectedNodes);
				for (int i = neighbors.Count - 1; i >= 0; i--)
				{
					if (canEnterNode(neighbors[i]) == false)
					{
						neighbors.RemoveAt(i);
					}
				}

				foreach (Node next in neighbors)
				{
					int costToNextHex = 1;
					int newCost = costSoFar[current] + costToNextHex;
					if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
					{
						costSoFar[next] = newCost;
						// this formula is referred to as "the heuristic"
						int priority = newCost + Math.Abs(next.x - target.x) + Math.Abs(next.y - target.y);
						frontier.Enqueue(next, priority);
						cameFrom[next] = current;
					}
				}
			}

			path = new List<Node>();

			if (cameFrom.ContainsKey(target) == false)
			{
				return false;
			}

			// walk backwards from the goal to the start to create the optimal path
			{
				var current = target;
				while (current != start)
				{
					path.Add(current);
					current = cameFrom[current];
				}
				path.Reverse();
			}

			return true;
		}		
	}
}
