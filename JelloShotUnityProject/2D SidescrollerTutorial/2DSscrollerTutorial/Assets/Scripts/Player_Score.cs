using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//namespace PitaBread
//{

    public class Player_Score : MonoBehaviour
    {
        [SerializeField]
        private GameObject timeUI, scoreUI, coinsUI;
        [SerializeField]
        internal float coinsCollected = 0, playerScore = 0;
        [SerializeField]
        private float timeLeft, rayLength;

        private void Start()
        {
            DataManagement.instance.LoadData();
        }

        void Update()
        {
            timeUI.GetComponent<Text>().text = ("Time  " + (int)timeLeft);
            scoreUI.GetComponent<Text>().text = ("Score  " + (int)playerScore);
            coinsUI.GetComponent<Text>().text = ("Coins " + (int)coinsCollected);

            timeLeft -= Time.deltaTime;

            if (timeLeft < 0)
            {
                Scene currentScene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(currentScene.name);
            }
        }

        void FixedUpdate()
        {
            RaycastUp();
        }

        void RaycastUp()
        {
            LayerMask playerMask = LayerMask.GetMask("Player");
            RaycastHit2D LevelEndDetect;
            LevelEndDetect = Physics2D.Raycast(transform.position, Vector2.up, rayLength, playerMask);
            Debug.DrawRay(transform.position, Vector2.up, Color.red, rayLength);
            if (LevelEndDetect)
            {
                CountScore();
                rayLength = 0;
            }
        }
        //void OnTriggerEnter2D (Collider2D other)  CountScore(); 

        void CountScore()
        {
            Debug.Log("Data says high score is currently " + DataManagement.instance.dManHighScore);
            playerScore += (int)(timeLeft * 10);
            DataManagement.instance.dManHighScore = (int)playerScore + (int)(timeLeft * 10);
            DataManagement.instance.SaveData();
            Debug.Log("Score:" + playerScore);
            Debug.Log("Now that we've added the score to DataManagement, Data says high score is " + DataManagement.instance.dManHighScore);
        }
    }
//}
