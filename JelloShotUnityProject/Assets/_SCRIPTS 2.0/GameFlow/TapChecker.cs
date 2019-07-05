using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Checks for number of taps. Add one to numberOfTaps int. Start a timer from first tap.
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
    }
    private void OnDisable()
    {
        instance = null;
    }

    public int _NumberOfTapsInARow = 0;

    private int _TouchCount;
    [SerializeField]
    private float _MaxTimeBetweenTaps = 0.2f;
    [SerializeField]
    private float _TimeSinceLastTap = 0;
    [SerializeField]
    private bool _HaveTapped;

    private void Update()
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
