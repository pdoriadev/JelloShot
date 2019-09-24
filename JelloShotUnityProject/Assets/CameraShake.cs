using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // reference to events
    public SlingShotMechanic slingShot;
    public DamageableBase damageable;
    #region UNITY CALLBACKS
    private void OnEnable()
    {
        if (slingShot == null)
        {
            slingShot = GameObject.FindGameObjectWithTag("Player").GetComponent<SlingShotMechanic>();
        }
        if (damageable == null)
        {
            damageable = GameObject.FindGameObjectWithTag("ground").GetComponent<DamageableBase>();
        }

        slingShot.slingShotResetEvent += ShotReleaseListener;
        slingShot.playerCollidesOnFriendly += LowIntensityShake;
        slingShot.playerCollidesOnEnemy += HighIntensityShake;
        damageable.onTakeDamageEvent += OnTakeDamageListener;

        if (_CamTransform == null)
            _CamTransform = gameObject.transform;
        _CamOGPos = _CamTransform.position;
    }
    private void OnDisable()
    {
        slingShot.slingShotResetEvent -= ShotReleaseListener;
        slingShot.playerCollidesOnFriendly -= LowIntensityShake;
        slingShot.playerCollidesOnEnemy -= HighIntensityShake;
        damageable.onTakeDamageEvent -= OnTakeDamageListener;
    }
    #endregion

    #region SHAKE CONTROL VARIABLES
    [SerializeField]
    private float _MaxDuration = 0.5f;
    [SerializeField]
    private float _Duration;
    [SerializeField]
    private float _DampingSpeed = 0.1f;
    [SerializeField]
    private float _LowMagnitude = 0.4f;
    [SerializeField]
    private float _RegularMagnitude = 1f;
    [SerializeField]
    private float _HighMagnitude = 2f;
    [SerializeField]
    private bool _Shaking = true;
    #endregion

    private float _Magnitude = 1f;
    private Transform _CamTransform;
    private Vector3 _CamOGPos;

    private void FixedUpdate()
    {
        if (_Shaking)
        {
            if (_Duration > 0)
            {
                _CamTransform.position = _CamOGPos + Random.insideUnitSphere * _Magnitude;
                _Duration -= Time.deltaTime * _DampingSpeed;
            }
            else if (_Duration < 0)
            {
                _CamTransform.position = _CamOGPos;
                _Duration = _MaxDuration;
                _Shaking = false;
            }
        }
    }
    private void ShotReleaseListener()
    {
        RegularIntensityShake();
    }
    private void OnTakeDamageListener(DamagedInfo _info)
    {
        HighIntensityShake();
    }

    private void LowIntensityShake()
    {
        _Magnitude = _LowMagnitude;
        _Shaking = true;
    }
    private void RegularIntensityShake()
    {
        _Magnitude = _RegularMagnitude;
        _Shaking = true;
    }
    private void HighIntensityShake()
    {
        _Magnitude = _HighMagnitude;
        _Shaking = true;
    }
}
