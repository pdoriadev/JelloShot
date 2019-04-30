using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflector : MonoBehaviour
{
    [SerializeField]
    private bool _ReflectOn = true;
    [SerializeField]
    private float _ReflectForce;
    protected float ReflectForce
    {
        get { return _ReflectForce; }
        set {
            if (value >= 0)
                _ReflectForce = value;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_ReflectOn && collision.gameObject.layer == (int)GameLayers.Player || collision.gameObject.layer == (int)GameLayers.BallsLayer)
        {
            Vector2 _ReflectorPos = transform.position;
            Vector2 _ReflectedObjPos = collision.transform.position;
            Vector2 _ReflectDirection = _ReflectedObjPos - _ReflectorPos;
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(_ReflectDirection * _ReflectForce, ForceMode2D.Impulse);
        }     
    }
}
