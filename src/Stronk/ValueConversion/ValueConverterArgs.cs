using System;
using System.Collections.Generic;

namespace Stronk.ValueConversion
{
	public class ValueConverterArgs
	{
		public Action<LogMessage> Logger { get; }
		public IEnumerable<IValueConverter> OtherConverters { get; }
		public Type Target { get; }
		public string Input { get; }

		public ValueConverterArgs(Action<LogMessage> logger, IEnumerable<IValueConverter> others, Type target, string input)
		{
			Logger = logger;
			OtherConverters = others;
			Target = target;
			Input = input;
		}
	}
}
