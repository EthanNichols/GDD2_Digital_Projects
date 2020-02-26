using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed, fireDelay;

    private Material currentMaterial;
    private MeshRenderer meshRenderer;
    private SphereCollider sphereCollider;

    private float fireTimer;
    private bool canFire;

    /// <summary>
    /// List of potential color states of the player
    /// </summary>
    private enum ColorState
    {
        Neutral,
        Red,
        Blue,
        Yellow
    }

    [SerializeField]
    private ColorState currentState;

    /// <summary>
    /// Contains list of materials for color changing
    /// </summary>
    [SerializeField]
    private Material[] colorRef;


    void Start()
    {
        currentState = ColorState.Neutral;
        currentMaterial = GetComponent<Material>();
        sphereCollider = GetComponent<SphereCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
        fireTimer = 0;
        canFire = true;
    }

    /// <summary>
    /// Handle/Hold Firing logic
    /// </summary>
    void Fire()
    {
        if (!canFire || ColorState.Neutral == currentState)
            return;

        // fire a bullet
        Debug.Log("Fire");

        // start the timer based on delay
        fireTimer = fireDelay;
    }

    /// <summary>
    /// Handle Color State switching, then update Material/Mesh
    /// </summary>
    /// <param name="colorIndex"></param>
    void ColorSwitch(int colorIndex)
    {
        switch (colorIndex)
        {
            case 1:
                currentState = ColorState.Red;
                break;
            case 2:
                currentState = ColorState.Blue;
                break;
            case 3:
                currentState = ColorState.Yellow;
                break;
            default:
                currentState = ColorState.Neutral;
                break;
        }

        UpdateMaterial();
    }

    void UpdateMaterial()
    {
        currentMaterial = colorRef[(int)currentState];
        meshRenderer.material = currentMaterial;
    }

    /// <summary>
    /// Handle player inputs
    /// </summary>
    void HandleInput()
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
        if (Input.GetButton("Jump"))
        {
            Fire();
        }

        // changing color
        if (Input.GetKeyDown(KeyCode.L))
        {
            ColorSwitch(1);
        }
        if (Input.GetKeyDown(KeyCode.Semicolon))
        {
            ColorSwitch(2);
        }
        if (Input.GetKeyDown(KeyCode.Quote))
        {
            ColorSwitch(3);
        }
    }

    /// <summary>
    /// Handle Inputs, check delays between firing
    /// </summary>
    void FixedUpdate()
    {
        HandleInput();

        // Fire Delay Logic
        if (!canFire)
            fireTimer -= Time.deltaTime;
        canFire = fireTimer <= 0;
    }
}
