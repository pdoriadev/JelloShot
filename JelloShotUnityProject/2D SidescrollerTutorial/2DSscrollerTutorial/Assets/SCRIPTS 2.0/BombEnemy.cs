using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Explode on contact. May add explode on timer feature. 
public class BombEnemy : MonoBehaviour
{
    [SerializeField]
    private BombCollisionTrigger _BombCollisionTrigger;

    #region UNITY CALLBACKS
    private void OnEnable()
    {
        _BombCollisionTrigger = GetComponentInChildren<BombCollisionTrigger>();
    }

    private void OnDisable()
    {
        _BombCollisionTrigger = null;
    }
    #endregion

    [SerializeField]
    private float _ExplosiveForce = 80f;

    internal void ApplyExplodingForce()
    {
        Vector2 _BombPosition = transform.position;

        for (int i = 0; i < _BombCollisionTrigger._ObjectsInBlastRadius.Count; i++)
        {
            // add force in direction from bomb to exploded upon object
            Vector2 _BlastedObjPosition = _BombCollisionTrigger._ObjectsInBlastRadius[i].gameObject.transform.position;
            Vector2 _BlastDirection = _BlastedObjPosition - _BombPosition;

            _BombCollisionTrigger._ObjectsInBlastRadius[i].GetComponent<Rigidbody2D>().AddForce(_BlastDirection * _ExplosiveForce, ForceMode2D.Impulse);          
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == (int)GameLayers.Player)
        {
            ApplyExplodingForce();
        }
    }
}
