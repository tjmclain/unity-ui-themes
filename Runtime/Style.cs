using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public abstract class Style : ScriptableObject
{
	[SerializeField]
	private string _className = "";

	[SerializeReference]
	private List<StyleProperty> _properties = new();

	public abstract Dictionary<string, System.Type> PropertyDefinitions { get; }

	public string ClassName => _className;
	public List<StyleProperty> Properties => _properties;

	public static string PropertiesFieldName => nameof(_properties);

	public bool TryGetProperty<T>(string propertyName, out T property) where T : StyleProperty
	{
		if (string.IsNullOrEmpty(propertyName))
		{
			Debug.LogWarning($"{nameof(propertyName)} is null or empty", this);
			property = default;
			return false;
		}

		int index = _properties.FindIndex(x => x != null && x.Name == propertyName);
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
		return $"{name} ({GetType().Name}{(string.IsNullOrEmpty(ClassName) ? string.Empty : $".{ClassName}")})";
	}
}