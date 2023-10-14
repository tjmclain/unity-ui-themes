using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Myna.Unity.Themes.Editor
{
	[CustomPropertyDrawer(typeof(ClassNameDropdownAttribute))]
	public class ClassNameDropdownPropertyDrawer : PropertyDrawer
	{
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

			int index = Array.IndexOf(classNames, property.stringValue);
			index = Math.Max(index, 0);

			var options = classNames.Select(x => new GUIContent(x)).ToArray();
			index = EditorGUI.Popup(position, label, index, options);

			property.stringValue = classNames[index];
		}
	}
}