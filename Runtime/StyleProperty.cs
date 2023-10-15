using System;
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

	[Serializable, StyleProperty(DefaultName)]
	public class FontAssetProperty : StyleProperty<TMPro.TMP_FontAsset>
	{
		public const string DefaultName = "FontAsset";
	}

	[Serializable, StyleProperty(DefaultName)]
	public class FontStylesProperty : StyleProperty<TMPro.FontStyles>
	{
		public const string DefaultName = "FontStyles";
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