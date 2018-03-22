using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Stronk.ConfigurationSources
{
	public class AppConfigSource : IConfigurationSource
	{
		private readonly Lazy<IDictionary<string, string>> _settings;
		private readonly Lazy<IDictionary<string, string>> _connections;

		public AppConfigSource()
		{
			_settings = new Lazy<IDictionary<string, string>>(() => ConfigurationManager
				.AppSettings
				.AllKeys
				.ToDictionary(
					key => key,
					key => ConfigurationManager.AppSettings[key],
					StringComparer.OrdinalIgnoreCase));

			_connections = new Lazy<IDictionary<string, string>>(() => ConfigurationManager
				.ConnectionStrings
				.Cast<ConnectionStringSettings>()
				.ToDictionary(
					c => c.Name,
					c => c.ConnectionString,
					StringComparer.OrdinalIgnoreCase));
		}

		public string GetValue(string key)
		{
			string value;

			if (_settings.Value.TryGetValue(key, out value))
				return value;

			if (_connections.Value.TryGetValue(key, out value))
				return value;

			return null;
		}

		public IEnumerable<string> GetAvailableKeys() => _settings.Value.Keys.Concat(_connections.Value.Keys);
	}
}
