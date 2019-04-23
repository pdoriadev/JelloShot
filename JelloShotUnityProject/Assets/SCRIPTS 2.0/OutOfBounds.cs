using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Detects if ball has passed through a wall or the ground. Calls scripts to destroy ball and update score. 
/// </summary>
public class OutOfBounds : PoolingTrigger
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
        if (collision.gameObject.layer == (int)GameLayers.BallsLayer || collision.gameObject.layer == (int)GameLayers.SideyTopsey)
        {
            // REPLACE WITH CHECK FOR REFLECTOR COMPONENT OR INTERFACE
            if (collision.gameObject.tag == "reflector")
            {
                collision.GetComponent<VelocityReflector>().OnDeath();
            }

            else
            {
                PoolPassedObj(collision.gameObject);
                IterateBallKoScore();
            }
        }
    }
}
