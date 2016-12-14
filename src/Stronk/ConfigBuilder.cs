using System;
using System.Linq;
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
			var converters = _options.ValueConverters.ToArray();

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

				var converter = GetValueConverter(converters, property);

				if (converter == null)
					if (_options.ErrorPolicy.OnConverterNotFound == PolicyActions.ThrowException)
						throw new ConverterNotFoundException(converters, property);
					else
						continue;

				var vca = new ValueConverterArgs(
					converters.Where(x => x != converter),
					property.Type,
					value
				);

				object converted;

				try
				{
					converted = converter.Map(vca);
				}
				catch (Exception ex)
				{
					if (_options.ErrorPolicy.OnConverterException == ConverterExceptionPolicy.ThrowException)
						throw new ValueConversionException("Error converting",new [] { ex });
					else
						continue;
				}

				property.Assign(target, converted);
			}
		}

		private static IValueConverter GetValueConverter(IValueConverter[] converters, PropertyDescriptor property)
		{
			foreach (var valueConverter in converters)
			{
				if (valueConverter.CanMap(property.Type) == false)
					continue;

				return valueConverter;
			}

			return null;
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
