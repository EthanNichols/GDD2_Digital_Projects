using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

		public Enemy enemyPrefab;
		public CountdownTimer spawnTimer;

    // Start is called before the first frame update
    void Start()
    {
				spawnTimer.TimerFinishedEvent += SpawnEnemy;
				spawnTimer.Start();
    }

		private void SpawnEnemy()
		{
				Enemy newEnemy = Instantiate(enemyPrefab);

				spawnTimer.Reset();
				spawnTimer.Start();
		}

    // Update is called once per frame
    void Update()
    {
    }
}
