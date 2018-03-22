using System;

namespace Stronk.ValueConverters
{
	public interface IValueConverter
	{
		bool CanMap(Type target);
		object Map(ValueConverterArgs e);
	}
}
