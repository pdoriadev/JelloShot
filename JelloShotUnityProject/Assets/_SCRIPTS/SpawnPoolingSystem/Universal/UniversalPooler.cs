using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalPooler : MonoBehaviour
{
	//public Vector3 holdingZone = new Vector3(100, 100, -100);
 //   private Quaternion holdingRotation;

    //private Dictionary<string, Queue<GameObject>> types;
    private Dictionary<PoolableType, Queue<GameObject>> _PoolTypes;
    private Queue<GameObject> pool;

    void Start()
    {
        //types = new Dictionary<string, Queue<GameObject>>();
        _PoolTypes = new Dictionary<PoolableType, Queue<GameObject>>();
        //holdingRotation.eulerAngles = new Vector3(0, 0, 0);
    }

    // Pools and spawns. Adds new queue pool to types if pool of this poolable's type does not exist.  
    public void PoolPoolable(PoolableInfo _info)
    {
        if (_PoolTypes.ContainsKey(_info.type))
        {
            _PoolTypes.TryGetValue(_info.type, out pool);
            pool.Enqueue(_info.poolableGO);
        }
        else 
        {
            pool = new Queue<GameObject>();
            pool.Enqueue(_info.poolableGO);
            _PoolTypes.Add(_info.type, pool);
        }
    }


    //public void addType(string type, GameObject prefab, int number)
    //{
    //    if (!types.ContainsKey(type))
    //    {
    //        pool = new Queue<GameObject>();
    //        for (int i = 0; i < number; i++)
    //        {
    //            pool.Enqueue(Instantiate(prefab, holdingZone, holdingRotation));
    //        }
    //        types.Add(type, pool);
    //    }
    //}

    //public void removeType(string type)
    //{
    //    if (types.ContainsKey(type))
    //    {
    //        types.Remove(type);
    //    }
    //}

    //public GameObject unPool(string type)
    //{
    //    if (types.ContainsKey(type))
    //    {
    //        types.TryGetValue(type, out pool);
    //        if (pool.Count >= 0)
    //        {
    //            return pool.Dequeue();
    //        }
    //    }
    //    return null;
    //}

    //public void rePool(string type, GameObject go)
    //{
    //    if (types.ContainsKey(type))
    //    {
    //        types.TryGetValue(type, out pool);
    //        pool.Enqueue(go);
    //    }
    //}
}


