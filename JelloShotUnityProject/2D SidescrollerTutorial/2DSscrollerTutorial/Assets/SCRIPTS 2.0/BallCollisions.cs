using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Handles ball collisions and ball stick mechanic. 
/// </summary>
public class BallCollisions : MonoBehaviour
{
    private DistanceJoint2D ballDistanceJoint;

    private void OnEnable()
    {
        ballDistanceJoint = GetComponent<DistanceJoint2D>();
    }

    // Handles ball to ball collision logic
    private void OnCollisionEnter2D(Collision2D collision)
    {

        // If ball collides with ground layer, call destroy ball method. 
        if (collision.gameObject.layer == (int)GameLayers.BallsLayer)
        {
            SpawnManager.instance.PoolObject(gameObject);
            return;
        }
    }
}
