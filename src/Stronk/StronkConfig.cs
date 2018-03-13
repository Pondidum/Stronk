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
	public class StronkConfig : ISourceExpression, IMapExpression, IWriterExpression, ILogExpression, IConversionExpression, IErrorPolicyExpression, IStronkConfig
	{
		private readonly List<IConfigurationSource> _sources;
		private readonly List<ISourceValueSelector> _selectors;
		private readonly List<IPropertyWriter> _writers;
		private readonly List<Action<LogMessage>> _loggers;
		private readonly List<IValueConverter> _converters;
		private bool _onlySpecifiedConverters;
		private ErrorPolicy _errorPolicy;

		public StronkConfig()
		{
			_writers = new List<IPropertyWriter>();
			_sources = new List<IConfigurationSource>();
			_selectors = new List<ISourceValueSelector>();
			_loggers = new List<Action<LogMessage>>();
			_converters = new List<IValueConverter>();
			_onlySpecifiedConverters = false;
		}

		public ISourceExpression From => this;
		public IMapExpression Map => this;
		public IWriterExpression Write => this;
		public ILogExpression Log => this;
		public IConversionExpression Convert => this;
		public IErrorPolicyExpression HandleErrors => this;

		public T Build<T>() where T : new()
		{
			var target = new T();
			var builder = new ConfigBuilder(this);

			builder.Populate(target);
			return target;
		}

		StronkConfig ISourceExpression.Source(IConfigurationSource source)
		{
			_sources.Add(source);
			return this;
		}

		StronkConfig IMapExpression.With(ISourceValueSelector selector)
		{
			_selectors.Add(selector);
			return this;
		}

		StronkConfig IWriterExpression.To(IPropertyWriter writer)
		{
			_writers.Add(writer);
			return this;
		}

		StronkConfig ILogExpression.Using(Action<LogMessage> logger)
		{
			_loggers.Add(logger);
			return this;
		}

		StronkConfig IConversionExpression.Using(params IValueConverter[] converters)
		{
			_converters.AddRange(converters);
			return this;
		}

		StronkConfig IConversionExpression.UsingOnly(params IValueConverter[] converters)
		{
			_converters.AddRange(converters);
			_onlySpecifiedConverters = true;
			return this;
		}

		StronkConfig IErrorPolicyExpression.Using(ErrorPolicy errorPolicy)
		{
			_errorPolicy = errorPolicy;
			return this;
		}

		IEnumerable<IValueConverter> IStronkConfig.ValueConverters => _onlySpecifiedConverters ? _converters : _converters.Concat(Default.ValueConverters);
		IEnumerable<IPropertyWriter> IStronkConfig.PropertyWriters => _writers.Any() ? _writers : Default.PropertyWriters;
		IEnumerable<ISourceValueSelector> IStronkConfig.ValueSelectors => _selectors.Any() ? _selectors : Default.SourceValueSelectors;
		IEnumerable<IConfigurationSource> IStronkConfig.ConfigSources => _sources.Any() ? _sources : Default.ConfigurationSources;

		ErrorPolicy IStronkConfig.ErrorPolicy => _errorPolicy ?? Default.ErrorPolicy;

		void IStronkConfig.WriteLog(string template, params object[] args)
		{
			_loggers.ForEach(logger => logger(new LogMessage(template, args)));
		}
	}
}
