using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour
{
    [SerializeField] GameObject PlayerSprite;
    [SerializeField] Rigidbody2D pRB;
    [SerializeField] private int playerSpeed = 10;
    [SerializeField] private int playerJumpPower = 20;
    [SerializeField] float moveX;

    [SerializeField] private bool facingRight = true;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool freezeYpos;

    private float currentXvelocity;
    private float currentYvelocity;
    private float reverseYvel;

    private LayerMask groundLayer;

    private void Start()
    {
        groundLayer = LayerMask.GetMask("Ground"); // sets groundLayer layermask to the Ground layermask
        pRB = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate ()
    {
        currentXvelocity = pRB.velocity.x;
        currentYvelocity = pRB.velocity.y;
        reverseYvel = -currentYvelocity;

        GroundedCheck();
        PlayerMove();
    }

    void GroundedCheck()
    {
        float rayLength = 1f;
        RaycastHit2D hit = (Physics2D.Raycast(transform.position, (Vector2.down), rayLength, groundLayer));
        Debug.DrawRay(transform.position, Vector2.down, Color.red, rayLength); 
       // reverseYvel = Mathf.Clamp(reverseYvel, 0, Mathf.Infinity);

        Debug.Log(currentYvelocity);
        Debug.Log(reverseYvel);

        if (hit)
            {
            Debug.DrawRay((transform.position), Vector3.down, Color.green, 1f);

            pRB.constraints = RigidbodyConstraints2D.FreezePositionY;

          //  currentYvelocity = (Mathf.Clamp(currentYvelocity, reverseYvel, Mathf.Infinity));
         //   transformYposit = (Mathf.Clamp(transform.position.y, hit.point.y, Mathf.Infinity));
          //  pRB.bodyType = RigidbodyType2D.Static;
         
            isGrounded = true;
            }

        else
            {
            isGrounded = false;
            pRB.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
            }
    }

    void PlayerMove()
    {
        // Controls 
        moveX = (Input.GetAxis("Horizontal"));
        if (Input.GetButtonDown("Jump") && isGrounded == true)
        {
            pRB.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
         //   pRB.constraints = ~RigidbodyConstraints2D.FreezePositionX;
            Jump();
        }

        //Animation
        // FacingDirection
        if (moveX < 0.0f && facingRight == true)
        {
            FlipPlayer();
        }
        else if (moveX > 0.0f && facingRight == false)
        {
            FlipPlayer();
        }
        // Physics
        GetComponent <Rigidbody2D>().velocity = new Vector2(moveX * playerSpeed, currentYvelocity);
    }

    void Jump()
    {
       // isJumping = true;
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * playerJumpPower);
    }

    void FlipPlayer ()
    {
        facingRight = !facingRight;
        Vector2 localScale = gameObject.transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
