using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Condition {
    public enum ConditionType
    {
        valueLargerThan = 0,
        valueSmallerThan = 1,
        valueEquals = 3,
        item = 4,
        noItem = 5
    }

    public ConditionType conditionType;
    public string valueName;
    public float comparingValue;
    public bool Evaluate()
    {
        switch (conditionType)
        {
            case ConditionType.valueLargerThan:
                return GameManager.instance.gameStatus.IsValueLargerThan(valueName, comparingValue);
            case ConditionType.valueSmallerThan:
                return GameManager.instance.gameStatus.IsValueSmallerThan(valueName, comparingValue);
            case ConditionType.valueEquals:
                return GameManager.instance.gameStatus.IsValueEquals(valueName, comparingValue);
            default:
                Debug.LogWarning("This condition:" + conditionType.ToString() + " is under construction. It's not supported for now.");
                return false;
        }
    }
}
