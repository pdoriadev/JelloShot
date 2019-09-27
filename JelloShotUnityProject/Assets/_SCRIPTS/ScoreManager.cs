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
        previouslySavedScore = DataManagement.instance.dManDiffKOScore;
    }
    #endregion

    public int ballsKnockedOut;
    public int previouslySavedScore;

    public void LevelScoreSetup()
    {
        //ballsKnockedOut = 0;
        //previouslySavedScore = DataManagement.instance.dManHighScore;
        _ScoreInfo.kOScore = 0;
        _ScoreInfo.bestScore = DataManagement.instance.dManDiffKOScore;
    }

    public void IterateBallsKoScore()
    {      
        //ballsKnockedOut++;
        _ScoreInfo.kOScore++;

        if (OnScoreUpdateEvent != null)
            OnScoreUpdateEvent(_ScoreInfo);
        else Debug.Log(OnScoreUpdateEvent + " is null");
    }

    public int CountScore(int difficulty)
    {
        _ScoreInfo.KOTimesDifficulty = difficulty * _ScoreInfo.kOScore;

        if (_ScoreInfo.KOTimesDifficulty > DataManagement.instance.dManDiffKOScore)
        {
            DataManagement.instance.dManDiffKOScore = _ScoreInfo.KOTimesDifficulty;
            DataManagement.instance.SaveData();
        }
        if (_ScoreInfo.kOScore > DataManagement.instance.dManKOScore)
        {
            DataManagement.instance.dManKOScore = _ScoreInfo.kOScore;
            DataManagement.instance.SaveData();
        }

        return _ScoreInfo.KOTimesDifficulty;
    }
}

public struct ScoreInfo
{
    public int bestScore;
    public int kOScore;
    public int KOTimesDifficulty;

    public ScoreInfo(int _bestScore, int _currentScore, int _ballsKOd)
    {
        bestScore = _bestScore;
        KOTimesDifficulty = _currentScore;
        kOScore = _ballsKOd;
    }
}
