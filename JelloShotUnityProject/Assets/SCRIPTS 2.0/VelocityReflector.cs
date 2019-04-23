using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Lerper))]
public class VelocityReflector : MonoBehaviour
{
    private Lerper _LerperInstance;
    
    #region UNITY CALLBACKS
    void OnEnable()
    {
        if (GetComponent<Lerper>() == null)
        {
            _LerperInstance = gameObject.AddComponent<Lerper>() as Lerper;
        }
        else
            _LerperInstance = GetComponent<Lerper>();
    }

    void OnDisable()
    {
        _LerperInstance = null;
    }
    #endregion

    [SerializeField]
    private float _ReflectForce;
    [SerializeField]
    private int _StartingHealth = 3;
    [SerializeField]
    private int _CurrentHealth;
    // Reflects player off itself. Calls StartLerp().
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == (int)GameLayers.Player)
        {       


            --_CurrentHealth;
            if (_CurrentHealth < 1)
            {
                StartCoroutine(CheckForDeathCo());
                print("Health is 0");
            }

            _LerperInstance.StartLerp(_CurrentHealth, _StartingHealth);     
        }
    }

    IEnumerator CheckForDeathCo()
    {
        bool _isChecking = true;
        while(_isChecking == true)
        {
            yield return new WaitForSeconds(0.01f);
            if (_CurrentHealth < 1 && _LerperInstance._IsLerping == false)
                OnDeath();
        }

        yield return null;
    }

    public void OnDeath()
    {
        print("Died at " + _CurrentHealth + " Health");
        StopCoroutine(CheckForDeathCo());
        transform.localScale = _LerperInstance._ObjScaleAtSpawn;
        _CurrentHealth = _StartingHealth;
        SpawnManager.instance.PoolObject(gameObject);
        ScoreManager.instance.IterateBallsKoScore();
    }
}
/// <summary>
/// The time taken to move from the start to finish positions
/// </summary>
//public float timeTakenDuringLerp = 1f;

///// <summary>
///// How far the object should move when 'space' is pressed
///// </summary>
//public float distanceToMove = 10;

////Whether we are currently interpolating or not
//private bool _isLerping;

////The start and finish positions for the interpolation
//private Vector3 _startPosition;
//private Vector3 _endPosition;

////The Time.time value when we started the interpolation
//private float _timeStartedLerping;

///// <summary>
///// Called to begin the linear interpolation
///// </summary>
//void StartLerping()
//{
//    _isLerping = true;
//    _timeStartedLerping = Time.time;

//    //We set the start position to the current position, and the finish to 10 spaces in the 'forward' direction
//    _startPosition = transform.position;
//    _endPosition = transform.position + Vector3.forward * distanceToMove;
//}

//void Update()
//{
//    //When the user hits the spacebar, we start lerping
//    if (Input.GetKey(KeyCode.Space))
//    {
//        StartLerping();
//    }
//}

////We do the actual interpolation in FixedUpdate(), since we're dealing with a rigidbody
//void FixedUpdate()
//{
//    if (_isLerping)
//    {
//        //We want percentage = 0.0 when Time.time = _timeStartedLerping
//        //and percentage = 1.0 when Time.time = _timeStartedLerping + timeTakenDuringLerp
//        //In other words, we want to know what percentage of "timeTakenDuringLerp" the value
//        //"Time.time - _timeStartedLerping" is.
//        float timeSinceStarted = Time.time - _timeStartedLerping;
//        float percentageComplete = timeSinceStarted / timeTakenDuringLerp;

//        //Perform the actual lerping.  Notice that the first two parameters will always be the same
//        //throughout a single lerp-processs (ie. they won't change until we hit the space-bar again
//        //to start another lerp)
//        transform.position = Vector3.Lerp(_startPosition, _endPosition, percentageComplete);

//        //When we've completed the lerp, we set _isLerping to false
//        if (percentageComplete >= 1.0f)
//        {
//            _isLerping = false;
//        }
//    }
//}