using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Linq;

[CustomPropertyDrawer(typeof(ImageTypeFillClockwiseAttribute))]
public class ImageTypeFillClockwisePropertyDrawer : ImageTypeFieldPropertyDrawer
{
	protected override bool ShowProperty(SerializedProperty property)
	{
		if (!base.ShowProperty(property))
		{
			return false;
		}

		var fillClockwiseAttribute = attribute as ImageTypeFillClockwiseAttribute;
		string fillMethodPropertyName = fillClockwiseAttribute.FillMethodPropertyName;
		var fillMethodProperty = property.serializedObject.FindProperty(fillMethodPropertyName);
		if (fillMethodProperty == null)
		{
			return false;
		}

		var fillMethod = (Image.FillMethod)fillMethodProperty.enumValueIndex;
		switch (fillMethod)
		{
			case Image.FillMethod.Radial360:
			case Image.FillMethod.Radial180:
			case Image.FillMethod.Radial90:
				return true;

			default:
				return false;
		}
	}
}