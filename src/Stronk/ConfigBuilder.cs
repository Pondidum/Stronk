using System.Linq;
using Stronk.Policies;
using Stronk.PropertyWriters;
using Stronk.SourceValueSelection;

namespace Stronk
{
	public class ConfigBuilder
	{
		private readonly IStronkConfig _options;

		public ConfigBuilder(IStronkConfig options)
		{
			_options = options;
		}

		public void Populate(object target)
		{
			LogPopulationStart(target);

			var writerArgs = new PropertyWriterArgs(_options.WriteLog, target.GetType());

			var properties = _options
				.PropertyWriters
				.SelectMany(writer => writer.Select(writerArgs))
				.ToArray();

			_options.WriteLog(
				"Selected {count} properties to populate: {properties}",
				properties.Length,
				properties.Select(p => p.Name));

			var applicator = new Applicator(_options);

			var values = properties
				.Select(NewPropertyConversionUnit)
				.Where(SelectedValueIsValid)
				.Where(SelectedConvertersAreValid);

			foreach (var descriptor in values)
			{
				applicator.Apply(descriptor, target);
			}
		}

		private void LogPopulationStart(object target)
		{
			var message = "Populating '{typeName}'...\n" +
			              "Reading from: {sourceTypeName}\n" +
			              "Writing to: {propertyWriters}\n" +
			              "Matching keys to properties with: {valueSelectors}\n" +
			              "Converting values using: {valueConverters}.";

			_options.WriteLog(
				message,
				target.GetType().Name,
				_options.ConfigSources.SelectTypeNames(),
				_options.PropertyWriters.SelectTypeNames(),
				_options.ValueSelectors.SelectTypeNames(),
				_options.ValueConverters.SelectTypeNames());
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
				Sources = descriptor.Sources
			});

			return false;
		}

		private PropertyConversionUnit NewPropertyConversionUnit(PropertyDescriptor property)
		{
			var selectionArgs = new ValueSelectorArgs(_options.WriteLog, _options.ConfigSources, property);

			return new PropertyConversionUnit
			{
				Sources = _options.ConfigSources,
				Property = property,
				Converters = _options.ValueConverters.Where(c => c.CanMap(property.Type)).ToArray(),
				Value = _options.ValueSelectors.Select(x => x.Select(selectionArgs)).FirstOrDefault(v => v != null)
			};
		}
	}
}
