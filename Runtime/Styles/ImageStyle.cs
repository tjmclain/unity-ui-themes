using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ImageStyle", menuName = "UI Themes/Style - Image")]
public class ImageStyle : Style
{
	public static class PropertyNames
	{
		public static readonly string Color = "Color";
	}

	public override Type ComponentType => typeof(UnityEngine.UI.Image);

	public override Dictionary<string, Type> PropertyDefinitions
	{
		get
		{
			return new Dictionary<string, Type>()
			{
				{ PropertyNames.Color, typeof(ColorProperty) },
			};
		}
	}
}