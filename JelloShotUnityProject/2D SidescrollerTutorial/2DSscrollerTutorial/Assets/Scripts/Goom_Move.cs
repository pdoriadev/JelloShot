using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goom_Move : MonoBehaviour
{

    Rigidbody2D erb;
    [SerializeField] private int enemySpeed, xMoveDir;
    [SerializeField] private float rayLength, deathXdir, deathYdir, deathGravity;
    [SerializeField] private bool dead = false;
    SpriteRenderer enemySprite;

    RaycastHit2D wallHit;
    LayerMask groundLayer;

    private void Start()
    {
        groundLayer = LayerMask.GetMask("Ground");
        erb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void FixedUpdate ()
    {
        if (dead == false)
        {
            RayWallCheck();
            EnemyPhysics();
        }

        if (wallHit)  Flip();
	}

    void RayWallCheck()
    {
        wallHit = Physics2D.Raycast(transform.position, new Vector2(xMoveDir, 0), rayLength, groundLayer);
        Debug.DrawRay(transform.position, new Vector2(xMoveDir, 0), Color.red, 1);
    }

    void EnemyPhysics()
    {
        erb.velocity = new Vector2(xMoveDir, 0) * enemySpeed;
    }

    void Flip()
    {
        xMoveDir = (xMoveDir > 0) ? -1 : 1;
       // gameObject.transform.localScale *= new Vector2(-1, gameObject.transform.localScale.y);
        Debug.DrawRay(transform.position, new Vector2(xMoveDir, 0), Color.green, 2);
    }
    internal void EnemyDeath()
    {
        erb.AddForce(new Vector2(deathXdir, deathYdir));
        erb.gravityScale = deathGravity;
        GetComponentInChildren<SpriteRenderer>().sortingOrder = 3;
        dead = true;
    }
}
