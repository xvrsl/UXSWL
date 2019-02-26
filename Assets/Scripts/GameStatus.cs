using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[System.Serializable]
public class GameVariableFloat
{
    [XmlAttribute("name")]
    public string name;
    public float value;
}
[System.Serializable]
public class GameVariableBool
{
    [XmlAttribute("name")]
    public string name;
    public bool value;
}
[System.Serializable]
public class GameVariableInt
{
    [XmlAttribute("name")]
    public string name;
    public int value;
}
[System.Serializable]
public class GameVariableString
{
    [XmlAttribute("name")]
    public string name;
    public string value;
}

[System.Serializable]
public class GameStatus {
    [XmlArray("floatVariables")]
    [XmlArrayItem("GameVariableFloat")]
    public List<GameVariableFloat> floatVariables;
    [XmlArray("boolVariables")]
    [XmlArrayItem("GameVariableBool")]
    public List<GameVariableBool> boolVariables;
    [XmlArray("intVariables")]
    [XmlArrayItem("GameVariableInt")]
    public List<GameVariableInt> intVariables;
    [XmlArray("stringVariables")]
    [XmlArrayItem("GameVariableString")]
    public List<GameVariableString> stringVariables;

    [XmlIgnore]
    Dictionary<string, GameVariableFloat> floatDictionary = new Dictionary<string, GameVariableFloat>();
    [XmlIgnore]
    Dictionary<string, GameVariableBool> boolDictionary = new Dictionary<string, GameVariableBool>();
    [XmlIgnore]
    Dictionary<string, GameVariableInt> intDictionary = new Dictionary<string, GameVariableInt>();
    [XmlIgnore]
    Dictionary<string, GameVariableString> stringDictionary = new Dictionary<string, GameVariableString>();

    public GameVariableFloat GetFloatVariable(string name)
    {
        if (floatDictionary.ContainsKey(name))
        {
            return floatDictionary[name];
        }

        foreach (var cur in floatVariables)
        {
            if (cur.name == name)
            {
                floatDictionary.Add(name, cur);
                return cur;
            }
        }
        return null;
    }
    public GameVariableBool GetBoolVariable(string name)
    {
        if (boolDictionary.ContainsKey(name))
        {
            return boolDictionary[name];
        }
        foreach (var cur in boolVariables)
        {
            if (cur.name == name)
            {
                boolDictionary.Add(name, cur);
                return cur;
            }
        }
        return null;
    }
    public GameVariableInt GetIntVariable(string name)
    {
        if (intDictionary.ContainsKey(name))
        {
            return intDictionary[name];
        }
        foreach (var cur in intVariables)
        {
            if (cur.name == name)
            {
                intDictionary.Add(name, cur);
                return cur;
            }
        }
        return null;
    }
    public GameVariableString GetStringVariable(string name)
    {
        if (stringDictionary.ContainsKey(name))
        {
            return stringDictionary[name];
        }
        foreach (var cur in stringVariables)
        {
            if (cur.name == name)
            {
                stringDictionary.Add(name, cur);
                return cur;
            }
        }
        return null;
    }

    public float GetFloat(string name)
    {
        return GetFloatVariable(name).value;
    }
    public bool GetBool(string name)
    {
        return GetBoolVariable(name).value;
    }
    public int GetInt(string name)
    {
        return GetIntVariable(name).value;
    }
    public string GetString(string name)
    {
        return GetStringVariable(name).value;
    }

    public void SetFloat(string name, float value)
    {
        var variable = GetFloatVariable(name);
        variable.value = value;
    }
    public void SetBool(string name, bool value)
    {
        var variable = GetBoolVariable(name);
        variable.value = value;
    }
    public void SetInt(string name, int value)
    {
        var variable = GetIntVariable(name);
        variable.value = value;
    }
    public void SetString(string name,string value)
    {
        var variable = GetStringVariable(name);
        variable.value = value;
    }

    public void CreateFloat(string name, float value)
    {
        if(GetFloatVariable(name) == null)
        {
            GameVariableFloat newVariable = new GameVariableFloat();
            newVariable.name = name;
            newVariable.value = value;
            floatDictionary.Add(name, newVariable);
        }
        else
        {
            Debug.LogWarning("Float variable " + name + " already exists.");
            return;
        }
    }
    public void CreateBool(string name, bool value)
    {
        if (GetBoolVariable(name) == null)
        {
            GameVariableBool newVariable = new GameVariableBool();
            newVariable.name = name;
            newVariable.value = value;
            boolDictionary.Add(name, newVariable);
        }
        else
        {
            Debug.LogWarning("Bool variable " + name + " already exists.");
            return;
        }
    }
    public void CreateInt(string name, int value)
    {
        if (GetIntVariable(name) == null)
        {
            GameVariableInt newVariable = new GameVariableInt();
            newVariable.name = name;
            newVariable.value = value;
            intDictionary.Add(name, newVariable);
        }
        else
        {
            Debug.LogWarning("Int variable " + name + " already exists.");
            return;
        }
    }
    public void CreateString(string name, string value)
    {
        if (GetStringVariable(name) == null)
        {
            GameVariableString newVariable = new GameVariableString();
            newVariable.name = name;
            newVariable.value = value;
            stringDictionary.Add(name, newVariable);
        }
        else
        {
            Debug.LogWarning("String variable " + name + " already exists.");
            return;
        }
    }

    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(GameStatus));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }
    public static GameStatus Load(string path)
    {
        var serializer = new XmlSerializer(typeof(GameStatus));
        using(var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as GameStatus;
        }
    }


    public string GetVariableValue(string name)
    {
        var floatVariable = GetFloatVariable(name);
        if(floatVariable != null)
        {
            return floatVariable.value.ToString();
        }
        var boolVariable = GetBoolVariable(name);
        if(boolVariable != null)
        {
            return boolVariable.value.ToString();
        }
        var intVariable = GetIntVariable(name);
        if(intVariable != null)
        {
            return intVariable.value.ToString();
        }
        var stringVariable = GetStringVariable(name);
        if(stringVariable != null)
        {
            return stringVariable.value.ToString();
        }

        Debug.LogError("Cannot find variable " + name);
        return "";
    }

    public void SetVariableValue(string name,string value)
    {
        var floatVariable = GetFloatVariable(name);
        if (floatVariable != null)
        {
            float newValue;
            if(float.TryParse(value,out newValue))
            {
                floatVariable.value = newValue;
            }
            else
            {
                Debug.LogError("Cannot parse the value \""+value+"\" to fit the float variable " + name);
            }
            return;
        }
        var boolVariable = GetBoolVariable(name);
        if (boolVariable != null)
        {
            boolVariable.value = value == "true" ? true : false;

            return;
        }
        var intVariable = GetIntVariable(name);
        if (intVariable != null)
        {
            int newValue;
            if (int.TryParse(value, out newValue))
            {
                intVariable.value = newValue;
            }
            else
            {
                Debug.LogError("Cannot parse the value \"" + value + "\" to fit the int variable " + name);
            }
            return;
        }
        var stringVariable = GetStringVariable(name);
        if (stringVariable != null)
        {
            stringVariable.value = value;
        }

        return;
    }
}
