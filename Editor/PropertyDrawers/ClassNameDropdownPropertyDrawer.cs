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
			var target = property.serializedObject.targetObject;
			var styleHelper = target as StyleHelper;
			if (styleHelper == null)
			{
				Debug.LogWarning($"{nameof(target)} in not {nameof(StyleHelper)}", target);
				EditorGUI.PropertyField(position, property, label);
				return;
			}

			var theme = styleHelper.Theme;
			if (theme == null)
			{
				Debug.LogWarning($"{nameof(theme)} == null", target);
				EditorGUI.PropertyField(position, property, label);
				return;
			}

			var styleType = styleHelper.StyleType;
			var classNames = theme.GetStyleClassNames(styleType).ToArray();
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