using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.Experimental.GraphView;

namespace Myna.Unity.Themes.Editor
{
	[CustomEditor(typeof(ColorSchemeOverride))]
	public class ColorSchemeOverrideInspector : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			var referenceColorSchemeProperty = serializedObject.FindProperty(ColorSchemeOverride.ReferenceColorSchemePropertyName);
			EditorGUILayout.PropertyField(referenceColorSchemeProperty);

			var referenceColorScheme = referenceColorSchemeProperty.objectReferenceValue as ColorScheme;
			DrawColors(referenceColorScheme);

			if (serializedObject.ApplyModifiedProperties())
			{
				StyleHelperEditorUtility.ApplyStylesInScene();
			}
		}

		private void DrawColors(ColorScheme referenceColorScheme)
		{
			if (referenceColorScheme == null)
			{
				return;
			}

			var colors = serializedObject.FindProperty(ColorScheme.ColorsPropertyName);
			colors.isExpanded = EditorGUILayout.Foldout(colors.isExpanded, colors.displayName);

			if (!colors.isExpanded)
			{
				return;
			}

			float toggleWidth = EditorStyles.toggle.CalcSize(GUIContent.none).x;

			EditorGUI.indentLevel++;
			var referenceColors = referenceColorScheme.Colors;
			foreach (var referenceColor in referenceColors)
			{
				EditorGUILayout.BeginHorizontal();
				bool exists = colors.TryGetArrayElementIndex(x =>
				{
					var guid = x.FindPropertyRelative(ColorScheme.ColorInfo.GuidPropertyName);
					return guid.stringValue == referenceColor.Guid;
				}, out int index);

				if (exists)
				{
					var element = colors.GetArrayElementAtIndex(index);
					var name = element.FindPropertyRelative(ColorScheme.ColorInfo.NamePropertyName);
					name.stringValue = referenceColor.Name;

					var color = element.FindPropertyRelative(ColorScheme.ColorInfo.ColorPropertyName);
					color.colorValue = EditorGUILayout.ColorField(name.stringValue, color.colorValue);
				}
				else
				{
					EditorGUI.BeginDisabledGroup(true);
					EditorGUILayout.ColorField(referenceColor.Name, referenceColor.Color);
					EditorGUI.EndDisabledGroup();
				}

				bool toggle = EditorGUILayout.Toggle(exists, EditorStyles.toggle, GUILayout.Width(toggleWidth));
				if (toggle != exists)
				{
					if (toggle)
					{
						index = colors.arraySize;
						colors.arraySize++;

						var element = colors.GetArrayElementAtIndex(index);
						var name = element.FindPropertyRelative(ColorScheme.ColorInfo.NamePropertyName);
						name.stringValue = referenceColor.Name;

						var color = element.FindPropertyRelative(ColorScheme.ColorInfo.ColorPropertyName);
						color.colorValue = referenceColor.Color;

						var guid = element.FindPropertyRelative(ColorScheme.ColorInfo.GuidPropertyName);
						guid.stringValue = referenceColor.Guid;
					}
					else
					{
						colors.DeleteArrayElementAtIndex(index);
					}
				}

				EditorGUILayout.EndHorizontal();
			}
			EditorGUI.indentLevel--;
		}
	}
}