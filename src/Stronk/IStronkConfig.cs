using System;
using System.Collections.Generic;
using Stronk.ConfigurationSourcing;
using Stronk.Policies;
using Stronk.PropertyWriters;
using Stronk.SourceValueSelection;
using Stronk.ValueConversion;

namespace Stronk
{
	public interface IStronkConfig
	{
		IEnumerable<IValueConverter> ValueConverters { get; }
		IEnumerable<IPropertyWriter> PropertyWriters { get; }
		IEnumerable<ISourceValueSelector> ValueSelectors { get; }
		IEnumerable<IConfigurationSource> ConfigSources { get; }

		ErrorPolicy ErrorPolicy { get; }
		IEnumerable<Action<LogMessage>> Loggers { get; }

		void WriteLog(string template, params object[] args);
	}
}
