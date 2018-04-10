using System;
using System.Linq;
using Stronk.Policies;
using Stronk.PropertyWriters;
using Stronk.ValueConverters;

namespace Stronk
{
	public class ConversionProcess
	{
		private readonly IStronkConfig _options;

		public ConversionProcess(IStronkConfig options)
		{
			_options = options;
		}

		public object Convert(PropertyDescriptor property, IValueConverter[] converters, string sourceValue)
		{
			if (sourceValue == null)
				return null;

			var conversionPolicy = new ConversionExceptionPolicy(property, sourceValue);

			foreach (var converter in converters)
			{
				var vca = new ValueConverterArgs(
					_options.WriteLog,
					_options.ValueConverters.Where(x => x != converter),
					property.Type,
					sourceValue
				);

				try
				{
					var converted = converter.Map(vca);
					_options.WriteLog("Converted '{value}' to {typeName}", sourceValue, property.Type.Name);

					return converted;
				}
				catch (Exception ex)
				{
					_options.WriteLog("Converting '{value}' to {typeName} failed", sourceValue, property.Type.Name);
					conversionPolicy.OnConversionException(ex);
				}
			}

			conversionPolicy.AfterConversion();

			return null;
		}
	}
}
