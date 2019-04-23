using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Range
{

}

public struct IntRange
{
    public int min;
    public int max;

    public IntRange (int _startRange, int _endRange)
    {
        min = _startRange;
        max = _endRange;
    }
}

public struct FloatRange
{
    public float min;
    public float max;

    public FloatRange(float _startRange, float _endRange)
    {
        min = _startRange;
        max = _endRange;
    }
}


