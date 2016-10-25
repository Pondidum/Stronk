using System;

namespace Stronk.ValueConversion
{
	public class LambdaValueConverter<T> : IValueConverter
	{
		private readonly Func<string, T> _convert;

		public LambdaValueConverter(Func<string, T> convert)
		{
			_convert = convert;
		}

		public bool CanMap(Type target) => typeof(T).IsAssignableFrom(target);

		public object Map(Type target, string value)
		{
			return _convert(value);
		}
	}
}
