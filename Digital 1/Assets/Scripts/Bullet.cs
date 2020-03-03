using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : ColoredObj
{
	[SerializeField]
	private Vector3 defaultBulletVelocity = new Vector3(0, 0, -0.25f);

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
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		this.transform.position = this.transform.position + velocity;
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
}