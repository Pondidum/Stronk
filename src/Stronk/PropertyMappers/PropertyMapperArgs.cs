using System;
using System.Collections.Generic;
using System.Linq;
using Stronk.ConfigurationSourcing;
using Stronk.PropertyWriters;

namespace Stronk.PropertyMappers
{
	public class PropertyMapperArgs
	{
		private readonly IEnumerable<IConfigurationSource> _sources;

		internal PropertyMapperArgs(Action<string, object[]> logger, IEnumerable<IConfigurationSource> sources, PropertyDescriptor property)
		{
			_sources = sources;
			Logger = logger;
			Property = property;
		}
		
		public Action<string, object[]> Logger { get; }
		public PropertyDescriptor Property { get; }

		public string GetValue(string key) => _sources
			.Select(source => source.GetValue(key))
			.FirstOrDefault(value => value != null);
	}
}
