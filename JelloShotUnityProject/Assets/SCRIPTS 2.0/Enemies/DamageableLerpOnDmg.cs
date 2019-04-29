using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SizeLerper))]
public class DamageableLerpOnDmg : Damageable
{
    private SizeLerper _LerperInstance;
    [SerializeField]
    private float _MaxHealth = 3;
    private float _WaitTime
    {
        get { return waitTime; }
        set
        {
            _WaitTime = value;
            if (_WaitTime > 0)
                waitTime = _WaitTime;
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

        maxHealth = _MaxHealth;
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
}