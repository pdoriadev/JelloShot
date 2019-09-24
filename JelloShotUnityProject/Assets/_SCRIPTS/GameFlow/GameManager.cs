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
    MainMenu,
    Gameplay,
    Paused,
    LevelEnd
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


    #region EVENTS 

    public delegate void OnEnterMainMenu();
    public static event OnEnterMainMenu onEnterMainMenuEvent;
    public delegate void OnEnterGameplay();
    public static event OnEnterGameplay onEnterGameplayEvent;
    public delegate void OnRetry();
    public static event OnRetry onRetryEvent;
    public delegate void OnLevelEnd();
    public static event OnLevelEnd onLevelEndEvent;

    #endregion


    #region PROPERTIES

    private bool _RetryUIIsOn = false;
    public bool isRetryUIOn { get { return _RetryUIIsOn; }  set { _RetryUIIsOn = value; } }
    private GameState _State;
    public GameState state { get { return _State; } set { _State = value; } }

    #endregion


    #region PRIVATE SERIALIZED VARIABLES

    [SerializeField]
    private GameObject _PlayerGameObject;
    [SerializeField]
    private Vector2 _PlayerStartPos;
    [SerializeField]
    private float _CurrentTime;

    #endregion


    #region UNITY CALLBACKS

    private void OnEnable()
    {
        if (instance == null)
            instance = this;
        state = GameState.MainMenu;
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


    #region PUBLIC STATE CONTROL METHODS

    private int _FinalScore;
    public void EndGame()
    {
        if (onLevelEndEvent != null)
            onLevelEndEvent();
        else Debug.Log("OnLevelEndEvent is null.");

        state = GameState.LevelEnd;
        Time.timeScale = 0f;
        isRetryUIOn = true;

        _FinalScore = ScoreManager.instance.CountScore(DifficultyAdjuster.instance.currentDifficulty);
        UIManager.instance.RetryUI(_FinalScore, DataManagement.instance.dManHighScore, isRetryUIOn);
        SpawnManager.instance.PoolAllSpawnables();
    }

    public void ChangeStateTo(GameState _state)
    {
        if (state == GameState.Paused && _state == GameState.Paused)
        {
            state = GameState.Gameplay;
            return;
        }

        state = _state;

        if (state == GameState.MainMenu)
        {
            onEnterMainMenuEvent();
        }
        if (state == GameState.Paused)
        {
            PauseGame();
        }
    }

    #endregion


    #region PRIVATE STATE CONTROL METHODS

    private void PauseGame()
    {
        Time.timeScale = 0;
    }

    private void MainMenu()
    {

    }

    private void Retry()
    {
        if (onRetryEvent != null)
            onRetryEvent();
        else Debug.Log(onRetryEvent + " is null.");

        _CurrentTime = 0;
        _PlayerGameObject.transform.position = _PlayerStartPos;

        UIManager.instance.UIScoreUpdate(ScoreManager.instance.ballsKnockedOut);
        DifficultyAdjuster.instance.SetStartingDifficulty();

        isRetryUIOn = false;
        UIManager.instance.RetryUI(_FinalScore, DataManagement.instance.dManHighScore, isRetryUIOn);

        state = GameState.Gameplay;
        Time.timeScale = 1f;
    }

    #endregion
}
