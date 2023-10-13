using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Myna.Unity.Themes
{
	[CreateAssetMenu(fileName = "Theme", menuName = "UI Themes/Theme")]
	public class Theme : ScriptableObject
	{
		[SerializeField]
		private ColorScheme _defaultColorScheme = null;

		[SerializeField]
		private ColorScheme[] _colorSchemes = new ColorScheme[0];

		[SerializeField]
		private Style[] _styles = new Style[0];

		private ColorScheme _activeColorScheme = null;

		public static string StylesPropertyName => nameof(_styles);

		public ColorScheme DefaultColorScheme => _defaultColorScheme;
		public ColorScheme ActiveColorScheme => _activeColorScheme != null ? _activeColorScheme : _defaultColorScheme;
		public ColorScheme[] ColorSchemes => _colorSchemes;
		public Style[] Styles => _styles;

		public IEnumerable<string> ColorNames => _defaultColorScheme != null
			? _defaultColorScheme.ColorNames
			: Enumerable.Empty<string>();

		public IEnumerable<string> GetStyleClassNames(Type styleType)
		{
			return _styles.Where(x => styleType.IsAssignableFrom(x.GetType()))
				.Select(x => x.ClassName);
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

		public bool TryGetStyle(Type componentType, out Style style)
			=> TryGetStyle(componentType, string.Empty, out style);

		public bool TryGetStyle(Type styleType, string className, out Style style)
		{
			if (styleType == null)
			{
				Debug.LogError($"{nameof(styleType)} == null", this);
				style = default;
				return false;
			}

			var styles = Styles.Where(x => styleType.IsAssignableFrom(x.GetType())).ToArray();
			if (styles.Length == 0)
			{
				Debug.LogError($"Could not any styles for component type '{styleType.Name}'", this);
				style = default;
				return false;
			}

			// try to the find the style with our exact class name
			int index = Array.FindIndex(styles, x => x.ClassName == className);
			if (index >= 0)
			{
				style = styles[index];
				return true;
			}

			// otherwise, try to find the default style (no class name)
			index = Array.FindIndex(styles, x => x.ClassName == string.Empty);
			if (index >= 0)
			{
				style = styles[index];
				return true;
			}

			Debug.LogError($"Could not find class '{className}' or default style for component type '{styleType.Name}'", this);
			style = default;
			return false;
		}

		public bool TryGetStyle<T>(out T style) where T : Style
			=> TryGetStyle(string.Empty, out style);

		public bool TryGetStyle<T>(string className, out T style) where T : Style
		{
			if (!TryGetStyle(typeof(T), className, out Style value))
			{
				style = default;
				return false;
			}

			style = value as T;
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