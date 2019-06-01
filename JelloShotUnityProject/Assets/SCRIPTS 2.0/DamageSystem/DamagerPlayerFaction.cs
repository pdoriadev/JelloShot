using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagerPlayerFaction : Damager
{
    protected override void DamageOnColliderInteraction(ref Collision2D _collision)
    {
        if (_collision.gameObject.layer != (int)GameLayers.Ground) 
        {
            if (_collision.gameObject.layer != (int)GameLayers.Player)
                DamageEnemy(ref _collision);
        }
    }

    protected virtual void DamageEnemy(ref Collision2D _collision)
    {
        base.DamageOnColliderInteraction(ref _collision);
    }

}
