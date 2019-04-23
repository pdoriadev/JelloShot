using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Lerper : MonoBehaviour
{
    private void OnEnable()
    {
        _ObjScaleAtSpawn = transform.localScale;
    }

    #region LERP VARIABLES
    public bool _IsLerping = false;
    public Vector2 _ObjScaleAtSpawn;

    [SerializeField]
    private float _TimeTakenToLerp = 0.5f;
    [SerializeField]
    private float _StartLerpTime;
    [SerializeField]
    private float _TimeSinceLerpStarted;
    [SerializeField]
    private float _LerpPercentageComplete;
    [SerializeField]
    private Vector2 _ObjStartLerpScale;
    [SerializeField]
    private Vector2 _ObjNextScale;
    #endregion 

    public void StartLerp(float currentHealth, float startingHealth)
    {
        _IsLerping = true;
        _LerpPercentageComplete = 0.0f;
        _StartLerpTime = Time.time;

        _ObjStartLerpScale = transform.localScale;

        float _ScaleDifference = (currentHealth + 0.1f) / startingHealth;
        _ObjNextScale = new Vector2(_ObjScaleAtSpawn.x * _ScaleDifference, _ObjScaleAtSpawn.y * _ScaleDifference);
    }

    void FixedUpdate()
    {
        if (_IsLerping == true && _LerpPercentageComplete <= 1.0f)
        {
            //StartCoroutine(TestCo());
            _TimeSinceLerpStarted = Time.time - _StartLerpTime;
            _LerpPercentageComplete = _TimeSinceLerpStarted / _TimeTakenToLerp;
            transform.localScale = Vector2.Lerp(_ObjStartLerpScale, _ObjNextScale, _LerpPercentageComplete);
        }

        else if (_IsLerping == true && _LerpPercentageComplete >= 1.0f)
        {
            _IsLerping = false;
            //StopCoroutine(TestCo());
        }
    }
}
//    IEnumerator TestCo()
//    {
//        while (_IsLerping == true)
//        {
//            yield return new WaitForSeconds(_TimeTakenToLerp - 0.1f);
//            print("Lerping");
//        }

//        yield return null;
//    }

