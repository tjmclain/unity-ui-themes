using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Myna.Unity.Themes
{
	public class ImageStyleHelper : StyleHelper<ImageStyle, Image>
	{
		[Header("Overrides")]
		[SerializeField]
		private OverrideAlphaProperty _overrideAlpha = new OverrideAlphaProperty();

		protected Image Image => Component;

		protected override void ApplyStyle(ImageStyle style)
		{
			if (style.TryGetProperty(ImageStyle.PropertyNames.Color, out ColorProperty colorProperty))
			{
				var color = colorProperty.GetColor(Theme);
				color.a = _overrideAlpha.OverrideOrDefaultValue(color.a);
				Image.color = color;
			}
		}
	}
}