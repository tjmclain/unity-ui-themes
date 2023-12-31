using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ImageType))]
public class ImageTypePropertyDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		static void DrawProperty(SerializedProperty property, GUIContent label, ref Rect rect)
		{
			if (property == null)
			{
				return;
			}

			float height = EditorGUI.GetPropertyHeight(property);
			rect.height = height;

			EditorGUI.PropertyField(rect, property, label, true);

			rect.y += height;
			rect.y += EditorGUIUtility.standardVerticalSpacing;
		}

		var rect = new Rect(position)
		{
			height = EditorGUIUtility.singleLineHeight
		};
		EditorGUI.PropertyField(rect, property, label, false);

		if (!property.isExpanded)
		{
			return;
		}

		rect.y += EditorGUIUtility.singleLineHeight;
		rect.y += EditorGUIUtility.standardVerticalSpacing;

		var children = property.GetDirectChildren()
			.Where(x => IsVisible(property, x));

		EditorGUI.indentLevel++;
		foreach (var child in children)
		{
			DrawProperty(child, EditorGUIUtility.TrTextContent(child.displayName), ref rect);
		}
		EditorGUI.indentLevel--;
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		if (!property.isExpanded)
		{
			return EditorGUI.GetPropertyHeight(property);
		}

		var children = property.GetDirectChildren()
			.Where(x => IsVisible(property, x));

		float height = EditorGUIUtility.singleLineHeight;
		foreach (var child in children)
		{
			height += EditorGUI.GetPropertyHeight(child);
			height += EditorGUIUtility.standardVerticalSpacing;
		}

		return height;
	}

	private bool IsVisible(SerializedProperty imageTypeProperty, SerializedProperty property)
	{
		var flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
		var fieldInfo = typeof(ImageType).GetField(property.name, flags);
		if (fieldInfo == null)
		{
			Debug.LogError($"{nameof(ImageType)} does not contain field '{property.name}'");
			return false;
		}

		var attribute = fieldInfo.GetCustomAttribute<ImageTypeFieldAttribute>();
		if (attribute == null)
		{
			return true;
		}

		return attribute.IsVisible(imageTypeProperty);
	}
}