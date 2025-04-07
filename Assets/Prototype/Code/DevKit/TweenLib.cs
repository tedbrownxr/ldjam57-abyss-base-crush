using UnityEngine;

namespace DevKit
{
	// https://easings.net/
	public class TweenLib
	{
		public static float EaseInCirc (float x)
		{
			return 1 - Mathf.Sqrt(1 - Mathf.Pow(x, 2));
		}

		public static float EaseInCubic (float x)
		{
			return x * x * x;
		}

		public static float EaseInOutQuad (float x)
		{
			return x < 0.5f ? 2f * x * x : 1 - Mathf.Pow(-2f * x + 2f, 2f) / 2f;
		}

		public static float EaseInQuint (float x)
		{
			return x * x * x * x * x;
		}

		public static float EaseOutBounce (float x)
		{
			const float n1 = 7.5625f;
			const float d1 = 2.75f;

			if (x < 1 / d1) 
			{
				return n1 * x * x;
			} 
			else if (x < 2 / d1) 
			{
				return n1 * (x -= 1.5f / d1) * x + 0.75f;
			} 
			else if (x < 2.5 / d1) 
			{
				return n1 * (x -= 2.25f / d1) * x + 0.9375f;
			} 
			else 
			{
				return n1 * (x -= 2.625f / d1) * x + 0.984375f;
			}
		}

		public static float EaseOutCirc (float x)
		{
			return Mathf.Sqrt(1 - Mathf.Pow(x - 1, 2));
		}

		public static float EaseOutCubic (float x)
		{
			return 1 - Mathf.Pow(1 - x, 3);
		}

		public static float EaseOutQuart (float x)
		{
			return 1 - Mathf.Pow(1 - x, 4);			
		}
	}
}
