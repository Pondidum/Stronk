using System.Collections.Generic;

namespace Stronk.ConfigurationSourcing
{
	public interface IConfigurationSource
	{
		string GetValue(string key);
		IEnumerable<string> GetAvailableKeys();
	}
}
