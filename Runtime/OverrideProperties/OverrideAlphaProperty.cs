using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OverrideAlphaProperty : OverrideProperty<float>
{
	[SerializeField, Range(0f, 1f)]
	private float _alpha = 1f;

	public override float Value => _alpha;
}