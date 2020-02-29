﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Digital1
{
    public class ColoredObj : MonoBehaviour
    {
        /// <summary>
        /// List of potential color states of the player/bullet/enemy
        /// </summary>
        public enum ColorState
        {
            Neutral,
            Red,
            Blue,
            Yellow
        }

        [SerializeField]
        private Material currentMaterial;
        [SerializeField]
        private MeshRenderer meshRenderer;

        [SerializeField]
        protected ColorState currentState;
        public ColorState CurrentState
        {
            get { return currentState; }
        }

        /// <summary>
        /// Contains list of materials for color changing
        /// </summary>
        [SerializeField]
        private Material[] colorRef;

        // Start is called before the first frame update
        void Start()
        {
            currentState = ColorState.Neutral;
            currentMaterial = GetComponent<Material>();
            meshRenderer = GetComponent<MeshRenderer>();
			if (meshRenderer == null)
			{
                meshRenderer = GetComponentInChildren<MeshRenderer>();
            }
        }

        /// <summary>
        /// Handle Color State switching, then update Material/Mesh
        /// </summary>
        /// <param name="colorIndex"></param>
        protected void ColorSwitch(int colorIndex)
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
            if (meshRenderer == null)
			{
                meshRenderer = GetComponent<MeshRenderer>();
            }
            meshRenderer.material = currentMaterial;
        }
    }
}