using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayMove()
    {
        animator.SetBool("ShouldMove", true);
    }

    private Vector3 _Pos;
    public void EndAnimListener()
    {
        if (animator.GetBool("ShouldSpawn") == true
            && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.97f)
        {
            _Pos = transform.position;
            animator.StopPlayback();
            animator.SetBool("ShouldSpawn", false);
            animator.StartPlayback();
            animator.SetBool("ShouldRotate", true);
        }
    }
    public void StartAnimListener()
    {
        if (animator.GetBool("ShouldRotate") == true)
        {
            transform.position = _Pos;
        }
    }
}
