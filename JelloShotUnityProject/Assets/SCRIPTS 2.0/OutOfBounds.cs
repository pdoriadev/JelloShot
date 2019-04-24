using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Detects if ball has passed through a wall or the ground. Calls scripts to destroy ball and update score. 
/// </summary>
public class OutOfBounds : MonoBehaviour
{
    OutOfBounds instance;

    #region UNITY CALLBACKS
    private void OnEnable()
    {
        if (instance == null)
            instance = this;
    }

    private void OnDisable()
    {
        instance = null;
    }
    #endregion

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject _CollidedObj = collision.gameObject;
      
        if (_CollidedObj.layer == (int)GameLayers.BallsLayer || _CollidedObj.layer == (int)GameLayers.SideyTopsey)
        {
            if (_CollidedObj.GetComponent<IDamageTaker>() != null)
            {
                IDamageTaker damageTaker = _CollidedObj.GetComponent<IDamageTaker>();
                damageTaker.TakeFullDmg();
            }
        }
    }
}
