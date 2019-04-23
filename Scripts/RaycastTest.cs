using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastTest : MonoBehaviour {

    internal float TargetDistance;

    [SerializeField]
    private float RayLength = 4f;

    // Update is called once per frame
    void Update()
    {
        RaycastHit TheHit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out TheHit, RayLength))
        {
            TargetDistance = TheHit.distance;
            Debug.DrawRay(transform.position, (Vector3.down), Color.blue, RayLength);
        }
       
    }
}
