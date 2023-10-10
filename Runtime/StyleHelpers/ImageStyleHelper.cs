using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Myna.Unity.Themes
{
	public class ImageStyleHelper : StyleHelper<ImageStyle>
	{
		[SerializeField]
		private Image _image;

		[Header("Overrides")]
		[SerializeField]
		private OverrideSpriteProperty _overrideSourceImage = new();

		[SerializeField]
		private OverrideColorProperty _overrideColor = new();

		[SerializeField]
		private OverrideAlphaProperty _overrideAlpha = new();

		[SerializeField]
		private bool _overrideImageType = new();

		protected override void OnValidate()
		{
			if (_image == null)
			{
				TryGetComponent(out _image);
			}

			base.OnValidate();
		}

		protected override void ApplyStyle(ImageStyle style)
		{
			if (_image == null)
			{
				Debug.LogError($"{nameof(_image)} == null", this);
				return;
			}

			// Source Image
			if (style.TryGetProperty(ImageStyle.PropertyNames.SourceImage,
				out SpriteProperty spriteProperty))
			{
				_image.sprite = _overrideSourceImage.OverrideOrDefaultValue(spriteProperty.Sprite);
			}

			// Color
			var color = _image.color;
			if (style.TryGetProperty(ImageStyle.PropertyNames.Color,
				out ColorProperty colorProperty))
			{
				color = colorProperty.GetColor(Theme);
			}

			color = _overrideColor.OverrideOrDefaultValue(color);
			color.a = _overrideAlpha.OverrideOrDefaultValue(color.a);
			_image.color = color;

			// Image Type
			if (!_overrideImageType && style.TryGetProperty(ImageStyle.PropertyNames.ImageType,
				out ImageTypeProperty imageTypeProperty))
			{
				imageTypeProperty.Apply(_image);
			}
		}
	}
}