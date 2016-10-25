using System;

namespace Stronk.ValueConversion
{
	public class EnumValueConverter : IValueConverter
	{
		public bool CanMap(Type target) => target.IsEnum;

		public object Map(Type target, string value)
		{
			return Enum.Parse(target, value, ignoreCase: true);
		}
	}
}
