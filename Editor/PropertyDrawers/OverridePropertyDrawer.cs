using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.CodeDom;
using System.Reflection;
using System.Linq;

namespace Myna.Unity.Themes.Editor
{
	[CustomPropertyDrawer(typeof(OverrideProperty), true)]
	public class OverridePropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			// if we only have 1 child property, draw that; otherwise draw a normal property field
			var children = SerializedPropertyUtility.GetDirectChildren(property);
			var mainProperty = children.Count == 1 ? children[0] : property;
			var enabledProperty = property.FindPropertyRelative(OverrideProperty.EnabledPropertyName);

			var toggleSize = EditorStyles.toggle.CalcSize(GUIContent.none);

			float buffer = EditorGUIUtility.singleLineHeight * 0.5f;

			var valuePos = new Rect(position) { width = position.width - toggleSize.x - buffer };
			var enabledPos = new Rect(position)
			{
				x = position.x + valuePos.width + buffer,
				width = toggleSize.x,
			};

			EditorGUI.BeginDisabledGroup(!enabledProperty.boolValue);
			EditorGUI.PropertyField(valuePos, mainProperty, label);
			EditorGUI.EndDisabledGroup();

			enabledProperty.boolValue = EditorGUI.Toggle(enabledPos, enabledProperty.boolValue);
		}
	}
}