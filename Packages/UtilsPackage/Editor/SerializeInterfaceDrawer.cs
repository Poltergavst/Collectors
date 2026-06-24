using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomPropertyDrawer(typeof(SerializeInterfaceAttribute))]
public class SerializeInterfaceDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Type requiredType = (attribute as SerializeInterfaceAttribute).Type;

        //проверяем на gameObject
        if (!(fieldInfo.FieldType == typeof(GameObject) || typeof(IEnumerable<GameObject>).IsAssignableFrom(fieldInfo.FieldType)))
        {
            EditorGUI.HelpBox(position, "No.", MessageType.Error);
            return;
        }

        if(IsInvalidObject(property.objectReferenceValue, requiredType))
        {
            property.objectReferenceValue = null;
        }

        UpdateDropIcon(position, requiredType);

        property.objectReferenceValue = EditorGUI.ObjectField(position, label, property.objectReferenceValue, typeof(GameObject), true);
    }

    private void UpdateDropIcon(Rect position, Type requiredType)
    {
        if (position.Contains(Event.current.mousePosition) == false)
            return;

        foreach (Object reference in DragAndDrop.objectReferences)
        {
            if (IsInvalidObject(reference, requiredType))
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
                return;
            }
        }
    }

    private bool IsInvalidObject(Object sumobject, Type requiredType)
    {
        if (sumobject is GameObject gameObject)
        {
            return (gameObject.GetComponent(requiredType) == null);
        }

        return false;
    }
}
