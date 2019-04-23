using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == (int)GameLayers.SideyTopseyWall)
        {
            SpawnManager.instance.PoolObject(collision.gameObject);
            ScoreManager.instance.IterateBallsKoScore();
        } 
    }
}
