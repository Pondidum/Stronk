using System;
using System.Text;
using Stronk.PropertySelection;
using Stronk.ValueConversion;

namespace Stronk
{
	public class ConverterNotFoundException : Exception
	{
		public ConverterNotFoundException(IValueConverter[] converters, PropertyDescriptor property)
			:base(BuildMessage(converters, property))
		{
		}

		private static string BuildMessage(IValueConverter[] converters, PropertyDescriptor property)
		{
			var sb = new StringBuilder();
			sb.AppendLine($"None of the following converters was suitable to handle property '{property.Name}' of type '{property.Type.Name}':");
			sb.AppendLine();

			foreach (var converter in converters)
				sb.AppendLine(converter.GetType().Name);

			return sb.ToString();
		}
	}
}
