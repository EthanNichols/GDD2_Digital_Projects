using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperChargePowerup : InteractObj
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ApplyEffectToPlayer(PlayerShip player)
    {
        player.ActivateSuperCharge();
    }
}
