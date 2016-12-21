using System;

namespace Stronk.PropertySelection
{
	public class PropertySelectorArgs
	{
		public Action<string, object[]> Logger { get; }
		public Type TargetType { get; }

		public PropertySelectorArgs(Action<string, object[]> logger, Type targetType)
		{
			Logger = logger;
			TargetType = targetType;
		}
	}
}
