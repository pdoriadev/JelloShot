using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DamageableLerpOnDmg : Damageable
{
    private SizeLerper _SizerInstance = null;
    private ColorLerper _ColorerInstance = null;
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

    #region UNITY CALLBACKS
    void OnEnable()
    {
        if (GetComponent<SizeLerper>() == null)
        {
            _SizerInstance = gameObject.AddComponent<SizeLerper>() as SizeLerper;
        }
        else
            _SizerInstance = GetComponent<SizeLerper>();

        maxHealth = _NewMaxHealth;
        currentHealth = _NewMaxHealth;
        _WaitTime = _SizerInstance.lerpTime;
    }

    void OnDisable()
    {
        _SizerInstance = null;
    }
    #endregion

    public override void OnTakeDmg()
    {
        base.OnTakeDmg();
        _SizerInstance.StartLerp(currentHealth, maxHealth);
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

        DamageableLerpOnDmg script = (DamageableLerpOnDmg)target;

        // draw checkbox for the bool
        if (script.isSizeLerper == true && script.gameObject.GetComponent<SizeLerper>() == null)
        {
            script.gameObject.AddComponent<SizeLerper>();
        }
        else if (script.isSizeLerper == false && script.gameObject.GetComponent<SizeLerper>() != null)
        {

            DestroyImmediate(script.gameObject.GetComponent<SizeLerper>());
        }

        if (script.isColorLerper == true && script.gameObject.GetComponent<ColorLerper>() == null)
        {
            script.gameObject.AddComponent<ColorLerper>();
        }
        else if (script.isColorLerper == false && script.gameObject.GetComponent<ColorLerper>() != null)
        {
            DestroyImmediate(script.gameObject.GetComponent<ColorLerper>(), true);
        }

        if (script.gameObject.GetComponent<ColorLerper>() == null && script.gameObject.GetComponent<SizeLerper>() == null)
        {
            Debug.Log("ERROR ERROR: DamageableLerpOnDmg has nothing to lerp!");
        }
    }
}
#endif