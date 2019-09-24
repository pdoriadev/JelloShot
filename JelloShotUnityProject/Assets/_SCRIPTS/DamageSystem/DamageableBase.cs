using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

public struct DamagedInfo
{
    public float currentHealth;
    public float maxHealth;

    DamagedInfo(float _currentHealth, float _maxHealth)
    {
        currentHealth = _currentHealth;
        maxHealth = _maxHealth;
    }
}

[RequireComponent(typeof(DeathHandler))]
public abstract class DamageableBase : MonoBehaviour, IDamageable, IHealable
{
    public delegate void OnTakeDamageEvent(DamagedInfo _damagedInfo);
    public event OnTakeDamageEvent onTakeDamageEvent;
    public UnityEvent onTakeDamageUnityEvent;
    DamagedInfo _damagedInfo;

    void OnEnable()
    {
        currentHealth = maxHealth;
        if (GetComponent<DeathHandler>() == null)
        {
            Debug.LogError("No DeathHandler component on " + gameObject);
        }
        _damagedInfo.maxHealth = maxHealth;
        _damagedInfo.currentHealth = currentHealth;
    }

    void OnDisable()
    {

    }

    #region --HEALTH PROPERTIES--
    [SerializeField] // so can see it in inspector
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
    [SerializeField] // so can see it in inspector
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
    public bool isInvulnerable { get { return _IsInvulnerable; } set { _IsInvulnerable = value; } }

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
        if (gameObject.activeSelf)
        {
            onTakeDamageUnityEvent.Invoke();

            if (onTakeDamageEvent != null)
            {
                _damagedInfo.currentHealth = currentHealth;
                _damagedInfo.maxHealth = maxHealth;
                onTakeDamageEvent(_damagedInfo);
            }

            if (_IsCheckingDeath == false && DeathCheck() == true)
            {
                StartCoroutine(CheckForDeathCo());
            }
        }
    }
    #endregion

    #region DEATH CHECK
    protected virtual bool DeathCheck()
    {
        if (currentHealth <= 0 && isHealing == false) { return true; }
        else
            return false;
    }

    private float _WaitTime = 0.01f;
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

    private bool _IsCheckingDeath = false;
    protected IEnumerator CheckForDeathCo()
    {
        _IsCheckingDeath = true;
        while (_IsCheckingDeath == true)
        {
            if (GameManager.instance.state == GameState.LevelEnd)
            {
                OnDeath();
            }
            else if (DeathCheck() == true)
            {
                yield return new WaitForSeconds(waitTime);
                OnDeath();
            }
            yield return new WaitForSeconds(waitTime);
        }
        yield return null;
    }

    protected virtual void OnDeath()
    {
        StopCoroutine(CheckForDeathCo());

        // Handles any death related functionality unrelated to health system
        GetComponent<DeathHandler>().OnKill();

        _IsCheckingDeath = false;
        currentHealth = maxHealth;
    }
    #endregion

    #region HEALABLE PROPERTIES
    [SerializeField]
    private bool _IsHealable = true;
    public bool isHealable { get { return _IsHealable; } set { _IsHealable = value; } }
    private bool _IsHealing = false;
    public bool isHealing { get { return _IsHealing; } set { _IsHealing = value; } }
    private float _HealedHPValue;
    protected float healedHPValue
    {
        get { return _HealedHPValue; }
        set
        {
            _HealedHPValue = value;
            if (_HealedHPValue > maxHealth)
            {
                _HealedHPValue = maxHealth;
            }
            else if (_HealedHPValue < 0)
                _HealedHPValue = 0;
        }
    }
    #endregion

    #region IHealableMethods
    public virtual bool CanHealCheck()
    {
        if (isHealable)
            return true;
        else
        {
            Debug.Log(gameObject + " cannot be healed.");
            return false;
        }
    }

    public virtual void OnHeal()
    {
        currentHealth = healedHPValue;
        isHealing = false;
    }
    public virtual void HealToFullHealth()
    {
        if (CanHealCheck())
        {
            isHealing = true;
            healedHPValue = maxHealth;
            OnHeal();
        }
    }
    public virtual void HealToValue(float _newHealthValue)
    {
        if (CanHealCheck())
        {
            isHealing = true;
            healedHPValue = _newHealthValue;
            OnHeal();
        }
    }
    public virtual void HealByXMuch(float _addedHealth)
    {
        if (CanHealCheck())
        {
            isHealing = true;
            healedHPValue = currentHealth + _addedHealth;
            OnHeal();
        }
    }
    public virtual void HealPercentageOfMaxHealth(float _percentageRegenned)
    {
        if (CanHealCheck())
        {
            isHealing = true;
            healedHPValue = currentHealth + (_percentageRegenned * maxHealth);
            OnHeal();
        }
    }
    public virtual void HealPercentageOfCurrentHealth(float _percentageRegenned)
    {
        if (CanHealCheck())
        {
            isHealing = true;
            healedHPValue = currentHealth + (_percentageRegenned * currentHealth);
            OnHeal();
        }
    }
    public virtual void HealPercentageOfCurrentDamageTaken(float _percentageRegenned)
    {
        if (CanHealCheck())
        {
            isHealing = true;
            healedHPValue = currentHealth + (_percentageRegenned * (maxHealth - currentHealth));
            OnHeal();
        }
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