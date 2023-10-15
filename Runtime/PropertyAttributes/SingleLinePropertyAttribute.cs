using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleLinePropertyAttribute : PropertyAttribute
{
	public string LabelPropertyName { get; set; }
	public string ValuePropertyName { get; set; }

	public SingleLinePropertyAttribute(string labelPropertyName, string valuePropertyName)
	{
		LabelPropertyName = labelPropertyName;
		ValuePropertyName = valuePropertyName;
	}
}