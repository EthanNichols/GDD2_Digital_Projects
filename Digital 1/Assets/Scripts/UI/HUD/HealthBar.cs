using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public GameObject healthBarPanel;
    public GameObject healthBarFill;
    private RectTransform healthBarFillTransform;

    public float health;
    public float maxHealth;

    /// <summary>
    /// Get/Set the health value of the health bar
    /// </summary>
    public float Health
    {
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
        //For testing only
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Health++;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Health--;
        }
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
