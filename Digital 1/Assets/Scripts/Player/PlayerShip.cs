using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : ColoredObj
{
	[SerializeField]
	private float moveSpeed = default;

	[SerializeField]
	private float fireDelay = default;

	private SphereCollider sphereCollider;
	private Rigidbody rigidbody;

	private float fireTimer;
	private bool canFire;

	void Start()
	{
		sphereCollider = GetComponent<SphereCollider>();
		rigidbody = GetComponent<Rigidbody>();

		fireTimer = 0;
		canFire = true;
	}

	[SerializeField]
	private GameObject bulletPrefab;

	/// <summary>
	/// Handle/Hold Firing logic.
	/// </summary>
	void Fire()
	{
		if (!canFire || ColorState.Neutral == currentState)
			return;

		// Fire a bullet
		Debug.Log("Player Bullet Fired.");
		Vector3 bulletSpawnPos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - .1f);
		GameObject.Instantiate(bulletPrefab, bulletSpawnPos, bulletPrefab.transform.rotation * Quaternion.Euler(this.transform.forward.x, this.transform.forward.y, this.transform.forward.z));

		// start the timer based on delay
		fireTimer = fireDelay;
	}

	/// <summary>
	/// Handle player inputs.
	/// </summary>
	void HandleInput()
	{
		// movement
		Vector3 movement = Vector3.zero;
		movement.x = Input.GetAxis("Horizontal");
		movement.z = Input.GetAxis("Vertical");
		movement.Normalize();
		rigidbody.velocity = movement * moveSpeed;

		// shooting
		if (Input.GetButton("Jump") || Input.GetKey("space"))
		{
			Fire();
		}

		// changing color
		if (Input.GetKeyDown(KeyCode.L))
		{
			ColorSwitch(ColorState.Red);
		}
		if (Input.GetKeyDown(KeyCode.Semicolon))
		{
			ColorSwitch(ColorState.Blue);
		}
		if (Input.GetKeyDown(KeyCode.Quote))
		{
			ColorSwitch(ColorState.Yellow);
		}
	}

	/// <summary>
	/// Handle Inputs, check delays between firing.
	/// </summary>
	void FixedUpdate()
	{
		HandleInput();

		// Fire Delay Logic
		if (!canFire)
			fireTimer -= Time.deltaTime;
		canFire = fireTimer <= 0;
	}
}