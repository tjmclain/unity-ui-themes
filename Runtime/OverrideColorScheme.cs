using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Myna.Unity.Themes
{
	public class OverrideColorScheme : ColorScheme
	{
		[SerializeField]
		private ColorScheme _colorScheme;

		[SerializeField]
		private List<OverrideColorInfo> _overrideColors = new();

		public override IEnumerable<ColorInfo> Colors => _overrideColors;

		public override bool TryGetColor(string colorName, out Color color)
		{
			if (string.IsNullOrEmpty(colorName))
			{
				Debug.LogError($"{nameof(colorName)} is null or empty");
				color = default;
				return false;
			}

			int index = _overrideColors.FindIndex(x => x.Name == colorName);
			if (index <= 0)
			{
				color = default;
				return false;
			}

			var colorInfo = _overrideColors[index];
			if (colorInfo.Enabled)
			{
				color = colorInfo.Color;
				return true;
			}
			else
			{
				color = default;
				return _colorScheme != null && _colorScheme.TryGetColor(colorName, out color);
			}
		}

		public override bool TryGetColorByGuid(string guid, out Color color)
		{
			if (string.IsNullOrEmpty(guid))
			{
				Debug.LogError($"{nameof(guid)} is null or empty");
				color = default;
				return false;
			}

			int index = _overrideColors.FindIndex(x => x.Guid == guid);
			if (index <= 0)
			{
				color = default;
				return false;
			}

			var colorInfo = _overrideColors[index];
			if (colorInfo.Enabled)
			{
				color = colorInfo.Color;
				return true;
			}
			else
			{
				color = default;
				return _colorScheme != null && _colorScheme.TryGetColorByGuid(guid, out color);
			}
		}

		private void OnValidate()
		{
			if (_colorScheme == null)
			{
				return;
			}

			// map all of the colors from the reference color scheme
			var colors = _colorScheme.Colors;
			foreach (var color in colors)
			{
				if (_overrideColors.Exists(x => x.Guid == color.Guid))
				{
					continue;
				}

				var overrideColor = new OverrideColorInfo(color);
				_overrideColors.Add(overrideColor);
			}

			for (int i = 0; i < _overrideColors.Count; i++)
			{
				var overrideColor = _overrideColors[i];
				if (colors.Any(x => x.Guid == overrideColor.Guid))
				{
					continue;
				}

				_overrideColors.RemoveAt(i);
				i--;
			}
		}

		public class OverrideColorInfo : ColorInfo
		{
			[SerializeField]
			private bool _enabled;

			public OverrideColorInfo(ColorInfo original)
			{
				_name = original.Name;
				_color = original.Color;
				_guid = original.Guid;
				_enabled = false;
			}

			public bool Enabled => _enabled;
		}
	}
}