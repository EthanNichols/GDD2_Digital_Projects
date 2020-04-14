using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinFirePowerup : InteractObj
{
    public override void ApplyEffectToPlayer(PlayerShip player)
    {
        player.ActivateTwinFire();
    }
}
