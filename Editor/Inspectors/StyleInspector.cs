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
		private AddPropertyButton _addPropertyButton = null;
		private ReorderableList _propertiesList = null;

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			// properties list
			//var properties = serializedObject.FindProperty("_properties");
			//if (properties == null)
			//{
			//	return;
			//}
			//EditorGUILayout.PropertyField(properties);

			_propertiesList.DoLayoutList();

			// add property button
			_addPropertyButton.Draw();

			if (serializedObject.ApplyModifiedProperties())
			{
				ApplyStylesInScene();
			}
		}

		private void OnEnable()
		{
			_addPropertyButton = new AddPropertyButton(target);

			// TODO: move this to custom property drawer class
			// https://blog.terresquall.com/2020/03/creating-reorderable-lists-in-the-unity-inspector/
			// https://va.lent.in/unity-make-your-lists-functional-with-reorderablelist/
			var properties = serializedObject.FindProperty(Style.PropertiesFieldName);
			_propertiesList = new ReorderableList(serializedObject, properties, true, true, true, true);
			_propertiesList.drawHeaderCallback = DrawHeader;
			_propertiesList.drawElementCallback = DrawElement;
			_propertiesList.elementHeightCallback = GetElementHeight;
			_propertiesList.onAddDropdownCallback = AddDropdown;
		}

		private void DrawHeader(Rect rect)
		{
			var properties = serializedObject.FindProperty(Style.PropertiesFieldName);
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
			var property = Activator.CreateInstance(type) as StyleProperty;
			property.Name = propertyName;

			Undo.RecordObject(target, "Add Property");
			style.Properties.Add(property);
			EditorUtility.SetDirty(target);
		}

		private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
		{
			//static void DrawPropertyRecursive(SerializedProperty property, ref Rect rect)
			//{
			//	float height = property.hasChildren
			//		? EditorGUIUtility.singleLineHeight
			//		: EditorGUI.GetPropertyHeight(property);

			//	rect.height = height;

			//	EditorGUI.PropertyField(rect, property);

			//	rect.y += EditorGUIUtility.standardVerticalSpacing;
			//	rect.y += height;

			//	if (!property.isExpanded)
			//	{
			//		return;
			//	}

			//	EditorGUI.indentLevel++;

			//	var children = property.GetDirectChildren();
			//	foreach (var child in children)
			//	{
			//		DrawPropertyRecursive(child, ref rect);
			//	}

			//	EditorGUI.indentLevel--;
			//}

			var properties = serializedObject.FindProperty(Style.PropertiesFieldName);
			var property = properties.GetArrayElementAtIndex(index);

			float height = EditorGUIUtility.singleLineHeight;
			rect.height = height;
			EditorGUI.indentLevel++;
			EditorGUI.PropertyField(rect, property);

			if (property.isExpanded)
			{
				rect.y += height;
				rect.y += EditorGUIUtility.standardVerticalSpacing;

				EditorGUI.indentLevel++;

				var children = property.GetDirectChildren();
				foreach (var child in children)
				{
					height = EditorGUI.GetPropertyHeight(child);
					rect.height = height;

					EditorGUI.PropertyField(rect, child);

					rect.y += height;
					rect.y += EditorGUIUtility.standardVerticalSpacing;
				}

				EditorGUI.indentLevel--;
			}

			EditorGUI.indentLevel--;

			//float height = EditorGUI.GetPropertyHeight(property);

			//DrawPropertyRecursive(property, ref rect);
			//EditorGUI.indentLevel--;
		}

		private float GetElementHeight(int index)
		{
			var properties = serializedObject.FindProperty(Style.PropertiesFieldName);
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

		private class AddPropertyButton
		{
			private UnityObject _target = null;
			private int _index = -1;

			public AddPropertyButton(UnityObject target)
			{
				_target = target;
			}

			public void Draw()
			{
				var style = _target as Style;
				if (style == null)
				{
					Debug.LogError($"{nameof(_target)} is not {nameof(Style)}");
					return;
				}

				var propertyNames = style.PropertyDefinitions.Keys
					.Where(x => !style.Properties.Exists(y => y.Name == x))
					.ToArray();

				if (propertyNames.Length == 0)
				{
					return;
				}

				_index = Mathf.Clamp(_index, 0, propertyNames.Length - 1);

				EditorGUILayout.BeginHorizontal();
				_index = EditorGUILayout.Popup(_index, propertyNames);
				if (GUILayout.Button("Add", GUILayout.ExpandWidth(false)))
				{
					string propertyName = propertyNames[_index];
					var type = style.PropertyDefinitions[propertyName];
					var property = Activator.CreateInstance(type) as StyleProperty;
					property.Name = propertyName;

					Undo.RecordObject(_target, "Add Property");
					style.Properties.Add(property);
					EditorUtility.SetDirty(_target);
				}
				EditorGUILayout.EndHorizontal();
			}
		}
	}
}