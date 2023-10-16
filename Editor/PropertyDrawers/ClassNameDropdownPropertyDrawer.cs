using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Myna.Unity.Themes.Editor
{
	[CustomPropertyDrawer(typeof(ClassNameDropdownAttribute))]
	public class ClassNameDropdownPropertyDrawer : PropertyDrawer
	{
		private bool IsSerializedClassName => typeof(SerializedClassName).IsAssignableFrom(fieldInfo.FieldType);

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var dropdownAttribute = attribute as ClassNameDropdownAttribute;
			string themePropertyName = dropdownAttribute.ThemePropertyName;
			var themeProperty = !string.IsNullOrEmpty(themePropertyName)
				? property.GetSiblingProperty(themePropertyName) : null;

			// theme is either defined in a sibling property or the inspected target
			var theme = themeProperty != null
				? themeProperty.objectReferenceValue as Theme
				: property.serializedObject.targetObject as Theme;

			if (theme == null)
			{
				Debug.LogWarning($"{nameof(theme)} == null", property.serializedObject.targetObject);
				EditorGUI.PropertyField(position, property, label);
				return;
			}

			var classNames = theme.GetStyleClassNames().ToArray();
			if (classNames.Length < 1)
			{
				EditorGUI.PropertyField(position, property, label);
				return;
			}

			string className = GetClassName(property, theme);

			int index = Array.IndexOf(classNames, className);
			index = Math.Max(index, 0);

			var options = classNames.Select(x => new GUIContent(x)).ToArray();
			index = EditorGUI.Popup(position, label, index, options);

			SetClassName(property, theme, classNames[index]);
		}

		private string GetClassName(SerializedProperty property, Theme theme)
		{
			if (!IsSerializedClassName)
			{
				return property.stringValue;
			}

			var guid = property.FindPropertyRelative(SerializedClassName.GuidPropertyName);

			if (string.IsNullOrEmpty(guid.stringValue) || !theme.TryGetStyleClassName(guid.stringValue, out string className))
			{
				className = Theme.DefaultClassName;
			}

			var name = property.FindPropertyRelative(SerializedClassName.NamePropertyName);
			name.stringValue = className;
			return className;
		}

		private void SetClassName(SerializedProperty property, Theme theme, string className)
		{
			if (!IsSerializedClassName)
			{
				property.stringValue = className;
				return;
			}

			var name = property.FindPropertyRelative(SerializedClassName.NamePropertyName);
			name.stringValue = className;

			if (string.IsNullOrEmpty(className) || !theme.TryGetStyleGuid(className, out string guidValue))
			{
				guidValue = string.Empty;
			}

			var guid = property.FindPropertyRelative(SerializedClassName.GuidPropertyName);
			guid.stringValue = guidValue;
		}
	}
}