using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Myna.Unity.Themes.Editor
{
	[CustomPropertyDrawer(typeof(ColorNameDropdownAttribute))]
	public class ColorNameDropdownPropertyDrawer : PropertyDrawer
	{
		private static readonly Dictionary<int, Texture2D> _swatches = new();

		private bool IsSerializedColorName => typeof(SerializedColorName).IsAssignableFrom(fieldInfo.FieldType);

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var attribute = this.attribute as ColorNameDropdownAttribute;
			var colorScheme = GetDefaultColorScheme();

			// TODO draw color field only
			if (colorScheme == null)
			{
				Debug.LogWarning($"{nameof(colorScheme)} == null", property.serializedObject.targetObject);
				EditorGUI.PropertyField(position, property, label);
				return;
			}

			string defaultColorPropertyName = attribute.DefaultColorPropertyName;
			string defaultColorOption = GetDefaultColorOptionName(property, defaultColorPropertyName);

			var options = new List<GUIContent>() { new GUIContent(defaultColorOption) };

			var colors = colorScheme.Colors.ToArray();
			options.AddRange(colors.Select(x => new GUIContent()
			{
				text = $" {x.Name}",
				image = GetSwatch(x.Color)
			}));

			string colorName = GetColorName(property, colorScheme);
			int index = Array.FindIndex(colors, x => x.Name == colorName) + 1;
			index = Math.Max(index, 0);

			index = EditorGUI.Popup(position, label, index, options.ToArray());

			colorName = index > 0 ? colors[index - 1].Name : string.Empty;
			SetColorName(property, colorScheme, colorName);
		}

		private string GetColorName(SerializedProperty property, ColorScheme colorScheme)
		{
			if (!IsSerializedColorName)
			{
				return property.stringValue;
			}

			var guid = property.FindPropertyRelative(SerializedColorName.GuidPropertyName);
			string colorName = colorScheme.ColorGuidToName(guid.stringValue);

			var name = property.FindPropertyRelative(SerializedColorName.NamePropertyName);
			name.stringValue = colorName;
			return colorName;
		}

		private void SetColorName(SerializedProperty property, ColorScheme colorScheme, string colorName)
		{
			if (!IsSerializedColorName)
			{
				property.stringValue = colorName;
				return;
			}

			var name = property.FindPropertyRelative(SerializedColorName.NamePropertyName);
			name.stringValue = colorName;

			var guid = property.FindPropertyRelative(SerializedColorName.GuidPropertyName);
			guid.stringValue = colorScheme.ColorNameToGuid(colorName);
		}

		private static ColorScheme GetDefaultColorScheme()
		{
			var theme = Theme.Instance;
			return theme != null ? theme.DefaultColorScheme : null;
		}

		private static string GetDefaultColorOptionName(SerializedProperty property, string defaultColorPropertyName)
		{
			const string defaultName = "Default";

			if (string.IsNullOrEmpty(defaultColorPropertyName))
			{
				return defaultName;
			}

			var defaultColorProperty = property.GetSiblingProperty(defaultColorPropertyName);
			if (defaultColorProperty == null)
			{
				return defaultName;
			}

			return $"{defaultName} ({defaultColorProperty.displayName})";
		}

		private static Texture2D GetSwatch(Color color)
		{
			if (_swatches.TryGetValue(color.GetHashCode(), out var swatch) && swatch != null)
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
	}
}