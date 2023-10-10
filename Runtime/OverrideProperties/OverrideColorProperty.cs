using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OverrideColorProperty : OverrideProperty<Color>
{
	[SerializeField]
	private Color _color = Color.white;

	public override Color Value => _color;
}