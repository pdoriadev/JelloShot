using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    [SerializeField]
    private float _DmgPerAttack = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<IDamageable>() != null)
        {
            DamageOnCollisionEnter(ref collision);
        }     
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<IDamageable>() != null)
        {
            DamageOnTriggerExit(ref collision);
        }
    }

    protected virtual void DamageOnCollisionEnter(ref Collision2D _collision)
    {
        if (_collision.gameObject.GetComponent<IDamageable>() != null)
        {
            IDamageable damageTaker = _collision.gameObject.GetComponent<IDamageable>();
            damageTaker.TakeDmg(_DmgPerAttack);
        }
    }

    protected virtual void DamageOnTriggerExit(ref Collider2D _collision)
    {
        if (_collision.gameObject.GetComponent<IDamageable>() != null)
        {
            IDamageable damageTaker = _collision.gameObject.GetComponent<IDamageable>();
            damageTaker.TakeDmg(_DmgPerAttack);
        }
    }
}
