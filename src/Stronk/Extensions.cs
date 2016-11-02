using System.Linq;
using System.Runtime.CompilerServices;
using Stronk.PropertySelection;
using Stronk.ValueConversion;
using Stronk.ValueSelection;

namespace Stronk
{
	public static class Extensions
	{
		public static void FromAppConfig(this object target)
		{
			target.FromAppConfig(new StronkConfiguration());
		}

		public static void FromAppConfig(this object target, IStronkConfiguration configuration, IConfigurationProvider configProvider = null)
		{
			var propertySelectors = configuration.PropertySelectors;
			var valueSelectors = configuration.ValueSelectors.ToArray();
			var converters = configuration.ValueConverters.ToArray();

			var properties = propertySelectors
				.SelectMany(selector => selector.Select(target.GetType()));

			var args = new ValueSelectorArgs(configProvider ?? new AppConfigProvider());

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
