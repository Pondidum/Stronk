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

			var converter = new ConversionProcess(_options);

			foreach (var property in properties)
			{
				var selectionArgs = new PropertyMapperArgs(_options.WriteLog, _options.ConfigSources, property);

				var validConverters = _options.ValueConverters.Where(c => c.CanMap(property.Type)).ToArray();
				var valueToUse = _options.Mappers.Select(x => x.Select(selectionArgs)).FirstOrDefault(v => v != null);

				if (SelectedValueIsValid(property, validConverters, valueToUse) && SelectedConvertersAreValid(property, validConverters))
				{
					
					var converted = converter.Convert(property, validConverters, valueToUse);
					
					try
					{
						property.Assign(target, converted);
					}
					catch (TargetInvocationException e)
					{
						throw e.InnerException ?? e;
					}
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

		private bool SelectedConvertersAreValid(PropertyDescriptor property, IValueConverter[] converters)
		{
			if (converters.Any())
				return true;

			_options.WriteLog("Unable to any converters for {typeName} for property {propertyName}", property.Type.Name, property.Name);

			_options.ErrorPolicy.OnConverterNotFound.Handle(new ConverterNotFoundArgs
			{
				AvailableConverters = _options.ValueConverters,
				Property = property
			});

			return false;
		}

		private bool SelectedValueIsValid(PropertyDescriptor property, IValueConverter[] converters, string sourceValue)
		{
			if (sourceValue != null)
				return true;

			_options.WriteLog("Unable to find a value for {propertyName}", property.Name);

			_options.ErrorPolicy.OnSourceValueNotFound.Handle(new SourceValueNotFoundArgs
			{
				ValueSelectors = _options.Mappers,
				Property = property,
				Converters = converters,
				Sources = _options.ConfigSources
			});

			return false;
		}
	}
}
