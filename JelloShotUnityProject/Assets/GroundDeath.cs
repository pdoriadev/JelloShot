using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDeath : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GetComponent<DamageableLerpOnDmg>().currentHealth == 0)
        {
            GameManager.instance.LevelEnd();
        }
    }

}
