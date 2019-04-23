using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflector : MonoBehaviour
{
    [SerializeField]
    private float _ReflectForce;
    protected float ReflectForce
    {
        get { return _ReflectForce; }
        set { _ReflectForce = value; }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 _ReflectorPos = transform.position;
        Vector2 _ReflectedObjPos = collision.transform.position;
        Vector2 _ReflectDirection = _ReflectedObjPos - _ReflectorPos;
        collision.gameObject.GetComponent<Rigidbody2D>().AddForce(_ReflectDirection * _ReflectForce, ForceMode2D.Impulse);
    }
}
