using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuoEnemy : Enemy
{
	[Header("Lunging Enemy")]
	public GameObject leftShip;
	public GameObject rightShip;

	public Transform target;

	public float timeBetweenLunges;
	public float lungingSpeed;

	private float lungingTimer;
	private float currentSpeed = 0;

	private ColorState leftColor;
	private ColorState rightColor;

	protected override void Start()
	{
		base.Start();

		lungingTimer = timeBetweenLunges;

		int color = Random.Range(1, 4);
		leftColor = (ColorState)color;
		color = Random.Range(1, 4);
		color = color == (int)leftColor ? color % 3 + 1 : color;
		rightColor = (ColorState)color;
		UpdateMaterial();
	}

	private void Update()
	{
		if (transform == null || target == null)
		{
			return;
		}
		transform.localRotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);

		transform.position += transform.forward * currentSpeed * Time.deltaTime;

		currentSpeed *= 0.975f;
		if (currentSpeed < 0.01f)
		{
			currentSpeed = 0;
		}

		lungingTimer -= Time.deltaTime;

		if (lungingTimer <= 0)
		{
			lungingTimer = timeBetweenLunges;
			currentSpeed = lungingSpeed;
		}
	}

	public override void UpdateMaterial()
	{
		Material leftShipMat = leftShip.GetComponent<Material>();
		Material rightShipMat = rightShip.GetComponent<Material>();

		MeshRenderer leftShipMesh = leftShip.GetComponent<MeshRenderer>();
		MeshRenderer rightShipMesh = rightShip.GetComponent<MeshRenderer>();

		if (ColorState.Rainbow == currentState)
		{
			leftShipMat = colorRef[0];
			rightShipMat = colorRef[0];
			return;
		}

		try
		{
			leftShipMat = colorRef[(int)leftColor];
			rightShipMat = colorRef[(int)rightColor];
		}
		catch (System.IndexOutOfRangeException)
		{
			Debug.LogWarning("IndexOutOfRangeException in ColoredObj.UpdateMaterial(). Failed to change Material.");
		}

		leftShipMesh.material = leftShipMat;
		rightShipMesh.material = rightShipMat;
	}

	public bool BulletHit(ColorState color)
	{
		if (leftColor == color)
		{
			leftColor = ColorState.Neutral;
			UpdateMaterial();
		}
		else if (rightColor == color)
		{
			rightColor = ColorState.Neutral;
			UpdateMaterial();
		}

		if (leftColor == ColorState.Neutral && rightColor == ColorState.Neutral)
		{
			return true;
		}

		return false;
	}
}
