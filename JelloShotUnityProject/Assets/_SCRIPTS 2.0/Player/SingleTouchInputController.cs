using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Checks for touch inputs and passes relevant input data through OnTouchInput event as a struct argument. 

public enum TouchInputState
{
    AtRest, // 0
    BeginningTap, // 1
    Dragging, // 2
    Release // 3
}

public class SingleTouchInputController : MonoBehaviour
{
    public static SingleTouchInputController instance;

    // Touch Input Event
    public delegate void OnTouchInput(TouchInfo _touchInfo);
    public static event OnTouchInput OnTouchInputEvent;

    // Camera related
    private Camera _TouchCamera;
    private float _ScreenWidth;

    private Touch _LatestTouch;

    #region UNITY CALLBACKS
    private void OnEnable()
    {
        if (instance == null)
            instance = this;

        _TouchInfo = new TouchInfo(_LatestTouch);
        //_TouchCamera = CameraController.instance.mainCamera;
        _ScreenWidth = Screen.width;
    }
    private void OnDisable()
    {
        instance = null;
    }
    #endregion



    private void Update()
    {
#if UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS
        TouchMovementControls();
#endif
    }

    private TouchInfo _TouchInfo;

    // Controls Input States
    private void TouchMovementControls()
    {
        if (Input.touchCount > 0)
        {
            _LatestTouch = Input.GetTouch(0);
            _LatestTouch.position = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

            // Create drag anchor at tap position. Slow Down time.
            if (_LatestTouch.phase == TouchPhase.Began)
            {
                _TouchInfo = new TouchInfo(_LatestTouch);
                _TouchInfo.touchState = TouchInputState.BeginningTap;
                
                OnTouchInputEvent(_TouchInfo);
            }

            // Drag circle to latest touch position. 
            if (_LatestTouch.phase == TouchPhase.Stationary || _LatestTouch.phase == TouchPhase.Moved)
            {
                _TouchInfo.touchState = TouchInputState.Dragging;

                _TouchInfo.lastTouchPos = _LatestTouch.position;

                // Reference firstTouchPosition and lastTouchPosition to get dragVector and dragDistance.
                _TouchInfo.dragVector = new Vector3(_TouchInfo.firstTouchPos.x - _TouchInfo.lastTouchPos.x, _TouchInfo.firstTouchPos.y - _TouchInfo.lastTouchPos.y);
                // Can change this to square magnitude method
                _TouchInfo.dragDistance = Vector3.Distance(_TouchInfo.firstTouchPos, _TouchInfo.lastTouchPos);

                OnTouchInputEvent(_TouchInfo);
            }

            // Multiplies dragVector, dragDist, and slingShotForce to get shotVelocity. 
            // Applies shotVelocity to player rigidbody at transform.position. Speeds up timeScale. 
            else if (_LatestTouch.phase == TouchPhase.Ended)
            {
                _TouchInfo.touchState = TouchInputState.Release;
                OnTouchInputEvent(_TouchInfo);

                Reset();
            }

            void Reset()
            {
                _TouchInfo.touchState = TouchInputState.AtRest;
                OnTouchInputEvent(_TouchInfo);
            }
        }
    }
}

public struct TouchInfo
{
    public Vector3 firstTouchPos;
    public Vector3 lastTouchPos;
    public Vector3 dragVector;
    public float dragDistance;
    public TouchInputState touchState;

    public TouchInfo(Touch _touch)
    {
        firstTouchPos = _touch.position;
        lastTouchPos = _touch.position;
        dragVector = firstTouchPos - lastTouchPos;
        dragDistance = 0;
        touchState = TouchInputState.BeginningTap;
    }
}