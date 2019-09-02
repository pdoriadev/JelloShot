using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEventPublisher : MonoBehaviour
{
    public delegate void CollisionEvent();
    public event CollisionEvent collisionEvent;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collisionEvent();
    }
}
