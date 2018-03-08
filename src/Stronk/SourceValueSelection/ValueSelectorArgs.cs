using System;
using Stronk.ConfigurationSourcing;
using Stronk.PropertySelection;

namespace Stronk.SourceValueSelection
{
	public class ValueSelectorArgs
	{
		private readonly IConfigurationSource _source;

		internal ValueSelectorArgs(Action<LogMessage> logger, IConfigurationSource source, PropertyDescriptor property)
		{
			_source = source;
			Logger = logger;
			Property = property;
		}
		
		public Action<LogMessage> Logger { get; }
		public PropertyDescriptor Property { get; }

		public string GetValue(string key) => _source.GetValue(key);
	}
}
