// DevKit
// Copyright (c) 2024 Ted Brown

using System;
using System.Collections.Generic;

namespace DevKit
{
	// Source: https://www.redblobgames.com/pathfinding/a-star/implementation.html#csharp

	// This is a placeholder PriorityQueue<> that runs inefficiently but
	// will allow you to run the sample code on older C# implementations.
	//
	// If you're using a version of C# that doesn't have PriorityQueue<>,
	// consider using one of these fast libraries instead of my slow
	// placeholder:
	//
	// * https://github.com/BlueRaja/High-Speed-Priority-Queue-for-C-Sharp
	// * https://visualstudiomagazine.com/articles/2012/11/01/priority-queues-with-c.aspx
	// * http://xfleury.github.io/graphsearch.html
	// * http://stackoverflow.com/questions/102398/priority-queue-in-net
	public class PriorityQueue<TElement, TPriority>
	{
		private List<Tuple<TElement, TPriority>> elements = new List<Tuple<TElement, TPriority>>();

		public int Count
		{
			get { return elements.Count; }
		}
		
		public void Enqueue(TElement item, TPriority priority)
		{
			elements.Add(Tuple.Create(item, priority));
		}

		public TElement Dequeue()
		{
			Comparer<TPriority> comparer = Comparer<TPriority>.Default;
			int bestIndex = 0;

			for (int i = 0; i < elements.Count; i++) 
			{
				if (comparer.Compare(elements[i].Item2, elements[bestIndex].Item2) < 0) 
				{
					bestIndex = i;
				}
			}

			TElement bestItem = elements[bestIndex].Item1;
			elements.RemoveAt(bestIndex);
			return bestItem;
		}
	}
}
