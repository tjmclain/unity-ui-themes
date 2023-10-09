using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Myna.Unity.Themes.Editor
{
	[CustomEditor(typeof(StyleHelper), true)]
	public class StyleHelperInspector : UnityEditor.Editor
	{
		protected virtual void OnEnable()
		{
			var theme = serializedObject.FindProperty("_theme");
			if (theme.objectReferenceValue == null)
			{
				theme.objectReferenceValue = ProjectSettings.GetInstance().GetDefaultTheme();
				serializedObject.ApplyModifiedPropertiesWithoutUndo();
			}
		}
	}
}