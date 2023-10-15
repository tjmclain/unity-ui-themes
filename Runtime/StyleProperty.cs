using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Myna.Unity.Themes
{
	#region StyleProperty Base

	public interface IStyleProperty
	{
		string Name { get; set; }

		object GetValue(Theme theme);
	}

	public abstract class StyleProperty : IStyleProperty
	{
		[SerializeField, HideInInspector]
		protected string _name = "";

		public const string NamePropertyName = nameof(_name);

		public virtual string Name
		{
			get => _name;
			set => _name = value;
		}

		public abstract object GetValue(Theme theme);
	}

	public abstract class StyleProperty<T> : StyleProperty
	{
		[SerializeField]
		private T _value;

		public const string ValuePropertyName = nameof(_value);

		public override object GetValue(Theme theme)
		{
			return _value;
		}
	}

	#endregion StyleProperty Base

	#region StyleProperty Concrete Class Definitions

	[Serializable, StyleProperty(DefaultName)]
	public class AlphaProperty : StyleProperty<float>
	{
		public const string DefaultName = "Alpha";
	}

	[Serializable, StyleProperty(AutoSize)]
	public class BoolProperty : StyleProperty<bool>
	{
		public const string AutoSize = "AutoSize";
	}

	[Serializable, StyleProperty(DefaultName)]
	public class ColorProperty : StyleProperty
	{
		public const string DefaultName = "Color";

		[SerializeField, ColorSchemeReference]
		private ColorScheme _referenceColorScheme;

		[SerializeField, ColorNameDropdown(nameof(_referenceColorScheme), nameof(_fallbackColor))]
		private string _colorName = string.Empty;

		[SerializeField]
		private Color _fallbackColor = Color.white;

		public string ColorName => _colorName;
		public Color FallbackColor => _fallbackColor;

		public override object GetValue(Theme theme)
		{
			if (string.IsNullOrEmpty(ColorName))
			{
				return FallbackColor;
			}

			return theme.TryGetColorByGuid(ColorName, out var color)
				|| theme.TryGetColor(ColorName, out color)
				? color : FallbackColor;
		}
	}

	[Serializable, StyleProperty(FontSize, FontSizeMin, FontSizeMax)]
	public class FloatProperty : StyleProperty<float>
	{
		public const string FontSize = "FontSize";
		public const string FontSizeMin = "FontSizeMin";
		public const string FontSizeMax = "FontSizeMax";
	}

	[Serializable, StyleProperty(DefaultName)]
	public class FontAssetProperty : StyleProperty<TMPro.TMP_FontAsset>
	{
		public const string DefaultName = "FontAsset";
	}

	[Serializable, StyleProperty(DefaultName)]
	public class FontStyleProperty : StyleProperty<TMPro.FontStyles>
	{
		public const string DefaultName = "FontStyle";
	}

	[Serializable, StyleProperty(DefaultName)]
	public class ImageTypeProperty : StyleProperty<ImageType>
	{
		public const string DefaultName = "ImageType";
	}

	[Serializable, StyleProperty(DefaultName)]
	public class SpriteProperty : StyleProperty<Sprite>
	{
		public const string DefaultName = "Sprite";
	}

	#endregion StyleProperty Concrete Class Definitions
}