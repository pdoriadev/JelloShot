using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    [SerializeField]
    private float _DmgPerAttack = 1;
    private bool _DamageOnTrigger, _DamageOnCollision;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<IDamageable>() != null)
        {
            DamageOnColliderInteraction(ref collision);
        }     
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<IDamageable>() != null)
        {
            DamageOnTriggerInteraction(ref collision);
        }
    }

    protected virtual void DamageOnColliderInteraction(ref Collision2D _collision)
    {
        IDamageable damageTaker = _collision.gameObject.GetComponent<IDamageable>();
        damageTaker.TakeDmg(_DmgPerAttack);
    }

    protected virtual void DamageOnTriggerInteraction(ref Collider2D _collision)
    {
        IDamageable damageTaker = _collision.gameObject.GetComponent<IDamageable>();
        damageTaker.TakeDmg(_DmgPerAttack);
    }
}
