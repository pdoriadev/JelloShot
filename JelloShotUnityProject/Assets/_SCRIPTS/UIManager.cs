using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Updates UI for Score Panel (Difficulty text, KOs). Updates UI score text. 
/// References DataManagement script.
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    #region UNITY CALLBACKS
    private void OnEnable()
    {
        if (instance == null)
            instance = this;
        ScoreManager.OnScoreUpdateEvent += UIScoreUpdate;
        GameManager.onEnterMainMenuEvent += EnterMainMenuListener;
        GameManager.onExitMainMenuEvent += ExitMainMenuListener;
        GameManager.onEnterGameplayEvent += EnterGameplayListener;
        GameManager.onPauseGameplayEvent += EnterPauseListener;
        GameManager.onUnPauseGameplayEvent += ExitPauseListener;
        GameManager.onEnterTutorialEvent += onEnterTutorialListener;
        GameManager.onExitTutorialEvent += onExitTutorialListener;
        _GroundDmgble.onTakeDamageEvent += OnGroundDamaged;
        _GroundDeathHandler.preKillEvent += OnGroundDied;
    }
    private void OnDisable()
    {
        instance = null;
        ScoreManager.OnScoreUpdateEvent -= UIScoreUpdate;
        GameManager.onEnterMainMenuEvent -= EnterMainMenuListener;
        GameManager.onExitMainMenuEvent -= ExitMainMenuListener;
        GameManager.onEnterGameplayEvent -= EnterGameplayListener;
        GameManager.onPauseGameplayEvent -= EnterPauseListener;
        GameManager.onUnPauseGameplayEvent -= ExitPauseListener;
        GameManager.onEnterTutorialEvent -= onEnterTutorialListener;
        GameManager.onExitTutorialEvent -= onExitTutorialListener;
        _GroundDmgble.onTakeDamageEvent -= OnGroundDamaged;
        _GroundDeathHandler.preKillEvent -= OnGroundDied;
    }
    #endregion

    public Text ballsKnockedOutText;
    public Text inGameHighScoreText;

    #region PUBLIC METHODS

    public void UIScoreUpdate(ScoreInfo _scoreInfo)
    {
        ballsKnockedOutText.text = _scoreInfo.kOScore.ToString(); ;
        inGameHighScoreText.text = ("BEST:" + DataManagement.instance.dManDiffKOScore.ToString());
    }

    public Text currentDifficultyText;
    public void UpdateDifficulty (int _difficulty)
    {
        currentDifficultyText.text = ("DIFFICULTY: " + _difficulty);
    }

    public GameObject retryPanel;
    public Text finalScoreText;
    public Text retryHighScoreText;
    public void InputRetryUI(ScoreInfo _scoreInfo, bool _isPanelActive)
    {
        retryPanel.SetActive(_isPanelActive);
        
        if (ScoreManager.instance.previouslySavedScore < _scoreInfo.kOScore)
        {
            finalScoreText.text = ("KOs: " + _scoreInfo.kOScore);
            retryHighScoreText.text = ("Best: " + _scoreInfo.bestScore);
        }
        else
        {
            finalScoreText.text = ("KOs: " + _scoreInfo.kOScore);
            retryHighScoreText.text = ("Best: " + _scoreInfo.kOScore);
        }       
    }

    [SerializeField]
    private Text _GroundText;
    [SerializeField]
    private DamageableBase _GroundDmgble;
    [SerializeField]
    private DeathHandler _GroundDeathHandler;
    public void OnGroundDamaged(DamagedInfo _info)
    {
        _GroundText.text = "Ouch!";
    }
    public void OnGroundDied()
    {
        _GroundText.text = "):";
    }
    #endregion

    [SerializeField]
    private float _WaitTime = 1.5f;
    private bool _TutorialIsTrue;
    int i = 0;
    private IEnumerator TutorialTextCo()
    {
        while (_TutorialIsTrue)
        {
            if (i == 0)
                _GroundText.text = "You're a slingshot!";
            else if (i == 1)
            {
                _GroundText.text = "Tap, Drag, Release!";
            }

            yield return new WaitForSeconds(_WaitTime);

            if (i == 2)
            {
                _GroundText.text = "Stop the Falling Things!";
            }
            if (i == 3)
            {
                _GroundText.text = "Don't Let Them Hit Me!";
            }

            ++i;
        }

        yield return null;
    }

    #region StateEventListeners

    public GameObject MainMenuPanel;
    private void EnterMainMenuListener()
    {
        MainMenuPanel.SetActive(true);
    }
    private void ExitMainMenuListener()
    {
        MainMenuPanel.SetActive(false);
    }

    public GameObject groundVoicePanel; 
    private void onEnterTutorialListener()
    {
        groundVoicePanel.SetActive(true);
        _TutorialIsTrue = true;
        StartCoroutine(TutorialTextCo());
    }
    private void onExitTutorialListener()
    {
        if (GameManager.instance.isTutorial == false)
            _TutorialIsTrue = false;
    }

    private void EnterGameplayListener()
    {
        _GroundText.text = "(:";
    }

    private string _PreviousText;
    private void EnterPauseListener()
    {
        _PreviousText = _GroundText.text;

        if (_TutorialIsTrue)
            StopCoroutine(TutorialTextCo());

        _GroundText.text = "Paused...";
    }
    private void ExitPauseListener()
    {
        Debug.Log("UI Exit Pause");
        _GroundText.text = _PreviousText;
        if (_TutorialIsTrue)
            StartCoroutine(TutorialTextCo());
    }
    #endregion

}
