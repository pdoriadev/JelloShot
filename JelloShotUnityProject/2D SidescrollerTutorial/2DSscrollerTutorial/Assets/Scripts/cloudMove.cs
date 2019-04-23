using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloudMove : MonoBehaviour
{

    [SerializeField] 
    private float speed, xDir;

	// Use this for initialization
	void Start ()
    {
        xDir = transform.position.x;
        speed = Random.Range(0.5f, 1.5f);
	}

    // Update is called once per frame
    void Update()
    {
        xDir -= Time.deltaTime * speed;
        transform.position = new Vector3(xDir, transform.position.y, transform.position.z);

        if (transform.position.x < -10)
        {
            xDir = 10;
            transform.position = new Vector3(10, transform.position.y, transform.position.z);
            transform.position = new Vector3(xDir, transform.position.y, transform.position.z);
        }
	}
}
