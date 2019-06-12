using UnityEngine;
using System.Collections;

public class TweenTester : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        SpriteRenderer sRend = GetComponent<SpriteRenderer>();
        TweenManager.StartTween(
            (float lerp) => {
                transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 5f, lerp);
                sRend.color = Color.Lerp(Color.white, Color.red, lerp);
            },
            1f,
            () => {
                Debug.Log("Completed Size Lerp");
                TweenManager.StartTween(
                    (float lerp) => {
                    transform.localScale = Vector3.Lerp(Vector3.one*5f, Vector3.one*0.5f, lerp);
                    sRend.color = Color.Lerp(Color.red, Color.blue, lerp);
                    }, 
                    0.5f,
                    null
                ); 
            }
        );
    }

    // Update is called once per frame
    void Update()
    {

    }
}
