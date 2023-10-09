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
		private OverrideColorProperty _overrideColor = new();

		[SerializeField]
		private OverrideAlphaProperty _overrideAlpha = new();

		[SerializeField]
		private OverrideToggleProperty _overrideImageType = new();

		protected Image Image => Component;

		protected override void ApplyStyle(ImageStyle style)
		{
			var color = Image.color;
			if (style.TryGetProperty(ImageStyle.PropertyNames.Color,
				out ColorProperty colorProperty))
			{
				color = colorProperty.GetColor(Theme);
			}

			if (!_overrideImageType.Enabled)
			{
				if (style.TryGetProperty(ImageStyle.PropertyNames.ImageType,
					out ImageTypeProperty imageTypeProperty))
				{
					imageTypeProperty.Apply(Image);
				}
			}

			color = _overrideColor.OverrideOrDefaultValue(color);
			color.a = _overrideAlpha.OverrideOrDefaultValue(color.a);

			Image.color = color;
		}
	}
}