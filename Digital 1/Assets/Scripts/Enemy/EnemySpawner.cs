using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
		public LinearMovementEnemy enemyPrefab;

		public float spawnsPerSecond = 0.5f;
		private float timeSinceLastSpawn = 0.0f;
		private float timeBetweenSpawns = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
				timeBetweenSpawns = 1.0f / spawnsPerSecond;
    }

    // Update is called once per frame
    void Update()
    {
				timeSinceLastSpawn += Time.deltaTime;

				if (timeSinceLastSpawn > timeBetweenSpawns)
				{
						timeSinceLastSpawn = 0;
						LinearMovementEnemy obj = Instantiate(enemyPrefab);

						obj.transform.position = new Vector3(6, 0, Random.Range(-8.5f, 8.5f));
						obj.movementDirection = new Vector3(-1, 0, 0);
						obj.movementSpeed = Random.Range(1.0f, 3.0f);
				}
    }
}
