using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FactionsToReflect
{
    All,
    Player,
    Enemy
}

public class Reflector : MonoBehaviour
{
    [SerializeField]
    private bool _ReflectOn = true;
    [SerializeField]
    private float _ReflectForceMultiplier;
    protected float ReflectForce
    {
        get { return _ReflectForceMultiplier; }
        set {
            if (value >= 0)
                _ReflectForceMultiplier = value;
        }
    }
    [SerializeField]
    private FactionsToReflect _WhatToReflect;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody2D>() != null)
        {
            if (_WhatToReflect == FactionsToReflect.All)
            {
                Reflect(ref collision);
            }
            else if (_WhatToReflect == FactionsToReflect.Player)
            {
                if (_ReflectOn && collision.gameObject.layer == (int)GameLayers.Player)
                {
                    Reflect(ref collision);
                }
            }
            else if (_WhatToReflect == FactionsToReflect.Enemy)
            {
                if (collision.gameObject.layer == (int)GameLayers.BallsLayer)
                {
                    Reflect(ref collision);
                }
            }
        }     
    }

    private void Reflect(ref Collision2D _collision)
    {
        ContactPoint2D _ReflectionPoint = _collision.GetContact(0);
        Vector2 _ReflectDirection = _ReflectionPoint.normal * -1f; // Invert normal to point away from reflector
        float _ContactForce = _ReflectionPoint.normalImpulse; 
        _collision.gameObject.GetComponent<Rigidbody2D>().AddForce(_ReflectDirection * _ContactForce * _ReflectForceMultiplier, ForceMode2D.Impulse);
    }
}
