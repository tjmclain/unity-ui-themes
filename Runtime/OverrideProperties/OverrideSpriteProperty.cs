using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Myna.Unity.Themes
{
	[System.Serializable]
	public class OverrideSpriteProperty : OverrideProperty<Sprite>
	{
		[SerializeField]
		private Sprite _sprite;

		public override Sprite Value => _sprite;
	}
}