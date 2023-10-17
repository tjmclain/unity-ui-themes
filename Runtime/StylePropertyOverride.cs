using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Myna.Unity.Themes
{
	public abstract class StylePropertyOverride
	{
		public const string EnabledPropertyName = nameof(_enabled);

		[SerializeField, HideInInspector]
		private bool _enabled = false;

		[SerializeField, HideInInspector]
		private string _stylePropertyName = string.Empty;

		public StylePropertyOverride(string stylePropertyName = "")
		{
			_stylePropertyName = stylePropertyName;
		}

		public bool Enabled => _enabled;
		public string StylePropertyName => _stylePropertyName;
	}

	[System.Serializable]
	public class StylePropertyOverride<T> : StylePropertyOverride
	{
		public const string ValuePropertyName = nameof(_value);

		[SerializeField]
		private T _value;

		public StylePropertyOverride(string stylePropertyName = "") : base(stylePropertyName)
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
				Debug.LogWarning($"{GetTypeName()}.{nameof(StylePropertyName)} is null or empty");
				return defaultValue;
			}

			return style.GetPropertyValue(StylePropertyName, theme, defaultValue);
		}

		protected virtual string GetTypeName()
		{
			return $"{nameof(StylePropertyOverride)}<{typeof(T).Name}>";
		}
	}

	[System.Serializable]
	public class AlphaPropertyOverride : StylePropertyOverride<float>
	{
		public AlphaPropertyOverride(string targetPropertyName = "") : base(targetPropertyName)
		{
		}

		protected override string GetTypeName()
		{
			return nameof(AlphaPropertyOverride);
		}
	}
}