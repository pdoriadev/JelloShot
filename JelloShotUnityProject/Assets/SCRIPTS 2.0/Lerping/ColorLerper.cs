using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Lerps between green and red depending on the values passed to it.
public class ColorLerper : LerperBase
{
    private void OnEnable()
    {
        _ColorAtSpawn = _ObjSpriteRenderer.material.color;
    }

    private void OnDisable()
    {
        _ObjSpriteRenderer.material.color = _ColorAtSpawn;
    }

    private SpriteRenderer _ObjSpriteRenderer;

    [HeaderAttribute("Color")]
    [SerializeField]
    private Color _ColorAtSpawn;
    [SerializeField]
    private Color _StartLerpColor;
    [SerializeField]
    private Color _TargetLerpColor;

    public override void StartLerp(float _currentHealth, float _maxHealth)
    {
        _StartLerpColor = _ObjSpriteRenderer.material.color;

        float _ScaleDifference = (_currentHealth + 0.1f) / _maxHealth;
        _TargetLerpColor = new Color(_ColorAtSpawn.a * _ScaleDifference, _ColorAtSpawn.g * _ScaleDifference, _ColorAtSpawn.b * _ScaleDifference);
        base.StartLerp(_currentHealth, _maxHealth);
    }

    protected override void HandleLerp()
    {
        base.HandleLerp();
        _ObjSpriteRenderer.material.color = Color.Lerp(_StartLerpColor, _TargetLerpColor, lerpPercentageComplete);
    }
}
