using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Created 9/11/18
/// Need to get spawned objects.put them in data structure. Could be array or Queue. Go through
/// them to see if they are colliding every frame. 
///  
/// Why am I doing this optimization? So I don't end up with spaghetti code. All the ball collision
/// logic is one script. This includes sticking and ground collision logic. Also so every spawned object
/// doesn't load another whole script. Just have one script that manages all the objects. 
/// 
/// 1. Create data structure
/// 2. Find items to put in data structure
/// 3. Put items into data strucuture
/// 4. Run collision logic on individual item. 
/// 
/// Which data structure? --> Options: Queue, Array, List 
///     - First try: Array. New one created with each new spawn. Run foreach loop through 
///         every item in the array. 
/// Finding objects --> gameObject.findgameobjectwithtag
/// </summary>
public class BallCollisionManager : MonoBehaviour
{
    public static BallCollisionManager ballCollisionManagerInstance;
    public GameObject[] ballsArray;

    private void OnEnable()
    {
        if (ballCollisionManagerInstance == null)
            ballCollisionManagerInstance = this;

        //GameManager.OnFixedUpdateEvent += OnFixedUpdateHandler;
    }

    private void OnDisable()
    {
        //GameManager.OnFixedUpdateEvent -= OnFixedUpdateHandler;
    }

    private void FixedUpdate()
    {

    }
}
