﻿using System.Collections.Generic;
using Stronk.ConfigurationSources;
using Stronk.Policies;
using Stronk.PropertyMappers;
using Stronk.PropertyWriters;
using Stronk.ValueConverters;

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
