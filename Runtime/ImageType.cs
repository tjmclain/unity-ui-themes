using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

[System.Serializable]
public class ImageType
{
	[SerializeField]
	private Image.Type _type = Image.Type.Simple;

	[SerializeField, ImageTypeField(Image.Type.Simple)]
	private bool _useSpriteMesh = false;

	[SerializeField, ImageTypeField(Image.Type.Sliced, Image.Type.Tiled)]
	private bool _fillCenter = true;

	[SerializeField, ImageTypeField(Image.Type.Sliced, Image.Type.Tiled)]
	private float _pixelsPerUnitMultiplier = 1f;

	[SerializeField, ImageTypeField(Image.Type.Filled)]
	private Image.FillMethod _fillMethod = Image.FillMethod.Horizontal;

	// TODO: custom property drawer for this
	[SerializeField, ImageTypeFillOrigin]
	private int _fillOrigin = 0;

	[SerializeField, ImageTypeField(Image.Type.Filled), Range(0f, 1f)]
	private float _fillAmount = 1f;

	[SerializeField, ImageTypeFillClockwise]
	private bool _clockwise = true;

	[SerializeField, ImageTypeField(Image.Type.Simple, Image.Type.Filled)]
	private bool _preserveAspect = false;

	public static readonly string TypePropertyName = nameof(_type);
	public static readonly string FillMethodPropertyName = nameof(_fillMethod);

	public Image.Type Type => _type;
	public Image.FillMethod FillMethod => _fillMethod;

	public static ImageType FromImage(Image image)
	{
		var value = new ImageType();
		value.CopyValuesFromImage(image);
		return value;
	}

	public void CopyValuesFromImage(Image image)
	{
		_type = image.type;
		_useSpriteMesh = image.useSpriteMesh;
		_preserveAspect = image.preserveAspect;
		_fillCenter = image.fillCenter;
		_pixelsPerUnitMultiplier = image.pixelsPerUnitMultiplier;
		_fillMethod = image.fillMethod;
		_fillOrigin = image.fillOrigin;
		_fillAmount = image.fillAmount;
		_clockwise = image.fillClockwise;
	}

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