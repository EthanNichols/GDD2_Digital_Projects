/// <summary>
/// Singleton that controls any timer's updates during runtime
/// The reduces the amount of clutter that some tools would create and makes them simpler to use
/// This shouldn't be referenced outside of a <see cref="Timer"/> class
/// </summary>
public class TimerCoroutineManager : Singleton<TimerCoroutineManager>
{

		protected override void OnApplicationQuit()
		{
				StopAllCoroutines();
				base.OnApplicationQuit();
		}


		protected override void OnDestroy()
		{
				StopAllCoroutines();
		}
}
