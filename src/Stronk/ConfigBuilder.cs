using System.Linq;
using System.Reflection;
using Stronk.PropertyWriters;
using Stronk.Validation;

namespace Stronk
{
	public class ConfigBuilder
	{
		private readonly IStronkConfig _options;
		private readonly ConverterSelector _converterSelector;
		private readonly ValueSelector _valueSelector;
		private readonly ConversionProcess _conversionProcess;
		private readonly Validator _validator;

		public ConfigBuilder(IStronkConfig options)
		{
			_options = options;
			_converterSelector = new ConverterSelector(_options);
			_valueSelector = new ValueSelector(_options);
			_conversionProcess = new ConversionProcess(_options);
			_validator = new Validator(_options);
		}

		public void Populate<T>(T target)
		{
			LogPopulationStart(target);

			var writerArgs = new PropertyWriterArgs(_options.WriteLog, target.GetType());

			var properties = new FallbackPropertyWriter(_options.PropertyWriters)
				.Select(writerArgs)
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

			_validator.Validate(target);
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
