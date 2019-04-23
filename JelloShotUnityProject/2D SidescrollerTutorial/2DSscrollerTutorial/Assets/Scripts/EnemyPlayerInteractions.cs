using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyPlayerInteractions : MonoBehaviour {

    Rigidbody2D eRB;
    [SerializeField]    Vector2 distResult;
    [SerializeField]    private float xDist, xRightBounds, xLeftBounds, yDist, yBotUnsfBnds, yBotSfBnds, yTopSfBnds;
    [SerializeField]    private bool ySafeBounds, yUnsfBounds, inXBounds, eDead = false;
    Scene currentScene;
    //Scripts
    PlayerMove playerMoveScript; // For player bounce off enemy.
    Player_Score playerScoreScript; // Updating player score after bouncing off enemy
    LevelReset resetScript; // Resetting values if level reset
    Shared_Vars shared_VarsScript; // Getting player RigidBody

    void Start()  
    {
        FindScriptVars();
    }
	
    void FindScriptVars()
    {
        playerMoveScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
        playerScoreScript = GameObject.FindGameObjectWithTag("LevelEnd").GetComponent<Player_Score>();
        resetScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<LevelReset>();
        shared_VarsScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Shared_Vars>();
    }

	void FixedUpdate ()
    {
        shared_VarsScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Shared_Vars>();

        {
            foreach (GameObject item in resetScript.respawnObjsList)
            {
                eRB = item.GetComponent<Rigidbody2D>();
                CheckDistance(); // Connect enemy objects to enemy distance check functionality. 

                if (ySafeBounds == true && inXBounds == true)
                {
                    item.GetComponent<Goom_Move>().EnemyDeath();
                    playerMoveScript.isBouncing = true;
                    playerScoreScript.playerScore++;
                    eDead = true;
                }

                if (yUnsfBounds == true && inXBounds == true)
                {
                    shared_VarsScript.playerDead = true;
                    resetScript.Reset();
                }
            }
        }   
    }

    void CheckDistance()
    {
        xDist = shared_VarsScript.prb.transform.position.x - eRB.transform.position.x; // xDistance between player and enemy
        yDist = shared_VarsScript.prb.transform.position.y - eRB.transform.position.y; // yDistance between player and enemy

        distResult = new Vector2(xDist, yDist);

        if (xDist > xLeftBounds && xDist < xRightBounds) inXBounds = true; // When player and Enemy are in the same X area

        else inXBounds = false;

        if (yDist > yBotSfBnds && yDist < yTopSfBnds) // Y area where it's safe to land on top of enemy
        {
            ySafeBounds = true;
            yUnsfBounds = false;
        }

        if (yDist < yBotSfBnds && yDist > yBotUnsfBnds) // Y area where enemy will hurt player 
        {
            yUnsfBounds = true;
            ySafeBounds = false;
        }

        if (yDist < yBotUnsfBnds || yDist > yTopSfBnds) // if neither hurt Y area or safe to land Y area is true
        {
            ySafeBounds = false;
            yUnsfBounds = false;
        }
    }

    //void Pkill()
    //{
    //    resetScript.Reset();
    //}

    //void Ekill()
    //{
    //    print("EkillCall");
    //    GetComponent<Goom_Move>().EnemyDeath();
    //    playerMoveScript.isBouncing = true;
    //    playerScoreScript.playerScore++;
    //    eDead = true;
    //}
}
