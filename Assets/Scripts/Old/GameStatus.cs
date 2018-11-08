using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GameStatus {
    public string gameName;
    public List<GameValue> values = new List<GameValue>();
    public Dictionary<string, GameValue> valuesDictionary = new Dictionary<string, GameValue>();
    public float GetValue(string valueName)
    {
        if (valuesDictionary.ContainsKey(valueName))
        {
            return valuesDictionary[valueName].value;
        }
        int valueCount = values.Count;
        for(int i = 0; i < valueCount; i++)
        {
            if(values[i].name == valueName)
            {
                valuesDictionary.Add(valueName, values[i]);
                return values[i].value;
            }
        }
        Debug.LogWarning("Value not found");
        return -1;
    }
    public float CompareValue(string valueName, float others)
    {
        return GetValue(valueName) - others;
    }
    public bool IsValueLargerThan(string valueName, float others)
    {
        return CompareValue(valueName, others) > 0;
    }
    public bool IsValueSmallerThan(string valueName, float others)
    {
        return CompareValue(valueName, others) < 0;
    }
    public bool IsValueEquals(string valueName, float others)
    {
        return Mathf.Approximately(CompareValue(valueName, others), 0);
    }

}
