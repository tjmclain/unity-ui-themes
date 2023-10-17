using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Myna.Unity.Themes.Editor
{
	[CustomEditor(typeof(Theme))]
	public class ThemeInspector : UnityEditor.Editor
	{
		private readonly ArrayPropertySortButton _sortButton = new();
		private readonly RuntimeStylesDrawer _runtimeStylesDrawer = new();

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			DrawActiveColorScheme(serializedObject);

			var styles = serializedObject.FindProperty(Theme.StylesPropertyName);
			EditorGUILayout.PropertyField(styles);

			if (styles.isExpanded)
			{
				_sortButton.DrawLayout(styles);
			}

			_runtimeStylesDrawer.DrawLayout(serializedObject);

			serializedObject.ApplyModifiedProperties();
		}

		private static void DrawActiveColorScheme(SerializedObject serializedObject)
		{
			if (serializedObject.isEditingMultipleObjects)
			{
				return;
			}

			var theme = serializedObject.targetObject as Theme;
			var activeColorScheme = theme.ActiveColorScheme;
			if (activeColorScheme == null)
			{
				return;
			}

			var options = new List<string>();
			foreach (var colorScheme in theme.ColorSchemes)
			{
				options.Add(colorScheme.name);
			}

			var defaultOption = theme.DefaultColorScheme != null ? theme.DefaultColorScheme.name : string.Empty;
			if (!string.IsNullOrEmpty(defaultOption) && !options.Contains(defaultOption))
			{
				options.Add(defaultOption);
			}

			if (options.Count == 0)
			{
				return;
			}

			int index = options.IndexOf(activeColorScheme.name);

			string label = ObjectNames.NicifyVariableName(nameof(Theme.ActiveColorScheme));
			int selected = EditorGUILayout.Popup(label, Math.Max(0, index), options.ToArray());
			if (selected != index)
			{
				string option = options[selected];
				theme.SetColorScheme(option);
				StyleHelperEditorUtility.ApplyStylesInScene();
			}
		}

		private class RuntimeStylesDrawer
		{
			private bool _foldout = false;

			public void DrawLayout(SerializedObject serializedObject)
			{
				if (serializedObject.isEditingMultipleObjects)
				{
					return;
				}

				var theme = serializedObject.targetObject as Theme;
				var styles = theme.RuntimeStyles;
				if (styles.Count == 0)
				{
					return;
				}

				string label = ObjectNames.NicifyVariableName(nameof(Theme.RuntimeStyles));
				_foldout = EditorGUILayout.Foldout(_foldout, label);
				if (!_foldout)
				{
					return;
				}

				EditorGUI.BeginDisabledGroup(true);
				var list = styles.OrderBy(x => x.Key);
				foreach (var kvp in list)
				{
					EditorGUILayout.ObjectField(kvp.Key, kvp.Value, typeof(Theme.RuntimeStyle), false);
				}
				EditorGUI.EndDisabledGroup();
			}
		}
	}
}