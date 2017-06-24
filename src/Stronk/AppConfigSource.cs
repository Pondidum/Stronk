using System;
using System.Collections.Generic;

using System.Configuration;
using System.Linq;

namespace Stronk
{
	public class AppConfigSource : IConfigurationSource
	{
		public IDictionary<string, string> AppSettings => _settings.Value;
		public ConnectionStringSettingsCollection ConnectionStrings => ConfigurationManager.ConnectionStrings;

		private readonly Lazy<IDictionary<string, string>> _settings;

		public AppConfigSource()
		{
			_settings = new Lazy<IDictionary<string, string>>(() => ConfigurationManager
				.AppSettings
				.AllKeys
				.ToDictionary(
					key => key,
					key => ConfigurationManager.AppSettings[key]));
		}
	}
}
