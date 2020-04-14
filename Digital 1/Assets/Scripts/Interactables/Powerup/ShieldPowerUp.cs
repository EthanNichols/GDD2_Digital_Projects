using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerUp : InteractObj
{

    public override void ApplyEffectToPlayer(PlayerShip player)
    {
        player.DeployShield();
    }
}
