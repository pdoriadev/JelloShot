using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UniversalPoolable : MonoBehaviour
{
    // set type in inspector
    [SerializeField]
    private PoolableType _Type;
    private PoolableInfo _Info;

    public UnityEvent onPool;

    private void OnEnable()
    {
        _Info.poolableGO = gameObject;
        _Info.type = _Type;
    }

    public PoolableType GetPoolableType() { return _Type; }
    public void SetPoolableType(PoolableType _type) {  _Type = _type; }

    public void PoolThis()
    {
        onPool.Invoke();
        UniversalPooler.instance.PoolPoolable(_Info);
    }

}

public enum PoolableType
{
    NotSpawned,
    Spawned
}

public struct PoolableInfo
{
    public GameObject poolableGO;
    public PoolableType type;

    PoolableInfo(GameObject _go, PoolableType _type )
    {
        poolableGO = _go;
        type = _type;
    }
}
