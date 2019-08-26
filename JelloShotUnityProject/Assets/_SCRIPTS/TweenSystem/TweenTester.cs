using UnityEngine;
using System.Collections;
// By Michael Wolf
public class TweenTester : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        SpriteRenderer sRend = GetComponent<SpriteRenderer>();
        TweenManager.StartTween(
            // param 1
            (float lerp) => {
                transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 5f, lerp);
                sRend.color = Color.Lerp(Color.white, Color.red, lerp);
            },
            // param 2
            1f,
            // param 3
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
