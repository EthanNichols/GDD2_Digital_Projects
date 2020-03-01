using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = default;

    [SerializeField]
    private float fireDelay = default;

    private Material currentMaterial;
    private MeshRenderer meshRenderer;
    private SphereCollider sphereCollider;
	private Rigidbody rigidbody;

    private float fireTimer;
    private bool canFire;

    [SerializeField]
    private ColorState currentState;

    /// <summary>
    /// Contains list of materials for color changing
    /// </summary>
    [SerializeField]
    private Material[] colorRef = default;


    void Start()
    {
        currentState = ColorState.Neutral;
        currentMaterial = GetComponent<Material>();
        sphereCollider = GetComponent<SphereCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
		rigidbody = GetComponent<Rigidbody>();
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
        // TODO - add Bulet spawn

        // start the timer based on delay
        fireTimer = fireDelay;
    }

    /// <summary>
    /// Handle Color State switching, then update Material/Mesh
    /// </summary>
    /// <param name="newState"></param>
    void ColorSwitch(ColorState newState)
    {
        currentState = newState;
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
		Vector3 movement = Vector3.zero;
		movement.x = Input.GetAxis("Horizontal");
		movement.z = Input.GetAxis("Vertical");
		movement.Normalize();
		rigidbody.velocity = movement * moveSpeed;

        // shooting
        if (Input.GetButton("Jump"))
        {
            Fire();
        }

        // changing color
        if (Input.GetKeyDown(KeyCode.L))
        {
            ColorSwitch(ColorState.Red);
        }
        if (Input.GetKeyDown(KeyCode.Semicolon))
        {
            ColorSwitch(ColorState.Blue);
        }
        if (Input.GetKeyDown(KeyCode.Quote))
        {
            ColorSwitch(ColorState.Yellow);
        }
    }

    /// <summary>
    /// Handle Inputs, check delays between firing
    /// </summary>
    void Update()
    {
        HandleInput();

        // Fire Delay Logic
        if (!canFire)
            fireTimer -= Time.deltaTime;
        canFire = fireTimer <= 0;
    }
}
