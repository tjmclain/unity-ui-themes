using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StyleProperty
{
	[SerializeField, HideInInspector]
	private string _name;

	public string Name
	{
		get => _name;
		set => _name = value;
	}
}

[System.Serializable]
public class StyleProperty<T> : StyleProperty
{
	[SerializeField]
	private T _value;

	public T Value => _value;
}