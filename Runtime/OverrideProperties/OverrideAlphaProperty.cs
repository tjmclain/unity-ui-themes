using UnityEngine;

namespace Myna.Unity.Themes
{
	[System.Serializable]
	public class OverrideAlphaProperty : OverrideProperty<float>
	{
		[SerializeField, Range(0f, 1f)]
		private float _alpha = 1f;

		public override float Value => _alpha;
	}
}