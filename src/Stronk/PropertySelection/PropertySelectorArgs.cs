using System;

namespace Stronk.PropertySelection
{
	public class PropertySelectorArgs
	{
		public Action<LogMessage> Logger { get; }
		public Type TargetType { get; }

		public PropertySelectorArgs(Action<LogMessage> logger, Type targetType)
		{
			Logger = logger;
			TargetType = targetType;
		}
	}
}
