using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Myna.Unity.Themes.Editor
{
	[CustomPropertyDrawer(typeof(ColorNameAttribute))]
	public class ColorNamePropertyDrawer : PropertyDrawer
	{
		private static readonly Dictionary<int, Texture2D> _swatches = new();

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			static bool TryGetColors(out ColorInfo[] colors)
			{
				var theme = ProjectSettings.GetInstance().GetDefaultTheme();
				var colorScheme = theme.DefaultColorScheme;
				if (colorScheme == null)
				{
					colors = default;
					return false;
				}

				colors = theme.DefaultColorScheme.Colors.ToArray();
				return colors.Length > 0;
			}

			static Texture2D GetSwatch(Color color)
			{
				if (_swatches.TryGetValue(color.GetHashCode(), out var swatch))
				{
					return swatch;
				}

				swatch = new Texture2D(24, 12);
				for (int x = 0; x < swatch.width; x++)
				{
					for (int y = 0; y < swatch.height; y++)
					{
						swatch.SetPixel(x, y, color);
					}
				}
				swatch.Apply();

				_swatches[color.GetHashCode()] = swatch;
				return swatch;
			}

			if (!TryGetColors(out var colors))
			{
				EditorGUI.PropertyField(position, property, label);
				return;
			}

			var options = colors.Select(x => new GUIContent()
			{
				text = $" {x.Name}",
				image = GetSwatch(x.Color)
			}).ToArray();

			string value = property.stringValue;
			int index = System.Array.FindIndex(colors, x => x.Name == value || x.Guid == value);
			index = Mathf.Max(index, 0);

			index = EditorGUI.Popup(position, label, index, options);

			property.stringValue = colors[index].Guid;
		}
	}
}