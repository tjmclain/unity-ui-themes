using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.AttributeUsage(System.AttributeTargets.Field)]
public class ColorNameAttribute : PropertyAttribute
{
	public string ColorSchemePropertyName { get; private set; }

	public ColorNameAttribute(string colorSchemePropertyName)
	{
	}
}