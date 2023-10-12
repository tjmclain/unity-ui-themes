using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Linq;
using System;

namespace Myna.Unity.Themes.Editor
{
	using ReorderableList = UnityEditorInternal.ReorderableList;
	using UnityObject = UnityEngine.Object;

	[CustomEditor(typeof(Style), true)]
	public class StyleInspector : UnityEditor.Editor
	{
		private ReorderableList _propertiesList = null;

		public override void OnInspectorGUI()
		{
			var className = serializedObject.FindProperty(Style.ClassNameFieldName);
			EditorGUILayout.PropertyField(className);

			_propertiesList.DoLayoutList();

			if (serializedObject.ApplyModifiedProperties())
			{
				ApplyStylesInScene();
			}
		}

		private void OnEnable()
		{
			if (SerializationUtility.ClearAllManagedReferencesWithMissingTypes(target))
			{
				Debug.Log($"SerializationUtility.ClearAllManagedReferencesWithMissingTypes: {target.name}", target);
				EditorUtility.SetDirty(target);
			}

			// `ReorderableList` References
			// https://blog.terresquall.com/2020/03/creating-reorderable-lists-in-the-unity-inspector/
			// https://va.lent.in/unity-make-your-lists-functional-with-reorderablelist/
			// https://github.com/ButtonFactoryGames/BF_SubclassListAttribute
			var properties = serializedObject.FindProperty(Style.PropertiesFieldName);
			_propertiesList = new ReorderableList(serializedObject, properties, true, true, true, true)
			{
				drawHeaderCallback = DrawHeader,
				drawElementCallback = DrawElement,
				elementHeightCallback = GetElementHeight,
				onAddDropdownCallback = AddDropdown
			};
		}

		private void DrawHeader(Rect rect)
		{
			var properties = _propertiesList.serializedProperty;
			EditorGUI.LabelField(rect, EditorGUIUtility.TrTextContent(properties.displayName), EditorStyles.boldLabel);
		}

		private void AddDropdown(Rect buttonRect, ReorderableList list)
		{
			var menu = new GenericMenu();
			var style = target as Style;
			var options = style.PropertyDefinitions.Keys
				.Select(x => new GUIContent(x))
				.ToArray();

			foreach (var option in options)
			{
				menu.AddItem(option, false, OnAddProperty, option.text);
			}

			menu.ShowAsContext();
		}

		private void OnAddProperty(object userData)
		{
			string propertyName = userData.ToString();
			var style = target as Style;
			var type = style.PropertyDefinitions[propertyName];
			var instance = Activator.CreateInstance(type) as StyleProperty;
			instance.Name = propertyName;

			var properties = _propertiesList.serializedProperty;
			int index = properties.arraySize;
			properties.arraySize++;

			var property = properties.GetArrayElementAtIndex(index);
			property.managedReferenceValue = instance;

			serializedObject.ApplyModifiedProperties();
		}

		private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
		{
			var properties = _propertiesList.serializedProperty;
			var property = properties.GetArrayElementAtIndex(index);

			EditorGUI.indentLevel++;
			EditorGUI.PropertyField(rect, property, true);
			EditorGUI.indentLevel--;
		}

		private float GetElementHeight(int index)
		{
			var properties = _propertiesList.serializedProperty;
			var property = properties.GetArrayElementAtIndex(index);
			return EditorGUI.GetPropertyHeight(property);
		}

		private void ApplyStylesInScene()
		{
			var stageHandle = StageUtility.GetCurrentStageHandle();
			var styleHelpers = stageHandle.FindComponentsOfType<StyleHelper>();
			foreach (var styleHelper in styleHelpers)
			{
				styleHelper.ApplyStyle();
			}
		}
	}
}