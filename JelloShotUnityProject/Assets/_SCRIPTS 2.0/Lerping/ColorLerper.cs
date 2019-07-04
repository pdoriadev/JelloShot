﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Lerps between green and red depending on the values passed to it.
public class ColorLerper : LerperBase
{
    private void OnEnable()
    {
        if (GetComponent<SpriteRenderer>() == null)
        {
            _ObjSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        else
            _ObjSpriteRenderer = GetComponent<SpriteRenderer>();

        _ColorAtSpawn = _ObjSpriteRenderer.color;
    }

    private void OnDisable()
    {
        base.EndLerp();
        _ObjSpriteRenderer.color = _ColorAtSpawn;
    }

    private SpriteRenderer _ObjSpriteRenderer;

    [HeaderAttribute("Color")]
    [SerializeField]
    private Color _ColorAtSpawn;
    private Color _StartLerpColor;
    private Color _NextLerpColor;
    // Set in the inspector
    [SerializeField]
    private Color _FinalColor;


    public override void StartLerp(float _currentHealth, float _maxHealth)
    {
        _StartLerpColor = _ObjSpriteRenderer.color;

        float _Scaler = (_maxHealth - _currentHealth) / _maxHealth;
        _NextLerpColor = Color.Lerp(_ColorAtSpawn, _FinalColor, _Scaler);
        base.StartLerp(_currentHealth, _maxHealth);
    }

    protected override void HandleLerp()
    {
        base.HandleLerp();
        _ObjSpriteRenderer.color = Color.Lerp(_StartLerpColor, _NextLerpColor, lerpPercentageComplete);
    }
}