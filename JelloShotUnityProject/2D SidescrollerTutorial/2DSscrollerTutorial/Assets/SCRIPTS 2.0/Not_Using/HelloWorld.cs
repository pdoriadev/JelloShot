using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct Player 
{
    public GameObject playerObj;

    public Vector2 shotVelocity;
    public Vector2 playerCurrentVelocity;
    public Vector2 newBallVelocity;

    public float dragDistance;
    public float dragShotForce;
    public float shotVelocityMaxMagnitude;

    
}

public struct Alien
{
    public sbyte scorePointValue;
    public sbyte hitPoints;
    public bool alive;
}

//public class HelloWorld : MonoBehaviour
//{
//    sbyte oneBytenNegativeOneTwentyEightToPositiveOneTwentySeven;
//    byte oneByteOnlyPositiveIntsFromZeroToTwoFiftyFive;
//    short twoBytePositiveAndNegative;
//    ushort twoBytePostive;
//    int fourBytesPositiveAndNeg;
//    uint fourBytesPositive;
//    long eightBytesPositiveAndNeg;
//    ulong eightBytesPositive;

//    float fourBytesPosAndNeg;
//    double eightBytesPosAndNeg;
//    decimal sixteenBytesPosAndNeg;

//    string varies;

//    char twoBytesUnicodeVariables;

    

//    Alien alienOne;
//    private void Start()
//    {
//        alienOne.scorePointValue = 10;
//        alienOne.hitPoints = 3;
//        alienOne.alive = true;
//    }

//    void OnDisable()
//    {
//        Debug.Log("Score: " + alienOne.scorePointValue + ", HP: " + alienOne.hitPoints + ", ALIVE: " + alienOne.alive);
//    }
//}


public struct Person
{
    bool isAlive;
    string names;
    int scores;
    //Person[] personStructArray;

    public Person (bool _isAlive, string _names, int _scores)
    {       
        isAlive = _isAlive;
        names = _names;
        scores = _scores;
    }
}



public class HelloWorld : MonoBehaviour
{
    public List<Person> personData = new List<Person>();

    private void OnDisable()
    {
        Person Ted = new Person(true, "Ted", 10);
        Person Frank = new Person(false, "Frank", 1);
        Person Tim = new Person(true, "Tim", 5);

        personData.Add(Ted);
        personData.Add(Frank);
        personData.Add(Tim);

        foreach (Person _person in personData)
        {
            print(_person);
        }


        bool[] isAlive = new bool[] { true, false, true };
        string[] names = new string[] { "Ted", "Frank", "Tim" };
        int[] scores = new int[] { Random.Range(0,10), Random.Range(0,10), Random.Range(0,10) };

        foreach (bool isAliveBool in isAlive)
        {
            if (isAliveBool)
            {
               // if(scores)
            }
        }
    }

    void PrintPersonValues(Person _person1)
    {

    }
}

