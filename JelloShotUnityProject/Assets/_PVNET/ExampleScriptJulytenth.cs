using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleScriptJulytenth : MonoBehaviour {

    // Write variables here
    public bool isJumping = true;
    public bool isGrounded = false;
    
	// Use this for initialization
	void Start ()
    {
		// Write game instructions here

	}
	
	// Update is called once per frame
	void Update ()
    {
		// Write game instructions here
        if (Input.GetKey(KeyCode.Space))
        {
            isJumping = true;
            isGrounded = false;
        }

        else
        {
            isJumping = false;
            isGrounded = true;
        }
	}
}
