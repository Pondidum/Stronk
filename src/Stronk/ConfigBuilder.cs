using System;
using System.Collections.Generic;
using System.Linq;
using Stronk.Policies;
using Stronk.PropertySelection;
using Stronk.SourceValueSelection;
using Stronk.ValueConversion;

namespace Stronk
{
	public class ConfigBuilder
	{
		private readonly StronkOptions _options;

		public ConfigBuilder(StronkOptions options)
		{
			_options = options;
		}

		public void Populate(object target, IConfigurationSource configSource)
		{
			var valueSelectors = _options.ValueSelectors.ToArray();
			var availableConverters = _options.ValueConverters.ToArray();
			var propertySelectors = _options.PropertySelectors.ToArray();

			WriteLog(
				"Populating '{typeName}', from {sourceTypeName} using\nPropertySelectors: {propertySelectors}\nSourceValueSelectors: {valueSelectors}\nValueConverters: {valueConverters}.",
				target.GetType().Name,
				configSource.GetType().Name,
				propertySelectors.SelectTypeNames(),
				valueSelectors.SelectTypeNames(),
				availableConverters.SelectTypeNames());

			var propertySelectorArgs = new PropertySelectorArgs(
				_options.Logger,
				target.GetType());

			var properties = propertySelectors
				.SelectMany(selector => selector.Select(propertySelectorArgs))
				.ToArray();

			WriteLog(
				"Selected {count} properties to populate: {properties}",
				properties.Length,
				properties.Select(p => p.Name));

			var values = properties
				.Select(property =>  new ValueSelectorArgs(_options.Logger, configSource, property))
				.Select(args => new
				{
					Property = args.Property,
					Value = GetValueFromSource(valueSelectors, args)
				});

			foreach (var descriptor in values)
			{
				if (descriptor.Value == null)
				{
					WriteLog("Unable to find a value for {propertyName}", descriptor.Property.Name);

					_options.ErrorPolicy.OnSourceValueNotFound.Handle(new SourceValueNotFoundArgs
					{
						ValueSelectors = valueSelectors,
						Property = descriptor.Property
					});

					continue;
				}

				var converters = GetValueConverters(availableConverters, descriptor.Property);

				if (converters.Any() == false)
				{
					WriteLog("Unable to any converters for {typeName} for property {propertyName}", descriptor.Property.Type.Name, descriptor.Property.Name);

					_options.ErrorPolicy.OnConverterNotFound.Handle(new ConverterNotFoundArgs
					{
						AvailableConverters = availableConverters,
						Property = descriptor.Property
					});

					continue;
				}

				ApplyConversion(availableConverters, converters, target, descriptor.Property, descriptor.Value);
			}
		}

		private void WriteLog(string template, params object[] args)
		{
			_options.Logger(new LogMessage(template, args));
		}

		private void ApplyConversion(IValueConverter[] availableConverters, IEnumerable<IValueConverter> chosenConverters, object target, PropertyDescriptor property, string value)
		{
			var conversionPolicy = _options.ErrorPolicy.ConversionExceptionPolicy;
			conversionPolicy.BeforeConversion(new ConversionExceptionBeforeArgs
			{
				Logger = _options.Logger
			});

			foreach (var converter in chosenConverters)
			{
				var vca = new ValueConverterArgs(
					_options.Logger,
					availableConverters.Where(x => x != converter),
					property.Type,
					value
				);

				try
				{
					WriteLog("Converting '{value}' and assigning to {typeName}.{propertyName}", value, target.GetType().Name, property.Name);

					var converted = converter.Map(vca);
					property.Assign(target, converted);

					return;
				}
				catch (Exception ex)
				{
					conversionPolicy.OnConversionException(new ConversionExceptionArgs
					{
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