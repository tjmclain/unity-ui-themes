using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Myna.Unity.Themes
{
	[CreateAssetMenu(fileName = "Style", menuName = "UI Themes/Style")]
	public class Style : ScriptableObject
	{
		[SerializeReference]
		private List<IStyleProperty> _properties = new();

		public List<IStyleProperty> Properties => _properties;

		public static string PropertiesFieldName => nameof(_properties);

		public T GetPropertyValue<T>(Theme theme, T defaultValue)
		{
			int index = _properties.FindIndex(x => x.ValueType == typeof(T));
			if (index < 0)
			{
				//Debug.LogWarning($"{nameof(_properties)} does not contain property with {nameof(IStyleProperty.ValueType)} {typeof(T).Name}", this);
				return defaultValue;
			}

			var property = _properties[index];
			var value = property.GetValue(theme);
			if (value is not T typedValue)
			{
				Debug.LogWarning($"value of {property.Name} is not {typeof(T).Name}", this);
				return defaultValue;
			}

			return typedValue;
		}

		public T GetPropertyValue<T>(string propertyName, Theme theme, T defaultValue)
		{
			int index = _properties.FindIndex(x => x.Name == propertyName);
			if (index < 0)
			{
				//Debug.LogWarning($"{nameof(_properties)} does not contain property with {nameof(IStyleProperty.Name)} '{propertyName}'", this);
				return defaultValue;
			}

			var property = _properties[index];
			var value = property.GetValue(theme);
			if (value is not T typedValue)
			{
				Debug.LogWarning($"value of {property.Name} is not {typeof(T).Name}", this);
				return defaultValue;
			}

			return typedValue;
		}

		public bool TryGetProperty(string propertyName, out IStyleProperty property)
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
				Debug.LogWarning($"{nameof(_properties)} does not contain '{propertyName}'", this);
				property = default;
				return false;
			}

			property = _properties[index];
			return true;
		}

		public bool TryGetProperty<T>(string propertyName, out T property) where T : IStyleProperty
		{
			if (!TryGetProperty(propertyName, out IStyleProperty value))
			{
				property = default;
				return false;
			}

			if (value is not T typedValue)
			{
				Debug.LogWarning($"Property '{nameof(propertyName)}' is not {typeof(T).Name}", this);
				property = default;
				return false;
			}

			property = typedValue;
			return true;
		}
	}
}