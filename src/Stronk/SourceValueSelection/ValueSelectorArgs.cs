using System;
using System.Collections.Generic;
using System.Linq;
using Stronk.ConfigurationSourcing;
using Stronk.PropertyWriters;

namespace Stronk.SourceValueSelection
{
	public class ValueSelectorArgs
	{
		private readonly IEnumerable<IConfigurationSource> _sources;

		internal ValueSelectorArgs(Action<LogMessage> logger, IEnumerable<IConfigurationSource> sources, PropertyDescriptor property)
		{
			_sources = sources;
			Logger = logger;
			Property = property;
		}
		
		public Action<LogMessage> Logger { get; }
		public PropertyDescriptor Property { get; }

		public string GetValue(string key) => _sources
			.Select(source => source.GetValue(key))
			.FirstOrDefault(value => value != null);
	}
}
