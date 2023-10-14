using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Myna.Unity.Themes.Editor
{
	[CustomPropertyDrawer(typeof(ColorScheme.ColorInfo))]
	public class ColorInfoPropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var nameRect = new Rect(position)
			{
				width = EditorGUIUtility.labelWidth,
				height = EditorGUIUtility.singleLineHeight
			};

			float spacing = EditorGUIUtility.standardVerticalSpacing * 2f;
			var colorRect = new Rect(position)
			{
				x = position.x + EditorGUIUtility.labelWidth + spacing,
				width = position.width - EditorGUIUtility.labelWidth - spacing,
				height = EditorGUIUtility.singleLineHeight
			};

			var nameProperty = property.FindPropertyRelative(ColorScheme.ColorInfo.NamePropertyName);
			var colorProperty = property.FindPropertyRelative(ColorScheme.ColorInfo.ColorPropertyName);

			nameProperty.stringValue = EditorGUI.TextField(nameRect, nameProperty.stringValue);
			colorProperty.colorValue = EditorGUI.ColorField(colorRect, colorProperty.colorValue);
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight;
		}
	}
}