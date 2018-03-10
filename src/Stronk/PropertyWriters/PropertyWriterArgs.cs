using System;

namespace Stronk.PropertyWriters
{
	public class PropertyWriterArgs
	{
		public Action<LogMessage> Logger { get; }
		public Type TargetType { get; }

		public PropertyWriterArgs(Action<LogMessage> logger, Type targetType)
		{
			Logger = logger;
			TargetType = targetType;
		}
	}
}
