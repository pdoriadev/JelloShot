using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// DOUBLE TAP FEATURE TO-DO: check for one tap. Add one to numberOfTaps int. Start a timer from first tap.
// If timer exceeds double tap time window, then numberOfTaps = 0. If another tap occurs before timer exceeds
// double tap time window, then game is restarted.
// Checks for double tap on screen. Could easily make this a repurposable tap input script that checks how many taps have happened.
public class TapChecker : MonoBehaviour
{
    public static TapChecker instance;

    private void OnEnable()
    {
        if (instance == null)
            instance = this;
        GameManager.OnUpdateEvent += OnUpdateHandler;
    }
    private void OnDisable()
    {
        instance = null;
        GameManager.OnUpdateEvent -= OnUpdateHandler;
    }

    public int _NumberOfTapsInARow;

    private int _TouchCount;
    [SerializeField]
    private float _MaxTimeBetweenTaps;
    [SerializeField]
    private float _TimeSinceLastTap = 0;
    [SerializeField]
    private bool _HaveTapped;

    private void OnUpdateHandler()
    {
        _TouchCount = Input.touchCount;

        if (_HaveTapped)
        {
            _TimeSinceLastTap += Time.deltaTime;

            if (_TimeSinceLastTap >= _MaxTimeBetweenTaps)
            {
                _NumberOfTapsInARow = 0;
                _TimeSinceLastTap = 0;
                _HaveTapped = false;
            }
        }

        if (_TouchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                _HaveTapped = true;
                _NumberOfTapsInARow++;          
            }

            if (Input.touchCount == 2 && Input.GetTouch(1).phase == TouchPhase.Ended)
            {
                _HaveTapped = true;
                _NumberOfTapsInARow++;
            }
        }
    }
}
