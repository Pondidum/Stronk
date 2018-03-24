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
			var conversionPolicy = _options.ErrorPolicy.ConversionExceptionPolicy;
			conversionPolicy.BeforeConversion(new ConversionExceptionBeforeArgs
			{
				Logger = _options.WriteLog
			});

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
					conversionPolicy.OnConversionException(new ConversionExceptionArgs
					{
						Property = property,
						Value = sourceValue,
						Logger = _options.WriteLog,
						Exception = ex
					});
				}
			}

			conversionPolicy.AfterConversion(new ConversionExceptionAfterArgs
			{
				Logger = _options.WriteLog
			});

			return null;
		}
	}
}
