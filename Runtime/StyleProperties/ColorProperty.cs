using System;
using Myna.Unity.Themes;
using UnityEngine;

[Serializable]
public class ColorProperty : StyleProperty
{
	[SerializeField, ColorSchemeReference]
	private ColorScheme _referenceColorScheme;

	[SerializeField, ColorName(nameof(_referenceColorScheme))]
	private string _colorName = string.Empty;

	[SerializeField]
	private Color _fallbackColor = Color.white;

	public static string DefaultName => nameof(Color);

	public override Type ValueType => typeof(Color);
	public string ColorName => _colorName;
	public Color FallbackColor => _fallbackColor;

	public override object GetValue(Theme theme)
	{
		if (string.IsNullOrEmpty(ColorName))
		{
			return FallbackColor;
		}

		return theme.TryGetColorByGuid(ColorName, out var color)
			|| theme.TryGetColor(ColorName, out color)
			? color : FallbackColor;
	}
}