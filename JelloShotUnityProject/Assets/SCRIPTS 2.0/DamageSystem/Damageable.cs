using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Damageable : MonoBehaviour, IDamageTaker
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
            print("Current: " + _CurrentHealth);
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
            print("Max: " + _MaxHealth);
        }
    }
    #endregion

    // Alternative To this is to have static TakeDamage class where these are all methods that can be called by Damageable
    // under certain conditions. ADVANTAGE: Damager can directly interact with Damageable without creating spaghett.
    // Loose coupling. DISADVANTAGE: Interfaces are slower compared to direct calls, albeit only slightly --> https://stackoverflow.com/questions/7225205/performance-of-direct-virtual-call-vs-interface-call-in-c-sharp
    #region IDamageTakerMethods
    public void TakeFullDmg()
    {
        currentHealth -= currentHealth;
        OnTakeDmg();
    }

    public virtual void TakeDmg(float _damage)
    {
        print("PreDamage :" + gameObject + " : " + _CurrentHealth);
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

    public virtual void OnTakeDmg()
    {
        if (currentHealth < 1)
        {
            CheckForDeathCo();
        }
        print("Post-Damage :" + gameObject + " : " + _CurrentHealth);
    }
    #endregion

    #region DEATH HANDLING 
    private float _WaitTime;
    protected float waitTime
    {
        get { return _WaitTime; }

        set
        {
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

    protected virtual bool DeathCheck()
    {
        if (currentHealth <= 0)
            return true;
        else
            return false;
    }

    protected virtual void OnDeath()
    {
        print("Died at " + currentHealth + " Health");
        StopCoroutine(CheckForDeathCo());
        currentHealth = maxHealth;
        SpawnManager.instance.PoolObject(gameObject);
        ScoreManager.instance.IterateBallsKoScore();
    }

    #endregion
}
