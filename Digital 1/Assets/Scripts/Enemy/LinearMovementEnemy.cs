using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LinearMovementEnemy : EnemyBase
{
		public Vector3 movementDirection;
		public float movementSpeed;

		public LinearMovementEnemy(ColorType colorType) : base(colorType)
		{
		}

		private void FixedUpdate()
		{
				transform.position += movementDirection * movementSpeed * Time.deltaTime;
		}
}
