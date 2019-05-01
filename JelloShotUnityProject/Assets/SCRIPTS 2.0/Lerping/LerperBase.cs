using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Includes basic lerping functionality. Specific values to lerp are provided by concrete classes implementing LerperBase. 
/// Data: Can it lerp, is it lerping, for how long, and what is the state of the lerp. 
/// Behavior: Handles lerp.
/// </summary>
public abstract class LerperBase : MonoBehaviour
{
    #region LERP VARIABLES
    [SerializeField]
    private bool _CanLerp = true;
    [SerializeField]
    private bool _IsLerping = false;
    [SerializeField]
    private float _LerpPercentageComplete;
    [HeaderAttribute("Size")]
    //[SerializeField]
    //private Vector2 _ObjScaleAtSpawn;
    //[SerializeField]
    //private Vector2 _ObjStartLerpScale;
    //[SerializeField]
    //private Vector2 _ObjNextScale;
    [HeaderAttribute("Time")]
    [SerializeField]
    private float _LerpTime = 0.5f;
    [SerializeField]
    private float _StartLerpTime;
    [SerializeField]
    private float _TimeSinceLerpStarted;

    [HideInInspector]
    public float lerpTime
    {
        get { return _LerpTime; }
        set
        {
            if (value <= 0)
            {
                _LerpTime = value;
                lerpTime = _LerpTime;
            }

            if (_LerpTime <= 0)
            {
                _LerpTime = 0.4f;
                lerpTime = _LerpTime;
            }
        }
    }
    #endregion 

    public void StartLerp(float _currentHealth, float _startingHealth)
    {
        if (_CanLerp)
        {
            _IsLerping = true;
            _LerpPercentageComplete = 0.0f;
            _StartLerpTime = Time.time;

            _ObjStartLerpScale = transform.localScale;

            float _ScaleDifference = (_currentHealth + 0.1f) / _startingHealth;
            _ObjNextScale = new Vector2(_ObjScaleAtSpawn.x * _ScaleDifference, _ObjScaleAtSpawn.y * _ScaleDifference);
        }

        // else throw exception here for if _CanLerp is false
    }

    void FixedUpdate()
    {
        if (_IsLerping == true && _LerpPercentageComplete <= 1.0f)
        {

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
        transform.localScale = Vector2.Lerp(_ObjStartLerpScale, _ObjNextScale, _LerpPercentageComplete);
    }

    protected virtual void EndLerp()
    {
        _IsLerping = false;
    }
}

public struct LerpData
{

}
