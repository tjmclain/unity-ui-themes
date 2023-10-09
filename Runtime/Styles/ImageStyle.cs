using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ImageStyle", menuName = "UI Themes/Style - Image")]
public class ImageStyle : ThemeStyle
{
	public override Type ComponentType => typeof(UnityEngine.UI.Image);

	public override Dictionary<string, Type> PropertyDefinitions
	{
		get
		{
			return new Dictionary<string, Type>()
			{
				{ "Color", typeof(ColorProperty) },
			};
		}
	}
}