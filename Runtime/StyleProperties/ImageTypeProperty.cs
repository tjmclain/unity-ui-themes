using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageTypeProperty : StyleProperty
{
	[SerializeField]
	private Image.Type _type = Image.Type.Simple;

	[SerializeField, ImageTypeField(Image.Type.Simple)]
	private bool _useSpriteMesh = false;

	[SerializeField, ImageTypeField(Image.Type.Simple, Image.Type.Filled)]
	private bool _preserveAspect = false;

	[SerializeField, ImageTypeField(Image.Type.Sliced, Image.Type.Tiled)]
	private bool _fillCenter = true;

	[SerializeField, ImageTypeField(Image.Type.Sliced, Image.Type.Tiled)]
	private float _pixelsPerUnitMultiplier = 1f;

	[SerializeField, ImageTypeField(Image.Type.Filled)]
	private Image.FillMethod _fillMethod = Image.FillMethod.Horizontal;

	// TODO: custom property drawer for this
	[SerializeField, ImageTypeField(Image.Type.Filled)]
	private int _fillOrigin = 0;

	[SerializeField, ImageTypeField(Image.Type.Filled), Range(0f, 1f)]
	private float _fillAmount = 1f;

	// TODO: custom property drawer for this
	[SerializeField, ImageTypeField(Image.Type.Filled)]
	private bool _clockwise = true;

	public static string TypePropertyName => nameof(_type);

	public void Apply(Image image)
	{
		image.type = _type;
		image.useSpriteMesh = _useSpriteMesh;
		image.preserveAspect = _preserveAspect;
		image.fillCenter = _fillCenter;
		image.pixelsPerUnitMultiplier = _pixelsPerUnitMultiplier;
		image.fillMethod = _fillMethod;
		image.fillOrigin = _fillOrigin;
		image.fillAmount = _fillAmount;
		image.fillClockwise = _clockwise;
	}
}