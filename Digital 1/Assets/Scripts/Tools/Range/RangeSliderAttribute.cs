using System;
using UnityEngine;


/// <summary>
/// Attribute to create a min-max range for a <see cref="Vector2"/> or <see cref="Vector2Int"/> variable
/// </summary>
[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public class RangeSliderAttribute : PropertyAttribute
{
		/// <summary>
		/// The minimum float value for the range
		/// </summary>
		public readonly float minFloat;
		/// <summary>
		/// The maximum float value for the range
		/// </summary>
		public readonly float maxFloat;

		/// <summary>
		/// The minimum int value for the range
		/// </summary>
		public readonly int minInt;
		/// <summary>
		/// The maximum int value for the range
		/// </summary>
		public readonly int maxInt;


		/// <summary>
		/// Default constructor
		/// </summary>
		public RangeSliderAttribute() : this(0, 1) { }


		/// <summary>
		/// Set the min and max value for a float range
		/// </summary>
		/// <param name="min">The minimum value of the range</param>
		/// <param name="max">The maximum value of the range</param>
		public RangeSliderAttribute(float min, float max)
		{
				minFloat = min;
				maxFloat = max;

				minInt = Mathf.RoundToInt(min);
				maxInt = Mathf.RoundToInt(max);
		}


		/// <summary>
		/// Set the min and max value for a int range
		/// </summary>
		/// <param name="min">The minimum value of the range</param>
		/// <param name="max">The maximum value of the range</param>
		public RangeSliderAttribute(int min, int max)
		{
				minFloat = min;
				maxFloat = max;

				minInt = min;
				maxInt = max;
		}
}