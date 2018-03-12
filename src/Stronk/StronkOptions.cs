using System;
using System.Collections.Generic;
using System.Linq;
using Stronk.ConfigurationSourcing;
using Stronk.Policies;
using Stronk.PropertyWriters;
using Stronk.SourceValueSelection;
using Stronk.ValueConversion;

namespace Stronk
{
	public class StronkOptions : IStronkConfig
	{
		public IEnumerable<IValueConverter> ValueConverters { get; set; }
		public IEnumerable<IPropertyWriter> PropertyWriters { get; set; }
		public IEnumerable<ISourceValueSelector> ValueSelectors { get; set; }
		public IEnumerable<IConfigurationSource> ConfigSources { get; set; }

		public ErrorPolicy ErrorPolicy { get; set; }
		public IEnumerable<Action<LogMessage>> Loggers { get; set; }

		public StronkOptions()
		{
			ValueConverters = Default.ValueConverters;
			PropertyWriters = Default.PropertyWriters;
			ValueSelectors = Default.SourceValueSelectors;
			ConfigSources = Default.ConfigurationSources;
			Loggers = Enumerable.Empty<Action<LogMessage>>();

			ErrorPolicy = new ErrorPolicy();
		}

		public void WriteLog(string template, params object[] args) => Loggers.ForEach(
			logger => logger(new LogMessage(template, args)));
	}
}
