using System;

namespace Stronk.ValueConversion
{
	public class FallbackValueConverter : IValueConverter
	{
		public bool CanMap(Type target) => true;

		public object Map(Type target, string value)
		{
			return Convert.ChangeType(value, target);
		}
	}
}
