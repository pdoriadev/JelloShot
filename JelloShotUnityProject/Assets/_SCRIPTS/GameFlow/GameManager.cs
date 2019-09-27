using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Manages game state changes and enter/exit events for each state. 
/// Calls UIManager, ScoreManager, DifficultyAdjuster, and SpawnManager. References ScoreManager and DataManagement. 
/// LevelEnd() is called by Ground game object's DeathHandler. 
/// </summary>
public enum GameState
{
    MainMenu,
    Tutorial, 
    Gameplay,
    PausedGameplay,
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
    public delegate void OnExitMainMenu();
    public static event OnExitMainMenu onExitMainMenuEvent;
    public delegate void OnEnterTutorial();
    public static event OnEnterTutorial onEnterTutorialEvent;
    public delegate void OnExitTutorial();
    public static event OnExitTutorial onExitTutorialEvent;
    public delegate void OnEnterGameplay();
    public static event OnEnterGameplay onEnterGameplayEvent;
    public delegate void OnExitGameplay();
    public static event OnExitGameplay onExitGameplayEvent;
    public delegate void OnPauseGameplay();
    public static event OnPauseGameplay onPauseGameplayEvent;
    public delegate void OnUnPauseGameplay();
    public static event OnUnPauseGameplay onUnPauseGameplayEvent;
    public delegate void OnRetryOrNot();
    public static event OnRetryOrNot onRetryOrNot;
    public delegate void OnResetGame();
    public static event OnResetGame onResetLevel;
    public delegate void OnLevelEnd();
    public static event OnLevelEnd onLevelEndEvent;

    #endregion

    public bool isTutorial = true;
    public void SetTutorialFalse()
    {
        isTutorial = false;
    }


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
        ChangeStateTo(GameState.MainMenu);
        _PlayerStartPos = _PlayerGameObject.transform.position;
        isTutorial = true;
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
            ResetLevel();
        }
        if (isRetryUIOn == false)
        {
            UIManager.instance.UIScoreUpdate(ScoreManager.instance.scoreInfo);
        }

        // If double tap during main menu, enter gameplay state
        if (state == GameState.MainMenu && TapChecker.instance._NumberOfTapsInARow > 1)
        {
            ChangeStateTo(GameState.Gameplay);
        }
    }

    #endregion


    #region PUBLIC STATE CONTROL METHODS
    public void EndGame()
    {
        if (onLevelEndEvent != null)
            onLevelEndEvent();
        else Debug.LogWarning("OnLevelEndEvent is null.");

        ChangeStateTo(GameState.LevelEnd);
    }

    public void ChangeStateTo(GameState _state)
    {
        // if-else chain to see what state is being exited
        if (state == GameState.Gameplay && _state != GameState.Gameplay)
            ExitGameplay();

        else if (state == GameState.PausedGameplay && _state != GameState.PausedGameplay)
            UnPauseGameplay();

        else if (state == GameState.LevelEnd && _state == GameState.Gameplay)
            ResetLevel();

        else if (state == GameState.MainMenu && _state != GameState.MainMenu)
            ExitMainMenu();

        else if (state == GameState.Tutorial && _state != GameState.Tutorial)
            ExitTutorial();


        state = _state;

        // if-else chain to see what state is being entered
        if (state == GameState.Gameplay)
        {
            EnterGameplay();
            return;
        }
        else if (state == GameState.LevelEnd)
        {
            EnterLevelEnd();
            return;
        }
        else if (state == GameState.PausedGameplay)
        {
            PauseGameplay();
            return;
        }
        else if (state == GameState.MainMenu)
        {
            EnterMainMenu();
            return;
        }
        else if (state == GameState.Tutorial)
        {
            EnterTutorial();
            return;
        }
    }

    #endregion

    // Methods called from the ChangeStateTo() method. 
    #region PRIVATE STATE CONTROL METHODS
    // Main Menu
    private void EnterMainMenu()
    {
        if (onEnterMainMenuEvent != null)
            onEnterMainMenuEvent();
        else Debug.LogWarning(onEnterMainMenuEvent.ToString() + " is null ");

        _PlayerGameObject.SetActive(false);

    }
    private void ExitMainMenu()
    {
        if (onExitMainMenuEvent != null)
            onExitMainMenuEvent();
        else Debug.LogWarning(onExitMainMenuEvent.ToString() + " is null ");
    }

    // Tutorial
    private void EnterTutorial()
    {
        if (onEnterTutorialEvent != null)
            onEnterTutorialEvent();
        else Debug.LogWarning(onExitMainMenuEvent.ToString() + " is null ");
    }
    private void ExitTutorial()
    {
        if (onExitTutorialEvent != null)
            onExitTutorialEvent();
        else Debug.LogWarning(onExitTutorialEvent.ToString() + "is null");
    }

    // Gameplay
    private void EnterGameplay()
    {
        _PlayerGameObject.SetActive(true);
        Time.timeScale = 1f;
        if (onEnterGameplayEvent != null)
            onEnterGameplayEvent();
        else Debug.LogWarning(onEnterGameplayEvent.ToString() + " is null ");
    }
    private void ExitGameplay()
    {
        _PlayerGameObject.SetActive(false);
        if (onExitGameplayEvent != null)
            onExitGameplayEvent();
        else Debug.LogWarning(onExitGameplayEvent.ToString() + " is null ");
    }

    // Pause
    private void PauseGameplay()
    {
        if (onPauseGameplayEvent != null)
            onPauseGameplayEvent();
        else Debug.LogWarning(onPauseGameplayEvent.ToString() + " is null ");

        Time.timeScale = 0;
    }
    private void UnPauseGameplay()
    {
        if (onUnPauseGameplayEvent != null)
            onUnPauseGameplayEvent();
        else Debug.LogWarning(onUnPauseGameplayEvent.ToString() + " is null ");
    }

        #region LEVEL END AND RETRY

    private int _FinalScore;
    private void EnterLevelEnd()
    {
        Time.timeScale = 0f;
        isRetryUIOn = true;

        ScoreManager.instance.CountScore(DifficultyAdjuster.instance.currentDiff);
        UIManager.instance.InputRetryUI(ScoreManager.instance.scoreInfo, isRetryUIOn);
        SpawnManager.instance.PoolAllSpawnables();
    }

    private void ResetLevel()
    {
        if (onResetLevel != null)
        {
            onResetLevel();
        }
        else Debug.LogWarning(onResetLevel + " is null.");

        _CurrentTime = 0;
        _PlayerGameObject.transform.position = _PlayerStartPos;

        UIManager.instance.UIScoreUpdate(ScoreManager.instance.scoreInfo);
        DifficultyAdjuster.instance.SetStartingDifficulty();

        isRetryUIOn = false;
        UIManager.instance.InputRetryUI(ScoreManager.instance.scoreInfo, isRetryUIOn);
    }
        #endregion  

    #endregion
}
