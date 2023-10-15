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
			public OverrideProperty<Color> Color = new(ColorProperty.DefaultName);
			public OverrideAlphaProperty Alpha = new(AlphaProperty.DefaultName);
			public OverrideProperty<float> FontSize = new(FloatProperty.Names.FontSize);
			public OverrideProperty<bool> AutoSize = new(BoolProperty.Names.AutoSize);
			public OverrideProperty<float> FontSizeMin = new(FloatProperty.Names.FontSizeMin);
			public OverrideProperty<float> FontSizeMax = new(FloatProperty.Names.FontSizeMax);
			public OverrideProperty<FontStyles> FontStyle = new(FontStyleProperty.DefaultName);
			public OverrideProperty<bool> RaycastTarget = new(BoolProperty.Names.RaycastTarget);
		}

		[SerializeField]
		private TextMeshProUGUI _text;

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
			var color = _overrides.Color.GetValueOrOverride(style, Theme, _text.color);
			color.a = _overrides.Alpha.GetValueOrOverride(style, Theme, color.a);
			_text.color = color;

			// FontSize
			_text.fontSize = _overrides.FontSize.GetValueOrOverride(style, Theme, _text.fontSize);

			// AutoSize
			_text.enableAutoSizing = _overrides.AutoSize.GetValueOrOverride(style, Theme, _text.enableAutoSizing);

			// FontSizeMin
			_text.fontSizeMin = _overrides.FontSizeMin.GetValueOrOverride(style, Theme, _text.fontSizeMin);

			// FontSizeMax
			_text.fontSizeMax = _overrides.FontSizeMax.GetValueOrOverride(style, Theme, _text.fontSizeMax);

			// FontStyle
			_text.fontStyle = _overrides.FontStyle.GetValueOrOverride(style, Theme, _text.fontStyle);

			// RaycastTarget
			_text.raycastTarget = _overrides.RaycastTarget.GetValueOrOverride(style, Theme, _text.raycastTarget);
		}
	}
}