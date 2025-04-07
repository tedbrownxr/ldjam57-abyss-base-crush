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
	[Serializable]
	public class Threat 
	{
		/// <summary>Higher is more dangerous</summary>
		public int Danger;
		public Enemy Enemy;
		public int NearestDistance;

		public List<AwarenessTrigger> AwarenessTriggers;

		public Threat (Enemy enemy)
		{
			Enemy = enemy;
			AwarenessTriggers = new List<AwarenessTrigger>();
			NearestDistance = int.MaxValue;
		}

		public void AddAwarenessTrigger (AwarenessTrigger trigger)
		{
			if (AwarenessTriggers.Contains(trigger) == false)
			{
				AwarenessTriggers.Add(trigger);
			}
		}

		public bool IsSeenByTriggers (HashSet<AwarenessTrigger> awarenessTriggers)
		{
			foreach (AwarenessTrigger trigger in AwarenessTriggers)
			{
				if (awarenessTriggers.Contains(trigger))
				{
					return true;
				}
			}
			return false;
		}

		public void RemoveAwarenessTrigger (AwarenessTrigger trigger)
		{
			if (AwarenessTriggers.Contains(trigger))
			{
				AwarenessTriggers.Remove(trigger);
			}
		}

		public void UpdateThreat ()
		{
			if (AwarenessTriggers.Count == 0)
			{
				NearestDistance = 100000;
			}
			else
			{
				float distance = float.MaxValue;
				Vector3 position = Enemy.transform.position;
				foreach (AwarenessTrigger trigger in AwarenessTriggers)
				{
					distance = Mathf.Min(distance, trigger.GetDistance(position));
				}
				NearestDistance = (int) (distance * 100);
			}
			Danger = 1000 - NearestDistance;
		}
	}
}
