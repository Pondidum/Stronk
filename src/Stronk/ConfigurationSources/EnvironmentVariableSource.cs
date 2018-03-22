using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Stronk.ConfigurationSources
{
	public class EnvironmentVariableSource : IConfigurationSource
	{
		private readonly Lazy<Dictionary<string, string>> _variables;

		public EnvironmentVariableSource(string prefix = "", IDictionary source = null)
		{
			prefix = prefix ?? string.Empty;

			_variables = new Lazy<Dictionary<string, string>>(() =>
				(source ?? Environment.GetEnvironmentVariables())
				.Cast<DictionaryEntry>()
				.Where(e => ((string)e.Key).StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
				.ToDictionary(
					e => ((string)e.Key).Substring(prefix.Length),
					e => (string)e.Value,
					StringComparer.OrdinalIgnoreCase));
		}

		public string GetValue(string key) => _variables.Value.TryGetValue(key, out var value) ? value : null;

		public IEnumerable<string> GetAvailableKeys() => _variables.Value.Keys;
	}
}
