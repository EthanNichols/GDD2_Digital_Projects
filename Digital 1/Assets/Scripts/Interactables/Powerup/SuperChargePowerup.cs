using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperChargePowerup : InteractObj
{

    public override void ApplyEffectToPlayer(PlayerShip player)
    {
        player.ActivateSuperCharge();
    }
}
