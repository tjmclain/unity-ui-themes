using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json.Schema;

public static class SerializedPropertyUtility
{
	private static readonly IComparer<SerializedProperty> _comparer = new SerializedPropertyComparer();

	public static List<SerializedProperty> GetDirectChildren(this SerializedProperty property)
	{
		var children = new List<SerializedProperty>();

		if (!property.hasVisibleChildren)
		{
			return children;
		}

		int childDepth = property.depth + 1;

		var copy = property.Copy();
		bool enterChildren = true;
		while (copy.NextVisible(enterChildren))
		{
			var next = copy.Copy();
			if (next.depth != childDepth)
			{
				break;
			}

			children.Add(next);
			enterChildren = false;
		}

		return children;
	}

	public static SerializedProperty GetSiblingProperty(this SerializedProperty property, string siblingName)
	{
		string path = property.propertyPath;
		string name = property.name;
		int startIndex = path.Length - name.Length;
		string siblingPath = $"{path.Substring(0, startIndex)}{siblingName}";

		return property.serializedObject.FindProperty(siblingPath);
	}

	public static void SortArrayElements(this SerializedProperty property, IComparer<SerializedProperty> comparer = null)
	{
		if (!property.isArray)
		{
			Debug.LogWarning($"!{nameof(property)}.isArray");
			return;
		}

		comparer ??= _comparer;

		// this is slow, but I'm sure how to do a more optimal sort
		bool changed = true;
		int passes = 0;
		while (changed)
		{
			changed = false;
			passes++;
			for (int i = 0; i < property.arraySize - 1; i++)
			{
				var a = property.GetArrayElementAtIndex(i);
				var b = property.GetArrayElementAtIndex(i + 1);

				int comparison = comparer.Compare(a, b);
				if (comparison > 0)
				{
					changed = true;
					property.MoveArrayElement(i + 1, i);
				}
			}
		}

		// Debug.Log($"Sorted {property.name} in {passes} passes");
	}

	public static int CompareTo(this SerializedProperty property, SerializedProperty other)
	{
		return _comparer.Compare(property, other);
	}

	private class SerializedPropertyComparer : IComparer<SerializedProperty>
	{
		public int Compare(SerializedProperty x, SerializedProperty y)
		{
			return string.Compare(x.displayName, y.displayName, true);
		}
	}
}