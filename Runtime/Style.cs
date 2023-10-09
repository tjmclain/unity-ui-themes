using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public abstract class Style : ScriptableObject
{
	[SerializeField]
	private string _className = "";

	[SerializeField, HideInInspector]
	private List<StyleProperty> _properties = new();

	public abstract System.Type ComponentType { get; }
	public abstract Dictionary<string, System.Type> PropertyDefinitions { get; }

	public string ClassName => _className;
	public List<StyleProperty> Properties => _properties;

	public string Id => GetId(this);

	public static string GetId(Style style)
	{
		var sb = new StringBuilder();
		sb.Append(style.ComponentType.Name);

		if (!string.IsNullOrEmpty(style.ClassName))
		{
			sb.Append(".");
			sb.Append(style.ClassName);
		}

		return sb.ToString();
	}

	public bool TryGetProperty<T>(string propertyName, out T property) where T : StyleProperty
	{
		if (string.IsNullOrEmpty(propertyName))
		{
			Debug.LogWarning($"{nameof(propertyName)} is null or empty", this);
			property = default;
			return false;
		}

		int index = _properties.FindIndex(x => x.name == propertyName);
		if (index < 0)
		{
			property = default;
			return false;
		}

		if (_properties[index] is not T typedProperty)
		{
			Debug.LogWarning($"Property '{nameof(propertyName)}' is not {typeof(T).Name}");
			property = default;
			return false;
		}

		property = typedProperty;
		return true;
	}

	public override string ToString()
	{
		base.ToString();
		return $"{name} ({GetType().Name}, id: {Id})";
	}
}