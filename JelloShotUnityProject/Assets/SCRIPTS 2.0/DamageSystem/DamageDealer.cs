using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField]
    private float _DmgPerAttack = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<IDamageTaker>() != null)
        {
            IDamageTaker damageTaker = collision.gameObject.GetComponent<IDamageTaker>();
            damageTaker.TakeDmg(_DmgPerAttack);
        }     
    }
}
