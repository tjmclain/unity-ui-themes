using UnityEngine;
using UnityEditor;

namespace Myna.Unity.Themes.Editor
{
	[CustomPropertyDrawer(typeof(StylePropertyOverride), true)]
	public class OverridePropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			// if we only have 1 child property, draw that; otherwise draw a normal property field
			var children = SerializedPropertyUtility.GetDirectChildren(property);
			var mainProperty = children.Count == 1 ? children[0] : property;
			var enabledProperty = property.FindPropertyRelative(StylePropertyOverride.EnabledPropertyName);

			var toggleSize = EditorStyles.toggle.CalcSize(GUIContent.none);
			float buffer = EditorGUIUtility.standardVerticalSpacing * 2f;
			var propertyPos = new Rect(position)
			{
				width = position.width - toggleSize.x - buffer
			};
			var enabledPos = new Rect(position)
			{
				x = position.x + propertyPos.width + buffer,
				width = toggleSize.x,
			};

			EditorGUI.BeginDisabledGroup(!enabledProperty.boolValue);
			DrawMainProperty(propertyPos, mainProperty, label);
			EditorGUI.EndDisabledGroup();

			enabledProperty.boolValue = EditorGUI.Toggle(enabledPos, enabledProperty.boolValue, EditorStyles.toggle);
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var children = SerializedPropertyUtility.GetDirectChildren(property);
			var mainProperty = children.Count == 1 ? children[0] : property;

			return EditorGUI.GetPropertyHeight(mainProperty);
		}

		protected virtual void DrawMainProperty(Rect rect, SerializedProperty property, GUIContent label)
		{
			EditorGUI.PropertyField(rect, property, label, true);
		}
	}
}