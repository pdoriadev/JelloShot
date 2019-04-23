using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///    DID NOT COMPLETE BECAUSE DO NOT NEED THIS TO TEST SHADER ON 2D OBJ
/// </summary>
public class JellyClickReceiver2D : MonoBehaviour
{
    RaycastHit hit;
    Ray clickRay;

    SpriteRenderer spriteRender;
    float controlTime;

    // Use this for initialization
    void Start()
    {
        spriteRender = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        controlTime += Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            clickRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(clickRay, out hit))
            {
                controlTime = 0;

                spriteRender.material.SetVector("_ModelOrigin", transform.position);
                spriteRender.material.SetVector("_ImpactOrigin", hit.point);
            }
        }

        spriteRender.material.SetFloat("_ControlTime", controlTime);
    }
}
