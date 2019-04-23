using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetect : MonoBehaviour {

    //[SerializeField] Rigidbody2D pRB;
    //// Bools
    //[SerializeField] private bool isGrounded;
    ////RigidBody velocity and position floats
    //private float currentXvelocity;
    //private float currentYvelocity;
    //private float currentYposition;
    //// CollisionDetection
    //private float rayLength = 0.51f;
    //private RaycastHit2D groundRay;
    //private LayerMask groundLayer;

    //private void Awake()
    //{
    //    groundLayer = LayerMask.GetMask("Ground");
    //    pRB = GetComponent<Rigidbody2D>();
    //}

    //void FixedUpdate()
    //{
    //    currentXvelocity = GetComponent<Rigidbody2D>().velocity.x;
    //    currentYvelocity = GetComponent<Rigidbody2D>().velocity.y;
    //    currentYposition = transform.position.y;

    //    groundRay = Physics2D.Raycast(transform.position, Vector2.down, rayLength, groundLayer);
    //    Debug.DrawRay(transform.position, Vector2.down, Color.cyan, rayLength);
    //    if (groundRay)
    //    {
    //        isGrounded = true;
    //        Debug.Log("ground");
    //    }
        // GroundCheck();
      //  PlayerMove();
  //  }
}
