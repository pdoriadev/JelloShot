using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Singleton Manager spawning loop for enemy spawnables. 
/// 1. Does NOT select what to spawn (see ItemSelector script). 
/// 2. Spawning: If selected item has a matching pooled item, then SpawnManager spawns the pooled item. If selected item has no matching item 
///    in pooled objects list, then a new instance of that item is instantiated. 
/// 3. PoolObject() function.
/// 4. PoolAllSpawnables() function for game reset.
/// </summary>

public class SpawnManager : MonoBehaviour
{

    public static SpawnManager instance;

    #region PUBLIC VARIABLES

    public List<GameObject> spawnablesInGame = new List<GameObject>();
    public List<GameObject> pooledObjectsList = new List<GameObject>();

    // Wait time before checking if can spawn
    public float startWait;
    // bool that stops spawning when false. 
    public bool isSpawning = false;
    public float spawnMinWaitFloor;
    public float spawnMaxWaitFloor;

    public float currentWaitTime;
    public float startMinSpawnWait;
    public float startMaxSpawnWait;

    // Defines where objects can spawn
    public Vector3 spawningZone;
    // Where objects do spawn
    public Vector3 spawnPosition;

    #endregion

    #region WAIT TIME PROPERTIES

    [SerializeField]
    private float _CurrentMinWait;
    public float currentMinWait
    {
        get { return _CurrentMinWait; }
        set
        {
            _CurrentMinWait = value;
            if (_CurrentMinWait < spawnMinWaitFloor)
            {
                _CurrentMinWait = spawnMinWaitFloor;
            }
        }
    }
    [SerializeField]
    private float _CurrentMaxWait;
    public float currentMaxWait
    {
        get { return _CurrentMaxWait; }
        set
        {
            _CurrentMaxWait = value;
            if (_CurrentMaxWait < spawnMaxWaitFloor)
            {
                _CurrentMaxWait = spawnMaxWaitFloor;
            }
        }
    }
    #endregion

    #region UNITY CALLBACKS

    private void OnEnable()
    {
        if (instance == null)
            instance = this;
        GameManager.onRetryEvent += PoolAllSpawnables;
    }

    private void OnDisable()
    {
        GameManager.onRetryEvent -= PoolAllSpawnables;
        instance = null;
    }

    private void Awake()
    {
        currentMinWait = startMinSpawnWait;
        currentMaxWait = startMaxSpawnWait;
    }

    private void Update()
    {
        if (GameManager.instance.state == GameState.Gameplay && isSpawning == false && GameManager.instance.isTutorial == false)
        {
            StartCoroutine(CoSpawnItem());
            isSpawning = true;
        }
        else if (GameManager.instance.state != GameState.Gameplay && isSpawning == true)
        {
            StopCoroutine(CoSpawnItem());
            isSpawning = false;
        }
    }
    #endregion


    // Handles spawn loop
    IEnumerator CoSpawnItem()
    {
        yield return new WaitForSeconds(startWait);

        while (isSpawning)
        {
            currentWaitTime = Random.Range(currentMinWait, currentMaxWait);
            spawnPosition = new Vector3(Random.Range(-spawningZone.x, spawningZone.x), Random.Range(-spawningZone.y, spawningZone.y), 1);
            GameObject spawnable = null;
            int index = 0;
            bool spawnPooledObject = false;

            // 1. Select item to spawn
            spawnable = ItemSelector.instance.SelectItem();

            // 2. Check object pool for matching item
            for ( ; index < pooledObjectsList.Count; index++ )
            {
                // iterate through pooled obj list. if equivalent obj, spawn it.
                if (pooledObjectsList[index].tag == spawnable.tag)
                {
                    spawnPooledObject = true;
                    spawnable = pooledObjectsList[index];
                    break;
                }
            }

            // Unpool and move GameObject to spawnPosition. Add item to spawnablesInGameList.
            // random spawn position on x and z axis min/max of spawn values. spawn position does not go above 1 on the y axis.  <---- ???
            if (spawnPooledObject == true)
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

            yield return new WaitForSeconds(currentWaitTime);
        }
    }

    #region PUBLIC POOLING METHODS

    // Sets passed object to inactive and pools it. Called by OutOfBounds and BallCollisions Script
    public void PoolObject(GameObject _pooledObj)
    {
        _pooledObj.SetActive(false);
        pooledObjectsList.Add(_pooledObj);
        spawnablesInGame.Remove(_pooledObj);
    }

    // Pools all items in spawnablesInGameList into pooledObjectsList. Deactivates pooled items.
    public void PoolAllSpawnables()
    {
        pooledObjectsList.AddRange(spawnablesInGame);
        spawnablesInGame.RemoveRange(0, spawnablesInGame.Count);

        for (int index = 0; index < SpawnManager.instance.pooledObjectsList.Count; index++)
        {
            GameObject currentBall = (GameObject)SpawnManager.instance.pooledObjectsList[index];
            currentBall.SetActive(false);
        }

        //spawnablesInGame.Clear();
        if (SpawnManager.instance.spawnablesInGame.Count > 0)
            Debug.LogError("spawnablesInGame list should be empty");
    }

    #endregion
}


