using System;
using System.Collections.Generic;

namespace Stronk.ValueConverters
{
	public class ValueConverterArgs
	{
		public Action<string, object[]> Logger { get; }
		public IEnumerable<IValueConverter> OtherConverters { get; }
		public Type Target { get; }
		public string Input { get; }

		public ValueConverterArgs(Action<string, object[]> logger, IEnumerable<IValueConverter> others, Type target, string input)
		{
			Logger = logger;
			OtherConverters = others;
			Target = target;
			Input = input;
		}
	}
}
