using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private float time;
    [SerializeField] private float maxTime;
    [SerializeField] private Image icon;
    [SerializeField] private Sprite currentSprite, defaultSprite, testSprite;

    /// <summary>
    /// Get/Set the health value of the health bar. If the value is higher than <code>MaxHealth</code>, <code>Health</code> will simply be set to <code>MaxHealth</code>
    /// </summary>
    public float Time{
        get
        {
            return time;
        }
        set
        {
            if (value > maxTime)
            {
                time = maxTime;
            }
            else
            {
                time = value;
            }
            UpdateFill();
        }
    }
    /// <summary>
    /// Get/Set the maximum value of the powerup bar. If the new max is greater than the current value for <code>Time</code>, it will be set equal to the new value as well
    /// </summary>
    public float MaxTime
    {
        get
        {
            return maxTime;
        }
        set
        {
            maxTime = value;
            if (time > maxTime)
            {
                time = maxTime;
            }
            UpdateFill();
        }
    }

    public Sprite Sprite
    {
        get
        {
            return currentSprite;
        }
        set
        {
            currentSprite = value;
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

    public void ResetPowerup()
    {
        MaxTime = 100;
        Time = 100;
        currentSprite = defaultSprite;
    }

    /// <summary>
    /// Set the width of the powerup bar's colored fill to match the time value
    /// </summary>
    void UpdateFill()
    {
        slider.maxValue = maxTime;
        slider.value = time;
        icon.sprite = currentSprite;
    }
}
