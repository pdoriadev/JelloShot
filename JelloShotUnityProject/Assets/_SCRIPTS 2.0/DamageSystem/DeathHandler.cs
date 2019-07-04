using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeathHandler : MonoBehaviour
{
    public UnityEvent onKillEvent;
    public virtual void OnKill()
    {
        onKillEvent.Invoke();
    }
}
