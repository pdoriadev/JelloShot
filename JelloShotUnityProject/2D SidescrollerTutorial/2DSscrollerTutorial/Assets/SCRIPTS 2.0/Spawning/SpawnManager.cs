using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Manages spawning loop for all spawnables. 
/// 1. Does NOT select what to spawn (see ItemSelector script). 
/// 2. Spawning: If selected item has a matching pooled item, then SpawnManager spawns the pooled item. If selected item has no matching item 
/// in pooled objects list, then a new instance of that item is instantiated. 
/// 3. PoolObject() function.
/// 4. ResetSpawnListsAndTimers() function
/// </summary>

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    #region UNITY CALLBACKS
    // Called when monobehaviour has been enabled. Adds BallSpawnDestroy instance to scene if there is none. 
    // Subscribes BallSpawnDestroy's FixedUpdateHandler to GM's FixedUpdate Event
    private void OnEnable()
    {
        if (instance == null)
            instance = this;
    }

    private void OnDisable()
    {
        instance = null;
    }

    private void Start()
    {
        StartCoroutine(CoSpawnItem());
        startMinSpawnWait = spawnMinWait;
        startMaxSpawnWait = spawnMaxWait;
    }
    #endregion

    #region VARIABLES
    public List<GameObject> spawnablesInGame = new List<GameObject>();
    public List<GameObject> pooledObjectsList = new List<GameObject>();

    // Wait time before checking if can spawn
    public float startWait;
    // bool that stops spawning when true. 
    public bool stop;

    public float spawnMinWait;
    public float spawnMaxWait;
    public float spawnWait;
    public float startMinSpawnWait;
    public float startMaxSpawnWait;

    // Defines where objects can spawn
    public Vector3 spawningZone;
    // Where objects do spawn
    public Vector3 spawnPosition;
    #endregion

    IEnumerator CoSpawnItem()
    {
        yield return new WaitForSeconds(startWait);
        while (!stop)
        {
            spawnWait = Random.Range(spawnMinWait, spawnMaxWait);
            spawnPosition = new Vector3(Random.Range(-spawningZone.x, spawningZone.x), Random.Range(-spawningZone.y, spawningZone.y), 1);
            GameObject spawnable = null;
            int index = 0;
            bool spawnPooledObject = false;

            // 1. Select item to spawn
            spawnable = ItemSelector.instance.SelectItem();

            // 2. Check object pool for matching item
            while (index < pooledObjectsList.Count)
            {
                // iterate through pooled obj list. if equivalent obj, spawn it.
                if (pooledObjectsList[index].tag == spawnable.tag)
                {
                    spawnPooledObject = true;
                    spawnable = pooledObjectsList[index];
                    break;
                }
                index++;
            }

            // Unpool and move GameObject to spawnPosition. Add item to spawnablesInGameList.
            // random spawn position on x and z axis min/max of spawn values. spawn position does not go above 1 on the y axis.  <---- ???
            if (spawnPooledObject)
            {
                spawnable.SetActive(true);
                spawnable.transform.position = spawnPosition + transform.TransformPoint(0, 0, 0);
                spawnablesInGame.Add(spawnable);
                pooledObjectsList.Remove(spawnable);
            }

            // Instantiate GameObject at spawn position. Add to spawnablesInGameList.
            if (spawnPooledObject == false)
            {
                spawnablesInGame.Add(Instantiate(spawnable, spawnPosition + transform.TransformPoint(0, 0, 0), gameObject.transform.rotation));
                spawnable.SetActive(true);
            }

            yield return new WaitForSeconds(spawnWait);
        }
    }

    /// Sets passed object to inactive and pools it. Called by OutOfBounds and BallCollisions Script
    public void PoolObject(GameObject ball)
    {
        ball.SetActive(false);
        pooledObjectsList.Add(ball);
        spawnablesInGame.Remove(ball);
    }

    public void ResetSpawnListsAndTimers()
    {
        // Pools all items in spawnablesInGameList into pooledObjectsList
        for (int index = 0; index < SpawnManager.instance.spawnablesInGame.Count; index++)
        {
            GameObject currentBall = (GameObject)SpawnManager.instance.spawnablesInGame[index];
            pooledObjectsList.AddRange(spawnablesInGame);
            spawnablesInGame.RemoveRange(0, spawnablesInGame.Count);
        }

        spawnablesInGame.Clear();
    }
}


