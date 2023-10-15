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

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			var styles = serializedObject.FindProperty(Theme.StylesPropertyName);
			EditorGUILayout.PropertyField(styles);

			if (styles.isExpanded)
			{
				_sortButton.DrawLayout(styles);
			}

			serializedObject.ApplyModifiedProperties();
		}

		private class ThemeStylesDrawer
		{
			private bool _foldout = false;

			public void DrawLayout(SerializedObject serializedObject)
			{
				if (serializedObject.isEditingMultipleObjects)
				{
					return;
				}

				var theme = serializedObject.targetObject as Theme;
				var styles = theme.ThemeStyles;
				if (styles.Count == 0)
				{
					return;
				}

				string label = ObjectNames.NicifyVariableName(nameof(Theme.ThemeStyles));
				_foldout = EditorGUILayout.Foldout(_foldout, label);
				if (!_foldout)
				{
					return;
				}

				EditorGUI.BeginDisabledGroup(true);
				var list = styles.OrderBy(x => x.Key);
				foreach (var kvp in list)
				{
					EditorGUILayout.ObjectField(kvp.Key, kvp.Value, typeof(Theme.ThemeStyle), false);
				}
				EditorGUI.EndDisabledGroup();
			}
		}
	}
}