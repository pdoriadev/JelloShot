using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Manages when game state changes. Handles OnLevelEnd event and OnRetry event.
/// Calls UIManager, ScoreManager, DifficultyAdjuster, and SpawnManager. References ScoreManager and DataManagement. 
/// LevelEnd() is called by Ground game object's DeathHandler. 
/// </summary>
public enum GameState
{
    Gameplay,
    Paused,
    End
}
public enum GameLayers
{
    // Layers 0-7 are Built-in. Layers 8 and after are User Layers.
    Default, //0
    TransparentFX, //1
    IgnoreRaycast, //2
    BuiltInLayerNum3, //3
    Water, //4
    UI, //5
    BuiltInLayerNumber6, //6
    BuiltInLayerNum7, //7
    // USER LAYERS
    PostProcessing, //8
    Player, // 9
    Ground, // 10 
    Enemy, // 11
    Collectable, //12
    LevelBounds, //13
    BallsLayer, //14
    WallsLayer, // 15
    SideyTopseyWall, //16
    SideyTopsey, //17
    Bomb // 18
}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState state;
    // events
    public delegate void OnRetry();
    public static event OnRetry OnRetryEvent;
    public delegate void OnLevelEnd();
    public static event OnLevelEnd OnLevelEndEvent;

    private bool _RetryUIIsOn = false;
    public bool isRetryUIOn { get { return _RetryUIIsOn; }  set { _RetryUIIsOn = value; } }

    [SerializeField]
    private GameObject _PlayerGameObject;
    [SerializeField]
    private Vector2 _PlayerStartPos;
    [SerializeField]
    private float _CurrentTime;

    #region UNITY CALLBACKS
    private void OnEnable()
    {
        if (instance == null)
            instance = this;
        state = GameState.Gameplay;
        _PlayerStartPos = _PlayerGameObject.transform.position;
    }
    private void OnDisable()
    {
        instance = null;    
    }

    private void Update()
    {
        _CurrentTime += Time.deltaTime;

        if (isRetryUIOn && TapChecker.instance._NumberOfTapsInARow > 1)
        {
            Retry();
        }
        if (isRetryUIOn == false)
            UIManager.instance.UIScoreUpdate(ScoreManager.instance.ballsKnockedOut);
    }
    #endregion

    private int _FinalScore;
    public void LevelEnd()
    {
        OnLevelEndEvent();

        state = GameState.End;
        Time.timeScale = 0f;
        isRetryUIOn = true;

        _FinalScore = ScoreManager.instance.CountScore(DifficultyAdjuster.instance.currentDifficulty);
        UIManager.instance.RetryUI(_FinalScore, DataManagement.instance.dManHighScore, isRetryUIOn);
        SpawnManager.instance.PoolAllSpawnables();
    }

    private void Retry()
    {
        OnRetryEvent();

        _CurrentTime = 0;
        _PlayerGameObject.transform.position = _PlayerStartPos;

        UIManager.instance.UIScoreUpdate(ScoreManager.instance.ballsKnockedOut);
        DifficultyAdjuster.instance.SetStartingDifficulty();

        isRetryUIOn = false;
        UIManager.instance.RetryUI(_FinalScore, DataManagement.instance.dManHighScore, isRetryUIOn);

        state = GameState.Gameplay;
        Time.timeScale = 1f;
    }
}
