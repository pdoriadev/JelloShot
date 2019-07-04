﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
[RequireComponent(typeof(DeathHandler))]
public abstract class DamageableBase : MonoBehaviour, IDamageable, IHealable
{
    void OnEnable()
    {
        _CurrentHealth = _MaxHealth;
    }

    #region --HEALTH PROPERTIES--
    private float _CurrentHealth = 3f;
    public float currentHealth
    {
        get { return _CurrentHealth; }
        protected set
        {
            _CurrentHealth = value;
            if (_CurrentHealth < 0)
                _CurrentHealth = 0;
        }
    }
    private float _MaxHealth = 3f;
    public float maxHealth
    {
        get { return _MaxHealth; }
        protected set
        {
            if (_MaxHealth < 1)
                _MaxHealth = 1;
            if (value > 1)
                _MaxHealth = value;
        }
    }
    #endregion


    private bool _IsInvulnerable = false;
    public bool isInvulnerable
    {
        get { return _IsInvulnerable; }
        set { _IsInvulnerable = value; }
    }

    // Alternative To this is to use a OnTakeDmg event that dependent methods can listen to. 
    // ADVANTAGE: Damager can directly interact with Damageable. Understandable. Not inefficient.
    #region IDamageTakerMethods
    public virtual bool CanDamageCheck()
    {
        if (_IsInvulnerable == false)
            return true;
        else
        {
            Debug.Log(gameObject + " is invulnerable");
            return false;
        }
    }

    public virtual void OnTakeDmg()
    {
        if (currentHealth < 1)
        {
            StartCoroutine(CheckForDeathCo());
        }
    }

    public void TakeFullDmg()
    {
        currentHealth -= currentHealth;
        OnTakeDmg();
    }

    public virtual void TakeDmg(float _damage)
    {
        currentHealth -= _damage;
        OnTakeDmg();
    }

    public void TakeDmgPercentOfMaxHealth(float _damagePercent)
    {
        currentHealth -= (_damagePercent * maxHealth);
        OnTakeDmg();
    }

    public void TakeDmgPercentOfCurrentHealth(float _damagePercent)
    {
        currentHealth -= (_damagePercent * currentHealth);
        OnTakeDmg();
    }
    #endregion


    private bool _IsHealable = true;
    public bool isHealable
    {
        get { return _IsHealable; }
        set { _IsHealable = value; }
    }

    #region IHealableMethods
    public virtual bool CanHealCheck()
    {
        if (isHealable)
            return true;
        else
        {
            Debug.Log(gameObject + " cannot be healed");
            return false;
        }
    }

    public virtual void OnHeal()
    {

    }
    public virtual void HealToFullHealth()
    {
        if (CanHealCheck())
        {
            currentHealth = maxHealth;
            OnHeal();
        }
            
    }
    public virtual void HealToValue(float _newHealthValue)
    {
        if (CanHealCheck())
        {
            currentHealth = _newHealthValue;
            OnHeal();
        }

    }
    public virtual void HealByXMuch(float _addedHealth)
    {
        if (CanHealCheck())
        {
            currentHealth += _addedHealth;
            OnHeal();
        }      
    }

    public virtual void HealPercentageOfMaxHealth(float _percentageRegenned)
    {
        if (CanHealCheck())
        {
            currentHealth += (_percentageRegenned * maxHealth);
            OnHeal();
        }
    }
    public virtual void HealPercentageOfCurrentHealth(float _percentageRegenned)
    {
        if (CanHealCheck())
        {
            currentHealth += (_percentageRegenned * currentHealth);
            OnHeal();
        }
    }
    public virtual void HealPercentageOfCurrentDamageTaken(float _percentageRegenned)
    {
        if (CanHealCheck())
        {
            currentHealth += (_percentageRegenned * (maxHealth - currentHealth));
            OnHeal();
        }

    }
    #endregion

    #region DEATH HANDLING 
    protected virtual bool DeathCheck()
    {
        if (currentHealth <= 0)
            return true;
        else
            return false;
    }

    private float _WaitTime;
    protected float waitTime
    {
        get { return _WaitTime; }

        set
        {
            _WaitTime = value;
            if (_WaitTime < 0)
                _WaitTime = 0;
        }
    }
    protected IEnumerator CheckForDeathCo()
    {
        bool _isChecking = true;
        while (_isChecking == true)
        {
            yield return new WaitForSeconds(waitTime);
            if (DeathCheck() == true)
            {
                OnDeath();
            }
        }
        yield return null;
    }

    protected virtual void OnDeath()
    {
        StopCoroutine(CheckForDeathCo());
        currentHealth = maxHealth;

        if (GetComponent<DeathHandler>() != null)
        {
            GetComponent<DeathHandler>().OnKill();
        }
        else Debug.LogError("No Death Handler found on : " + gameObject);
    }
    #endregion
}
#if UNITY_EDITOR
[CustomEditor(typeof(DamageableBase))]
public class DeathHandlerCheck : Editor
{
    bool _ErrorSent = false;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // for other non-HideInInspector fields

        DamageableBase _DamageableBase = (DamageableBase)target;
        if (_DamageableBase.gameObject.GetComponent<DeathHandler>() == null && _ErrorSent == false)
        {
            Debug.LogError("ERROR: Missing derived DeathHandler script on gameobject!");
            _ErrorSent = true;
        }
        else if (_DamageableBase.gameObject.GetComponent<DeathHandler>() != null && _ErrorSent == true)
        {
            _ErrorSent = false;
        }
    }
}
#endif