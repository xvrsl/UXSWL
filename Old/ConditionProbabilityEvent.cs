using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[System.Serializable]
public class ConditionProbabilityEvent{
    public string discription;
    public Condition condition;
    public float probability;
    public UnityEvent unityEvent;
}
