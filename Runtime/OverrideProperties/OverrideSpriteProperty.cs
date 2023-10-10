using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OverrideSpriteProperty : OverrideProperty<Sprite>
{
	[SerializeField]
	private Sprite _sprite;

	public override Sprite Value => _sprite;
	public override string ValuePropertyName => nameof(_sprite);
}