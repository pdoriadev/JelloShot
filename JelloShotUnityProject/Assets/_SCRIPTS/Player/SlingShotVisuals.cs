
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// Moves and rotates touch visuals. To be attached to touch visuals. Subscribes to SlingShotMechanics' events. . 
/// </summary>
public class SlingShotVisuals : MonoBehaviour
{
    public LineRenderer lineRend;
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
        if (lineRend == null)
            lineRend = GameObject.FindGameObjectWithTag("dragLine").GetComponent<LineRenderer>();

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

    [SerializeField]
    private ParticleSystem _PSysOnStart;
    private void OnSlingStartListener(TouchInfo _touchInfo, SlingShotInfo _slingShotInfo)
    {
        _PSysOnStart.Play();
    }

    // Moves sling visuals
    private void OnSlingMoveListener(TouchInfo _touchInfo, SlingShotInfo _slingShotInfo)
    {
        // set position for gameObjects rotating about player
        ballArrowTransform.position = _slingShotInfo.slingerRigidbody.transform.position;


        Vector3 PosOffset = _slingShotInfo.slingerRigidbody.transform.position - _touchInfo.firstTouchPos;
        int MagnitudeIncrease = 100;
        // line end points positions
        lineRend.SetPosition(0, _slingShotInfo.slingerRigidbody.transform.position);
        lineRend.SetPosition(lineRend.positionCount - 1, Vector3.ClampMagnitude(_touchInfo.lastTouchPos + PosOffset, _slingShotInfo.slingMaxMagnitude - MagnitudeIncrease));
        //if ((_touchInfo.lastTouchPos + PosOffset).magnitude > _slingShotInfo.slingMaxMagnitude)
        //{
        //    lineRend.SetPosition(lineRend.positionCount - 1, Vector3.ClampMagnitude(_touchInfo.lastTouchPos + PosOffset, _slingShotInfo.slingMaxMagnitude + MagnitudeIncrease));
        //}
        Vector3 lineEndsOffset = lineRend.GetPosition(0) - lineRend.GetPosition(lineRend.positionCount - 1);
        // set each inner line point's position equidistant to each other with the correct offset.
        for (int i = 1; i < lineRend.positionCount - 1; i++)
        {
            lineRend.SetPosition(i , (lineRend.GetPosition(lineRend.positionCount - 1) + (lineEndsOffset / lineRend.positionCount) * (i + 1)));
        }

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

        // Moves finger touch visual to the latest touch position
        latestTouchVisualTransform.position = _touchInfo.lastTouchPos;
        // Moves big arrows to first touch position
        arrowsAtFirstTouchTransform.position = _touchInfo.firstTouchPos;
    }


    public void OnSlingResetListener()
    {
        ballArrowTransform.position = offScreenPosition;
        arrowsAtFirstTouchTransform.position = offScreenPosition;
        latestTouchVisualTransform.position = offScreenPosition;
        for (int i = 0; i < lineRend.positionCount; i++)
        {
            lineRend.SetPosition(i, offScreenPosition);
        }
        OnRelease.Invoke();
    }

    // IDEA: Dont' want another line/trail where the player is touching the screen. Can distract from actual sling. BUT could have 'sparkle' effect wherever they 
    //      last touched it. 
    // IDEA: Effect where sling gradient flashes white and blue with same timing as other sling shot effect flashes. 
    // IDEA: Effect where, on release, the sling will zoom into its center point where the player was. When it reaches that point, an EXPLOSION or some other particle effect happens.
    //      Could use a coroutine. 
    
}