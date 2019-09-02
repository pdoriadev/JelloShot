using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Detects if ball has passed through a wall or the ground. Calls scripts to destroy ball and update score. 
/// </summary>
public class OutOfBounds : MonoBehaviour
{


    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject _CollidedObj = collision.gameObject;
      
        if (_CollidedObj.layer == (int)GameLayers.BallsLayer || _CollidedObj.layer == (int)GameLayers.SideyTopsey)
        {
            if (_CollidedObj.GetComponent<IDamageable>() != null)
            {
                IDamageable damageTaker = _CollidedObj.GetComponent<IDamageable>();
                damageTaker.TakeFullDmg();
            }
        }
    }
}
