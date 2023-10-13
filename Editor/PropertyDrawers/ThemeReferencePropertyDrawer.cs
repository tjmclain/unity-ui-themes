using System.Collections;
using System.Collections.Generic;
using Myna.Unity.Themes;
using UnityEditor;
using UnityEngine;

namespace Myna.Unity.Themes.Editor
{
	[CustomPropertyDrawer(typeof(ThemeReferenceAttribute))]
	public class ThemeReferencePropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.PropertyField(position, property, label);

			if (property.objectReferenceValue == null)
			{
				property.objectReferenceValue = ProjectSettings.GetInstance().GetDefaultTheme();
			}
		}
	}
}