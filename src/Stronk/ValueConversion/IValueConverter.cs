using System;

namespace Stronk.ValueConversion
{
	public interface IValueConverter
	{
		bool CanMap(Type target);
		object Map(ValueConverterArgs e);
	}
}
