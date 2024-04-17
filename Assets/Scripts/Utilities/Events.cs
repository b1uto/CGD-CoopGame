using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace CGD.Events
{
    [System.Serializable]
    public class IntEvent : UnityEvent<int> { }
    [System.Serializable]
    public class StringEvent : UnityEvent<string> { }
    [System.Serializable]
    public class FloatEvent : UnityEvent<float> { }
    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public delegate void IntDelegate(int value);
    public delegate void DoubleDelegate(double value);
    public delegate void StringDelegate(string value);
    public delegate void FloatDelegate(float value);
    public delegate void BoolDelegate(bool value);
}
