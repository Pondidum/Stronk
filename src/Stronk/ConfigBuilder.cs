﻿using System;
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
				var value = GetValueFromSource(valueSelectors, args.With(property));

				if (value == null)
					if (_options.ErrorPolicy.OnSourceValueNotFound == PolicyActions.ThrowException)
						throw new SourceValueNotFoundException(valueSelectors, property);
					else
						continue;

				var converters = GetValueConverters(availableConverters, property);

				if (converters.Any() == false)
					if (_options.ErrorPolicy.OnConverterNotFound == PolicyActions.ThrowException)
						throw new ConverterNotFoundException(availableConverters, property);
					else
						continue;

				ApplyConversion(availableConverters, converters, target, property, value);
			}
		}

		private void ApplyConversion(IValueConverter[] availableConverters, IEnumerable<IValueConverter> chosenConverters, object target, PropertyDescriptor property, string value)
		{
			var exceptions = new List<Exception>();

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
					if (_options.ErrorPolicy.OnConverterException == ConverterExceptionPolicy.ThrowException)
						throw new ValueConversionException("Error converting", new[] { ex });

					exceptions.Add(ex);
				}
			}
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
