using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolingTrigger : MonoBehaviour
{   
    public void PoolPassedObj(GameObject gameObject)
    {
        SpawnManager.instance.PoolObject(gameObject);
    }

    public void IterateBallKoScore()
    {
        ScoreManager.instance.IterateBallsKoScore();
    }
}
