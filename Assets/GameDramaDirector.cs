using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameDramaDirector : MonoBehaviour {
    public static GameDramaDirector instance;
    public List<GameEvent> publicGameEvents = new List<GameEvent>();

    public List<DramaUnit> dramaQueue = new List<DramaUnit>();
    public DramaUnit playingUnit;

    public DramaUnit DequeueDrama()
    {
        DramaUnit result = dramaQueue[0];
        dramaQueue.RemoveAt(0);
        return result;
    }
    public void EnqueueDrama(DramaUnit dramaUnit)
    {
        dramaQueue.Add(dramaUnit);
        return;
    }
    public void EnqueueDrama(DramaUnit[] dramaUnits)
    {
        dramaQueue.AddRange(dramaUnits);
        return;
    }
    public void InsertDrama(int index,DramaUnit[] dramaUnits)
    {
        dramaQueue.InsertRange(index, dramaUnits);
    }

    public void Next()
    {
        if(dramaQueue.Count > 0)
        {
            playingUnit = DequeueDrama();
            playingUnit.OnFinish += Next;
            playingUnit.Execute();
        }
    }

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        EnqueueDrama( publicGameEvents[0].dramaUnits.ToArray());
        Invoke("Next", 0.1f);
    }
}
[System.Serializable]
public abstract class DramaUnit
{
    public abstract void Execute();
    public void Finish()
    {
        if(OnFinish != null)
        {
            OnFinish();
        }
    }
    public delegate void DramaUnitEventHandler();
    public event DramaUnitEventHandler OnFinish;

}
[System.Serializable]
public class DialogueDramaUnit:DramaUnit
{
    public string content;
    public override void Execute()
    {
        GameActionExecuter.instance.ShowDialogue(content);
        Finish();
    }
}
[System.Serializable]
public class FloatVariableOperationDramaUnit:DramaUnit
{
    public string variableName;
    public NumOperation operation;
    public float attribute;
    public override void Execute()
    {
        GameActionExecuter.instance.OperateFloat(variableName, operation, attribute);
        Finish();
    }
}
[System.Serializable]
public class BoolVariableOperationDramaUnit:DramaUnit
{
    public string variableName;
    public BoolOperation operation;
    public bool attribute;
    public override void Execute()
    {
        GameActionExecuter.instance.OperateBool(variableName, operation, attribute);
        Finish();
    }
}
[System.Serializable]
public class IntVariableOperationDramaUnit : DramaUnit
{
    public string variableName;
    public NumOperation operation;
    public int attribute;

    public override void Execute()
    {
        GameActionExecuter.instance.OperateInt(variableName, operation, attribute);
        Finish();
    }
}
[System.Serializable]
public class StringVariableOperationDramaUnit : DramaUnit
{
    public string variableName;
    public StringOperation operation;
    public string attribute;
    public override void Execute()
    {
        GameActionExecuter.instance.OperateString(variableName, operation, attribute);
        Finish();
    }
}

public class ConditionDramaUnit : DramaUnit
{
    public Condition condition;
    public List<GeneralDramaUnit> subDramaUnitsIfTrue = new List<GeneralDramaUnit>();
    public List<GeneralDramaUnit> subDramaUnitsIfFalse = new List<GeneralDramaUnit>();

    public override void Execute()
    {
        if (condition.Evaluate())
        {
            GameDramaDirector.instance.InsertDrama(0, subDramaUnitsIfTrue.ToArray());
        }
        else
        {
            GameDramaDirector.instance.InsertDrama(0, subDramaUnitsIfFalse.ToArray());
        }
        Finish();
    }

}

public enum DramaUnitType
{
    Dialogue = 0,
    FloatOperation = 1,
    BoolOperation =2,
    IntOperation = 3,
    StringOperation =4,
    Condition =5
}
[System.Serializable]
public class GeneralDramaUnit : DramaUnit, ISerializationCallbackReceiver
{
    public string name;
    public DialogueDramaUnit dialogue;
    public FloatVariableOperationDramaUnit floatVariable;
    public IntVariableOperationDramaUnit intVariable;
    public BoolVariableOperationDramaUnit boolVariable;
    public StringVariableOperationDramaUnit stringVariable;
    public bool startIf;
    public bool startElse;
    public bool endIf;
    public Condition condition;

    public struct SerializableDramaUnit
    {
        public string name;
        public DialogueDramaUnit dialogue;
        public FloatVariableOperationDramaUnit floatVariable;
        public IntVariableOperationDramaUnit intVariable;
        public BoolVariableOperationDramaUnit boolVariable;
        public StringVariableOperationDramaUnit stringVariable;
        //ListTrue
        public int childCount;
        public int indexOfFirstChild;
    }

    public override void Execute()
    {
        dialogue.Execute();
        floatVariable.Execute();
        intVariable.Execute();
        boolVariable.Execute();
        stringVariable.Execute();
    }

    public void OnAfterDeserialize()
    {
        
    }

    public void OnBeforeSerialize()
    {

    }

    public void AddNodeToSerializedDramaUnits(ConditionDramaUnit unit)
    {

    }
}