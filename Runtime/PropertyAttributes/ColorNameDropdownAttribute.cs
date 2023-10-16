using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.AttributeUsage(System.AttributeTargets.Field)]
public class ColorNameDropdownAttribute : PropertyAttribute
{
	public string ColorSchemePropertyName { get; set; }
	public string DefaultColorPropertyName { get; set; }

	public ColorNameDropdownAttribute(string colorSchemePropertyName, string defaultColorPropertyName = "")
	{
		ColorSchemePropertyName = colorSchemePropertyName;
		DefaultColorPropertyName = defaultColorPropertyName;
	}
}