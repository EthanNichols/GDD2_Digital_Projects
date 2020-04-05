using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObj : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
		if (spriteRenderer == null)
		{
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Checks if player enters interactable object collision
    /// </summary>
    void OnCollisionEnter(Collision collision)
    {
        PlayerShip player = collision.gameObject.GetComponent<PlayerShip>();
        if (player != null)
        {
            ApplyEffectToPlayer(player);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Grabs player object and applies powerup my toggling flags
    /// </summary>
    public virtual void ApplyEffectToPlayer(PlayerShip player)
    {
        // do nothing right now...
    }
}
