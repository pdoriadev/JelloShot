using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.UI;
/// <summary>
/// Manages ballsKnockedOut. Calculates Score.
/// Called by OutOfBoundsScript and GameManager. Referenced by UIManager for previouslySavedScore. 
/// </summary>
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public delegate void OnScoreUpdate(ScoreInfo _scoreInfo);
    public static event OnScoreUpdate OnScoreUpdateEvent;

    private ScoreInfo _ScoreInfo;
    public ScoreInfo scoreInfo { get { return _ScoreInfo; } }

    #region UNITY CALLBACKS
    private void OnEnable()
    {
        if (instance == null)
            instance = this;
        GameManager.onResetLevel += LevelScoreSetup;
    }

    private void OnDisable()
    {
        GameManager.onResetLevel -= LevelScoreSetup;
        instance = null;
    }

    private void Start()
    {
        DataManagement.instance.LoadData();
        _ScoreInfo.bestKOScore = DataManagement.instance.savedKOScore;
        _ScoreInfo.bestKOTimesDiffScore = DataManagement.instance.savedDiffKOScore;
    }
    #endregion

    public int ballsKnockedOut;
    public int previouslySavedScore;

    public void LevelScoreSetup()
    {
        _ScoreInfo.kOScore = 0;
        _ScoreInfo.bestKOScore = DataManagement.instance.savedKOScore;
        _ScoreInfo.bestKOTimesDiffScore = DataManagement.instance.savedDiffKOScore;
    }

    public void IterateBallsKoScore()
    {      
        _ScoreInfo.kOScore++;

        if (OnScoreUpdateEvent != null)
            OnScoreUpdateEvent(_ScoreInfo);
        else Debug.Log(OnScoreUpdateEvent + " is null");
    }

    public int CountScore(int difficulty)
    {
        _ScoreInfo.KOTimesDifficulty = difficulty * _ScoreInfo.kOScore;

        // if this round's KO times Difficulty score is better than its high score, replace it. 
        if (_ScoreInfo.KOTimesDifficulty > DataManagement.instance.savedDiffKOScore)
        {
            DataManagement.instance.savedDiffKOScore = _ScoreInfo.KOTimesDifficulty;
            DataManagement.instance.SaveData();
        }
        // if this round's KO score is better than its high score, replace it. 
        if (_ScoreInfo.kOScore > DataManagement.instance.savedKOScore)
        {
            DataManagement.instance.savedKOScore = _ScoreInfo.kOScore;
            DataManagement.instance.SaveData();
        }

        return _ScoreInfo.KOTimesDifficulty;
    }
}

public struct ScoreInfo
{
    public int bestKOScore;
    public int bestKOTimesDiffScore;
    public int kOScore;
    public int KOTimesDifficulty;

    public ScoreInfo(int _bestScore, int _currentScore, int _ballsKOd, int _bestKOTimesDiff)
    {
        bestKOScore = _bestScore;
        bestKOTimesDiffScore = _bestKOTimesDiff;
        KOTimesDifficulty = _currentScore;
        kOScore = _ballsKOd;
    }
}
