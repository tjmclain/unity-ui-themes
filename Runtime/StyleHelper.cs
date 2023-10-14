using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Myna.Unity.Themes
{
	public abstract class StyleHelper : MonoBehaviour
	{
		public abstract Theme Theme { get; }
		public abstract string ClassName { get; }
		public abstract Type StyleType { get; }

		public abstract void ApplyStyle();
	}

	public abstract class StyleHelper<TStyle> : StyleHelper where TStyle : Style
	{
		[SerializeField, ThemeReference]
		private Theme _theme;

		[SerializeField, ClassNameDropdown]
		private string _className;

		#region IStyleHelper

		public override Theme Theme => _theme;
		public override string ClassName => _className;
		public override Type StyleType => typeof(TStyle);

		public override void ApplyStyle()
		{
			if (TryGetStyle(out TStyle style))
			{
				ApplyStyle(style);
			}
		}

		#endregion IStyleHelper

		protected bool TryGetStyle(out TStyle style)
		{
			if (_theme == null)
			{
				Debug.LogWarning($"{nameof(_theme)} == null");
				style = default;
				return false;
			}

			return _theme.TryGetStyle<TStyle>(_className, out style);
		}

		#region MonoBehaviour

		protected virtual void OnEnable()
		{
			ApplyStyle();
		}

		protected virtual void OnValidate()
		{
			ApplyStyle();
		}

		#endregion MonoBehaviour

		protected abstract void ApplyStyle(TStyle style);
	}
}