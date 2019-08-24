using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagerPlayerFaction : Damager
{
    // Checks if colliding game object is friendly to the player faction. 
    protected override void DamageOnCollisionEnter(ref Collision2D _collision)
    {
        if (_collision.gameObject.layer != (int)GameLayers.Ground) 
        {
            if (_collision.gameObject.layer != (int)GameLayers.Player)
                DamageEnemy(ref _collision);
        }
    }

    protected virtual void DamageEnemy(ref Collision2D _collision)
    {
        base.DamageOnCollisionEnter(ref _collision);
    }
}
