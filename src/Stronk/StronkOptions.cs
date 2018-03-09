using System;
using Stronk.ConfigurationSourcing;
using Stronk.Policies;
using Stronk.PropertySelection;
using Stronk.SourceValueSelection;
using Stronk.ValueConversion;

namespace Stronk
{
	public class StronkOptions
	{
		public IValueConverter[] ValueConverters { get; set; }
		public IPropertySelector[] PropertySelectors { get; set; }
		public ISourceValueSelector[] ValueSelectors { get; set; }
		public IConfigurationSource[] ConfigSources { get; set; }

		public ErrorPolicy ErrorPolicy { get; set; }
		public Action<LogMessage> Logger { get; set; }


		public StronkOptions()
		{
			ValueConverters = Default.ValueConverters;
			PropertySelectors = Default.PropertySelectors;
			ValueSelectors = Default.SourceValueSelectors;
			ConfigSources = Default.ConfigurationSources;

			ErrorPolicy = new ErrorPolicy();
			Logger = message => { };
		}

		public void WriteLog(string template, params object[] args) => Logger(new LogMessage(template, args));
	}
}
