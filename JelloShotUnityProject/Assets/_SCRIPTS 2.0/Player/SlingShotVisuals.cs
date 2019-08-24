
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Moves and rotates touch visuals. To be attached to touch visuals. Called by player manager. 
/// </summary>
public class SlingShotVisuals : MonoBehaviour
{
    // Arrows rotating right around player
    public Transform ballArrowTransform;
    // Arrows rotating at First touch
    public Transform arrowsAtFirstTouchTransform;
    public Transform latestTouchVisualTransform;
    public Vector3 offScreenPosition;
    public float angleArrowIsPointing;

    [SerializeField]
    SlingShotMechanic _slingShotInstance;
    #region UNITY CALLBACKS

    private void OnEnable()
    {
        _slingShotInstance.slingShotMovesEvent += MoveVisualsOnSlingMove;
        _slingShotInstance.slingShotResetEvent += MoveVisualsOffScreen;
    }

    private void OnDisable()
    {
        _slingShotInstance.slingShotMovesEvent -= MoveVisualsOnSlingMove;
        _slingShotInstance.slingShotResetEvent -= MoveVisualsOffScreen;
    }
    #endregion 

    private void MoveVisualsOnSlingMove(TouchInfo _touchInfo, SlingShotInfo _slingShotInfo)
    {
        // arrows that rotate around player. 
        ballArrowTransform.position = _slingShotInfo.slingerRigidbody.transform.position;
        // ballArrowTransform and arrowsAtFirstTouchPosition's angle of rotation
        angleArrowIsPointing = Vector3.Angle(Vector3.up, _slingShotInfo.shotVelocity);

        // Need 2 statements to calculate angle because Vector3.Angle only goes up to 180. 
        // Does not calculate angle going in one direction. Takes shortest distance from either side. 
        // if lastTouchPosition is right of firstTouchPosition, the arrowTransform's rotation changes to match
        if (_touchInfo.lastTouchPos.x > _touchInfo.firstTouchPos.x)
        {
            ballArrowTransform.eulerAngles = new Vector3(0, 0, angleArrowIsPointing);
            arrowsAtFirstTouchTransform.eulerAngles = new Vector3(0, 0, angleArrowIsPointing);
            latestTouchVisualTransform.eulerAngles = new Vector3(0, 0, angleArrowIsPointing);
        }

        if (_touchInfo.lastTouchPos.x <= _touchInfo.firstTouchPos.x)
        {
            ballArrowTransform.eulerAngles = new Vector3(0, 0, 360 - angleArrowIsPointing);
            arrowsAtFirstTouchTransform.eulerAngles = new Vector3(0, 0, 360 - angleArrowIsPointing);
            latestTouchVisualTransform.eulerAngles = new Vector3(0, 0, 360 - angleArrowIsPointing);
        }

        // Moves finger touch visual to the latest touch position
        latestTouchVisualTransform.position = _touchInfo.lastTouchPos;
        // Moves big arrows to first touch position
        arrowsAtFirstTouchTransform.position = _touchInfo.firstTouchPos;
    }

    public void MoveVisualsOffScreen()
    {
        ballArrowTransform.position = offScreenPosition;
        arrowsAtFirstTouchTransform.position = offScreenPosition;
        latestTouchVisualTransform.position = offScreenPosition;
    }
}