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

			_options.WriteLog(
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

			_options.WriteLog(
				"Selected {count} properties to populate: {properties}",
				properties.Length,
				properties.Select(p => p.Name));
			var applicator = new Applicator(_options);

			var values = properties
				.Select(property => new PropertyConversionUnit
				{
					Property = property,
					Converters = GetValueConverters(availableConverters, property),
					Value = GetValueFromSource(valueSelectors, new ValueSelectorArgs(_options.Logger, configSource, property))
				})
				.Where(descriptor =>
				{
					if (descriptor.Value != null)
						return true;

					_options.WriteLog("Unable to find a value for {propertyName}", descriptor.Property.Name);

					_options.ErrorPolicy.OnSourceValueNotFound.Handle(new SourceValueNotFoundArgs
					{
						ValueSelectors = valueSelectors,
						Property = descriptor.Property
					});

					return false;
				})
				.Where(descriptor =>
				{
					if (descriptor.Converters.Any())
						return true;

					_options.WriteLog("Unable to any converters for {typeName} for property {propertyName}", descriptor.Property.Type.Name, descriptor.Property.Name);

					_options.ErrorPolicy.OnConverterNotFound.Handle(new ConverterNotFoundArgs
					{
						AvailableConverters = availableConverters,
						Property = descriptor.Property
					});

					return false;
				});

			foreach (var descriptor in values)
			{
				applicator.Apply(descriptor, target);
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
