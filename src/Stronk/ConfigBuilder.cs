using System.Linq;
using System.Reflection;
using Stronk.Policies;
using Stronk.PropertyMappers;
using Stronk.PropertyWriters;

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

			var values = properties
				.Select(NewPropertyConversionUnit)
				.Where(SelectedValueIsValid)
				.Where(SelectedConvertersAreValid);

			var converter = new ConversionProcess(_options);
			
			foreach (var descriptor in values)
			{
				var converted = converter.Convert(descriptor);

				try
				{
					descriptor.Property.Assign(target, converted);
				}
				catch (TargetInvocationException e)
				{
					throw e.InnerException ?? e;
				}
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
				_options.Mappers.SelectTypeNames(),
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
				ValueSelectors = _options.Mappers,
				Property = descriptor.Property,
				Converters = descriptor.Converters,
				Sources = descriptor.Sources
			});

			return false;
		}

		private PropertyConversionUnit NewPropertyConversionUnit(PropertyDescriptor property)
		{
			var selectionArgs = new PropertyMapperArgs(_options.WriteLog, _options.ConfigSources, property);

			return new PropertyConversionUnit
			{
				Sources = _options.ConfigSources,
				Property = property,
				Converters = _options.ValueConverters.Where(c => c.CanMap(property.Type)).ToArray(),
				Value = _options.Mappers.Select(x => x.Select(selectionArgs)).FirstOrDefault(v => v != null)
			};
		}
	}
}
