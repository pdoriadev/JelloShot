using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum Factions
{
    Generic, // 0                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              
    Player, // 1
    Enemy // 2
}

[CreateAssetMenu(fileName = "FactionImmunity", menuName = "FactionData")]
public class FactionImmunitySO : ScriptableObject
{
    // 2D Array index matches the matching Faction's enum value. i.e. Generic is the first column or subscript 0.
    // 1st dimension is width. 2nd dimension is height.
    public bool[,] _FriendOrFoe = new bool[3, 3];

    public new string name;

    public void SetImmunity(int _damagerFaction, int _damageableFaction, bool _canDamage)
    {
        _FriendOrFoe[_damagerFaction, _damageableFaction] = _canDamage;
    }

    // Can damagerFaction damage damageableFaction
    public bool CheckImmunity(int _damagerFaction, int _damageableFaction)
    {
        if (_FriendOrFoe[_damagerFaction, _damageableFaction] == true)
            { return true; }
        else
            return false;
    }	
}
