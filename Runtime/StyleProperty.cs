using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Myna.Unity.Themes
{
	[System.Serializable]
	public class StyleProperty
	{
		[SerializeField, HideInInspector]
		protected string _name;

		public string Name
		{
			get => _name;
			set => _name = value;
		}

		public virtual object GetValue(Theme theme)
		{
			throw new System.NotImplementedException();
		}
	}

	[System.Serializable]
	public class StyleProperty<T> : StyleProperty
	{
		[SerializeField]
		private T _value;

		public override object GetValue(Theme theme)
		{
			return _value;
		}
	}
}