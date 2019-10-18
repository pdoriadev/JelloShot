
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// Moves and rotates touch visuals. To be attached to touch visuals. Subscribes to SlingShotMechanics' events. 
/// </summary>
public class SlingShotVisuals : MonoBehaviour
{
    public LineRenderer dragLineRend;
    public TrailRenderer dragLineTrail;
    public TrailRenderer lastTouchTrail;
    // Arrows rotating right around player ball
    public Transform ballArrowTransform;
    // Arrows rotating at First touch
    public Transform arrowsAtFirstTouchTransform;
    public Transform latestTouchVisualTransform;
    public Vector3 offScreenPosition;
    public float angleArrowIsPointing;

    public UnityEvent OnRelease;

    [SerializeField]
    private SlingShotMechanic _SlingShotInstance;
    #region UNITY CALLBACKS

    private void OnEnable()
    {
        if (_SlingShotInstance == null)
            _SlingShotInstance = GameObject.FindGameObjectWithTag("Player").GetComponent<SlingShotMechanic>();
        if (dragLineRend == null)
            dragLineRend = GameObject.FindGameObjectWithTag("dragLine").GetComponent<LineRenderer>();

        _SlingShotInstance.slingShotStartEvent += OnSlingStartListener;
        _SlingShotInstance.slingShotMovesEvent += OnSlingMoveListener;
        _SlingShotInstance.slingShotResetEvent += OnSlingResetListener;
    }

    private void OnDisable()
    {
        _SlingShotInstance.slingShotStartEvent -= OnSlingStartListener;
        _SlingShotInstance.slingShotMovesEvent -= OnSlingMoveListener;
        _SlingShotInstance.slingShotResetEvent -= OnSlingResetListener;
    }
    #endregion

    private bool areTrailsCleared = false;
    [SerializeField]
    private ParticleSystem _PSysOnStart;
    private void OnSlingStartListener(TouchInfo _touchInfo, SlingShotInfo _slingShotInfo)
    {
        _PSysOnStart.Play();
        areTrailsCleared = false;
    }



    // Moves sling visuals
    private void OnSlingMoveListener(TouchInfo _touchInfo, SlingShotInfo _slingShotInfo)
    {
        #region HANDLE DRAG LINE
        // set line end points positions
        Vector3 PosOffset = _slingShotInfo.slingerRigidbody.transform.position - _touchInfo.firstTouchPos;
        int MagnitudeOffset = 100;
        dragLineRend.SetPosition(0, _slingShotInfo.slingerRigidbody.transform.position);
        // clamps line from becoming too long
        dragLineRend.SetPosition(dragLineRend.positionCount - 1, Vector3.ClampMagnitude(_touchInfo.lastTouchPos + PosOffset, _slingShotInfo.slingMaxMagnitude - MagnitudeOffset));

        Vector3 lineEndsVector = dragLineRend.GetPosition(0) - dragLineRend.GetPosition(dragLineRend.positionCount - 1);
        // set each inner line point's position equidistant to each other with the correct offset.
        for (int i = 1; i < dragLineRend.positionCount - 1; i++)
        {
            dragLineRend.SetPosition(i , (dragLineRend.GetPosition(dragLineRend.positionCount - 1) + (lineEndsVector / dragLineRend.positionCount) * (i + 1)));
        }
        #endregion

        #region SET BALL ARROWS POSITION, ROTATION
        // set position for gameObjects rotating about player
        ballArrowTransform.position = _slingShotInfo.slingerRigidbody.transform.position;
        // ballArrowTransform and arrowsAtFirstTouchPosition's angle of rotation
        angleArrowIsPointing = Vector3.Angle(Vector3.up, _slingShotInfo.shotVelocity);

        // Need 2 ifs to calculate angle because Vector3.Angle only goes up to 180. 
        // Does not calculate angle going in one direction. Takes shortest distance from either side. 
        // if lastTouchPosition is right of firstTouchPosition, the arrowTransform's rotation changes to match
        if (_touchInfo.lastTouchPos.x > _touchInfo.firstTouchPos.x)
        {
            ballArrowTransform.eulerAngles = new Vector3(0, 0, angleArrowIsPointing);
            //arrowsAtFirstTouchTransform.eulerAngles = new Vector3(0, 0, angleArrowIsPointing);
            //latestTouchVisualTransform.eulerAngles = new Vector3(0, 0, angleArrowIsPointing);
        }

        if (_touchInfo.lastTouchPos.x <= _touchInfo.firstTouchPos.x)
        {
            ballArrowTransform.eulerAngles = new Vector3(0, 0, 360 - angleArrowIsPointing);
            //arrowsAtFirstTouchTransform.eulerAngles = new Vector3(0, 0, 360 - angleArrowIsPointing);
            //latestTouchVisualTransform.eulerAngles = new Vector3(0, 0, 360 - angleArrowIsPointing);
        }

        // Moves big arrows to first touch position
        arrowsAtFirstTouchTransform.position = _touchInfo.firstTouchPos;
        #endregion

        // Moves finger touch visual to the latest touch position
        latestTouchVisualTransform.position = _touchInfo.lastTouchPos;

        // set trail positions and clear any trail from their previous position
        //dragLineTrail.SetPosition(0, dragLineRend.GetPosition(dragLineRend.positionCount - 1));
        dragLineTrail.gameObject.transform.position = dragLineRend.GetPosition(dragLineRend.positionCount - 1);
        if (areTrailsCleared == false)
        {
            dragLineTrail.Clear();
            lastTouchTrail.Clear();
            areTrailsCleared = true;
        }
    }


    public void OnSlingResetListener()
    {
        // sets all offscreen visuals offscreen
        ballArrowTransform.position = offScreenPosition;
        arrowsAtFirstTouchTransform.position = offScreenPosition;
        latestTouchVisualTransform.position = offScreenPosition;
        if (dragLineTrail.positionCount > 0)
        {
            dragLineTrail.SetPosition(0, offScreenPosition);
        }
        dragLineTrail.Clear();
        lastTouchTrail.Clear();
        for (int i = 0; i < dragLineRend.positionCount; i++)
        {
            dragLineRend.SetPosition(i, offScreenPosition);
        }
        OnRelease.Invoke();
    }

    // IDEA: Effect where sling gradient flashes white and blue with same timing as other sling shot effect flashes. 
    // IDEA: Effect where, on release, the sling will zoom into its center point where the player was. When it reaches that point, an EXPLOSION or some other particle effect happens.
    //      Could use a coroutine. 
    
}