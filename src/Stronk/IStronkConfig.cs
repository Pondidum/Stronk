using System.Collections.Generic;
using Stronk.ConfigurationSourcing;
using Stronk.Policies;
using Stronk.PropertyMappers;
using Stronk.PropertyWriters;
using Stronk.ValueConversion;

namespace Stronk
{
	public interface IStronkConfig
	{
		IEnumerable<IValueConverter> ValueConverters { get; }
		IEnumerable<IPropertyWriter> PropertyWriters { get; }
		IEnumerable<IPropertyMapper> Mappers { get; }
		IEnumerable<IConfigurationSource> ConfigSources { get; }

		ErrorPolicy ErrorPolicy { get; }

		void WriteLog(string template, params object[] args);
	}
}
