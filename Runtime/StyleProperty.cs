using System;
using UnityEngine;

namespace Myna.Unity.Themes
{
	public interface IStyleProperty
	{
		string Name { get; }

		Type ValueType { get; }

		object GetValue(Theme theme);
	}

	public abstract class StyleProperty : IStyleProperty
	{
		[SerializeField, HideInInspector]
		protected string _name = "";

		public virtual string Name => _name;

		public abstract Type ValueType { get; }

		public abstract object GetValue(Theme theme);
	}

	public abstract class StyleProperty<T> : StyleProperty
	{
		[SerializeField]
		private T _value;

		public StyleProperty(string name)
		{
			_name = name;
		}

		public override Type ValueType => typeof(T);

		public override object GetValue(Theme theme)
		{
			return _value;
		}
	}

	[Serializable]
	public class AlphaProperty : StyleProperty<float>
	{
		public const string DefaultName = "Alpha";

		public AlphaProperty() : base(DefaultName)
		{
		}
	}

	[Serializable]
	public class FontAssetProperty : StyleProperty<TMPro.TMP_FontAsset>
	{
		public const string DefaultName = "FontAsset";

		public FontAssetProperty() : base(DefaultName)
		{
		}
	}

	[Serializable]
	public class FontStylesProperty : StyleProperty<TMPro.FontStyles>
	{
		public const string DefaultName = "FontStyles";

		public FontStylesProperty() : base(DefaultName)
		{
		}
	}

	[System.Serializable]
	public class ImageTypeProperty : StyleProperty<ImageType>
	{
		public const string DefaultName = "ImageType";

		public ImageTypeProperty() : base(DefaultName)
		{
		}
	}

	[System.Serializable]
	public class SpriteProperty : StyleProperty<Sprite>
	{
		public const string DefaultName = "Sprite";

		public SpriteProperty() : base(DefaultName)
		{
		}
	}
}