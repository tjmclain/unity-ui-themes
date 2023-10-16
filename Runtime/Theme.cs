using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Myna.Unity.Themes
{
	[CreateAssetMenu(fileName = "Theme", menuName = "UI Themes/Theme")]
	public class Theme : ScriptableObject
	{
		public const string DefaultClassName = ".";
		public const string StylesPropertyName = nameof(_styles);

		private readonly Dictionary<string, ThemeStyle> _themeStyles = new();

		[SerializeField]
		private ColorScheme _defaultColorScheme = null;

		[SerializeField]
		private ColorScheme[] _colorSchemes = new ColorScheme[0];

		[SerializeField, HideInInspector, SingleLineProperty(StyleInfo.ClassPropertyName, StyleInfo.StylePropertyName)]
		private StyleInfo[] _styles = new StyleInfo[0];

		private ColorScheme _activeColorScheme = null;

		public ColorScheme DefaultColorScheme
		{
			get => _defaultColorScheme;
			set => _defaultColorScheme = value;
		}

		public ColorScheme ActiveColorScheme => _activeColorScheme != null ? _activeColorScheme : _defaultColorScheme;

		public ColorScheme[] ColorSchemes => _colorSchemes;

		public Dictionary<string, ThemeStyle> ThemeStyles => _themeStyles;

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
			// Reset to default color scheme
			if (_defaultColorScheme != null && colorSchemeName == _defaultColorScheme.name)
			{
				_activeColorScheme = _defaultColorScheme;
				return;
			}

			int index = Array.FindIndex(_colorSchemes, x => x.name == colorSchemeName);
			if (index < 0)
			{
				Debug.LogError($"{nameof(ColorSchemes)} does not contain '{colorSchemeName}'", this);
				return;
			}

			_activeColorScheme = _colorSchemes[index];
		}

		public void ResetColorSchemeToDefault()
		{
			_activeColorScheme = _defaultColorScheme;
		}

		public bool TryGetStyle(string className, out Style style)
		{
			if (string.IsNullOrEmpty(className))
			{
				style = default;
				return false;
			}

			if (TryGetThemeStyle(className, out var themeStyle))
			{
				style = themeStyle;
				return true;
			}

			// try to the find the style with our exact class name
			int index = Array.FindIndex(_styles, x => x.Class == className);
			if (index < 0)
			{
				Debug.LogWarning($"No {nameof(Style)} for {nameof(className)} '{className}'", this);
				style = default;
				return false;
			}

			var info = _styles[index];
			style = info.Style;
			return true;
		}

		public bool TryGetStyleByGuid(string guid, out Style style)
		{
			if (string.IsNullOrEmpty(guid))
			{
				style = default;
				return false;
			}

			int index = Array.FindIndex(_styles, x => x.Guid == guid);
			if (index < 0)
			{
				Debug.LogWarning($"No {nameof(Style)} for {nameof(guid)} '{guid}'", this);
				style = default;
				return false;
			}

			var info = _styles[index];
			style = TryGetThemeStyle(info.Class, out var themeStyle) ? themeStyle : info.Style;
			return true;
		}

		public bool TryGetStyleClassName(string guid, out string className)
		{
			int index = Array.FindIndex(_styles, x => x.Guid == guid);
			if (index < 0)
			{
				Debug.LogWarning($"No {nameof(Style)} for {nameof(guid)} '{guid}'", this);
				className = string.Empty;
				return false;
			}

			var info = _styles[index];
			className = info.Class;
			return true;
		}

		public bool TryGetStyleGuid(string className, out string guid)
		{
			int index = Array.FindIndex(_styles, x => x.Class == className);
			if (index < 0)
			{
				Debug.LogWarning($"No {nameof(Style)} for {nameof(className)} '{className}'", this);
				guid = string.Empty;
				return false;
			}

			var info = _styles[index];
			guid = info.Guid;
			return true;
		}

		public bool TryGetColor(string colorName, out Color color)
		{
			if (string.IsNullOrEmpty(colorName))
			{
				color = default;
				return false;
			}

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
			if (string.IsNullOrEmpty(guid))
			{
				color = default;
				return false;
			}

			var colorScheme = ActiveColorScheme;
			if (colorScheme == null)
			{
				Debug.LogError($"{nameof(ActiveColorScheme)} == null", this);
				color = default;
				return false;
			}

			return colorScheme.TryGetColorByGuid(guid, out color);
		}

		protected virtual bool TryGetThemeStyle(string className, out ThemeStyle themeStyle)
		{
			if (string.IsNullOrEmpty(className))
			{
				Debug.LogError($"{nameof(TryGetThemeStyle)}: string.IsNullOrEmpty({nameof(className)})", this);
				themeStyle = default;
				return false;
			}

			if (Application.isPlaying)
			{
				if (_themeStyles.TryGetValue(className, out themeStyle))
				{
					return true;
				}
			}

			themeStyle = CreateThemeStyle(className);
			return true;
		}

		protected virtual ThemeStyle CreateThemeStyle(string className)
		{
			var themeStyle = CreateInstance<ThemeStyle>();
			themeStyle.ClassName = className;

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

			foreach (var key in classNames)
			{
				int index = Array.FindIndex(_styles, x => x.Class == key);
				if (index < 0)
				{
					continue;
				}

				var style = _styles[index].Style;
				if (style == null)
				{
					Debug.LogError($"{nameof(Style)} of class '{key}' is null", this);
					continue;
				}

				themeStyle.CopyPropertiesFrom(style);
			}

			_themeStyles[className] = themeStyle;
			return themeStyle;
		}

		[Serializable]
		public class StyleInfo : ISerializationCallbackReceiver
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

			public void OnBeforeSerialize()
			{
			}

			public void OnAfterDeserialize()
			{
				if (string.IsNullOrEmpty(_guid))
				{
					_guid = System.Guid.NewGuid().ToString();
				}
			}
		}

		public class ThemeStyle : Style
		{
			private string _className;
			private readonly Dictionary<string, IStyleProperty> _propertyDict = new();

			public string ClassName
			{
				get => _className;
				set => _className = value;
			}

			public override T GetPropertyValue<T>(string propertyName, Theme theme, T defaultValue)
			{
				return TryGetProperty(propertyName, out var property)
					? (T)property.GetValue(theme)
					: defaultValue;
			}

			public override bool TryGetProperty(string propertyName, out IStyleProperty property)
			{
				return _propertyDict.TryGetValue(propertyName, out property);
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