using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Timer that counts up from 0 and can create <see cref="TimerSplit"/>s
/// Stopwatch splits can keep track of progress without stopping or reseting the timer.
/// </summary>
[System.Serializable]
public class Stopwatch : Timer
{
		/// <summary>
		/// The time that the countdown started at.
		/// </summary>
		private System.TimeSpan startingTime;

		/// <summary>
		/// List of splits that have been created from this timer
		/// </summary>
		[SerializeField]
		private List<TimerSplit> splits = new List<TimerSplit>();

		/// <summary>
		/// The index of the fastest split
		/// </summary>
		private int fastestSplitIndex = 0;
		/// <summary>
		/// The index of the slowest split
		/// </summary>
		private int slowestSplitIndex = 0;


		#region Properties
		/// <summary>
		/// Get the list of all of the splits that have been created
		/// </summary>
		public List<TimerSplit> Splits
		{
				get
				{
						return splits;
				}
		}

		/// <summary>
		/// The amount of splits that have been created
		/// </summary>
		public int SplitCount
		{
				get
				{
						return splits.Count;
				}
		}


		/// <summary>
		/// Get the current time that the timer is at
		/// </summary>
		public override System.TimeSpan TimeElasped
		{
				get
				{
						return startingTime - currentTime;
				}
		}
		#endregion


		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public Stopwatch() : base()
		{
				startingTime = currentTime;
		}


		/// <summary>
		/// Manual overloaded constructor
		/// </summary>
		public Stopwatch(int hours = 0, int minutes = 0, int seconds = 0) : base(hours, minutes, seconds)
		{
				startingTime = currentTime;
		}


		/// <summary>
		/// Manual overloaded constructor
		/// </summary>
		public Stopwatch(int days = 0, int hours = 0, int minutes = 0, int seconds = 0) : base(days, hours, minutes, seconds)
		{
				startingTime = currentTime;
		}


		/// <summary>
		/// Manual overloaded constructor
		/// </summary>
		public Stopwatch(int days = 0, int hours = 0, int minutes = 0, int seconds = 0, int milliseconds = 0) : base(days, hours, minutes, seconds, milliseconds)
		{
				startingTime = currentTime;
		}


		/// <summary>
		/// Time overloaded constructor
		/// </summary>
		public Stopwatch(System.TimeSpan time) : base(time)
		{
				startingTime = currentTime;
		}
		#endregion


		#region Time Status Functions
		/// <summary>
		/// Start the timer from where it was stopped
		/// </summary>
		public override void Start()
		{
				if (timerState == TimerState.Stopped || timerCoroutine == null)
				{
						startingTime = currentTime;
				}
				base.Start();
		}


		/// <summary>
		/// Start the timer from a specific time
		/// </summary>
		/// <param name="startTime">The time the timer will start at</param>
		public override void Start(System.TimeSpan startTime)
		{
				currentTime = startTime;
				startingTime = startTime;
				base.Start(startTime);
		}


		/// <summary>
		/// Reset the timer to the time that it was started at
		/// All splits that were created are destroyed
		/// </summary>
		public override void Reset()
		{
				ClearSplits();
				base.Reset();
		}
		#endregion


		#region Split Creation and Deletion
		/// <summary>
		/// Get the current split that hasn't been created yet
		/// </summary>
		public TimerSplit GetCurrentSplit()
		{
				System.TimeSpan start = new System.TimeSpan(0);
				if (splits.Count > 0)
				{
						start = splits[splits.Count - 1].splitFinish;
				}

				TimerSplit newSplit = new TimerSplit(start, currentTime);

				return newSplit;
		}


		/// <summary>
		/// Create a new split and add it to the list of splits
		/// </summary>
		/// <returns>The split that was created</returns>
		public TimerSplit? CreateSplit()
		{
				if (IsStopped)
				{
						Debug.LogWarning("Stopwatch isn't running, not creating new split");
						return null;
				}

				TimerSplit newSplit = GetCurrentSplit();

				if (newSplit.splitTime == new System.TimeSpan(0))
				{
						Debug.LogWarning(string.Format("Stopwatch split time is: {0}, not creating new split", newSplit.splitTime.val.ToString()));
						return null;
				}

				if (splits.Count == 0 || (System.TimeSpan)newSplit.splitTime < splits[fastestSplitIndex].splitTime)
				{
						fastestSplitIndex = splits.Count;
				}
				if (splits.Count == 0 || (System.TimeSpan)newSplit.splitTime > splits[slowestSplitIndex].splitTime)
				{
						slowestSplitIndex = splits.Count;
				}

				splits.Add(newSplit);

				return newSplit;
		}


		/// <summary>
		/// Delete a specific split from the split list
		/// </summary>
		public bool DeleteSplit(TimerSplit split)
		{
				return splits.Remove(split);
		}


		/// <summary>
		/// Delete an indexed split from the split list
		/// </summary>
		/// <param name="index">The index of the split to delete</param>
		public bool DeleteSplit(int index)
		{
				if (index < splits.Count - 1)
				{
						splits.RemoveAt(index);
						return true;
				}

				Debug.LogWarning(string.Format("Tried to remove split at index {0}, which doesn't exists", index));
				return false;
		}


		/// <summary>
		/// Clear all of the splits in the split list
		/// </summary>
		public void ClearSplits()
		{
				splits.Clear();
		}
		#endregion


		#region Split Calculation Logic
		/// <summary>
		/// Return the average time of all the splits
		/// </summary>
		public System.TimeSpan AveragesplitTime()
		{
				System.TimeSpan averageTime = new System.TimeSpan(0);

				foreach (TimerSplit split in splits)
				{
						averageTime += split.splitTime;
				}

				return System.TimeSpan.FromTicks(averageTime.Ticks / splits.Count);
		}


		/// <summary>
		/// Return the fastest split achieved
		/// Returns null if there are no splits
		/// </summary>
		public TimerSplit? GetFastestsplit()
		{
				if (splits.Count == 0)
				{
						return null;
				}

				return splits[fastestSplitIndex];
		}


		/// <summary>
		/// Return the slowest split achieve
		/// Returns null if there are no splits
		/// </summary>
		public TimerSplit? GetSlowestsplit()
		{
				if (splits.Count == 0)
				{
						return null;
				}

				return splits[slowestSplitIndex];
		}
		#endregion
}
