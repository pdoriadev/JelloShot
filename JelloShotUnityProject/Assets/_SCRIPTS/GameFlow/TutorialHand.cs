using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHand : MonoBehaviour
{
    public static TutorialHand instance;

    private Animator _HandAnimator;

    #region UNITY CALLBACKS
    private void OnEnable()
    {
        if (instance == null)
            instance = this;
        GameManager.onEnterTutorialEvent += StartTutorial;
        GameManager.onExitTutorialEvent += ExitTutorial;

        _HandAnimator = GetComponent<Animator>();
    }
    private void OnDisable()
    {
        GameManager.onEnterTutorialEvent -= StartTutorial;
        GameManager.onExitTutorialEvent -= ExitTutorial;

        _HandAnimator = null;

        instance = null;
    }
    #endregion

    private Vector3 _StartPos;
    private void StartTutorial()
    {
        _HandAnimator.gameObject.SetActive(true);
        _HandAnimator.SetBool("IsAnimating", true);
    }

    // De-spawn start tutorial hand  
    private void ExitTutorial()
    {
        _HandAnimator.SetBool("IsAnimating", false);
        _HandAnimator.gameObject.transform.position = _StartPos;
        _HandAnimator.gameObject.SetActive(false);
    }

}
