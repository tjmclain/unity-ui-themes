using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;

// TODO: not sure if I still need this; I might be able to just delete this
public static class ReflectionUtility
{
	public static bool TryGetReflectedValue<T>(Object root, string path, out T value)
	{
		if (!TryGetReflectedValue(root, path, out var result))
		{
			value = default;
			return false;
		}

		if (result is not T convertedResult)
		{
			Debug.LogError($"{nameof(value)} at {path} is {result.GetType()}, not {typeof(T).Name}", root);
			value = default;
			return false;
		}

		value = convertedResult;
		return true;
	}

	public static bool TryGetReflectedValue(Object root, string path, out object value)
	{
		if (root == null)
		{
			value = default;
			return false;
		}

		if (string.IsNullOrEmpty(path))
		{
			value = default;
			return false;
		}

		object target = root;
		var memberNames = path.Split('.');
		foreach (var memberName in memberNames)
		{
			if (!TryGetMemberValue(target, memberName, out object result))
			{
				Debug.LogError($"Could not find {memberName} in {path}", root);
				value = default;
				return false;
			}

			target = result;
		}

		value = target;
		return true;
	}

	public static bool TryGetMemberValue(object target, string memberName, out object value)
	{
		var type = target.GetType();
		var flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

		// Try getting value from FieldInfo
		var field = type.GetField(memberName, flags);
		if (field != null)
		{
			value = field.GetValue(target);
			return true;
		}

		// Try getting value from PropertyInfo
		var property = type.GetProperty(memberName, flags);
		if (property != null)
		{
			value = property.GetValue(target);
			return true;
		}

		value = default;
		return false;
	}
}