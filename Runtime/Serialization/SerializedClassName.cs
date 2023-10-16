using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Myna.Unity.Themes
{
	[Serializable]
	public class SerializedClassName
	{
		public const string NamePropertyName = nameof(_name);
		public const string GuidPropertyName = nameof(_guid);

		[SerializeField]
		private string _name = Theme.DefaultClassName;

		[SerializeField]
		private string _guid;

		public string Name => _name;
		public string Guid => _guid;

		public bool TryGetStyle(Theme theme, out Style style)
		{
			return theme.TryGetStyleByGuid(_guid, out style) || theme.TryGetStyle(_name, out style);
		}
	}
}