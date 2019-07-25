using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(DragReleaseStateHandler))]
public class DragReleaseInputController : MonoBehaviour
{
    public static DragReleaseInputController instance;

    // Touch Input Event
    public delegate void OnTouchInput(TouchInfo _touchInfo);
    public static event OnTouchInput OnTouchInputEvent;

    // Camera related
    private Camera _TouchCamera;
    private float _ScreenWidth;

    // Touch info
    DragReleaseStateHandler _MyDragReleaseStateHandler;
    private Touch _LatestTouch;

    #region UNITY CALLBACKS
    private void OnEnable()
    {
        if (instance == null)
            instance = this;

        _TouchCamera = CameraController.instance.mainCamera;
        _ScreenWidth = Screen.width;
        _MyDragReleaseStateHandler = GetComponent<DragReleaseStateHandler>();
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
            _LatestTouch.position = _TouchCamera.ScreenToWorldPoint(Input.GetTouch(0).position);

            // Create drag anchor at tap position. Slow Down time.
            if (_LatestTouch.phase == TouchPhase.Began)
            {
                _MyDragReleaseStateHandler.slingshotState = TouchInputState.BeginningTap;
                _TouchInfo = new TouchInfo(_LatestTouch);
                OnTouchInputEvent(_TouchInfo);
            }

            // Drag circle to latest touch position. 
            if (_LatestTouch.phase == TouchPhase.Stationary || _LatestTouch.phase == TouchPhase.Moved)
            {
                _MyDragReleaseStateHandler.slingshotState = TouchInputState.Dragging;

                _LastTouchPosition = _LatestTouch.position;

                // Reference firstTouchPosition and lastTouchPosition to get dragVector and dragDistance.
                _TouchInfo._DragVector = new Vector3(_FirstTouchPosition.x - _LastTouchPosition.x, _FirstTouchPosition.y - _LastTouchPosition.y);
                _TouchInfo._DragDistance = Vector3.Distance(_FirstTouchPosition, _LastTouchPosition);

                OnTouchInputEvent(_TouchInfo);
            }

            // Multiplies dragVector, dragDist, and slingShotForce to get shotVelocity. 
            // Applies shotVelocity to player rigidbody at transform.position. Speeds up timeScale. 
            else if (_LatestTouch.phase == TouchPhase.Ended)
            {
                _MyDragReleaseStateHandler.slingshotState = TouchInputState.Release;
                OnTouchInputEvent(_TouchInfo);

                Reset();
            }

            void Reset()
            {
                _MyDragReleaseStateHandler.slingshotState = TouchInputState.AtRest;
                OnTouchInputEvent(_TouchInfo);
            }
        }
    }
}

public struct TouchInfo
{
    public Vector3 _FirstTouchPos;
    public Vector3 _LastTouchPos;
    public Vector3 _DragVector;
    public float _DragDistance;
    public TouchInputState _TouchState;

    public TouchInfo(Touch _touch)
    {
        _FirstTouchPos = _touch.position;
        _LastTouchPos = _touch.position;
        _DragVector = _FirstTouchPos - _LastTouchPos;
        _DragDistance = 0;
        _TouchState = TouchInputState.BeginningTap;
    }
}