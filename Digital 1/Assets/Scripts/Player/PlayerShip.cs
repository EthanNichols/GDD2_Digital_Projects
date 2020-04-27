using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : ColoredObj
{
    [SerializeField]
    private float moveSpeed = default;

    [SerializeField]
    private float fireDelay = default;

    //UI Elements
    [SerializeField]
    private HealthBar healthBar;
    [SerializeField]
    private ColorIndicator colorIndicator;
    [SerializeField]
    private PowerupBar twinFireBar;
    [SerializeField]
    private PowerupBar superChargeBar;
    [SerializeField]
    private PowerupBar shieldBar;
    [SerializeField]
    private Score scoreValue;

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

    private ParticleSystem ps_flames;

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

    // gamepad flags
    private bool usingGamepad = false;
    public bool UsingGamepad
    {
        set { usingGamepad = value; }
    }
    private bool colorButtonDown = false;
    [SerializeField]
    private float inputThreshold;

    public override void Start()
    {
        base.Start();
        sphereCollider = GetComponent<SphereCollider>();
        rigidbody = GetComponent<Rigidbody>();

        fireTimer = 0;
        canFire = true;

        twinFireBar.MaxTime = maxTwinFireTimer;
        superChargeBar.MaxTime = maxSuperChargeTimer;
        ps_flames = GetComponentInChildren<ParticleSystem>();
        
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

		AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
		for (int i = 0; i < audioSources.Length; i++)
		{
			if (audioSources[i].name == "LaserShot")
			{
				audioSources[i].Play();
			}
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
        rigidbody.velocity = movement * moveSpeed;
        rigidbody.angularVelocity = Vector3.zero;

        // shooting
        if (Input.GetAxis("Fire1") == 1.0f && !usingGamepad)
        {
            Fire();
        }

        // gamepad specific rotating
        if (usingGamepad)
        {
            float x = Input.GetAxis("GP X");
            float y = Input.GetAxis("GP Y");

            //Debug.Log(x + ", " + y);

            if (Mathf.Abs(x) < inputThreshold) x = 0;
            if (Mathf.Abs(y) < inputThreshold) y = 0;

            // process input
            if (Mathf.Abs(x) >= inputThreshold || Mathf.Abs(y) >= inputThreshold)
            {
                // rotate ship
                float angle = Mathf.Atan2(-x, -y) * Mathf.Rad2Deg;
                //Debug.Log(angle);
                transform.rotation = Quaternion.Euler(new Vector3(0,angle,0));

                // fire
                Vector2 tempV2 = new Vector2(x, y);
                if (tempV2.magnitude > .95)
                    Fire();
            }

        }


        // changing color
        if (!superCharge)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0 && !colorButtonDown)
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

            if (Input.GetAxis("Mouse ScrollWheel") < 0 && !colorButtonDown)
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

            if (Input.GetAxis("Mouse ScrollWheel") != 0)
                colorButtonDown = true;
            else
                colorButtonDown = false;
        }
    }

    /// <summary>
    /// Method called when Player is hit and destroyed
    /// </summary>
    void GameOver()
    {
		//AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
		//for (int i = 0; i < audioSources.Length; i++)
		//{
		//	if (audioSources[i].name == "SpecialExplosion")
		//	{
		//		audioSources[i].Play();
		//	}
		//}
		healthBar.Health = 0;
		Assets.Scripts.Shared.ScoreManager.Instance.isShuttingDown = true;
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

	void PlayPowerUpSound()
	{
		AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
		for (int i = 0; i < audioSources.Length; i++)
		{
			if (audioSources[i].name == "PowerupGet")
			{
				audioSources[i].Play();
			}
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
		PlayPowerUpSound();
    }

    void DestroyShield()
    {
        activeShield.GetComponent<Shield>().DecreaseShieldCharge();
    }

    public void ActivateTwinFire()
    {
        twinFire = true;
        twinFireTimer = maxTwinFireTimer;
		PlayPowerUpSound();
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
		PlayPowerUpSound();
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
    public /*override */void LateUpdate()
    {
        base.Update();

        HandleInput();

        // Get Mouse Position, rotate ship to that position
        if (!usingGamepad)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.y = 0;

            Vector3 tempPosition = transform.position;
            tempPosition.y = 0;

            transform.rotation = Quaternion.LookRotation(tempPosition - mousePosition);
        }

        if(rigidbody.velocity.magnitude < 1f)
        {
            ps_flames.Play();
        }


        // Fire Delay Logic
        if (!canFire)
            fireTimer -= Time.deltaTime;
        canFire = fireTimer <= 0;

        if (twinFire)
        {
            twinFireTimer -= Time.deltaTime;
        }
        twinFire = twinFireTimer > 0;

        if (superCharge)
        {
            superChargeTimer -= Time.deltaTime;
            DeactivateSuperCharge();
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (activeShield != null)
        {
            shieldBar.Time = activeShield.GetComponent<Shield>().Charges;
            shieldBar.MaxTime = activeShield.GetComponent<Shield>().MaxCharges;
        }
        twinFireBar.Time = twinFireTimer;
        superChargeBar.Time = superChargeTimer;

        //Update UI bars
        shieldBar.gameObject.SetActive(shieldBar.Time > 0 ? true : false);
        superChargeBar.gameObject.SetActive(superChargeBar.Time > 0 ? true : false);
        twinFireBar.gameObject.SetActive(twinFireBar.Time > 0 ? true : false);

        scoreValue.UpdateScore();
    }
}
