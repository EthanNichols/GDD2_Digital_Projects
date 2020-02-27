using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
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

	// Start is called before the first frame update
	void Start()
    {
		if (velocity == null)
		{
            velocity = defaultVelocity;
		}
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.position = this.transform.position + velocity;
    }
}
