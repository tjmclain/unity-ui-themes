using System;
using UnityEngine;
using UnityEngine.UI;

public class ImageTypeFieldAttribute : PropertyAttribute
{
	public Image.Type[] VisibleInTypes { get; set; } = new Image.Type[0];

	public ImageTypeFieldAttribute(params Image.Type[] visibleInTypes)
	{
		VisibleInTypes = visibleInTypes;
	}

#if UNITY_EDITOR

	public virtual bool IsVisible(UnityEditor.SerializedProperty imageTypeProperty)
	{
		var imageType = imageTypeProperty.FindPropertyRelative(ImageType.TypePropertyName);
		if (imageType == null)
		{
			Debug.LogWarning($"imageType == null; property = {imageTypeProperty.name}");
			return false;
		}

		var imageTypeValue = (Image.Type)imageType.enumValueIndex;
		return Array.Exists(VisibleInTypes, x => x == imageTypeValue);
	}

#endif
}