using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
[System.Serializable]
public class DramaScriptCore{

    public const string keyword_if = "if";
    public const string keyword_else = "else";
    public const string keyword_endif = "endif";
    public const string keyword_loop = "loop";
    public const string keyword_endloop = "endloop";
    public const string keyword_int = "int";
    public const string keyword_dialogue = "dialogue";
    public const string keyword_while = "while";
    public const string keyword_break = "break";
    public const char symbol_reference = '&';
    public const char symbol_note = '#';

    public List<string> scriptLines;
    public bool hold = false;

    public int index;
    public int ifDepth = 0;
    public bool ifSatisfied;
    [System.Serializable]
    public struct JumpInfo
    {
        public int index;
        public int target;

        public JumpInfo(int index, int target)
        {
            this.index = index;
            this.target = target;
        }
    }
    public List<JumpInfo> jumps = new List<JumpInfo>();

    public void Initialize(string[] scriptLines)
    {
        this.scriptLines = new List<string>(scriptLines);
        index = 0;
    }

    public void Next()
    {
        if(index < scriptLines.Count)
        {

            RunLine(scriptLines[index]);
            index++;
        }
    }

    public string GetCommandName(string command)
    {
        return SplitLine(command)[0];
    }

    public void RunLine(string script)
    {
        int commentStart = script.IndexOf(symbol_note);
        if(commentStart >= 0)
        {
            script = script.Substring(0, commentStart);
        }
        string[] splittedScript = SplitLine(script);
        if(splittedScript.Length <= 0)
        {
            return;
        }
        string command = splittedScript[0];
        //parse the command
        switch (command)
        {
            //logic
            case keyword_if:
                IfCommand(splittedScript);
                break;
            case keyword_else:
                CheckJump();
                break;
            case keyword_endif:
                break;
            case keyword_loop:
                LoopCommand(splittedScript);
                break;
            case keyword_endloop:
                CheckJump();
                break;
            case keyword_break:
                int jumpTo = FindCorrespondingCommand(keyword_endloop, keyword_loop, keyword_endloop, index);
                index = jumpTo;
                ClearJumpStartFrom(jumpTo);
                break;
            case keyword_int:
                IntCommand(splittedScript);
                break;
            case keyword_dialogue:
                DialogueCommand(splittedScript);
                break;
            default:
                Debug.Log(script);
                break;
        }
    }
    public void CheckJump()
    {
        for (int i = 0; i < jumps.Count; i++)
        {
            JumpInfo cur = jumps[i];
            if (cur.index == index)
            {
                index = cur.target;
                jumps.RemoveAt(i);
                index--;
                return;
            }
        }
    }

    public void LoopCommand(string[] attributes)
    {
        int endLoopIndex = FindCorrespondingCommand(keyword_endloop, keyword_loop, keyword_endloop, index);
        if(attributes.Length == 2)
        {
            //time loop
            int time;
            if(int.TryParse(attributes[1], out time))
            {
                for (int t = 0; t < time - 1; t++)
                {
                    jumps.Add(new JumpInfo(endLoopIndex, index + 1));
                }
            }
            else
            {
                Debug.LogError("Invalid loop time count attribute");
                return;
            }
        }
        else if(attributes.Length >= 3 && attributes[1] == keyword_while)
        {
            //condition loop
            string a = attributes[2];
            string comparation = "";
            string b = "";
            if(attributes.Length >= 5)
            {
                comparation = attributes[3];
                b = attributes[4];
            }
            if(EvaluateCondition(a, comparation, b))
            {
                jumps.Add(new JumpInfo(endLoopIndex, index));
            }
            else
            {
                index = endLoopIndex;
            }
        }
        else
        {
            Debug.LogError("Invalid loop description!");
        }
    }
    public int FindCorrespondingCommand(string targetCommand, string enterCommand, string exitCommand, int start)
    {
        int depth = 0;
        for (int i = start + 1; i < scriptLines.Count; i++)
        {
            string curCommand = GetCommandName(scriptLines[i]);
            if(depth == 0 && curCommand == targetCommand)
            {
                return i;
            }
            if (curCommand == enterCommand)
            {
                depth++;
                continue;
            }
            if (curCommand == exitCommand)
            {
                depth--;
                if(depth < 0)
                {
                    return -1;
                }
            }
            
        }
        return -1;
    }


    public void IfCommand(string[] attributes)
    {
        bool conditionValue = false;
        if(attributes.Length == 2)
        {
            conditionValue = EvaluateCondition(attributes[1], "", "");
        }
        else if (attributes.Length >= 4)
        {
            conditionValue = EvaluateCondition(attributes[1], attributes[2], attributes[3]);
        }
        else
        {
            Debug.LogError("Invalid condition discription!");
            return;
        }
        //Jump
        if (conditionValue)
        {
            //true: Eliminate the else part
            int elseIndex;
            int endIfIndex = FindCorrespondingEndif(index,out elseIndex);
            if (elseIndex > 0)
            {
                jumps.Add(new JumpInfo(elseIndex, endIfIndex));
            }
        }
        else
        {
            //false: Jump to the else part
            int elseIndex;
            int endIfIndex = FindCorrespondingEndif(index, out elseIndex);
            if (elseIndex > 0)
            {
                index = elseIndex;
            }
            else if(endIfIndex > 0)
            {
                index = endIfIndex;
            }
            else
            {
                Debug.LogError("Invalid syntax! There is supposed to be an endif command!");
            }
        }
    }
    public void ClearJumpStartFrom(int startIndex)
    {
        for(int i = 0; i < jumps.Count; i++)
        {
            JumpInfo curJump = jumps[i];
            if(curJump.index == startIndex)
            {
                jumps.RemoveAt(i);
                i--;
            }
        }
    }
    public int FindCorrespondingEndif(int ifIndex, out int elseIndex)
    {
        elseIndex = -1;
        int depth = 0;
        for(int i = ifIndex + 1; i < scriptLines.Count; i++)
        {
            string commandName = GetCommandName(scriptLines[i]);
            if (commandName == keyword_if)
            {
                depth++;
                continue;
            }
            if(commandName == keyword_endif)
            {
                if(depth == 0)
                {
                    return i;
                }
                else
                {
                    depth--;
                }
            }
            if(commandName == keyword_else)
            {
                if(depth == 0)
                {
                    elseIndex = i;
                }
            }
        }
        return -1;
    }
    public void IntCommand(string[] attributes)
    {
        if(attributes.Length < 4)
        {
            string debugAttributesValue = "";
            for(int i = 0; i < attributes.Length; i++)
            {
                debugAttributesValue += attributes[i] + " ";
            }
            Debug.LogError("Invalid int command! Int command takes 3 attributes. Please check your script! " + " Attributes Input: "+ debugAttributesValue);
            return;
        }
        string variableName = attributes[1];
        string operation = attributes[2];
        string attribute = attributes[3];

        var variableRef = GameManager.status.GetIntVariable(variableName);
        if(variableRef == null)
        {
            Debug.LogError("Cannot find int variable called " + variableName);
        }
        int attributeInt = 0;
        if(attribute[0] == symbol_reference)
        {
            attribute = Parse(attribute);
        }
        if(!int.TryParse(attribute,out attributeInt))
        {
            Debug.LogError("Cannot parse attribute " + attribute + " into int");
            return;
        }
        switch (operation)
        {
            case "+":
                variableRef.value += attributeInt;
                break;
            case "-":
                variableRef.value -= attributeInt;
                break;
            case "*":
                variableRef.value *= attributeInt;
                break;
            case "/":
                variableRef.value /= attributeInt;
                break;
            case "set":
                variableRef.value = attributeInt;
                break;
        }
    }
    public void DialogueCommand(string[] attributes)
    {
        if(attributes.Length == 2)
        {
            Debug.Log(Parse(attributes[1]));
            hold = true;
        }
    }

    public string Parse(string data)
    {
        if(data.Length < 1)
        {
            return "";
        }
        bool referencing = false;
        bool escape = false;
        string result = "";
        string referenceName = "";
        for(int i = 0; i < data.Length; i++)
        {
            char curChar = data[i];
            if (referencing)
            {
                if(curChar != ' ' && curChar != symbol_reference)
                {
                    referenceName += curChar;
                }
                else
                {
                    referencing = false;
                    result += GetReferenceValue(referenceName);
                }
                continue;
            }
            if (escape)
            {
                result += curChar;
                continue;
            }
            if (curChar == '\\')
            {
                escape = true;
                continue;
            }
            if (curChar == symbol_reference)
            {
                referencing = true;
                referenceName = "";
                continue;
            }
            
            result += curChar;
        }
        if (referencing)
        {
            result += GetReferenceValue(referenceName);
        }
        return result ;
    }
    public string GetReferenceValue(string attribute)
    {
        if(attribute[0] == symbol_reference)
        {
            attribute = attribute.Substring(1);
        }
        attribute = Unquote(attribute);
        return GameManager.status.GetVariableValue(attribute);
    }
    public string Unquote(string quoted)
    {
        if(quoted[0] == '"' && quoted[quoted.Length - 1] == '"')
        {
            quoted = quoted.Substring(1, quoted.Length - 2);
        }
        return quoted;
    }

    public bool EvaluateCondition(string attributeA, string comparation, string attributeB)
    {
        string value = Parse(attributeA);

        if(comparation == "")
        {
            if(value == "True")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        string anotherValue;
        if(attributeB[0] == symbol_reference)
        {
            anotherValue = GetReferenceValue(attributeB);
        }
        else
        {
            anotherValue = Unquote(attributeB);
        }
        float valueFloat;
        float anotherValueFloat;
        switch (comparation)
        {
            case "==":
                if(value == anotherValue)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case ">":
                
                if(float.TryParse(value,out valueFloat) && float.TryParse(anotherValue,out anotherValueFloat))
                {
                    if (valueFloat > anotherValueFloat)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                Debug.LogError("Failed to parse value " + value + " or " + anotherValue + " in to float number");
                return false;
            case ">=":
                if (value == anotherValue)
                {
                    return true;
                }

                if (float.TryParse(value, out valueFloat) && float.TryParse(anotherValue, out anotherValueFloat))
                {
                    if (valueFloat >= anotherValueFloat)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                Debug.LogError("Failed to parse value " + value + " or " + anotherValue + " in to float number");
                return false;
            case "<":
                if (float.TryParse(value, out valueFloat) && float.TryParse(anotherValue, out anotherValueFloat))
                {
                    if (valueFloat < anotherValueFloat)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                Debug.LogError("Failed to parse value " + value + " or " + anotherValue + " in to float number");
                return false;
            case "<=":
                if (value == anotherValue)
                {
                    return true;
                }

                if (float.TryParse(value, out valueFloat) && float.TryParse(anotherValue, out anotherValueFloat))
                {
                    if (valueFloat < anotherValueFloat)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                Debug.LogError("Failed to parse value " + value + " or " + anotherValue + " in to float number");
                return false;
            case "&&":
                if(value == "true" && anotherValue == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case "||":
                if(value == "true" || anotherValue == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case "not":
                if(value != anotherValue)
                {
                    return true;
                }
                else
                {
                    return false;
                }
        }
        return false;
    }

    public string[] SplitLine(string script)
    {
        bool insideString = false;
        bool escape = false;
        List<string> resultList = new List<string>();

        string curString = "";
        int scriptLength = script.Length;
        for(int i = 0; i < scriptLength; i++)
        {
            char curChar = script[i];
            if (insideString)
            {
                if (!escape && curChar == '\\')
                {
                    escape = true;
                }
                else if (!escape && curChar == '"')
                {
                    insideString = false;
                    resultList.Add(curString);
                    curString = "";
                }
                else if (escape)
                {
                    escape = false;
                    curString += curChar;
                }
                else
                {
                    curString += curChar;
                }
            }
            else if (curChar == ' '|| curChar == '\t')
            {
                if (curString.Length > 0)
                {
                    resultList.Add(curString);
                    curString = "";
                }
            }
            else if (curChar == '"')
            {
                insideString = !insideString;
            }
            else
            {
                curString += "" + curChar;
            }
        }
        if (curString.Length > 0)
        {
            resultList.Add(curString);
        }
        return resultList.ToArray();
    }

    public void Run()
    {
        while(!hold && index < scriptLines.Count)
        {
            Next();
        }
    }
}
