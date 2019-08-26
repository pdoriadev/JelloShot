using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SizeLerper))]
public class PlayerSizeController : MonoBehaviour
{
    private Transform _PlayerTransform;
    private LerperBase _SizeLerper;

    public Vector3 minSize = new Vector3(0.5f, 0.5f, 0.5f);
    public Vector3 maxSize;
    public Vector3 currentSize;

    public  float _ShotMagnitPercent;

    #region UNITY CALLBACKS
    private void OnEnable()
    {

        _SizeLerper = GetComponent<LerperBase>();
        _PlayerTransform = gameObject.transform;

        GetComponent<SlingShotMechanic>().slingShotMovesEvent += ChangePlayerVisOnDrag;
        GetComponent<SlingShotMechanic>().slingShotResetEvent += ChangePlayerVisOnRelease;
    }
    private void OnDisable()
    {
        GetComponent<SlingShotMechanic>().slingShotMovesEvent -= ChangePlayerVisOnDrag;
        GetComponent<SlingShotMechanic>().slingShotResetEvent -= ChangePlayerVisOnRelease;
    }
    #endregion

    void ChangePlayerVisOnDrag(TouchInfo _touchInfo, SlingShotInfo _slingShotInfo)
    {
        _ShotMagnitPercent = _slingShotInfo.shotVelocity.magnitude / _slingShotInfo.slingMaxMagnitude;
        currentSize = maxSize - (maxSize * _ShotMagnitPercent);

        if (currentSize.x < minSize.x || currentSize.y < minSize.y)
            currentSize = minSize;

        _PlayerTransform.localScale = currentSize;
    }

    void ChangePlayerVisOnRelease()
    {
        _SizeLerper.StartLerp(1, 1);
    }
}
