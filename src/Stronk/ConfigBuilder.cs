using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Stronk.Policies;
using Stronk.PropertySelection;
using Stronk.SourceValueSelection;
using Stronk.ValueConversion;

namespace Stronk
{
	public class ConfigBuilder
	{
		private readonly IStronkOptions _options;

		public ConfigBuilder(IStronkOptions options)
		{
			_options = options;
		}

		public void Populate(object target, IConfigurationSource configSource)
		{
			var valueSelectors = _options.ValueSelectors.ToArray();
			var availableConverters = _options.ValueConverters.ToArray();

			var properties = _options
				.PropertySelectors
				.SelectMany(selector => selector.Select(target.GetType()));

			var args = new ValueSelectorArgs(configSource);

			foreach (var property in properties)
			{
				var selectorArgs = args.With(property);
				var value = GetValueFromSource(valueSelectors, selectorArgs);

				if (value == null)
				{
					_options.ErrorPolicy.OnSourceValueNotFound.Handle(valueSelectors, selectorArgs);
					continue;
				}

				var converters = GetValueConverters(availableConverters, property);

				if (converters.Any() == false && _options.ErrorPolicy.OnConverterNotFound == PolicyActions.ThrowException)
					throw new ConverterNotFoundException(availableConverters, property);

				if (converters.Any() == false)
					continue;

				ApplyConversion(availableConverters, converters, target, property, value);
			}
		}

		private void ApplyConversion(IValueConverter[] availableConverters, IEnumerable<IValueConverter> chosenConverters, object target, PropertyDescriptor property, string value)
		{
			var conversionPolicy = _options.ErrorPolicy.ConversionPolicy;
			conversionPolicy.BeforeConversion();

			foreach (var converter in chosenConverters)
			{
				var vca = new ValueConverterArgs(
					availableConverters.Where(x => x != converter),
					property.Type,
					value
				);

				try
				{
					var converted = converter.Map(vca);
					property.Assign(target, converted);

					return;
				}
				catch (Exception ex)
				{
					conversionPolicy.OnConversionException(ex);
				}
			}

			conversionPolicy.AfterConversion();
		}

		private static IValueConverter[] GetValueConverters(IValueConverter[] converters, PropertyDescriptor property)
		{
			return converters
				.Where(c => c.CanMap(property.Type))
				.ToArray();
		}

		private static string GetValueFromSource(ISourceValueSelector[] sourceValueSelectors, ValueSelectorArgs args)
		{
			foreach (var filter in sourceValueSelectors)
			{
				var value = filter.Select(args);

				if (value != null)
					return value;
			}

			return null;
		}
	}
}
