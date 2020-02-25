using UnityEngine;


/// <summary>
/// Time that countdowns from a starting time and stops once it reaches 0
/// </summary>
[System.Serializable]
public class CountdownTimer : Timer
{
		/// <summary>
		/// The time that the countdown started at.
		/// </summary>
		private System.TimeSpan startingTime;
		/// <summary>
		/// Event called when the timer reaches 0 (finishes)
		/// </summary>
		public event VoidTimerDelegate TimerFinishedEvent;
		/// <summary>
		/// Event called every frame that the timer is running
		/// </summary>
		public override event VoidTimerDelegate TimerUpdateEvent;


		#region Properties

		/// <summary>
		/// Get the amount of time that has passed since the timer was started
		/// </summary>
		public override System.TimeSpan CurrentTime
		{
				get
				{
						return currentTime;
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


		#region Constructors / Destructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public CountdownTimer() : base()
		{
				startingTime = currentTime;
		}


		/// <summary>
		/// Manual overloaded constructor
		/// </summary>
		public CountdownTimer(int hours = 0, int minutes = 0, int seconds = 0) : base(hours, minutes, seconds)
		{
				startingTime = currentTime;
		}


		/// <summary>
		/// Manual overloaded constructor
		/// </summary>
		public CountdownTimer(int days = 0, int hours = 0, int minutes = 0, int seconds = 0) : base(days, hours, minutes, seconds)
		{
				startingTime = currentTime;
		}


		/// <summary>
		/// Manual overloaded constructor
		/// </summary>
		public CountdownTimer(int days = 0, int hours = 0, int minutes = 0, int seconds = 0, int milliseconds = 0) : base(days, hours, minutes, seconds, milliseconds)
		{
				startingTime = currentTime;
		}


		/// <summary>
		/// Time overloaded constructor
		/// </summary>
		public CountdownTimer(System.TimeSpan time) : base(time)
		{
				startingTime = currentTime;
		}

		#endregion


		#region Events

		/// <summary>
		/// Subscribe to timer events
		/// </summary>
		protected override void SubscribeToEvents()
		{
				TimerFinishedEvent += TimerFinished;
		}


		/// <summary>
		/// Unsubscribe from timer events
		/// </summary>
		protected override void UnSubscribeToEvents()
		{
				TimerFinishedEvent -= TimerFinished;
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
		/// </summary>
		public override void Reset()
		{
				currentTime = startingTime;
				TimerUpdateEvent?.Invoke();
		}

		#endregion


		#region Timer Running Functions

		/// <summary>
		/// Update the timer every frame if the timer is running
		/// This function should be called in an update function
		/// </summary>
		protected override void UpdateTime()
		{
				if (timerState != TimerState.Running)
				{
						return;
				}

				currentTime = currentTime.val.Add(-System.TimeSpan.FromSeconds(Time.deltaTime));

				if (IsTimerFinished())
				{
						TimerFinished();
				}
				else
				{
						TimerUpdateEvent?.Invoke();
				}
		}


		/// <summary>
		/// Check if the timer has reached 0 (finished)
		/// </summary>
		/// <returns>True if the timer has finished</returns>
		protected bool IsTimerFinished()
		{
				return currentTime <= new System.TimeSpan(0);
		}


		/// <summary>
		/// Called when the timer finishes.
		/// Stops the timer and calls the timer finished event
		/// </summary>
		protected virtual void TimerFinished()
		{
				timerState = TimerState.Stopped;
				currentTime = System.TimeSpan.Zero;
				TimerFinishedEvent?.Invoke();
		}

		#endregion
}
