using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Myna.Unity.Themes
{
	public abstract class StyleHelper : MonoBehaviour
	{
		[SerializeField, ThemeReference]
		private Theme _theme;

		[SerializeField, ClassNameDropdown(nameof(_theme))]
		private string _className;

		#region IStyleHelper

		public Theme Theme => _theme;
		public string ClassName => _className;

		public void ApplyStyle()
		{
			if (TryGetStyle(out Style style))
			{
				ApplyStyle(style);
			}
		}

		#endregion IStyleHelper

		protected bool TryGetStyle(out Style style)
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

		protected abstract void ApplyStyle(Style style);
	}
}