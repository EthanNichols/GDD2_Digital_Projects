using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    //This should not be the slider UI. It is purely visual and doesn't use player input
    [SerializeField] private GameObject healthBarPanel;
    [SerializeField] private GameObject healthBarFill;
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;

    private RectTransform healthBarFillTransform;

    /// <summary>
    /// Get/Set the health value of the health bar
    /// </summary>
    public float Health{
        get
        {
            return health;
        }
        set
        {
            health = value;
            UpdateFill();
        }
    }
    /// <summary>
    /// Get/Set the maximum value of the health bar
    /// </summary>
    public float MaxHealth
    {
        get
        {
            return maxHealth;
        }
        set
        {
            maxHealth = value;
            UpdateFill();
        }
    }

    void Start()
    {
        healthBarFillTransform = healthBarFill.GetComponent<RectTransform>();
        UpdateFill();
    }

    void Update()
    {
        /*
         *         //For testing only
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Health++;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Health--;
        }
         */

    }

    /// <summary>
    /// Set the width of the health bar's colored fill to match the health value
    /// </summary>
    void UpdateFill()
    {
        float value = health / maxHealth;
        healthBarFillTransform.transform.localScale = new Vector3(value, 1, 1);
    }
}
