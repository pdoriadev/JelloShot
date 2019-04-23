using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move2 : MonoBehaviour {
    // Serialized Fields
    [SerializeField] GameObject PlayerSprite;
    [SerializeField] Rigidbody2D pRB;
    [SerializeField] private int playerSpeed = 10;
    [SerializeField] private int playerJumpPower = 1250;
    [SerializeField] float moveX;
    // Bools
    [SerializeField] private bool facingRight = true;
    [SerializeField] private bool isGrounded;
    //RigidBody velocity and position floats
    private float currentXvelocity;
    private float currentYvelocity;
    private float currentYposition;
    // CollisionDetection
    private float rayLength = 0.51f;
    private RaycastHit2D groundRay;
    private LayerMask groundLayer;

    //Called once every physics cycle
    private void Start()
    {
        groundLayer = LayerMask.GetMask("Ground");
        pRB = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        currentXvelocity = GetComponent<Rigidbody2D>().velocity.x;
        currentYvelocity = GetComponent<Rigidbody2D>().velocity.y;
        currentYposition = transform.position.y;

        groundRay = Physics2D.Raycast(transform.position, Vector2.down, rayLength, groundLayer);
        Debug.DrawRay(transform.position, Vector2.down, Color.cyan, rayLength);
        if (groundRay)
        {
            isGrounded = true;
            Debug.Log("ground");
        }
        // GroundCheck();
        PlayerMove();
    }

 #region //groundCheck
    //void GroundCheck()
    //{
    //    if (Collider2D.IsTouchingLayers(GameManager.LayerMasks(groundLayer)))
    //        { isGrounded = true; }

    //    else
    //        { isGrounded = false; }
    //}

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collided");
       
    }

    void OnCollisionExit2D(Collision2D collision)
    {
           // isGrounded = false;
    }

 #endregion

#region //playerMove and jump
    void PlayerMove()
    {
        // Controls 
        moveX = (Input.GetAxis("Horizontal"));

        if (Input.GetButtonDown("Jump") && isGrounded == true)
            { Jump(); }

        ////Animation
       
        //// FacingDirection
        FlipPlayer();

        // Physics
        GetComponent<Rigidbody2D>().velocity = new Vector2(moveX * playerSpeed, currentYvelocity);
    }

    void Jump()
    {
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * playerJumpPower);
    }
#endregion
    #region // FlipPlayer logic
    void FlipPlayer()
    {
        if (moveX < 0.0f && facingRight == true)
        {
            facingRight = !facingRight;
            Vector2 localScale = gameObject.transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
        else if (moveX > 0.0f && facingRight == false)
        {
            facingRight = !facingRight;
            Vector2 localScale = gameObject.transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }
#endregion
}
