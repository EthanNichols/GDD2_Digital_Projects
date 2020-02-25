using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
		public Vector3 spawnPosition;
		public Vector3 movementDirection;

		[Header("Linear Movement")]
		public float speed;
		public float maxSpeed;

		[Header("Rotation Movement")]
		public float angularVelocity;
		public float acceleration;

		private void Start()
		{
				transform.position = spawnPosition;
				transform.forward = movementDirection;
		}


		private void Update()
		{
				speed += acceleration * Time.deltaTime;
				speed = Mathf.Clamp(speed, -maxSpeed, maxSpeed);
				transform.Rotate(Vector3.up, angularVelocity * Time.deltaTime);

				transform.position += transform.forward * speed * Time.deltaTime;
		}
}
