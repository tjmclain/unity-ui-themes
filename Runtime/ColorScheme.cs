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
		public class ColorInfo : ISingleLineProperty
		{
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

		[SerializeField]
		private ColorInfo[] _colors = new ColorInfo[0];

		public IEnumerable<ColorInfo> Colors => _colors;
		public IEnumerable<string> ColorNames => _colors.Select(x => x.Name);

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
	}
}