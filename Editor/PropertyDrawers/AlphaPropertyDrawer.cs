using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Myna.Unity.Themes.Editor
{
	[CustomPropertyDrawer(typeof(AlphaProperty))]
	public class AlphaPropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var alpha = property.FindPropertyRelative(AlphaProperty.ValuePropertyName);

			alpha.floatValue = EditorGUI.Slider(position, label, alpha.floatValue, 0f, 1f);
		}
	}
}