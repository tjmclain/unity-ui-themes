using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Myna.Unity.Themes
{
	[CreateAssetMenu(fileName = "ColorScheme", menuName = "UI Themes/Color Scheme")]
	public class ColorScheme : ScriptableObject
	{
		[Serializable]
		public class ColorInfo
		{
			public const string NamePropertyName = nameof(_name);
			public const string ColorPropertyName = nameof(_color);

			[SerializeField]
			private string _name = "Color";

			[SerializeField]
			private Color _color = Color.white;

			[SerializeField, HideInInspector]
			private string _guid = System.Guid.NewGuid().ToString();

			public string Name => _name;
			public Color Color => _color;
			public string Guid => _guid;
		}

		public const string ColorsPropertyName = nameof(_colors);

		[SerializeField, HideInInspector, SingleLineProperty(ColorInfo.NamePropertyName, ColorInfo.ColorPropertyName)]
		private ColorInfo[] _colors = new ColorInfo[0];

		public IEnumerable<ColorInfo> Colors => _colors;

		public IEnumerable<string> GetColorNames()
		{
			return _colors.Select(x => x.Name);
		}

		public bool TryGetColor(string colorName, out Color color)
		{
			if (string.IsNullOrEmpty(colorName))
			{
				Debug.LogError($"{nameof(colorName)} is null or empty");
				color = default;
				return false;
			}

			int index = Array.FindIndex(_colors, x => x.Name == colorName);
			if (index < 0)
			{
				color = default;
				return false;
			}

			color = _colors[index].Color;
			return true;
		}

		public bool TryGetColorByGuid(string guid, out Color color)
		{
			if (string.IsNullOrEmpty(guid))
			{
				Debug.LogError($"{nameof(guid)} is null or empty");
				color = default;
				return false;
			}

			int index = Array.FindIndex(_colors, x => x.Guid == guid);
			if (index < 0)
			{
				color = default;
				return false;
			}

			color = _colors[index].Color;
			return true;
		}

		public bool TryGetColorName(string guid, out string colorName)
		{
			int index = Array.FindIndex(_colors, x => x.Guid == guid);
			if (index < 0)
			{
				Debug.LogError($"No {nameof(ColorInfo)} for {nameof(guid)} '{guid}'", this);
				colorName = string.Empty;
				return false;
			}

			var info = _colors[index];
			colorName = info.Name;
			return true;
		}

		public bool TryGetColorGuid(string colorName, out string guid)
		{
			int index = Array.FindIndex(_colors, x => x.Name == colorName);
			if (index < 0)
			{
				Debug.LogError($"No {nameof(ColorInfo)} for {nameof(colorName)} '{colorName}'", this);
				guid = string.Empty;
				return false;
			}

			var info = _colors[index];
			guid = info.Guid;
			return true;
		}
	}
}