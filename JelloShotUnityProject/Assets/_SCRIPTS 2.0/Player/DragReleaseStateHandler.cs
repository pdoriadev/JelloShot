using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DragReleaseState
{
    AtRest, // 0
    BeginningTap, // 1
    WindUp, // 2
    Release // 3
}

public class DragReleaseStateHandler : MonoBehaviour
{
    private void OnEnable()
    {
        slingshotState = DragReleaseState.AtRest;
    }

    DragReleaseState _SlingShotState;
    public DragReleaseState slingshotState
    {
        get { return _SlingShotState; }
        set { _SlingShotState = value; }
    }
}
