using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;

    /// <summary>
    /// Get/Set the health value of the health bar. If the value is higher than <code>MaxHealth</code>, <code>Health</code> will simply be set to <code>MaxHealth</code>
    /// </summary>
    public float Health{
        get
        {
            return health;
        }
        set
        {
            if (value > maxHealth)
            {
                health = maxHealth;
            }
            else
            {
                health = value;
            }
            UpdateFill();
        }
    }
    /// <summary>
    /// Get/Set the maximum value of the health bar. If the new max is greater than the current value for <code>Health</code>, it will be set equal to the new value as well
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
            if (health > maxHealth)
            {
                health = maxHealth;
            }
            UpdateFill();
        }
    }

    void Start()
    {
        UpdateFill();
    }

    void Update()
    {         

    }

    /// <summary>
    /// Set the width of the health bar's colored fill to match the health value
    /// </summary>
    void UpdateFill()
    {
        slider.maxValue = maxHealth;
        slider.value = health;
    }
}
