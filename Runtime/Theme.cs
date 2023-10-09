using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Myna.Unity.Themes
{
	[CreateAssetMenu(fileName = "Theme", menuName = "UI Themes/Theme")]
	public class Theme : ScriptableObject
	{
		[SerializeField]
		private ColorScheme _defaultColorScheme = null;

		[SerializeField]
		private List<Style> _styles = new();

		public ColorScheme DefaultColorScheme => _defaultColorScheme;
		public List<Style> Styles => _styles;

		public IEnumerable<string> ColorNames => _defaultColorScheme != null
			? _defaultColorScheme.ColorNames
			: Enumerable.Empty<string>();

		public IEnumerable<string> GetStyleClassNames(Type componentType)
		{
			return _styles.Where(x => x.ComponentType == componentType)
				.Select(x => x.ClassName);
		}

		public bool TryGetStyle(Type componentType, out Style style)
			=> TryGetStyle(componentType, string.Empty, out style);

		public bool TryGetStyle(Type componentType, string className, out Style style)
		{
			if (componentType == null)
			{
				Debug.LogError($"{nameof(componentType)} == null", this);
				style = default;
				return false;
			}

			var styles = Styles.Where(x => x.ComponentType == componentType).ToArray();
			if (styles.Length == 0)
			{
				Debug.LogError($"Could not any styles for component type '{componentType.Name}'", this);
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

			Debug.LogError($"Could not find class '{className}' or default style for component type '{componentType.Name}'", this);
			style = default;
			return false;
		}

		public bool TryGetStyle<T>(Type componentType, out T style) where T : Style
			=> TryGetStyle(componentType, string.Empty, out style);

		public bool TryGetStyle<T>(Type componentType, string className, out T style) where T : Style
		{
			if (!TryGetStyle(componentType, className, out Style untypedStyle))
			{
				style = default;
				return false;
			}

			if (untypedStyle is not T typedStyle)
			{
				Debug.LogError($"{untypedStyle.Id} is not {typeof(T).Name}");
				style = default;
				return false;
			}

			style = typedStyle;
			return true;
		}

		public bool TryGetColor(string colorName, out Color color)
		{
			if (_defaultColorScheme == null)
			{
				Debug.LogError($"{nameof(_defaultColorScheme)} == null");
				color = default;
				return false;
			}

			return _defaultColorScheme.TryGetColor(colorName, out color);
		}

		public bool TryGetColorByGuid(string guid, out Color color)
		{
			if (_defaultColorScheme == null)
			{
				Debug.LogError($"{nameof(_defaultColorScheme)} == null");
				color = default;
				return false;
			}

			return _defaultColorScheme.TryGetColorByGuid(guid, out color);
		}
	}
}