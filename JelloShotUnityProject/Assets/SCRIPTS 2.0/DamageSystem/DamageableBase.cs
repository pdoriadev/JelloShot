using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class DamageableBase : MonoBehaviour, IDamageable
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

    // Alternative To this is to use a OnTakeDmg event that dependent methods can listen to. 
    // ADVANTAGE: Damager can directly interact with Damageable without creating spaghett.
    // Loose coupling. 
    #region IDamageTakerMethods
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

    public virtual void OnTakeDmg()
    {
        if (currentHealth < 1)
        {
            StartCoroutine(CheckForDeathCo());
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

        GetComponent<IKillable>();
        SpawnManager.instance.PoolObject(gameObject);
        ScoreManager.instance.IterateBallsKoScore();
    }
    #endregion
}
#if UNITY_EDITOR
[CustomEditor(typeof(DamageableBase))]
public class DeathHandlerCheck : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // for other non-HideInInspector fields

        DamageableBase _DamageableBase = (DamageableBase)target;
        if (_DamageableBase.gameObject.GetComponent<IKillable>() == null)
        {
            Debug.LogError("ERROR: Missing derived DeathHandler script on gameobject!");
        }
    }
}
#endif