using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Singleton Manager spawning loop for enemy spawnables. 
/// 1. Does NOT select what to spawn (see ItemSelector script). Only when to spawn. 
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
    public GameObject tutorialSpawnable;

    // Wait time before checking if can spawn
    public float startWait;
    // bool that stops spawning when false. 
    public bool isSpawning = false;
    public float spawnMinWaitFloor;
    public float spawnMaxWaitFloor;

    public float beginnerHandicapTime = 1f;
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

    [SerializeField]
    private float _PreviousWaitTime;
    private float _TimeGamePaused;

    #region UNITY CALLBACKS

    private void OnEnable()
    {
        if (instance == null)
            instance = this;
        GameManager.onResetLevel += PoolAllSpawnables;
        GameManager.onEnterTutorialEvent += onEnterTutorialListener;
        GameManager.onEnterGameplayEvent += onEnterGameplayListener;
        GameManager.onPauseGameplayEvent += onEnterPauseListener;
    }

    private void OnDisable()
    {
        GameManager.onResetLevel -= PoolAllSpawnables;
        GameManager.onEnterTutorialEvent -= onEnterTutorialListener;
        GameManager.onEnterGameplayEvent -= onEnterGameplayListener;
        GameManager.onPauseGameplayEvent -= onEnterPauseListener;

        instance = null;
    }

    private void Awake()
    {
        currentMinWait = startMinSpawnWait;
        currentMaxWait = startMaxSpawnWait;
    }

    #endregion

    #region StateEventListeners
    private bool _IsFirstIntro = true;
    private void onEnterTutorialListener()
    {
        if (_IsFirstIntro == true)
        {
            StartCoroutine(CoSpawnItem());
            _IsFirstIntro = false;
        }
        isSpawning = true;
    }
    private void onEnterGameplayListener()
    {
        isSpawning = true;
    }  
    private void onEnterPauseListener()
    {
        _TimeGamePaused = Time.time;
        isSpawning = false;
    }
    #endregion

    private bool _WasStoppedMidWait = false;
    private float _TimeStartedWaiting = 2;
    // Handles spawn loop
    IEnumerator CoSpawnItem()
    {
        yield return new WaitForSeconds(startWait);

        while (isSpawning)
        {
            if (_PreviousWaitTime < (_PreviousWaitTime + beginnerHandicapTime) 
                && GameManager.instance.state == GameState.Tutorial)
            {
                currentWaitTime = Random.Range(currentMinWait + beginnerHandicapTime, currentMaxWait);
            }
            else
            {
                currentWaitTime = Random.Range(currentMinWait, currentMaxWait);
            }
            _TimeStartedWaiting = Time.time;
            yield return new WaitForSeconds (currentWaitTime);
            _PreviousWaitTime = currentWaitTime;

            spawnPosition = new Vector3(Random.Range(-spawningZone.x, spawningZone.x), Random.Range(-spawningZone.y, spawningZone.y), 1);
            GameObject spawnable = null;
            int index = 0;
            bool spawnPooledObject = false;

            // 1. Select item to spawn
            if (GameManager.instance.state != GameState.Tutorial)
            {
                spawnable = ItemSelector.instance.SelectItem();
            }
            else spawnable = tutorialSpawnable;

            // 2. Check object pool for matching item
            for ( ; index < pooledObjectsList.Count; index++ )
            {
                // iterate through pooled obj list. if obj of equivalent type is found, spawn .
                if (pooledObjectsList[index].tag == spawnable.tag)
                {
                    spawnPooledObject = true;
                    spawnable = pooledObjectsList[index];
                    break;
                }
            }

            // 3a. If spawnPooledObject is true, Unpool and move GameObject to spawnPosition. Add item to spawnablesInGameList.
            // random spawn position on x and z axis min/max of spawn values. spawn position does not go above 1 on the y axis.  <---- ???
            if (spawnPooledObject == true)
            {
                spawnable.SetActive(true);
                spawnable.transform.position = spawnPosition + transform.TransformPoint(0, 0, 0);
                spawnablesInGame.Add(spawnable);
                pooledObjectsList.Remove(spawnable);
            }

            // 3b. Instantiate GameObject at spawn position. Add to spawnablesInGameList.
            else
            {
                spawnablesInGame.Add(Instantiate(spawnable, spawnPosition + transform.TransformPoint(0, 0, 0), gameObject.transform.rotation));
                spawnable.SetActive(true);
            }

        }
    }

    #region PUBLIC POOLING METHODS

    // Sets passed object to inactive and pools it. 
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

        if (SpawnManager.instance.spawnablesInGame.Count > 0)
            Debug.LogError("spawnablesInGame list should be empty");
    }

    #endregion
}


