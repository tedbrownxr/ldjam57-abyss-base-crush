// DevKit
// Copyright (c) 2024 Ted Brown

using System;
using UnityEngine;

namespace DevKit
{
	// https://www.redblobgames.com/grids/hexagons/
	/// <summary>
	/// A hex value and math library that uses an axial coordinate system.
	/// This is a flat-top, "odd q" hex system.
	/// </summary>
	[System.Serializable]
	public struct Hex : IEquatable<Hex>, IComparable<Hex>
	{
#region static
		public static Hex invalid => new Hex(int.MinValue, int.MinValue);
		public static Hex zero => new Hex(0, 0);

		private static readonly float Sqrt3 = Mathf.Sqrt(3);
		private static readonly float ThreeOverTwo = 3f/2f;
		private static readonly Hex[] DirectionVectors = 
		{
			new Hex(+1, 0), new Hex(+1, -1), new Hex(0, -1), 
			new Hex(-1, 0), new Hex(-1, +1), new Hex(0, +1)
		};

		public static Hex Add (Hex a, Hex b)
		{
			return new Hex(a.q + b.q, a.r + b.r);
		}

		public static int Distance (Hex a, Hex b)
		{
			return (Math.Abs(a.q - b.q) 
				+ Math.Abs(a.q + a.r - b.q - b.r)
				+ Math.Abs(a.r - b.r)) / 2;
		}

		/// <summary>Subtract value B from value A</summary>
		public static Hex Subtract (Hex a, Hex b)
		{
			return new Hex(a.q + b.q, a.r + b.r);
		}

		public static Hex GetHex (Vector3 position, float radius)
		{
			float qf = position.x / (radius * ThreeOverTwo);
			float rf = position.z / radius; // rf = (Sqrt3/2f * hex.q) + (Sqrt3 * hex.r)
			rf -= Sqrt3 / 2f * qf; // rf = Sqrt3 * hex.r
			rf /= Sqrt3;
			int q = (int) Math.Round(qf, MidpointRounding.AwayFromZero); // so 0.5f = 1
			int r = (int) Math.Round(rf, MidpointRounding.AwayFromZero);
			return new Hex(q, r);
		}

		public static Hex[] GetNeighborValues (Hex hex)
		{
			Hex[] neighbors = new Hex[DirectionVectors.Length];
			for (int i = 0; i < DirectionVectors.Length; i++)
			{
				neighbors[i] = Hex.Add(hex, DirectionVectors[i]);
			}
			return neighbors;
		}

		public static Vector3 GetPosition (Hex hex, float radius)
		{
			var x = radius * ThreeOverTwo * hex.q;
			var y = radius * ((Sqrt3/2f * hex.q) + (Sqrt3 * hex.r));
			return new Vector3(x, 0, y);
		}
#endregion

		public int q;
		public int r;
		public int s => -q-r;

		public Hex (int q, int r)
		{
			this.q = q;
			this.r = r;
		}

#region IComparable
		public int CompareTo (Hex other)
		{
			// according to MSDN, when comparing to an other object that is null, this instance is greater
			if (this.IsGreaterThan(other)) return 1;
			if (this.IsLessThan(other)) return -1;
			return 0; // they are equal
		}

		public bool IsGreaterThan (Hex other)
		{
			if (other.q > q) return false;
			if (other.r > r) return false;
			return true;
		}

		public bool IsLessThan (Hex other)
		{
			if (other.q < q) return false;
			if (other.r < r) return false;
			return true;
		}
#endregion

#region IEquatable
		public static bool operator == (Hex left, Hex right)
		{
			return left.Equals(right);
		}

		public static bool operator != (Hex left, Hex right)
		{
			return left.Equals(right) == false;
		}

		public override bool Equals (object obj)
		{
			if (obj == null) return false;
			if (!(obj is Hex)) return false;
			return obj.Equals(this);
		}

		public bool Equals (Hex other)
		{
			if (other.q != this.q) return false;
			if (other.r != this.r) return false;
			return true;
		}

		// source: https://stackoverflow.com/questions/3404715/c-sharp-hashcode-for-array-of-ints
		public override int GetHashCode ()
		{
			int hashcode = unchecked(q);
			hashcode = unchecked(hashcode * 314159 + r);
			return hashcode;
		}
#endregion // IEquatable

		public Hex[] GetNeighbors ()
		{
			return Hex.GetNeighborValues(this);
		}

		public Vector3 GetPosition (float radius)
		{
			return GetPosition(this, radius);
		}

		public override string ToString ()
		{
			return $"{q},{r}";
		}
	}
}
