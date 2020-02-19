using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject : MonoBehaviour
{
    [SerializeField]
    protected float xPos = 0.0f, zPos = 0.0f, moveSpeed = 0.0f;
    protected Collider collision;

    protected virtual void Start()
    {
        collision = GetComponent<Collider>();
        transform.position = new Vector3(xPos, 0, zPos);
    }
}
