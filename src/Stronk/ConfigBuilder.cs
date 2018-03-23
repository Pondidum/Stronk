using System;
using System.Linq;
using System.Reflection;
using Stronk.Policies;
using Stronk.PropertyMappers;
using Stronk.PropertyWriters;
using Stronk.ValueConverters;

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

			foreach (var descriptor in values)
			{
				var converted = Convert(descriptor);

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

		private object Convert(PropertyConversionUnit unit)
		{
			var conversionPolicy = _options.ErrorPolicy.ConversionExceptionPolicy;
			conversionPolicy.BeforeConversion(new ConversionExceptionBeforeArgs
			{
				Logger = _options.WriteLog
			});

			foreach (var converter in unit.Converters)
			{
				var vca = new ValueConverterArgs(
					_options.WriteLog,
					_options.ValueConverters.Where(x => x != converter),
					unit.Property.Type,
					unit.Value
				);

				try
				{
					//_options.WriteLog("Converted '{value}' and {typeName}", unit.Value, unit.Property.Type.Name);
					return converter.Map(vca);
				}
				catch (Exception ex)
				{
					conversionPolicy.OnConversionException(new ConversionExceptionArgs
					{
						Property = unit.Property,
						Value = unit.Value,
						Logger = _options.WriteLog,
						Exception = ex
					});
				}
			}

			conversionPolicy.AfterConversion(new ConversionExceptionAfterArgs
			{
				Logger = _options.WriteLog
			});

			return null;
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
