using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Myna.Unity.Themes.Editor
{
	[CustomEditor(typeof(StyleHelper), true)]
	public class StyleHelperInspector : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			EditorGUI.BeginChangeCheck();
			base.OnInspectorGUI();

			if (EditorGUI.EndChangeCheck())
			{
				foreach (var target in targets)
				{
					var styleHelper = target as StyleHelper;
					if (styleHelper != null)
					{
						styleHelper.ApplyStyle();
					}
				}
			}
		}
	}
}