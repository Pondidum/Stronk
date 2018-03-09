using System;
using System.Linq;
using Stronk.ConfigurationSourcing;
using Stronk.PropertySelection;

namespace Stronk.SourceValueSelection
{
	public class ValueSelectorArgs
	{
		private readonly IConfigurationSource[] _sources;

		internal ValueSelectorArgs(Action<LogMessage> logger, IConfigurationSource[] sources, PropertyDescriptor property)
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
