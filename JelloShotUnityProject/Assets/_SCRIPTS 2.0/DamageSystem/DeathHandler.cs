using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeathHandler : MonoBehaviour
{
    [SerializeField]
    private bool _IsPooled = true;
    public UnityEvent onKillEvent;

    public virtual void OnKill()
    {
        onKillEvent.Invoke();
        if (_IsPooled)
        {
            ScoreManager.instance.IterateBallsKoScore();
            SpawnManager.instance.PoolObject(gameObject);
        }
        else
        {
            GetComponent<DamageableBase>().HealToFullHealth();
        }
    }
}
