using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPattern : MonoBehaviour
{
		public GameObject projectilePrefab;

		public float startAngle = 0;
		[Range(-1.0f, 1.0f)]
		public float deltaAngle = 0.1f;
		private float currentAngle = 0.0f;

		public float projectileSpeed = 1.0f;

		public int projectileCount = 50;
		private int nextProjectile = 0;

		public List<GameObject> projectiles = new List<GameObject>();

		// Start is called before the first frame update
		void Start()
		{
				currentAngle = startAngle;

				for (int i = 0; i < projectileCount; ++i)
				{
						GameObject newProjectile = Instantiate(projectilePrefab);
						projectiles.Add(newProjectile);
						newProjectile.SetActive(false);
				}
		}


		private void SpawnProjectile()
		{
				GameObject fireProjectile = projectiles[nextProjectile];
				nextProjectile = (nextProjectile + 1) % projectileCount;

				currentAngle += (deltaAngle * 360) % 360;
				fireProjectile.transform.rotation = Quaternion.AngleAxis(currentAngle, Vector3.up);
				fireProjectile.SetActive(true);
				fireProjectile.transform.position = transform.position;
		}


		// Update is called once per frame
		void Update()
		{
				SpawnProjectile();

				foreach (GameObject obj in projectiles)
				{
						obj.transform.position += obj.transform.forward * projectileSpeed * Time.deltaTime;
				}
		}
}
