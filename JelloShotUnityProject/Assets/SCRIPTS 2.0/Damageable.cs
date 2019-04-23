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
    private float _CurrentHealth;
    [SerializeField]
    private float _MaxHealth = 3;

    //public float currentHealth
    //{
    //    get { return _CurrentHealth; }
    //    set { _CurrentHealth = value; }
    //}

    //public float startingHealth
    //{
    //    get { return _StartingHealth; }
    //    set { _StartingHealth = value; }
    //}
    #endregion

    public virtual void TakeDamage(float damage)
    {
        // Define how much damage
    }

    protected IEnumerator CheckForDeathCo()
    {
        bool _isChecking = true;
        while (_isChecking == true)
        {
            yield return new WaitForSeconds(0.01f);
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
