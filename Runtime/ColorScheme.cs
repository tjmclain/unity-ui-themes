using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Myna.Unity.Themes
{
	[CreateAssetMenu(fileName = "ColorScheme", menuName = "UI Themes/Color Scheme")]
	public class ColorScheme : ScriptableObject
	{
		public const string ColorsPropertyName = nameof(_colors);

		[SerializeField, HideInInspector, SingleLineProperty(ColorInfo.NamePropertyName, ColorInfo.ColorPropertyName)]
		private ColorInfo[] _colors = new ColorInfo[0];

		public virtual ColorInfo[] Colors
		{
			get => _colors;
			protected set => _colors = value;
		}

		public virtual IEnumerable<string> GetColorNames()
		{
			return _colors.Select(x => x.Name);
		}

		public virtual bool TryGetColor(string colorName, out Color color)
		{
			if (_colors.TryGetValue(x => x.Name == colorName, out var info))
			{
				color = info.Color;
				return true;
			}
			else
			{
				color = default;
				return false;
			}
		}

		public virtual bool TryGetColorByGuid(string guid, out Color color)
		{
			if (_colors.TryGetValue(x => x.Guid == guid, out var info))
			{
				color = info.Color;
				return true;
			}
			else
			{
				color = default;
				return false;
			}
		}

		public virtual string ColorGuidToName(string guid)
		{
			return _colors.TryGetValue(x => x.Guid == guid, out var color)
				? color.Name : string.Empty;
		}

		public virtual string ColorNameToGuid(string colorName)
		{
			return _colors.TryGetValue(x => x.Name == colorName, out var color)
				? color.Guid : string.Empty;
		}

		[Serializable]
		public class ColorInfo
		{
			public const string NamePropertyName = nameof(_name);
			public const string ColorPropertyName = nameof(_color);
			public const string GuidPropertyName = nameof(_guid);

			[SerializeField]
			protected string _name = "Color";

			[SerializeField]
			protected Color _color = Color.white;

			[SerializeField, HideInInspector]
			protected string _guid = System.Guid.NewGuid().ToString();

			public string Name => _name;
			public Color Color => _color;
			public string Guid => _guid;
		}
	}
}