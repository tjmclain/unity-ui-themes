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

		public abstract Type ComponentType { get; }

		public abstract void ApplyStyle();

		protected bool TryGetStyle<T>(out T style) where T : Style
		{
			if (_theme == null)
			{
				Debug.LogWarning($"{nameof(_theme)} == null");
				style = default;
				return false;
			}

			return _theme.TryGetStyle(ComponentType, _className, out style);
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

	public abstract class StyleHelper<TStyle, TComponent> : StyleHelper
		where TStyle : Style
		where TComponent : Component
	{
		[SerializeField, StyleHelperComponent]
		private TComponent _component;

		public TComponent Component => _component;

		public override Type ComponentType => typeof(TComponent);

		public override sealed void ApplyStyle()
		{
			if (TryGetStyle(out TStyle style))
			{
				//Debug.Log($"ApplyStyle: {style}", this);
				ApplyStyle(style);
			}
		}

		protected abstract void ApplyStyle(TStyle style);

		protected override void OnValidate()
		{
			if (_component == null)
			{
				TryGetComponent(out _component);
			}

			base.OnValidate();
		}
	}
}