using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

	}
}