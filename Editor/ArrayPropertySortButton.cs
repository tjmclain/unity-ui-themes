using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ArrayPropertySortButton
{
    private const float _defaultLayoutHeight = 24f;
    private static readonly GUIContent _label = new("Sort");

    private readonly IComparer<SerializedProperty> _comparer = null;

    public ArrayPropertySortButton(IComparer<SerializedProperty> comparer = null)
    {
        _comparer = comparer;
	}

    public void DrawLayout(SerializedProperty property, float height = -1f)
    {
        height = height < 0 ? _defaultLayoutHeight : height;
        if (GUILayout.Button(_label, GUILayout.Height(height)))
        {
            property.SortArrayElements(_comparer);
		}
    }

    public void Draw(Rect position, SerializedProperty property)
    {
        if (GUI.Button(position, _label))
        {
            property.SortArrayElements(_comparer);

		}
    }
}
