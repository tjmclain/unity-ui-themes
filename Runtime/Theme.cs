using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Myna.Unity.Themes
{
	[CreateAssetMenu(fileName = "Theme", menuName = "UI Themes/Theme")]
	public class Theme : ScriptableObject
	{
		[Serializable]
		public class StyleInfo
		{
			public const string ClassPropertyName = nameof(_class);
			public const string StylePropertyName = nameof(_style);

			[SerializeField, ClassName]
			private string _class = Theme.DefaultClassName;

			[SerializeField]
			private Style _style;

			[SerializeField, HideInInspector]
			private string _guid = System.Guid.NewGuid().ToString();

			public string Class => _class;
			public Style Style => _style;
			public string Guid => _guid;
		}

		public const string DefaultClassName = ".";

		[SerializeField]
		private ColorScheme _defaultColorScheme = null;

		[SerializeField]
		private ColorScheme[] _colorSchemes = new ColorScheme[0];

		[SerializeField, HideInInspector, SingleLineProperty(StyleInfo.ClassPropertyName, StyleInfo.StylePropertyName)]
		private StyleInfo[] _styles = new StyleInfo[0];

		private ColorScheme _activeColorScheme = null;

		public static string StylesPropertyName => nameof(_styles);

		public ColorScheme DefaultColorScheme
		{
			get => _defaultColorScheme;
			set => _defaultColorScheme = value;
		}

		public ColorScheme ActiveColorScheme => _activeColorScheme != null ? _activeColorScheme : _defaultColorScheme;
		public ColorScheme[] ColorSchemes => _colorSchemes;

		public IEnumerable<string> ColorNames => _defaultColorScheme != null
			? _defaultColorScheme.ColorNames
			: Enumerable.Empty<string>();

		public IEnumerable<string> GetStyleClassNames()
		{
			return _styles.Select(x => x.Class);
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
			// try to the find the style with our exact class name
			int index = Array.FindIndex(_styles, x => x.Class == className);
			if (index < 0)
			{
				Debug.LogError($"Could not find style for class '{className}'", this);
				style = default;
				return false;
			}

			var info = _styles[index];
			style = info.Style;
			return true;
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
	}
}