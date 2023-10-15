using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Myna.Unity.Themes.Editor
{
	[CustomEditor(typeof(ColorScheme), true)]
	public class ColorSchemeInspector : UnityEditor.Editor
	{
		private readonly ArrayPropertySortButton _sortButton = new();

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			var colors = serializedObject.FindProperty(ColorScheme.ColorsPropertyName);
			EditorGUILayout.PropertyField(colors);

			if (colors.isExpanded)
			{
				_sortButton.DrawLayout(colors);
			}


			serializedObject.ApplyModifiedProperties();
		}

	}
}