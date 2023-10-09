using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Myna.Unity.Themes.Editor
{
	[CustomPropertyDrawer(typeof(StyleHelperClassNameAttribute))]
	public class StyleHelperClassNamePropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var target = property.serializedObject.targetObject;
			var styleHelper = target as StyleHelper;
			if (styleHelper == null)
			{
				Debug.LogWarning($"{nameof(styleHelper)} == null", target);
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

			var classNames = theme.GetStyleClassNames(styleHelper.ComponentType).ToArray();
			if (classNames.Length < 2)
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