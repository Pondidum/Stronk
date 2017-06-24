using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Stronk
{
	public class AppConfigSource : IConfigurationSource
	{
		public IDictionary<string, string> AppSettings => _settings.Value;
		public IDictionary<string, ConnectionStringSettings> ConnectionStrings => _connections.Value;

		private readonly Lazy<IDictionary<string, string>> _settings;
		private readonly Lazy<IDictionary<string, ConnectionStringSettings>> _connections;

		public AppConfigSource()
		{
			_settings = new Lazy<IDictionary<string, string>>(() => ConfigurationManager
				.AppSettings
				.AllKeys
				.ToDictionary(
					key => key,
					key => ConfigurationManager.AppSettings[key]));

			_connections = new Lazy<IDictionary<string, ConnectionStringSettings>>(() => ConfigurationManager
				.ConnectionStrings
				.Cast<ConnectionStringSettings>()
				.ToDictionary(
					c => c.Name,
					c => c));
		}
	}
}
