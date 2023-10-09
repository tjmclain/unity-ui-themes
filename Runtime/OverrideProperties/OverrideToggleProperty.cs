using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OverrideToggleProperty : OverrideProperty<bool>
{
	public override bool Value => Enabled;
	public override string ValuePropertyName => EnabledPropertyName;
}