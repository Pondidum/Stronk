using System.Collections.Specialized;
using System.Configuration;

namespace Stronk
{
	public interface IConfigurationProvider
	{
		NameValueCollection AppSettings { get; }
		ConnectionStringSettingsCollection ConnectionStrings { get; }
	}
}
