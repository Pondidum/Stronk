using System;

namespace Stronk.PropertyWriters
{
	public class PropertyWriterArgs
	{
		public Action<string, object[]> Logger { get; }
		public Type TargetType { get; }

		public PropertyWriterArgs(Action<string, object[]> logger, Type targetType)
		{
			Logger = logger;
			TargetType = targetType;
		}
	}
}
