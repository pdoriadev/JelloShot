using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCollisionTrigger : MonoBehaviour
{
    internal List<GameObject> _ObjectsInBlastRadius = new List<GameObject>();

    #region AddRemoveFromList
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)GameLayers.Player || collision.gameObject.layer == (int)GameLayers.BallsLayer)
        {
            _ObjectsInBlastRadius.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _ObjectsInBlastRadius.Remove(collision.gameObject);
    }
    #endregion
}
