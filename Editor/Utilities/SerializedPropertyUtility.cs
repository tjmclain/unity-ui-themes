using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public static class SerializedPropertyUtility
{
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
}