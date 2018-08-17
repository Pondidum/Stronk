using System.Collections.Generic;
using Stronk.ConfigurationSources;
using Stronk.PropertyMappers;
using Stronk.PropertyWriters;
using Stronk.Validation;
using Stronk.ValueConverters;

namespace Stronk
{
	public interface IStronkConfig
	{
		IEnumerable<IValueConverter> ValueConverters { get; }
		IEnumerable<IPropertyWriter> PropertyWriters { get; }
		IEnumerable<IPropertyMapper> Mappers { get; }
		IEnumerable<IConfigurationSource> ConfigSources { get; }
		IEnumerable<IValidator> Validators { get; }

		void WriteLog(string template, params object[] args);
	}
}
