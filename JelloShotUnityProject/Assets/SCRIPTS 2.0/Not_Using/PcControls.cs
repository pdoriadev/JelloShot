using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Leftover PC controls copy/pasted from PlayerManager
public class PcControls : MonoBehaviour {


    public static GameObject playerGameObject;
    public static PlayerManager instance;

    [Space(10)]
    public Rigidbody2D playerRigidbody;
    // Rigidbody of ball that player is colliding with.
    public Rigidbody2D ballRigidBody;

#if UNITY_STANDALONE || UNITY_WEBPLAYER
    [Header("PC Variables")]
    public RaycastHit2D groundAndBallDetectRay;
    public LayerMask groundAndBallsLayer;

    // MOVEMENT FLOAT VALUES. Includes X move direction, move speed, ground pound force, and ground detect raycast's length. "norm" and "buffed" floats
    // change the moveSpeed and poundForce variables depending if a ball or the ground is right below the player. 
    public float playerXMove;
    public float moveSpeed;
    public float normMoveSpeed;
    public float initialMoveSpeed, currentMoveSpeed, buffedMoveSpeed;
    public float poundForce, normPoundForce, buffedPoundForce;
    public float rayLength;
#endif

    //private void OnEnable()
    //{
    //    if (instance == null)
    //        instance = this;

    //    // If no player in scene
    //    if (playerGameObject == null)
    //        playerGameObject = gameObject;

    //    GameManager.OnFixedUpdateEvent += OnFixedUpdateHandler;
    //    GameManager.OnUpdateEvent += OnUpdateHandler;

    //    playerCurrentVelocity = playerRigidbody.velocity;

    //}

    // Called when monobehaviour has been disabled or attached object is destroyed. Unsubscribes Handlers.
    //private void OnDisable()
    //{
    //    GameManager.OnFixedUpdateEvent -= OnFixedUpdateHandler;
    //    GameManager.OnUpdateEvent -= OnUpdateHandler;
    //    instance = null;
    //}
    // Update is called once per frame
    void Update () {
#if UNITY_STANDALONE || UNITY_WEBPLAYER
        GroundAndBallDetect();
#endif
    }

    //private void FixedUpdate()
    //{
    //    playerCurrentVelocity = playerRigidbody.velocity;
    //}

    #region PC / WEB PLAYER MOVEMENT

#if UNITY_STANDALONE || UNITY_WEBPLAYER
        playerXMove = (Input.GetAxisRaw("Horizontal"));

        // Left and Right Movement
        playerRB.AddForceAtPosition(Vector2.right * playerXMove * moveSpeed, playerRB.transform.position, ForceMode2D.Force);

        // Dash move speed at beginning of left/right input
        if (Input.GetButtonDown("Left") || Input.GetButtonDown("Right") && !((Input.GetButtonDown("Left")) && (Input.GetButtonDown("Right"))))
            playerRB.AddForceAtPosition(Vector2.right * playerXMove * moveSpeed, playerRB.transform.position);

        // GROUND POUND MOVE. THROWS PLAYER DOWN 
        if ((Input.GetButton("Left")) && (Input.GetButton("Right")))
            playerRB.AddForceAtPosition(Vector2.down * poundForce, playerRB.transform.position);

        
    // Ground Detect raycasts and logic

    // Important Buffs for PC platform. 
    void GroundAndBallDetect()
    {
        // Raycasts detecting ground and balls
        groundAndBallDetectRay = Physics2D.Raycast(transform.position, Vector2.down, rayLength, groundAndBallsLayer);

        // Debug line duplicating raycast trajectory
        Debug.DrawRay(transform.position, Vector2.down, Color.red);

        // if either raycast returns true, then movespeed and pound force get a buff
        if (groundAndBallDetectRay)
        {
            moveSpeed = buffedMoveSpeed;
            poundForce = buffedPoundForce;

            // Green Debug line to show a raycast returned true. 
            Debug.DrawRay(transform.position, Vector2.down, Color.green);
        }

        // else moveSpeed and poundForce are returned to normal. 
        else 
        {
            poundForce = normPoundForce;
        }  
    }
#endif



    // PC ground detect for ground puound feature.
#if UNITY_STANDALONE || UNITY_WEBPLAYER
    // Important Buffs for PC platform. 
    void GroundAndBallDetect()
    {
        // Raycasts detecting ground and balls
        groundAndBallDetectRay = Physics2D.Raycast(transform.position, Vector2.down, rayLength, groundAndBallsLayer);

        // Debug line duplicating raycast trajectory
        Debug.DrawRay(transform.position, Vector2.down, Color.red);

        // if either raycast returns true, then movespeed and pound force get a buff
        if (groundAndBallDetectRay)
        {
            moveSpeed = buffedMoveSpeed;
            poundForce = buffedPoundForce;

            // Green Debug line to show a raycast returned true. 
            Debug.DrawRay(transform.position, Vector2.down, Color.green);
        }

        // else moveSpeed and poundForce are returned to normal. 
        else
        {
            poundForce = normPoundForce;
        }
    }
#endif

    #endregion
}
