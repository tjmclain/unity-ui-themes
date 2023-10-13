using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Myna.Unity.Themes
{
	public abstract class OverrideProperty
	{
		[SerializeField, HideInInspector]
		private bool _enabled = false;

		public static string EnabledPropertyName => nameof(_enabled);

		public bool Enabled => _enabled;
	}

	[System.Serializable]
	public class OverrideProperty<T> : OverrideProperty
	{
		[SerializeField]
		private T _value;

		public virtual T Value => _value;

		public static string ValuePropertyName => nameof(_value);

		public T OverrideOrDefaultValue(T defaultValue)
		{
			return Enabled ? Value : defaultValue;
		}
	}
}