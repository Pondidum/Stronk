using System.Collections.Generic;

namespace Stronk.ConfigurationSourcing
{
	public class DictionaryConfigurationSource : IConfigurationSource
	{
		private readonly IDictionary<string, string> _settings;

		public DictionaryConfigurationSource(IDictionary<string, string> settings)
		{
			_settings = settings;
		}

		public string GetValue(string key) => _settings.TryGetValue(key, out var value) ? value : null;
		public IEnumerable<string> GetAvailableKeys() => _settings.Keys;
	}
}
