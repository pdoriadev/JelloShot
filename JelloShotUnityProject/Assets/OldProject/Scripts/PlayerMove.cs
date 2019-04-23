using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    [SerializeField]
    private float pSpeed, moveX, jumpPower, currentGravity, airGravity, bounceGravity, rayLength, hitDistance;
    [SerializeField]
    private double cyteTimePlusTime, coyoteTime;
    [SerializeField]
    internal float bouncePower;
    [SerializeField]
    Vector2 lftGrndRayOrgnOffset, rghtGrndRayOrgnOffset, WllRayOriginBot, WllRayOriginTop;

    //[SerializeField] Vector2 rayDir = Vector2.down;
    private SpriteRenderer pSprite;
    private Animator pAnimator;
    private float yVel, yPos, xPos;
    private bool  isGrounded, isJumping, hitWall;
    internal bool isBouncing;
    private LayerMask groundLayer, enemyLayer;
    RaycastHit2D leftGroundHit, rightGroundHit, enemyHit, topRightWallHit, tpLftWllHt, botRghtWllHt, botLftWllHt;
    Shared_Vars shared_VarsScript;

    // Use this for initialization
    void Start ()
    { 
        groundLayer = LayerMask.GetMask("Ground");
        enemyLayer = LayerMask.GetMask("Enemy");
        shared_VarsScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Shared_Vars>();
        pSprite = GameObject.FindGameObjectWithTag("PlayerSprite").GetComponent<SpriteRenderer>();
        pAnimator = GameObject.FindGameObjectWithTag("PlayerSprite").GetComponent<Animator>();
    }

    internal void UpdateSharedVars()
    {

    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        shared_VarsScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Shared_Vars>();  
        shared_VarsScript.prb.gravityScale = currentGravity;
        yVel = shared_VarsScript.prb.velocity.y;
        yPos = shared_VarsScript.prb.transform.position.y;
        xPos = shared_VarsScript.prb.transform.position.x;

        if (yVel > 10 || yVel < -10) shared_VarsScript.prb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        else shared_VarsScript.prb.collisionDetectionMode = CollisionDetectionMode2D.Discrete;

        RayGroundCheck();
        RayWallCheck();
        Player_Controls();
        MovePhysics();
        HandleFlip();

        if (isBouncing == true)
        {
            EnemyBouncePhysics();
            if (currentGravity == bounceGravity) Debug.Log("bounceGrav");
            isBouncing = !isBouncing;
        }
	}

    #region WALL AND GROUND CHECKS

    void RayGroundCheck()
    {
        // ray to check if player is colliding below themselves on their left side. 
        leftGroundHit = Physics2D.Raycast((Vector2)shared_VarsScript.prb.transform.localPosition + lftGrndRayOrgnOffset, Vector2.down, rayLength, groundLayer);
        Debug.DrawRay((Vector2)shared_VarsScript.prb.transform.localPosition + lftGrndRayOrgnOffset, Vector2.down, Color.red, rayLength);

        // ray to check if player is colliding below themselves on their right side. 
        rightGroundHit = Physics2D.Raycast((Vector2)shared_VarsScript.prb.transform.localPosition + rghtGrndRayOrgnOffset, Vector2.down, rayLength, groundLayer);
        Debug.DrawRay((Vector2)shared_VarsScript.prb.transform.localPosition + rghtGrndRayOrgnOffset, Vector2.down, Color.red, rayLength);

        // ray to check if an enemy is beneath the player. 
        enemyHit = Physics2D.Raycast((Vector2)shared_VarsScript.prb.transform.localPosition + lftGrndRayOrgnOffset, Vector2.down, rayLength, enemyLayer);
        
Debug.DrawRay((Vector2)shared_VarsScript.prb.transform.localPosition + lftGrndRayOrgnOffset, Vector2.down, Color.red, rayLength);
        if (leftGroundHit || rightGroundHit || cyteTimePlusTime > Time.time) GroundedPhysics();

        else if (enemyHit) EnemyBouncePhysics();

   
        else
        {
            isGrounded = false; 
            currentGravity = airGravity;
        }
    }

    private void RayWallCheck()
    {
        if (isGrounded == false)
        {
            // Origins of Wall Check Rays
            WllRayOriginTop = new Vector2(shared_VarsScript.prb.transform.localPosition.x, shared_VarsScript.prb.transform.localPosition.y + 0.3f);
            WllRayOriginBot = new Vector2(shared_VarsScript.prb.transform.localPosition.x, shared_VarsScript.prb.transform.localPosition.y -0.3f);

            topRightWallHit = Physics2D.Raycast((Vector2)WllRayOriginTop, Vector2.right, rayLength, groundLayer);

            tpLftWllHt = Physics2D.Raycast((Vector2)WllRayOriginTop, Vector2.left, rayLength, groundLayer);

            botRghtWllHt = Physics2D.Raycast((Vector2)WllRayOriginBot, Vector2.right, rayLength, groundLayer);

            botLftWllHt = Physics2D.Raycast((Vector2)WllRayOriginBot, Vector2.left, rayLength, groundLayer);
        }

        if (topRightWallHit || tpLftWllHt || botRghtWllHt || botLftWllHt)
        {
            hitWall = true;
            // HitWallPhysics();
        }

        else
        {
            hitWall = false;
        }
    }

    #endregion GROUND AND WALL CHECKS END

    #region // PHYSICS

    void GroundedPhysics()
    {
        isGrounded = true;
        Debug.DrawRay((Vector2)shared_VarsScript.prb.transform.localPosition + lftGrndRayOrgnOffset, Vector2.down, Color.green, rayLength);

        if (leftGroundHit || rightGroundHit )
        {
            cyteTimePlusTime = Time.time + coyoteTime;
        }

        // Non-collider-based player physics

        // currentGravity = 0;
        // yVel = 0f; 
        // yPos = (Mathf.Clamp(yPos,leftGroundHit.transform.position.y, Mathf.Infinity));
        // yVel = (Mathf.Clamp(yVel, 0, Mathf.Infinity));
        // Back up to FixedUpdate
    }

    void HitWallPhysics()
    {
       // shared_VarsScript.prb.transform.position = (new Vector2(0, -1));
    }

    void Jump()
    {
        //Debug.Log("JUMP");
        currentGravity = airGravity;
        //shared_VarsScript.prb.AddForce(new Vector2(shared_VarsScript.prb.velocity.x, 0));
        shared_VarsScript.prb.AddForce(new Vector2(shared_VarsScript.prb.velocity.x, jumpPower));
        isJumping = true;
    }

    void MovePhysics()
    {
        shared_VarsScript.prb.velocity = new Vector2(moveX * pSpeed, yVel);
    }

    internal void EnemyBouncePhysics()
    {
        currentGravity = bounceGravity;
        yVel = 0;
        shared_VarsScript.prb.AddForce(new Vector2(shared_VarsScript.prb.velocity.x, bouncePower));
    }

    #endregion

    // Player Controls
    internal void Player_Controls()
    {
        if (hitWall == false)
        {
            moveX = (Input.GetAxisRaw("Horizontal"));
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
            cyteTimePlusTime = Time.time;
        }
    }

    // Flips player sprite and adjusts animator
    void HandleFlip()
    {
        if ((moveX > 0.1)) 
        {
             pSprite.flipX = true;
             pAnimator.SetBool("IsRolling", true);
        }

        if ((moveX < -0.1))
        {
             pSprite.flipX = false;
             pAnimator.SetBool("isRolling", true);
        }

        else pAnimator.SetBool("isRolling", false);
    }   
}