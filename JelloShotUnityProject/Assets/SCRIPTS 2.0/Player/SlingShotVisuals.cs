using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Moves and rotates touch visuals. To be attached to touch visuals. Called by player manager. 
/// </summary>
public class SlingShotVisuals : MonoBehaviour
{
    public static SlingShotVisuals instance;
    // Arrows rotating right around player
    public Transform ballArrowTransform;
    // Arrows rotating at First touch
    public Transform arrowsAtFirstTouchTransform;
    public Transform latestTouchVisualTransform;
    public Vector2 offScreenPosition;
    public float angleArrowIsPointing;

    private void OnEnable()
    {
        if (instance == null)
            instance = this;
    }

    private void OnDisable()
    {
        instance = null;
    }
    
    // NOTE: Maybe make struct of these values? 
    public void MoveTouchVisuals(Vector2 _playerPosition, Vector2 _shotVelocity, Vector2 _firstTouchPosition, Vector2 _lastTouchPosition) 
    {

        // arrows that rotate around player. 
        ballArrowTransform.position = _playerPosition;
        // ballArrowTransform and arrowsAtFirstTouchPosition's angle of rotation
        angleArrowIsPointing = Vector3.Angle(Vector3.up, _shotVelocity);

        // Need 2 statements to calculate angle because Vector3.Angle only goes up to 180. 
        // Does not calculate angle going in one direction. Takes shortest distance from either side. 
        // if lastTouchPosition is right of firstTouchPosition, the arrowTransform's rotation changes to match
        if (_lastTouchPosition.x > _firstTouchPosition.x)
        {
            ballArrowTransform.eulerAngles = new Vector3(0, 0, angleArrowIsPointing);
            arrowsAtFirstTouchTransform.eulerAngles = new Vector3(0, 0, angleArrowIsPointing);
            latestTouchVisualTransform.eulerAngles = new Vector3(0, 0, angleArrowIsPointing);
        }
            
        if (_lastTouchPosition.x <= _firstTouchPosition.x)
        {
            ballArrowTransform.eulerAngles = new Vector3(0, 0, 360 - angleArrowIsPointing);
            arrowsAtFirstTouchTransform.eulerAngles = new Vector3(0, 0, 360 - angleArrowIsPointing);
            latestTouchVisualTransform.eulerAngles = new Vector3(0, 0, 360 - angleArrowIsPointing);
        }

        // Moves finger touch visual to the latest touch position
        latestTouchVisualTransform.position = _lastTouchPosition;
        // Moves big arrows to first touch position
        arrowsAtFirstTouchTransform.position = _firstTouchPosition;
    }   

    public void MoveVisualsOffScreen()
    {
        ballArrowTransform.position = offScreenPosition;
        arrowsAtFirstTouchTransform.position = offScreenPosition;
        latestTouchVisualTransform.position = offScreenPosition;
    }
}
