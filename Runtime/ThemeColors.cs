using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Myna.Unity.Themes
{
	[System.Serializable]
	public class ThemeColor
	{
		[SerializeField]
		private string _name = "Color";

		[SerializeField]
		private Color _color = Color.white;

		public string Name => _name;
		public Color Color => _color;
	}

	[CreateAssetMenu(fileName = "ThemeColors", menuName = "UI Themes/ThemeColors")]
	public class ThemeColors : ScriptableObject
	{
		[SerializeField]
		private List<ThemeColor> _colors = new();

		public IEnumerable<ThemeColor> Colors => _colors;
		public IEnumerable<string> ColorNames => _colors.Select(x => x.Name);

		public bool TryGetColor(string colorName, out Color color)
		{
			if (string.IsNullOrEmpty(colorName))
			{
				Debug.LogWarning($"{nameof(colorName)} is null or empty");
				color = default;
				return false;
			}

			int index = _colors.FindIndex(x => x.Name == colorName);
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