using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomPropertyDrawer(typeof(GeneralDramaUnit))]
public class GeneralDramaUnitPropertyDrawer : PropertyDrawer {
    float totalHeight;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        float initialY = position.y;
        int lineHeight = 18;
        string name = property.FindPropertyRelative("name").stringValue;
        bool show = property.FindPropertyRelative("show").boolValue;
        position.height = lineHeight;
        property.FindPropertyRelative("show").boolValue = EditorGUI.Foldout(position, show, name, true);
        EditorGUI.LabelField(position, name);
        
        if (show)
        {
            position.y += lineHeight;
            EditorGUI.PropertyField(position, property.FindPropertyRelative("name"));

            position.y += lineHeight;
            var typeProperty = property.FindPropertyRelative("type");
            string typeName;

            if (typeProperty.type == "Enum")
            {
                typeName = typeProperty.enumNames[typeProperty.enumValueIndex];
            }
            else
            {
                DramaUnitType temp = (DramaUnitType)typeProperty.intValue;
                typeName = temp.ToString();
            }

            EditorGUI.PropertyField(position, typeProperty);
            //EditorGUI.BeginChangeCheck();
            //var value = EditorGUI.Popup(position, typeProperty.enumValueIndex,typeProperty.enumDisplayNames);
            //if (EditorGUI.EndChangeCheck())
            //{
            //    typeProperty.enumValueIndex = value;
            //}
            position.y += lineHeight;
            EditorGUI.PropertyField(position, property.FindPropertyRelative("variableName"));
            switch (typeName)
            {
                case "Dialogue":
                    position.y += lineHeight;
                    EditorGUI.PropertyField(position, property.FindPropertyRelative("dialogueContent"));
                    break;
                case "FloatOperation":
                    position.y += lineHeight;
                    EditorGUI.PropertyField(position, property.FindPropertyRelative("numOperation"));
                    position.y += lineHeight;
                    EditorGUI.PropertyField(position, property.FindPropertyRelative("floatAttribute"));
                    break;
                case "IntOperation":
                    position.y += lineHeight;
                    EditorGUI.PropertyField(position, property.FindPropertyRelative("numOperation"));
                    position.y += lineHeight;
                    EditorGUI.PropertyField(position, property.FindPropertyRelative("intAttribute"));
                    break;
                case "BoolOperation":
                    position.y += lineHeight;
                    EditorGUI.PropertyField(position, property.FindPropertyRelative("boolOperation"));
                    position.y += lineHeight;
                    EditorGUI.PropertyField(position, property.FindPropertyRelative("boolAttribute"));
                    break;
                case "StringOperation":
                    position.y += lineHeight;
                    EditorGUI.PropertyField(position, property.FindPropertyRelative("stringOperation"));
                    position.y += lineHeight;
                    EditorGUI.PropertyField(position, property.FindPropertyRelative("stringAttribute"));
                    break;
                case "Condition":
                    position.y += lineHeight;
                    var subTrueProperty = property.FindPropertyRelative("subDramaUnitsIfTrue");
                    EditorGUI.PropertyField(position, subTrueProperty,true);
                    float subTrueHeight = EditorGUI.GetPropertyHeight(subTrueProperty);
                    position.y += subTrueHeight;
                    var subFalseProperty = property.FindPropertyRelative("subDramaUnitsIfFalse");
                    EditorGUI.PropertyField(position, subFalseProperty,true);
                    float subFalseHeight = EditorGUI.GetPropertyHeight(subFalseProperty);
                    position.y += subFalseHeight;
                    break;
            }
            
        }
        totalHeight = position.y - initialY + lineHeight;
        property.FindPropertyRelative("attributeHeight").floatValue = totalHeight;
        

        //Debug.Log(GetPropertyHeight(property,label));

        //float totalHeight = EditorGUI.GetPropertyHeight(property, label, true);
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return property.FindPropertyRelative("attributeHeight").floatValue;
    }
}
