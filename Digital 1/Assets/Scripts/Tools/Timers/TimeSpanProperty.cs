using UnityEngine;


/// <summary>
/// Wrapper class for System.TimeSpan struct
/// Allows System.TimeSpan to be edited in the inspector
/// </summary>
[System.Serializable]
public class TimeSpanProperty : ISerializationCallbackReceiver
{
		/// <summary>
		/// Local System.TimeSpan variable for data getting/setting
		/// </summary>
		[HideInInspector]
		public System.TimeSpan val;

		/// Serialized property fields
		[SerializeField]
		private int days;
		[SerializeField]
		private int hours;
		[SerializeField]
		private int minutes;
		[SerializeField]
		private int seconds;
		[SerializeField]
		private int milliseconds;


		#region Cast Operators
		/// <summary>
		/// Operator to cast from TimeSpanProperty to System.TimeSpan
		/// </summary>
		public static implicit operator System.TimeSpan(TimeSpanProperty timespan)
		{
				return timespan.val;
		}


		/// <summary>
		/// Operator to cast from System.TimeSpan to TimeSpanProperty
		/// </summary>
		public static implicit operator TimeSpanProperty(System.TimeSpan timespan)
		{
				return new TimeSpanProperty() { val = timespan };
		}
		#endregion


		#region Serialization Implementation
		/// <summary>
		/// Called before the editor draws
		/// Which sets the local serialized values for to display in the inspector
		/// </summary>
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
				days = val.Days;
				hours = val.Hours;
				minutes = val.Minutes;
				seconds = val.Seconds;
				milliseconds = val.Milliseconds;
		}


		/// <summary>
		/// Called after the editor draws
		/// Set the local System.TimeSpan variable to the modified inspector values
		/// Which sets the
		/// </summary>
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
				val = new System.TimeSpan(days, hours, minutes, seconds, milliseconds);
		}
		#endregion
}
