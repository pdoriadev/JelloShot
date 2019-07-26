using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public Camera mainCamera;

    //public Transform playerTransform;
    //public Vector2 mainCamPosition;
    //public Vector2 playerPosition;
    //public float cameraDistanceFromPlayer;

    //public float screenWidth;

    //// DEFINE SCREEN.WIDTH AND EVERYTHING CAMERA RELATED HERE!!!

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    private void Start()
    {
        if (instance == null)
            instance = this;
    }

    private void OnDisable()
    {
        instance = null;
        mainCamera = null;
    }

    //private void LateUpdate()
    //{
    //    playerPosition = playerTransform.position;
    //    Camera.main.transform.position = Vector3.ClampMagnitude(playerPosition, cameraDistanceFromPlayer);
    //    Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, -10);

    //    //Camera.main.transform.position = new Vector3 ( mainCamPosition.x, mainCamPosition.y, -10);
    //}

}
   