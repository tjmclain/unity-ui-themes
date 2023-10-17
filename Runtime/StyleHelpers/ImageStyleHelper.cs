using System;
using UnityEngine;
using UnityEngine.UI;

namespace Myna.Unity.Themes
{
	public class ImageStyleHelper : StyleHelper
	{
		[Serializable]
		public class OverrideProperties
		{
			public StylePropertyOverride<Sprite> Sprite = new(SpriteProperty.DefaultName);
			public StylePropertyOverride<Color> Color = new(ColorProperty.DefaultName);
			public AlphaPropertyOverride Alpha = new(AlphaProperty.DefaultName);
			public StylePropertyOverride<ImageType> ImageType = new(ImageTypeProperty.DefaultName);
			public StylePropertyOverride<bool> RaycastTarget = new(BoolProperty.Names.RaycastTarget);
		}

		[SerializeField]
		private Image _image;

		[SerializeField]
		private OverrideProperties _overrides = new();

		public Image Image => _image;

#if UNITY_EDITOR

		protected override void OnValidate()
		{
			if (_image == null)
			{
				TryGetComponent(out _image);
			}
		}

#endif

		protected override void ApplyStyle(Style style)
		{
			if (_image == null)
			{
				Debug.LogError($"{nameof(_image)} == null", this);
				return;
			}

			// Source Image
			_image.sprite = _overrides.Sprite.GetValueOrOverride(style, Theme, _image.sprite);

			// Color
			var color = _overrides.Color.GetValueOrOverride(style, Theme, _image.color);
			color.a = _overrides.Alpha.GetValueOrOverride(style, Theme, color.a);
			_image.color = color;

			// Image Type
			var imageType = ImageType.FromImage(_image);
			imageType = _overrides.ImageType.GetValueOrOverride(style, Theme, imageType);
			imageType.Apply(_image);

			// RaycastTarget
			_image.raycastTarget = _overrides.RaycastTarget.GetValueOrOverride(style, Theme, _image.raycastTarget);
		}
	}
}