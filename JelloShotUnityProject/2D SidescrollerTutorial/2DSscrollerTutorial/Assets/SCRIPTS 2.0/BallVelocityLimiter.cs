using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Limits ball velocity. 
/// </summary>
public class BallVelocityLimiter : MonoBehaviour
{
    // Static instance of this script
    public static BallVelocityLimiter instance;
    // Max velocity of balls
    public float ballVelocityMagnitudeCap = 100;

    #region ENABLE AND DISABLE

    // Called when monobehaviour has been enabled. Adds BallSpawnDestroy instance to scene if there is none. 
    // Subscribes BallSpawnDestroy's FixedUpdateHandler to GM's FixedUpdate Event
    private void OnEnable()
    {
        if (instance == null)
            instance = this;
        GameManager.OnFixedUpdateEvent += OnFixedUpdateHandler;
    }

    private void OnDisable()
    {
        // Unsubscribes BallSpawnDestroy methods from GM's Events
        GameManager.OnFixedUpdateEvent -= OnFixedUpdateHandler;
    }

    void OnFixedUpdateHandler()
    {
        LimitBallVelocity();
    }
    #endregion

    public void LimitBallVelocity()
    {
        for (int index = 0; index < SpawnManager.instance.spawnablesInGame.Count; index++)
        { 
            GameObject currentBall = (GameObject)SpawnManager.instance.spawnablesInGame[index];
           // if (currentBall.gameObject.tag != "bomb")
            //{
                if (currentBall.gameObject.layer == (int)GameLayers.SideyTopsey)
                {
                    Vector2 currentBallVel = Vector2.ClampMagnitude(currentBall.GetComponent<Rigidbody2D>().velocity, ballVelocityMagnitudeCap);
                    currentBall.GetComponent<Rigidbody2D>().velocity = currentBallVel;
                }

                else
                {
                    Vector2 currentBallVel = Vector2.ClampMagnitude(currentBall.GetComponent<Rigidbody2D>().velocity, ballVelocityMagnitudeCap);
                    currentBall.GetComponent<Rigidbody2D>().velocity = currentBallVel;
                }
            //}
        }
    }
}
