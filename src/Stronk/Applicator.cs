using System;
using System.Linq;
using Stronk.Policies;
using Stronk.ValueConversion;

namespace Stronk
{
	public class Applicator
	{
		private readonly IStronkConfig _options;

		public Applicator(IStronkConfig options)
		{
			_options = options;
		}

		public void Apply(PropertyConversionUnit unit, object target)
		{
			var conversionPolicy = _options.ErrorPolicy.ConversionExceptionPolicy;
			conversionPolicy.BeforeConversion(new ConversionExceptionBeforeArgs
			{
				Logger = _options.Logger
			});

			foreach (var converter in unit.Converters)
			{
				var vca = new ValueConverterArgs(
					_options.Logger,
					_options.ValueConverters.Where(x => x != converter),
					unit.Property.Type,
					unit.Value
				);

				try
				{
					_options.WriteLog("Converting '{value}' and assigning to {typeName}.{propertyName}", unit.Value, target.GetType().Name, unit.Property.Name);

					var converted = converter.Map(vca);
					unit.Property.Assign(target, converted);

					return;
				}
				catch (Exception ex)
				{
					conversionPolicy.OnConversionException(new ConversionExceptionArgs
					{
						Property = unit.Property,
						Value = unit.Value,
						Logger = _options.Logger,
						Exception = ex
					});
				}
			}

			conversionPolicy.AfterConversion(new ConversionExceptionAfterArgs
			{
				Logger = _options.Logger
			});
		}
	}
}
