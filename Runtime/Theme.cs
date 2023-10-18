using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Myna.Unity.Themes
{
	[CreateAssetMenu(fileName = "Theme", menuName = "UI Themes/Theme")]
	public class Theme : ScriptableObject
	{
		public const string DefaultClassName = ".";
		public const string StylesPropertyName = nameof(_styles);

		public static readonly UnityEvent<Theme> SettingsChanged = new();

		private const string _singletonResourcesPath = "UI Themes/Theme";
		private static Theme _instance = null;

		private readonly Dictionary<string, RuntimeStyle> _runtimeStyles = new();

		[SerializeField]
		private ColorScheme _defaultColorScheme = null;

		[SerializeField]
		private ColorScheme[] _colorSchemes = new ColorScheme[0];

		[SerializeField, HideInInspector, SingleLineProperty(StyleInfo.ClassPropertyName, StyleInfo.StylePropertyName)]
		private StyleInfo[] _styles = new StyleInfo[0];

		private ColorScheme _activeColorScheme = null;

		public static Theme Instance => GetSingletonInstance();
		public static bool IsInitialized => _instance != null;

		public ColorScheme DefaultColorScheme
		{
			get => _defaultColorScheme;
			set => _defaultColorScheme = value;
		}

		public ColorScheme ActiveColorScheme => _activeColorScheme != null ? _activeColorScheme : _defaultColorScheme;

		public ColorScheme[] ColorSchemes => _colorSchemes;

		public Dictionary<string, RuntimeStyle> RuntimeStyles => _runtimeStyles;

		public IEnumerable<string> GetStyleClassNames()
		{
			return _styles.Select(x => x.Class);
		}

		public IEnumerable<string> GetColorNames()
		{
			return _defaultColorScheme != null
				? _defaultColorScheme.GetColorNames()
				: Enumerable.Empty<string>();
		}

		public void SetColorScheme(string colorSchemeName)
		{
			const string colorScheme = "ColorScheme";
			const string colors = "Colors";

			bool Match(ColorScheme cs)
			{
				if (cs == null)
				{
					Debug.LogWarning($"{nameof(SetColorScheme)}: {nameof(ColorScheme)} == null", this);
					return false;
				}

				string name = cs.name;
				if (name == colorSchemeName)
				{
					return true;
				}

				if (name.EndsWith(colorScheme))
				{
					return name[..^colorScheme.Length] == colorSchemeName;
				}

				if (name.EndsWith(colors))
				{
					return name[..^colors.Length] == colorSchemeName;
				}

				return false;
			}

			// Reset to default color scheme
			if (Match(_defaultColorScheme))
			{
				_activeColorScheme = _defaultColorScheme;
				SettingsChanged.Invoke(this);
				return;
			}

			int index = Array.FindIndex(_colorSchemes, x => x.name == colorSchemeName);
			if (index < 0)
			{
				Debug.LogError($"{nameof(ColorSchemes)} does not contain '{colorSchemeName}'", this);
				return;
			}

			_activeColorScheme = _colorSchemes[index];
			SettingsChanged.Invoke(this);
		}

		public void ResetColorSchemeToDefault()
		{
			_activeColorScheme = _defaultColorScheme;
		}

		public bool TryGetStyle(string styleClass, out Style style)
		{
			if (TryGetRuntimeStyle(styleClass, out var themeStyle))
			{
				style = themeStyle;
				return true;
			}

			if (!_styles.TryGetValue(x => x.Class == styleClass, out var info))
			{
				Debug.LogWarning($"No {nameof(Style)} for {nameof(styleClass)} '{styleClass}'", this);
				style = default;
				return false;
			}

			style = info.Style;
			return true;
		}

		public bool TryGetStyleByGuid(string styleGuid, out Style style)
		{
			if (!_styles.TryGetValue(x => x.Guid == styleGuid, out var info))
			{
				Debug.LogWarning($"No {nameof(Style)} for {nameof(styleGuid)} '{styleGuid}'", this);
				style = default;
				return false;
			}

			style = info.Style;
			return true;
		}

		public string StyleGuidToClassName(string styleGuid)
		{
			return _styles.TryGetValue(x => x.Guid == styleGuid, out var info) ? info.Class : DefaultClassName;
		}

		public string StyleClassToGuid(string styleClass)
		{
			return _styles.TryGetValue(x => x.Class == styleClass, out var info) ? info.Guid : string.Empty;
		}

		public bool TryGetColor(string colorName, out Color color)
		{
			var colorScheme = ActiveColorScheme;
			if (colorScheme == null)
			{
				Debug.LogError($"{nameof(ActiveColorScheme)} == null", this);
				color = default;
				return false;
			}

			return colorScheme.TryGetColor(colorName, out color);
		}

		public bool TryGetColorByGuid(string guid, out Color color)
		{
			var colorScheme = ActiveColorScheme;
			if (colorScheme == null)
			{
				Debug.LogError($"{nameof(ActiveColorScheme)} == null", this);
				color = default;
				return false;
			}

			return colorScheme.TryGetColorByGuid(guid, out color);
		}

		protected virtual bool TryGetRuntimeStyle(string className, out RuntimeStyle style)
		{
			if (string.IsNullOrEmpty(className))
			{
				Debug.LogError($"{nameof(TryGetRuntimeStyle)}: string.IsNullOrEmpty({nameof(className)})", this);
				style = default;
				return false;
			}

			if (_runtimeStyles.TryGetValue(className, out style))
			{
#if UNITY_EDITOR
				style.Initialize(_styles, className);
#endif
				return true;
			}

			style = CreateInstance<RuntimeStyle>();
			style.Initialize(_styles, className);
			_runtimeStyles[className] = style;
			return true;
		}

		private static Theme GetSingletonInstance()
		{
			if (_instance != null)
			{
				return _instance;
			}

			CreateSingletonAssetIfMissing();

			_instance = Resources.Load<Theme>(_singletonResourcesPath);
			return _instance;
		}

#if UNITY_EDITOR

		[UnityEditor.InitializeOnLoadMethod]
		[System.Diagnostics.Conditional("UNITY_EDITOR")]
		private static void CreateSingletonAssetIfMissing()
		{
			var instance = Resources.Load<Theme>(_singletonResourcesPath);
			if (instance != null)
			{
				return;
			}

			string assetPath = $"Assets/Resources/{_singletonResourcesPath}.asset";
			if (UnityEditor.AssetDatabase.DeleteAsset(assetPath))
			{
				UnityEditor.AssetDatabase.SaveAssets();
				UnityEditor.AssetDatabase.Refresh();
			}

			string directory = System.IO.Path.GetDirectoryName(assetPath);
			System.IO.Directory.CreateDirectory(directory);

			instance = CreateInstance<Theme>();
			UnityEditor.AssetDatabase.CreateAsset(instance, assetPath);
			UnityEditor.AssetDatabase.SaveAssets();
			UnityEditor.AssetDatabase.Refresh();
		}

#endif

		[Serializable]
		public class StyleInfo
		{
			public const string ClassPropertyName = nameof(_class);
			public const string StylePropertyName = nameof(_style);

			[SerializeField, ClassName]
			private string _class = DefaultClassName;

			[SerializeField]
			private Style _style;

			[SerializeField, HideInInspector]
			private string _guid = System.Guid.NewGuid().ToString();

			public string Class => _class;
			public Style Style => _style;
			public string Guid => _guid;
		}

		public class RuntimeStyle : Style
		{
			private readonly Dictionary<string, IStyleProperty> _propertyDict = new();

			public override T GetPropertyValue<T>(string propertyName, Theme theme, T defaultValue)
			{
				return TryGetProperty(propertyName, out var property)
					? (T)property.GetValue(theme) : defaultValue;
			}

			public override bool TryGetProperty(string propertyName, out IStyleProperty property)
			{
				return _propertyDict.TryGetValue(propertyName, out property);
			}

			public void Initialize(StyleInfo[] styles, string className)
			{
				_propertyDict.Clear();

				const char dot = '.';
				var classNames = new List<string>() { DefaultClassName };
				var sb = new StringBuilder();
				foreach (char c in className)
				{
					if (c == dot && sb.Length > 0)
					{
						classNames.Add(sb.ToString());
					}

					sb.Append(c);
				}

				if (sb.Length > 1)
				{
					classNames.Add(sb.ToString());
				}

				//Debug.Log($"Creating {nameof(ThemeStyle)} from classes: {string.Join(", ", classNames)}", this);

				foreach (var name in classNames)
				{
					if (!styles.TryGetValue(x => x.Class == name, out var info))
					{
						continue;
					}

					var style = info.Style;
					if (style == null)
					{
						Debug.LogError($"{nameof(Style)} of class '{name}' is null", this);
						continue;
					}

					CopyPropertiesFrom(style);
				}
			}

			public void CopyPropertiesFrom(Style other)
			{
				foreach (var property in other.Properties)
				{
					_propertyDict[property.Name] = property.Clone();
				}
			}
		}
	}
}