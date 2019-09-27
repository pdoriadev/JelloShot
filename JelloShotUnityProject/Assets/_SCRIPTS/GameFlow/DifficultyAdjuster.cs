using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// SETS STARTING DIFFICULTY
/// Set SpawnManager's spawnMinWait and spawnMaxWait to equal SpawnManager's startMinSpawnWait and startMaxSpawnWait. Repeats
/// for every difficulty level. 
/// 
/// INCREASES DIFFICULTY BY KO NUMBER
/// Increases difficulty for every number of KOs after last difficulty level change. Calls UI Script to update Difficulty UI.
/// </summary>
public class DifficultyAdjuster : MonoBehaviour
{
    public static DifficultyAdjuster instance;

    #region UNITY CALLBACKS
    private void OnEnable()
    {
        if (instance == null)
            instance = this;
    }
    private void OnDisable()
    {
        instance = null;
        StopAllCoroutines();
    }

    private void Start()
    {
        SetStartingDifficulty();
    }
    #endregion

    private int _CurrentDiff;
    public int currentDiff { get { return _CurrentDiff; } }
    [SerializeField]
    private int _BeginnerDiff = 10;
    public int beginnerDiff { get { return _BeginnerDiff; } }
    public int startingDiff;
    [SerializeField]
    private float _MinSpawnRateChange; 
    [SerializeField]
    private float _MaxSpawnRateChange;

    public void SetStartingDifficulty()
    {
        SpawnManager.instance.currentMinWait = SpawnManager.instance.startMinSpawnWait;
        SpawnManager.instance.currentMaxWait = SpawnManager.instance.startMaxSpawnWait;

        for (int i = 0; i <= startingDiff; i++)
        {
            SpawnManager.instance.currentMinWait -= _MinSpawnRateChange;
            SpawnManager.instance.currentMaxWait -= _MaxSpawnRateChange;

            if (SpawnManager.instance.currentMaxWait < SpawnManager.instance.currentMinWait)
                SpawnManager.instance.currentMaxWait = SpawnManager.instance.currentMinWait + _MaxSpawnRateChange;

            _CurrentDiff = i;
        }
        UIManager.instance.UpdateDifficulty(currentDiff);

        _KOsNeededForChange = _StartingKOsNeededForChange;
        _IsChecking = true;
        StartCoroutine(ChangeDifficultyCheckerCo());
    }

    [SerializeField]
    private int _LastNumberOfBallsKOd = 0;
    [SerializeField]
    private int _KOsNeededForChange = 0;
    [SerializeField]
    private int _StartingKOsNeededForChange = 2;
    [SerializeField]
    private int _KODifferenceBetweenDifficulties = 5;
    [SerializeField]
    private int _KODifferenceBetweenDifficultiesIncrease = 1;
    [SerializeField]
    private float _WaitTime = 0.02f;

    private bool _IsChecking = false;
    IEnumerator ChangeDifficultyCheckerCo()
    {
        while (_IsChecking)
        {
            yield return new WaitForSeconds(_WaitTime);
            if (ShouldChangeDifficulty() ) { ChangeDifficulty(); }
        }

        yield return null;
    }

    bool ShouldChangeDifficulty()
    {
        if (ScoreManager.instance.ballsKnockedOut == _KOsNeededForChange)
            return true;
        else return false;
    }

    void ChangeDifficulty()
    {
        if ((SpawnManager.instance.currentMaxWait - _MaxSpawnRateChange) > SpawnManager.instance.currentMinWait)
        {
            SpawnManager.instance.currentMaxWait -= _MaxSpawnRateChange;
        }
        SpawnManager.instance.currentMinWait -= _MinSpawnRateChange;

        _LastNumberOfBallsKOd = ScoreManager.instance.ballsKnockedOut;
        _KODifferenceBetweenDifficulties += _KODifferenceBetweenDifficultiesIncrease;
        _KOsNeededForChange += _KODifferenceBetweenDifficulties;
        _CurrentDiff += 1;
        UIManager.instance.UpdateDifficulty(currentDiff);
    }
}
