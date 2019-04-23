using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    internal static Game_Manager GameManager;
    internal LayerMask groundLayer;

    internal void LayerMasks()
    {
        groundLayer = LayerMask.GetMask("Ground");
    }

}
