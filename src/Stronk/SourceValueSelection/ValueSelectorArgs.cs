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
		public ConnectionStringSettingsCollection ConnectionStrings { get; }
		public PropertyDescriptor Property { get; private set; }

		internal ValueSelectorArgs(Action<LogMessage> logger, IConfigurationSource source)
		{
			Logger = logger;
			AppSettings = source.AppSettings;
			ConnectionStrings = source.ConnectionStrings;
		}

		internal ValueSelectorArgs With(PropertyDescriptor property)
		{
			Property = property;
			return this;
		}
	}
}
