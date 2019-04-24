using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Lerper))]
public class VelocityReflector : Damageable, ILerpable
{
    private Lerper _LerperInstance;
    
    #region UNITY CALLBACKS
    void OnEnable()
    {
        if (GetComponent<Lerper>() == null)
        {
            _LerperInstance = gameObject.AddComponent<Lerper>() as Lerper;
        }
        else
            _LerperInstance = GetComponent<Lerper>();
    }

    void OnDisable()
    {
        _LerperInstance = null;
    }
    #endregion

    // Lerp dependencies with damage. Lerping after taking damage.
    public void Lerp()
    {
        _LerperInstance.StartLerp(_CurrentHealth, _MaxHealth);
    }

    //IEnumerator CheckForDeathCo()
    //{
    //    bool _isChecking = true;
    //    while(_isChecking == true)
    //    {
    //        yield return new WaitForSeconds(0.01f);
    //        if (_CurrentHealth < 1 && _LerperInstance._IsLerping == false)
    //            OnDeath();
    //    }

    //    yield return null;
    //}

    //public void OnDeath()
    //{
    //    print("Died at " + _CurrentHealth + " Health");
    //    StopCoroutine(CheckForDeathCo());
    //    transform.localScale = _LerperInstance._ObjScaleAtSpawn;
    //    _CurrentHealth = _StartingHealth;
    //    SpawnManager.instance.PoolObject(gameObject);
    //    ScoreManager.instance.IterateBallsKoScore();
    //}
}