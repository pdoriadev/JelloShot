using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateChanger : MonoBehaviour
{
    // Use this if you want something to return to the state it originally changed from. 
    public bool isRevolvingSwitch = false;
    private bool _HasSwitchedBack = true;

    [SerializeField]
    private GameState _ChangeStateToThis;
    [SerializeField]
    private GameState _PreviousState;



    public void ChangeState()
    {
        // switch back
        if (isRevolvingSwitch && _HasSwitchedBack == false)
        {
            _HasSwitchedBack = true;
            GameState holdingState = _ChangeStateToThis;
            _ChangeStateToThis = _PreviousState;
            GameManager.instance.ChangeStateTo(_ChangeStateToThis);
            _ChangeStateToThis = holdingState;
        }
        // switch to
        else
        {
            if (isRevolvingSwitch == true)
                _HasSwitchedBack = false;

            _PreviousState = GameManager.instance.state;
            GameManager.instance.ChangeStateTo(_ChangeStateToThis);
        }
    }
    /// <summary>
    /// Gets previous state. Makes it so the next state change will be to that. Saves 
    /// </summary>
    public void SwitchStatesToChangeToAndPreviousState()
    {
        GameState holdingState = _ChangeStateToThis;
        _ChangeStateToThis = _PreviousState;
        _PreviousState = holdingState;
    }
}
