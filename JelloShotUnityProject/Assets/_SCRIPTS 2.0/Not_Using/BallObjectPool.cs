using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///  https://forum.unity.com/threads/healthy-alternative-to-gameobject-find.352061/
///  Info on best practices for referencing objects. Extremely important at run-time.
/// </summary>
public class BallObjectPool : MonoBehaviour
{
    // holdingZone is where objects are located before being needed?
    public Vector3 holdingZone = new Vector3(100, 100, -100);
    // makes sure rotation doesn't change before removing object from pool ????
    private Quaternion holdingRotation;

    // Declares dictionary for game objects. Dictionary<key, queue of <game objects>> name of dictionary;
    private Dictionary<string, Queue<GameObject>> types;
    // Declares queue of game objects called pool
    private Queue<GameObject> pool;

    // Use this for initialization. 
    void Awake()
    {
        // Dictionary declared as types before awake is redefined
        types = new Dictionary<string, Queue<GameObject>>();
        // Object script is attached to's rotation is reset to 0,0,0.
        holdingRotation.eulerAngles = new Vector3(0, 0, 0);
    }

    // when called, has key, prefab, and associated int passed to it. 
    public void addType(string type, GameObject prefab, int number)
    {
        // if there is no string key
        if (!types.ContainsKey(type))
        {
            // creates a new game object queue and assigns it to pool queue.
            pool = new Queue<GameObject>();
            // for every object in pool queue
            for (int i = 0; i < number; i++)
            {
                // and uses the Enqueue method to queue five strings. 
                // The elements of the queue are enumerated, 
                // which does not change the state of the queue. 

                // "pool.Enqueue" queues (<-- verb) items in queue. Like starting to move a line of people
                pool.Enqueue(Instantiate(prefab, holdingZone, holdingRotation));
            }
            // ?? Add another key called type into the pool queue ??
            types.Add(type, pool);
        }
    }

    // when called, is passed a key from the pool queue
    public void removeType(string type)
    {
        // if a matching key is found in the dictionary
        if (types.ContainsKey(type))
        {
            // Remove the key from the dictionary.
            types.Remove(type);
        }
    }

    // passed key to gameobject 
    public GameObject unPool(string type)
    {
        // game object has a key
        if (types.ContainsKey(type))
        {
            // Get matching key in dictionary
            types.TryGetValue(type, out pool);
            // if number of objects is equal to or greater than 0
            if (pool.Count >= 0)
            {
                // remove objects from the pool
                return pool.Dequeue();
            }
        }
        return null;
    }

    // when called, passed a key and gameobject
    public void rePool(string type, GameObject go)
    {
        // if find key in dictionary 
        if (types.ContainsKey(type))
        {
            // Match found key with object in queue
            // The out keyword causes arguments to be passed by reference. 
            // It is like the ref keyword, except that ref requires that the variable be initialized before 
            // it is passed. It is also like the in keyword, except that in does not allow the called method 
            // to modify the argument value. To use an out parameter, both the method definition and
            // the calling method must explicitly use the out keyword. 
            types.TryGetValue(type, out pool);
            // Queue object in pool queue
            pool.Enqueue(go);
        }
    }
}   
