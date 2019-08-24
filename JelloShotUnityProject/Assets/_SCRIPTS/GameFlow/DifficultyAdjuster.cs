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

    public int currentDifficulty;
    public int startingDifficulty;
    [SerializeField]
    private float _MinSpawnRateChange;
    [SerializeField]
    private float _MaxSpawnRateChange;

    public void SetStartingDifficulty()
    {
        SpawnManager.instance.spawnMinWait = SpawnManager.instance.startMinSpawnWait;
        SpawnManager.instance.spawnMaxWait = SpawnManager.instance.startMaxSpawnWait;

        for (int i = 0; i <= startingDifficulty; i++)
        {
            SpawnManager.instance.spawnMinWait -= _MinSpawnRateChange;
            SpawnManager.instance.spawnMaxWait -= _MaxSpawnRateChange;

            if (SpawnManager.instance.spawnMaxWait < SpawnManager.instance.spawnMinWait)
                SpawnManager.instance.spawnMaxWait = SpawnManager.instance.spawnMinWait + _MaxSpawnRateChange;

            currentDifficulty = i;
        }
        UIManager.instance.DifficultyUpdate(currentDifficulty);

        _IsChecking = true;
        StartCoroutine(ChangeDifficultyCheckerCo());
    }

    [SerializeField]
    private int _LastNumberOfBallsKOd = 0;
    [SerializeField]
    private int _KOsNeededForChange = 0;
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
        if ((SpawnManager.instance.spawnMaxWait - _MaxSpawnRateChange) > SpawnManager.instance.spawnMinWait)
        {
            SpawnManager.instance.spawnMaxWait -= _MaxSpawnRateChange;
        }
        SpawnManager.instance.spawnMinWait -= _MinSpawnRateChange;

        _LastNumberOfBallsKOd = ScoreManager.instance.ballsKnockedOut;
        _KODifferenceBetweenDifficulties += _KODifferenceBetweenDifficultiesIncrease;
        _KOsNeededForChange += _KODifferenceBetweenDifficulties;
        currentDifficulty += 1;
        UIManager.instance.DifficultyUpdate(currentDifficulty);
    }
}
