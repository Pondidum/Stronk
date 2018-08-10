using System;
using System.Collections.Generic;
using System.Linq;
using Stronk.ConfigurationSources;
using Stronk.PropertyWriters;

namespace Stronk.PropertyMappers
{
	public class PropertyMapperArgs
	{
		private readonly IEnumerable<IConfigurationSource> _sources;

		public PropertyMapperArgs(Action<string, object[]> logger, IEnumerable<IConfigurationSource> sources, PropertyDescriptor property)
		{
			_sources = sources;
			Logger = logger;
			Property = property;
		}
		
		public Action<string, object[]> Logger { get; }
		public PropertyDescriptor Property { get; }

		public string GetValue(string key)
		{
			if (key == null)
				return null;

			return _sources
				.Select(source => source.GetValue(key))
				.FirstOrDefault(value => value != null);
		}

		public string GetValue(Func<string, bool> matchKey) => GetValue(FindKey(matchKey));

		private string FindKey(Func<string, bool> matchKey) => _sources
			.SelectMany(source => source.GetAvailableKeys())
			.FirstOrDefault(matchKey);
	}
}
