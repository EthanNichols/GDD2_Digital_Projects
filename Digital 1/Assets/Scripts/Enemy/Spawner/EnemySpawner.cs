﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	/// <summary>
	/// Prefab of the enemy that this spawner spawns
	/// </summary>
	public Enemy enemyPrefab;

	/// <summary>
	/// The amount of enemies this spawner will spawn before being destroyed
	/// </summary>
	public int spawnCount;

	/// <summary>
	/// The time between enemy spawns
	/// </summary>
	public float timeBetweenSpawns;
	/// <summary>
	/// The time since the last enemy spawn
	/// </summary>
	private float timeSinceLastSpawn;

	/// <summary>
	/// The area that this spawner spawns enemies in
	/// </summary>
	public Rect spawnRect;


    [SerializeField] private GameObject[] powerupList;
    [SerializeField] private float powerupSpawnRate;

	/// <summary>
	/// Spawn the <see cref="enemyPrefab"/> in the <see cref="spawnRect"/>
	/// </summary>
	public virtual void SpawnEnemy()
	{
		spawnRect = PlayArea.Rect;
		spawnRect.size *= 2.0f;

		Enemy newEnemy = Instantiate(enemyPrefab);
		newEnemy.transform.position = new Vector3(spawnRect.x + Random.Range(-spawnRect.width, spawnRect.width) * 0.5f, 0.0f, spawnRect.y + Random.Range(-spawnRect.height, spawnRect.height) * 0.5f);

        SetSpawn(newEnemy);
	}


    protected void SetSpawn(Enemy newEnemy)
    {
        if (Random.value < powerupSpawnRate)
			newEnemy.powerupRef = powerupList[(int) Mathf.Floor(Random.Range(0.0f, powerupList.Length - 0.001f))];  
    }   

	protected virtual void Update()
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
		Gizmos.DrawWireCube(new Vector3(spawnRect.x, 0.0f, spawnRect.y), new Vector3(spawnRect.width, 0.0f, spawnRect.height));
	}
}
