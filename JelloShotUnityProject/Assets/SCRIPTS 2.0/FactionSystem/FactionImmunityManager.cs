using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class FactionImmunityManager : MonoBehaviour
{
    public FactionImmunitySO factionData;    
}

#if UNITY_EDITOR
[RequireComponent(typeof(FactionImmunityManager))]
public class FactionImmunityManagerEditor : Editor
{
    private bool[,] _ImmunityGrid;
    private int _Width = 1;
    private int _Height = 0;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FactionImmunityManager _ImmunityManager = (FactionImmunityManager)target;


    }

}
#endif
