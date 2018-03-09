using System.Linq;
using Stronk.ConfigurationSourcing;
using Stronk.Policies;
using Stronk.PropertySelection;
using Stronk.SourceValueSelection;

namespace Stronk
{
	public class ConfigBuilder
	{
		private readonly StronkOptions _options;

		public ConfigBuilder(StronkOptions options)
		{
			_options = options;
		}

		public void Populate(object target)
		{
			var propertySelectors = _options.PropertySelectors.ToArray();

			_options.WriteLog(
				"Populating '{typeName}', from {sourceTypeName} using\nPropertySelectors: {propertySelectors}\nSourceValueSelectors: {valueSelectors}\nValueConverters: {valueConverters}.",
				target.GetType().Name,
				_options.ConfigSource.GetType().Name,
				propertySelectors.SelectTypeNames(),
				_options.ValueSelectors.SelectTypeNames(),
				_options.ValueConverters.SelectTypeNames());

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
				.Select(property => NewPropertyConversionUnit(_options.ConfigSource, property))
				.Where(SelectedValueIsValid)
				.Where(SelectedConvertersAreValid);

			foreach (var descriptor in values)
			{
				applicator.Apply(descriptor, target);
			}
		}

		private bool SelectedConvertersAreValid(PropertyConversionUnit descriptor)
		{
			if (descriptor.Converters.Any())
				return true;

			_options.WriteLog("Unable to any converters for {typeName} for property {propertyName}", descriptor.Property.Type.Name, descriptor.Property.Name);

			_options.ErrorPolicy.OnConverterNotFound.Handle(new ConverterNotFoundArgs
			{
				AvailableConverters = _options.ValueConverters,
				Property = descriptor.Property
			});

			return false;
		}

		private bool SelectedValueIsValid(PropertyConversionUnit descriptor)
		{
			if (descriptor.Value != null)
				return true;

			_options.WriteLog("Unable to find a value for {propertyName}", descriptor.Property.Name);

			_options.ErrorPolicy.OnSourceValueNotFound.Handle(new SourceValueNotFoundArgs
			{
				ValueSelectors = _options.ValueSelectors,
				Property = descriptor.Property,
				Converters = descriptor.Converters,
				Source = descriptor.Source
			});

			return false;
		}

		private PropertyConversionUnit NewPropertyConversionUnit(IConfigurationSource configSource, PropertyDescriptor property)
		{
			var selectionArgs = new ValueSelectorArgs(_options.Logger, configSource, property);
			
			return new PropertyConversionUnit
			{
				Source = configSource,
				Property = property,
				Converters = _options.ValueConverters.Where(c => c.CanMap(property.Type)).ToArray(),
				Value = _options.ValueSelectors.Select(x => x.Select(selectionArgs)).FirstOrDefault(v => v != null)
			};
		}
	}
}
