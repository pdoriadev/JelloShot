using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScreenWrap : MonoBehaviour {

    // public static instance of this script
    public static PlayerScreenWrap playerScreenWrapInstance;
    [SerializeField]
    private float screenLeft, screenRight; 

    private void OnTriggerExit2D(Collider2D collision)
    {
        // if collided with object is player
        if (collision.gameObject.tag == "Player")
        {
            //Transform of player
            Transform playerTransform = collision.gameObject.transform;

            // if player position passes left side of screen, then the x position will change to right side of screen
            if (playerTransform.position.x < screenLeft)
                playerTransform.position = new Vector2( screenRight, playerTransform.position.y);

            // if player position passes right side of screen, then the x position will change to the left side of screen
            if (playerTransform.position.x > screenRight)
                playerTransform.position = new Vector2(screenLeft, playerTransform.position.y);

            //DON'T NEED THIS BECAUSE WE HAVE GROUND
            //if (playerTransform.position.y < screenBottom)
            //    playerTransform.position = new Vector2(screenRight, playerTransform.position.y);

            //if (playerTransform.position.x < screenLeft)
            //    playerTransform.position = new Vector2(screenRight, playerTransform.position.y);
        }
    }
    
}
