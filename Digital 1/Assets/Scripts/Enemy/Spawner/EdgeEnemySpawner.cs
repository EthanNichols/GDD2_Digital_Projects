using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeEnemySpawner : EnemySpawner
{
	/// <summary>
	/// The time since the last enemy spawn
	/// </summary>
	private float timeSinceLastSpawn;

	/// <summary>
	/// The distance from the center that enemies will spawn at.
	/// </summary>
	private float radius;


	private void Start()
	{
	}


	/// <summary>
	/// Spawn the <see cref="enemyPrefab"/> in the <see cref="spawnRect"/>
	/// </summary>
	public override void SpawnEnemy()
	{
		radius = PlayArea.Rect.size.magnitude * 1.2f;

		Enemy newEnemy = Instantiate(enemyPrefab);

		Vector3 spawnDirection = new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f));
		Vector3 spawnPosition = spawnDirection.normalized * radius;
		newEnemy.transform.position = spawnPosition;

		newEnemy.directionRandomness = 25f;
		newEnemy.movementDirection = -spawnDirection;
		newEnemy.maxSpeed = 5.0f;
		newEnemy.speed = 5.0f;

        SetSpawn(newEnemy);
	}


	void Update()
	{
		timeSinceLastSpawn += Time.deltaTime;

		if (timeSinceLastSpawn >= timeBetweenSpawns)
		{
			timeSinceLastSpawn -= timeBetweenSpawns;
			spawnCount -= 1;
			SpawnEnemy();

			// Destroy the object if the spawner is done spawning enemies
			if (spawnCount == 0)
			{
				Destroy(gameObject);
			}
		}
	}


	// Draw the rectangle that enemies are spawned in
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(Vector3.zero, radius);
	}
}
