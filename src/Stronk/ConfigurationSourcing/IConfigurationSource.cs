using System.Collections.Generic;
using System.Configuration;

namespace Stronk.ConfigurationSourcing
{
	public interface IConfigurationSource
	{
		IDictionary<string, string> AppSettings { get; }
		IDictionary<string, ConnectionStringSettings> ConnectionStrings { get; }
	}
}
