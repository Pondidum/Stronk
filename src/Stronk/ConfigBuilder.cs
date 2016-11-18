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

		public void Populate(object target, IConfigurationSource configSource = null)
		{
			var valueSelectors = _configuration.ValueSelectors.ToArray();
			var converters = _configuration.ValueConverters.ToArray();

			var properties = _configuration
				.PropertySelectors
				.SelectMany(selector => selector.Select(target.GetType()));

			var args = new ValueSelectorArgs(configSource ?? new AppConfigSource());

			foreach (var property in properties)
			{
				var value = GetValueFromSource(valueSelectors, args, property);

				if (value == null)
					continue;

				IValueConverter converter = null;

				foreach (var valueConverter in converters)
				{
					if (valueConverter.CanMap(property.Type) == false)
						continue;

					converter = valueConverter;
					break;
				}

				if (converter == null)
					continue;

				var converted = converter.Map(property.Type, value);

				property.Assign(target, converted);
			}
		}

		private static string GetValueFromSource(IValueSelector[] valueSelectors, ValueSelectorArgs args, PropertyDescriptor property)
		{
			foreach (var filter in valueSelectors)
			{
				var value = filter.Select(args.With(property));

				if (value != null)
					return value;
			}

			return null;
		}
	}
}
