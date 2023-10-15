using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Myna.Unity.Themes
{
	public class TextStyleHelper : StyleHelper
	{
		[Serializable]
		public class OverrideProperties
		{
			public OverrideProperty<Color> Color = new();
			public OverrideAlphaProperty Alpha = new();
			public OverrideProperty<float> FontSize = new();
			public OverrideProperty<bool> AutoSize = new();
			public OverrideProperty<float> FontSizeMin = new();
			public OverrideProperty<float> FontSizeMax = new();
			public OverrideProperty<FontStyles> FontStyle = new();
		}

		[SerializeField]
		private TMPro.TextMeshProUGUI _text;

		[SerializeField]
		private OverrideProperties _overrides = new();

		protected override void OnValidate()
		{
			if (_text == null)
			{
				TryGetComponent(out _text);
			}
		}

		protected override void ApplyStyle(Style style)
		{
			if (_text == null)
			{
				Debug.LogError($"{nameof(_text)} == null", this);
				return;
			}

			// Color
			var color = style.GetPropertyValue(ColorProperty.DefaultName, Theme, _text.color);
			color = _overrides.Color.OverrideOrDefaultValue(color);

			// Alpha
			color.a = style.GetPropertyValue(AlphaProperty.DefaultName, Theme, color.a);
			color.a = _overrides.Alpha.OverrideOrDefaultValue(color.a);
			_text.color = color;

			// FontSize
			var fontSize = style.GetPropertyValue(FloatProperty.FontSize, Theme, _text.fontSize);
			fontSize = _overrides.FontSize.OverrideOrDefaultValue(fontSize);
			_text.fontSize = fontSize;

			// AutoSize
			bool autoSize = style.GetPropertyValue(BoolProperty.AutoSize, Theme, _text.enableAutoSizing);
			autoSize = _overrides.AutoSize.OverrideOrDefaultValue(autoSize);
			_text.enableAutoSizing = autoSize;

			// FontSizeMin
			var minFontSize = style.GetPropertyValue(FloatProperty.FontSizeMin, Theme, _text.fontSizeMin);
			minFontSize = _overrides.FontSizeMin.OverrideOrDefaultValue(minFontSize);
			_text.fontSizeMin = minFontSize;

			// FontSizeMax
			var maxFontSize = style.GetPropertyValue(FloatProperty.FontSizeMax, Theme, _text.fontSizeMax);
			maxFontSize = _overrides.FontSizeMax.OverrideOrDefaultValue(maxFontSize);
			_text.fontSizeMax = maxFontSize;

			// FontStyle
			var fontStyle = style.GetPropertyValue(FontStyleProperty.DefaultName, Theme, _text.fontStyle);
			fontStyle = _overrides.FontStyle.OverrideOrDefaultValue(fontStyle);
			_text.fontStyle = fontStyle;
		}
	}
}