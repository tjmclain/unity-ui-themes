using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Myna.Unity.Themes.Editor
{
	[CustomPropertyDrawer(typeof(StyleProperty), true)]
	public class StylePropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var children = property.GetDirectChildren();
			var mainProperty = children.Count == 1 ? children[0] : property;

			EditorGUI.PropertyField(position, mainProperty, label, true);
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var children = property.GetDirectChildren();
			var mainProperty = children.Count == 1 ? children[0] : property;

			return EditorGUI.GetPropertyHeight(mainProperty);
		}
	}
}