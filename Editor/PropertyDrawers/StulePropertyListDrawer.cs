using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

using ReorderableList = UnityEditorInternal.ReorderableList;

[CustomPropertyDrawer(typeof(StylePropertyListAttribute))]
public class StylePropertyListDrawer : PropertyDrawer
{
	private bool _initialized = false;
	private ReorderableList _list;

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		if (!_initialized)
		{
			// https://blog.terresquall.com/2020/03/creating-reorderable-lists-in-the-unity-inspector/
			// https://va.lent.in/unity-make-your-lists-functional-with-reorderablelist/
			// https://github.com/ButtonFactoryGames/BF_SubclassListAttribute/blob/main/BF_SubclassListAttribute/Editor/SubclassListPropertyDrawer.cs
			var serializedObject = property.serializedObject;
			var properties = serializedObject.FindProperty(Style.PropertiesFieldName);
			_list = new ReorderableList(serializedObject, properties, true, true, true, true)
			{
				drawHeaderCallback = DrawHeader,
				drawElementCallback = DrawElement,
				elementHeightCallback = GetElementHeight,
				onAddDropdownCallback = AddDropdown
			};

			_initialized = true;
		}

		_list.DoList(position);
	}

	private void DrawHeader(Rect rect)
	{
		var property = _list.serializedProperty;
		EditorGUI.LabelField(rect, EditorGUIUtility.TrTextContent(property.displayName), EditorStyles.boldLabel);
	}

	private void AddDropdown(Rect buttonRect, ReorderableList list)
	{
		var propertyAttribute = attribute as StylePropertyListAttribute;
		var styleType = propertyAttribute.StyleType;
		var propertyDefintionsInfo = styleType.GetProperty(nameof(Style.PropertyDefinitions));
		var propertyDefinitions = propertyDefintionsInfo.GetValue(null) as Dictionary<string, Type>;

		var menu = new GenericMenu();
		foreach (var kvp in propertyDefinitions)
		{
			menu.AddItem(new GUIContent(kvp.Key), false, OnAddProperty, kvp);
		}
		menu.ShowAsContext();

		// TODO
		//var menu = new GenericMenu();
		//var style = target as Style;

		//foreach (var option in options)
		//{
		//	menu.AddItem(option, false, OnAddProperty, option.text);
		//}

		//menu.ShowAsContext();
	}

	private void OnAddProperty(object userData)
	{
		var kvp = (KeyValuePair<string, Type>)userData;
		var type = kvp.Value;

		var instance = Activator.CreateInstance(type) as StyleProperty;
		instance.Name = kvp.Key;

		var properties = _list.serializedProperty;
		int index = properties.arraySize;
		properties.arraySize++;

		var property = properties.GetArrayElementAtIndex(index);
		property.managedReferenceValue = instance;

		_list.index = index;

		properties.serializedObject.ApplyModifiedProperties();
	}

	private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
	{
		var properties = _list.serializedProperty;
		var property = properties.GetArrayElementAtIndex(index);

		EditorGUI.indentLevel++;
		EditorGUI.PropertyField(rect, property, true);
		EditorGUI.indentLevel--;
	}

	private float GetElementHeight(int index)
	{
		var properties = _list.serializedProperty;
		var property = properties.GetArrayElementAtIndex(index);
		return EditorGUI.GetPropertyHeight(property);
	}
}