using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

namespace Myna.Unity.Themes
{
	public abstract class StyleHelper : MonoBehaviour
	{
		[SerializeField]
		private Theme _theme;

		[SerializeField, StyleHelperClassName]
		private string _className;

		public Theme Theme => _theme;
		public string ClassName => _className;

		public abstract Type StyleType { get; }

		public abstract void ApplyStyle();

		protected bool TryGetStyle<T>(out T style) where T : Style
		{
			if (_theme == null)
			{
				Debug.LogWarning($"{nameof(_theme)} == null");
				style = default;
				return false;
			}

			return _theme.TryGetStyle(_className, out style);
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
	}

	public abstract class StyleHelper<TStyle> : StyleHelper where TStyle : Style
	{
		public override Type StyleType => typeof(TStyle);

		public override sealed void ApplyStyle()
		{
			if (TryGetStyle(out TStyle style))
			{
				ApplyStyle(style);
			}
		}

		protected abstract void ApplyStyle(TStyle style);
	}
}