using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SizeLerper))]
public class PlayerSizeController : MonoBehaviour
{
    //[Space(10)]
    //public SpriteRenderer playerSpriteRenderer;

    //// Player sprite colors dependsing on gamestate.
    //[SerializeField]
    //private Color NormalColor, FullyChargedColor;
    //private Color _PlayerSpriteColor;
    private Transform _PlayerTransform;
    private LerperBase _SizeLerper;

    public Vector3 minSize;
    public Vector3 maxSize;
    public Vector3 currentSize;

    public  float _ShotMagnitPercent;

    #region UNITY CALLBACKS
    private void OnEnable()
    {
        _SizeLerper = GetComponent<LerperBase>();
        _PlayerTransform = gameObject.transform;

        SlingShotMechanic.slingShotMovesEvent += ChangePlayeVisOnDrag;
        SlingShotMechanic.slingShotResetEvent += ChangePlayerVisOnRelease;
    }

    private void OnDisable()
    {
        SlingShotMechanic.slingShotMovesEvent -= ChangePlayeVisOnDrag;
        SlingShotMechanic.slingShotResetEvent -= ChangePlayerVisOnRelease;
    }
    #endregion

    void ChangePlayeVisOnDrag(TouchInfo _touchInfo, SlingShotInfo _slingShotInfo)
    {
        _ShotMagnitPercent = _slingShotInfo.shotVelocity.magnitude / ( _slingShotInfo.slingShotMaxMagnitude);

        currentSize = maxSize - (maxSize * _ShotMagnitPercent);

        if (currentSize.x < minSize.x)
        {
            currentSize = minSize;
        }

        _PlayerTransform.localScale = currentSize;
    }

    private float _SizeNumerator = 1f;
    private float _SizeDenominator = 1f;
    void ChangePlayerVisOnRelease()
    {
        _SizeLerper.StartLerp(_SizeNumerator, _SizeDenominator);
    }

}
