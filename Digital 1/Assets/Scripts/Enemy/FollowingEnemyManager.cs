using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingEnemyManager : EnemySpawner
{
	//The boid prefab, and list of boids in the scene
	public List<GameObject> boids = new List<GameObject>();

	//The target being seeked
	public Transform target;
	private Vector3 targetPos;

	//Average velocity the boids are moving
	public Vector3 avgDirection;

	// Use this for initialization
	void Start()
	{
		if (target == null)
		{
			target = FindObjectOfType<PlayerShip>().transform;
		}
	}

	// Update is called once per frame
	protected override void Update()
	{
		base.Update();

		//Calculate information about the flock
		BoidCenter();
		DirectionAverage();

		//Set the direction the center is facing
		float angle = Mathf.Atan2(avgDirection.x, avgDirection.z) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, new Vector3(0, 1, 0));
	}

	public void SetTarget(Transform newTarget)
	{
		target = newTarget;
	}

	private void BoidCenter()
	{
		if (boids.Count == 0)
		{
			return;
		}

		Vector3 totalPosition = Vector3.zero;

		//Get the sum of all of the boids
		foreach (GameObject getBoid in boids)
		{
			if (getBoid == null)
			{
				continue;
			}

			totalPosition += getBoid.transform.position;
		}

		//Calculate and set the average for all of the positions
		totalPosition /= boids.Count;
		totalPosition.y += 1;
		transform.position = totalPosition;
	}

	private void DirectionAverage()
	{
		Vector3 totalDirection = Vector3.zero;

		//Get the direction all of the boids are moving in
		foreach (GameObject getBoid in boids)
		{
			if (getBoid == null)
			{
				continue;
			}

			totalDirection += getBoid.GetComponent<FollowingEnemy>().velocity;
		}

		//Calculate and normalize the direction
		totalDirection /= boids.Count;
		avgDirection = totalDirection.normalized;
	}

	public void SetNewRandomTarget()
	{
		targetPos = new Vector3(Random.Range(-PlayArea.Rect.width * 0.5f, PlayArea.Rect.width * 0.5f), 0.0f, Random.Range(-PlayArea.Rect.height * 0.5f, PlayArea.Rect.height * 0.5f)); ;
	}

	public Vector3 GetTargetPosition()
	{
		if (target == null)
		{
			return targetPos;
		}

		//Set a new position for the boids to seek
		return target.position;
	}

	public override void SpawnEnemy()
	{
		Vector3 spawnPos = new Vector3(transform.position.x + Random.Range(spawnRect.x - spawnRect.width, spawnRect.width) * 0.5f, transform.position.z, transform.position.z + Random.Range(spawnRect.y - spawnRect.height, spawnRect.height) * 0.5f);

		if (Vector3.Distance(target.position, spawnPos) < 5.0f)
		{
			return;
		}

		GameObject newBoid = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity).gameObject;
		newBoid.transform.position = spawnPos;

		newBoid.GetComponent<FollowingEnemy>().manager = this;

		boids.Add(newBoid);
	}
}
