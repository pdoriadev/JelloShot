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

    #region UNITY CALLBACKS
    private void OnEnable()
    {
        if (instance == null)
            instance = this;
        ScoreManager.OnScoreUpdateEvent += UIScoreUpdate;
        GameManager.onEnterMainMenuEvent += EnterMainMenuListener;
        GameManager.onExitMainMenuEvent += ExitMainMenuListener;
        GameManager.onEnterGameplayEvent += EnterGameplayListener;
        GameManager.onPauseGameplayEvent += EnterPauseListener;
    }
    private void OnDisable()
    {
        instance = null;
        ScoreManager.OnScoreUpdateEvent -= UIScoreUpdate;
    }
    #endregion

    public Text ballsKnockedOutText;
    public Text inGameHighScoreText;

    #region PUBLIC METHODS

    public void UIScoreUpdate(int _ballsKnockedOut)
    {
        ballsKnockedOutText.text = _ballsKnockedOut.ToString(); ;
        inGameHighScoreText.text = ("HIGH SCORE: " + DataManagement.instance.dManHighScore);
    }

    public Text currentDifficultyText;
    public void UpdateDifficulty (int _difficulty)
    {
        currentDifficultyText.text = ("DIFFICULTY: " + _difficulty);
    }

    public GameObject retryPanel;
    public Text finalScoreText;
    public Text retryHighScoreText;
    public void EnterRetryUI(int _finalScore, int _highScore, bool _isPanelActive)
    {
        retryPanel.SetActive(_isPanelActive);
        
        if (ScoreManager.instance.previouslySavedScore < _finalScore)
        {
            finalScoreText.text = ("SCORE: " + _finalScore);
            retryHighScoreText.text = ("NEW HIGH SCORE: " + _highScore);
        }
        else
        {
            finalScoreText.text = ("SCORE: " + _finalScore);
            retryHighScoreText.text = ("HIGH SCORE: " + _highScore);
        }       
    }
    #endregion

    public GameObject MainMenuPanel;
    private void EnterMainMenuListener()
    {
        MainMenuPanel.SetActive(true);
    }
    private void ExitMainMenuListener()
    {
        MainMenuPanel.SetActive(false);
    }

    private void EnterGameplayListener()
    {

    }

    private void EnterPauseListener()
    {

    }
    private void ExitPauseListener()
    {

    }

}
