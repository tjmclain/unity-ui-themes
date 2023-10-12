using System.Collections;
using System.Collections.Generic;
using Myna.Unity.Themes;
using UnityEngine;

[System.Serializable]
public class ColorProperty : StyleProperty<Color>
{
	[SerializeField, ColorName]
	private string _colorName = string.Empty;

	[SerializeField]
	private Color _fallbackColor = Color.white;

	public string ColorName => _colorName;
	public Color FallbackColor => _fallbackColor;

	public Color GetColor(Theme theme)
	{
		return theme.TryGetColorByGuid(_colorName, out var color) || theme.TryGetColor(_colorName, out color)
			? color : _fallbackColor;
	}
}