using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideyCollisions : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)GameLayers.Player)
        {
            SpawnManager.instance.PoolObject(gameObject);
            ScoreManager.instance.IterateBallsKoScore();
        }
    }
}
