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
        
        // spawn bullet, set the veloctiy based on ship
        Vector3 bulletSpawnPos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z) - transform.forward;
        GameObject newBullet = GameObject.Instantiate(bulletPrefab, bulletSpawnPos, Quaternion.Euler(bulletPrefab.transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, bulletPrefab.transform.rotation.eulerAngles.z));
        newBullet.GetComponent<Bullet>().Velocity = transform.forward;

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
		rigidbody.AddForce(movement * moveSpeed);

		// shooting
		if (Input.GetAxis("Fire1") == 1.0f)
		{
			Fire();
		}

		// changing color
        if (Input.GetAxis("Mouse ScrollWheel") > 0) 
        {
            if ((int)currentState < 3)
            {
                ColorSwitch(currentState + 1);
            }
            else 
            {
                ColorSwitch((ColorState)1);
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0) 
        {
            if ((int)currentState > 1)
            {
                ColorSwitch(currentState - 1); ;
            }
            else
            {
                ColorSwitch((ColorState)3);
            }
        }
	}

    /// <summary>
    /// Method called when Player is hit and destroyed
    /// </summary>
    void GameOver()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Detect collisions with enemy
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        Enemy collidedEnemy = collision.gameObject.GetComponent<Enemy>();
        if (collidedEnemy != null)
        {
            GameOver();
        }
    }

    /// <summary>
    /// Handle Inputs, check delays between firing.
    /// </summary>
    void Update()
	{
        HandleInput();

        // Get Mouse Position, rotate ship to that position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.y = 0;

        Vector3 tempPosition = transform.position;
        tempPosition.y = 0;

        transform.rotation = Quaternion.LookRotation(tempPosition - mousePosition);

        // Fire Delay Logic
        if (!canFire)
			fireTimer -= Time.deltaTime;
		canFire = fireTimer <= 0;
	}
}
