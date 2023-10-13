using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Myna.Unity.Themes
{
	public abstract class Style : ScriptableObject
	{
		[SerializeField]
		private string _className = "";

		[SerializeReference]
		private List<StyleProperty> _properties = new();

		public abstract Dictionary<string, System.Type> PropertyDefinitions { get; }

		public string ClassName => _className;
		public List<StyleProperty> Properties => _properties;

		public static string ClassNameFieldName => nameof(_className);
		public static string PropertiesFieldName => nameof(_properties);

		public T GetPropertyValue<T>(string propertyName, Theme theme, T defaultValue)
		{
			if (!TryGetProperty(propertyName, out var property))
			{
				return defaultValue;
			}

			var value = property.GetValue(theme);
			if (value is not T typedValue)
			{
				Debug.LogWarning($"value of {propertyName} is not {typeof(T).Name}", this);
				return defaultValue;
			}

			return typedValue;
		}

		public bool TryGetProperty(string propertyName, out StyleProperty property)
		{
			if (string.IsNullOrEmpty(propertyName))
			{
				Debug.LogError($"{nameof(propertyName)} is null or empty", this);
				property = default;
				return false;
			}

			int index = _properties.FindIndex(x => x != null && x.Name == propertyName);
			if (index < 0)
			{
				Debug.LogWarning($"{nameof(_properties)} does not contain '{propertyName}'");
				property = default;
				return false;
			}

			property = _properties[index];
			return true;
		}

		public bool TryGetProperty<T>(string propertyName, out T property) where T : StyleProperty
		{
			if (!TryGetProperty(propertyName, out StyleProperty untypedProperty))
			{
				property = default;
				return false;
			}

			if (untypedProperty is not T typedProperty)
			{
				Debug.LogWarning($"Property '{nameof(propertyName)}' is not {typeof(T).Name}", this);
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
}