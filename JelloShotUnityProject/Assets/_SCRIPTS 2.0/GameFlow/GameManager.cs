using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Manages when game state changes from gameplay to retry states. Handles OnStart, OnUpdate, and OnFixedUpdate events. 
/// Calls UiManager, ScoreManager, GroundScript, and BallManager. References ScoreManager. 
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
    // instance of game manager script
    internal static GameManager instance;
    // variable for GameState enum
    internal GameState state;
    // events
    internal delegate void OnUpdate();
    internal static event OnUpdate OnUpdateEvent; 
    internal delegate void OnFixedUpdate();
    internal static event OnFixedUpdate OnFixedUpdateEvent; 
    internal delegate void OnRetry();
    internal static event OnRetry OnRetryEvent;

    public bool retryUiIsOn = false;
    public int finalScore;

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
        OnUpdateEvent();

        _CurrentTime += Time.deltaTime;

        if (TapChecker.instance._NumberOfTapsInARow > 1 && retryUiIsOn)
        {
            Restart();
        }

        if (retryUiIsOn == false)
            UIManager.instance.UIScoreUpdate(ScoreManager.instance.ballsKnockedOut);
    }

    private void FixedUpdate()
    {
        OnFixedUpdateEvent();
    }
    #endregion

    public void LevelEnd()
    {
        Time.timeScale = 0f;

        ScoreManager.instance.CountScore(DifficultyAdjuster.instance._CurrentDifficulty);

        retryUiIsOn = true;
        UIManager.instance.RetryUI(finalScore, DataManagement.instance.dManHighScore, retryUiIsOn);

        SpawnManager.instance.ResetSpawnListsAndTimers();
    }

    private void Restart()
    {
        _CurrentTime = 0;
        _PlayerGameObject.transform.position = _PlayerStartPos;

        ScoreManager.instance.LevelScoreSetup();
        DifficultyAdjuster.instance.SetStartingDifficulty();

        retryUiIsOn = false;
        UIManager.instance.RetryUI(finalScore, DataManagement.instance.dManHighScore, retryUiIsOn);

        
        Time.timeScale = 1f;
    }
}
