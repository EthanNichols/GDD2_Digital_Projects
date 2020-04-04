using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
	private bool positionInitialized = false;

	[SerializeField]
	private new Camera camera;

	[SerializeField]
	private GameObject playerObj;

	[SerializeField]
	private float scale = .50f;

	//[SerializeField]
	//private float maxMouseDeltaFromCenter = 100f;

	private float initialSize;

	//[SerializeField]
	//private float yOffset = -250f;

	// Start is called before the first frame update
	void Start()
    {
		camera = gameObject.GetComponent<Camera>();
		// Ensure Camera is orthographic
		camera.orthographic = true;
		initialSize = camera.orthographicSize;
		if (playerObj == null)
		{
			playerObj = GameObject.FindGameObjectWithTag("Player");
		}
		Debug.Assert(playerObj != null);
		//SetInitialPos();
    }

    // Update is called once per frame
    void LateUpdate()
	{
		camera.orthographicSize = initialSize * scale;
		//Debug.Log(Input.mousePosition);
		//if (!Input.GetKey(KeyCode.LeftShift))
			transform.position = new Vector3(playerObj.transform.position.x, transform.position.y, playerObj.transform.position.z);
		//else
		//{
		//	Vector3 delta = camera.ScreenToWorldPoint(Input.mousePosition) - playerObj.transform.position;
		//	delta *= .0005f;
		//	if (delta.magnitude > maxMouseDeltaFromCenter)
		//		delta = delta.normalized * maxMouseDeltaFromCenter;

		//	transform.position = camera.WorldToScreenPoint(delta);
		//}
	}

	public void SetInitialPos()
	{
		if (!positionInitialized)
		{
			//transform.position = new Vector3(playerObj.transform.position.x, transform.position.y, playerObj.transform.position.z);
			camera.orthographicSize = initialSize * scale;
			positionInitialized = true;
			//Debug.Log("SetInit Done");
		}
		else
		{
			Debug.LogWarning("CameraFollow.SetInitialPos was already called.");
		}
	}
}
