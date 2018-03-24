using System;
using System.Linq;
using Stronk.Policies;
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

		public object Convert(PropertyConversionUnit unit)
		{
			var conversionPolicy = _options.ErrorPolicy.ConversionExceptionPolicy;
			conversionPolicy.BeforeConversion(new ConversionExceptionBeforeArgs
			{
				Logger = _options.WriteLog
			});

			foreach (var converter in unit.Converters)
			{
				var vca = new ValueConverterArgs(
					_options.WriteLog,
					_options.ValueConverters.Where(x => x != converter),
					unit.Property.Type,
					unit.Value
				);

				try
				{
					var converted = converter.Map(vca);
					_options.WriteLog("Converted '{value}' to {typeName}", unit.Value, unit.Property.Type.Name);

					return converted;
				}
				catch (Exception ex)
				{
					_options.WriteLog("Converting '{value}' to {typeName} failed", unit.Value, unit.Property.Type.Name);
					conversionPolicy.OnConversionException(new ConversionExceptionArgs
					{
						Property = unit.Property,
						Value = unit.Value,
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
