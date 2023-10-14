using System;
using UnityEngine;

namespace Myna.Unity.Themes
{
	#region Style Property Base

	public interface IStyleProperty
	{
		string Name { get; set; }

		Type ValueType { get; }

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

		public abstract Type ValueType { get; }

		public abstract object GetValue(Theme theme);
	}

	public abstract class StyleProperty<T> : StyleProperty
	{
		[SerializeField]
		private T _value;

		public const string ValuePropertyName = nameof(_value);

		public override Type ValueType => typeof(T);

		public override object GetValue(Theme theme)
		{
			return _value;
		}
	}

	#endregion Style Property Base

	#region Style Property Class Definitions

	[Serializable, StyleProperty(StylePropertyNames.Alpha)]
	public class AlphaProperty : StyleProperty<float>
	{
	}

	[Serializable, StyleProperty(StylePropertyNames.Color)]
	public class ColorProperty : StyleProperty
	{
		[SerializeField, ColorSchemeReference]
		private ColorScheme _referenceColorScheme;

		[SerializeField, ColorNameDropdown(nameof(_referenceColorScheme), nameof(_fallbackColor))]
		private string _colorName = string.Empty;

		[SerializeField]
		private Color _fallbackColor = Color.white;

		public override Type ValueType => typeof(Color);
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

	[Serializable, StyleProperty(StylePropertyNames.FontAsset)]
	public class FontAssetProperty : StyleProperty<TMPro.TMP_FontAsset>
	{
	}

	[Serializable, StyleProperty(StylePropertyNames.FontStyles)]
	public class FontStylesProperty : StyleProperty<TMPro.FontStyles>
	{
	}

	[Serializable, StyleProperty(StylePropertyNames.ImageType)]
	public class ImageTypeProperty : StyleProperty<ImageType>
	{
	}

	[Serializable, StyleProperty(StylePropertyNames.Sprite)]
	public class SpriteProperty : StyleProperty<Sprite>
	{
	}

	#endregion Style Property Class Definitions
}