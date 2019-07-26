using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualsController : MonoBehaviour {

    [Space(10)]
    public SpriteRenderer playerSpriteRenderer;
    // Player sprite colors depending on gamestate.
    [SerializeField]
    private Color _BeforeHitColor, _AfterHitColor;
    private Color _PlayerSpriteColor;

    private void BeforeHit()
    {
        _PlayerSpriteColor = playerSpriteRenderer.color;
        _PlayerSpriteColor = _BeforeHitColor;
        playerSpriteRenderer.color = _PlayerSpriteColor;
    }

    private void AfterHit()
    {
        _PlayerSpriteColor = _AfterHitColor;
        playerSpriteRenderer.color = _PlayerSpriteColor;
    }
}
