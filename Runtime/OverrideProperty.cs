using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OverrideProperty
{
	[SerializeField]
	private bool _enabled = false;

	public bool Enabled => _enabled;

	public static string EnabledPropertyName => nameof(_enabled);
	public abstract string ValuePropertyName { get; }
}

public abstract class OverrideProperty<T> : OverrideProperty
{
	public abstract T Value { get; }

	public T OverrideOrDefaultValue(T defaultValue)
	{
		return Enabled ? Value : defaultValue;
	}
}