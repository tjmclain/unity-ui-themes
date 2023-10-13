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
			

			EditorGUI.PropertyField(position, property, label);

			if (property.objectReferenceValue == null)
			{
				property.objectReferenceValue = ProjectSettings.GetInstance().GetDefaultTheme().DefaultColorScheme;
			}
		}
	}
}