using System.Collections.Specialized;
using System.Configuration;

namespace Stronk
{
	public class AppConfigSource : IConfigurationSource
	{
		public NameValueCollection AppSettings => ConfigurationManager.AppSettings;
		public ConnectionStringSettingsCollection ConnectionStrings => ConfigurationManager.ConnectionStrings;
	}
}
