using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : ColoredObj
{
	public Vector3 movementDirection;

    [Header("Score Variables")]
    
    /// Value of points player gets when they kill this enemy
    [SerializeField]
    private int scoreValue = 10;
    public int ScoreValue { get => scoreValue; }

    [Header("Linear Movement")]
	/// <summary>
	/// The speed that the enemy moves
	/// </summary>
	public float speed;
	/// <summary>
	/// The maximum speed this enemy moves at
	/// </summary>
	public float maxSpeed;
	/// <summary>
	/// Change in <see cref="Speed"/> per tick
	/// </summary>
	public float fAcceleration;

	public float directionRandomness = 0.0f;

	public GameObject powerupRef = null;


    //[Header("Rotation Movement")]
    //public float angularVelocity;

    protected virtual void Start()
	{
		transform.forward = movementDirection;
		transform.Rotate(Vector3.up, Random.Range(-directionRandomness, directionRandomness));

        // set self to random color
        int color = Random.Range(1, 4);
        ColorSwitch((ColorState)color);
	}


	private void Update()
	{
		speed += fAcceleration * Time.deltaTime;
		speed = Mathf.Clamp(speed, -maxSpeed, maxSpeed);
		//transform.Rotate(Vector3.up, angularVelocity * Time.deltaTime);

		transform.position += transform.forward * speed * Time.deltaTime;
	}

    public void DestroyShip() 
    {
		if (powerupRef)
		{
			GameObject powerup = Instantiate(powerupRef);
			powerup.transform.position = transform.position;
		}
        Destroy(gameObject);
    }
}
