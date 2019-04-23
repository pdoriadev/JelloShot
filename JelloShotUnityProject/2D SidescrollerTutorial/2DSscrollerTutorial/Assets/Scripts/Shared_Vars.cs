using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shared_Vars : MonoBehaviour
{
    //Objects
    [SerializeField]
    internal GameObject plyrObj, enmyObj;
    //Rigidbodies
    [SerializeField]
    internal Rigidbody2D prb, erb;
    //Transforms
    [SerializeField]
    internal Transform plyrTrnsfm;
    [SerializeField]
    internal bool playerDead = false;
    // Scripts

    private void Start()
    {
        FindVars();
    }
    
    internal void FindVars()
    {
        plyrObj = GameObject.FindGameObjectWithTag("Player");
        enmyObj = GameObject.FindGameObjectWithTag("Enemy");

        prb = plyrObj.GetComponent<Rigidbody2D>();
        erb = enmyObj.GetComponent<Rigidbody2D>();

        plyrTrnsfm = plyrObj.GetComponent<Transform>();

        //shared_VarsScript = GetComponent<Shared_Vars>();
    }

}
