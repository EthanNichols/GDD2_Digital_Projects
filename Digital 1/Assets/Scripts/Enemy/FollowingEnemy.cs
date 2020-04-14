using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingEnemy : Enemy
{
	[Header("Follow Parameters")]
	//The center and manager of the boid, and the target that is being seeked
	//
	public FollowingEnemyManager manager;

	//Information about the boid's movement
	public float mass;

	//Movement weights
	public float seperateWeight;
	public float cohesionWeight;
	public float alignmentWeight;
	public float seekTargetWeight;

	//Movement information
	private Vector3 position = Vector3.zero;
	private Vector3 direction = Vector3.zero;
	public Vector3 velocity = Vector3.zero;
	private Vector3 v3Acceleration = Vector3.zero;


	// Use this for initialization
	protected override void Start()
	{
		base.Start();

		//Set the position to the current position
		position = transform.position;

		if (manager == null)
		{
			manager = FindObjectOfType<FollowingEnemyManager>();
		}
	}

	void Update()
	{
		//Calculate the forces and set the position
		CalcSteeringForces();
		UpdatePosition();

		//Test if the target needs to move
		MoveTarget();

		//Set the Y position for the boid based on the terrain
		SetHeightPos();
	}

	private void SetHeightPos()
	{
		Vector3 localPos = transform.position;

		localPos.y = 0.0f;

		//Set the position of the boid
		transform.position = localPos;
	}

	void MoveTarget()
	{
		//Create a local position for the boid, set Y to 0
		Vector3 localPos = position;
		localPos.y = transform.position.y;

		//Test the distance from the boid to the target
		if (Vector3.Distance(manager.GetTargetPosition(), localPos) < 1.0f)
		{
			manager.SetNewRandomTarget();
		}
	}

	public Vector3 Avoid()
	{
		//The desired position
		Vector3 desired = Vector3.zero;

		//Go through all of the obstacles
		/*
		foreach (GameObject obstacle in GameObject.FindGameObjectsWithTag("Obstacle"))
		{
			//Test if they are close to the vehicle and will be in the way
			if ((float)Vector3.Dot(transform.forward, obstacle.transform.position - transform.position) > 0 &&
			   Vector3.Distance(transform.position, obstacle.transform.position) < (transform.localScale.x + obstacle.transform.localScale.x) * 1.75f)
			{

				//Steer the vehicle to the right ot left of the obstacle
				if (Vector3.Dot(transform.right, obstacle.transform.position - transform.position) < 0)
				{
					desired = obstacle.transform.position + transform.right * 3;
				}
				else
				{
					desired = obstacle.transform.position + (transform.right * -3);
				}
			}
		}
		*/

		return desired;
	}

	private void UpdatePosition()
	{
		//Apply the forces and set the direction for the vehicle
		velocity += v3Acceleration * Time.deltaTime;
		position += velocity * Time.deltaTime;
		velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
		direction = velocity.normalized;
		v3Acceleration = Vector3.zero;

		direction.y = 0;
		velocity.y = 0;

		position.y = transform.position.y;

		//Calculate the angle the vehicle is facing
		float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

		//Set the rotation and position of the vehicle
		transform.rotation = Quaternion.AngleAxis(angle, new Vector3(0, 1, 0));
		transform.position = position;
	}

	private void CalcSteeringForces()
	{
		//The sum of all forces
		Vector3 ultimateforce = Vector3.zero;

		//Keep the flock together and going the same direction
		ultimateforce += Alignment() * alignmentWeight;
		ultimateforce += Cohesion() * cohesionWeight;

		//Seek the target
		ultimateforce += Seek(manager.GetTargetPosition()) * seekTargetWeight;

		//Keep them away from each other and obstacles
		ultimateforce += Seperation() * seperateWeight;
		ultimateforce += Avoid();

		//Apply the force to the acceleration
		applyForces(ultimateforce);
	}

	private void applyForces(Vector3 force)
	{
		v3Acceleration += force / mass;
	}

	private Vector3 Seperation()
	{
		//The desired position
		Vector3 desired = Vector3.zero;
		GameObject closest = null;

		//Get all of the vehicles that are the same
		foreach (GameObject obstacle in manager.boids)
		{
			//Avoid testing a vehicle to itself
			if (obstacle == gameObject) { continue; }

			if (closest == null ||
				Vector3.Distance(transform.position, obstacle.transform.position) < Vector3.Distance(transform.position, closest.transform.position))
			{
				closest = obstacle;
			}
		}

		if (closest == null)
		{
			return desired;
		}

		//Steer the vehicles away from each other
		if (Vector3.Distance(transform.position, closest.transform.position) < 5)
		{
			desired = transform.position - closest.transform.position;
		}

		desired.y = transform.position.y;

		//Return the desired position
		return desired;
	}

	private Vector3 Cohesion()
	{
		return Seek(manager.gameObject.transform.position);
	}

	public Vector3 Seek(Vector3 target)
	{
		//Calculate the desired position
		Vector3 desired = target - position;

		// Scale to max speed
		desired.Normalize();
		desired = desired * maxSpeed;

		// Return the seeking steering force
		Vector3 seekingForce = desired - velocity;
		return seekingForce;
	}

	private Vector3 Alignment()
	{
		return manager.avgDirection;
	}

	private void OnDestroy()
	{
		base.DestroyShip();

		manager.boids.Remove(gameObject);
	}
}
