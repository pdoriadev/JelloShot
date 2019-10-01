using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateChanger : MonoBehaviour
{
    [SerializeField]
    GameState stateToChangeTo;
    private GameState stateToSwitchTo;

    public void ChangeState()
    {
        stateToSwitchTo = GameManager.instance.state;
        GameManager.instance.ChangeStateTo(stateToChangeTo);
    }
    public void SwitchStatesToChangeTo()
    {
        GameState tempState = stateToChangeTo;
        stateToChangeTo = stateToSwitchTo;
        stateToSwitchTo = tempState;
    }
}
