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
    public int ballsKnockedOut;
    public int previouslySavedScore;

    [SerializeField]
    private AudioClipPlayer _ClipPlayer;

    #region UNITY CALLBACKS
    private void OnEnable()
    {
        if (instance == null)
            instance = this;
        GameManager.onResetLevel += LevelScoreSetup;
        _ClipPlayer = GetComponent<AudioClipPlayer>();
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

    public void LevelScoreSetup()
    {
        _ScoreInfo.kOScore = 0;
        _ScoreInfo.bestKOScore = DataManagement.instance.savedKOScore;
        _ScoreInfo.bestKOTimesDiffScore = DataManagement.instance.savedDiffKOScore;
    }

    public void IterateBallsKoScore()
    {
        _ScoreInfo.kOScore++;
        PlaySound();

        if (OnScoreUpdateEvent != null)
            OnScoreUpdateEvent(_ScoreInfo);
        else Debug.Log(OnScoreUpdateEvent + " is null");
    }

    private float _PitchFloor = 0.5f;
    private float _PitchIterator = 0.1f;
    private void PlaySound()
    {
        _ClipPlayer.audioFileName = "SFX_Blop";
        if (_ClipPlayer.audioSource.pitch < (_PitchFloor + (_PitchIterator * 10))) 
        {
            _ClipPlayer.audioSource.pitch += _PitchIterator;
        }
        else _ClipPlayer.audioSource.pitch = _PitchFloor;
        _ClipPlayer.PlayAudio();
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
