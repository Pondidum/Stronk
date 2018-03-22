using System;
using System.Collections.Generic;
using System.Text;
using Stronk.PropertyWriters;
using Stronk.ValueConverters;

namespace Stronk.Policies
{
	public class ConverterNotFoundException : Exception
	{
		public ConverterNotFoundException(IEnumerable<IValueConverter> converters, PropertyDescriptor property)
			:base(BuildMessage(converters, property))
		{
		}

		private static string BuildMessage(IEnumerable<IValueConverter> converters, PropertyDescriptor property)
		{
			var sb = new StringBuilder();
			sb.AppendLine($"None of the following converters were suitable to handle property '{property.Name}' of type '{property.Type.Name}':");
			sb.AppendLine();

			foreach (var converter in converters)
				sb.AppendLine(converter.GetType().Name);

			return sb.ToString();
		}
	}
}
