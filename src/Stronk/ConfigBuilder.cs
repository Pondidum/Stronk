using System.Linq;
using System.Reflection;
using Stronk.PropertyWriters;

namespace Stronk
{
	public class ConfigBuilder
	{
		private readonly IStronkConfig _options;
		private readonly ConverterSelector _converterSelector;
		private readonly ValueSelector _valueSelector;
		private readonly ConversionProcess _conversionProcess;

		public ConfigBuilder(IStronkConfig options)
		{
			_options = options;
			_converterSelector = new ConverterSelector(_options);
			_valueSelector = new ValueSelector(_options);
			_conversionProcess = new ConversionProcess(_options);
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

			foreach (var property in properties)
			{
				var value = _conversionProcess.Convert(
					property,
					_converterSelector.Select(property),
					_valueSelector.Select(property));

				try
				{
					property.Assign(target, value);
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
