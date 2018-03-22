using System.Collections.Generic;

namespace Stronk.ConfigurationSources
{
	public interface IConfigurationSource
	{
		string GetValue(string key);
		IEnumerable<string> GetAvailableKeys();
	}
}
