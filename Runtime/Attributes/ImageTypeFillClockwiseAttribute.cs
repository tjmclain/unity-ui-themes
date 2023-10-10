using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageTypeFillClockwiseAttribute : ImageTypeFieldAttribute
{
	private string _overrideFillMethodPropertyName = string.Empty;

	public string FillMethodPropertyName
	{
		get
		{
			return string.IsNullOrEmpty(_overrideFillMethodPropertyName)
				? ImageTypeProperty.FillMethodPropertyName
				: _overrideFillMethodPropertyName;
		}
		set
		{
			_overrideFillMethodPropertyName = value;
		}
	}

	public ImageTypeFillClockwiseAttribute() : base(Image.Type.Filled)
	{
	}
}