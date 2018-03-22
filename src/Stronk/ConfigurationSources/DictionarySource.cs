using System.Collections.Generic;

namespace Stronk.ConfigurationSources
{
	public class DictionarySource : IConfigurationSource
	{
		private readonly IDictionary<string, string> _settings;

		public DictionarySource(IDictionary<string, string> settings)
		{
			_settings = settings;
		}

		public string GetValue(string key) => _settings.TryGetValue(key, out var value) ? value : null;
		public IEnumerable<string> GetAvailableKeys() => _settings.Keys;
	}
}
