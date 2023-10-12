using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StylePropertyListAttribute : PropertyAttribute
{
	public System.Type StyleType { get; private set; }

	public StylePropertyListAttribute(System.Type styleType)
	{
		StyleType = styleType;
	}
}