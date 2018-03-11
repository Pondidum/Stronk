using System;
using Stronk.ConfigurationSourcing;
using Stronk.Policies;
using Stronk.PropertyWriters;
using Stronk.SourceValueSelection;
using Stronk.ValueConversion;

namespace Stronk
{
	public interface IStronkConfig
	{
		IValueConverter[] ValueConverters { get; }
		IPropertyWriter[] PropertyWriters { get; }
		ISourceValueSelector[] ValueSelectors { get; }
		IConfigurationSource[] ConfigSources { get; }

		ErrorPolicy ErrorPolicy { get; }
		Action<LogMessage> Logger { get; }

		void WriteLog(string template, params object[] args);
	}

	public class StronkOptions : IStronkConfig
	{
		public IValueConverter[] ValueConverters { get; set; }
		public IPropertyWriter[] PropertyWriters { get; set; }
		public ISourceValueSelector[] ValueSelectors { get; set; }
		public IConfigurationSource[] ConfigSources { get; set; }

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
