using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for colored objects (i.e. player, bullets, enemies).
/// </summary>
//[RequireComponent(typeof(MeshRenderer))]
public class ColoredObj : MonoBehaviour
{
    /// <summary>
    /// List of potential color states of the player/bullet/enemy.
    /// </summary>
    public enum ColorState
    {
        Neutral = 0,
        Red     = 1,
        Blue    = 2,
        Yellow  = 3
    }

    //[SerializeField]
    private Material currentMaterial;
    //[SerializeField]
    private MeshRenderer meshRenderer;

    [SerializeField]
    protected ColorState currentState;
    public ColorState CurrentState
    {
        get { return currentState; }
    }

    /// <summary>
    /// Contains list of materials for color changing.
    /// </summary>
    [SerializeField]
    private Material[] colorRef;

    /// <summary>
	/// Start is called before the first frame update
	/// </summary>
	/// <remarks>Known problem where meshRenderer is not initialized here; currently effective workaround is in <see cref="UpdateMaterial"/>.</remarks>
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
    /// Handle Color State switching, then update Material/Mesh.
    /// </summary>
    /// <param name="color"></param>
    protected void ColorSwitch(ColorState color)
    {
        switch (color)
        {
            case ColorState.Red:
                currentState = ColorState.Red;
                break;
            case ColorState.Blue:
                currentState = ColorState.Blue;
                break;
            case ColorState.Yellow:
                currentState = ColorState.Yellow;
                break;
            case ColorState.Neutral:
            default:
                currentState = ColorState.Neutral;
                break;
        }

        UpdateMaterial();
    }

    void UpdateMaterial()
    {
		// currentMaterial = colorRef?[(int)currentState] ?? currentMaterial; // I think Unity has null operator problems, not sure. If not, this is better.
		try
		{
            currentMaterial = colorRef[(int)currentState];
        }
		catch (System.IndexOutOfRangeException)
		{
            Debug.LogWarning("IndexOutOfRangeException in ColoredObj.UpdateMaterial(). Failed to change Material.");
		}

        if (meshRenderer == null) // Fallback for faulty init
		{
            meshRenderer = GetComponent<MeshRenderer>();
        }
        meshRenderer.material = currentMaterial;
    }
}