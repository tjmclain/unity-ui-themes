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

		[SerializeField]
		private string _className;

		public Theme Theme => _theme;
		public string ClassName => _className;

		protected abstract System.Type ComponentType { get; }

		public abstract void ApplyStyle();

		protected bool TryGetStyle<T>(out T style) where T : Style
		{
			if (_theme == null)
			{
				Debug.LogWarning($"{nameof(_theme)} == null");
				style = default;
				return false;
			}

			return !_theme.TryGetStyle(ComponentType, _className, out style);
		}

		#region MonoBehaviour

		protected virtual void OnEnable()
		{
			ApplyStyle();
		}

		#endregion MonoBehaviour
	}
}