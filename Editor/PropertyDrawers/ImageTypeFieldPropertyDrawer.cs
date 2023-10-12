using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[CustomPropertyDrawer(typeof(ImageTypeFieldAttribute))]
public class ImageTypeFieldPropertyDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		if (ShowProperty(property))
		{
			EditorGUI.PropertyField(position, property, label);
		}
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		if (ShowProperty(property))
		{
			return EditorGUI.GetPropertyHeight(property);
		}
		else
		{
			return -EditorGUIUtility.standardVerticalSpacing;
		}
	}

	protected virtual bool ShowProperty(SerializedProperty property)
	{
		var fieldAttribute = attribute as ImageTypeFieldAttribute;
		var visibleInTypes = fieldAttribute.VisibleInTypes;
		var serializedObject = property.serializedObject;
		var imageTypeProperty = property.GetSiblingProperty(ImageType.TypePropertyName);
		var imageType = (Image.Type)imageTypeProperty.enumValueIndex;
		return System.Array.Exists(visibleInTypes, x => x == imageType);
	}
}