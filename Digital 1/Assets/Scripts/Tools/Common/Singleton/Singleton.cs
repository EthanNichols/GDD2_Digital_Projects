using UnityEngine;

/// <summary>
/// Inherit from this base class to create a singleton.
/// e.g. public class MyClassName : Singleton<MyClassName> {}
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
		/// <summary>
		/// Check to see if the singleton is about to be destroyed.
		/// Which prevent new instances from being created.
		/// </summary>
		private static bool shuttingDown = false;
		/// <summary>
		/// Prevent multithreaded applications from accidentally creating two instances
		/// </summary>
		private static object instanceLock = new object();
		/// <summary>
		/// The single instance of the class
		/// </summary>
		private static T instance;


		/// <summary>
		/// Access singleton instance through this propriety.
		/// </summary>
		public static T Instance
		{
				get
				{
						if (shuttingDown && Application.isPlaying)
						{
								Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
										"' already destroyed. Returning null.");
								return null;
						}

						lock (instanceLock)
						{
								if (instance == null)
								{
										// Search for existing instance.
										instance = (T)FindObjectOfType(typeof(T));

										// Create new instance if one doesn't already exist.
										if (instance == null)
										{
												// Need to create a new GameObject to attach the singleton to.
												GameObject singletonObject = new GameObject();
												instance = singletonObject.AddComponent<T>();
												singletonObject.name = typeof(T).ToString() + " (Singleton)";

												// Make instance persistent.
												if (Application.isPlaying)
												{
														DontDestroyOnLoad(singletonObject);
												}
										}
								}

								return instance;
						}
				}
		}


		protected virtual void OnApplicationQuit()
		{
				shuttingDown = true;
		}


		protected virtual void OnDestroy()
		{
				shuttingDown = true;
		}
}
