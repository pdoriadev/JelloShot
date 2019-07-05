using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagerEnemyFaction : Damager
{
    bool _HasDamaged = false;

    private void OnEnable()
    {
        _HasDamaged = false;
    }

    protected override void DamageOnColliderInteraction(ref Collision2D _collision)
    {
        if (_collision.gameObject.layer != (int)GameLayers.Enemy && _HasDamaged == false)
        {
            if (_collision.gameObject.layer != (int)GameLayers.BallsLayer)
            {
                _HasDamaged = true;
                
                base.DamageOnColliderInteraction(ref _collision);
            }
        }
    }
}
