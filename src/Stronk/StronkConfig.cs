using System;
using System.Collections.Generic;
using Stronk.ConfigurationSourcing;
using Stronk.Dsl;
using Stronk.PropertyWriters;
using Stronk.SourceValueSelection;

namespace Stronk
{
	public class StronkConfig : ISourceExpression, IMapExpression, IWriterExpression, ILogExpression
	{
		private readonly List<IConfigurationSource> _sources;
		private readonly List<ISourceValueSelector> _selectors;
		private readonly List<IPropertyWriter> _writers;
		private readonly List<Action<LogMessage>> _loggers;

		public StronkConfig()
		{
			_writers = new List<IPropertyWriter>();
			_sources = new List<IConfigurationSource>();
			_selectors = new List<ISourceValueSelector>();
			_loggers = new List<Action<LogMessage>>();
		}

		public ISourceExpression From => this;
		public IMapExpression Map => this;
		public IWriterExpression Write => this;
		public ILogExpression Log => this;

		public T Build<T>() where T : new()
		{
			var options = new StronkOptions
			{
				ConfigSources = _sources,
				ValueSelectors = _selectors,
				PropertyWriters = _writers,
				Loggers = _loggers,
			};

			var config = new T();
			new ConfigBuilder(options).Populate(config);

			return config;
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
	}
}
