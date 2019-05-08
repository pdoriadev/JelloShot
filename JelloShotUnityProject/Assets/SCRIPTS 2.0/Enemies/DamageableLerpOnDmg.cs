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
//[CustomEditor(typeof(DamageableLerpOnDmg))]
//public class RandomScript_Editor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector(); // for other non-HideInInspector fields

//        DamageableLerpOnDmg script = (DamageableLerpOnDmg)target;

//        // draw checkbox for the bool
//        script.isSizeLerper = EditorGUILayout.Toggle("Size Lerper", script.isSizeLerper);
//        if (EditorGUILayout.) ? script.isSizeLerper = true// if bool is true, show other fields
//        {
//            script.isSizeLerper = true;
//            //script.iField = EditorGUILayout.ObjectField("I Field", script.iField, typeof(InputField), true) as InputField;
//            //script.Template = EditorGUILayout.ObjectField("Template", script.Template, typeof(GameObject), true) as GameObject;
//        }


//    }
//}
#endif