using System;
using System.Collections.Generic;

namespace Stronk.ValueConversion
{
	public interface IValueConverter
	{
		bool CanMap(Type target);
		object Map(ValueConverterArgs e);
	}

	public class ValueConverterArgs
	{
		public Type Target { get; }
		public string Input { get; }
		public IEnumerable<IValueConverter> OtherConverters { get; }

		public ValueConverterArgs(IEnumerable<IValueConverter> others, Type target, string input)
		{
			OtherConverters = others;
			Target = target;
			Input = input;
		}
	}
}
