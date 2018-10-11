using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Stronk.ConfigurationSources
{
	public class ConfigurationSourcesMonitor : IEnumerable<IConfigurationSource>
	{
		private readonly HashSet<string> _usedKeys;
		private readonly SourceDecorator[] _sources;

		public ConfigurationSourcesMonitor(IEnumerable<IConfigurationSource> sources)
		{
			_usedKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			_sources = sources.Select(source => new SourceDecorator(source, _usedKeys)).ToArray();
		}

		public string[] GetUnusedKeys()
		{
			var allKeys = new HashSet<string>(
				_sources.SelectMany(source => source.GetAvailableKeys()),
				StringComparer.OrdinalIgnoreCase
			);

			allKeys.ExceptWith(_usedKeys);

			return allKeys.ToArray();
		}

		public IEnumerator<IConfigurationSource> GetEnumerator()
		{
			return ((IEnumerable<IConfigurationSource>)_sources).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		private class SourceDecorator : IConfigurationSource
		{
			private readonly IConfigurationSource _inner;
			private readonly HashSet<string> _usedKeys;

			public SourceDecorator(IConfigurationSource inner, HashSet<string> usedKeys)
			{
				_inner = inner;
				_usedKeys = usedKeys;
			}

			public string GetValue(string key)
			{
				_usedKeys.Add(key);
				return _inner.GetValue(key);
			}

			public IEnumerable<string> GetAvailableKeys()
			{
				return _inner.GetAvailableKeys();
			}
		}
	}
}
