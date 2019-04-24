using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Damageable : MonoBehaviour, IDamageTaker
{
    void OnEnable()
    {
        _CurrentHealth = _MaxHealth;
    }

    #region --VARIABLES--
    public float _CurrentHealth
    {
        get { return _CurrentHealth; }

        set
        {
            if (_CurrentHealth < 0)
                _CurrentHealth = 0;
        }
    }
    public float _MaxHealth { get; }

    #endregion

    #region TakeDamageInterfaces
    public void TakeFullDmg()
    {
        _CurrentHealth -= _CurrentHealth;

        if (_CurrentHealth < 1)
        {
            CheckForDeathCo();
        }
    }

    public virtual void TakeDmg(float _damage)
    {
        _CurrentHealth -= _damage;

        if (_CurrentHealth < 1)
        {
            CheckForDeathCo();
        }
    }

    public void TakeDmgPercentOfMaxHealth(float _damagePercent)
    {
        _CurrentHealth -= (_damagePercent * _MaxHealth);

        if (_CurrentHealth < 1)
        {
            CheckForDeathCo();
        }
    }

    public void TakeDmgPercentOfCurrentHealth(float _damagePercent)
    {
        _CurrentHealth -= (_damagePercent * _CurrentHealth);

        if (_CurrentHealth < 1)
        {
            CheckForDeathCo();
        }
    }
    #endregion

    protected IEnumerator CheckForDeathCo()
    {
        bool _isChecking = true;
        while (_isChecking == true)
        {
            yield return new WaitForSeconds(0.02f);
            if (DeathCheck() == true)
            {
                OnDeath();
            }
        }
        yield return null;
    }

    protected virtual bool DeathCheck()
    {
        if (_CurrentHealth <= 0)
            return true;
        else
            return false;
    }

    protected virtual void OnDeath()
    {
        print("Died at " + _CurrentHealth + " Health");
        StopCoroutine(CheckForDeathCo());
        _CurrentHealth = _MaxHealth;
        SpawnManager.instance.PoolObject(gameObject);
        ScoreManager.instance.IterateBallsKoScore();
    }
}
