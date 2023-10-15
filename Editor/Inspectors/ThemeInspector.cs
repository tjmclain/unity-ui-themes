using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Myna.Unity.Themes.Editor
{
	[CustomEditor(typeof(Theme))]
	public class ThemeInspector : UnityEditor.Editor
	{
		private ArrayPropertySortButton _sortButton;

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			var styles = serializedObject.FindProperty(Theme.StylesPropertyName);
			EditorGUILayout.PropertyField(styles, true);

			_sortButton.DrawLayout();

			serializedObject.ApplyModifiedProperties();
		}

		protected virtual void OnEnable()
		{
			var styles = serializedObject.FindProperty(Theme.StylesPropertyName);
			_sortButton = new ArrayPropertySortButton(styles);
		}
	}
}