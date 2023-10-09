using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace Myna.Unity.Themes.Editor
{
	public class ProjectSettingsProvider : SettingsProvider
	{
		private ProjectSettings _projectSettings = null;

		public ProjectSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords)
		{
		}

		[SettingsProvider]
		public static SettingsProvider CreateSettingsProvider()
		{
			var provider = new ProjectSettingsProvider("Project/UI Themes", SettingsScope.Project)
			{
				// Automatically extract all keywords from the Styles.
				keywords = GetSearchKeywordsFromGUIContentProperties<Styles>()
			};
			return provider;
		}

		public override void OnActivate(string searchContext, VisualElement rootElement)
		{
			_projectSettings = ProjectSettings.GetInstance();
			_projectSettings.GetDefaultTheme();
		}

		public override void OnGUI(string searchContext)
		{
			using var serializedObject = new SerializedObject(_projectSettings);

			var defaultTheme = serializedObject.FindProperty("_defaultTheme");
			EditorGUILayout.PropertyField(defaultTheme, Styles.DefaultTheme);

			serializedObject.ApplyModifiedProperties();
		}

		private class Styles
		{
			public static GUIContent DefaultTheme = new GUIContent("Default Theme");
		}
	}
}