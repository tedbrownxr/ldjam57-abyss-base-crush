// DevKit
// Copyright (c) 2024 Ted Brown

using UnityEngine;

namespace DevKit
{
	public class ColorTool 
	{
		public static string DefaultMaterialColorName = "_BaseColor";
		private static MaterialPropertyBlock s_propertyBlock;

		/// <summary>Determines a random hue and returns a color with the set saturation, brightness, and alpha. Uses normalized values.</summary>
		/// <param name="saturation">0..1</param>
		/// <param name="value">0..1</param>
		/// <param name="alpha">0..1 (1 is default)</param>
		public static Color GetRandomColor (float saturation, float value, float alpha = 1)
		{
			float hue = UnityEngine.Random.Range(0, 1f);
			Color c = Color.HSVToRGB(hue, saturation, value);
			c.a = alpha;
			return c;
		}

		public static Color GetRendererColor (Renderer renderer)
		{
			return GetRendererColor(renderer, DefaultMaterialColorName);
		}

		public static Color GetRendererColor (Renderer renderer, string materialColorName)
		{
			if (s_propertyBlock == null)
			{
				s_propertyBlock = new MaterialPropertyBlock();
			}
			renderer.GetPropertyBlock(s_propertyBlock);
			return s_propertyBlock.GetColor(materialColorName);
		}

		public static void SetRendererColor (Renderer renderer, Color color)
		{
			SetRendererColor(renderer, color, DefaultMaterialColorName);
		}

		public static void SetRendererColor (Renderer renderer, Color color, string materialColorName)
		{
			if (s_propertyBlock == null)
			{
				s_propertyBlock = new MaterialPropertyBlock();
			}
			renderer.GetPropertyBlock(s_propertyBlock);
			s_propertyBlock.SetColor(materialColorName, color);
			renderer.SetPropertyBlock(s_propertyBlock);
		}
	}
}
