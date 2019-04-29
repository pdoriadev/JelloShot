using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SizeLerper : MonoBehaviour
{
    private void OnEnable()
    {
        _ObjScaleAtSpawn = transform.localScale;
    }

    private void OnDisable()
    {
        transform.localScale = _ObjScaleAtSpawn;
    }

    #region LERP VARIABLES
    [SerializeField]
    private bool _CanLerp = true;
    [SerializeField]
    private bool _IsLerping = false;
    [SerializeField]
    private float _TimeTakenToLerp = 0.5f;
    [SerializeField]
    private float _StartLerpTime;
    [SerializeField]
    private float _TimeSinceLerpStarted;
    [SerializeField]
    private float _LerpPercentageComplete;
    [SerializeField]
    private Vector2 _ObjScaleAtSpawn;
    [SerializeField]
    private Vector2 _ObjStartLerpScale;
    [SerializeField]
    private Vector2 _ObjNextScale;
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

/// <summary>
/// The time taken to move from the start to finish positions
/// </summary>
//public float timeTakenDuringLerp = 1f;

///// <summary>
///// How far the object should move when 'space' is pressed
///// </summary>
//public float distanceToMove = 10;

////Whether we are currently interpolating or not
//private bool _isLerping;

////The start and finish positions for the interpolation
//private Vector3 _startPosition;
//private Vector3 _endPosition;

////The Time.time value when we started the interpolation
//private float _timeStartedLerping;

///// <summary>
///// Called to begin the linear interpolation
///// </summary>
//void StartLerping()
//{
//    _isLerping = true;
//    _timeStartedLerping = Time.time;

//    //We set the start position to the current position, and the finish to 10 spaces in the 'forward' direction
//    _startPosition = transform.position;
//    _endPosition = transform.position + Vector3.forward * distanceToMove;
//}

//void Update()
//{
//    //When the user hits the spacebar, we start lerping
//    if (Input.GetKey(KeyCode.Space))
//    {
//        StartLerping();
//    }
//}

////We do the actual interpolation in FixedUpdate(), since we're dealing with a rigidbody
//void FixedUpdate()
//{
//    if (_isLerping)
//    {
//        //We want percentage = 0.0 when Time.time = _timeStartedLerping
//        //and percentage = 1.0 when Time.time = _timeStartedLerping + timeTakenDuringLerp
//        //In other words, we want to know what percentage of "timeTakenDuringLerp" the value
//        //"Time.time - _timeStartedLerping" is.
//        float timeSinceStarted = Time.time - _timeStartedLerping;
//        float percentageComplete = timeSinceStarted / timeTakenDuringLerp;

//        //Perform the actual lerping.  Notice that the first two parameters will always be the same
//        //throughout a single lerp-processs (ie. they won't change until we hit the space-bar again
//        //to start another lerp)
//        transform.position = Vector3.Lerp(_startPosition, _endPosition, percentageComplete);

//        //When we've completed the lerp, we set _isLerping to false
//        if (percentageComplete >= 1.0f)
//        {
//            _isLerping = false;
//        }
//    }
//}
