/// <summary>
/// A time split made from a stopwatch
/// Keeps track of the starting time, finishing time,
/// and the time between the starting time and finishing time
/// </summary>
[System.Serializable]
public struct TimerSplit
{
		/// <summary>
		/// The amount of time that the split lasted
		/// </summary>
		public TimeSpanProperty splitTime;
		/// <summary>
		/// The time that the split started (relative to the timer)
		/// </summary>
		public TimeSpanProperty splitStart;
		/// <summary>
		/// The time that the split ended (relative to the timer)
		/// </summary>
		public TimeSpanProperty splitFinish;

		/// <summary>
		/// Create a new time split
		/// </summary>
		/// <param name="start">The time the split started</param>
		/// <param name="finish">The time the split finished</param>
		public TimerSplit(System.TimeSpan start, System.TimeSpan finish)
		{
				splitTime = finish - start;
				splitStart = start;
				splitFinish = finish;
		}
}
