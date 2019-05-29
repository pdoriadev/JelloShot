using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    [SerializeField]
    private float _DmgPerAttack = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<IDamageTaker>() != null)
        {

            DamageOnCollision(ref collision);
        }     
    }

    protected virtual void DamageOnCollision(ref Collision2D _collision)
    {
        IDamageTaker damageTaker = _collision.gameObject.GetComponent<IDamageTaker>();
        damageTaker.TakeDmg(_DmgPerAttack);
    }
}
