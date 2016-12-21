using System;

namespace Stronk.PropertySelection
{
	public class PropertySelectorArgs
	{
		public Type TargetType { get; }

		public PropertySelectorArgs(Type targetType)
		{
			TargetType = targetType;
		}
	}
}
