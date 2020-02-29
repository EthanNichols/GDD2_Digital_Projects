using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Digital1
{
	public class Bullet : ColoredObj
	{
		[SerializeField]
		public Vector3 defaultVelocity = new Vector3(0, 0, -3);
		[SerializeField]
		private Vector3 velocity;
		public Vector3 Velocity
		{
			set
			{
				velocity = value;
			}
		}

		public void SetColor()
		{
			ColorSwitch((int)(GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerShip>().CurrentState));
		}

		// Start is called before the first frame update
		void Start()
		{
			if (velocity == null || velocity.sqrMagnitude == 0)
			{
				velocity = defaultVelocity;
			}
			SetColor();
		}

		// Update is called once per frame
		void FixedUpdate()
		{
			this.transform.position = this.transform.position + velocity;
		}
	}
}
