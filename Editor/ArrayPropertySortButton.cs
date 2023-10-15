using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ArrayPropertySortButton
{
    private const float _defaultLayoutHeight = 24f;
    private static readonly GUIContent _label = new("Sort");

    private readonly SerializedObject _serializedObject;
    private readonly string _propertyPath;

    public ArrayPropertySortButton(SerializedProperty property)
    {
        _serializedObject = property.serializedObject;
        _propertyPath = property.propertyPath;
    }

    public void DrawLayout(float height = -1f)
    {
        height = height < 0 ? _defaultLayoutHeight : height;
        if (GUILayout.Button(_label, GUILayout.Height(height)))
        {
            Sort();
		}
    }

    public void Draw(Rect position)
    {
        if (GUI.Button(position, _label))
        {
            Sort();
        }
    }

    private void Sort()
    {
        if (_serializedObject == null)
        {
            Debug.LogError($"{nameof(_serializedObject)} == null");
            return;
        }

        var property = _serializedObject.FindProperty(_propertyPath);
        if (property == null)
        {
            Debug.Log($"{nameof(property)} == null; {nameof(_propertyPath)} = {_propertyPath}");
            return;
        }

        property.SortArrayElements();
    }
}
