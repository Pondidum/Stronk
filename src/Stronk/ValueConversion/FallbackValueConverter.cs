using System;

namespace Stronk.ValueConversion
{
	public class FallbackValueConverter : IValueConverter
	{
		public bool CanMap(Type target) => true;

		public object Map(ValueConverterArgs e)
		{
			return Convert.ChangeType(e.Input, e.Target);
		}
	}
}
