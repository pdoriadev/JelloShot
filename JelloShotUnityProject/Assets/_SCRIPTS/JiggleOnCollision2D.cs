using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JiggleOnCollision2D : MonoBehaviour
{
    SpriteRenderer spriteRender;
    float controlTime;

    // Use this for initialization
    void OnEnable()
    {
        spriteRender = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        controlTime = 0;

        spriteRender.material.SetVector("_ModelOrigin", transform.position);
        spriteRender.material.SetVector("_ImpactOrigin", collision.GetContact(0).point);
    }

    // Update is called once per frame
    void Update()
    {
        controlTime += Time.deltaTime;      

        spriteRender.material.SetFloat("_ControlTime", controlTime);
    }
}
