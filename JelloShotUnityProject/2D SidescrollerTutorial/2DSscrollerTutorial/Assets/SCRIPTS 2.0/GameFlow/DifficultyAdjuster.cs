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
        GameManager.OnUpdateEvent += OnUpdateHandler;
        SetStartingDifficulty();
    }

    private void OnDisable()
    {
        instance = null;
        GameManager.OnUpdateEvent -= OnUpdateHandler;
    }
    #endregion

    public int _CurrentDifficulty;
    public int _StartingDifficulty;
    [SerializeField]
    private float _MinSpawnRateChange;
    [SerializeField]
    private float _MaxSpawnRateChange;

    public void SetStartingDifficulty()
    {
        SpawnManager.instance.spawnMinWait = SpawnManager.instance.startMinSpawnWait;
        SpawnManager.instance.spawnMaxWait = SpawnManager.instance.startMaxSpawnWait;

        for (int i = 0; i <= _StartingDifficulty; i++)
        {
            SpawnManager.instance.spawnMinWait -= _MinSpawnRateChange;
            SpawnManager.instance.spawnMaxWait -= _MaxSpawnRateChange;

            _CurrentDifficulty = i;
        }
        UIManager.instance.DifficultyUpdate(_CurrentDifficulty);
    }

    [SerializeField]
    private int _LastNumberOfBallsKOd = 0;
    [SerializeField]
    private int _KOsNeededForChange = 0;
    [SerializeField]
    private int _KODifferenceBetweenDifficulties = 5;

    public void OnUpdateHandler()
    {
        if (ScoreManager.instance.ballsKnockedOut == _KOsNeededForChange)
        {
            SpawnManager.instance.spawnMinWait -= _MinSpawnRateChange;
            SpawnManager.instance.spawnMaxWait -= _MaxSpawnRateChange;

            _LastNumberOfBallsKOd = ScoreManager.instance.ballsKnockedOut;
            _KOsNeededForChange += _KODifferenceBetweenDifficulties;
            _CurrentDifficulty += 1;
            UIManager.instance.DifficultyUpdate(_CurrentDifficulty);
        }
    }
}
