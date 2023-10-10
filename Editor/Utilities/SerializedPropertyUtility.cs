using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public static class SerializedPropertyUtility
{
	public static List<SerializedProperty> GetDirectChildren(SerializedProperty property)
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
}