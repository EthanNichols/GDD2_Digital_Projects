using Assets.Scripts.Shared;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : ColoredObj
{
	[SerializeField]
	private Vector3 defaultBulletVelocity = Vector3.forward;

    [SerializeField]
    private float bulletSpeed;

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
	public override void Start()
	{
        base.Start();
		if (velocity == null || velocity.sqrMagnitude == 0)
		{
			velocity = defaultBulletVelocity;
		}
		SetColor();
        currLifeTime = maxLifeTime;
	}

    /// <summary>
    /// Check if bullet collides with Enemy ship
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        Enemy collidedEnemy = collision.gameObject.GetComponent<Enemy>();
        if (collidedEnemy != null)
        {
            if (collidedEnemy.CurrentState == this.currentState || ColorState.Rainbow == this.currentState)
            {
                ScoreManager.Instance.ChangeScoreBy(collidedEnemy.ScoreValue);
                collidedEnemy.DestroyShip();
            }

            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
	{
		this.transform.position += velocity * bulletSpeed;
	}

    // non-physics update
    public override void Update()
    {
        base.Update();
        currLifeTime -= Time.deltaTime;
        if (currLifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}