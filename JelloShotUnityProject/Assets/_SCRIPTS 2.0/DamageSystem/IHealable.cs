using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealable
{
    void OnHeal();
    bool CanHealCheck();

    void HealToFullHealth();
    void HealToValue(float _newHealthValue);
    void HealByXMuch(float _addedHealth);

    void HealPercentageOfMaxHealth(float _percentageRegenned);
    void HealPercentageOfCurrentHealth(float _percentageRegenned);
    void HealPercentageOfCurrentDamageTaken(float _percentageRegenned);
}
