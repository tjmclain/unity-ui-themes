using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace Myna.Unity.Themes.Editor
{
	[CustomEditor(typeof(Theme))]
	public class ThemeInspector : UnityEditor.Editor
	{
		private StylesByTypeDrawer _stylesByTypeDrawer = new();

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			// organize styles by type
			_stylesByTypeDrawer.Draw(serializedObject);
		}

		private class StylesByTypeDrawer
		{
			private Dictionary<Type, bool> _foldouts = new();

			public void Draw(SerializedObject serializedObject)
			{
				var styles = new Dictionary<Type, List<Style>>();
				var stylesProperty = serializedObject.FindProperty(Theme.StylesPropertyName);
				int count = count = stylesProperty.arraySize;
				if (count == 0)
				{
					return;
				}

				for (int i = 0; i < count; i++)
				{
					var element = stylesProperty.GetArrayElementAtIndex(i);
					var style = element.objectReferenceValue as Style;
					if (style == null)
					{
						continue;
					}

					var type = style.GetType();
					if (!styles.TryGetValue(type, out var list))
					{
						list = new List<Style>();
						styles[type] = list;
					}

					list.Add(style);
				}

				EditorGUILayout.LabelField("Styles By Type", EditorStyles.boldLabel);
				EditorGUI.indentLevel++;
				EditorGUI.BeginDisabledGroup(true);
				foreach (var kvp in styles)
				{
					var type = kvp.Key;

					// draw foldout
					_foldouts.TryGetValue(type, out bool foldout);
					string label = $"{ObjectNames.NicifyVariableName(type.Name)}s";
					foldout = EditorGUILayout.Foldout(foldout, label);
					_foldouts[type] = foldout;

					if (!foldout)
					{
						continue;
					}

					EditorGUI.indentLevel++;

					// draw styles in list
					var list = kvp.Value;
					list.Sort((x, y) => string.Compare(x.ClassName, y.ClassName, true));
					foreach (var style in kvp.Value)
					{
						string className = $".{style.ClassName}";
						EditorGUILayout.ObjectField(className, style, typeof(Style), false);
					}

					EditorGUI.indentLevel--;
				}
				EditorGUI.EndDisabledGroup();
				EditorGUI.indentLevel--;
			}
		}
	}
}