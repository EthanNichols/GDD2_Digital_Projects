using UnityEngine;


/// <summary>
/// Range value for floats
/// </summary>
[System.Serializable]
public class ValueRange
{
		/// <summary>
		/// The minimum value that the range can generate
		/// </summary>
		[SerializeField]
		private float minValue;
		/// <summary>
		/// The max value that the range can gernerate
		/// </summary>
		[SerializeField]
		private float maxValue;

		/// <summary>
		/// The previously generated value
		/// </summary>
		private float value;


		#region Properties
		/// <summary>
		/// Returns the minimum possible value that the range can generate
		/// </summary>
		public float MinValue
		{
				get
				{
						return minValue;
				}
				set
				{
						float tempValue = value > maxValue ? maxValue : value;

						minValue = tempValue;

						if (value > maxValue)
						{
								maxValue = value;
						}
				}
		}


		/// <summary>
		/// Returns the max possible value that the range can generate
		/// </summary>
		public float MaxValue
		{
				get
				{
						return maxValue;
				}
				set
				{
						float tempValue = value < minValue ? minValue : value;

						maxValue = tempValue;

						if (value < maxValue)
						{
								minValue = value;
						}
				}
		}


		/// <summary>
		/// Returns the previously generated value
		/// </summary>
		public float Value
		{
				get
				{
						return value;
				}
		}
		#endregion


		#region Constructors
		public ValueRange()
		{
				minValue = 0;
				maxValue = 1;
		}


		public ValueRange(float min, float max)
		{
				SetValueRange(min, max);
		}
		#endregion


		#region Setting Functions
		public void SetValueRange(float min, float max)
		{
				float tempMin = Mathf.Min(min, max);
				float tempMax = Mathf.Max(min, max);

				minValue = tempMin;
				maxValue = tempMax;
		}
		#endregion


		#region Getting Functions
		/// <summary>
		/// Generate a random value between the min and max values inclusive
		/// </summary>
		/// <returns></returns>
		public float GetRandomValue()
		{
				value = Random.Range(MinValue, maxValue);
				return value;
		}


		/// <summary>
		/// Get the medium value of the range
		/// </summary>
		public float MedianRangeValue()
		{
				return (maxValue + MinValue) / 0.5f;
		}


		/// <summary>
		/// Get the amount of values that the range can generate
		/// </summary>
		/// <returns></returns>
		public float RangeSize()
		{
				return Mathf.Abs(maxValue - MinValue);
		}
		#endregion


		#region Operator Overrides
		public override string ToString()
		{
				return string.Format("({0}, {1})", minValue, maxValue);
		}
		#endregion
}