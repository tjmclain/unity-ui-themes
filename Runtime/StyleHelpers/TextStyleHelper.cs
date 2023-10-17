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
			public StylePropertyOverride<Color> Color = new(ColorProperty.DefaultName);
			public AlphaPropertyOverride Alpha = new(AlphaProperty.DefaultName);
			public StylePropertyOverride<float> FontSize = new(FloatProperty.Names.FontSize);
			public StylePropertyOverride<bool> AutoSize = new(BoolProperty.Names.AutoSize);
			public StylePropertyOverride<float> FontSizeMin = new(FloatProperty.Names.FontSizeMin);
			public StylePropertyOverride<float> FontSizeMax = new(FloatProperty.Names.FontSizeMax);
			public StylePropertyOverride<FontStyles> FontStyle = new(FontStyleProperty.DefaultName);
			public StylePropertyOverride<bool> RaycastTarget = new(BoolProperty.Names.RaycastTarget);
		}

		[SerializeField]
		private TextMeshProUGUI _text;

		[SerializeField]
		private OverrideProperties _overrides = new();

#if UNITY_EDITOR

		protected override void OnValidate()
		{
			if (_text == null)
			{
				TryGetComponent(out _text);
			}
		}

#endif

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