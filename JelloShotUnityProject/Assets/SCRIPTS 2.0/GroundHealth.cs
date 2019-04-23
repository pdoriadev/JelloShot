using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// Ball hits, pass correct negative value to "addToScore()" method in Score script. 
/// </summary>
public class GroundHealth : PoolingTrigger
{
    public static GroundHealth instance;

    public Color[] groundColors = new[] { Color.red, Color.yellow, Color.green };
    public SpriteRenderer groundSpriteRenderer;
    public int groundHealth = 3; // 0 is max health
    public int startingGroundHealth;
    public UnityEvent groundExitCollision;

    private void OnEnable()
    {
        if (instance == null)
            instance = this;

        startingGroundHealth = groundHealth;
        groundSpriteRenderer = GetComponent<SpriteRenderer>();

        GroundSetup();
    }

    public void GroundSetup()
    {
        groundHealth = startingGroundHealth;
        groundSpriteRenderer.color = groundColors[groundHealth - 1];
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == (int)GameLayers.BallsLayer && collision.gameObject.tag != "reflector")
        {
            
            int previousGroundHealth = groundHealth;
            groundHealth -= 1;

            if (groundHealth < previousGroundHealth && groundHealth > 0)
            {
                groundSpriteRenderer.color = groundColors[groundHealth - 1];
                PoolPassedObj(collision.gameObject);
                return;
            }
               
            else 
                GameManager.instance.LevelEnd();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        groundExitCollision.Invoke();
        // Insert FX here. Pass collision position and velocity info where desired to Death FX script. 
    }
}





