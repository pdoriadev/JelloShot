using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SizeLerper))]
public class DamageableLerpOnDmg : Damageable
{
    private SizeLerper _LerperInstance;
    [SerializeField]
    private float _NewMaxHealth = 3;
    private float _WaitTime
    {
        get { return waitTime; }
        set
        {
            if (value >= 0)
                waitTime = value;
        }
    }

    #region UNITY CALLBACKS
    void OnEnable()
    {
        if (GetComponent<SizeLerper>() == null)
        {
            _LerperInstance = gameObject.AddComponent<SizeLerper>() as SizeLerper;
        }
        else
            _LerperInstance = GetComponent<SizeLerper>();

        maxHealth = _NewMaxHealth;
        currentHealth = _NewMaxHealth;
        _WaitTime = _LerperInstance.lerpTime;
    }

    void OnDisable()
    {
        _LerperInstance = null;
    }
    #endregion

    public override void OnTakeDmg()
    {
        base.OnTakeDmg();
        _LerperInstance.StartLerp(currentHealth, maxHealth);
    }

    protected override void OnDeath()
    {

        base.OnDeath();
    }
}