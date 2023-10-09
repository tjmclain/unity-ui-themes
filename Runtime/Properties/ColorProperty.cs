using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorProperty : StyleProperty
{
	[SerializeField, HideInInspector]
	private string _colorName = string.Empty;

	[SerializeField]
	private Color _fallbackColor = Color.white;

	public string ColorName => _colorName;
	public Color FallbackColor => _fallbackColor;
}