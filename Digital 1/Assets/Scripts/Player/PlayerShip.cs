using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Digital1
{
    public class PlayerShip : ColoredObj
    {
        [SerializeField]
        private float moveSpeed, fireDelay;

        private SphereCollider sphereCollider;

        private float fireTimer;
        private bool canFire;
        //[SerializeField]
        //private Vector3 bulletVelocity;

        void Start()
        {
            sphereCollider = GetComponent<SphereCollider>();
            fireTimer = 0;
            canFire = true;

            //      if (bulletVelocity == null) // Are we using full expanded single line if statements?
            //{
            //          bulletVelocity = new Vector3(0, 0, -5);
            //}
        }

        [SerializeField]
        private GameObject bulletPrefab;

        /// <summary>
        /// Handle/Hold Firing logic
        /// </summary>
        void Fire()
        {
            if (!canFire || ColorState.Neutral == currentState)
                return;

            // fire a bullet
            Debug.Log("Fire");
            Vector3 tempPos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - .1f);
            GameObject.Instantiate(bulletPrefab, tempPos, bulletPrefab.transform.rotation * Quaternion.Euler(this.transform.forward.x, this.transform.forward.y, this.transform.forward.z));

            // start the timer based on delay
            fireTimer = fireDelay;
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
            if (Input.GetButton("Jump") || Input.GetKey("space"))
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
}