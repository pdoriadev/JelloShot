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
    internal delegate void OnScoreUpdate(int _param);
    internal static event OnScoreUpdate OnScoreUpdateEvent;

    #region UNITY CALLBACKS
    private void OnEnable()
    {
        if (instance == null)
            instance = this;
        GameManager.OnRetryEvent += LevelScoreSetup;
    }

    private void OnDisable()
    {
        GameManager.OnRetryEvent -= LevelScoreSetup;
        instance = null;
    }

    private void Start()
    {
        DataManagement.instance.LoadData();
        previouslySavedScore = DataManagement.instance.dManHighScore;
    }
    #endregion

    public int ballsKnockedOut;
    public int previouslySavedScore;

    public void LevelScoreSetup()
    {
        ballsKnockedOut = 0;
        previouslySavedScore = DataManagement.instance.dManHighScore;
    }

    public void IterateBallsKoScore()
    {      
        ballsKnockedOut++;
        if (OnScoreUpdateEvent != null)
            OnScoreUpdateEvent(ballsKnockedOut);
        else Debug.Log(OnScoreUpdateEvent + " is null");
    }

    public int CountScore(int difficulty)
    {
        int _FinalScore;
        _FinalScore = difficulty * ballsKnockedOut;

        if (_FinalScore > DataManagement.instance.dManHighScore)
        {
            DataManagement.instance.dManHighScore = _FinalScore;
            DataManagement.instance.SaveData();
        }

        return _FinalScore;
    }
}

