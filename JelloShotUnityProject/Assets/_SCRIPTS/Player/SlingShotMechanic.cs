using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// - Controls slingshot physics and listens to SingleTouchInputController's events to know when to enter each phase of the slingshot. 
/// - Has events for when the slingshot starts, when it changes position (pre-release), and when it is released. 
/// - Clamps player rigidbody's velocity so (1) it will never exceed the max velocity and (2) whenever the player shoots the slingshot, 
///     it will always be at least a minimum velocity. 
/// - Adds additional force to each enemy the player collides with. 
/// - Slows down time when slingshot is pulled back. Resets time upon slingshot release. 
/// - SlingShotInfo struct at bottom of struct for events to pass relevant info. 
/// - Calls animation clips in BounceAnimation script on OnCollisionEnter and OnCollisionExit
/// 
/// If I have time, I'd like to separate some of this functionality, some which can be easily done, but for the time being, it works.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class SlingShotMechanic : MonoBehaviour
{
    #region SLING SHOT EVENTS

    public delegate void SlingShotStart(TouchInfo _touchInfo, SlingShotInfo _slingShotInfo);
    public event SlingShotStart slingShotStartEvent;
    public delegate void SlingShotMoves(TouchInfo _touchInfo, SlingShotInfo _slingShotInfo);
    public event SlingShotMoves slingShotMovesEvent;
    public delegate void SlingShotReset();
    public event SlingShotReset slingShotResetEvent;
    public delegate void PlayerCollides();
    public event PlayerCollides playerCollidesOnEnemy;
    public event PlayerCollides playerCollidesOnFriendly;
    
    #endregion

    #region PUBLIC VARIABLES

    [Space(10)]
    public Vector3 shotVelocity;
    public float shotVelocityMaxMagnitude = 0;
    public float VelocityMultiplier = 0;

    [Space(10)]
    public Vector3 playerReboundVelocity;

    [Space(10)]
    public float fastTimeScale = 1.25f;
    public float regularTimeScale = 1f;
    public float slowTimeScale = 0.25f;

    #endregion

    #region PRIVATE VARIABLES

    [Space(10)]
    [SerializeField]
    private float _OtherBallSpeedMultip = 1;
    // Rigidbody of ball that player is colliding with.
    private Rigidbody2D _CollidedRigidBody;
    private Rigidbody2D _PlayerRigidbody;
    private SlingShotInfo _SlingShotInfo;

    #endregion

    #region UNITY CALLBACKS

    private void OnEnable()
    {
        _PlayerRigidbody = GetComponent<Rigidbody2D>();
        _SlingShotInfo = new SlingShotInfo(_PlayerRigidbody, shotVelocity, shotVelocityMaxMagnitude );

        SingleTouchInputController.OnTouchInputEvent += OnTouchInputObserver;
        GameManager.onResetLevel += ResetSlingShot;
    }

    private void OnDisable()
    {
        SingleTouchInputController.OnTouchInputEvent -= OnTouchInputObserver;
        GameManager.onResetLevel -= ResetSlingShot;
    }

    private void FixedUpdate()
    {
        _PlayerRigidbody.velocity = Vector2.ClampMagnitude(_PlayerRigidbody.velocity, shotVelocityMaxMagnitude);
    }
    #endregion

    #region PRIVATE SLING SHOT METHODS
    private void OnTouchInputObserver(TouchInfo _touchInfo)
    {
        // Controls 3  phases of touch movement
#if UNITY_STANDALONE || UNITY_ANDROID
        if (GameManager.instance.state != GameState.LevelEnd || GameManager.instance.state != GameState.MainMenu)
        {
            // Create drag anchor at tap position. Slow Down time.
            if (_touchInfo.touchState == TouchInputState.BeginningTap)
            {
                Time.timeScale = slowTimeScale;
                if (slingShotStartEvent != null)
                {
                    slingShotStartEvent(_touchInfo, _SlingShotInfo);
                }
            }

            // Drag circle to latest touch position. 
            if (_touchInfo.touchState == TouchInputState.Dragging)
            {
                shotVelocity = new Vector3(_touchInfo.dragVector.x * _touchInfo.dragDistance * VelocityMultiplier, _touchInfo.dragVector.y * _touchInfo.dragDistance * VelocityMultiplier);
                shotVelocity = Vector3.ClampMagnitude(shotVelocity, shotVelocityMaxMagnitude);

                _SlingShotInfo.shotVelocity = shotVelocity;
                if (slingShotMovesEvent != null)
                {
                    slingShotMovesEvent(_touchInfo, _SlingShotInfo);
                }
            }

            // Multiplies dragVector, dragDist, and slingShotForce to get shotVelocity. 
            // Applies shotVelocity to player rigidbody at transform.position. Speeds up timeScale. 
            else if (_touchInfo.touchState == TouchInputState.Release)
            {
                MinMaxVelocity();

                _PlayerRigidbody.AddForceAtPosition(shotVelocity, transform.position, ForceMode2D.Impulse);

                Time.timeScale = fastTimeScale;

                ResetSlingShot();
            }

            // if shot velocity is slower than min/max velocities, shotVelocity = appropriate min/max velocities
            void MinMaxVelocity()
            {
                if (shotVelocity.x > 0)
                    shotVelocity.x = Mathf.Max(shotVelocity.x, 10);

                if (shotVelocity.x < 0)
                    shotVelocity.x = Mathf.Min(shotVelocity.x, -10);

                if (shotVelocity.y > 0)
                    shotVelocity.y = Mathf.Max(shotVelocity.y, 10);

                if (shotVelocity.y < 0)
                    shotVelocity.y = Mathf.Min(shotVelocity.y, -10);
            }
        }
#endif
    }

    private void ResetSlingShot()
    {
        if (slingShotResetEvent != null)
            slingShotResetEvent();
        else Debug.LogWarning(slingShotResetEvent + " is null");
    }

    #endregion

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<DamagerPlayerFaction>() != null)
        {
            playerCollidesOnFriendly();
        }
        else
        {

            playerCollidesOnEnemy();
        }

        // Applies force to ball player collides with
        if (collision.gameObject.layer == (int)GameLayers.BallsLayer)
        {
            Rigidbody2D ballRigidBody = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector3 newBallVelocity = new Vector3((ballRigidBody.velocity.x + (_PlayerRigidbody.velocity.x * _OtherBallSpeedMultip)), (ballRigidBody.velocity.y + (_PlayerRigidbody.velocity.y * _OtherBallSpeedMultip)));

            //newBallVelocity = Vector3.ClampMagnitude(newBallVelocity, BallVelocityLimiter.instance.ballVelocityMagnitudeCap);

            //Velocity added to collided with ball is proportional to player's current velocity.
            ballRigidBody.AddForceAtPosition(newBallVelocity, collision.gameObject.transform.position, ForceMode2D.Impulse);
        }

        BounceAnimation.instance.PlayBounce();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        BounceAnimation.instance.PlayerBounceExitClip(_PlayerRigidbody.velocity);
        _PlayerRigidbody.velocity = Vector3.ClampMagnitude(_PlayerRigidbody.velocity, shotVelocityMaxMagnitude);
    }
}

public struct SlingShotInfo
{
    public Rigidbody2D slingerRigidbody;
    public Vector3 shotVelocity;
    public float slingMaxMagnitude;

    public SlingShotInfo(Rigidbody2D _slingerRB, Vector3 _shotVel, float _slingShotMaxMagnitude)
    {
        slingerRigidbody = _slingerRB;
        shotVelocity = _shotVel;
        slingMaxMagnitude = _slingShotMaxMagnitude;
    }
}
