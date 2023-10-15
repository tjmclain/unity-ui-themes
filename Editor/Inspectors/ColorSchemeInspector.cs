using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Myna.Unity.Themes.Editor
{
	[CustomEditor(typeof(ColorScheme), true)]
	public class ColorSchemeInspector : UnityEditor.Editor
	{
		private ArrayPropertySortButton _sortButton;

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			var colors = serializedObject.FindProperty(ColorScheme.ColorsPropertyName);
			EditorGUILayout.PropertyField(colors, true);

			_sortButton.DrawLayout();

			serializedObject.ApplyModifiedProperties();
		}

		protected virtual void OnEnable()
		{
			var colors = serializedObject.FindProperty(ColorScheme.ColorsPropertyName);
			_sortButton = new ArrayPropertySortButton(colors);
		}
	}
}