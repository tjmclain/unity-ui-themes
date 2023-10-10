using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageTypeFieldAttribute : PropertyAttribute
{
	private string _overrideTypePropertyName = string.Empty;

	public string TypePropertyName
	{
		get
		{
			return string.IsNullOrEmpty(_overrideTypePropertyName)
				? ImageTypeProperty.TypePropertyName
				: _overrideTypePropertyName;
		}
		set
		{
			_overrideTypePropertyName = value;
		}
	}

	public Image.Type[] VisibleInTypes { get; set; } = new Image.Type[0];

	public ImageTypeFieldAttribute(params Image.Type[] visibleInTypes)
	{
		VisibleInTypes = visibleInTypes;
	}
}