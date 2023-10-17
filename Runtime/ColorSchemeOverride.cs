using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Myna.Unity.Themes
{
	[CreateAssetMenu(fileName = "ColorSchemeOverride", menuName = "UI Themes/Color Scheme Override")]
	public class ColorSchemeOverride : ColorScheme
	{
		public const string ReferenceColorSchemePropertyName = nameof(_referenceColorScheme);

		[SerializeField, ColorSchemeReference]
		private ColorScheme _referenceColorScheme;

		public override bool TryGetColor(string colorName, out Color color)
		{
			if (Colors.TryGetValue(x => x.Name == colorName, out var info))
			{
				color = info.Color;
				return true;
			}

			return _referenceColorScheme.TryGetColor(colorName, out color);
		}

		public override bool TryGetColorByGuid(string guid, out Color color)
		{
			if (Colors.TryGetValue(x => x.Guid == guid, out var info))
			{
				color = info.Color;
				return true;
			}

			return _referenceColorScheme.TryGetColorByGuid(guid, out color);
		}

		public override string ColorGuidToName(string colorGuid)
		{
			return _referenceColorScheme.ColorGuidToName(colorGuid);
		}

		public override string ColorNameToGuid(string colorName)
		{
			return _referenceColorScheme.ColorNameToGuid(colorName);
		}

		private void OnValidate()
		{
			if (_referenceColorScheme == null)
			{
				return;
			}

			// map all of the colors from the reference color scheme
			var colors = _referenceColorScheme.Colors;
			var overrideColors = new List<ColorInfo>(Colors);

			for (int i = 0; i < overrideColors.Count; i++)
			{
				var overrideColor = overrideColors[i];
				if (Array.Exists(colors, x => x.Guid == overrideColor.Guid))
				{
					continue;
				}

				overrideColors.RemoveAt(i);
				i--;
			}

			Colors = overrideColors.ToArray();
		}
	}
}