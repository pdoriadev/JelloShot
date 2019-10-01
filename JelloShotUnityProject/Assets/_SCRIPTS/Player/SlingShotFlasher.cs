using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ColorLerper))]
public class SlingShotFlasher : MonoBehaviour
{
    #region UNITY CALLBACKS
    [SerializeField]
    private SlingShotMechanic slingInstance;
    private ColorLerper lerperInstance;
    private void OnEnable()
    {
        if (slingInstance == null)
        {
            slingInstance = GameObject.FindGameObjectWithTag("Player").GetComponent<SlingShotMechanic>();
            Debug.LogWarning("SlingShotMechanic instance had to be found with FindGameObjectWithTag for " + gameObject.ToString());
        }
        if (lerperInstance == null)
        {
            lerperInstance = GetComponent<ColorLerper>();
        }

        slingInstance.slingShotMovesEvent += slingShotMovingObserver;
        slingInstance.slingShotResetEvent += slingShotResetObserver;
    }
    private void OnDisable()
    {
        slingInstance.slingShotMovesEvent -= slingShotMovingObserver;
        slingInstance.slingShotResetEvent -= slingShotResetObserver;

        slingInstance = null;
        lerperInstance = null;
    }
    #endregion

    private float _FlashRate;
    [SerializeField]
    private float _FlashRateMultiplier = 0.5f;
    // Takes how far sling shot is pulled back. Farther pulled back means faster flash rate.  
    void slingShotMovingObserver(TouchInfo _touchInfo, SlingShotInfo _slingShotInfo)
    {
        _FlashRate = (_slingShotInfo.slingMaxMagnitude / _slingShotInfo.shotVelocity.magnitude);
        lerperInstance.lerpTime = _FlashRate * _FlashRateMultiplier;

        FlashHandler();
    }
    void slingShotResetObserver()
    {
        // Resets sprite color back to normal
        lerperInstance.StartLerp(1, 0);
    }

    // Handles sprite color flashing
    private bool _FlashFromStartColor = true;
    void FlashHandler()
    {
        if (lerperInstance.isLerping == false)
        {
            if (_FlashFromStartColor)
            {
                lerperInstance.StartLerp(0, 1);
                _FlashFromStartColor = false;
            }
            else
            {
                lerperInstance.StartLerp(1, 0);
                _FlashFromStartColor = true;
            }
        }        
    }
}