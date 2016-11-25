using System;

namespace Stronk.ValueConversion
{
	public class EnumValueConverter : IValueConverter
	{
		public bool CanMap(Type target) => target.IsEnum;

		public object Map(ValueConverterArgs e)
		{
			return Enum.Parse(e.Target, e.Input, ignoreCase: true);
		}
	}
}
