using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

namespace Myna.Unity.Themes.Editor
{
	using ReorderableList = UnityEditorInternal.ReorderableList;

	[CustomEditor(typeof(Style), true)]
	public class StyleInspector : UnityEditor.Editor
	{
		private readonly ArrayPropertySortButton _sortButton = new();
		private ReorderableList _propertiesList = null;

		public override void OnInspectorGUI()
		{
			_propertiesList.DoLayoutList();

			_sortButton.DrawLayout(_propertiesList.serializedProperty);

			if (serializedObject.ApplyModifiedProperties())
			{
				StyleHelperEditorUtility.ApplyStylesInScene();
			}
		}

		protected virtual void OnEnable()
		{
			if (SerializationUtility.ClearAllManagedReferencesWithMissingTypes(target))
			{
				Debug.Log($"{nameof(SerializationUtility.ClearAllManagedReferencesWithMissingTypes)}: {target.name}", target);
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
				onAddDropdownCallback = AddDropdown,
				onCanAddCallback = CanAddProperty,
			};
		}

		private IEnumerable<string> GetPropertyNamesForAdd(SerializedProperty property)
		{
			var existingPropertyNames = new List<string>();
			for (int i = 0; i < property.arraySize; i++)
			{
				var element = property.GetArrayElementAtIndex(i);
				var name = element.FindPropertyRelative(StyleProperty.NamePropertyName);
				if (name == null)
				{
					continue;
				}
				existingPropertyNames.Add(name.stringValue);
			}

			return StylePropertyDefinitions.PropertyNames
				.Where(x => !existingPropertyNames.Contains(x));
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

			var propertyNames = GetPropertyNamesForAdd(list.serializedProperty).OrderBy(x => x);

			foreach (var propertyName in propertyNames)
			{
				menu.AddItem(new GUIContent(propertyName), false, OnAddProperty, propertyName);
			}

			menu.ShowAsContext();
		}

		private void OnAddProperty(object userData)
		{
			string propertyName = userData.ToString();
			if (!StylePropertyDefinitions.TryGetPropertyType(propertyName, out var propertyType))
			{
				Debug.LogError($"No property definition for '{propertyName}'");
				return;
			}

			var instance = Activator.CreateInstance(propertyType) as IStyleProperty;
			instance.Name = propertyName;

			var properties = _propertiesList.serializedProperty;
			int index = properties.arraySize;
			properties.arraySize++;

			var property = properties.GetArrayElementAtIndex(index);
			property.managedReferenceValue = instance;

			serializedObject.ApplyModifiedProperties();

			_propertiesList.Select(index);
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

		private bool CanAddProperty(ReorderableList list)
		{
			return GetPropertyNamesForAdd(list.serializedProperty).Count() > 0;
		}
	}
}