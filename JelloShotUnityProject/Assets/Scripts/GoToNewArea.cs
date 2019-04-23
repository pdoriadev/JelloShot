using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToNewArea : MonoBehaviour
{

    [SerializeField]
    public GameObject sp1, sp2;

	// Use this for initialization
	void Start ()
    {
        sp1 = this.gameObject; // The this keyword refers to the current instance of the class and is also 
                               // used as a modifier of the first parameter of an extension method.
       // sp2 = GameObject.FindGameObjectWithTag("sp2");
    }

    void OnTriggerStay2D(Collider2D trig)
    {
        if (Input.GetButtonDown("Jump"))
        {
            print("trighit");
            trig.gameObject.transform.position = sp2.gameObject.transform.position;
        }
    }
}
