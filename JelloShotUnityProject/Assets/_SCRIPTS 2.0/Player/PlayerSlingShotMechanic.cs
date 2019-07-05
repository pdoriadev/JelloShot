using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
/// <summary>
///  Manages player movement inputs and physics for both PC and Mobile. On collision with player, Handles other ball's velocity changes.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerSlingShotMechanic : MonoBehaviour
{
    public static PlayerSlingShotMechanic instance;
    public static GameObject playerGOInstance;

    #region PUBLIC VARIABLES
    [Space(10)]
    public Rigidbody2D playerRigidbody;

    [Space(10)]
    public SpriteRenderer playerSpriteRenderer;
    // Player sprite colors depending on gamestate.
    [SerializeField]
    private Color _BeforeHitColor, _AfterHitColor;
    private Color _PlayerSpriteColor;

    [Space(10)]
    public Vector2 offScreenPosition;
    public Vector2 shotVelocity;
    public Vector2 playerCurrentVelocity;
    public Vector2 newBallVelocity;
    [Space(10)]
    public float dragDistance = 0;
    public float dragShotForce = 0;
    public float shotVelocityMaxMagnitude = 0;
    [Space(10)]
    public float fastTimeScale = 1.25f;
    public float regularTimeScale = 1f;
    public float slowTimeScale = 0.25f;

    public bool hasShot = false;
    #endregion

    #region PRIVATE VARIABLES
    // PRIVATE VARIABLES
    [Space(10)]
    [SerializeField]
    private float _BallSpeedMultiplier = 1;
    private float _ScreenWidth; 
    // Rigidbody of ball that player is colliding with.
    private Rigidbody2D _BallRigidBody;

    private Touch _LatestTouch;
    private Vector2 _FirstTouchPosition;
    private Vector2 _LastTouchPosition;
    private Vector2 _DragVector;
    private Vector2 _PreviousDragVector;
    #endregion

    // Handles when script instance is enabled or disabled. 
    #region UNITY CALLBACKS

    // Called when monobehaviour has been enabled. Subscribes Handlers.
    private void OnEnable()
    {
        if (instance == null)
            instance = this;
        
        // If no player in scene
        if (playerGOInstance == null)
            playerGOInstance = gameObject;

        GameManager.OnFixedUpdateEvent += OnFixedUpdateHandler;
        GameManager.OnUpdateEvent += OnUpdateHandler;

        _ScreenWidth = Screen.width;

        playerCurrentVelocity = playerRigidbody.velocity;

    }

    // Called when monobehaviour has been disabled or attached object is destroyed. Unsubscribes Handlers.
    private void OnDisable()
    {
        GameManager.OnFixedUpdateEvent -= OnFixedUpdateHandler;
        GameManager.OnUpdateEvent -= OnUpdateHandler;
        playerGOInstance = null;
        instance = null;
    }

#endregion

    #region HANDLERS
    private void OnUpdateHandler()
    {
        MovementControls();
        
        if (hasShot == true)
            BeforeHit();

        else
            AfterHit();
    }

    private void OnFixedUpdateHandler()
    {
        playerCurrentVelocity = playerRigidbody.velocity;
        playerRigidbody.velocity = Vector2.ClampMagnitude(playerRigidbody.velocity, shotVelocityMaxMagnitude);
    }
    #endregion

    #region METHODS FOR BOTH PC AND MOBILE
    private void BeforeHit()
    {
        _PlayerSpriteColor = playerSpriteRenderer.color;
        _PlayerSpriteColor = _BeforeHitColor;
        playerSpriteRenderer.color = _PlayerSpriteColor;
    }

    private void AfterHit()
    {
        _PlayerSpriteColor = _AfterHitColor;
        playerSpriteRenderer.color = _PlayerSpriteColor;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Applies force to ball player collides with
        if (collision.gameObject.layer == (int)GameLayers.BallsLayer)
        {
            Rigidbody2D ballRigidBody = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 newBallVelocity = new Vector2((ballRigidBody.velocity.x + (playerCurrentVelocity.x * _BallSpeedMultiplier)), (ballRigidBody.velocity.y + (playerCurrentVelocity.y * _BallSpeedMultiplier)));

            newBallVelocity = Vector2.ClampMagnitude(newBallVelocity, BallVelocityLimiter.instance.ballVelocityMagnitudeCap);

            //Velocity added to collided with ball is proportional to player's current velocity.
            ballRigidBody.AddForceAtPosition(newBallVelocity, collision.gameObject.transform.position, ForceMode2D.Impulse);

            Time.timeScale = regularTimeScale;
        }
        
        else if (collision.gameObject.tag == ("ground") || collision.gameObject.tag == ("wall"))
        {

        }

        //hasShot = false;
        BounceAnimation.instance.PlayBounce();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        BounceAnimation.instance.PlayerBounceExitClip(playerRigidbody.velocity);
        playerRigidbody.velocity = Vector2.ClampMagnitude(playerRigidbody.velocity, shotVelocityMaxMagnitude);
    }
    #endregion

    #region MOBILE MOVEMENT
    private void MovementControls()
    {

        // Mobile movement.
#if UNITY_STANDALONE || UNITY_ANDROID 
        if (GameManager.instance.state == GameState.Gameplay && Input.touchCount > 0)
        {
            _LatestTouch = Input.GetTouch(0);
            _LatestTouch.position = CameraController.instance.mainCamera.ScreenToWorldPoint(Input.GetTouch(0).position);

            // Create drag anchor at tap position. Slow Down time.
            if (_LatestTouch.phase == TouchPhase.Began)
            {
                _FirstTouchPosition = _LatestTouch.position;
                Time.timeScale = slowTimeScale;
            }

            // Drag circle to latest touch position. 
            if (_LatestTouch.phase == TouchPhase.Stationary || _LatestTouch.phase == TouchPhase.Moved)
            {
                _LastTouchPosition = _LatestTouch.position;

                // Reference firstTouchPosition and lastTouchPosition to get dragVector and dragDistance.
                _DragVector = new Vector2(_FirstTouchPosition.x - _LastTouchPosition.x, _FirstTouchPosition.y - _LastTouchPosition.y);
                dragDistance = Vector2.Distance(_FirstTouchPosition, _LastTouchPosition);

                shotVelocity = new Vector2(_DragVector.x * dragDistance * dragShotForce, _DragVector.y * dragDistance * dragShotForce);
                shotVelocity = Vector2.ClampMagnitude(shotVelocity, shotVelocityMaxMagnitude);

                SlingShotVisuals.instance.MoveTouchVisuals(transform.position, shotVelocity, _FirstTouchPosition, _LastTouchPosition);
            }

            // Multiplies dragVector, dragDist, and slingShotForce to get shotVelocity. 
            // Applies shotVelocity to player rigidbody at transform.position. Speeds up timeScale. 
            else if (_LatestTouch.phase == TouchPhase.Ended)
            {
                MinMaxVelocity();

                playerRigidbody.AddForceAtPosition(shotVelocity, transform.position, ForceMode2D.Impulse);

                Time.timeScale = fastTimeScale;
                // hasShot = true;

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
        #endregion

    }
}
