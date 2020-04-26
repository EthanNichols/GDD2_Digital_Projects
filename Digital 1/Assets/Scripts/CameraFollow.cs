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

    [Header("XZ Axis Variables")]
    [SerializeField]
    float xzPositionSmoothTime = .03f;
    float newPosZ, newPosX;
    Vector3 xzAxisCameraPosition;

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
        if (playerObj != null && camera != null)
        {
            // In case scale has changed, update orthographicSize
            camera.orthographicSize = initialSize * scale;

            CalculateLerpZXPosition();
            gameObject.transform.position = xzAxisCameraPosition;
        }
    }


    public void CalculateLerpZXPosition()
    {
        newPosZ = Mathf.Lerp(gameObject.transform.position.z, playerObj.transform.position.z, xzPositionSmoothTime);
        newPosX = Mathf.Lerp(gameObject.transform.position.x, playerObj.transform.position.x, xzPositionSmoothTime);
        
        xzAxisCameraPosition = new Vector3(newPosX, transform.position.y, newPosZ);
    }
}

