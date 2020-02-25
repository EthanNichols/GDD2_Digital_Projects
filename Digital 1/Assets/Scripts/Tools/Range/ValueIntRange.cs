using UnityEngine;


/// <summary>
/// Range value for ints
/// </summary>
[System.Serializable]
public class ValueIntRange
{
		/// <summary>
		/// The minimum value that the range can generate
		/// </summary>
		[SerializeField]
		private int minValue;
		/// <summary>
		/// The max value that the range can gernerate
		/// </summary>
		[SerializeField]
		private int maxValue;

		/// <summary>
		/// The previously generated value
		/// </summary>
		private int value;


		#region Properties
		/// <summary>
		/// Returns the minimum possible value that the range can generate
		/// </summary>
		public int MinValue
		{
				get
				{
						return minValue;
				}
				set
				{
						int tempValue = value > maxValue ? maxValue : value;

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
		public int MaxValue
		{
				get
				{
						return maxValue;
				}
				set
				{
						int tempValue = value < minValue ? minValue : value;

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
		public int Value
		{
				get
				{
						return value;
				}
		}
		#endregion


		#region Constructors
		public ValueIntRange()
		{
				minValue = 0;
				maxValue = 1;
		}


		public ValueIntRange(int min, int max)
		{
				SetValueIntRange(min, max);
		}
		#endregion


		#region Setting Functions
		public void SetValueIntRange(int min, int max)
		{
				int tempMin = Mathf.Min(min, max);
				int tempMax = Mathf.Max(min, max);

				minValue = tempMin;
				maxValue = tempMax;
		}
		#endregion


		#region Getting Functions
		/// <summary>
		/// Generate a random value between the min and max values inclusive
		/// </summary>
		/// <returns></returns>
		public int GetRandomValue()
		{
				/// Add 1 to maxValue since <see cref="Random.Range(int, int)" max in exculsive/>
				value = Random.Range(minValue, maxValue+1);
				return value;
		}


		/// <summary>
		/// Get the medium value of the range
		/// </summary>
		public int MedianRangeValue()
		{
				return Mathf.RoundToInt((maxValue + MinValue) / 0.5f);
		}


		/// <summary>
		/// Get the amount of values that the range can generate
		/// </summary>
		/// <returns></returns>
		public int RangeSize()
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