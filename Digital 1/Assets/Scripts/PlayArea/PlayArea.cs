using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayArea : MonoBehaviour
{
    /// <summary>
    /// TEMPORARY variable for debugging purposes
    /// Can also be kept to ensure there is a background
    /// </summary>
    [SerializeField]
    private GameObject background = null;
    [SerializeField]
    private Material borderMat;

	public Vector2 playAreaSize;


	public static Rect rect;
	/// <summary>
	/// Reference to the main camera
	/// </summary>
	[SerializeField]
	private new Camera camera;

	/// <summary>
	/// A 2D rectangle defining the play area.
	/// </summary>
	/// <remarks>Orig Comment: "Get the size of a 2d (XZ) slice that is visible to the main camera"</remarks>
	public static Rect Rect
	{
		get
		{
			return rect;
		}
	}

	/// <summary>
	///
	/// </summary>
	void Start()
	{
		camera = Camera.main;

		rect.size = playAreaSize * 0.5f;

		//CalculateSize();
		CreateBounds();

		//FindObjectOfType<CameraFollow>().SetInitialPos();
		Debug.Log(rect);

		Assets.Scripts.Shared.ScoreManager.Instance.isShuttingDown = false;
	}


	/// <summary>
	/// Calculate the 2d width and height that is visible to the main camera
	/// </summary>
	private void CalculateSize()
	{
		if (!Application.isPlaying)
		{
			camera = Camera.main;
		}
		if (camera == null)
		{
			Debug.LogWarning("PlayArea.camera was null at unexpected time. Aborting CalculateSize.");
			return;
		}

		// Check cam's orientation
		if (camera.transform.forward != Vector3.down)
		{
			camera.transform.forward = Vector3.down;
		}

		// Ensure that the camera is above the XZ axis
		// If either bottom-left or top-right corner is below the XZ axis
		// The raycast value returned will be 0
		camera.transform.position = -camera.transform.forward * camera.farClipPlane * 0.5f;

		// Create an invisible plane on the XZ axis
		Plane plane = new Plane(Vector3.up, Vector3.zero);

		// Create a raycast from the bottom-left and top-right corners of the camera
		Ray ray1 = camera.ScreenPointToRay(new Vector3(0, 0, 0));
		Ray ray2 = camera.ScreenPointToRay(new Vector3(camera.pixelWidth - 1, camera.pixelHeight - 1, 0));

		// Intersection distances and points on the XZ axis declerations
		float intersect1 = 0.0f;
		float intersect2 = 0.0f;
		Vector3 point1 = Vector3.zero;
		Vector3 point2 = Vector3.zero;

		// If there was a successful raycast from the bottom-left set the point
		// Get the first point intesection position
		if (plane.Raycast(ray1, out intersect1))
		{
			point1 = ray1.GetPoint(intersect1);
		}
		// If there was a successful raycast from the top-right set the point
		// Get the second point intesection position
		if (plane.Raycast(ray2, out intersect2))
		{
			point2 = ray2.GetPoint(intersect2);
		}

		// Calculate the distance between the X and Z values of the points
		// Half of the distance is the width and height of the plane
		Vector2 rectPosition = new Vector2(point1.x, point1.z);
		Vector2 rectSize = new Vector2(point2.x, point2.z) - rectPosition;
		rect = new Rect(rectPosition, rectSize);

		// If there is a background gameobject
		// Set the local scale to match with viewport size
		if (background)
		{
			background.transform.localScale = new Vector3(rect.width, 1, rect.height);
		}
	}


	private void CreateBounds()
	{
		BoxCollider topBound = gameObject.AddComponent<BoxCollider>();
		topBound.center = new Vector3(0.0f, 0.0f, Rect.height);
		topBound.size = new Vector3(rect.width * 2.0f, 1.0f, 0.0f);

		BoxCollider bottomBound = gameObject.AddComponent<BoxCollider>();
		bottomBound.center = new Vector3(0.0f, 0.0f, -rect.height);
		bottomBound.size = new Vector3(rect.width * 2.0f, 1.0f, 0.0f);

		BoxCollider rightBound = gameObject.AddComponent<BoxCollider>();
		rightBound.center = new Vector3(Rect.width, 0.0f, 0.0f);
		rightBound.size = new Vector3(0.0f, 1.0f, rect.height * 2.0f);

		BoxCollider leftBound = gameObject.AddComponent<BoxCollider>();
		leftBound.center = new Vector3(-rect.width, 0.0f, 0.0f);
		leftBound.size = new Vector3(0.0f, 1.0f, rect.height * 2.0f);
	}

    private void OnRenderObject()
    {
        borderMat.SetPass(0);

        GL.Begin(GL.LINES);

        GL.Vertex(new Vector3(rect.x - rect.width, 0, rect.y - rect.height));
        GL.Vertex(new Vector3(rect.x - rect.width, 0, rect.y + rect.height));

        GL.Vertex(new Vector3(rect.x - rect.width, 0, rect.y + rect.height));
        GL.Vertex(new Vector3(rect.x + rect.width, 0, rect.y + rect.height));

        GL.Vertex(new Vector3(rect.x + rect.width, 0, rect.y + rect.height));
        GL.Vertex(new Vector3(rect.x + rect.width, 0, rect.y - rect.height));

        GL.Vertex(new Vector3(rect.x + rect.width, 0, rect.y - rect.height));
        GL.Vertex(new Vector3(rect.x - rect.width, 0, rect.y - rect.height));

        GL.End();
    }
}
