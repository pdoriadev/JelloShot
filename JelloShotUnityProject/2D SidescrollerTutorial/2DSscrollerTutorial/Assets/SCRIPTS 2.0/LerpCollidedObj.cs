using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpCollidedObj : MonoBehaviour
{



}

public struct BallHealth
{
    public float currentHealth;
    public float startingHealth;

    public  BallHealth(float _currentHealth, float _startingHealth)
    {
        currentHealth = _currentHealth;
        startingHealth = _startingHealth;
    }
}