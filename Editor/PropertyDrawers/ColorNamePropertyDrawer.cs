using System;
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
			var colorNameAttribute = attribute as ColorNameAttribute;
			var colorScheme = GetColorScheme(property, colorNameAttribute.ColorSchemePropertyName);

			if (colorScheme == null)
			{
				Debug.LogError($"{nameof(colorScheme)} == null", property.serializedObject.targetObject);
				EditorGUI.PropertyField(position, property, label);
				return;
			}

			var colors = colorScheme.Colors.ToArray();
			var options = new List<GUIContent>() { new GUIContent("[none]") };
			options.AddRange(colors.Select(x => new GUIContent()
			{
				text = $" {x.Name}",
				image = GetSwatch(x.Color)
			}));

			string value = property.stringValue;
			int index = Array.FindIndex(colors, x => x.Name == value || x.Guid == value) + 1;
			index = Math.Max(index, 0);

			index = EditorGUI.Popup(position, label, index, options.ToArray());

			property.stringValue = index > 0 ? colors[index - 1].Guid : string.Empty;
		}

		private static ColorScheme GetColorScheme(SerializedProperty property, string colorSchemePropertyName)
		{
			var colorSchemeProperty = property.GetSiblingProperty(colorSchemePropertyName);
			if (colorSchemeProperty == null)
			{
				return GetDefaultColorScheme();
			}

			var colorScheme = colorSchemeProperty.objectReferenceValue as ColorScheme;
			return colorScheme != null ? colorScheme : GetDefaultColorScheme();
		}

		private static ColorScheme GetDefaultColorScheme()
		{
			return ProjectSettings.GetInstance().GetDefaultColorScheme();
		}

		private Texture2D GetSwatch(Color color)
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