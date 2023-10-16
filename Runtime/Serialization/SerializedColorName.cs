using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Myna.Unity.Themes
{
	[Serializable]
	public class SerializedColorName
	{
		public const string NamePropertyName = nameof(_name);
		public const string GuidPropertyName = nameof(_guid);

		[SerializeField]
		private string _name = string.Empty;

		[SerializeField, HideInInspector]
		private string _guid = string.Empty;

		public string Name => _name;
		public string Guid => _guid;

		public bool TryGetColor(Theme theme, out Color color)
		{
			return theme.TryGetColorByGuid(_guid, out color) || theme.TryGetColor(_name, out color);
		}
	}
}