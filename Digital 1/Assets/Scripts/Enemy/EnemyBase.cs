using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
		protected float health;

		protected ColorType colorType = ColorType.None;
		public ColorType ColorType
		{
				get
				{
						return colorType;
				}
		}

		public EnemyBase(ColorType colorType)
		{
				this.colorType = colorType;
		}
}
