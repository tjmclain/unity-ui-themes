using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class ThemeStyle : ScriptableObject
{
	[SerializeField]
	private string _className = "";

	[SerializeField, HideInInspector]
	private List<StyleProperty> _properties = new();

	public abstract System.Type ComponentType { get; }
	public abstract Dictionary<string, System.Type> PropertyDefinitions { get; }

	public string ClassName => _className;

	public IEnumerable<string> PropertyNames => _properties
		.Where(x => x != null)
		.Select(x => x.name);

	protected List<StyleProperty> Properties => _properties;

	public bool TryGetProperty<T>(string propertyName, out T property) where T : ThemeStyle
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
}