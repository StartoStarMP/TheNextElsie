using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

/*
[CustomPropertyDrawer(typeof(Requirement))]
public class RequirementEditor : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        var container = new VisualElement();

        var reqTypeField = new PropertyField(property.FindPropertyRelative("requirementType"));

        container.Add(reqTypeField);
        return container;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //base.OnGUI(position, property, label);

        //SerializedProperty type = property.FindPropertyRelative("requirementType");
        //EditorGUI.LabelField(new Rect(position.x, position.y, position.width, position.height), label.text);

        //GUIContent guiType = new GUIContent("Type");

        //EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, position.height), type, guiType);
    }
}
*/
