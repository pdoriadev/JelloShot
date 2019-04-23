using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceAnimation : MonoBehaviour
{
    #region VARIABLES

    public static BounceAnimation instance;

    [SerializeField]
    private GameObject playerGameObject;
    private AnimationClip playerBounceEnter;
    private AnimationClip playerBounceExit;
    private Transform playerTransform;
  
    public Transform collisionTransform;

    public Vector2 contactNormal;
    public Vector2 reflectedVector;
    public Vector2 scaledY;
    public Vector2 scaledX;
    public Vector2 currentPlayerScale;
    public Vector2 normalPlayerScale;

    public Animator playerAnimator;

    public float scalerValue;
    public float scaleTime;

    #endregion

    private void OnEnable()
    {
        if (instance == null)
            instance = this;
    }

    private void OnDisable()
    {
        instance = null;
    }
    private void Start()
    {
        currentPlayerScale = playerGameObject.transform.localScale;
    }

    public void PlayBounce()
    {
        playerAnimator.SetBool("EnterBounce", true);
        playerAnimator.Play("EnterBounce");
    }

    public void PlayerBounceExitClip(Vector2 playerExitVelocity)
    {

    }
}
