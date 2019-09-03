using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateChanger : MonoBehaviour
{
    [SerializeField]
    GameState stateToChangeTo;

    public void ChangeState()
    {
        GameManager.instance.state = stateToChangeTo;
    }
}
