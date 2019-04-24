using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageTaker
{
    void TakeFullDmg();
    void TakeDmg(float damage);
    void TakeDmgPercentOfMaxHealth(float damagePercent);
    void TakeDmgPercentOfCurrentHealth(float damagePercent);
}
