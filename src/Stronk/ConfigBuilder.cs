using System;
using System.Linq;
using System.Reflection;
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

			var converter = new ConversionProcess(_options);
			var converterSelector = new ConverterSelector(_options);
			var valueSelector = new ValueSelector(_options);

			foreach (var property in properties)
			{
				var validConverters = converterSelector.Select(property);
				var valueToUse = valueSelector.Select(property);

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
	}
}
