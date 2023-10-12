using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;
using System.IO;

// TODO: not sure if I still need this; I might be able to just delete this
public static class ReflectionUtility
{
	public static bool TryGetMemberValueRelative<T>(Object root, string memberPath, out T value)
	{
		if (!TryGetMemberValueRelative(root, memberPath, out var result))
		{
			value = default;
			return false;
		}

		if (result is not T convertedResult)
		{
			Debug.LogError($"{nameof(value)} at {memberPath} is {result.GetType()}, not {typeof(T).Name}", root);
			value = default;
			return false;
		}

		value = convertedResult;
		return true;
	}

	public static bool TryGetMemberValueRelative(Object root, string memberPath, out object value)
	{
		if (root == null)
		{
			value = default;
			return false;
		}

		if (string.IsNullOrEmpty(memberPath))
		{
			value = default;
			return false;
		}

		object target = root;
		var memberNames = memberPath.Split('.');
		foreach (var memberName in memberNames)
		{
			if (!TryGetMemberValue(target, memberName, out object result))
			{
				Debug.LogError($"Could not find {memberName} in {memberPath}", root);
				value = default;
				return false;
			}

			target = result;
		}

		value = target;
		return true;
	}

	public static bool TryGetMemberRelative(Object root, string memberPath, out MemberInfo member)
	{
		member = default;

		if (root == null)
		{
			return false;
		}

		if (string.IsNullOrEmpty(memberPath))
		{
			return false;
		}

		object target = root;
		var memberNames = memberPath.Split('.');
		foreach (var memberName in memberNames)
		{
			if (!TryGetMember(target, memberName, out member))
			{
				Debug.LogError($"Could not find {memberName} in {memberPath}", root);
				member = default;
				return false;
			}

			if (!TryGetMemberValue(target, member, out target))
			{
				Debug.LogError($"Could not get value for {memberName} in {memberPath}", root);
				return false;
			}
		}

		return true;
	}

	private static bool TryGetMember(object target, string memberName, out MemberInfo value)
	{
		var type = target.GetType();
		var flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
		var members = type.GetMember(memberName, flags);
		if (members == null || members.Length == 0)
		{
			value = default;
			return false;
		}

		value = members[0];
		return true;
	}

	private static bool TryGetMemberValue(object target, string memberName, out object value)
	{
		if (!TryGetMember(target, memberName, out MemberInfo member))
		{
			value = default;
			return false;
		}

		return TryGetMemberValue(target, member, out value);
	}

	private static bool TryGetMemberValue(object target, MemberInfo member, out object value)
	{
		switch (member.MemberType)
		{
			case MemberTypes.Field:
				var field = member as FieldInfo;
				value = field.GetValue(target);
				return true;

			case MemberTypes.Property:
				var property = member as PropertyInfo;
				value = property.GetValue(target);
				return true;
		}

		value = default;
		return false;
	}
}