using System.Collections.Generic;
using Stronk.ConfigurationSources;
using Stronk.Dsl;
using Stronk.Policies;
using Stronk.PropertyMappers;
using Stronk.PropertyWriters;
using Stronk.Validation;
using Stronk.ValueConverters;

namespace Stronk
{
	public class StronkConfig : IStronkConfig
	{
		public StronkConfig()
		{
			From = new SourceExpression(this);
			Write = new WriterExpression(this);
			Map = new MapExpression(this);
			Convert = new ConversionExpression(this);
			Validate = new ValidationExpression(this);
			Log = new LogExpression(this);
		}

		public SourceExpression From { get; }
		public WriterExpression Write { get; }
		public MapExpression Map { get; }
		public ConversionExpression Convert { get; }
		public ValidationExpression Validate { get; }
		public LogExpression Log { get; }

		IEnumerable<IValueConverter> IStronkConfig.ValueConverters => Convert.Converters;
		IEnumerable<IPropertyWriter> IStronkConfig.PropertyWriters => Write.Writers;
		IEnumerable<IPropertyMapper> IStronkConfig.Mappers => Map.Mappers;
		IEnumerable<IConfigurationSource> IStronkConfig.ConfigSources => From.Sources;
		IEnumerable<IValidator> IStronkConfig.Validators => Validate.Validators;

		void IStronkConfig.WriteLog(string template, params object[] args) => Log.Write(template, args);

		public T Build<T>() where T : new()
		{
			var target = new T();
			ApplyTo<T>(target);
			return target;
		}

		public void ApplyTo<T>(T target)
		{
			var builder = new ConfigBuilder(this);
			builder.Populate(target);
		}
	}
}
