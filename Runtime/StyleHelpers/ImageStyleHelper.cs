using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Myna.Unity.Themes
{
	public class ImageStyleHelper : StyleHelper
	{
		[System.Serializable]
		public class OverrideProperties
		{
			public OverrideProperty<Sprite> SourceImage = new();
			public OverrideProperty<Color> Color = new();
			public OverrideAlphaProperty Alpha = new();
			public OverrideProperty<ImageType> ImageType = new();
		}

		[SerializeField]
		private Image _image;

		[SerializeField]
		private OverrideProperties _overrides = new();

		public Image Image => _image;

		protected override void OnValidate()
		{
			if (_image == null)
			{
				TryGetComponent(out _image);
			}
		}

		protected override void ApplyStyle(Style style)
		{
			if (_image == null)
			{
				Debug.LogError($"{nameof(_image)} == null", this);
				return;
			}

			// Source Image
			var sprite = style.GetPropertyValue(SpriteProperty.DefaultName, Theme, _image.sprite);
			sprite = _overrides.SourceImage.OverrideOrDefaultValue(sprite);
			_image.sprite = sprite;

			// Color
			var color = style.GetPropertyValue(ColorProperty.DefaultName, Theme, _image.color);
			color = _overrides.Color.OverrideOrDefaultValue(color);

			// Alpha
			color.a = style.GetPropertyValue(AlphaProperty.DefaultName, Theme, color.a);
			color.a = _overrides.Alpha.OverrideOrDefaultValue(color.a);
			_image.color = color;

			// Image Type
			var imageType = ImageType.FromImage(_image);
			imageType = style.GetPropertyValue(ImageTypeProperty.DefaultName, Theme, imageType);
			imageType = _overrides.ImageType.OverrideOrDefaultValue(imageType);
			imageType.Apply(_image);
		}
	}
}