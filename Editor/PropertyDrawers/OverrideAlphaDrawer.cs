using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Myna.Unity.Themes.Editor
{
	[CustomPropertyDrawer(typeof(AlphaPropertyOverride))]
	public class OverrideAlphaDrawer : OverridePropertyDrawer
	{
		protected override void DrawMainProperty(Rect rect, SerializedProperty property, GUIContent label)
		{
			property.floatValue = EditorGUI.Slider(rect, label, property.floatValue, 0f, 1f);
		}
	}
}