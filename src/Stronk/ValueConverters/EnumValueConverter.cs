using System;

namespace Stronk.ValueConverters
{
	public class EnumValueConverter : IValueConverter
	{
		public bool CanMap(Type target) => target.IsEnum;

		public object Map(ValueConverterArgs e)
		{
			try
			{
				var value =  Enum.Parse(e.Target, e.Input, ignoreCase: true);

				return Enum.IsDefined(e.Target, value)
					? value
					: null;
			}
			catch (ArgumentException)
			{
				return null;
			}
			
		}
	}
}
