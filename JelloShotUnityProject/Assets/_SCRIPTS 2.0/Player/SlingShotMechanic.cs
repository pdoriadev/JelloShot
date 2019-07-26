using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Write the script description here

[RequireComponent(typeof(Rigidbody2D))]
public class SlingShotMechanic : MonoBehaviour
{
    public static GameObject playerGOInstance;

    #region PUBLIC VARIABLES
    [Space(10)]
    public Vector2 offScreenPosition;
    public Vector2 shotVelocity;
    public Vector2 newBallVelocity;

    [Space(10)]
    public float dragShotForce = 0;
    public float shotVelocityMaxMagnitude = 0;
    [Space(10)]
    public float fastTimeScale = 1.25f;
    public float regularTimeScale = 1f;
    public float slowTimeScale = 0.25f;
    #endregion

    #region PRIVATE VARIABLES
    [Space(10)]
    [SerializeField]
    private float _BallSpeedMultiplier = 1;
    // Rigidbody of ball that player is colliding with.
    private Rigidbody2D _CollidedRigidBody;
    private Rigidbody2D playerRigidbody;
    #endregion

    #region UNITY CALLBACKS
    private void OnEnable()
    {
        // If no player in scene
        if (playerGOInstance == null)
            playerGOInstance = gameObject;

        playerRigidbody = GetComponent<Rigidbody2D>();

        SingleTouchInputController.OnTouchInputEvent += OnTouchInputObserver;
    }

    private void OnDisable()
    {
        playerGOInstance = null;
    }

    private void FixedUpdate()
    {
        playerRigidbody.velocity = Vector2.ClampMagnitude(playerRigidbody.velocity, shotVelocityMaxMagnitude);
    }
    #endregion
    private void OnTouchInputObserver(TouchInfo _touchInfo)
    {
        // Controls 3 phases of touch movement
#if UNITY_STANDALONE || UNITY_ANDROID
        if (GameManager.instance.state == GameState.Gameplay)
        {
            // Create drag anchor at tap position. Slow Down time.
            if (_touchInfo.touchState == TouchInputState.BeginningTap)
            {
                Time.timeScale = slowTimeScale;
            }

            // Drag circle to latest touch position. 
            if (_touchInfo.touchState == TouchInputState.Dragging)
            {
                shotVelocity = new Vector2(_touchInfo.dragVector.x * _touchInfo.dragDistance * dragShotForce, _touchInfo.dragVector.y * _touchInfo.dragDistance * dragShotForce);
                shotVelocity = Vector2.ClampMagnitude(shotVelocity, shotVelocityMaxMagnitude);

                SlingShotVisuals.instance.MoveTouchVisuals(transform.position, shotVelocity, _touchInfo.firstTouchPos, _touchInfo.lastTouchPos);
            }

            // Multiplies dragVector, dragDist, and slingShotForce to get shotVelocity. 
            // Applies shotVelocity to player rigidbody at transform.position. Speeds up timeScale. 
            else if (_touchInfo.touchState == TouchInputState.Release)
            {
                MinMaxVelocity();

                playerRigidbody.AddForceAtPosition(shotVelocity, transform.position, ForceMode2D.Impulse);

                Time.timeScale = fastTimeScale;

                Reset();
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

            // Moves touch position visuals off screen.
            void Reset()
            {
                SlingShotVisuals.instance.MoveVisualsOffScreen();
            }
        }
#endif
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Applies force to ball player collides with
        if (collision.gameObject.layer == (int)GameLayers.BallsLayer)
        {
            Rigidbody2D ballRigidBody = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 newBallVelocity = new Vector2((ballRigidBody.velocity.x + (playerRigidbody.velocity.x * _BallSpeedMultiplier)), (ballRigidBody.velocity.y + (playerRigidbody.velocity.y * _BallSpeedMultiplier)));

            newBallVelocity = Vector2.ClampMagnitude(newBallVelocity, BallVelocityLimiter.instance.ballVelocityMagnitudeCap);

            //Velocity added to collided with ball is proportional to player's current velocity.
            ballRigidBody.AddForceAtPosition(newBallVelocity, collision.gameObject.transform.position, ForceMode2D.Impulse);

            Time.timeScale = regularTimeScale;
        }

        BounceAnimation.instance.PlayBounce();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        BounceAnimation.instance.PlayerBounceExitClip(playerRigidbody.velocity);
        playerRigidbody.velocity = Vector2.ClampMagnitude(playerRigidbody.velocity, shotVelocityMaxMagnitude);
    }
}

