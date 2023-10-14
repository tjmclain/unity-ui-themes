using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static PlasticGui.PlasticTableCell;

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

		public ColorScheme DefaultColorScheme
		{
			get => _defaultColorScheme;
			set => _defaultColorScheme = value;
		}

		public ColorScheme ActiveColorScheme => _activeColorScheme != null ? _activeColorScheme : _defaultColorScheme;
		public ColorScheme[] ColorSchemes => _colorSchemes;
		public Style[] Styles => _styles;

		public IEnumerable<string> ColorNames => _defaultColorScheme != null
			? _defaultColorScheme.ColorNames
			: Enumerable.Empty<string>();

		public IEnumerable<string> GetStyleClassNames()
		{
			return _styles.Select(x => x.ClassName);
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
			int index = Array.FindIndex(_styles, x => x.ClassName == className);
			if (index < 0)
			{
				Debug.LogError($"Could not find style for class '{className}'", this);
				style = default;
				return false;
			}

			style = _styles[index];
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