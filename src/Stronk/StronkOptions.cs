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
		Action<LogMessage> Logger { get; }

		void WriteLog(string template, params object[] args);
	}

	public class StronkOptions : IStronkConfig
	{
		public IEnumerable<IValueConverter> ValueConverters { get; set; }
		public IEnumerable<IPropertyWriter> PropertyWriters { get; set; }
		public IEnumerable<ISourceValueSelector> ValueSelectors { get; set; }
		public IEnumerable<IConfigurationSource> ConfigSources { get; set; }

		public ErrorPolicy ErrorPolicy { get; set; }
		public Action<LogMessage> Logger { get; set; }


		public StronkOptions()
		{
			ValueConverters = Default.ValueConverters;
			PropertyWriters = Default.PropertyWriters;
			ValueSelectors = Default.SourceValueSelectors;
			ConfigSources = Default.ConfigurationSources;

			ErrorPolicy = new ErrorPolicy();
			Logger = message => { };
		}

		public void WriteLog(string template, params object[] args) => Logger(new LogMessage(template, args));
	}
}
