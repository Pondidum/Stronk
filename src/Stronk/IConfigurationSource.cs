using System.Collections.Specialized;
using System.Configuration;

namespace Stronk
{
	public interface IConfigurationSource
	{
		NameValueCollection AppSettings { get; }
		ConnectionStringSettingsCollection ConnectionStrings { get; }
	}
}
