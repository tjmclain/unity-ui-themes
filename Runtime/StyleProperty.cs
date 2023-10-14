using System;
using UnityEngine;

namespace Myna.Unity.Themes
{
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

	[Serializable]
	public class AlphaProperty : StyleProperty<float>
	{
	}

	[Serializable]
	public class FontAssetProperty : StyleProperty<TMPro.TMP_FontAsset>
	{
	}

	[Serializable]
	public class FontStylesProperty : StyleProperty<TMPro.FontStyles>
	{
	}

	[Serializable]
	public class ImageTypeProperty : StyleProperty<ImageType>
	{
	}

	[Serializable]
	public class SpriteProperty : StyleProperty<Sprite>
	{
	}
}