using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnCollisionEventPublisher : MonoBehaviour
{
    public UnityEvent OnCollision;
    public delegate void OnCollisionEvent(Collision2D _collision);
    public event OnCollisionEvent onCollisionEvent;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnCollision.Invoke();
        onCollisionEvent(collision);
    }

}
