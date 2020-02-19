using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class s_PlayerShip : BaseObject 
{
    private enum ColorState
    {
        Neutral,
        Red,
        Blue,
        Green
    }

    ColorState currentState;

    // Start is called before the first frame update
    protected override void Start()
    {
        currentState = ColorState.Neutral;

        base.Start();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // movement
        if (Input.GetButton("Horizontal"))
        {
            transform.localPosition += Input.GetAxis("Horizontal") * transform.right * Time.deltaTime * moveSpeed;
        }

        if (Input.GetButton("Vertical"))
        {
            transform.localPosition += Input.GetAxis("Vertical") * transform.forward * Time.deltaTime * moveSpeed;
        }

        // shooting

        // changing color
    }
}
