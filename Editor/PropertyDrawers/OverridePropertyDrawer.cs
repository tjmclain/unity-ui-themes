using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.CodeDom;
using System.Reflection;
using System.Linq;

namespace Myna.Unity.Themes.Editor
{
	[CustomPropertyDrawer(typeof(OverrideProperty), true)]
	public class OverridePropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			// if we only have 1 child property, draw that; otherwise draw a normal property field
			var children = SerializedPropertyUtility.GetDirectChildren(property);
			var mainProperty = children.Count == 1 ? children[0] : property;
			var enabledProperty = property.FindPropertyRelative(OverrideProperty.EnabledPropertyName);

			var toggleSize = EditorStyles.toggle.CalcSize(GUIContent.none);

			float buffer = EditorGUIUtility.singleLineHeight * 0.5f;

			var valuePos = new Rect(position) { width = position.width - toggleSize.x - buffer };
			var enabledPos = new Rect(position)
			{
				x = position.x + valuePos.width + buffer,
				width = toggleSize.x,
			};

			EditorGUI.BeginDisabledGroup(!enabledProperty.boolValue);
			EditorGUI.PropertyField(valuePos, mainProperty, label);
			EditorGUI.EndDisabledGroup();

			enabledProperty.boolValue = EditorGUI.Toggle(enabledPos, enabledProperty.boolValue);
		}

		private static bool TryGetReflectedValue<T>(Object root, string path, out T value)
		{
			if (!TryGetReflectedValue(root, path, out var result))
			{
				value = default;
				return false;
			}

			if (result is not T typedResult)
			{
				value = default;
				return false;
			}

			value = typedResult;
			return true;
		}

		private static bool TryGetReflectedValue(Object root, string path, out object value)
		{
			static bool TryGetMemberValue(object target, string memberName, ref object value)
			{
				var type = target.GetType();
				var flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
				var members = type.GetFields(flags)
					.Cast<MemberInfo>()
					.Concat(type.GetProperties(flags))
					.Where(x => x.Name == memberName)
					.ToArray();

				var member = members[0];
				if (member == null)
				{
					return false;
				}

				switch (member.MemberType)
				{
					case MemberTypes.Field:
						value = ((FieldInfo)member).GetValue(target);
						return true;

					case MemberTypes.Property:
						value = ((PropertyInfo)member).GetValue(target);
						return true;
				}

				return false;
			}

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
				if (!TryGetMemberValue(target, memberName, ref target))
				{
					Debug.LogError($"Could not find {memberName} in {path}", root);
					value = default;
					return false;
				}
			}

			value = target;
			return true;
		}
	}
}