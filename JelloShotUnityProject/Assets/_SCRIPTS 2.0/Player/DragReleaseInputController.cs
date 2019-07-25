using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(DragReleaseStateHandler))]
public class DragReleaseInputController : MonoBehaviour
{
    // Touch Input Event
    public delegate void OnTouchInput(Vector3 _dragVector, float _dragDistance, DragReleaseState _dragReleaseState);
    public static event OnTouchInput OnTouchInputEvent;

    // Camera related
    private Camera _TouchCamera;
    private float _ScreenWidth;


    // Touch info
    DragReleaseStateHandler _MyDragReleaseStateHandler;
    private Touch _LatestTouch;
    private Vector3 _FirstTouchPosition;
    private Vector3 _LastTouchPosition;

    // Drag info
    private float _DragDistance = 0;
    private Vector3 _DragVector;

    #region UNITY CALLBACKS
    private void OnEnable()
    {
        _TouchCamera = CameraController.instance.mainCamera;
        _ScreenWidth = Screen.width;
        _MyDragReleaseStateHandler = GetComponent<DragReleaseStateHandler>();
    }
    #endregion

    private void Update()
    {
#if UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS
        TouchMovementControls();
#endif
    }

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
                _MyDragReleaseStateHandler.slingshotState = DragReleaseState.BeginningTap;
                OnTouchInputEvent(_DragVector, _DragDistance, _MyDragReleaseStateHandler.slingshotState);
            }

            // Drag circle to latest touch position. 
            if (_LatestTouch.phase == TouchPhase.Stationary || _LatestTouch.phase == TouchPhase.Moved)
            {
                _MyDragReleaseStateHandler.slingshotState = DragReleaseState.WindUp;

                _LastTouchPosition = _LatestTouch.position;

                // Reference firstTouchPosition and lastTouchPosition to get dragVector and dragDistance.
                _DragVector = new Vector3(_FirstTouchPosition.x - _LastTouchPosition.x, _FirstTouchPosition.y - _LastTouchPosition.y);
                _DragDistance = Vector3.Distance(_FirstTouchPosition, _LastTouchPosition);

                OnTouchInputEvent(_DragVector, _DragDistance, _MyDragReleaseStateHandler.slingshotState);
            }

            // Multiplies dragVector, dragDist, and slingShotForce to get shotVelocity. 
            // Applies shotVelocity to player rigidbody at transform.position. Speeds up timeScale. 
            else if (_LatestTouch.phase == TouchPhase.Ended)
            {
                _MyDragReleaseStateHandler.slingshotState = DragReleaseState.Release;
                OnTouchInputEvent(_DragVector, _DragDistance, _MyDragReleaseStateHandler.slingshotState);

                Reset();
            }

            void Reset()
            {
                _MyDragReleaseStateHandler.slingshotState = DragReleaseState.AtRest;
                OnTouchInputEvent(_DragVector, _DragDistance, _MyDragReleaseStateHandler.slingshotState);
            }
        }
    }
}
