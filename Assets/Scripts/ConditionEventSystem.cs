using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Condition{
    public enum VariableType
    {
        Float,
        Bool,
        Int,
        String
    }
    public enum ConditionType
    {
        largerThan = 0,
        smallerThan = 1,
        valueEquals = 2,
        valueTrue,
        valueFalse
    }
    public VariableType variableType;
    public string variableName;
    public ConditionType conditionType;
    public float floatValue;
    public int intValue;
    public string stringValue;

    public bool Evaluate()
    {

        return false;
    }
}
