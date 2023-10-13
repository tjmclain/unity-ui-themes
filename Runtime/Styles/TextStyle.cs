using System;
using System.Collections;
using System.Collections.Generic;
using Myna.Unity.Themes;
using UnityEngine;

[CreateAssetMenu(fileName = "TextStyle", menuName = "UI Themes/Style - Text")]
public class TextStyle : Style
{
	public static class PropertyNames
	{
		public static readonly string FontAsset = "FontAsset";
		public static readonly string Color = "Color";
		public static readonly string FontSize = "FontSize";
		public static readonly string FontStyle = "FontStyle";
	}

	public override Dictionary<string, Type> PropertyDefinitions
	{
		get
		{
			return new Dictionary<string, Type>()
			{
			};
		}
	}
}