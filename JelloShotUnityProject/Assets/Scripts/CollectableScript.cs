using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableScript : MonoBehaviour
{
    [SerializeField]    LayerMask targetsLayerMask;
    [SerializeField]    float collectRange, curntDistance, coinWorth, coinPointWorth; 
    Player_Score pScoreScript;
    Shared_Vars shared_VarsScript;

    private void Start()
    {
        pScoreScript = GameObject.FindGameObjectWithTag("LevelEnd").GetComponent<Player_Score>();     
        shared_VarsScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Shared_Vars>();
    }

    internal void UpdateSharedVars()
    {
        shared_VarsScript.FindVars();
    }

    private void FixedUpdate()
    {
        shared_VarsScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Shared_Vars>();
        DistanceCheck();
    }

    void DistanceCheck() // Checks distance between player and coin
    {
        curntDistance = Vector2.Distance(shared_VarsScript.plyrTrnsfm.position, transform.position);
        
        if (curntDistance < collectRange) // collectRange currently set to 0.5 in the inspector
        {
            CollectCoin();  
        }
    }

    void CollectCoin() 
    {
        pScoreScript.coinsCollected += coinWorth; //coinWorth set to 1 in the inspector
        pScoreScript.playerScore += coinPointWorth;
        Destroy(gameObject); // Attached game object is destroyed if the above 2 lines are commented out.
    }
}
