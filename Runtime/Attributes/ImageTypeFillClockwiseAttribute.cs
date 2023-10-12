using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageTypeFillClockwiseAttribute : ImageTypeFieldAttribute
{
	public ImageTypeFillClockwiseAttribute() : base(Image.Type.Filled)
	{
	}

#if UNITY_EDITOR

	public override bool IsVisible(UnityEditor.SerializedProperty property)
	{
		if (!base.IsVisible(property))
		{
			return false;
		}

		var fillMethod = property.FindPropertyRelative(ImageType.FillMethodPropertyName);
		var fillMethodValue = (Image.FillMethod)fillMethod.enumValueIndex;

		switch (fillMethodValue)
		{
			case Image.FillMethod.Radial360:
			case Image.FillMethod.Radial180:
			case Image.FillMethod.Radial90:
				return true;
		}

		return false;
	}

#endif
}