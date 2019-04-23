using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Lerper))]
public class VelocityReflector : MonoBehaviour
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

    [SerializeField]
    private float _ReflectForce;
    [SerializeField]
    private int _StartingHealth = 3;
    [SerializeField]
    private int _CurrentHealth;
    // Reflects player off itself. Calls StartLerp().
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == (int)GameLayers.Player)
        {      
            _CurrentHealth--;
            if (_CurrentHealth < 1)
            {
                StartCoroutine(CheckForDeathCo());
                print("Health is 0");
            }

            _LerperInstance.StartLerp(_CurrentHealth, _StartingHealth);     
        }
    }

    IEnumerator CheckForDeathCo()
    {
        bool _isChecking = true;
        while(_isChecking == true)
        {
            yield return new WaitForSeconds(0.01f);
            if (_CurrentHealth < 1 && _LerperInstance._IsLerping == false)
                OnDeath();
        }

        yield return null;
    }

    public void OnDeath()
    {
        print("Died at " + _CurrentHealth + " Health");
        StopCoroutine(CheckForDeathCo());
        transform.localScale = _LerperInstance._ObjScaleAtSpawn;
        _CurrentHealth = _StartingHealth;
        SpawnManager.instance.PoolObject(gameObject);
        ScoreManager.instance.IterateBallsKoScore();
    }
}