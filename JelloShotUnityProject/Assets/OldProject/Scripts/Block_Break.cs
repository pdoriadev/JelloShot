using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Block_Break : MonoBehaviour {
    Rigidbody2D plyrRB;
    private RaycastHit2D blockAboveRay;
    [SerializeField] private float rayLength;
    [SerializeField] Vector2 rayDir;
    LayerMask groundLayer;
    Shared_Vars shared_VarsScript;
    [SerializeField] Tilemap tilemap;
    TileBase destroyedTile;
    [SerializeField] Vector3Int abovePlayer, abvPlyrOffest = new Vector3Int (0,1,0), tileCrrntPos;
    [SerializeField] ITilemap iTileMap;
    [SerializeField] TileData tileData;

    // Make list that stores Vectors of destroyed tiles. On reset, instantiate tiles at those vectors. 


    // Use this for initialization
    void Start ()
    {
        groundLayer = LayerMask.GetMask("Ground");
        plyrRB = GetComponent<Rigidbody2D>();
    }                                                                                                               
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        abovePlayer.x = (Mathf.RoundToInt(shared_VarsScript.prb.position.x));
        abovePlayer.y = (Mathf.RoundToInt(shared_VarsScript.prb.position.y));
        abovePlayer += abvPlyrOffest;
        destroyedTile = tilemap.GetTile(abovePlayer + abvPlyrOffest);
        RaycastUpCheck();
        CheckForTileAbove();
	}

    void CheckForTileAbove()
    {
        // prb's position. 
        // Get position right above player. 
        destroyedTile.GetTileData(tileCrrntPos, iTileMap, ref tileData);

        if (tileCrrntPos == abovePlayer)
        {              //tilemap.GetTile(abovePlayer);
            Destroy(destroyedTile);
        }
            // Check if tilemap has tile above prb's position. 
        // If tile position is within 0.5 of prb position, destroy tile.
       // tilemap.HasTile
    }

  
    void RaycastUpCheck()
    {
        blockAboveRay = Physics2D.Raycast(plyrRB.transform.position, rayDir, rayLength, groundLayer);
        Debug.DrawRay(plyrRB.transform.position, rayDir, Color.red);

        if (blockAboveRay.collider.IsTouchingLayers(groundLayer))
        {
            
            // tilemap.GetTile<TileBase>();
        }
    }
}
