using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteProperty : StyleProperty
{
	[SerializeField]
	private Sprite _sprite;

	public Sprite Sprite => _sprite;
}