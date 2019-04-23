using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Updates UI for Score Panel (Difficulty text, KOs). Updates UI score text. 
/// References DataManagement script.
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    private void OnEnable()
    {
        if (instance == null)
            instance = this;
    }

    private void OnDisable()
    {
        instance = null;
    }

    //public GameObject timeTextObj;
    public Text ballsKnockedOutText;
    public Text inGameHighScoreText;

    internal void ScoreUpdate (int _ballsKnockedOut)
    {
        //_currentTime = (int)_currentTime;
        //timeTextObj.GetComponent<Text>().text = ("CUTE BALL'S LIVED FOR: " + _currentTime);
        ballsKnockedOutText.text = ("KOs: " + _ballsKnockedOut);
        inGameHighScoreText.text = ("HIGH SCORE: " + DataManagement.instance.dManHighScore);
    }

    public Text currentDifficultyText;
    
    internal void DifficultyUpdate (int _difficulty)
    {
        currentDifficultyText.text = ("DIFFICULTY: " + _difficulty);
    }

    public GameObject retryPanel;
    public Text finalScoreText;
    public Text retryHighScoreText;

    internal void RetryUI(int _finalScore, int _highScore, bool _activePanel)
    {
        retryPanel.SetActive(_activePanel);
        
        if (ScoreManager.instance.previouslySavedScore < _finalScore)
        {
            retryHighScoreText.text = ("NEW HIGH SCORE: " + _highScore);
        }

        else
        {
            finalScoreText.text = ("SCORE: " + _finalScore);
            retryHighScoreText.text = ("HIGH SCORE: " + _highScore);
        }       
    }
}
