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
		private SerializedClassName _className = new();

		public Theme Theme => _theme;
		public string ClassName => _className.Name;

		public virtual void ApplyStyle()
		{
			if (TryGetStyle(out Style style))
			{
				ApplyStyle(style);
			}
		}

		protected virtual bool TryGetStyle(out Style style)
		{
			if (_theme == null)
			{
				Debug.LogWarning($"{nameof(_theme)} == null");
				style = default;
				return false;
			}

			return _className.TryGetStyle(_theme, out style);
		}

		protected virtual void OnThemeSettingsChanged(Theme theme)
		{
			if (theme == _theme)
			{
				ApplyStyle();
			}
		}

		#region MonoBehaviour

		protected virtual void OnEnable()
		{
			Theme.SettingsChanged.AddListener(OnThemeSettingsChanged);
			ApplyStyle();
		}

		protected virtual void OnDisable()
		{
			Theme.SettingsChanged.RemoveListener(OnThemeSettingsChanged);
		}

#if UNITY_EDITOR

		protected virtual void OnValidate()
		{
		}

#endif

		#endregion MonoBehaviour

		protected abstract void ApplyStyle(Style style);
	}
}