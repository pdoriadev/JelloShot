using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MusicController : MonoBehaviour
{
    #region Unity Callbacks
    private bool _WasDisabled;
    private void Awake()
    {
        GameManager.onEnterMainMenuEvent += OnEnterMainMenuListener;
        GameManager.onEnterGameplayEvent += OnEnterGameplayListener;
        GameManager.onLevelEndEvent += OnEnterLevelEndListener;
    }
    private void OnEnable()
    {
        if (_WasDisabled)
        {
            GameManager.onEnterMainMenuEvent += OnEnterMainMenuListener;
            GameManager.onEnterGameplayEvent += OnEnterGameplayListener;
            GameManager.onLevelEndEvent += OnEnterLevelEndListener;
            _WasDisabled = false;
        }
    }
    private void OnDisable()
    {
        GameManager.onEnterMainMenuEvent -= OnEnterMainMenuListener;
        GameManager.onEnterGameplayEvent -= OnEnterGameplayListener;
        GameManager.onLevelEndEvent -= OnEnterLevelEndListener;
        _WasDisabled = true;
    }
    #endregion

    public UnityEvent playMenuEvent;
    public UnityEvent playGameplayEvent;
    public UnityEvent playLevelEndEvent;

    private bool _IsMenuMusic = false, _IsGameplayMusic = false, _IsLevelEndMusic = false;
    private void OnEnterMainMenuListener()
    {
        if (_IsMenuMusic == false)
        {
            playMenuEvent.Invoke();
            _IsGameplayMusic = false;
            _IsLevelEndMusic = false;
        }

    }
    private void OnEnterGameplayListener()
    {
        if (_IsGameplayMusic == false)
        {
            playGameplayEvent.Invoke();
            _IsMenuMusic = false;
            _IsLevelEndMusic = false;
        }
    }
    private void OnEnterLevelEndListener()
    {
        if (_IsLevelEndMusic == false)
        {
            playLevelEndEvent.Invoke();
            _IsGameplayMusic = false;
            _IsMenuMusic = false;
        }
    }
}
