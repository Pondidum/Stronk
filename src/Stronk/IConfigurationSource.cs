using System.Collections.Generic;
using System.Configuration;

namespace Stronk
{
	public interface IConfigurationSource
	{
		IDictionary<string, string> AppSettings { get; }
		ConnectionStringSettingsCollection ConnectionStrings { get; }
	}
}
