using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : ColoredObj
{
	[SerializeField]
	private float moveSpeed = default;

	[SerializeField]
	private float fireDelay = default;

    [SerializeField]
    private HealthBar healthBar;
    [SerializeField]
    private ColorIndicator colorIndicator;

	private SphereCollider sphereCollider;
	private Rigidbody rigidbody;

	private float fireTimer;
	private bool canFire;

    public bool IsDead
    {
        get
        {
            return healthBar.Health <= 0;
        }
    }

    // powerup flags
    private bool twinFire = false;
    private float twinFireTimer;

    private float twinFireAngle = 15;

    [SerializeField]
    private float maxTwinFireTimer;

    [SerializeField]
    private GameObject shieldRef;
    private GameObject activeShield; 

    private bool superCharge = false;
    private float superChargeTimer;
    [SerializeField]
    private float maxSuperChargeTimer;
    private ColorState preSCColorState;


    public bool TwinFire
    {
        set
        {
            twinFire = value;
        }
    }

    public bool SuperCharge
    {
        set
        {
            twinFire = value;
        }
    }

	public override void Start()
	{
        base.Start();
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

        if (twinFire)
        {
            // spawn bullet, set the veloctiy based on ship
            GameObject bulletTwo = GameObject.Instantiate(bulletPrefab, bulletSpawnPos, Quaternion.Euler(bulletPrefab.transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + twinFireAngle, bulletPrefab.transform.rotation.eulerAngles.z));
            GameObject bulletThree = GameObject.Instantiate(bulletPrefab, bulletSpawnPos, Quaternion.Euler(bulletPrefab.transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - twinFireAngle, bulletPrefab.transform.rotation.eulerAngles.z));
            bulletTwo.GetComponent<Bullet>().Velocity = Quaternion.AngleAxis(twinFireAngle, Vector3.up) * transform.forward;
            bulletThree.GetComponent<Bullet>().Velocity = Quaternion.AngleAxis(-twinFireAngle, Vector3.up) * transform.forward;
            
        }

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
        if (!superCharge)
        {
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
                colorIndicator.FillColor = gameObject.GetComponent<MeshRenderer>().material.color;
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
                colorIndicator.FillColor = gameObject.GetComponent<MeshRenderer>().material.color;
            }
        }
	}

    /// <summary>
    /// Method called when Player is hit and destroyed
    /// </summary>
    void GameOver()
    {
        healthBar.Health = 0;
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

    // activate shield. or increase charge
    public void DeployShield()
    {
        if (null == activeShield)
        {
            activeShield = Instantiate(shieldRef, transform.position, Quaternion.identity);
            activeShield.transform.parent = gameObject.transform;
        }
        else
            activeShield.GetComponent<Shield>().IncreaseShieldCharge();
    }

    void DestroyShield()
    {
        activeShield.GetComponent<Shield>().DecreaseShieldCharge();
    }

    public void ActivateTwinFire()
    {
        twinFire = true;
        twinFireTimer = maxTwinFireTimer;
    }

    public void ActivateSuperCharge()
    {
        superCharge = true;
        superChargeTimer = maxSuperChargeTimer;
        if (currentState != ColorState.Rainbow)
        {
            preSCColorState = currentState;
            ColorSwitch(ColorState.Rainbow);
        }
    }

    private void DeactivateSuperCharge()
    {
        superCharge = superChargeTimer > 0;
        if (!superCharge)
            ColorSwitch(preSCColorState);
    }

    /// <summary>
    /// Handle Inputs, check delays between firing.
    /// </summary>
    public override void Update()
	{
        base.Update();

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

        if (twinFire)
            twinFireTimer -= Time.deltaTime;
        twinFire = twinFireTimer > 0;

        if (superCharge)
        {
            superChargeTimer -= Time.deltaTime;
            DeactivateSuperCharge();
        }
	}
}
