using System;

namespace Myna.Unity.Themes
{
	[AttributeUsage(AttributeTargets.Class)]
	public class StylePropertyAttribute : Attribute
	{
		public string[] Names { get; private set; } = Array.Empty<string>();

		public StylePropertyAttribute(params string[] names)
		{
			Names = names;
		}
	}
}