using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
				public Vector3 movementDirection;

				[Header("Linear Movement")]
				/// <summary>
				/// The speed that the enemy moves
				/// </summary>
				public float speed;
				/// <summary>
				/// The maximum speed this enemy moves at
				/// </summary>
				public float maxSpeed;
				/// <summary>
				/// Change in <see cref="Speed"/> per tick
				/// </summary>
				public float acceleration;

				//[Header("Rotation Movement")]
				//public float angularVelocity;

				private void Start()
				{
								transform.forward = movementDirection;
				}


				private void Update()
				{
								speed += acceleration * Time.deltaTime;
								speed = Mathf.Clamp(speed, -maxSpeed, maxSpeed);
								//transform.Rotate(Vector3.up, angularVelocity * Time.deltaTime);

								transform.position += transform.forward * speed * Time.deltaTime;
				}
}
