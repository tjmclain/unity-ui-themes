using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Myna.Unity.Themes.Editor
{
	[CustomPropertyDrawer(typeof(ColorSchemeReferenceAttribute))]
	public class ColorSchemeReferencePropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var colorScheme = property.objectReferenceValue;
			if (colorScheme == null)
			{
				colorScheme = ProjectSettings.GetInstance().GetDefaultTheme().DefaultColorScheme;
			}

			property.objectReferenceValue = EditorGUI.ObjectField(position, label, colorScheme, typeof(ColorScheme), false);
		}
	}
}