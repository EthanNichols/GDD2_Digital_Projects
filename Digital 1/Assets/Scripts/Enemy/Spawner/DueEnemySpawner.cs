using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DueEnemySpawner : EnemySpawner
{
	public Transform target;

	// Start is called before the first frame update
	void Start()
    {
        
    }


	public override void SpawnEnemy()
	{
		DuoEnemy newEnemy = Instantiate(enemyPrefab).GetComponent<DuoEnemy>();
		newEnemy.transform.position = new Vector3(spawnRect.x + Random.Range(-spawnRect.width, spawnRect.width) * 0.5f, 0.0f, spawnRect.y + Random.Range(-spawnRect.height, spawnRect.height) * 0.5f);
		newEnemy.target = target;
		newEnemy.movementDirection = target.position - newEnemy.transform.position;
	}
}
