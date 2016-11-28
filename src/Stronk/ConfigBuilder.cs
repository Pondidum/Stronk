using System.Linq;
using Stronk.PropertySelection;
using Stronk.ValueConversion;
using Stronk.ValueSelection;

namespace Stronk
{
	public class ConfigBuilder
	{
		private readonly IStronkConfiguration _configuration;

		public ConfigBuilder(IStronkConfiguration configuration)
		{
			_configuration = configuration;
		}

		public void Populate(object target, IConfigurationSource configSource)
		{
			var valueSelectors = _configuration.ValueSelectors.ToArray();
			var converters = _configuration.ValueConverters.ToArray();

			var properties = _configuration
				.PropertySelectors
				.SelectMany(selector => selector.Select(target.GetType()));

			var args = new ValueSelectorArgs(configSource);

			foreach (var property in properties)
			{
				var value = GetValueFromSource(valueSelectors, args, property);

				if (value == null)
					continue;

				var converter = GetValueConverter(converters, property);

				if (converter == null)
					continue;

				var vca = new ValueConverterArgs(
					converters.Where(x => x != converter),
					property.Type,
					value
				);

				var converted = converter.Map(vca);

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

		private static string GetValueFromSource(ISourceValueSelector[] sourceValueSelectors, ValueSelectorArgs args, PropertyDescriptor property)
		{
			foreach (var filter in sourceValueSelectors)
			{
				var value = filter.Select(args.With(property));

				if (value != null)
					return value;
			}

			return null;
		}
	}
}
