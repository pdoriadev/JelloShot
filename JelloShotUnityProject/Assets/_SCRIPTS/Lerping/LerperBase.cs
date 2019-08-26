using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Includes basic lerping functionality. Specific values to lerp are provided by concrete classes implementing LerperBase. 
/// Data: Can it lerp, is it lerping, for how long, and what is the state of the lerp. 
/// Behavior: Handles above lerp data. Provides virtual functions for child classes. 
/// </summary>
public abstract class LerperBase : MonoBehaviour
{
    #region LERP VARIABLES
    [SerializeField]
    private bool _CanLerp = true;
    [HideInInspector]
    public bool canLerp { get { return _CanLerp; } }
    [SerializeField]
    private bool _IsLerping = false;
    public bool isLerping
    {
        get { return _IsLerping; }
        set { _IsLerping = value; }
    }
    [SerializeField]
    private float _LerpPercentageComplete;
    [HideInInspector]
    public float lerpPercentageComplete { get { return _LerpPercentageComplete; } }
    [HeaderAttribute("Time")]
    [SerializeField]
    private float _StartLerpTime;
    [SerializeField]
    private float _TimeSinceLerpStarted;
    [SerializeField]
    private float _LerpTime = 0.5f;
    [HideInInspector]
    public float lerpTime
    {
        get { return _LerpTime; }
        set
        {
            _LerpTime = value;

            if (_LerpTime <= 0)
            {
                _LerpTime = 0.2f;
            }
        }
    }
    #endregion 

    // params are used in child class overrides
    public virtual void StartLerp(float _scalarNumerator, float _scalarDenominator)
    {
        if (_CanLerp)
        {
            _IsLerping = true;
            _LerpPercentageComplete = 0.0f;
            _StartLerpTime = Time.time;
        }

        else { Debug.LogError("_CanLerp = false on derived LerperBase class, but StartLerp() was called."); }
    }

    void FixedUpdate()
    {
        if (_IsLerping == true && _LerpPercentageComplete <= 1.0f)
        {
            HandleLerp();
        }

        else if (_IsLerping == true && _LerpPercentageComplete >= 1.0f)
        {
            EndLerp();
        }
    }

    protected virtual void HandleLerp()
    {
        _TimeSinceLerpStarted = Time.time - _StartLerpTime;
        _LerpPercentageComplete = _TimeSinceLerpStarted / _LerpTime;
    }

    protected virtual void EndLerp()
    {
        _IsLerping = false;
    }
}