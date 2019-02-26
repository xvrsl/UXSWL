using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum NumOperation
{
    set = 0,
    plus = 1,
    minus = 2,
    multiply = 3,
    divideWith = 4,
    power = 5
}
public enum BoolOperation
{
    set = 0,
    inverse = 1,
    and = 2,
    or = 3,
    xor = 4
}
public enum StringOperation
{
    set = 0,
    cat = 1
}
public class GameActionExecuter : MonoBehaviour {
    public static GameActionExecuter instance;
    public void ShowDialogue(string text)
    {
        Debug.Log(text);
    }

    public void SetFloat(string name, float value)
    {
        GameManager.instance.gameStatus.SetFloat(name, value);
    }
    public float AddToFloat(string name, float value)
    {
        var variable = GameManager.instance.gameStatus.GetFloatVariable(name);
        variable.value += value;
        return variable.value;
    }
    public float OperateFloat(string name, NumOperation operation, float attribute)
    {
        var variable = GameManager.instance.gameStatus.GetFloatVariable(name);
        if(variable == null)
        {
            return -1;
        }
        switch (operation)
        {
            case NumOperation.set:
                variable.value = attribute;
                break;
            case NumOperation.plus:
                variable.value += attribute;
                break;
            case NumOperation.minus:
                variable.value -= attribute;
                break;
            case NumOperation.multiply:
                variable.value *= attribute;
                break;
            case NumOperation.divideWith:
                variable.value /= attribute;
                break;
            case NumOperation.power:
                variable.value = Mathf.Pow(variable.value, attribute);
                break;
            default:
                Debug.Log("Invalid Operation");
                break;
        }
        return variable.value;
    }
    
    public void SetBool(string name, bool value)
    {
        GameManager.instance.gameStatus.SetBool(name, value);
    }
    public bool ToggleBool(string name)
    {
        var variable = GameManager.instance.gameStatus.GetBoolVariable(name);
        variable.value = !variable.value;
        return variable.value;
    }
    public bool OperateBool(string name, BoolOperation operation, bool attribute)
    {
        var variable = GameManager.instance.gameStatus.GetBoolVariable(name);
        switch (operation)
        {
            case BoolOperation.set:
                variable.value = attribute;
                break;
            case BoolOperation.inverse:
                variable.value = !variable.value;
                break;
            case BoolOperation.and:
                variable.value = variable.value && attribute;
                break;
            case BoolOperation.or:
                variable.value = variable.value || attribute;
                break;
            case BoolOperation.xor:
                variable.value = variable.value != attribute;
                break;
            default:
                Debug.LogWarning("Operation " + operation.ToString() + " Not Implemented yet.");
                break;
        }
        return variable.value;
    }
    public void SetInt(string name, int value)
    {
        GameManager.instance.gameStatus.SetInt(name, value);
    }
    public int OperateInt(string name, NumOperation operation, int attribute)
    {
        var variable = GameManager.instance.gameStatus.GetIntVariable(name);

        switch (operation)
        {
            case NumOperation.plus:
                variable.value += attribute;
                break;
            case NumOperation.minus:
                variable.value -= attribute;
                break;
            case NumOperation.multiply:
                variable.value *= attribute;
                break;
            case NumOperation.divideWith:
                variable.value /= attribute;
                break;
            case NumOperation.power:
                variable.value =(int)Mathf.Pow(variable.value,attribute);
                break;
            default:
                Debug.Log("Invalid Operation");
                break;
        }
        return variable.value;
    }

    public void SetString(string name, string value)
    {
        GameManager.instance.gameStatus.SetString(name, value);
    }
    public string OperateString(string name, StringOperation operation, string attribute)
    {
        var variable = GameManager.instance.gameStatus.GetStringVariable(name);
        switch (operation)
        {
            case StringOperation.set:
                variable.value = attribute;
                break;
            case StringOperation.cat:
                variable.value = variable.value + attribute;
                break;
            default:
                Debug.LogWarning("Operation " + operation.ToString()+"Not implemented yet");
                break;
        }
        return variable.value;
    }
    
    // Use this for initialization
    void Start () {
	    if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
