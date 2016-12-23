﻿using System;
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

			var args = new ValueSelectorArgs(_options.Logger, configSource);

			foreach (var property in properties)
			{
				var selectorArgs = args.With(property);
				var value = GetValueFromSource(valueSelectors, selectorArgs);

				if (value == null)
				{
					WriteLog("Unable to find a value for {propertyName}", property.Name);
					_options.ErrorPolicy.OnSourceValueNotFound.Handle(valueSelectors, selectorArgs);
					continue;
				}

				var converters = GetValueConverters(availableConverters, property);

				if (converters.Any() == false)
				{
					WriteLog("Unable to any converters for {typeName} for property {propertyName}", property.Type.Name, property.Name);

					_options.ErrorPolicy.OnConverterNotFound.Handle(new ConverterNotFoundArgs
					{
						AvailableConverters = availableConverters,
						Property = property
					});

					continue;
				}

				ApplyConversion(availableConverters, converters, target, property, value);
			}
		}

		private void WriteLog(string template, params object[] args)
		{
			_options.Logger(new LogMessage(template, args));
		}

		private void ApplyConversion(IValueConverter[] availableConverters, IEnumerable<IValueConverter> chosenConverters, object target, PropertyDescriptor property, string value)
		{
			var conversionPolicy = _options.ErrorPolicy.ConversionPolicy;
			conversionPolicy.BeforeConversion();

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