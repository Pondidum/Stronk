using System;
using System.Collections.Generic;
using System.Linq;
using Stronk.ConfigurationSourcing;
using Stronk.Dsl;
using Stronk.Policies;
using Stronk.PropertyWriters;
using Stronk.SourceValueSelection;
using Stronk.ValueConversion;

namespace Stronk
{
	public class StronkConfig : IErrorPolicyExpression, IStronkConfig
	{
		private ErrorPolicy _errorPolicy;

		public StronkConfig()
		{
			From = new SourceExpression(this);
			Write = new WriterExpression(this);
			Map = new MapExpression(this);
			Convert = new ConversionExpression(this);
			Log = new LogExpression(this);
		}

		public SourceExpression From { get; }
		public WriterExpression Write { get; }
		public MapExpression Map { get; }
		public ConversionExpression Convert { get; }

		public LogExpression Log { get; }
		public IErrorPolicyExpression HandleErrors => this;

		public T Build<T>() where T : new()
		{
			var target = new T();
			var builder = new ConfigBuilder(this);

			builder.Populate(target);
			return target;
		}

		StronkConfig IErrorPolicyExpression.Using(ErrorPolicy errorPolicy)
		{
			_errorPolicy = errorPolicy;
			return this;
		}

		IEnumerable<IValueConverter> IStronkConfig.ValueConverters => Convert.Converters;
		IEnumerable<IPropertyWriter> IStronkConfig.PropertyWriters => Write.Writers;
		IEnumerable<ISourceValueSelector> IStronkConfig.ValueSelectors => Map.Selectors;
		IEnumerable<IConfigurationSource> IStronkConfig.ConfigSources => From.Sources;

		ErrorPolicy IStronkConfig.ErrorPolicy => _errorPolicy ?? Default.ErrorPolicy;
		void IStronkConfig.WriteLog(string template, params object[] args) => Log.Write(template, args);
	}
}
