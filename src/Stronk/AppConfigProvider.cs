using System.Collections.Specialized;
using System.Configuration;

namespace Stronk
{
	public class AppConfigProvider : IConfigurationProvider
	{
		public NameValueCollection AppSettings => ConfigurationManager.AppSettings;
		public ConnectionStringSettingsCollection ConnectionStrings => ConfigurationManager.ConnectionStrings;
	}
}
