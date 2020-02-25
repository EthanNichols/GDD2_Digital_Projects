/// <summary>
/// The different states that a timer can be
/// </summary>
public enum TimerState
{
		Stopped,    // The timer is not running, and has been reset
		Running,    // The timer is current running
		Paused,     // The timer was running, and hasn't been reset
}