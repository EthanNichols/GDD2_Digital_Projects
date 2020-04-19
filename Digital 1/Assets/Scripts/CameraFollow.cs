using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
	[SerializeField]
	private new Camera camera;

	[SerializeField]
	private GameObject playerObj;

	[SerializeField]
	[Tooltip("This adjusts the camera zoom.")]
	private float scale = .50f;

	private float initialSize;

	void Start()
	{
		camera = gameObject.GetComponent<Camera>();
		// Ensure Camera is orthographic
		camera.orthographic = true;
		initialSize = camera.orthographicSize;
		camera.orthographicSize = initialSize * scale;
		if (playerObj == null)
		{
			playerObj = GameObject.FindGameObjectWithTag("Player");
		}
		Debug.Assert(playerObj != null);
		transform.position = new Vector3(playerObj.transform.position.x, transform.position.y, playerObj.transform.position.z);
	}

	void LateUpdate()
	{
		// In case scale has changed, update orthographicSize
		camera.orthographicSize = initialSize * scale;
		// Update position
		transform.position = new Vector3(playerObj.transform.position.x, transform.position.y, playerObj.transform.position.z);
	}
}
