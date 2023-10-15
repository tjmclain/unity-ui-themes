using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Myna.Unity.Themes
{
	public abstract class OverrideProperty
	{
		public const string EnabledPropertyName = nameof(_enabled);

		[SerializeField, HideInInspector]
		private bool _enabled = false;

		[SerializeField, HideInInspector]
		private string _stylePropertyName = string.Empty;

		public OverrideProperty(string stylePropertyName = "")
		{
			_stylePropertyName = stylePropertyName;
		}

		public bool Enabled => _enabled;
		public string StylePropertyName => _stylePropertyName;
	}

	[System.Serializable]
	public class OverrideProperty<T> : OverrideProperty
	{
		public const string ValuePropertyName = nameof(_value);

		[SerializeField]
		private T _value;

		public OverrideProperty(string targetPropertyName = "") : base(targetPropertyName)
		{
		}

		public virtual T Value => _value;

		public T GetValueOrOverride(Style style, Theme theme, T defaultValue)
		{
			if (Enabled)
			{
				return Value;
			}

			if (style == null)
			{
				return defaultValue;
			}

			if (string.IsNullOrEmpty(StylePropertyName))
			{
				return defaultValue;
			}

			return style.GetPropertyValue(StylePropertyName, theme, defaultValue);
		}
	}

	[System.Serializable]
	public class OverrideAlphaProperty : OverrideProperty<float>
	{
		public OverrideAlphaProperty(string targetPropertyName = "") : base(targetPropertyName)
		{
		}
	}
}