using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

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

		public bool TryGetStyle(System.Type componentType, out Style style)
			=> TryGetStyle(componentType, string.Empty, out style);

		public bool TryGetStyle(System.Type componentType, string className, out Style style)
		{
			if (componentType == null)
			{
				Debug.LogError($"{nameof(componentType)} == null", this);
				style = default;
				return false;
			}

			int index = Styles.FindIndex(x => x.ComponentType == componentType && x.ClassName == className);
			if (index < 0)
			{
				style = default;
				return false;
			}

			style = Styles[index];
			return true;
		}

		public bool TryGetStyle<T>(System.Type componentType, out T style) where T : Style
			=> TryGetStyle(componentType, string.Empty, out style);

		public bool TryGetStyle<T>(System.Type componentType, string className, out T style) where T : Style
		{
			if (!TryGetStyle(componentType, className, out Style untypedStyle))
			{
				style = default;
				return false;
			}

			if (untypedStyle is not T typedStyle)
			{
				Debug.LogError($"{componentType.Name}{(string.IsNullOrEmpty(className) ? string.Empty : $".{className}")} is not {typeof(T).Name}");
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