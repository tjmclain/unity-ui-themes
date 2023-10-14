using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class ClassNameDropdownAttribute : PropertyAttribute
{
	public string ThemePropertyName { get; private set; }

	public ClassNameDropdownAttribute(string themePropertyName = "")
	{
		ThemePropertyName = themePropertyName;
	}
}