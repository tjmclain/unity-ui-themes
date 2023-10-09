using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Myna.Unity.Themes
{
	public class ImageStyleHelper : StyleHelper
	{
		[SerializeField]
		private Image _image;

		protected override Type ComponentType => typeof(Image);

		public override void ApplyStyle()
		{
			if (!TryGetStyle(out ImageStyle style))
			{
				return;
			}

			if (style.TryGetProperty(ImageStyle.PropertyNames.Color, out ColorProperty colorProperty))
			{
				_image.color = colorProperty.GetColor(Theme);
			}
		}

		#region MonoBehaviour

		private void OnValidate()
		{
			if (_image == null)
			{
				TryGetComponent(out _image);
			}
		}

		#endregion MonoBehaviour
	}
}