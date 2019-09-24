using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    #region VARIABLES
    [SerializeField]
    private AnimationClip[] clipArray;

    private Animator animator;
    #endregion

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayMove()
    {
        animator.SetBool("ShouldMove", true);
    }
}
