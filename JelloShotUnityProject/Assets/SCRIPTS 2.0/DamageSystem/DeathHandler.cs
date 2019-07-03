using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeathHandlerBase : MonoBehaviour, IKillable
{
    UnityEvent onKillEvent;

    public virtual void OnKill()
    {
        onKillEvent.Invoke();
    }
}
