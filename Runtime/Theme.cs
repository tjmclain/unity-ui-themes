using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Myna.Unity.Themes
{
	[CreateAssetMenu(fileName = "Theme", menuName = "UI Themes/Theme")]
	public class Theme : ScriptableObject
	{
		public ThemeColors DefaultThemeColors;

		public List<ThemeStyle> Styles = new();

		public bool TryGetThemeStyle(System.Type componentType, out ThemeStyle style)
			=> TryGetThemeStyle(componentType, string.Empty, out style);

		public bool TryGetThemeStyle(System.Type componentType, string className, out ThemeStyle style)
		{
			if (componentType == null)
			{
				Debug.LogWarning($"{nameof(componentType)} == null", this);
				style = default;
				return false;
			}

			int index = Styles.FindIndex(x => x.ComponentType == componentType && x.ClassName == className);
			if (index < 0)
			{
				style = default;
				return false;
			}

			style = Styles[index];
			return true;
		}
	}
}