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

    public void UIScoreUpdate(ScoreInfo _scoreInfo)
    {
        ballsKnockedOutText.text = _scoreInfo.kOScore.ToString(); ;
        inGameHighScoreText.text = ("BEST:" + DataManagement.instance.dManDiffKOScore);
    }

    public Text currentDifficultyText;
    public void UpdateDifficulty (int _difficulty)
    {
        currentDifficultyText.text = ("DIFFICULTY: " + _difficulty);
    }

    public GameObject retryPanel;
    public Text finalScoreText;
    public Text retryHighScoreText;
    public void InputRetryUI(ScoreInfo _scoreInfo, bool _isPanelActive)
    {
        retryPanel.SetActive(_isPanelActive);
        
        if (ScoreManager.instance.previouslySavedScore < _scoreInfo.kOScore)
        {
            finalScoreText.text = ("KOs: " + _scoreInfo.kOScore);
            retryHighScoreText.text = ("Best: " + _scoreInfo.bestScore);
        }
        else
        {
            finalScoreText.text = ("KOs: " + _scoreInfo.kOScore);
            retryHighScoreText.text = ("Best: " + _scoreInfo.kOScore);
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
