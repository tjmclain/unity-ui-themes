using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OverrideProperty
{
	[SerializeField, HideInInspector]
	private bool _enabled = false;

	public static string EnabledPropertyName => nameof(_enabled);

	public bool Enabled => _enabled;
}

public abstract class OverrideProperty<T> : OverrideProperty
{
	public abstract T Value { get; }

	public T OverrideOrDefaultValue(T defaultValue)
	{
		return Enabled ? Value : defaultValue;
	}
}