using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageTypeFieldAttribute : PropertyAttribute
{
	public Image.Type[] VisibleInTypes { get; set; } = new Image.Type[0];

	public ImageTypeFieldAttribute(params Image.Type[] visibleInTypes)
	{
		VisibleInTypes = visibleInTypes;
	}
}