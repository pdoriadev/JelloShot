using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/// <summary>
/// Called by BallManager. 
/// Takes set of int values and normalizes all of them by a taking each value and putting it over the sum of all the values. 
/// We pass this fraction as a percent. (i.e. value / sumOfValues)
/// +
/// Selects what will be spawned based on random selection and percentage selection. 
/// 1ST QUESTION --> WHAT KIND OF BALL?
/// 2ND: select ball type (Tank, Support, or Attack). THEN: select ball from collection of the selected ball type. 
/// 
/// Items are selected based on a random value between 0 and 100. Each potentially selected item is paired with a threshold
///     also between 0 and 100. The item who's threshold is least less than but not greater than the random value is the item
///     that's chosen. 
/// Didn't use tuples because adding values to each item of each tuple is tedious and ugly, hindering readability. 
/// 
public class ItemSelector : MonoBehaviour
{

    public static ItemSelector instance;

    #region PRIVATE VARIABLES 

    // Spawnable Objects
    [SerializeField]
    private GameObject _TankOne, _TankTwo, _SupportOne, _SupportTwo, _AttackOne, _AttackTwo;

    [Space(10, order = 0)]
    [Header("GameObject TYPE Thresholds")]
    // Tank Chance's threshold is 0 because it is the first threshold which always equals 0. 
    [SerializeField]
    private int _SupportChanceThreshold;
    [SerializeField]
    private int _AttackChanceThreshold;

    [Space(10, order = 0)]
    [Header("Each GAMEOBJECT's Thresholds")]
    // Spawnable Object's spawn chance thresholds. First threshold always = 0, so no point in making a variable for it. 
    [SerializeField]
    private int _TankTwoThreshold;
    [SerializeField]
    private int _SupportTwoThreshold;
    [SerializeField]
    private int _AttackTwoThreshold;

    [SerializeField]
    private Dictionary<int, string> _BallOrPowerUpDict = new Dictionary<int, string>();
    [SerializeField]
    private Dictionary<int, string> _BallTypeDict = new Dictionary<int, string>();
    [SerializeField]
    private Dictionary<int, GameObject> _TankBalls = new Dictionary<int, GameObject>();
    [SerializeField]
    private Dictionary<int, GameObject> _SupportBalls = new Dictionary<int, GameObject>();
    [SerializeField]
    private Dictionary<int, GameObject> _AttackBalls = new Dictionary<int, GameObject>();
    #endregion

    #region UNITY CALLBACKS
    private void OnEnable()
    {
        if (instance == null)
            instance = this;
    }

    private void OnDisable()
    {
        instance = null;
    }

    // Adding pairs OnStart because only static GameObjects can be added to dictionaries before run-time. 
    private void Start()
    {
        _BallOrPowerUpDict.Add(0, "Ball");
        _BallOrPowerUpDict.Add(101, "PowerUp");   // WON'T BE USED TIL POWERUPS ARE IMPLEMENTED

        _BallTypeDict.Add(0, "Tank");
        _BallTypeDict.Add(_SupportChanceThreshold, "Support");
        _BallTypeDict.Add(_AttackChanceThreshold, "Attack");

        _TankBalls.Add(0, _TankOne);
        _TankBalls.Add(_TankTwoThreshold, _TankTwo);
        _SupportBalls.Add(0, _SupportOne);
        _SupportBalls.Add(_SupportTwoThreshold, _SupportTwo);
        _AttackBalls.Add(0, _AttackOne);
        _AttackBalls.Add(_AttackTwoThreshold, _AttackTwo);
    }
    #endregion

    public GameObject SelectItem()
    {
        GameObject gameObjectToReturn = null;

        if (SelectItem(_BallOrPowerUpDict) == "Ball")
        {
            string selectedBallType = string.Empty;
            selectedBallType = (SelectItem(_BallTypeDict));

        //selectedBallType = "Tank";

            if (selectedBallType == "Tank")
            {
                gameObjectToReturn = (SelectItem(_TankBalls));
            }

            else if (selectedBallType == "Support")
            {
                gameObjectToReturn = (SelectItem(_SupportBalls));
            }

            else if (selectedBallType == "Attack")
            {
                gameObjectToReturn = (SelectItem(_AttackBalls));
            }
        }

        else if (gameObjectToReturn == null)
        {
            print("ERROR: gameObjectToReturn is null. ItemSelector.SelectItem();");
            Time.timeScale = 0.1f;
        }

        // else if (power-up)

        return gameObjectToReturn;
    }

    /// Finds the threshold/key that is less than the selected random value AND least different to the selected random value.
    /// For example, if random value = 60, threshold one/key one = 10, and threshold two/key two = 50, both thresholds (10 and 50) are 
    /// less than 60, but 50 is closer. The value paired with 50 will be chosen.
    #region SelectKeyBasedOnValuesMethods

    private static string SelectItem(Dictionary<int, string> _keyValuePairs)
    {
        int selectedValue = Random.Range(0, 100);
        int index = 0;
        int selectedKey = 0;
        int difference = 0;
        int previousDifference = 0;
        int intArrayIndexCount = _keyValuePairs.Count;
        string selectedString = string.Empty;

        while (_keyValuePairs.Count > index)
        {
            KeyValuePair<int, string> entry = _keyValuePairs.ElementAt(index);

            if (entry.Key < selectedValue)
            {
                difference = selectedValue - entry.Key;

                if (previousDifference == 0)
                {
                    previousDifference = difference;
                    selectedKey = entry.Key;
                }

                else if (previousDifference > difference)
                {
                    difference = previousDifference;
                    selectedKey = entry.Key;
                }
            }

            index++;
        }

        if (_keyValuePairs.TryGetValue(selectedKey, out string _string))
        {
            selectedString = _string;
        }

        return selectedString;
    }

    private static GameObject SelectItem(Dictionary<int, GameObject> _keyValuePairs)
    {
        int selectedValue = Random.Range(0, 100);
        int index = 0;
        int selectedKey = 0;
        int difference = 0;
        int previousDifference = 0;
        int intArrayIndexCount = _keyValuePairs.Count;
        GameObject selectedGameObject = null;

        // Finds the threshold/key that is less than the selected random value AND least different to the selected random value
        while (_keyValuePairs.Count > index)
        {
            KeyValuePair<int, GameObject> entry = _keyValuePairs.ElementAt(index);

            if (entry.Key < selectedValue)
            {
                difference = selectedValue - entry.Key;

                if (previousDifference == 0)
                {
                    previousDifference = difference;
                    selectedKey = entry.Key;
                }

                else if (previousDifference > difference)
                {
                    difference = previousDifference;
                    selectedKey = entry.Key;
                }
            }

            ++index;
        }

        if (_keyValuePairs.TryGetValue(selectedKey, out GameObject _gameObject))
        {
            selectedGameObject = _gameObject;
        }

        else print("ERROR: selectedGameObject does not equal _gameObject");

        return selectedGameObject;
    }

    #endregion
}
