using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Myna.Unity.Themes.Editor
{
	[CustomPropertyDrawer(typeof(StyleHelperComponentAttribute))]
	public class StyleHelperCopmonentPropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var target = property.serializedObject.targetObject;
			var objectType = target.GetType();
			var componentTypeProperty = objectType.GetProperty(nameof(StyleHelper.ComponentType));
			var componentType = componentTypeProperty.GetValue(target) as System.Type;

			label.text = componentType.Name;

			EditorGUI.PropertyField(position, property, label);
		}
	}
}