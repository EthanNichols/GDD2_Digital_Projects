using System.Collections;
using UnityEngine;


/// <summary>
/// Base class for more advanced timer <see cref="Stopwatch"/> <see cref="CountdownTimer"/>
/// Simple timer that counts up from 0
/// </summary>
[System.Serializable]
public class Timer
{
		/// <summary>
		/// The current state of the timer
		/// </summary>
		[SerializeField]
		protected TimerState timerState = TimerState.Stopped;
		/// <summary>
		/// The current time of the timer
		/// </summary>
		[SerializeField]
		protected TimeSpanProperty currentTime;

		/// <summary>
		/// The coroutine that is updating the currenttime
		/// </summary>
		protected Coroutine timerCoroutine = null;
		private bool shuttingDown = false;

		public delegate void VoidTimerDelegate();
		/// <summary>
		/// Void delegate that is called when the timer is updated (running)
		/// </summary>
		public virtual event VoidTimerDelegate TimerUpdateEvent;

		#region Properties
		/// <summary>
		/// Get the amount of time that has passed since the timer was started
		/// </summary>
		public virtual System.TimeSpan CurrentTime
		{
				get
				{
						return currentTime;
				}
		}


		/// <summary>
		/// Get the time that has elasped since the timer was started
		/// </summary>
		public virtual System.TimeSpan TimeElasped
		{
				get
				{
						return currentTime;
				}
		}


		/// <summary>
		/// Returns true if the timer state is running
		/// </summary>
		public bool IsRunning
		{
				get
				{
						return timerState == TimerState.Running;
				}
		}


		/// <summary>
		/// Returns true if the timer state is stopped or paused
		/// </summary>
		public bool IsStopped
		{
				get
				{
						return timerState != TimerState.Running;
				}
		}

		/// <summary>
		/// Returns the current state of the timer
		/// </summary>
		public TimerState TimerState
		{
				get
				{
						return timerState;
				}
		}
		#endregion


		#region Constructors / Destructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public Timer()
		{
				currentTime = new System.TimeSpan(0);
		}

		/// <summary>
		/// Manual overloaded constructor
		/// </summary>
		public Timer(int hours = 0, int minutes = 0, int seconds = 0)
		{
				currentTime = new System.TimeSpan(hours, minutes, seconds);
		}

		/// <summary>
		/// Manual overloaded constructor
		/// </summary>
		public Timer(int days = 0, int hours = 0, int minutes = 0, int seconds = 0)
		{
				currentTime = new System.TimeSpan(days, hours, minutes, seconds);
		}

		/// <summary>
		/// Manual overloaded constructor
		/// </summary>
		public Timer(int days = 0, int hours = 0, int minutes = 0, int seconds = 0, int milliseconds = 0)
		{
				currentTime = new System.TimeSpan(days, hours, minutes, seconds, milliseconds);
		}

		/// <summary>
		/// Time overloaded constructor
		/// </summary>
		public Timer(System.TimeSpan time)
		{
				currentTime = time;
		}


		/// <summary>
		/// If the timer is being destroyed shutdown the coroutine
		/// </summary>
		~Timer()
		{
				shuttingDown = true;
				ShutdownCoroutine();
		}
		#endregion


		#region Events
		/// <summary>
		/// Subscribe to timer events
		/// </summary>
		protected virtual void SubscribeToEvents()
		{

		}


		/// <summary>
		/// Unsubscribe from timer events
		/// </summary>
		protected virtual void UnSubscribeToEvents()
		{

		}
		#endregion


		#region Time Status Functions
		/// <summary>
		/// Setup the timer coroutine without changing the current timer state.
		/// This allows the timer state to be set in the inspector or for
		/// initializing the timer without starting the timer.
		/// </summary>
		public void Init()
		{
				if (timerCoroutine == null && !shuttingDown)
				{
						timerCoroutine = TimerCoroutineManager.Instance.StartCoroutine(CoroutineTime());
				}
		}


		/// <summary>
		/// Start the timer from the current time
		/// </summary>
		public virtual void Start()
		{
				timerState = TimerState.Running;

				if (timerCoroutine == null && !shuttingDown)
				{
						timerCoroutine = TimerCoroutineManager.Instance.StartCoroutine(CoroutineTime());
				}
		}


		/// <summary>
		/// Start the timer from a specific time
		/// </summary>
		/// <param name="startTime">The time the timer will start at</param>
		public virtual void Start(System.TimeSpan startTime)
		{
				timerState = TimerState.Running;
				currentTime = startTime;

				if (timerCoroutine == null && !shuttingDown)
				{
						timerCoroutine = TimerCoroutineManager.Instance.StartCoroutine(CoroutineTime());
				}
		}

		/// <summary>
		/// Stop the timer at the current time.
		/// </summary>
		public virtual void Pause()
		{
				timerState = TimerState.Paused;
		}


		/// <summary>
		/// Stop and reset the timer
		/// </summary>
		public virtual void Stop()
		{
				timerState = TimerState.Stopped;
				Reset();
		}


		/// <summary>
		/// Reset the timer to the time that is was started at
		/// </summary>
		public virtual void Reset()
		{
				currentTime = new System.TimeSpan(0);
				TimerUpdateEvent?.Invoke();
		}
		#endregion


		#region Timer Running Functions
		/// <summary>
		/// Update the timer every frame if the timer is running
		/// This function should be called in an update function
		/// </summary>
		protected virtual void UpdateTime()
		{
				if (timerState != TimerState.Running)
				{
						return;
				}

				currentTime = currentTime.val.Add(System.TimeSpan.FromSeconds(Time.deltaTime));

				TimerUpdateEvent?.Invoke();
		}


		/// <summary>
		/// Update the timer every frame
		/// This function only runs if the timer isn't running when called
		/// </summary>
		private IEnumerator CoroutineTime()
		{
				while (!shuttingDown)
				{
						UpdateTime();
						yield return new WaitForEndOfFrame();
				}
		}


		/// <summary>
		/// Shutdown the coroutine that is running for this timer
		/// </summary>
		private void ShutdownCoroutine()
		{
				timerState = TimerState.Stopped;

				TimerCoroutineManager.Instance.StopCoroutine(timerCoroutine);
				timerCoroutine = null;
		}
		#endregion
}
