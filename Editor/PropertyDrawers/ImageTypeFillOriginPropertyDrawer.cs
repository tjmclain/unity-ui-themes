using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Linq;

[CustomPropertyDrawer(typeof(ImageTypeFillOriginAttribute))]
public class ImageTypeFillOriginPropertyDrawer : ImageTypeFieldPropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		static bool TypeGetFillOriginTypeFor(Image.FillMethod fillMethod, out Type type)
		{
			switch (fillMethod)
			{
				case Image.FillMethod.Vertical:
					type = typeof(Image.OriginVertical);
					return true;

				case Image.FillMethod.Horizontal:
					type = typeof(Image.OriginHorizontal);
					return true;

				case Image.FillMethod.Radial90:
					type = typeof(Image.Origin90);
					return true;

				case Image.FillMethod.Radial180:
					type = typeof(Image.Origin180);
					return true;

				case Image.FillMethod.Radial360:
					type = typeof(Image.Origin360);
					return true;
			}

			type = default;
			return false;
		}

		if (!ShowProperty(property))
		{
			return;
		}

		var fillOriginAttribute = attribute as ImageTypeFillOriginAttribute;
		string fillMethodPropertyName = fillOriginAttribute.FillMethodPropertyName;
		var fillMethodProperty = property.serializedObject.FindProperty(fillMethodPropertyName);
		if (fillMethodProperty == null)
		{
			return;
		}

		var fillMethod = (Image.FillMethod)fillMethodProperty.enumValueIndex;
		if (!TypeGetFillOriginTypeFor(fillMethod, out var fillOriginType))
		{
			return;
		}

		var options = Enum.GetNames(fillOriginType)
			.Select(x => new GUIContent(x))
			.ToArray();

		int index = property.intValue;
		index = Math.Clamp(index, 0, options.Length - 1);
		index = EditorGUI.Popup(position, label, index, options);
		property.intValue = index;
	}
}