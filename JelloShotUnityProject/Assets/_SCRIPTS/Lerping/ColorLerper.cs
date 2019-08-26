using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Lerps between green and red depending on the values passed to it.
public class ColorLerper : LerperBase
{
    private void OnEnable()
    {
        if (GetComponent<SpriteRenderer>() == null)
        {
            _SpriteRend = GetComponentInChildren<SpriteRenderer>();
        }
        else if (GetComponent<SpriteRenderer>() != null)
            _SpriteRend = GetComponent<SpriteRenderer>();
        else 
            Debug.LogError("No Sprite Renderer found on " + gameObject);

        _ColorAtSpawn = _SpriteRend.color;
    }

    private void OnDisable()
    {
        base.EndLerp();
        _SpriteRend.color = _ColorAtSpawn;
    }

    private SpriteRenderer _SpriteRend;

    [HeaderAttribute("Color")]
    [SerializeField]
    private Color _ColorAtSpawn;
    private Color _StartLerpColor;
    private Color _NextLerpColor;
    // Set in the inspector
    [SerializeField]
    private Color _FinalColor;


    public override void StartLerp(float _scalarNumerator, float _scalarDenominator)
    {
        _StartLerpColor = _SpriteRend.color;

        float _Scaler = (_scalarDenominator - _scalarNumerator) / _scalarDenominator;
        _NextLerpColor = Color.Lerp(_ColorAtSpawn, _FinalColor, _Scaler);
        base.StartLerp(_scalarNumerator, _scalarDenominator);
    }

    protected override void HandleLerp()
    {
        base.HandleLerp();
        _SpriteRend.color = Color.Lerp(_StartLerpColor, _NextLerpColor, lerpPercentageComplete);
    }
}
