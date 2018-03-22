using System;

namespace Stronk.ValueConverters
{
	public class LambdaValueConverter<T> : IValueConverter
	{
		private readonly Func<string, T> _convert;

		public LambdaValueConverter(Func<string, T> convert)
		{
			_convert = convert;
		}

		public bool CanMap(Type target) => typeof(T).IsAssignableFrom(target);

		public object Map(ValueConverterArgs e)
		{
			return _convert(e.Input);
		}
	}
}
