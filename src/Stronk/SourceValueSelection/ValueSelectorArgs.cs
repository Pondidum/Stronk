using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using Stronk.PropertySelection;

namespace Stronk.SourceValueSelection
{
	public class ValueSelectorArgs
	{
		public Action<LogMessage> Logger { get; }
		public IDictionary<string, string> AppSettings { get; }
		public IDictionary<string, ConnectionStringSettings> ConnectionStrings { get; }
		public PropertyDescriptor Property { get; }

		internal ValueSelectorArgs(Action<LogMessage> logger, IConfigurationSource source, PropertyDescriptor property)
		{
			Logger = logger;
			AppSettings = source.AppSettings;
			ConnectionStrings = source.ConnectionStrings;
			Property = property;
		}
	}
}
