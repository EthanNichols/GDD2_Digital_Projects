using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : ColoredObj
{
	[SerializeField]
	private Vector3 defaultBulletVelocity = Vector3.forward;

    [SerializeField]
    private float bulletSpeed = -0.25f;

    [SerializeField]
    private float maxLifeTime = 5.0f;
    private float currLifeTime;

	[SerializeField]
	private Vector3 velocity;
	public Vector3 Velocity
	{
		set
		{
			velocity = value;
		}
	}

	public void SetColor()
	{
		ColorSwitch(GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerShip>().CurrentState);
	}

	// Start is called before the first frame update
	void Start()
	{
		if (velocity == null || velocity.sqrMagnitude == 0)
		{
			velocity = defaultBulletVelocity;
		}
		SetColor();
        currLifeTime = maxLifeTime;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		this.transform.position += velocity * bulletSpeed;
	}

    // if a bullet hits an enemy ship
    private void OnCollisionEnter(Collision collision)
    {
        Enemy collidedEnemy = collision.gameObject.GetComponent<Enemy>();
        if (collidedEnemy != null)
        {
            if (collidedEnemy.CurrentState == this.currentState)
                collidedEnemy.DestroyShip();
        }
    }

    // non-physics update
    void Update()
    {
        currLifeTime -= Time.deltaTime;
        if (currLifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}