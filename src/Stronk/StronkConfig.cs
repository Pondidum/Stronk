using System.Collections.Generic;
using Stronk.ConfigurationSourcing;
using Stronk.Dsl;
using Stronk.Policies;
using Stronk.PropertyWriters;
using Stronk.SourceValueSelection;
using Stronk.ValueConversion;

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
			Log = new LogExpression(this);
			HandleErrors = new ErrorPolicyExpression(this);
		}

		public SourceExpression From { get; }
		public WriterExpression Write { get; }
		public MapExpression Map { get; }
		public ConversionExpression Convert { get; }
		public LogExpression Log { get; }
		public ErrorPolicyExpression HandleErrors { get; }

		IEnumerable<IValueConverter> IStronkConfig.ValueConverters => Convert.Converters;
		IEnumerable<IPropertyWriter> IStronkConfig.PropertyWriters => Write.Writers;
		IEnumerable<ISourceValueSelector> IStronkConfig.ValueSelectors => Map.Selectors;
		IEnumerable<IConfigurationSource> IStronkConfig.ConfigSources => From.Sources;
		ErrorPolicy IStronkConfig.ErrorPolicy => HandleErrors.Policy;
		void IStronkConfig.WriteLog(string template, params object[] args) => Log.Write(template, args);

		public T Build<T>() where T : new()
		{
			var target = new T();
			var builder = new ConfigBuilder(this);

			builder.Populate(target);
			return target;
		}
	}
}
