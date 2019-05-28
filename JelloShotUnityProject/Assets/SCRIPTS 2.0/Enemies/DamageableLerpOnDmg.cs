using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DamageableLerpOnDmg : DamageableBase
{
    #region VARIABLES
    private SizeLerper _SizeLerper = null;
    private ColorLerper _ColorLerper = null;
    [SerializeField]
    private bool _IsSizeLerper;
    public bool isSizeLerper { get { return _IsSizeLerper; } set { _IsSizeLerper = value; } }
    [SerializeField]
    private bool _IsColorLerper;
    public bool isColorLerper { get { return _IsColorLerper; } set { _IsColorLerper = value; } }
    [SerializeField]
    private float _NewMaxHealth = 3;
    private float _WaitTime
    {
        get { return waitTime; }
        set
        {
            if (value >= 0)
                waitTime = value;
        }
    }
    #endregion

    #region UNITY CALLBACKS
    void OnEnable()
    {
        if (GetComponent<SizeLerper>() != null)
            _SizeLerper = GetComponent<SizeLerper>();
        if (GetComponent<ColorLerper>() != null)
            _ColorLerper = GetComponent<ColorLerper>();

        maxHealth = _NewMaxHealth;
        currentHealth = _NewMaxHealth;
        if (_SizeLerper != null)
        {
            _WaitTime = _SizeLerper.lerpTime;
        }
        if (_ColorLerper != null)
        {
            _WaitTime = _ColorLerper.lerpTime;
        }
    }

    void OnDisable()
    {
        _SizeLerper = null;
    }
    #endregion

    public override void OnTakeDmg()
    {
        base.OnTakeDmg();
        if (_SizeLerper != null)
            _SizeLerper.StartLerp(currentHealth, maxHealth);
        if (_ColorLerper != null)
            _ColorLerper.StartLerp(currentHealth, maxHealth);
    }

    protected override void OnDeath()
    {
        base.OnDeath();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(DamageableLerpOnDmg))]
public class LerperAdder : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // for other non-HideInInspector fields

        DamageableLerpOnDmg _DamageableLerpScript = (DamageableLerpOnDmg)target;

        SizeLerper _SLerper = (SizeLerper)target;
        if (_DamageableLerpScript.isSizeLerper == true && _DamageableLerpScript.gameObject.GetComponent<SizeLerper>() == null)
        {
            _DamageableLerpScript.gameObject.AddComponent<SizeLerper>();
            _SLerper = _DamageableLerpScript.gameObject.GetComponent<SizeLerper>();
        }
        else if (_DamageableLerpScript.isSizeLerper == false && _DamageableLerpScript.gameObject.GetComponent<SizeLerper>() != null)
        {
            DestroyImmediate(_SLerper, true);
        }

        ColorLerper _CLerper = (ColorLerper)target;
        if (_DamageableLerpScript.isColorLerper == true && _DamageableLerpScript.gameObject.GetComponent<ColorLerper>() == null)
        {
            _DamageableLerpScript.gameObject.AddComponent<ColorLerper>();
            _CLerper = _DamageableLerpScript.gameObject.GetComponent<ColorLerper>();
        }
        else if (_DamageableLerpScript.isColorLerper == false && _DamageableLerpScript.gameObject.GetComponent<ColorLerper>() != null)
        {
            DestroyImmediate(_CLerper, true);
        }

        if (_DamageableLerpScript.gameObject.GetComponent<ColorLerper>() == null && _DamageableLerpScript.gameObject.GetComponent<SizeLerper>() == null)
        {
            Debug.Log("ERROR ERROR: DamageableLerpOnDmg has nothing to lerp!");
        }
    }
}
#endif