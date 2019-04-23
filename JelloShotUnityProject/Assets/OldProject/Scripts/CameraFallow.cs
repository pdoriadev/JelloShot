using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFallow : MonoBehaviour
{
    private float zDepth = -10;
    [SerializeField] private float xMin;
    [SerializeField] private float xMax;
    [SerializeField] private float yMin;
    [SerializeField] private float yMax;
    Shared_Vars shared_VarsScript;

    // Use this for initialization
    void Start ()
    {
        shared_VarsScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Shared_Vars>();
    }

    internal void UpdateSharedVars()
    {

    }

    void LateUpdate ()
    {
        shared_VarsScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Shared_Vars>();
        float x = (Mathf.Clamp(shared_VarsScript.plyrTrnsfm.position.x, xMin, xMax));
        float y = (Mathf.Clamp(shared_VarsScript.plyrTrnsfm.position.y, yMin, yMax));
        gameObject.transform.position = new Vector3(x, y, zDepth);
    }
}
