using UnityEngine;
using System.Collections;
using System.Collections.Generic; // For List
using System;
// By Michael Wolf
public class TweenManager : LazySingleton<TweenManager>
{
    private List<Coroutine> currentTweens = new List<Coroutine>();

    public static void StartTween(Action<float> tweenFunc, float duration, Action onComplete)
    {
        TweenManager.Instance.BeginTween(tweenFunc, duration, onComplete);
    }

    public void BeginTween(Action<float> tweenFunc, float duration, Action onComplete)
    {
        Coroutine tween = StartCoroutine(CoTween(tweenFunc, duration, onComplete));

        //currentTweens.Add(tween); // Track?
    }

    private IEnumerator CoTween(Action<float> tweenFunc, float duration, Action onComplete)
    {
        float timer = 0f;
        while(timer < duration)
        {
            timer += Time.deltaTime;

            if (tweenFunc != null)
                tweenFunc(timer / duration);

            //yield return new WaitForFixedUpdate();
            yield return null;
        }
        if(onComplete != null)
        {
            onComplete();
        }
    }

    private void Update()
    {
        //foreach (var tween in currentTweens)
        //{
        //    // Clear Finished
        //    //if(tween.)
        //}
    }

}
