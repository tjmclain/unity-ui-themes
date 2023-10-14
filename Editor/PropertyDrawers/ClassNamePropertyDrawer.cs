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
		private const char _dot = '.';

		private static readonly Regex _classNameRegex = new(@"^\.[a-zA-Z][\w-]*(?:\.[\w-]+)*$");

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (!property.stringValue.StartsWith(_dot))
			{
				property.stringValue = _dot + property.stringValue;
			}

			string value = EditorGUI.TextField(position, label, property.stringValue);
			if (_classNameRegex.IsMatch(value))
			{
				property.stringValue = value;
			}
		}
	}
}