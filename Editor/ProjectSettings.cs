using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Myna.Unity.Themes.Editor
{
	public class ProjectSettings : ScriptableObject
	{
		private const string _assetsDirectory = "Assets/UI Themes";

		private static readonly string _path = $"{_assetsDirectory}/Editor/ThemesSettings.asset";
		private static readonly string _defaultThemePath = $"{_assetsDirectory}/DefaultTheme.asset";

		[SerializeField]
		private Theme _defaultTheme;

		public Theme DefaultTheme
		{
			get
			{
				return GetDefaultTheme();
			}
			set
			{
				using var serializedObject = new SerializedObject(this);
				var property = serializedObject.FindProperty(nameof(_defaultTheme));
				property.objectReferenceValue = value;
				serializedObject.ApplyModifiedPropertiesWithoutUndo();
			}
		}

		public static ProjectSettings GetInstance()
		{
			static bool TryLoadInstance(out ProjectSettings instance)
			{
				if (!File.Exists(_path))
				{
					instance = null;
					return false;
				}

				instance = AssetDatabase.LoadAssetAtPath<ProjectSettings>(_path);
				if (instance == null)
				{
					AssetDatabase.DeleteAsset(_path);
					AssetDatabase.SaveAssets();
					return false;
				}

				return true;
			}

			if (TryLoadInstance(out ProjectSettings instance))
			{
				return instance;
			}

			var dir = Path.GetDirectoryName(_path);
			if (!Directory.Exists(dir))
			{
				Directory.CreateDirectory(dir);
			}

			instance = CreateInstance<ProjectSettings>();

			AssetDatabase.CreateAsset(instance, _path);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			return instance;
		}

		public Theme GetDefaultTheme()
		{
			static bool TryFindThemeAsset(out Theme theme)
			{
				var assetGuids = AssetDatabase.FindAssets("t:Theme");
				Debug.Log($"found {assetGuids.Length} {nameof(Theme)}s in Assets");
				foreach (var guid in assetGuids)
				{
					string assetPath = AssetDatabase.GUIDToAssetPath(guid);
					theme = AssetDatabase.LoadAssetAtPath<Theme>(assetPath);
					if (theme != null)
					{
						Debug.Log($"Found default {nameof(Theme)} at {assetPath}");
						return true;
					}
				}

				theme = null;
				return false;
			}

			if (_defaultTheme != null)
			{
				return _defaultTheme;
			}

			if (TryFindThemeAsset(out Theme theme))
			{
				DefaultTheme = theme;
				return theme;
			}

			theme = CreateInstance<Theme>();

			var dir = Path.GetDirectoryName(_defaultThemePath);
			if (!Directory.Exists(dir))
			{
				Directory.CreateDirectory(dir);
			}

			Debug.Log($"Creating default {nameof(Theme)} at {_defaultThemePath}");

			AssetDatabase.CreateAsset(theme, _defaultThemePath);
			AssetDatabase.SaveAssets();

			DefaultTheme = theme;

			AssetDatabase.Refresh();
			return theme;
		}
	}
}