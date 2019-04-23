using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelReset : MonoBehaviour
{
    [SerializeField] internal List<GameObject> respawnObjsList = new List<GameObject>(), plyrRspnObjsList = new List<GameObject>();
    [SerializeField] List<Vector2> rspnObjsSpawnPoints = new List<Vector2>();
    [SerializeField] int vecIndex = 0;
    // [SerializeField] bool moreObjs = true;
    Vector2 newItemPosition;
    Shared_Vars shared_VarsScript;
    
    #region MonoBehavior Callbacks

    #endregion
    
    #region Private Functions

    void Start()
    {
        AddSpawnObjectsToList();
        AddSpwnObjsSpwnPntsToList();
    }
    //Create Seperate Lists for Player vs. Enemy
    void AddSpawnObjectsToList()
    {
        respawnObjsList.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        plyrRspnObjsList.Add(GameObject.FindGameObjectWithTag("Player"));
    }

    void AddSpwnObjsSpwnPntsToList()
    {
        foreach (GameObject item in respawnObjsList)
        {
            newItemPosition = item.transform.position;
            rspnObjsSpawnPoints.Add(newItemPosition);
        }
    }

    // Update is called once per frame
    void Update()
    {
        shared_VarsScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Shared_Vars>();
        vecIndex = 0;
        Reset();
    }

    #endregion

    #region Internal Functions

    internal void Reset()
    {
        if (Input.GetButtonDown("Cancel") || shared_VarsScript.plyrObj.transform.position.y < -10  || shared_VarsScript.playerDead == true)
        {
            shared_VarsScript.playerDead = false;

            foreach (GameObject item in respawnObjsList)
            {
                Object.Instantiate(item, rspnObjsSpawnPoints[vecIndex] /* Need to use corresponding vector for each item*/, transform.rotation);
                vecIndex += 1;
            }

            foreach (GameObject item in respawnObjsList)
            {
                Destroy(item);
            }

            shared_VarsScript.FindVars();
            AddSpawnObjectsToList();
            print(respawnObjsList[7]);// player. 3, 4, 5, and 6 were either an enemy or a enemy (clone). 
            print(respawnObjsList[8]);
            print(respawnObjsList[9]);
        }
    }

    #endregion

  
    #region FuckThisKeyboardAndYOuPetertttterterteterteterte

    #endregion

}
