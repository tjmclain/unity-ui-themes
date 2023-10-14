using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Myna.Unity.Themes.Editor
{
	[CustomPropertyDrawer(typeof(ISingleLineProperty), true)]
	public class SingleLinePropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var properties = property.GetDirectChildren();
			if (properties.Count < 2)
			{
				return;
			}

			var labelProperty = properties[0];
			var valueProperty = properties[1];

			var labelRect = new Rect(position)
			{
				width = EditorGUIUtility.labelWidth,
				height = EditorGUIUtility.singleLineHeight
			};

			float spacing = EditorGUIUtility.standardVerticalSpacing * 2f;
			var valueRect = new Rect(position)
			{
				x = position.x + EditorGUIUtility.labelWidth + spacing,
				width = position.width - EditorGUIUtility.labelWidth - spacing,
				height = EditorGUIUtility.singleLineHeight
			};

			EditorGUI.PropertyField(labelRect, labelProperty, GUIContent.none);
			EditorGUI.PropertyField(valueRect, valueProperty, GUIContent.none);
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight;
		}
	}
}