using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class JointConnect : MonoBehaviour
{
    private DistanceJoint2D ballDistanceJoint;

    private void OnEnable()
    {
        ballDistanceJoint = GetComponent<DistanceJoint2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == (int)GameLayers.BallsLayer)
        {
            
        }
    }
}
