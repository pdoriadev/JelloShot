using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TouchInputState
{
    AtRest, // 0
    BeginningTap, // 1
    Dragging, // 2
    Release // 3
}

public class DragReleaseStateHandler : MonoBehaviour
{
    private void OnEnable()
    {
        slingshotState = TouchInputState.AtRest;
    }

    TouchInputState _SlingShotState;
    public TouchInputState slingshotState
    {
        get { return _SlingShotState; }
        set { _SlingShotState = value; }
    }
}
