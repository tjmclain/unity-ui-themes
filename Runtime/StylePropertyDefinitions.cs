using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Myna.Unity.Themes
{
	public static class StylePropertyDefinitions
	{
		private static readonly Dictionary<string, Type> _definitions = new();

		static StylePropertyDefinitions()
		{
			var types = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(x => x.GetTypes())
				.Where(x => x.IsClass && !x.IsAbstract)
				.Where(x => typeof(IStyleProperty).IsAssignableFrom(x));

			_definitions.Clear();
			foreach (var type in types)
			{
				var attribute = type.GetCustomAttribute<StylePropertyAttribute>();
				if (attribute != null)
				{
					foreach (string name in attribute.Names)
					{
						_definitions[name] = type;
					}
				}
				else
				{
					string name = GetDefaultPropertyName(type);
					_definitions[name] = type;
				}
			}
		}

		public static IEnumerable<string> PropertyNames => _definitions.Keys;

		public static bool TryGetPropertyType(string propertyName, out Type propertyType)
		{
			return _definitions.TryGetValue(propertyName, out propertyType);
		}

		private static string GetDefaultPropertyName(Type propertyType)
		{
			const string property = "Property";

			string name = propertyType.Name;
			return name.EndsWith(property) ? name[..^property.Length] : name;
		}
	}
}