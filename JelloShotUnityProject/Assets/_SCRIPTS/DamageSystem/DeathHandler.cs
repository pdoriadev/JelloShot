using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeathHandler : MonoBehaviour
{
    public delegate void PreKillEvent();
    public event PreKillEvent preKillEvent;
    public UnityEvent onKillEvent;
    public GameObject killEffectGO;

    //[SerializeField]
    //private float _KillWaitTime;
    [SerializeField]
    private bool _ShouldPool = true;

    public virtual void OnKill()
    {
        if (preKillEvent != null)
            preKillEvent();
        UniversalPooler.instance.SpawnGameObject(killEffectGO, transform.position);

        onKillEvent.Invoke();
        if (_ShouldPool)
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
