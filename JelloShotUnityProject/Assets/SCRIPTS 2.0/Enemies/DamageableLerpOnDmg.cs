using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SizeLerper))]
public class DamageableLerpOnDmg : Damageable
{
    private SizeLerper _LerperInstance;
    
    #region UNITY CALLBACKS
    void OnEnable()
    {
        if (GetComponent<SizeLerper>() == null)
        {
            _LerperInstance = gameObject.AddComponent<SizeLerper>() as SizeLerper;
        }
        else
            _LerperInstance = GetComponent<SizeLerper>();
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