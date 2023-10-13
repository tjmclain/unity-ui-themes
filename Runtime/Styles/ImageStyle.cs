using System;
using System.Collections;
using System.Collections.Generic;
using Myna.Unity.Themes;
using UnityEngine;

[CreateAssetMenu(fileName = "ImageStyle", menuName = "UI Themes/Style - Image")]
public class ImageStyle : Style
{
	public static class PropertyNames
	{
		public static readonly string SourceImage = "SourceImage";
		public static readonly string Color = "Color";
		public static readonly string ImageType = "ImageType";
	}

	public override Dictionary<string, Type> PropertyDefinitions
	{
		get
		{
			return new Dictionary<string, Type>()
			{
				{ PropertyNames.SourceImage, typeof(SpriteProperty) },
				{ PropertyNames.Color, typeof(ColorProperty) },
				{ PropertyNames.ImageType, typeof(ImageTypeProperty) }
			};
		}
	}
}