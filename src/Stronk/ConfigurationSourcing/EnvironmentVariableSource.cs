using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Stronk.ConfigurationSourcing
{
	public class EnvironmentVariableSource : IConfigurationSource
	{
		private readonly Lazy<Dictionary<string, string>> _variables;

		public EnvironmentVariableSource()
		{
			_variables = new Lazy<Dictionary<string, string>>(() => Environment
				.GetEnvironmentVariables()
				.Cast<DictionaryEntry>()
				.ToDictionary(
					e => (string)e.Key,
					e => (string)e.Value,
					StringComparer.OrdinalIgnoreCase));
		}

		public string GetValue(string key) => _variables.Value.TryGetValue(key, out var value) ? value : null;

		public IEnumerable<string> GetAvailableKeys() => _variables.Value.Keys;
	}
}
