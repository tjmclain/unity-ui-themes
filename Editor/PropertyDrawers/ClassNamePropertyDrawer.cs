using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Myna.Unity.Themes.Editor
{
	[CustomPropertyDrawer(typeof(ClassNameAttribute))]
	public class ClassNamePropertyDrawer : PropertyDrawer
	{
		private static readonly Regex _classNameRegex = new(@"^[a-zA-Z][\w-]*(?:\.[\w-]+)*$");

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			const char dot = '.';

			string value = EditorGUI.TextField(position, label, property.stringValue);
			if (_classNameRegex.IsMatch(value))
			{
				property.stringValue = value;
			}

			if (!property.stringValue.StartsWith(dot))
			{
				property.stringValue = $"{dot}{property.stringValue}";
			}
		}
	}
}